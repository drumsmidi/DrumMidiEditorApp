using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

using DrumMidiEditor.pDMS;

using Font = System.Drawing.Font;

namespace DrumMidiEditor.pConfig;

/// <summary>
/// Editerタブ設定
/// </summary>
public class ConfigEditer
{
    #region Update flag

    /// <summary>
    /// Undo実行フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateUndoFlag { get; set; } = false;

    /// <summary>
    /// Redo実行フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateRedoFlag { get; set; } = false;

    /// <summary>
    /// 履歴クリアフラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateResumeClearFlag { get; set; } = false;

    /// <summary>
    /// シート移動フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateCameraFlag { get; set; } = false;

    /// <summary>
    /// スコア全体更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreFlag { get; set; } = false;

    /// <summary>
    /// 表示レイアウト更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreLayoutFlag { get; set; } = false;

    /// <summary>
    /// 小節線更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreLineFlag { get; set; } = false;

    /// <summary>
    /// BGM更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreBgmFlag { get; set; } = false;

    /// <summary>
    /// BGM音階画像更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreBgmScaleFlag { get; set; } = false;

    /// <summary>
    /// BPM更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreBpmFlag { get; set; } = false;

    /// <summary>
    /// ノート編集フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreNoteFlag { get; set; } = false;

    /// <summary>
    /// 機械学習 予測ノート更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreNotePredictFlag { get; set; } = false;

    /// <summary>
    /// ノート音量更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreNoteVolumeFlag { get; set; } = false;

    /// <summary>
    /// ヘッダ更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreHeaderFlag { get; set; } = false;

    /// <summary>
    /// ノート範囲選択クリアフラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateClearRangeFlag { get; set; } = false;

    /// <summary>
    /// 更新対象の小節番号リスト：BPM
    /// </summary>
    public List<int> UpdateScoreBpmMeasureNoList { get; private set; } = new();

    /// <summary>
    /// 更新対象の小節番号リスト：NOTE
    /// </summary>
    public List<int> UpdateScoreNoteMeasureNoList { get; private set; } = new();

    /// <summary>
    /// 更新対象の小節番号リスト：NOTE音量
    /// </summary>
    public List<int> UpdateScoreNoteVolumeMeasureNoList { get; private set; } = new();

    #endregion

    #region 文字共通

    /// <summary>
    /// 文字書式（共通）
    /// </summary>
    [JsonInclude]
    public StringFormat StrFmt { get; set; } = new()
    { 
        Alignment		= StringAlignment.Near,
        LineAlignment   = StringAlignment.Center,
    };

    /// <summary>
    /// 文字フォント（共通）
    /// </summary>
    [JsonInclude]
    public Font StrFont { get; set; } = new( FontFamily.GenericSerif, 8F );

    /// <summary>
    /// 文字色（共通）
    /// </summary>
    [JsonInclude]
    public SolidBrush StrBrush { get; set; } = new( Color.FromArgb( 255, 100, 200, 100 ) );

    #endregion

    #region Info

    /// <summary>
    /// 情報ヘッダ背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush InfoHeadBrush { get; set; } = new( Color.FromArgb( 255, 0, 0, 0 ) );

    #endregion

    #region Bpm

    /// <summary>
    /// BPM行：高さ
    /// </summary>
    [JsonInclude]
    public int BpmHeightSize { get; set; } = 12;

    /// <summary>
    /// BPMヘッダ背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush BpmHeadBrush { get; set; } = new( Color.FromArgb( 255, 0, 0, 0 ) );

    /// <summary>
    /// BPMボディ背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush BpmBodyBrush { get; set; } = new( Color.FromArgb( 255, 0, 0, 0 ) );

    /// <summary>
    /// BPM非選択時の背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush BpmNonSelectBrush { get; set; } = new( Color.FromArgb( 255, 0, 200, 0 ) );

    /// <summary>
    /// BPM選択時の背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush BpmSelectBrush { get; set; } = new( Color.FromArgb( 255, 200, 0, 0 ) );

    #endregion

    #region Measure No

