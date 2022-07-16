using System.Drawing;
using System.Text.Json.Serialization;

using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pDirectX;

namespace DrumMidiEditor.pConfig;

/// <summary>
/// プレイヤー設定
/// </summary>
public class ConfigPlayerSequenceVertical
{
    #region Bpm

    /// <summary>
    /// BPM行の高さ
    /// </summary>
    [JsonInclude]
    public int BpmHeightSize { get; set; } = 18;

    /// <summary>
    /// BPMテキスト表示横幅
    /// </summary>
    [JsonInclude]
    public int BpmWidthSize { get; set; } = 40;

    /// <summary>
    /// BPM描画アイテム
    /// </summary>
    [JsonInclude]
    public GraphicItem BpmGI { get; set; } = new()
    {
        BackBrush   = new( Color.FromArgb( 255,   0, 200,   0 ) ),
        ForePen     = new( Color.FromArgb( 255,  60,  60,  60 ) ),
        FontBrush   = new( Color.FromArgb( 255, 100, 200, 100 ) ),
        Font        = new( FontFamily.GenericSerif, 14F ),
        FontFormat  = new()
            {
                Alignment       = StringAlignment.Near,
                LineAlignment   = StringAlignment.Center,
            }
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
    public GraphicItem BpmNowGI { get; set; } = new()
    {
        BackBrush   = new( Color.FromArgb( 160,   0,   0,   0 ) ),
        ForePen     = new( Color.FromArgb( 255,  60,  60,  60 ), 0.4F ),
        FontBrush   = new( Color.FromArgb( 255, 100, 200, 100 ) ),
        Font        = new( FontFamily.GenericSerif, 14F ),
        FontFormat  = new()
            {
                Alignment       = StringAlignment.Near,
                LineAlignment   = StringAlignment.Center,
            }
    };

    #endregion

    #region Measure number

    /// <summary>
    /// 小節番号行の横幅
    /// </summary>
    [JsonInclude]
    public int MeasureNoWidthSize { get; set; } = 22;

    /// <summary>
    /// 小節番号描画アイテム
    /// </summary>
    [JsonInclude]
    public GraphicItem MeasureNoGI { get; set; } = new()
    {
        BackBrush   = new( Color.FromArgb( 255,   0,   0,   0 ) ),
        ForePen     = new( Color.FromArgb( 255,  60,  60,  60 ) ),
        FontBrush   = new( Color.FromArgb( 255, 100, 200, 100 ) ),
        Font        = new( FontFamily.GenericSerif, 14F ),
        FontFormat  = new()
            {
                Alignment       = StringAlignment.Far,
                LineAlignment   = StringAlignment.Far,
            }
    };

    #endregion

    #region Header

    /// <summary>
    /// ヘッダーグループ表示
    /// </summary>
    [JsonInclude]
    public bool HeaderGroupOn { get; set; } = false;

    /// <summary>
    /// ヘッダーエフェクト
    /// </summary>
    [JsonInclude]
    public bool HeaderEffectOn { get; set; } = false;

    /// <summary>
    /// ヘッダー高さ
    /// </summary>
    [JsonInclude]
    public int HeaderGroupHeightSize { get; set; } = 30;

    /// <summary>
    /// ヘッダー高さ
    /// </summary>
    [JsonInclude]
    public int HeaderHeightSize { get; set; } = 100;

    /// <summary>
    /// ヘッダー高さ
    /// </summary>
    [JsonInclude]
    public int HeaderTotalHeightSize 
        => HeaderGroupOn ? HeaderHeightSize : HeaderHeightSize + HeaderGroupHeightSize;

    /// <summary>
    /// ヘッダー描画アイテム
    /// </summary>
    [JsonInclude]
    public GraphicItem HeaderGI { get; set; } = new()
    {
        BackBrush   = new( Color.FromArgb( 160,   0,   0,   0 ) ),
        ForePen     = new( Color.FromArgb( 255,  60,  60,  60 ), 0.4F ),
        FontBrush   = new( Color.FromArgb( 255, 100, 200, 100 ) ),
        Font        = new( FontFamily.GenericSerif, 14F ),
        FontFormat  = new()
            {
                Alignment       = StringAlignment.Center,
                LineAlignment   = StringAlignment.Near,
            }
    };

    #endregion

    #region Note

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
    /// ノート高さ
    /// </summary>
    [JsonIgnore]
    public int NoteHeightSize
    {
        get => HeaderGroupOn ? NoteGroupOnHeightSize : NoteGroupOffHeightSize;
        set
        {
            if ( HeaderGroupOn )
            {
                NoteGroupOnHeightSize = value;
            }
            else
            {
                NoteGroupOffHeightSize = value;
            }
        }
    }

    /// <summary>
    /// ノート高さ：グループON時
    /// </summary>
    [JsonInclude]
    public int NoteGroupOnHeightSize { get; set; } = 16;

    /// <summary>
    /// ノート高さ：グループOFF時
    /// </summary>
    [JsonInclude]
    public int NoteGroupOffHeightSize { get; set; } = 6;

    /// <summary>
    /// ノート横幅
    /// </summary>
    [JsonIgnore]
    public int NoteWidthSize
    {
        get => HeaderGroupOn ? NoteGroupOnWidthSize : NoteGroupOffWidthSize;
        set
        {
            if ( HeaderGroupOn )
            {
                NoteGroupOnWidthSize = value;
            }
            else
            {
                NoteGroupOffWidthSize = value;
            }
        }
    }

    /// <summary>
    /// ノート横幅：グループON時
    /// </summary>
    [JsonInclude]
    public int NoteGroupOnWidthSize { get; set; } = 16;

    /// <summary>
    /// ノート横幅：グループOFF時
    /// </summary>
    [JsonInclude]
    public int NoteGroupOffWidthSize { get; set; } = 6;

    /// <summary>
    /// ノート間隔：横
    /// </summary>
    [JsonInclude]
    public int NoteTermWidthSize
    {
        get => HeaderGroupOn ? NoteGroupOnTermWidthSize : NoteGroupOffTermWidthSize ;
        set
        {
            if ( HeaderGroupOn )
            {
                NoteGroupOnTermWidthSize = value;
            }
            else
            {
                NoteGroupOffTermWidthSize = value;
            }
        }
    }

    /// <summary>
    /// ノート間隔：横：グループON時
    /// </summary>
    [JsonInclude]
    public int NoteGroupOnTermWidthSize { get; set; } = 10;

    /// <summary>
    /// ノート間隔：横：グループOFF時
    /// </summary>
    [JsonInclude]
    public int NoteGroupOffTermWidthSize { get; set; } = 6;

    /// <summary>
    /// ノート間隔：縦
    /// </summary>
    [JsonInclude]
    public int NoteTermHeightSize { get; set; } = 2;

    #endregion

    #region Line

    /// <summary>
    /// 小節128分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure128Pen { get; set; } = new( Color.FromArgb( 255, 80, 80, 80 ), 1.0F );

    /// <summary>
    /// 小節64分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure064Pen { get; set; } = new( Color.FromArgb( 255, 70, 70, 70 ), 0.1F );

    /// <summary>
    /// 小節32分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure032Pen { get; set; } = new( Color.FromArgb( 255, 60, 60, 60 ), 0.0F );

    /// <summary>
    /// 小節16分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure016Pen { get; set; } = new( Color.FromArgb( 255, 50, 50, 50 ), 0.0F );

    /// <summary>
    /// 小節8分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure008Pen { get; set; } = new( Color.FromArgb( 255, 40, 40, 40 ), 0.0F );

    /// <summary>
    /// 小節4分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure004Pen { get; set; } = new( Color.FromArgb( 255, 30, 30, 30 ), 0.0F );

    /// <summary>
    /// 小節1分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public Pen SheetMeasure001Pen { get; set; } = new( Color.FromArgb( 255, 20, 20, 20 ), 0.0F );

    #endregion

    /// <summary>
    /// スコア最大横幅
    /// </summary>
	public int ScoreMaxWidth => NoteTermWidthSize * ( HeaderGroupOn ? DMS.SCORE.EditMidiMapSet.DisplayGroupCount : DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount ) ;

    /// <summary>
    /// スコア最大高さ
    /// </summary>
    public int ScoreMaxHeight => NoteTermHeightSize * Config.System.NoteCount;

    /// <summary>
    /// １小節の横幅
    /// </summary>
    public int MeasureSize => NoteTermHeightSize * Config.System.MeasureNoteNumber;
}
