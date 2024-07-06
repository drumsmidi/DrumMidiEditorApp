using System.Collections.Generic;
using System.Text.Json.Serialization;
using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pUtil;
using Microsoft.Graphics.Canvas.Text;
using Windows.Foundation;
using Windows.UI;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// Editerタブ設定
/// </summary>
public class ConfigEditer
{
    #region Update flag

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
    public List<int> UpdateScoreBpmMeasureNoList { get; private set; } = [];

    /// <summary>
    /// 更新対象の小節番号リスト：NOTE
    /// </summary>
    public List<int> UpdateScoreNoteMeasureNoList { get; private set; } = [];

    /// <summary>
    /// 更新対象の小節番号リスト：NOTE音量
    /// </summary>
    public List<int> UpdateScoreNoteVolumeMeasureNoList { get; private set; } = [];

    #endregion

    #region 初期値設定

    /// <summary>
    /// 初期値：テキストカラー（共通）
    /// </summary>
    [JsonIgnore]
    private static Color _BassTextColor = Color.FromArgb( 255, 100, 200, 100 );

    /// <summary>
    /// 初期値：フォントファミリア（共通）
    /// </summary>
    [JsonIgnore]
    private static readonly string _BassFontFamily = "system-ui";

    /// <summary>
    /// 初期値：フォントサイズ（共通）
    /// </summary>
    [JsonIgnore]
    private static readonly float _BassFontSize = 14F;

    /// <summary>
    /// 初期値：ラインサイズ（共通）
    /// </summary>
    [JsonIgnore]
    private static Color _BassLineColor = Color.FromArgb( 255, 90, 90, 90 );

    /// <summary>
    /// 初期値：ラインサイズ（共通）
    /// </summary>
    [JsonIgnore]
    private static readonly float _BassLineSize = 1F;

    /// <summary>
    /// 初期値：ライン高さ（共通）
    /// </summary>
    [JsonIgnore]
    private static readonly float _BassLineHeight = 16F;

    #endregion

    #region Info

