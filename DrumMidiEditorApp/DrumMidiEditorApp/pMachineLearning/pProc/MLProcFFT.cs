using System.Collections.Generic;
using System.IO;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pMachineLearning.pModel;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pLog;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Windows.Foundation;

namespace DrumMidiEditorApp.pMachineLearning.pProc;

/// <summary>
/// ML.NET 機械学習
/// </summary>
internal class MLProcFFT : MLProcBase
{
    protected override IDataView? CreateTrainingDataSet( MLContext aMLContext )
        => CreateDataSet( aMLContext, false, 0, DMS.SCORE.EditChannel.MaxMeasureNo );

    protected override IDataView? CreatePredictionDataSet( MLContext aMLContext )
        => CreateDataSet( aMLContext, true, 0, Config.System.MeasureMaxNumber );

    /// <summary>
    /// トレーニング／予測対象データセット作成
    /// </summary>
    /// <param name="aMLContext">MLコンテキスト</param>
    /// <param name="aPrediction">True:予測、False:トレーニング</param>
    /// <param name="aStartMeasureNo">小節開始番号</param>
    /// <param name="aEndMeasureNo">小節終了番号</param>
    /// <returns>小節開始～終了間のデータセット</returns>
    private IDataView? CreateDataSet( MLContext aMLContext, bool aPrediction, int aStartMeasureNo, int aEndMeasureNo )
    {
        var bgm = new NAudioData( DMS.SCORE.BgmFilePath );

        bgm?.CalcFFT();

        if ( !( bgm?.IsEnableFFT() ?? false ) )
        {
            return null;
        }

        var dataSet = new List<ModelFFTData>();

        var range = new Rect( 0, 0, bgm.Channels, bgm.FFTBufferLength1 );

        for ( var offset_x = 0; offset_x < bgm.FFTBufferLength0 - range.Width; offset_x += (int)range.Width )
        {
            var offset_time = bgm.GetFFTTime( offset_x );

#if DEBUG
			if ( offset_x % 1000 == 0 )
			{ 
				Log.Info( $"{offset_x}-{offset_time}" );
			}
#endif
            var note_pos = DmsControl.SearchPosition(offset_time);

            if ( note_pos < aStartMeasureNo * Config.System.MeasureNoteNumber )
            {
                continue;
            }
            else if ( note_pos >= ( aEndMeasureNo + 1 ) * Config.System.MeasureNoteNumber )
            {
                break;
            }

            range.X = offset_x;

            for ( var offset_y = 0; offset_y < bgm.FFTBufferLength1; offset_y += (int)range.Height )
            {
                range.Y = offset_y;

                dataSet.Add
                    (
                        new()
                        {
                            DrumPattern     = aPrediction ? string.Empty : DMS.SCORE.EditChannel.GetMLData( note_pos ) ?? string.Empty,
                            AbsoultNotePos  = note_pos,
                            HzCenter        = bgm.GetHz( offset_y + ( (int)range.Height / 2 ) ),
                            FFTBuffer       = [ .. bgm.GetFFTBuffer( range, Config.Scale.VolumeLevelLow ) ],
                        }
                    );
            }
        }

        return aMLContext.Data.LoadFromEnumerable<ModelFFTData>( dataSet );
    }

