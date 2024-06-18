using System.Text.Json.Serialization;
using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pUtil;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// プレイヤー設定
/// </summary>
public class ConfigPlayerScore
{
    #region Bpm

    /// <summary>
    /// BPM行の高さ
    /// </summary>
    [JsonInclude]
    public float BpmHeightSize { get; set; } = 18F;

    /// <summary>
    /// BPMテキスト表示横幅
    /// </summary>
    [JsonInclude]
    public float BpmWidthSize { get; set; } = 40F;

    /// <summary>
    /// BPM描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect BpmRect
    {
        get; set;
    } = new()
    {
        Background = new( Color.FromArgb( 0, 0, 0, 0 ) ),
        Line = new( Color.FromArgb( 255, 60, 60, 60 ), 14F ),
        Text = new( Color.FromArgb( 255, 100, 200, 100 ),
                            new()
                            {
                                FontFamily = "system-ui",
                                FontSize = 14F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// 現在のBPM値表示フラグ
    /// </summary>
    [JsonInclude]
    public bool BpmNowDisplay { get; set; } = false;

    /// <summary>
    /// 現在のBPM値描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect BpmNowRect
    {
        get; set;
    } = new()
    {
        Background = new( Color.FromArgb( 160, 0, 0, 0 ) ),
        Line = new( Color.FromArgb( 255, 60, 60, 60 ), 0.4F ),
        Text = new( Color.FromArgb( 255, 100, 200, 100 ),
                            new()
                            {
                                FontFamily = "system-ui",
                                FontSize = 14F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment = CanvasVerticalAlignment.Center,
                            } ),
    };

    #endregion

    #region Measure number

    /// <summary>
    /// 小節番号行の高さ
    /// </summary>
    [JsonInclude]
    public float MeasureNoHeightSize { get; set; } = 18F;

    /// <summary>
    /// 小節番号描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect MeasureNoRect
    {
        get; set;
    } = new()
    {
        Background = new( Color.FromArgb( 0, 0, 0, 0 ) ),
        Line = new( Color.FromArgb( 255, 60, 60, 60 ), 0.4F ),
        Text = new( Color.FromArgb( 255, 100, 200, 100 ),
                            new()
                            {
                                FontFamily = "system-ui",
                                FontSize = 14F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment = CanvasVerticalAlignment.Center,
                            } ),
    };

    #endregion

    #region Header

    /// <summary>
    /// ヘッダーエフェクト
    /// </summary>
    [JsonInclude]
    public bool HeaderEffectOn { get; set; } = false;

    /// <summary>
    /// ヘッダー横幅
    /// </summary>
    [JsonInclude]
    public float HeaderWidthSize { get; set; } = 100F;

    /// <summary>
    /// ヘッダー描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatLine HeaderLine
    {
        get; set;
    } = new()
    {
        LineColor = new( Color.FromArgb( 255, 80, 80, 80 ) ),
        LineSize = 1.0F,
    };

    #endregion

    #region Note

    /// <summary>
    /// ノート高さ
    /// </summary>
    [JsonInclude]
    public float NoteHeightSize { get; set; } = 8F;

    /// <summary>
    /// ノート横幅
    /// </summary>
    [JsonInclude]
    public float NoteWidthSize { get; set; } = 8F;

    /// <summary>
    /// 音量ゼロノート表示フラグ
    /// </summary>
    [JsonInclude]
    public bool NoteVolumeZeroOn { get; set; } = false;

    /// <summary>
    /// 音量サイズ変更フラグ
    /// </summary>
    [JsonInclude]
    public bool NoteVolumeSizeOn { get; set; } = true;

    /// <summary>
    /// ノート間隔：横
    /// </summary>
    [JsonInclude]
    public float NoteTermWidthSize { get; set; } = 2F;

    /// <summary>
    /// ノート間隔：縦
    /// </summary>
    [JsonInclude]
    public float NoteTermHeightSize { get; set; } = 8F;

    #endregion

    #region Line

    /// <summary>
    /// カーソルペン
    /// </summary>
    [JsonInclude]
    public FormatLine CursolLine
    {
        get; set;
    } = new()
    {
        LineColor = new( Color.FromArgb( 255, 255, 255, 255 ) ),
        LineSize = 1.0F,
    };

    /// <summary>
    /// 小節128分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure128Line
    {
        get; set;
    } = new()
    {
        LineColor = new( Color.FromArgb( 255, 80, 80, 80 ) ),
        LineSize = 1.0F,
    };

    /// <summary>
    /// 小節64分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure064Line
    {
        get; set;
    } = new()
    {
        LineColor = new( Color.FromArgb( 255, 70, 70, 70 ) ),
        LineSize = 0.5F,
    };

    /// <summary>
    /// 小節32分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure032Line
    {
        get; set;
    } = new()
    {
        LineColor = new( Color.FromArgb( 255, 60, 60, 60 ) ),
        LineSize = 0.3F,
    };

    /// <summary>
    /// 小節16分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure016Line
    {
        get; set;
    } = new()
    {
        LineColor = new( Color.FromArgb( 255, 50, 50, 50 ) ),
        LineSize = 0.0F,
    };

    /// <summary>
    /// 小節8分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure008Line
    {
        get; set;
    } = new()
    {
        LineColor = new( Color.FromArgb( 255, 40, 40, 40 ) ),
        LineSize = 0.0F,
    };

    /// <summary>
    /// 小節4分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure004Line
    {
        get; set;
    } = new()
    {
        LineColor = new( Color.FromArgb( 255, 30, 30, 30 ) ),
        LineSize = 0.0F,
    };

    /// <summary>
    /// 小節1分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure001Line
    {
        get; set;
    } = new()
    {
        LineColor = new( Color.FromArgb( 255, 20, 20, 20 ) ),
        LineSize = 0.0F,
    };

    #endregion

    /// <summary>
    /// スコア最大横幅
    /// </summary>
	public float ScoreMaxWidth => NoteTermWidthSize * Config.System.NoteCount;

    /// <summary>
    /// スコア最大高さ
    /// </summary>
    public float ScoreMaxHeight => NoteTermHeightSize * DMS.SCORE.EditMidiMapSet.DisplayGroupCount;

    /// <summary>
    /// １小節の横幅
    /// </summary>
    public float MeasureSize => NoteTermWidthSize * Config.System.MeasureNoteNumber;
}