    /// <summary>
    /// 情報ヘッダ背景色
    /// </summary>
    [JsonInclude]
    public FormatRect InfoHeaderRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 0, 0, 0 ) ),
        Line        = new( _BassLineColor, _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    #endregion

    #region Bpm

    /// <summary>
    /// BPM行：高さ
    /// </summary>
    [JsonInclude]
    public float BpmHeightSize { get; set; } = _BassLineHeight;

    /// <summary>
    /// BPMヘッダ背景色
    /// </summary>
    [JsonInclude]
    public FormatRect BpmHeadRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 0, 0, 0 ) ),
        Line        = new( _BassLineColor, _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// BPMボディ背景色
    /// </summary>
    [JsonInclude]
    public FormatRect BpmBodyRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 0, 0, 0 ) ),
        Line        = new( _BassLineColor, _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// BPM非選択時の背景色
    /// </summary>
    [JsonInclude]
    public FormatRect BpmNonSelectRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 0, 200, 0 ) ),
        Line        = new( Color.FromArgb( 255, 90, 90, 90 ), _BassLineSize ),
        Text        = new( Color.FromArgb( 255, 100, 200, 100 ),
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// BPM選択時の背景色
    /// </summary>
    [JsonInclude]
    public FormatRect BpmSelectRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 200, 0, 0 ) ),
        Line        = new( Color.FromArgb( 255, 90, 90, 90 ), _BassLineSize ),
        Text        = new( Color.FromArgb( 255, 100, 200, 100 ),
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    #endregion

    #region Measure No

    /// <summary>
    /// 小節番号行：高さ
    /// </summary>
    [JsonInclude]
    public float MeasureNoHeightSize { get; set; } = _BassLineHeight;

    /// <summary>
    /// 小節番号ヘッダ背景色
    /// </summary>
    [JsonInclude]
    public FormatRect MeasureNoHeadRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 60, 60, 60 ) ),
        Line        = new( _BassLineColor, _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// 小節番号ボディ背景色
    /// </summary>
    [JsonInclude]
    public FormatRect MeasureNoBodyRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 60, 60, 60 ) ),
        Line        = new( _BassLineColor, _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    #endregion

    #region Header

    /// <summary>
    /// ヘッダー横幅
    /// </summary>
    [JsonInclude]
    public float HeaderWidthSize { get; set; } = 200F;

    /// <summary>
    /// ヘッダー非選択時の書式
    /// </summary>
    [JsonInclude]
    public FormatRect HeaderNonSelectRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 20, 20, 20 ) ),
        Line        = new( _BassLineColor, _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// ヘッダー選択時の書式
    /// </summary>
    [JsonInclude]
    public FormatRect HeaderSelectRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 80, 40, 40 ) ),
        Line        = new( _BassLineColor, _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    #endregion

    #region Note

    /// <summary>
    /// シートノート位置
    /// </summary>
    [JsonIgnore]
    public PointInt NotePosition { get; set; } = new( 0, 0 );

    /// <summary>
    /// ノートOFFペン
    /// </summary>
    [JsonInclude]
    public FormatLine NoteOffLine
    {
        get; set;
    } = new()
    {
        LineSize = _BassLineSize,
    };

    /// <summary>
    /// ノート選択時の外枠ペン
    /// </summary>
    [JsonInclude]
    public FormatRect NoteSelectRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 0, 0, 0, 0 ) ),
        Line        = new( Color.FromArgb( 255, 150, 20, 20 ), _BassLineSize * 2 ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// 機械学習 予測結果用のノート外枠ペン
    /// </summary>
    [JsonInclude]
    public FormatRect NotePredictRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 0, 0, 0, 0 ) ),
        Line        = new( Color.FromArgb( 255, 200, 200, 200 ), _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// ノート高さ
    /// </summary>
    [JsonInclude]
    public float NoteHeightSize { get; set; } = _BassLineHeight;

    /// <summary>
    /// ノート間の横間隔。
    /// ノートの横幅はノート高さを適用します。
    /// </summary>
    [JsonInclude]
    public float NoteWidthSize { get; set; } = 4F;

    /// <summary>
    /// NoteOn入力フラグ（True:NoteOn, False:NoteOff）
    /// </summary>
    [JsonIgnore]
    public bool NoteOn { get; set; } = true;

    #endregion

    #region Volume

    /// <summary>
    /// 音量ヘッダ背景色
    /// </summary>
    [JsonInclude]
    public List<int> NoteSelectVolumeList
    {
        get; set;
    } =
    [
        0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50,
        55, 60, 65, 70, 75, 80, 85, 90, 95, 100,
        105, 110, 115, 120, 125, 127,
    ];

    /// <summary>
    /// ノート入力音量
    /// </summary>
    [JsonIgnore]
    public int NoteSelectVolume { get; set; } = 100;

    /// <summary>
    /// ノート入力音量（インデックス）
    /// </summary>
    [JsonIgnore]
    public int NoteSelectVolumeIndex
    {
        get => NoteSelectVolumeList.IndexOf( NoteSelectVolume );
        set => NoteSelectVolume = NoteSelectVolumeList [ value ];
    }

    /// <summary>
    /// 音量行の高さ
    /// </summary>
    [JsonIgnore]
    public float VolumeHeightSize { get; set; } = 150F;

    /// <summary>
    /// 音量ヘッダ背景色
    /// </summary>
    [JsonInclude]
    public FormatRect VolumeHeadRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 0, 0, 0 ) ),
        Line        = new( _BassLineColor, _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// 音量ボディ背景色
    /// </summary>
    [JsonInclude]
    public FormatRect VolumeBodyRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 0, 0, 0 ) ),
        Line        = new( _BassLineColor, _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// 音量入力ペン
    /// </summary>
    [JsonInclude]
    public FormatLine VolumeInputLine
    {
        get; set;
    } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 255, 0, 0 ) ),
        LineSize    = _BassLineSize,
    };

    /// <summary>
    /// 音量行表示フラグ
    /// </summary>
    [JsonIgnore]
    public bool VolumeDisplay { get; set; } = false;

    /// <summary>
    /// 音量値表示フラグ
    /// </summary>
    [JsonInclude]
    public bool VolumeValueDisplay { get; set; } = true;

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
        UpDown,
        IntonationHL,
        IntonationH,
        IntonationL,
    }

    /// <summary>
    /// 音量入力タイプ
    /// </summary>
    [JsonInclude]
    public VolumeEditType VolumeEditSelect { get; set; } = VolumeEditType.FreeHand;

    /// <summary>
    /// 音量入力タイプ
    /// </summary>
    [JsonIgnore]
    public int VolumeEditSelectIndex
    {
        get => (int)VolumeEditSelect;
        set => VolumeEditSelect = (VolumeEditType)value;
    }

    #endregion

    #region Sheet

    /// <summary>
    /// シートタイマー時間（秒）
    /// </summary>
    [JsonInclude]
    public double SheetTimerSecond { get; set; } = 0.1d;

    /// <summary>
    /// ノート範囲選択・移動時にスコア内の外周付近でシート位置を移動させる範囲のサイズを指定
    /// </summary>
    [JsonInclude]
    public Size SheetMovePaddingSize { get; set; } = new( 80, 60 );

    /// <summary>
    /// シート移動速度
    /// </summary>
    [JsonInclude]
    public float SheetMoveSpeed { get; set; } = 0.5F;

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

    /// <summary>
    /// シートカーソル縦線
    /// </summary>
    [JsonInclude]
    public FormatLine SheetCursorVerticleLine
    {
        get; set;
    } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 30, 30, 30 ) ),
        LineSize    = _BassLineSize,
    };

    /// <summary>
    /// シートカーソル横線
    /// </summary>
    [JsonInclude]
    public FormatRect SheetCursorHorizonRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 30, 30, 30 ) ),
        Line        = new( Color.FromArgb( 255, 30, 30, 30 ), _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
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
        LineColor   = new( Color.FromArgb( 255, 80, 80, 80 ) ),
        LineSize    = _BassLineSize,
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
        LineColor   = new( Color.FromArgb( 255, 70, 70, 70 ) ),
        LineSize    = _BassLineSize,
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
        LineColor   = new( Color.FromArgb( 255, 60, 60, 60 ) ),
        LineSize    = _BassLineSize,
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
        LineColor   = new( Color.FromArgb( 255, 50, 50, 50 ) ),
        LineSize    = _BassLineSize,
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
        LineColor   = new( Color.FromArgb( 255, 40, 40, 40 ) ),
        LineSize    = _BassLineSize,
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
        LineColor   = new( Color.FromArgb( 255, 30, 30, 30 ) ),
        LineSize    = 0.0F,
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
        LineColor   = new( Color.FromArgb( 255, 20, 20, 20 ) ),
        LineSize    = 0.0F,
    };

    /// <summary>
    /// MidiMapGroup横線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetStaffGroupLine
    {
        get; set;
    } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 120, 120, 120 ) ),
        LineSize    = _BassLineSize,
    };

    /// <summary>
    /// MidiMap横線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetStaffMidiMapLine
    {
        get; set;
    } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 30, 30, 30 ) ),
        LineSize    = _BassLineSize,
    };


    /// <summary>
    /// １小節辺りのノート区切り数リスト
    /// </summary>
    [JsonInclude]
    public List<int> SheetDivisionLineList
    {
        get; set;
    } =
    [
        128, 64, 32, 16, 8, 4, 2, 1,
    ];

    /// <summary>
    /// １小節辺りのノート区切り数
    /// （全音入力、４分音符入力 的な）
    /// </summary>
    [JsonIgnore]
    public int SheetDivisionLine { get; set; } = 16;

    /// <summary>
    /// １小節辺りのノート区切り数（インデックス）
    /// </summary>
    [JsonIgnore]
    public int SheetDivisionLineIndex
    {
        get => SheetDivisionLineList.IndexOf( SheetDivisionLine );
        set => SheetDivisionLine = SheetDivisionLineList [ value ];
    }


    #endregion

    #region Support line

    /// <summary>
    /// サポート線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetSupportLine
    {
        get; set;
    } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 80, 100, 255 ) ),
        LineSize    = _BassLineSize,
    };

    #endregion

    #region Note range

    /// <summary>
    /// ノート範囲選択ペン
    /// </summary>
    [JsonInclude]
    public FormatRect NoteRangeRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb(  30, 255, 0, 0 ) ),
        Line        = new( Color.FromArgb( 255, 255, 0, 0 ), _BassLineSize ),
        Text        = new( _BassTextColor,
                            new()
                            {
                                FontFamily          = _BassFontFamily,
                                FontSize            = _BassFontSize,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

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

    /// <summary>
    /// 範囲選択タイプ
    /// </summary>
    [JsonIgnore]
    public int RangeSelectIndex
    {
        get => (int)RangeSelect;
        set => RangeSelect = (RangeSelectType)value;
    }

    /// <summary>
    /// 範囲選択状態
    /// </summary>
    [JsonIgnore]
    public bool IsRangeSelect { get; set; } = false;

    #endregion

    /// <summary>
    /// レジュームスタック数
    /// </summary>
    [JsonInclude]
    public int ResumeStackCount { get; set; } = 5000;

    /// <summary>
    /// スコア高さ
    /// </summary>
	public float ScoreMaxHeight
        => NoteHeightSize * DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount;

    /// <summary>
    /// １小節の横幅
    /// </summary>
    public float MeasureSize
        => NoteWidthSize * Config.System.MeasureNoteNumber;

    /// <summary>
    /// １小節辺りのノート区切り数
    /// </summary>
    public int DivisionLineCount
        => Config.System.MeasureNoteNumber / SheetDivisionLine;
}