    /// <summary>
    /// 小節番号行：高さ
    /// </summary>
    [JsonInclude]
    public int MeasureNoHeightSize { get; set; } = 12;

    /// <summary>
    /// 小節番号ヘッダ背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush MeasureNoHeadBrush { get; set; } = new( Color.FromArgb( 255, 60, 60, 60 ) );

    /// <summary>
    /// 小節番号ボディ背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush MeasureNoBodyBrush { get; set; } = new( Color.FromArgb( 255, 60, 60, 60 ) );

    #endregion

    #region Header

    /// <summary>
    /// ヘッダー横幅
    /// </summary>
    [JsonInclude]
    public int HeaderWidthSize { get; set; } = 150;

    /// <summary>
    /// ヘッダー非選択時の背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush HeaderNonSelectBrush { get; set; } = new( Color.FromArgb( 255, 20, 20, 20 ) );

    /// <summary>
    /// ヘッダー選択時の背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush HeaderSelectBrush { get; set; } = new( Color.FromArgb( 255, 80, 40, 40 ) );

    /// <summary>
    /// ヘッダー外枠ペン
    /// </summary>
    [JsonInclude]
    public Pen HeaderLinePen { get; set; } = new( Color.FromArgb( 255, 90, 90, 90 ), 1F );

    #endregion

    #region Note
        
    /// <summary>
    /// シートノート位置
    /// </summary>
    [JsonIgnore]
    public Point NotePosition { get; set; } = new( 0, 0 );

    /// <summary>
    /// ノート選択時の外枠ペン
    /// </summary>
    [JsonInclude]
    public Pen NoteSelectPen { get; set; } = new( Color.FromArgb( 255, 150, 20, 20 ), 2.0F );

    /// <summary>
    /// ノート非選択時の外枠ペン
    /// </summary>
    [JsonInclude]
    public Pen NoteNonSelectPen { get; set; } = new( Color.FromArgb( 255, 0, 0, 0 ), 1.0F );

    /// <summary>
    /// 機械学習 予測結果用のノート外枠ペン
    /// </summary>
    [JsonInclude]
    public Pen NotePredictPen { get; set; } = new( Color.FromArgb( 255, 200, 200, 200 ), 1.0F );

    /// <summary>
    /// ノート高さ
    /// </summary>
    [JsonInclude]
    public int NoteHeightSize { get; set; } = 10;

    /// <summary>
    /// ノート間の横間隔。
    /// ノートの横幅はノート高さを適用します。
    /// </summary>
    [JsonInclude]
    public int NoteWidthSize { get; set; } = 4;

    /// <summary>
    /// NoteOn入力フラグ（True:NoteOn, False:NoteOff）
    /// </summary>
    [JsonIgnore]
    public bool NoteOn { get; set; } = true;

    #endregion

    #region Volume

    /// <summary>
    /// ノート入力音量
    /// </summary>
    [JsonIgnore]
    public int NoteSelectVolume { get; set; } = 100;

    /// <summary>
    /// 音量行の高さ
    /// </summary>
    [JsonIgnore]
    public int VolumeHeightSize { get; set; } = 150;

    /// <summary>
    /// 音量ヘッダ背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush VolumeHeadBrush { get; set; } = new( Color.FromArgb( 255, 0, 0, 0 ) );

    /// <summary>
    /// 音量ボディ背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush VolumeBodyBrush { get; set; } = new( Color.FromArgb( 255, 0, 0, 0 ) );

    /// <summary>
    /// 音量入力ペン
    /// </summary>
    [JsonInclude]
    public Pen VolumePen { get; set; } = new( Color.FromArgb( 255, 255, 0, 0 ), 1.0F );

    /// <summary>
    /// 音量行表示フラグ
    /// </summary>
    [JsonIgnore]
    public bool VolumeDisplay { get; set; } = false;

    /// <summary>
    /// 音量入力タイプ
    /// </summary>
    public enum VolumeEditType : int
    {
        FreeHand = 0,
        StraightLine,
        Curve1,
        Curve2,
        Curve3,
        Curve4,
        VolumeUpDown,
        VolumeIntonationHL,
        VolumeIntonationH,
        VolumeIntonationL,
    }

