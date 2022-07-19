using System.Text.Json.Serialization;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// 機械学習用設定
/// </summary>
public class ConfigMachine
{
    #region 機械学習

    /// <summary>
    /// トレーニングモデル
    /// </summary>
    public enum TrainingModel : int
    {
        /// <summary>
        /// ライト Gbm を使用したブーストデシジョンツリー多重クラス分類モデルのトレーニング用
        /// </summary>
        //LightGbmMulticlassTrainer = 0,
        /// <summary>
        /// 最大 エントロピ多クラス分類子を使用してターゲットを予測する。
        /// トレーニング済みのモデル MaximumEntropyModelParameters では、クラスの確率が生成されます。
        /// </summary>
        SdcaMaximumEntropyMulticlassTrainer,
        /// <summary>
        /// 線形 多クラス分類子を使用してターゲットを予測する。
        /// トレーニング済みのモデル LinearMulticlassModelParameters では、クラスの確率が生成されます。
        /// </summary>
        SdcaNonCalibratedMulticlassTrainer,
        /// <summary>
        /// L-BFGS メソッドでトレーニングされた最大エントロピ多クラス分類子を使用して
        /// ターゲットを予測する 。
        /// </summary>
        LbfgsMaximumEntropyMulticlassTrainer,
        /// <summary>
        /// バイナリ機能値をサポートする多クラス Naive Bayes モデルをトレーニングするための。
        /// </summary>
        NaiveBayesMulticlassTrainer,
        /// <summary>
        /// 指定されたバイナリ分類子を使用する、1対すべてのマルチクラス分類子をトレーニングするための。
        /// </summary>
        //OneVersusAllTrainer,
        /// <summary>
        /// 指定されたバイナリ分類子を使用する、ペアワイズ結合マルチクラス分類子をトレーニングするための。
        /// </summary>
        //PairwiseCouplingTrainer,
        /// <summary>
        /// クラスタリング
        /// </summary>
        //KMeansTrainer,
    }

    /// <summary>
    /// トレーニングモデル選択
    /// </summary>
    [JsonIgnore]
    public TrainingModel TrainingModeTypeSelect { get; set; }
        = TrainingModel.SdcaMaximumEntropyMulticlassTrainer;

    #endregion
}
