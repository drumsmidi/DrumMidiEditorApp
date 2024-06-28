using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DrumMidiEditorApp.pAudio;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pUtil;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface.pSequence;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class PlayerSurface : PlayerSurfaceBase
{
    #region Member

    /// <summary>
    /// プレイヤー設定
    /// </summary>
    private ConfigPlayerSequence DrawSet => Config.Player.Sequence;

    /// <summary>
    /// BPM行ヘッダ範囲
    /// </summary>
	private Rect _BpmHeadRange = new();

    /// <summary>
    /// BPM行ボディ範囲
    /// </summary>
	private Rect _BpmBodyRange = new();

    /// <summary>
    /// 小節番号行ヘッダ範囲
    /// </summary>
    private Rect _MeasureNoHeadRange = new();

    /// <summary>
    /// 小節番号行ボディ範囲
    /// </summary>
	private Rect _MeasureNoBodyRange = new();

    /// <summary>
    /// ノート行ヘッダ範囲
    /// </summary>
    private Rect _ScoreHeadRange = new();

    /// <summary>
    /// ノート行ボディ範囲
    /// </summary>
    private Rect _ScoreBodyRange = new();

    /// <summary>
    /// 小節分割線リスト
    /// </summary>
    private readonly List<DmsItemLine> _MeasureLineList = [];

    /// <summary>
    /// MidiMapGroupヘッダリスト（MidiMapGroupキー、MidiMapGroup描画アイテム）
    /// </summary>
    private readonly Dictionary<int,DmsItemMidiMap> _HeaderGroupList = [];

    /// <summary>
    /// MidiMapヘッダリスト（MidiMapキー、MidiMap描画アイテム）
    /// </summary>
    private readonly Dictionary<int, DmsItemMidiMap> _HeaderMidiMapList = [];

    /// <summary>
    /// BPMリスト（小節番号、BPM描画アイテム）
    /// </summary>
    private readonly Dictionary<int,List<DmsItemBpm>> _BpmList = [];

    /// <summary>
    /// NOTEリスト（小節番号、NOTE描画アイテム）
    /// </summary>
    private readonly Dictionary<int,List<DmsItemNote>> _NoteList = [];

    /// <summary>
    /// NOTE-OFFリスト（NOTE描画アイテム、小節番号開始、終了）
    /// </summary>
    private readonly Dictionary<DmsItemNote, Point> _NoteOffList = [];

    /// <summary>
    /// ノート背景色リスト＜MidiMapKey、背景色＞
    /// </summary>
    private readonly Dictionary<int, FormatRect> _MidiMapNoteFormatList = [];

    /// <summary>
    /// 小節番号
    /// </summary>
    private DmsItemMeasure? _MeasureNo = null;

    /// <summary>
    /// 現在のBPM
    /// </summary>
    private DmsItemLabel? _NowBpm = null;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PlayerSurface() : base() { }

    public override bool OnMove( double aFrameTime ) => base.OnMove( aFrameTime );

    protected override void UpdateScore()
    {
        base.UpdateScore();

        // screen
        if ( DrawSet.DrawDirectionModeSelect == ConfigPlayerSequence.DrawDirectionMode.Vertical )
        {
            var s = _ScreenSize;

            _ScreenSize.Height  = s.Width;
            _ScreenSize.Width   = s.Height;
        }

        // bpm header
        _BpmHeadRange.X         = 0;
        _BpmHeadRange.Y         = 0;
        _BpmHeadRange.Width     = DrawSet.HeaderTotalWidthSize;
        _BpmHeadRange.Height    = DrawSet.BpmHeightSize;

        // bpm body
        _BpmBodyRange.X         = _BpmHeadRange.Right;
        _BpmBodyRange.Y         = _BpmHeadRange.Top;
        _BpmBodyRange.Width     = _ScreenSize.Width - _BpmHeadRange.Right;
        _BpmBodyRange.Height    = _BpmHeadRange.Height;

        // measure no header
        _MeasureNoHeadRange.X       = 0;
        _MeasureNoHeadRange.Y       = _BpmBodyRange.Bottom;
        _MeasureNoHeadRange.Width   = DrawSet.HeaderTotalWidthSize;
        _MeasureNoHeadRange.Height  = DrawSet.MeasureNoHeightSize;

        // measure no body
        _MeasureNoBodyRange.X       = _MeasureNoHeadRange.Right;
        _MeasureNoBodyRange.Y       = _MeasureNoHeadRange.Top;
        _MeasureNoBodyRange.Width   = _ScreenSize.Width - _MeasureNoHeadRange.Right;
        _MeasureNoBodyRange.Height  = _MeasureNoHeadRange.Height;

        // score header
        _ScoreHeadRange.X       = 0;
        _ScoreHeadRange.Y       = _MeasureNoBodyRange.Bottom;
        _ScoreHeadRange.Width   = DrawSet.HeaderTotalWidthSize;
        _ScoreHeadRange.Height  = DrawSet.ScoreMaxHeight;

        // score body
        _ScoreBodyRange.X       = _ScoreHeadRange.Right;
        _ScoreBodyRange.Y       = _ScoreHeadRange.Top;
        _ScoreBodyRange.Width   = _ScreenSize.Width - _ScoreHeadRange.Right;
        _ScoreBodyRange.Height  = _ScoreHeadRange.Height;
    }

    protected override void UpdateScoreLine()
    {
        var body            = _ScoreBodyRange;
        var measure_size    = DrawSet.MeasureSize;

        #region Measure line
        {
            var pens = new FormatLine[]
                {
                    DrawSet.SheetMeasure001Line,
                    DrawSet.SheetMeasure004Line,
                    DrawSet.SheetMeasure008Line,
                    DrawSet.SheetMeasure016Line,
                    DrawSet.SheetMeasure032Line,
                    DrawSet.SheetMeasure064Line,
                    DrawSet.SheetMeasure128Line,
                };

            _MeasureLineList.Clear();

            var linesize =  pens[ 0 ].LineSize == 0 ? 0 : 1 ;

            if ( linesize == 0 )
            {
                for ( var i = 1; i < pens.Length; i++ )
                {
                    if ( pens [ i ].LineSize != 0 )
                    {
                        linesize = (int)Math.Pow( 2, i + 1 );
                        break;
                    }
                }
            }

            if ( linesize != 0 )
            {
                FormatLine? pen = null;

                var note_num   = ConfigSystem.MeasureNoteNumber;
                var note_width = DrawSet.NoteTermWidthSize;

                for ( var i = 0; i < note_num; i += linesize )
                {
                    for ( int x = 6, y = 1; x >= 0; x--, y *= 2 )
                    {
                        pen = pens [ x ];

                        if ( i % ( note_num / y ) == 0 && ( pen.LineSize != 0 ) )
                        {
                            break;
                        }
                    }

                    if ( pen == null )
                    {
                        continue;
                    }

                    _MeasureLineList.Add
                        (
                            new
                            (
                                (float)( body.X + ( note_width * i ) - ( pen.LineSize / 2F ) ),
                                body._y,
                                0,
                                body._height,
                                pen
                            )
                        );

                    pen = null;
                }
            }
        }
        #endregion

        #region MeasureNo
        {
            _MeasureNo = new
                (
                    _MeasureNoBodyRange._x,
                    _MeasureNoBodyRange._y,
                    measure_size,
                    _MeasureNoBodyRange._height,
                    DrawSet.MeasureNoRect
                );
        }
        #endregion
    }

    protected override void UpdateScoreHeader()
    {
        _HeaderGroupList.Clear();
        _HeaderMidiMapList.Clear();

        var head = _ScoreHeadRange;

        var x = head.X;
        var y = head.Y;
        var w = DrawSet.HeaderGroupOn ? head.Width : DrawSet.HeaderGroupWidthSize ;
        var h = DrawSet.HeaderGroupOn ? DrawSet.NoteTermHeightSize : 0 ;

        #region MidiMapGroup
        {
            var index = 0;

            foreach ( var group in Score.EditMidiMapSet.DisplayMidiMapGroups )
            {
                if ( !DrawSet.HeaderGroupOn )
                {
                    h = DrawSet.NoteTermHeightSize * Score.EditMidiMapSet.DisplayMidiMapCountByGroup [ index ];

                    index++;
                }

                if ( group != null )
                {
                    var obj = new DmsItemMidiMap
                        (
                            group,
                            (float)x,
                            (float)y,
                            (float)w,
                            (float)h,
                            DrawSet.HeaderRect
                        );

                    _HeaderGroupList.Add( group.GroupKey, obj );
                }

                y += h;
            }
        }
        #endregion

        #region MidiMap
        {
            x = head.X + w;
            y = head.Y;
            w = DrawSet.HeaderWidthSize;
            h = DrawSet.NoteTermHeightSize;

            foreach ( var midiMap in Score.EditMidiMapSet.DisplayMidiMaps )
            {
                if ( midiMap != null )
                {
                    // ノート描画用の書式を登録
                    if ( !_MidiMapNoteFormatList.TryGetValue( midiMap.MidiMapKey, out var value ) )
                    {
                        // TODO: 線の色とか情報追加が必要
                        var formatRect = new FormatRect
                        {
                            Background = new( midiMap.Color )
                        };

                        _MidiMapNoteFormatList.Add( midiMap.MidiMapKey, formatRect );
                    }
                    else
                    {
                        value.Background = new( midiMap.Color );
                    }

                    if ( !DrawSet.HeaderGroupOn )
                    {
                        // アイテム登録
                        var obj = new DmsItemMidiMap
                            (
                                midiMap,
                                (float)x,
                                (float)y,
                                (float)w,
                                (float)h,
                                DrawSet.HeaderRect
                            );

                        _HeaderMidiMapList.Add( midiMap.MidiMapKey, obj );
                    }
                }

                y += h;
            }
        }
        #endregion

        #region Bpm now
        {
            _NowBpm = new DmsItemLabel
                (
                    _BpmHeadRange._x,
                    _BpmHeadRange._y,
                    _BpmHeadRange._width,
                    _BpmHeadRange._height,
                    string.Empty,
                    DrawSet.BpmNowRect
                );
        }
        #endregion
    }

    protected override void ClearMeasure()
    {
        foreach ( var nList in _NoteList )
        {
            nList.Value.Clear();
        }
        _NoteList.Clear();
        _NoteOffList.Clear();

        foreach ( var nList in _BpmList )
        {
            nList.Value.Clear();
        }
        _BpmList.Clear();
    }

    protected override void UpdateScoreMeasure( int aMeasureNo )
    {
        #region Clear note
        {
            if ( _NoteList.TryGetValue( aMeasureNo, out var nList ) )
            {
                foreach ( var item in nList )
                {
                    _ = _NoteOffList.Remove( item );
                }

                nList.Clear();
                _ = _NoteList.Remove( aMeasureNo );
            }
        }
        #endregion

        var measure = Score.EditChannel.GetMeasure( aMeasureNo );

        if ( measure == null )
        {
            return;
        }

        var note_rect = new Rect( 0, 0, DrawSet.NoteWidthSize, DrawSet.NoteHeightSize );

        #region Set note
        {
            foreach ( var midiMap in Score.EditMidiMapSet.DisplayMidiMaps )
            {
                if ( midiMap.Group == null )
                {
                    continue;
                }

                if ( !measure.NoteLines.TryGetValue( midiMap.MidiMapKey, out var measure_line ) )
                {
                    continue;
                }

                var idx = DrawSet.HeaderGroupOn ? Score.EditMidiMapSet.GetDisplayMidiMapGroupIndex( midiMap.Group.GroupKey )
                                                : Score.EditMidiMapSet.GetDisplayMidiMapIndex( midiMap.MidiMapKey ) ;

                foreach ( var info in measure_line.InfoStates.Values )
                {
                    if ( !info.NoteOn )
                    {
                        continue;
                    }

                    var volume = 1F;
                    if ( DrawSet.NoteVolumeSizeOn )
                    {
                        volume = ( info.Volume + midiMap.VolumeAddIncludeGroup ) / (float)MidiNet.MidiMaxVolume;

                        if ( volume > 1F )
                        {
                            volume = 1F;
                        }
                        else if ( volume < 0F )
                        {
                            volume = 0F;
                        }
                    }
                    if ( DrawSet.NoteVolumeZeroOn && volume <= 0F )
                    {
                        volume = 1F;
                    }

                    note_rect.Width = DrawSet.NoteWidthSize * volume;
                    note_rect.Height = DrawSet.NoteHeightSize * volume;

                    if ( volume != 0F )
                    {
                        if ( note_rect.Width <= 0F )
                        {
                            note_rect.Width = 0.1F;
                        }
                        if ( note_rect.Height <= 0F )
                        {
                            note_rect.Height = 0.1F;
                        }
                    }

                    var distanceToNextNoteOff = Score.EditChannel.GetNotePosDistanceToNextNoteOff( info );

                    var distanceToNextNoteOffWidth = distanceToNextNoteOff * DrawSet.NoteTermWidthSize;

                    note_rect.X = ( info.NotePos * DrawSet.NoteTermWidthSize ) - ( note_rect.Width / 2.0F );

                    note_rect.Width += distanceToNextNoteOffWidth;

                    var obj = new DmsItemNote
                        (
                            note_rect._x,
                            note_rect._width,
                            note_rect._height,
                            _MidiMapNoteFormatList[ midiMap.MidiMapKey ],
                            DrawSet.HeaderGroupOn ? _HeaderGroupList[ midiMap.Group.GroupKey ]
                                                  : _HeaderMidiMapList[ midiMap.MidiMapKey ]
                        );

                    // NoteOn描画補助
                    if ( !_NoteList.TryGetValue( aMeasureNo, out var lst ) )
                    {
                        lst = [];
                    }
                    lst.Add( obj );

                    _NoteList [ aMeasureNo ] = lst;

                    // NoteOff描画補助
                    var measureMax = ( info.NotePos + distanceToNextNoteOff ) / ConfigSystem.MeasureNoteNumber;

                    if ( aMeasureNo < measureMax )
                    {
                        _NoteOffList [ obj ] = new( aMeasureNo, measureMax );
                    }
                }
            }
        }
        #endregion

        #region Sort note
        {
            if ( _NoteList.TryGetValue( aMeasureNo, out var lst ) )
            {
                lst.Sort();
            }
        }
        #endregion
    }

    protected override void UpdateBpmMeasure( int aMeasureNo )
    {
        #region Clear
        {
            if ( _BpmList.TryGetValue( aMeasureNo, out var nList ) )
            {
                nList.Clear();
                _ = _BpmList.Remove( aMeasureNo );
            }
        }
        #endregion

        var body      = _BpmBodyRange;
        var note_rect = new Rect( body.X, body.Y, DrawSet.BpmWidthSize, DrawSet.BpmHeightSize );

        #region Set bpm
        {
            var measure = Score.SysChannel.GetMeasure( aMeasureNo );

            if ( measure == null )
            {
                return;
            }

            var bpm_line = measure.BpmLine;

            if ( bpm_line == null )
            {
                return;
            }

            foreach ( var info in bpm_line.InfoStates.Values )
            {
                note_rect.X = body.X + ( info.NotePos * DrawSet.NoteTermWidthSize );

                var obj = new DmsItemBpm
                    (
                        note_rect._x,
                        note_rect._y,
                        note_rect._width,
                        note_rect._height,
                        info,
                        DrawSet.BpmRect
                    );

                if ( !_BpmList.TryGetValue( aMeasureNo, out var lst ) )
                {
                    lst = [];
                }
                lst.Add( obj );

                _BpmList [ aMeasureNo ] = lst;
            }
        }
        #endregion

        #region Sort
        {
            if ( _BpmList.TryGetValue( aMeasureNo, out var lst ) )
            {
                lst.Sort();
            }
        }
        #endregion
    }

    public override bool OnDraw( CanvasDrawEventArgs args )
    {
        if ( !base.OnDraw( args ) )
        {
            return false;
        }

        // 描画回転
        if ( DrawSet.DrawDirectionModeSelect == ConfigPlayerSequence.DrawDirectionMode.Vertical )
        {
            args.DrawingSession.Transform =
                Matrix3x2.CreateTranslation( -_ScreenSize._height, 0 ) *
                Matrix3x2.CreateRotation( (float)( Math.PI * -90 / 180.0 ) ) *
                Matrix3x2.CreateTranslation( 0, _ScreenSize._width - _ScreenSize._height );
        }

        var head            = _ScoreHeadRange;
        var body            = _ScoreBodyRange;
        var note_pos        = _NotePositionX;
        var sheet_pos_x     = (float)Math.Round( _SheetPosX * DrawSet.NoteTermWidthSize, 0 );
        var measure_size    = DrawSet.MeasureSize;
        var measure_start   = (int)( ( note_pos - (head.Width / DrawSet.NoteTermWidthSize) ) / ConfigSystem.MeasureNoteNumber ) - 1;
        var measure_end     = (int)( ( note_pos + (body.Width / DrawSet.NoteTermWidthSize) ) / ConfigSystem.MeasureNoteNumber ) + 1;

        if ( measure_start < 0 )
        {
            measure_start = 0;
        }
        if ( measure_end > ConfigSystem.MeasureMaxNumber )
        {
            measure_end = ConfigSystem.MeasureMaxNumber;
        }

        #region Paint measure line
        {
            int     cnt;
            float   diff_x;

            for ( var measure_no = measure_start; measure_no <= measure_end; measure_no++ )
            {
                diff_x = ( measure_size * measure_no ) - sheet_pos_x;
                cnt = _MeasureLineList.Count;

                for ( var index = 0; index < cnt; index++ )
                {
                    _MeasureLineList [ index ].Draw( args.DrawingSession, diff_x, 0 );
                }
            }
        }
        #endregion

        #region Paint note
        {
            float diff_x;

            for ( var measure_no = measure_start; measure_no <= measure_end; measure_no++ )
            {
                if ( !_NoteList.TryGetValue( measure_no, out var notes ) )
                {
                    continue;
                }

                diff_x = ( measure_size * measure_no ) - sheet_pos_x;

                foreach ( var note in notes )
                {
                    note.Draw( args.DrawingSession, diff_x, 0 );
                }
            }

            // 描画範囲外のノート描画
            foreach ( var item in _NoteOffList.Where( c => c.Value.X < measure_start && measure_start <= c.Value.Y ) )
            {
                diff_x = (float)( ( measure_size * item.Value.X ) - sheet_pos_x );

                item.Key.Draw( args.DrawingSession, diff_x, 0 );
            }
        }
        #endregion

        #region Paint header of MidiMapGroup / MidiMap
        {
            foreach ( var obj in _HeaderGroupList.Values )
            {
                obj.Draw( args.DrawingSession, 0, 0 );
            }

            foreach ( var obj in _HeaderMidiMapList.Values )
            {
                obj.Draw( args.DrawingSession, 0, 0 );
            }
        }
        #endregion

        #region Paint bpm
        {
            #region Paint body
            {
                float diff_x;

                for ( var measure_no = measure_start; measure_no <= measure_end; measure_no++ )
                {
                    if ( !_BpmList.TryGetValue( measure_no, out var bpms ) )
                    {
                        continue;
                    }

                    diff_x = ( measure_size * measure_no ) - sheet_pos_x;

                    foreach ( var bpm in bpms )
                    {
                        bpm.Draw( args.DrawingSession, diff_x, 0 );
                    }
                }
            }
            #endregion

            #region Paint bpm now
            {
                if ( DrawSet.BpmNowDisplay && _NowBpm != null )
                {
                    _NowBpm.Text = string.Format( "[Bpm:{0, 6:##0.00}]", DmsControl.GetBpm( _NotePositionX ) );
                    _NowBpm.Draw( args.DrawingSession );
                }
            }
            #endregion
        }
        #endregion

        #region Paint measure number
        {
            #region Paint body
            {
                float diff_x;

                for ( var measure_no = measure_start; measure_no <= measure_end; measure_no++ )
                {
                    diff_x = ( measure_size * measure_no ) - sheet_pos_x;

                    _MeasureNo?.Draw( args.DrawingSession, measure_no, diff_x, 0 );
                }
            }
            #endregion
        }
        #endregion

        return true;
    }
}
