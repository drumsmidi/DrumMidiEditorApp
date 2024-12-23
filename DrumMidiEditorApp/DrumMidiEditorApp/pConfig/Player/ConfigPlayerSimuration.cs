using System.Text.Json.Serialization;
using DrumMidiEditorApp.pUtil.pFormat;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// プレイヤー設定
/// </summary>
public class ConfigPlayerSimuration
{
    #region Sheet

    /// <summary>
    /// 背景色
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

    #region Bpm

    /// <summary>
    /// 現在のBPM値表示フラグ
    /// </summary>
    [JsonInclude]
    public bool BpmNowDisplay { get; set; } = true;

    /// <summary>
    /// BPMテキスト表示横幅
    /// </summary>
    [JsonInclude]
    public float BpmNowWidthSize { get; set; } = 60F;

    /// <summary>
    /// BPM行の高さ
    /// </summary>
    [JsonInclude]
    public float BpmNowHeightSize { get; set; } = 36F;

    /// <summary>
    /// 現在のBPM値描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect BpmNowRect
    {
        get; set;
    } = new()
        {
            Background  = new( Color.FromArgb( 160, 0, 0, 0 ) ),
            Line        = new( Color.FromArgb( 255, 60, 60, 60 ), 0F ),
            Text        = new( Color.FromArgb( 255, 100, 200, 100 ),
                                new()
                                {
                                    FontFamily          = Config.Media.DefaultFontFamily,
                                    FontSize            = 18F,
                                    HorizontalAlignment = CanvasHorizontalAlignment.Center,
                                    VerticalAlignment   = CanvasVerticalAlignment.Center,
                                } ),
        };

    #endregion

    #region Measure number

    /// <summary>
    /// 小節番号表示フラグ
    /// </summary>
    [JsonInclude]
    public bool MeasureNoDisplay { get; set; } = true;

    /// <summary>
    /// 小節番号行の横幅
    /// </summary>
    [JsonInclude]
    public float MeasureNoWidthSize { get; set; } = 50F;

    /// <summary>
    /// 小節番号行の高さ
    /// </summary>
    [JsonInclude]
    public float MeasureNoHeightSize { get; set; } = 36F;

    /// <summary>
    /// 小節番号描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect MeasureNoRect
    {
        get; set;
    } = new()
        {
            Background  = new( Color.FromArgb( 160, 0, 0, 0 ) ),
            Line        = new( Color.FromArgb( 255, 60, 60, 60 ), 0F ),
            Text        = new( Color.FromArgb( 255, 100, 200, 100 ),
                                new()
                                {
                                    FontFamily          = Config.Media.DefaultFontFamily,
                                    FontSize            = 18F,
                                    HorizontalAlignment = CanvasHorizontalAlignment.Center,
                                    VerticalAlignment   = CanvasVerticalAlignment.Center,
                                } ),
        };

    #endregion

    #region Header

    /// <summary>
    /// ヘッダーエフェクト
    /// </summary>
    [JsonInclude]
    public bool HeaderEffectOn { get; set; } = true;

    /// <summary>
    /// ヘッダー文字
    /// </summary>
    [JsonInclude]
    public bool HeaderStrOn { get; set; } = true;

    /// <summary>
    /// ヘッダー横幅
    /// </summary>
    [JsonInclude]
    public float HeaderSize { get; set; } = 80F;

    /// <summary>
    /// ヘッダー描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect HeaderRect
    {
        get; set;
    } = new()
        {
            Background  = new( Color.FromArgb( 0, 0, 0, 0 ) ),
            Line        = new( Color.FromArgb( 255, 60, 60, 60 ), 0.4F ),
            Text        = new( Color.FromArgb( 255, 100, 200, 100 ),
                                new()
                                {
                                    FontFamily          = Config.Media.DefaultFontFamily,
                                    FontSize            = 14F,
                                    HorizontalAlignment = CanvasHorizontalAlignment.Center,
                                    VerticalAlignment   = CanvasVerticalAlignment.Center,
                                } ),
        };

    #endregion

    #region Note

    /// <summary>
    /// ノート間隔：横
    /// </summary>
    [JsonInclude]
    public float NoteTermSize { get; set; } = 2;

    /// <summary>
    /// １回の描画で描画する小節数
    /// </summary>
    [JsonInclude]
    public int DrawMeasureCount { get; set; } = 2;

    #endregion

    /// <summary>
    /// １小節の横幅
    /// </summary>
    public float MeasureSize => NoteTermSize * Config.System.MeasureNoteNumber;
}
