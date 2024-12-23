using System.Text.Json.Serialization;
using DrumMidiEditorApp.pUtil.pFormat;
using Windows.UI;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// Scoreタブ設定
/// </summary>
public class ConfigML
{
    #region Update flag

    /// <summary>
    /// BGM更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreBgmScaleFlag { get; set; } = false;

    #endregion

    #region Sheet

    /// <summary>
    /// シート背景色
    /// </summary>
    [JsonInclude]
    public FormatColor SheetColor
    {
        get; set;
    } = new()
    {
        Color = Color.FromArgb( 255, 0, 0, 0 ),
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

    #endregion
}
