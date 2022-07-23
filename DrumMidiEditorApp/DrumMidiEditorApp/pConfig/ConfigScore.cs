using System.Text.Json.Serialization;
using Windows.UI;

using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// Scoreタブ設定
/// </summary>
public class ConfigScore
{
    #region Update flag

    /// <summary>
    /// スコア更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreFlag { get; set; } = false;

    #endregion

    #region Sheet

    /// <summary>
    /// シート背景色
    /// </summary>
    [JsonInclude]
    public FormatColor SheetColor { get; set; } = new()
    {
        Color = Color.FromArgb( 255, 0, 0, 0 ),
    };

    #endregion

    #region Measure

    /// <summary>
    /// 小節囲い線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine MeasureLine { get; set; } = new()
    {
        LineColor   = Color.FromArgb( 255, 200, 200, 200 ),
        LineSize    = 1.0F,
    };

    #endregion

    #region Note

    /// <summary>
    /// ノート横幅
    /// </summary>
    [JsonInclude]
    public float NoteWidthSize { get; set; } = 8;

    /// <summary>
    /// ノート高さ
    /// </summary>
    [JsonInclude]
    public float NoteHeightSize { get; set; } = 8;

    /// <summary>
    /// ノート音量０表示フラグ
    /// </summary>
    [JsonInclude]
    public bool NoteVolumeZeroOn { get; set; } = false;

    /// <summary>
    /// ノート音量サイズ表示フラグ
    /// </summary>
    [JsonInclude]
    public bool NoteVolumeSizeOn { get; set; } = true;

    #endregion
}
