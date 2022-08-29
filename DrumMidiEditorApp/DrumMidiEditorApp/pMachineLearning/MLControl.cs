using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

using DrumMidiClassLibrary.pAudio;
using DrumMidiClassLibrary.pConfig;
using DrumMidiClassLibrary.pControl;
using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pModel;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;

namespace DrumMidiEditorApp.pMachineLearning;

/// <summary>
/// ML.NET 機械学習
/// </summary>
public static class MLControl
{
	/// <summary>
	/// 処理中フラグ
	/// </summary>
	private static bool _Processing = false;

	/// <summary>
	/// 機械学習＆予測処理（試作）
	/// TODO: お試しで作成したもの、今のところ実装予定なし
	/// </summary>
	public static async void Start()
    {
		if ( _Processing )
        {
			return;
        }

		_Processing = true;

		try
		{
			var mlContext = new MLContext( 0 );

			#region 学習データ作成

			Log.Info( $"Createing learning input data." );

			var trainInputdataSet = CreateDataSet( mlContext, false, 0, DMS.SCORE.EditChannel.MaxMeasureNo );

			if ( trainInputdataSet == null )
			{
				return;
			}

			#endregion

			#region 予測対象データ作成

			Log.Info( $"Createing predictive input data." );

			var predictInputDataSet = CreateDataSet( mlContext, true, 0, Config.System.MeasureMaxNumber );

			if ( predictInputDataSet == null )
			{
				Log.Warning( $"Unable to create predictive input data." );
				return;
			}

			#endregion

			await Task.Run( () =>
				{ 
					#region 学習＆評価

					Log.Info( $"Learning & evaluation.", true );

					BuildAndTrainAndEvaluate( mlContext, trainInputdataSet );

					#endregion

					#region 予測

					Log.Info( $"Forecast start", true );

					Predict( mlContext, predictInputDataSet );

					Log.Info( $"Forecast end.", true );

					#endregion
				} );
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
		{ 
			_Processing = false;
		}
	}

	/// <summary>
	/// トレーニング／予測対象データセット作成
	/// </summary>
	/// <param name="aMLContext">MLコンテキスト</param>
	/// <param name="aPrediction">True:予測、False:トレーニング</param>
	/// <param name="aStartMeasureNo">小節開始番号</param>
	/// <param name="aEndMeasureNo">小節終了番号</param>
	/// <returns>小節開始～終了間のデータセット</returns>
	private static IDataView? CreateDataSet( MLContext aMLContext, bool aPrediction, int aStartMeasureNo, int aEndMeasureNo )
    {
		var bgm = DmsControl.AudioData;

		if ( !( bgm?.IsEnableFFT() ?? false ) )
        {
			return null;
        }

		var dataSet = new List<DrumModel>();

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
            else if ( note_pos >= (aEndMeasureNo + 1) * Config.System.MeasureNoteNumber )
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
                            DrumPattern		= aPrediction ? string.Empty : DMS.SCORE.EditChannel.GetMLData( note_pos ) ?? string.Empty,
                            AbsoultNotePos	= note_pos,
                            HzCenter		= bgm.GetHz( offset_y + (int)range.Height / 2 ),
                            FFTBuffer		= bgm.GetFFTBuffer( range, ConfigLocal.Scale.VolumeLevelLow ).ToArray(),
                        }
                    );
            }
        }

        return aMLContext.Data.LoadFromEnumerable<DrumModel>( dataSet );
	}

	/// <summary>
	/// ビルド&トレーニング&評価
	/// </summary>
	/// <param name="aMLContext">MLコンテキスト</param>
	/// <param name="aTrainingData">トレーニングデータセット</param>
	private static void BuildAndTrainAndEvaluate( MLContext aMLContext, IDataView aTrainingData )
	{
		// トレーニングデータとテストデータに分ける
		var dataSplit	= aMLContext.Data.TrainTestSplit( aTrainingData, 0.2d );
		var trainData	= dataSplit.TrainSet;
		var testData	= dataSplit.TestSet;

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
							nameof( DrumModel.FFTBuffer ) 
						) 
					)
				.AppendCacheCheckpoint( aMLContext );

		var formatedModel		= formatPipeline.Fit( trainData );
		var formatedModelData	= formatedModel.Transform( trainData );

        #endregion

        #region トレーニング

        var retraining_filepath = string.Empty;

		IEstimator<ITransformer>?	trainingPipeline;
        ITransformer?				trainedModel = null;
		IDataView?					trainedModelData;

		var mode = ConfigLocal.Machine.TrainingModeTypeSelect;

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
					retraining_filepath = $"{Config.System.FolderModel.RelativeFolderPath}\\model_retrain_LbfgsMaximumEntropyMulticlassTrainer.zip";

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

		var predictPipeline = trainingPipeline
            .Append( aMLContext.Transforms.Conversion.MapKeyToValue( "PredictedLabel" ) );

		var predictModel	 = predictPipeline.Fit( trainedModelData );
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
		aMLContext.Model.Save( predictModel, predictModelData.Schema, $"{Config.System.FolderModel.RelativeFolderPath}\\model_predict_{DMS.OpenFilePath.FileName}.zip" );
	}

	/// <summary>
	/// 評価結果（多クラス分類）
	/// </summary>
	/// <param name="aMetrics">評価結果</param>
	private static void PrintMetrics( MulticlassClassificationMetrics aMetrics )
    {
		Log.Info( $"***************************************************************");
		Log.Info( $"* Metrics for Multi-class Classification model - Test Data     ");
		Log.Info( $"*--------------------------------------------------------------");
		Log.Info( $"* MicroAccuracy:    {aMetrics.MicroAccuracy:0.###}"				);
		Log.Info( $"* MacroAccuracy:    {aMetrics.MacroAccuracy:0.###}"				);
		Log.Info( $"* LogLoss:          {aMetrics.LogLoss:#.###}"					);
		Log.Info( $"* LogLossReduction: {aMetrics.LogLossReduction:#.###}"			);
		Log.Info( $"***************************************************************");

		// -----------------------------------------------------------------------------------
		// (Reference: https://docs.microsoft.com/ja-jp/dotnet/machine-learning/tutorials/github-issue-classification )
		//
		// 多クラス分類では、次のメトリックが評価されます。
		//
		// マイクロ精度	: すべてのサンプルとクラスのペアが、精度メトリックに均等に作用します。
		//				  マイクロ精度は可能な限り 1 に近づけます。
		// マクロ精度	: すべてのクラスが、精度メトリックに均等に作用します。
		//				  少数派のクラスは、大規模なクラスと同じ重みが与えられています。
		//				  マクロ精度は可能な限り 1 に近づけます。
		// 対数損失		: 対数損失に関するページを参照してください。
		//				  対数損失は可能な限り 1 に近づけます。
		// 対数損失還元	: 範囲は[-inf, 1.00] です。
		//				  ここで、1.00 は完璧な予測で、0 は平均の予測です。
		//				  対数損失還元は可能な限り 1 に近づけます。
		// -----------------------------------------------------------------------------------
	}