    /// <summary>
    /// 音量入力タイプ
    /// </summary>
    [JsonInclude]
    public VolumeEditType VolumeEditSelect { get; set; } = VolumeEditType.FreeHand;

    #endregion

    #region Sheet

    /// <summary>
    /// シート背景色
    /// </summary>
    [JsonInclude]
    public Color SheetColor { get; set; } = Color.Black;

    /// <summary>
    /// シートカーソル縦線
    /// </summary>
    [JsonInclude]
    public Pen SheetCursorLineV { get; set; } = new( Color.FromArgb( 255, 30, 30, 30 ), 1F );

    /// <summary>
    /// シートカーソル横線
    /// </summary>
    [JsonInclude]
    public SolidBrush SheetCursorLineH { get; set; } = new( Color.FromArgb( 255, 30, 30, 30 ) );

    /// <summary>
    /// 小節128分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure128Pen { get; set; } = new( Color.FromArgb( 255, 80, 80, 80 ), 1F );

    /// <summary>
    /// 小節64分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure064Pen { get; set; } = new( Color.FromArgb( 255, 70, 70, 70 ), 1F );

    /// <summary>
    /// 小節32分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure032Pen { get; set; } = new( Color.FromArgb( 255, 60, 60, 60 ), 1F );

    /// <summary>
    /// 小節16分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure016Pen { get; set; } = new( Color.FromArgb( 255, 50, 50, 50 ), 1F );

    /// <summary>
    /// 小節8分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure008Pen { get; set; } = new( Color.FromArgb( 255, 40, 40, 40 ), 1F );

    /// <summary>
    /// 小節4分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure004Pen { get; set; } = new( Color.FromArgb( 255, 30, 30, 30 ), 0F );

    /// <summary>
    /// 小節1分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure001Pen { get; set; } = new( Color.FromArgb( 255, 20, 20, 20 ), 0F );

    /// <summary>
    /// MidiMapGroup横線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetStaffGroupPen { get; set; } = new( Color.FromArgb( 255, 120, 120, 120 ), 1F );

    /// <summary>
    /// MidiMap横線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetStaffMidiMapPen { get; set; } = new( Color.FromArgb( 255, 30, 30, 30 ), 1F );

    /// <summary>
    /// １小節辺りのノート区切り数
    /// （全音入力、４分音符入力 的な）
    /// </summary>
    [JsonIgnore]
    public int SheetDivisionLine { get; set; } = 128;

    #endregion

    #region Support line

    /// <summary>
    /// サポート線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetSupportPen { get; set; } = new( Color.FromArgb( 255, 0, 100, 255 ), 1F );

    #endregion

    #region Note range

    /// <summary>
    /// ノート範囲選択ペン
    /// </summary>
    [JsonInclude]
    public Pen NoteRangePen { get; set; } = new( Color.FromArgb( 255, 255, 0, 0 ), 1F );

    /// <summary>
    /// ノート範囲選択にBPMの選択を含む
    /// </summary>
    [JsonIgnore]
    public bool IncludeBpm { get; set; } = false;

    /// <summary>
    /// 範囲選択タイプ
    /// </summary>
    public enum RangeSelectType
    {
        Normal = 0,
        Split,
        All,
    }

    /// <summary>
    /// 範囲選択タイプ
    /// </summary>
    [JsonInclude]
    public RangeSelectType RangeSelect { get; set; } = RangeSelectType.Normal;

    #endregion

    /// <summary>
    /// レジュームスタック数
    /// </summary>
    [JsonInclude]
    public int ResumeStackCount { get; set; } = 5000;

    /// <summary>
    /// スコア高さ
    /// </summary>
	public int ScoreMaxHeight 
        => NoteHeightSize * DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount; 

    /// <summary>
    /// １小節の横幅
    /// </summary>
	public int MeasureSize 
        => NoteWidthSize * Config.System.MeasureNoteNumber;

    /// <summary>
    /// １小節辺りのノート区切り数
    /// </summary>
    public int DivisionLineCount 
        => Config.System.MeasureNoteNumber / SheetDivisionLine;
}
