using Microsoft.ML.Data;

namespace DrumMidiEditorApp.pMachineLearning.pModel;

/// <summary>
/// ドラムモデル
/// </summary>
internal class ModelFFTData
{
    /// <summary>
    /// ドラムパターン
    /// </summary>
    [LoadColumn( 0 )]
    public string DrumPattern { get; set; } = string.Empty;

    /// <summary>
    /// ノート位置
    /// </summary>
    [LoadColumn( 1 )]
    public int AbsoluteNotePos { get; set; } = 0;

    /// <summary>
    /// Hz中心
    /// </summary>
    [LoadColumn( 2 )]
    public float HzCenter { get; set; } = 0;

    /// <summary>
    /// 周辺Hz
    /// </summary>
    [LoadColumn( 3, 2 + 1024 ), VectorType( 1024 )]
    public float [] FFTBuffer { get; set; } = new float [ 1024 ];
}