    /// <summary>
    /// ビルド&トレーニング&評価
    /// </summary>
    /// <param name="aMLContext">MLコンテキスト</param>
    /// <param name="aTrainingData">トレーニングデータセット</param>
    protected override void BuildAndTrainAndEvaluate( MLContext aMLContext, IDataView aTrainingData )
    {
        // トレーニングデータとテストデータに分ける
        var dataSplit  = aMLContext.Data.TrainTestSplit( aTrainingData, 0.2d );
        var trainData  = dataSplit.TrainSet;
        var testData   = dataSplit.TestSet;

        #region 書式変換

        // 1.文字列[DrumPattern]を数値キー[Label]に変換
        // 2.[Features] 項目として、項目を定義
        // 3.キャッシュ設定
        var formatPipeline = aMLContext.Transforms.Conversion.MapValueToKey( "Label", "DrumPattern" )
                .Append
                    (
                        aMLContext.Transforms.Concatenate
                        (
                            "Features", 
						//	nameof( DrumModel.HzCenter ), 
							nameof( ModelFFTData.FFTBuffer )
                        )
                    )
                .AppendCacheCheckpoint( aMLContext );

        var formatedModel      = formatPipeline.Fit( trainData );
        var formatedModelData  = formatedModel.Transform( trainData );

        #endregion

        #region トレーニング

        var retraining_filepath = string.Empty;

        IEstimator<ITransformer>? trainingPipeline;
        ITransformer?             trainedModel = null;
        IDataView?                trainedModelData;

        var mode = Config.Machine.TrainingModeTypeSelect;

        switch ( mode )
        {
            //case ConfigScale.TrainingModel.LightGbmMulticlassTrainer:
            //	{ 
            //		trainingPipeline = formatPipeline
            //			.Append( aMLContext.MulticlassClassification.Trainers.SdcaMaximumEntropy() );
            //	}
            //	break;
            case ConfigMachine.TrainingModel.SdcaMaximumEntropyMulticlassTrainer:
                {
                    trainingPipeline = formatPipeline
                        .Append( aMLContext.MulticlassClassification.Trainers.SdcaMaximumEntropy() );
                }
                break;
            case ConfigMachine.TrainingModel.SdcaNonCalibratedMulticlassTrainer:
                {
                    trainingPipeline = formatPipeline
                        .Append( aMLContext.MulticlassClassification.Trainers.SdcaNonCalibrated() );
                }
                break;
            case ConfigMachine.TrainingModel.LbfgsMaximumEntropyMulticlassTrainer:
                {
                    trainingPipeline = formatPipeline
                        .Append( aMLContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy() );

                    // 再トレーニング
                    retraining_filepath = $"{Config.File.FolderModel.RelativeFolderPath}\\model_retrain_LbfgsMaximumEntropyMulticlassTrainer.zip";

                    if ( File.Exists( retraining_filepath ) )
                    {
                        Log.Info( $"ReTraining." );

                        var retrainedModel = aMLContext.Model.Load( retraining_filepath, out _ );

                        // TODO: 2回目の再トレーニングでエラーになる。。
                        var originalModelParameters =
                            ( ( retrainedModel as TransformerChain<ITransformer> )?
                            .LastTransformer as MulticlassPredictionTransformer<MaximumEntropyModelParameters> )?
                            .Model;

                        // 再トレーニング実施
                        trainedModel = aMLContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy()
                            .Fit( formatedModelData, originalModelParameters );
                    }
                }
                break;
            case ConfigMachine.TrainingModel.NaiveBayesMulticlassTrainer:
                {
                    trainingPipeline = formatPipeline
                        .Append( aMLContext.MulticlassClassification.Trainers.NaiveBayes() );
                }
                break;
            //case ConfigScale.TrainingModel.OneVersusAllTrainer:
            //	{ 
            //		trainingPipeline = formatPipeline
            //			.Append( aMLContext.MulticlassClassification.Trainers.OneVersusAll() );
            //	}
            //  break;
            //case ConfigScale.TrainingModel.PairwiseCouplingTrainer:
            //	{ 
            //		trainingPipeline = formatPipeline
            //			.Append( aMLContext.MulticlassClassification.Trainers.PairwiseCoupling( "Label", "Features" ) );
            //	}
            //  break;
            //case ConfigScale.TrainingModel.KMeansTrainer:
            //	{ 
            //		trainingPipeline = formatPipeline
            //			.Append( aMLContext.Clustering.Trainers.KMeans() );
            //	}
            //  break;
            default:
                return;
        }

        trainedModel ??= trainingPipeline.Fit( formatedModelData );

        trainedModelData = trainedModel.Transform( formatedModelData );

        #endregion

        #region 予測用のキー変換処理追加

        var predictPipeline  = trainingPipeline
            .Append( aMLContext.Transforms.Conversion.MapKeyToValue( "PredictedLabel" ) );

        var predictModel     = predictPipeline.Fit( trainedModelData );
        var predictModelData = predictModel.Transform( trainedModelData );

        #endregion

        #region モデルを評価する

        switch ( mode )
        {
            //case ConfigMachine.TrainingModel.LightGbmMulticlassTrainer:
            case ConfigMachine.TrainingModel.SdcaMaximumEntropyMulticlassTrainer:
            case ConfigMachine.TrainingModel.SdcaNonCalibratedMulticlassTrainer:
            case ConfigMachine.TrainingModel.LbfgsMaximumEntropyMulticlassTrainer:
            case ConfigMachine.TrainingModel.NaiveBayesMulticlassTrainer:
                //case ConfigMachine.TrainingModel.OneVersusAllTrainer:
                //case ConfigMachine.TrainingModel.PairwiseCouplingTrainer:
                PrintMetrics( aMLContext.MulticlassClassification.Evaluate( predictModel.Transform( testData ) ) );
                break;
            //case ConfigMachine.TrainingModel.KMeansTrainer:
            //	PrintMetrics( aMLContext.Clustering.Evaluate( trainedModel.Transform( testData ) ) );
            //	break;
            default:
                return;
        }

        #endregion

        if ( retraining_filepath.Length != 0 )
        {
            // 再トレーニング用データ作成
            aMLContext.Model.Save( trainedModel, trainedModelData.Schema, retraining_filepath );
        }

        // 予測用データの保存
        aMLContext.Model.Save( predictModel, predictModelData.Schema, $"{Config.File.FolderModel.RelativeFolderPath}\\model_predict_{DMS.OpenFilePath.FileName}.zip" );
    }

