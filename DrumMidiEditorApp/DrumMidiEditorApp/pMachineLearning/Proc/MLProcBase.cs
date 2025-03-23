using System;
using DrumMidiLibrary.pLog;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace DrumMidiEditorApp.pMachineLearning;

/// <summary>
/// ML.NET 機械学習
/// </summary>
internal abstract class MLProcBase
{
    /// <summary>
    /// 機械学習＆予測処理（試作）
    /// TODO: お試しで作成したもの、今のところ実装予定なし
    /// </summary>
    public void Start()
    {
        try
        {
            var mlContext = new MLContext( 0 );

            #region 学習データ作成

            Log.Info( $"Createing learning input data." );

            var trainInputdataSet = CreateTrainingDataSet( mlContext );

            if ( trainInputdataSet == null )
            {
                return;
            }

            #endregion

            #region 予測対象データ作成

            Log.Info( $"Createing predictive input data." );

            var predictInputDataSet = CreatePredictionDataSet( mlContext );

            if ( predictInputDataSet == null )
            {
                Log.Warning( $"Unable to create predictive input data." );
                return;
            }

            #endregion

            #region 学習＆評価

            Log.Info( $"Learning & evaluation.", true );

            BuildAndTrainAndEvaluate( mlContext, trainInputdataSet );

            #endregion

            #region 予測

            Log.Info( $"Forecast start", true );

            Predict( mlContext, predictInputDataSet );

            Log.Info( $"Forecast end.", true );

            #endregion
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// トレーニングデータセット作成
    /// </summary>
    /// <param name="aMLContext">MLコンテキスト</param>
    /// <returns>トレーニングデータセット</returns>
    protected virtual IDataView? CreateTrainingDataSet( MLContext aMLContext )
        => null;

    /// <summary>
    /// 予測対象データセット作成
    /// </summary>
    /// <param name="aMLContext">MLコンテキスト</param>
    /// <returns>予測対象データセット</returns>
    protected virtual IDataView? CreatePredictionDataSet( MLContext aMLContext )
        => null;

    /// <summary>
    /// ビルド&トレーニング&評価
    /// </summary>
    /// <param name="aMLContext">MLコンテキスト</param>
    /// <param name="aTrainingData">トレーニングデータセット</param>
    protected virtual void BuildAndTrainAndEvaluate( MLContext aMLContext, IDataView aTrainingData )
    {
    }

    /// <summary>
    /// 予測
    /// </summary>
    /// <param name="aMLContext">MLコンテキスト</param>
    /// <param name="aPredictInputData">予測対象データセット</param>
    protected virtual void Predict( MLContext aMLContext, IDataView aPredictInputData )
    {
    }

    /// <summary>
    /// 評価結果（多クラス分類）
    /// </summary>
    /// <param name="aMetrics">評価結果</param>
    protected void PrintMetrics( MulticlassClassificationMetrics aMetrics )
    {
        Log.Info( $"***************************************************************" );
        Log.Info( $"* Metrics for Multi-class Classification model - Test Data     " );
        Log.Info( $"*--------------------------------------------------------------" );
        Log.Info( $"* MicroAccuracy:    {aMetrics.MicroAccuracy:0.###}" );
        Log.Info( $"* MacroAccuracy:    {aMetrics.MacroAccuracy:0.###}" );
        Log.Info( $"* LogLoss:          {aMetrics.LogLoss:#.###}" );
        Log.Info( $"* LogLossReduction: {aMetrics.LogLossReduction:#.###}" );
        Log.Info( $"***************************************************************" );

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

	/// <summary>
	/// 評価結果（クラスタリング）
	/// </summary>
	/// <param name="aMetrics">評価結果</param>
	protected static void PrintMetrics( ClusteringMetrics aMetrics )
    {
		Log.Info( $"***************************************************************");
		Log.Info( $"* Metrics for Multi-class Classification model - Test Data     ");
		Log.Info( $"*--------------------------------------------------------------");
		Log.Info( $"* AverageDistance            : {aMetrics.AverageDistance:0.###}"			);
		Log.Info( $"* DaviesBouldinIndex         : {aMetrics.DaviesBouldinIndex:0.###}"			);
		Log.Info( $"* NormalizedMutualInformation: {aMetrics.NormalizedMutualInformation:#.###}");
		Log.Info( $"***************************************************************");
	}
}
