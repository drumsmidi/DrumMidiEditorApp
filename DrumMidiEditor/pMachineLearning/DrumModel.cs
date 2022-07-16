using System;
using Microsoft.ML.Data;

namespace DrumMidiEditor.pMachineLearning;

/// <summary>
/// ドラムモデル
/// </summary>
internal class DrumModel
{
    /// <summary>
    /// ドラムパターン
    /// </summary>
    [LoadColumn(0)]
    public string DrumPattern { get; set; } = String.Empty;

    /// <summary>
    /// ノート位置
    /// </summary>
    [LoadColumn(1)]
    public int AbsoultNotePos { get; set; } = 0;

    /// <summary>
    /// Hz中心
    /// </summary>
    [LoadColumn(2)]
    public float HzCenter { get; set; } = 0;

    /// <summary>
    /// 周辺Hz
    /// </summary>
    [LoadColumn(3, 2 + 1024), VectorType(1024)]
    public float[] FFTBuffer { get; set; } = new float[ 1024 ];
}

/// <summary>
/// ドラム予測
/// </summary>
internal class DrumPrediction
{
    /// <summary>
    /// ドラムパターン予測結果
    /// </summary>
    [ColumnName("PredictedLabel")]
    public string DrumPattern { get; set; } = String.Empty;
}