    /// <summary>
    /// 予測
    /// </summary>
    /// <param name="aMLContext">MLコンテキスト</param>
    /// <param name="aPredictInputData">予測対象データセット</param>
    protected override void Predict( MLContext aMLContext, IDataView aPredictInputData )
    {
        // 保存されたモデルを読み込む
        var loadedModel = aMLContext.Model.Load( $"{Config.File.FolderModel.RelativeFolderPath}\\model_predict_{DMS.OpenFilePath.FileName}.zip", out var modelInputSchema );

        DMS.SCORE_PREDICT.EditChannelNo = DMS.SCORE.EditChannelNo;
        DMS.SCORE_PREDICT.EditChannel.ClearAll();

        var engine = aMLContext.Model.CreatePredictionEngine<ModelFFTData, ModelFFTPrediction>( loadedModel );

        var en = aMLContext.Data.CreateEnumerable<ModelFFTData>( aPredictInputData, false ).GetEnumerator();

#if DEBUG
		long cnt	= 0;
		var cnt_max = aPredictInputData.GetRowCount();
#endif
        while ( en.MoveNext() )
        {
#if DEBUG
			if ( cnt % 10000 == 0 )
            {
				Log.Info( $"{cnt}/{cnt_max}({cnt*100/cnt_max}%)" );
            }
			cnt++;
#endif

            var result = engine.Predict( en.Current );

            if ( result.DrumPattern.Equals( "NONE" ) )
            {
                continue;
            }

            foreach ( var drumKey in result.DrumPattern.Split( ":" ) )
            {
                DMS.SCORE_PREDICT.EditChannel.AddNote
                    (
                        int.Parse( drumKey ),
                        en.Current.AbsoultNotePos,
                        MidiNet.MidiMaxVolume,
                        true,
                        false,
                        false
                    );
            }

            Log.Info( $"{en.Current.AbsoultNotePos / Config.System.MeasureNoteNumber}-{en.Current.AbsoultNotePos % Config.System.MeasureNoteNumber}-{result.DrumPattern}" );
        }

        EventManage.Event_Editer_UpdateScorePredict();
    }
}