#if false
	/// <summary>
	/// 評価結果（クラスタリング）
	/// </summary>
	/// <param name="aMetrics">評価結果</param>
	private static void PrintMetrics( ClusteringMetrics aMetrics )
    {
		Log.Info( $"***************************************************************");
		Log.Info( $"* Metrics for Multi-class Classification model - Test Data     ");
		Log.Info( $"*--------------------------------------------------------------");
		Log.Info( $"* AverageDistance            : {aMetrics.AverageDistance:0.###}"			);
		Log.Info( $"* DaviesBouldinIndex         : {aMetrics.DaviesBouldinIndex:0.###}"			);
		Log.Info( $"* NormalizedMutualInformation: {aMetrics.NormalizedMutualInformation:#.###}");
		Log.Info( $"***************************************************************");
	}
#endif

	/// <summary>
	/// 予測
	/// </summary>
	/// <param name="aMLContext">MLコンテキスト</param>
	/// <param name="aPredictInputData">予測対象データセット</param>
	private static void Predict( MLContext aMLContext, IDataView aPredictInputData )
	{
		// 保存されたモデルを読み込む
		var loadedModel = aMLContext.Model.Load( $"{Config.System.FolderModel.RelativeFolderPath}\\model_predict_{DMS.OpenFilePath.FileName}.zip", out var modelInputSchema );

		DMS.SCORE_PREDICT.EditChannel.ClearAll();

		var engine = aMLContext.Model.CreatePredictionEngine<DrumModel, DrumPrediction>( loadedModel );

		var en = aMLContext.Data.CreateEnumerable<DrumModel>( aPredictInputData, false ).GetEnumerator();

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

		EventManage.EventEditUpdateScorePredict();
	}
}
