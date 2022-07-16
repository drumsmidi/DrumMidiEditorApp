using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using DrumMidiEditor.pAudio;
using DrumMidiEditor.pConfig;
using DrumMidiEditor.pControl;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pAudio;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pGeneralFunction.pUtil;
using DrumMidiEditor.pResume;

using Timer = System.Windows.Forms.Timer;

namespace DrumMidiEditor.pView.pEditer.pEdit;

/// <summary>
/// EditerPanel
/// </summary>
public class EditerPanel : Panel
{
    #region Member

    /// <summary>
    /// アクション状態一覧
    /// </summary>
    private enum EActionState
    {
        None = 0,
        EditVolume,
        AddBpm,
        RemoveBpm,
        AddNote,
        RemoveNote,
        EditSupportLine,
        EditSelectDrum,
        SelectNoteRange,
        MoveNoteRange,
        RemoveNoteRange,
    }

    /// <summary>
    /// アクション状態
    /// </summary>
    private EActionState _ActionState = EActionState.None;

    /// <summary>
    /// 編集履歴管理
    /// </summary>
    private readonly ResumeManage _EditResumeMng = new();

    /// <summary>
    /// パネルスクリーンサイズ
    /// </summary>
    private Size _ScreenSize = new();

    /// <summary>
    /// 情報テキスト表示範囲
    /// </summary>
	private Rectangle _InfoRange = new();

    /// <summary>
    /// BPM行ヘッダ範囲
    /// </summary>
	private Rectangle _BpmHeadRange = new();

    /// <summary>
    /// BPM行ボディ範囲
    /// </summary>
	private Rectangle _BpmBodyRange = new();

    /// <summary>
    /// 小節番号行ヘッダ範囲
    /// </summary>
	private Rectangle _MeasureNoHeadRange = new();

    /// <summary>
    /// 小節番号行ボディ範囲
    /// </summary>
	private Rectangle _MeasureNoBodyRange = new();

    /// <summary>
    /// DrumGroup/Drum行ヘッダ範囲
    /// </summary>
    private Rectangle _ScoreHeadRange = new();

    /// <summary>
    /// DrumGroup/Drum行ボディ範囲
    /// </summary>
    private Rectangle _ScoreBodyRange = new();

    /// <summary>
    /// 音量行ヘッダ範囲
    /// </summary>
    private Rectangle _VolumeHeadRange = new();

    /// <summary>
    /// 音量行ボディ範囲
    /// </summary>
    private Rectangle _VolumeBodyRange = new();

    /// <summary>
    /// DrumGroup/Drum行の表示高さ。
    /// （Volume行が被さっている場合、その高さが差し引かれる）
    /// </summary>
    private int _ScoreViewHeight = 0;

    /// <summary>
    /// マウスダウン押下時のマウス位置
    /// </summary>
    private Point _MouseDownPosition = new();

    /// <summary>
    /// マウスダウン押下時のマウス位置にあるノート位置（絶対値）
    /// </summary>
    private Point _NoteDownPosition = new();

    /// <summary>
    /// マウス位置にあるノート位置（絶対値）
    /// </summary>
    private Point _NoteCursorPosition = new( -1 );

    /// <summary>
    /// ノートコピー範囲内で左端ノートのノート位置（絶対値）
    /// </summary>
	private int	_CopyNotePositionX = 0;

    /// <summary>
    /// コピーしたNOTE情報リスト
    /// </summary>
	private readonly List<InfoNote> _CopyNoteList = new();

    /// <summary>
    /// コピーしたBPM情報リスト
    /// </summary>
	private readonly List<InfoBpm> _CopyBpmList = new();

    /// <summary>
    /// 五線リスト
    /// </summary>
	private readonly List<DmsItemLine> _StaffLineList = new();

    /// <summary>
    /// 小節線リスト
    /// </summary>
    private readonly List<DmsItemLine> _MeasureLineList	= new();

    /// <summary>
    /// DrumGroup/Drumヘッダリスト
    /// </summary>
    private readonly List<DmsItemMidiMap> _HeaderList = new();

    /// <summary>
    /// BPMリスト＜小節番号、アイテム＞
    /// </summary>
    private readonly Dictionary<int,List<DmsItemBpm>> _BpmList = new();

    /// <summary>
    /// ノートリスト＜小節番号、アイテム＞
    /// </summary>
    private readonly Dictionary<int,List<DmsItemNote>> _NoteList = new();

    /// <summary>
    /// ノート予測リスト＜小節番号、アイテム＞
    /// </summary>
    private readonly Dictionary<int,List<DmsItemNote>> _NotePredictList = new();

    /// <summary>
    /// ノート音量リスト＜小節番号、アイテム＞
    /// </summary>
    private readonly Dictionary<int,List<DmsItemNoteVolume>> _VolumeList = new();

    /// <summary>
    /// ノート背景色リスト＜MidiMapKey、背景色＞
    /// </summary>
    private readonly Dictionary<int, SolidBrush> _MidiMapBrushList = new();

    /// <summary>
    /// サポート線
    /// </summary>
    private readonly DmsItemSupportLine _SupportLine = new();

    /// <summary>
    /// ノート範囲選択
    /// </summary>
    private DmsItemNoteRange _NoteRange = new();

    /// <summary>
    /// ノート範囲選択（前回値）
    /// </summary>
    private readonly DmsItemNoteRange _NoteRangeBef = new();

    /// <summary>
    /// 音量入力
    /// </summary>
    private readonly DmsItemVolumeRange _VolumeRange = new();

    /// <summary>
    /// 等間隔処理実行用タイマー
    /// </summary>
    private readonly Timer _Timer = new();

    /// <summary>
    /// 音階別周波数表示用BGM
    /// </summary>
    private NAudioData? _ScaleBgm = null;

    /// <summary>
    /// 音階別周波数表示用Bitmap
    /// </summary>
    private Bitmap? _ScaleBitmap = null;

    #endregion

    /// <summary>
    /// Editerタブ設定
    /// </summary>
    private static ConfigEditer DrawSet => Config.Editer;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EditerPanel()
    {
        SetStyle( ControlStyles.ResizeRedraw,           true	);
        SetStyle( ControlStyles.OptimizedDoubleBuffer,  true	);
        SetStyle( ControlStyles.UserPaint,              true	);
        SetStyle( ControlStyles.AllPaintingInWmPaint,   true	);
        SetStyle( ControlStyles.DoubleBuffer,           true	); 
        SetStyle( ControlStyles.Opaque,                 true	);

		SuspendLayout();
		{
			Margin  = new Padding( 0 );
			Padding = new Padding( 0 );
		}
		ResumeLayout( false );

        MouseDown	+= new( SheetMouseDown );
		MouseMove	+= new( SheetMouseMove );
		MouseUp	    += new( SheetMouseUp );

        _Timer.Interval     = 100;
        _Timer.Enabled      = true;
        _Timer.Tick        += new( SheetMouseTimer );
        _Timer.Start();
    }

    #region Mouse Event

    /// <summary>
    /// シートマウスタイマー処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void SheetMouseTimer( object? sender, EventArgs ev )
    {
        try
        {
            switch ( _ActionState )
            {
				case EActionState.None:
					{
						var pos = PointToClient( Cursor.Position );

						if ( CheckNoteRangeArea( pos ) )
						{
							if ( _NoteCursorPosition.X != -1 || _NoteCursorPosition.Y != -1 )
							{
								_NoteCursorPosition.X = -1;
								_NoteCursorPosition.Y = -1;
								Invalidate( true );
							}
						}
						else if ( CheckBpmBodyArea( pos ) || CheckMeasureNoBodyArea( pos ) || CheckScoreBodyArea( pos ) || CheckVolumeBodyArea( pos ) )
						{
							var note_pos = CalcNotePosition( pos );

							if ( _NoteCursorPosition != note_pos )
							{
								_NoteCursorPosition = note_pos;
								Invalidate( true );
							}
						}
					}
					break;
				case EActionState.SelectNoteRange:
					{
						var pos = PointToClient( Cursor.Position );

                        var note_pos = DrawSet.NotePosition;

                        if ( pos.Y < _ScoreBodyRange.Y )
						{
                            note_pos.Y -= ( _ScoreBodyRange.Y - pos.Y ) / DrawSet.NoteHeightSize;

							if ( note_pos.Y < 0 )
							{
                                note_pos.Y = 0;
							}
						}
						if ( pos.Y > Height )
						{
                            note_pos.Y += ( pos.Y - Height ) / DrawSet.NoteHeightSize;

							if ( note_pos.Y >= DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount  )
							{
                                note_pos.Y = DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount - 1;
							}
						}
						if ( pos.X < _ScoreBodyRange.X )
						{
                            note_pos.X -= ( _ScoreBodyRange.X - pos.X ) / DrawSet.NoteWidthSize;

							if ( note_pos.X < 0 )
							{
                                note_pos.X = 0;
							}
						}
						if ( pos.X > Width )
						{
                            note_pos.X += ( pos.X - Width ) / DrawSet.NoteWidthSize;

							if ( note_pos.X >= Config.System.NoteCount )
							{
                                note_pos.X = Config.System.NoteCount - 1;
							}
						}

                        DrawSet.NotePosition = note_pos;

                        EditNoteRange( pos, false );
                        Invalidate( true );
					}
					break;
				default:
					{ 
						if ( _NoteCursorPosition.X != -1 || _NoteCursorPosition.Y != -1 )
						{
							_NoteCursorPosition.X = -1;
							_NoteCursorPosition.Y = -1;
							Invalidate( true );
						}
					}
					break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// シートマウスダウン処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void SheetMouseDown( object? sender, MouseEventArgs ev )
	{
        if ( _ActionState != EActionState.None )
        {
            return;
        }

        try
        {
			switch ( ev.Button )
			{
				case MouseButtons.Left:
					{
                        if ( CheckVolumeBodyArea( ev.Location ) )
                        {
                            EditVolumeRange( ev.Location, true );

    			            _ActionState = EActionState.EditVolume;
                        }
                        else if ( CheckMeasureNoBodyArea( ev.Location ) )
                        {
                            EditSupportRange( ev.Location, true );

                            _ActionState = EActionState.EditSupportLine;
                        }
                        else if ( CheckScoreHeadArea( ev.Location ) )
                        {
    			            _ActionState = EActionState.EditSelectDrum;
                        }
                        else if ( CheckBpmBodyArea( ev.Location ) )
                        {
							_NoteRangeBef.Set( _NoteRange );

							if ( _NoteRange.Selected )
							{
								ClearSelectNoteRange();

								_ActionState = EActionState.None;
							}
							else
							{
                                _ActionState = EActionState.AddBpm;
                            }
                        }
                        else if ( CheckNoteRangeArea( ev.Location ) )
                        {
    						_NoteRangeBef.Set( _NoteRange );

							if ( ( Control.ModifierKeys & Keys.Control ) == Keys.Control )
							{
								CopyNoteRange();
							}
							else
							{
								EditMoveNoteRange( ev.Location, true );

								_ActionState = EActionState.MoveNoteRange;
							}
						}
                        else if ( CheckScoreBodyArea( ev.Location ) )
                        {
							_NoteRangeBef.Set( _NoteRange );

							if ( ( Control.ModifierKeys & Keys.Control ) == Keys.Control )
							{
                                PasteNoteRange( ev.Location );
                            }
                            else
							{
								if ( _NoteRange.Selected )
								{
									ClearSelectNoteRange();

									_ActionState = EActionState.None;
								}
								else
								{
									_MouseDownPosition = ev.Location;
									_NoteDownPosition  = CalcNotePosition( ev.Location );

									_ActionState = EActionState.AddNote;
								}
							}
                        }
                        else
                        {
                            return;
                        }
					}
					break;
				case MouseButtons.Right:
					{
                        if ( CheckVolumeBodyArea( ev.Location ) )
                        {
                            ClearVolumeRange();
                        }
                        else if ( CheckMeasureNoBodyArea( ev.Location ) )
                        {
                            ClearSupportRange();
                        }
                        else if ( CheckScoreHeadArea( ev.Location ) )
                        {
                        }
                        else if ( CheckBpmBodyArea( ev.Location ) )
                        {
							_NoteRangeBef.Set( _NoteRange );

                            if ( _NoteRange.Selected )
                            {
                                ClearSelectNoteRange();

                                _ActionState = EActionState.None;
                            }
                            else
                            {
                                _ActionState = EActionState.RemoveBpm;
                            }
                        }
                        else if ( CheckNoteRangeArea( ev.Location ) )
                        {
    						_NoteRangeBef.Set( _NoteRange );

                            _ActionState = EActionState.RemoveNoteRange;
                        }
                        else if ( CheckScoreBodyArea( ev.Location ) )
                        {
							_NoteRangeBef.Set( _NoteRange );

                            if ( _NoteRange.Selected )
                            {
                                ClearSelectNoteRange();

                                _ActionState = EActionState.None;
                            }
                            else
                            {
                                _MouseDownPosition = ev.Location;
                                _NoteDownPosition  = CalcNotePosition( ev.Location );

                                _ActionState = EActionState.RemoveNote;
                            }
                        }
                        else
                        {
    			            return;
                        }
					}
					break;
                default:
                    return;
			}

            Invalidate( true );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            _ActionState = EActionState.None;
        }
	}

    /// <summary>
    /// シートマウス移動処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
	private void SheetMouseMove( object? sender, MouseEventArgs ev )
	{
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
			switch ( ev.Button )
			{
				case MouseButtons.Left:
					{
                    	var pos = ev.Location;

						switch ( _ActionState )
						{
                            case EActionState.EditVolume:
                                {
                                    EditVolumeRange( pos, false );
                                }
                                break;
                            case EActionState.EditSupportLine:
                                {
                                    EditSupportRange( pos, false );
                                }
                                break;
                            case EActionState.AddBpm:
                                return;
                            case EActionState.AddNote:
                                {
                                    var note_pos = CalcNotePosition( pos );

                                    if ( note_pos == _NoteDownPosition )
                                    {
                                        _ActionState = EActionState.AddNote;
                                    }
                                    else
                                    { 
                                        EditNoteRange( _MouseDownPosition, true );
                                        EditNoteRange( pos, false );

                                        _ActionState = EActionState.SelectNoteRange;
                                    }
                                }
                                break;
                            case EActionState.SelectNoteRange:
                                {
                                    EditNoteRange( pos, false );
                                }
                                break;
                            case EActionState.MoveNoteRange:
                                {
                                    EditMoveNoteRange( pos, false );
                                }
                                break;
                            default:
                                return;
    					}
					}
					break;
				default:
					return;
			}

			Invalidate( true );
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            _ActionState = EActionState.None;
        }
	}

    /// <summary>
    /// シートマウスアップ処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
	private void SheetMouseUp( object? sender, MouseEventArgs ev )
	{
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
			switch ( ev.Button )
			{
				case MouseButtons.Left:
					{
                        switch ( _ActionState )
                        {
                            case EActionState.EditVolume:
                                {
                                    EditNoteVolume( ev.Location );
                                }
                                break;
                            case EActionState.EditSupportLine:
                                {
                                    EditSupportRange( ev.Location, false );
                                }
                                break;
                            case EActionState.AddBpm:
                                {
                                    EditBpm( ev.Location, true );
                                }
                                break;
                            case EActionState.AddNote:
                                {
                                    EditNote( ev.Location, true );
                                }
                                break;
                            case EActionState.EditSelectDrum:
                                {
                                    EditSelectMidiMap( ev.Location );
                                }
                                break;
                            case EActionState.SelectNoteRange:
                                {
                                    EditSelectNoteRange( ev.Location );
                                }
                                break;
                            case EActionState.MoveNoteRange:
                                {
                                    EditMoveNoteRange( ev.Location );
                                }
                                break;
                            default:
                                return;
                        }
                    }
                    break;
				case MouseButtons.Right:
					{
                        switch ( _ActionState )
                        {
                            case EActionState.RemoveBpm:
                                {
                                    EditBpm( ev.Location, false );
                                }
                                break;
                            case EActionState.RemoveNote:
                                {
                                    EditNote( ev.Location, false );
                                }
                                break;
                            case EActionState.RemoveNoteRange:
                                {
									RemoveNoteRange();
                                }
                                break;
                            default:
                                return;
                        }
					}
					break;
			}

            Invalidate( true );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
        finally
        {
            _ActionState = EActionState.None;
        }
    }

	#endregion

	#region Check

    /// <summary>
    /// 小節番号行ボディ範囲チェック
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>True:範囲内、False:範囲外</returns>
	private bool CheckMeasureNoBodyArea( Point aMousePos )
        => !CheckVolumeBodyArea( aMousePos ) 
        && FormUtil.CheckRange( aMousePos, _MeasureNoBodyRange );

    /// <summary>
    /// MidiMapGroup/MidiMapヘッダ範囲チェック
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>True:範囲内、False:範囲外</returns>
    private bool CheckScoreHeadArea( Point aMousePos )
        => !CheckVolumeBodyArea( aMousePos ) 
        && FormUtil.CheckRange( aMousePos, _ScoreHeadRange );

    /// <summary>
    /// MidiMapGroup/MidiMapボディ範囲チェック
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>True:範囲内、False:範囲外</returns>
    private bool CheckScoreBodyArea( Point aMousePos )
        => !CheckVolumeBodyArea( aMousePos ) 
        && FormUtil.CheckRange( aMousePos, _ScoreBodyRange );

    /// <summary>
    /// BPM行ボディ範囲チェック
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>True:範囲内、False:範囲外</returns>
    private bool CheckBpmBodyArea( Point aMousePos )
        => !CheckVolumeBodyArea( aMousePos ) 
        && FormUtil.CheckRange( aMousePos, _BpmBodyRange );

    /// <summary>
    /// 音量行ボディ範囲チェック
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>True:範囲内、False:範囲外</returns>
    private bool CheckVolumeBodyArea( Point aMousePos )
        => DrawSet.VolumeDisplay 
        && FormUtil.CheckRange( aMousePos, _VolumeBodyRange );

    /// <summary>
    /// 範囲選択内チェック
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>True:範囲内、False:範囲外</returns>
    private bool CheckNoteRangeArea( Point aMousePos )
        => !CheckVolumeBodyArea( aMousePos ) 
        && FormUtil.CheckRange( aMousePos, _NoteRange.GetSelectRange( DrawSet.NotePosition ) );

    #endregion

    #region Calc

    /// <summary>
    /// MidiMap位置取得
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>X=-1:未取得、0:MidiMapGroup、1:MidiMap、Y=-1:未取得、0-n=MidiMapGroup/MidiMap表示連番</returns>
    private Point CalcMidiMapPosition( Point aMousePos )
    {
		var	head		= _ScoreHeadRange;
		var	note_pos    = DrawSet.NotePosition;

        var pos = new Point()
        {
            X = ( aMousePos.X - head.X ) / ( DrawSet.HeaderWidthSize / 2 ),
            Y = note_pos.Y + ( aMousePos.Y - head.Y ) / DrawSet.NoteHeightSize
        };

        if ( pos.X < 0 || 2 <= pos.X )
        {
            pos.X = -1;
        }

        if ( pos.Y < 0 || DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount <= pos.Y )
		{
			pos.Y = -1;
		}

        return pos;
	}

    /// <summary>
    /// １小節分割数を考慮したノート位置（絶対値）を取得
    /// </summary>
    /// <param name="aAbsolutePosX">ノート位置（絶対値）</param>
    /// <returns>計算後ノート位置（絶対値）</returns>
    private static int CalcNotePosition( int aAbsolutePosX )
    {
        var sa = aAbsolutePosX % DrawSet.DivisionLineCount;

        return aAbsolutePosX - sa + ( ( sa <= DrawSet.DivisionLineCount / 2 ) ? 0 : DrawSet.DivisionLineCount );
    }

    /// <summary>
    /// ノート位置（絶対値）取得
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>X=-1:未取得、0-n:ノート位置（絶対値）、Y=-1:未取得、0-n=MidiMapGroup/MidiMap表示連番</returns>
    private Point CalcNotePosition( Point aMousePos )
    {
		var	body	    = _ScoreBodyRange;
		var note_pos    = DrawSet.NotePosition;

        var pos = new Point()
        { 
			X = note_pos.X + ( aMousePos.X - body.X + DrawSet.NoteWidthSize / 2 ) / DrawSet.NoteWidthSize,
			Y = note_pos.Y + ( aMousePos.Y - body.Y ) / DrawSet.NoteHeightSize,
        };

        pos.X = CalcNotePosition( pos.X );

        if ( pos.X < 0 || Config.System.NoteCount <= pos.X )
        {
            pos.X = -1;
        }

        if ( pos.Y < 0 || DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount <= pos.Y )
		{
			pos.Y = -1;
		}

		return pos;
	}

    /// <summary>
    /// BPM位置（絶対値）取得
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>X=-1:未取得、0-n:NOTE位置（絶対値）、Y=0</returns>
    private Point CalcBpmPosition( Point aMousePos )
    {
		var	note_pos    = DrawSet.NotePosition;
		var	body		= _BpmBodyRange;

        var pos = new Point()
        {
            X = note_pos.X + ( aMousePos.X - body.X + DrawSet.NoteWidthSize / 2 ) / DrawSet.NoteWidthSize,
            Y = 0
        };

        pos.X = CalcNotePosition( pos.X );

        if ( pos.X < 0 || Config.System.NoteCount <= pos.X )
        {
            pos.X = -1;
        }

		return pos;
	}

    /// <summary>
    /// 小節番号位置（絶対値）取得
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>X=-1:未取得、0-n:NOTE位置（絶対値）、Y=0</returns>
    private Point CalcMeasureNoPosition( Point aMousePos )
    {
        var note_pos    = DrawSet.NotePosition;
        var body        = _MeasureNoBodyRange;

        var pos = new Point()
        {
            X = note_pos.X + ( aMousePos.X - body.X + DrawSet.NoteWidthSize / 2 ) / DrawSet.NoteWidthSize,
            Y = 0
        };

        pos.X = CalcNotePosition( pos.X );

        if ( pos.X < 0 || Config.System.NoteCount <= pos.X )
        {
            pos.X = -1;
        }

        return pos;
    }

    /// <summary>
    /// ノート音量位置（絶対値）取得
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>X=-1:未取得、0-n:ノート位置（絶対値）、Y=音量(127基準)</returns>
    private Point CalcNoteVolumePosition( Point aMousePos )
    {
		var	note_pos    = DrawSet.NotePosition;
		var	body		= _VolumeBodyRange;

        var pos = new Point()
        {
            X = note_pos.X + ( aMousePos.X - body.X + DrawSet.NoteWidthSize / 2 ) / DrawSet.NoteWidthSize,
            Y = body.Bottom - aMousePos.Y
        };

        if ( pos.X < 0 || Config.System.NoteCount <= pos.X )
        {
            pos.X = -1;
        }

        pos.Y = Config.Media.CheckMidiVolume( pos.Y );

		return pos;
	}

    #endregion

    #region Edit

    /// <summary>
    /// BPM編集
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <param name="aAddFlag">True:追加/変更、False:削除</param>
    private void EditBpm( Point aMousePos, bool aAddFlag )
    {
        var pos = CalcBpmPosition( aMousePos );

        if ( pos.X == -1 || pos.Y == -1 )
        {
            return;
        }

        int measure_no  = pos.X / Config.System.MeasureNoteNumber;
        int note        = pos.X % Config.System.MeasureNoteNumber;

        var info_old = DMS.SCORE.SysChannel.GetBpm( measure_no, note );

        var rs = new ResumeMultiple();

        #region Input bpm
        if ( aAddFlag )
        {
            InfoBpm info_new;

            if ( info_old == null )
            {
                info_new = new( measure_no, note );
            }
            else
            {
                info_new = info_old.Clone();
            }

            try
            { 
                DMS.PlayerForm?.TemporaryHide();

                using var fm = new BpmInputForm();
                if ( fm.ShowDialog( info_new.Bpm ) != DialogResult.Yes )
                {
                    return;
                }

                info_new.Bpm = fm.Bpm;
            }
            finally
            {
                DMS.PlayerForm?.TemporaryShow();
            }

            rs.AddBpm( info_old, info_new );
        }
        #endregion
        #region Remove bpm
        else
        {
            if ( info_old == null )
            {
                return;
            }

            rs.RemoveBpm( info_old );
        }
        #endregion

        _EditResumeMng.ExcuteAndResume( rs );

        UpdateBpmMeasure( measure_no );
    }

    /// <summary>
    /// サポート線クリア
    /// </summary>
    private void ClearSupportRange() => _SupportLine.ClearPos();

    /// <summary>
    /// サポート線の編集
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <param name="aStart">True:入力開始、False:入力途中/終了</param>
    private void EditSupportRange( Point aMousePos, bool aStart )
    {
        var pos = CalcMeasureNoPosition( aMousePos );

        if ( pos.X == -1 || pos.Y == -1 )
        {
            return;
        }

        if ( aStart )
        {
            _SupportLine.SetStartPos( pos.X );
        }
        else
        {
            _SupportLine.SetEndPos( pos.X );
        }
    }

    /// <summary>
    /// MidiMapGroup/MidiMapヘッダの選択状態編集
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    private void EditSelectMidiMap( Point aMousePos )
    {
        if ( !CheckScoreHeadArea( aMousePos ) )
        {
            return;
        }

        var pos = CalcMidiMapPosition( aMousePos );

        if ( pos.X == -1 || pos.Y == -1 )
        {
            return;
        }

        var midiMap = DMS.SCORE.EditMidiMapSet.DisplayMidiMaps[ pos.Y ];

        if ( midiMap == null || midiMap.Group == null )
        {
            return;
        }

        var rs = new ResumeMultiple();

        switch ( pos.X )
        {
            case 0: rs.SelectMidiMapGroup( midiMap.Group );   break;
            case 1: rs.SelectMidiMap( midiMap );              break;
            default: return;
        }

        _EditResumeMng.ExcuteAndResume( rs );

        UpdateNoteVolumeMeasure();
    }

    /// <summary>
    /// NOTE編集
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <param name="aAddFlag">True:追加/変更、False:削除</param>
	private void EditNote( Point aMousePos, bool aAddFlag )
	{
        if ( !CheckScoreBodyArea( aMousePos ) )
        {
            return;
        }

		var pos = CalcNotePosition( aMousePos );

        if ( pos.X == -1 || pos.Y == -1 )
        {
            return;
        }

        var midiMap = DMS.SCORE.EditMidiMapSet.DisplayMidiMaps[ pos.Y ];

        if ( midiMap == null )
        {
            return;
        }

        var rs = new ResumeMultiple();

		int measure_no = pos.X / Config.System.MeasureNoteNumber;
		int note       = pos.X % Config.System.MeasureNoteNumber;

        var info_old = DMS.SCORE.EditChannel.GetNote( midiMap.MidiMapKey, measure_no, note );

        #region Add note
        if ( aAddFlag )
        {
            InfoNote info_new;

            if ( info_old == null )
            {
                info_new = new( DMS.SCORE.EditChannelNo, midiMap.MidiMapKey, measure_no, note, DrawSet.NoteOn, !DrawSet.NoteOn );
            }
            else
            {
                if (    ( DrawSet.NoteOn && info_old.NoteOn == true && info_old.Volume == DrawSet.NoteSelectVolume )
                    ||  ( !DrawSet.NoteOn && info_old.NoteOff == true ) )
                {
                    return;
                }

                info_new = info_old.Clone();

                if ( DrawSet.NoteOn )
                {
                    info_new.NoteOn = true;
                }
                else
                {
                    info_new.NoteOff = true;
                }
            }

            if ( DrawSet.NoteOn )
            {
                info_new.Volume = DrawSet.NoteSelectVolume;
            }

            rs.AddNote( info_old, info_new );

            AudioFactory.SinglePlay( DMS.SCORE.EditChannelNo, midiMap.Midi, (byte)( midiMap.VolumeAddIncludeGroup + info_new.Volume ) );
        }
        #endregion
        #region Remove note
        else if ( info_old == null )
        {
            return;
        }
        else if ( DrawSet.NoteOn && info_old.NoteOff )
        {
            var info_new = info_old.Clone();
            info_new.NoteOn = false;

            rs.AddNote( info_old, info_new );
        }
        else if ( !DrawSet.NoteOn && info_old.NoteOn )
        {
            var info_new = info_old.Clone();
            info_new.NoteOff = false;

            rs.AddNote( info_old, info_new );
        }
        else
        { 
            rs.RemoveNote( info_old );
        }
        #endregion

        _EditResumeMng.ExcuteAndResume( rs );
    }

    /// <summary>
    /// 範囲選択解除
    /// </summary>
    private void ClearSelectNoteRange()
    {
		var rs = new ResumeMultiple();

		ClearSelectNoteRange( ref rs );

		if ( rs.Count > 0 )
        {
            _EditResumeMng.ExcuteAndResume( rs );
        }
	}

    /// <summary>
    /// 範囲選択解除。処理の実行は呼び出し元で実施が必要です。
    /// </summary>
    /// <param name="aResume">履歴</param>
	private void ClearSelectNoteRange( ref ResumeMultiple aResume )
    {
        DMS.EditerForm?.EditerCtl.DoClearRangeCheckBox( false );

        if ( !_NoteRange.Selected )
        {
            return;
        }

        #region NoteRange
        {
            _NoteRange.ClearPos();

			aResume.SelectNoteRange( ref _NoteRange, _NoteRangeBef );
        }
        #endregion

        #region Cancel current selected note
        {
            foreach ( var info in DMS.SCORE.EditChannel.NoteInfoList.Values )
            {
                if ( info.Selected )
                {
					aResume.SelectNote( info, false );
                }
            }
        }
        #endregion

        #region Cancel current selected bpm
        {
            foreach ( var info in DMS.SCORE.SysChannel.BpmInfoList.Values )
            {
                if ( info.Selected )
                {
					aResume.SelectBpm( info, false );
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 範囲選択の選択
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <param name="aStart">True:入力開始、False:入力途中/終了</param>
    private void EditNoteRange( Point aMousePos, bool aStart )
    {
        var pos = CalcNotePosition( aMousePos );

        if ( pos.X == -1 || pos.Y == -1 )
        {
            return;
        }

        if ( aStart )
        {
            _NoteRange.SetStartPos( pos.X, pos.Y, DrawSet.RangeSelect );
        }
        else
        {
            _NoteRange.SetEndPos( pos.X, pos.Y );
        }
    }

    /// <summary>
    /// 範囲選択内のNOTE/BPM選択処理
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    private void EditSelectNoteRange( Point aMousePos )
    {
        var rs = new ResumeMultiple();

        #region NoteRange
        {
            EditNoteRange( aMousePos, false );

            rs.SelectNoteRange( ref _NoteRange, _NoteRangeBef );
        }
        #endregion

        #region Cancel current selected note
        {
            foreach ( var info in DMS.SCORE.EditChannel.NoteInfoList.Values )
            {
                if ( info.Selected )
                {
                    rs.SelectNote( info, false );
                }
            }
        }
        #endregion

        #region Select notes within range
        {
            int sx = _NoteRange.StartNotePosX;
            int sy = _NoteRange.StartNotePosY;
            int ex = _NoteRange.EndNotePosX;
            int ey = _NoteRange.EndNotePosY;

            int measureNoStart  = sx / Config.System.MeasureNoteNumber;
            int measureNoEnd    = ex / Config.System.MeasureNoteNumber; // + ( ex % Config.System.MeasureNoteNumber == 0 ? 1 : 0 );

            int notePosStart;
            int notePosEnd;

            for ( int measure_no = measureNoStart; measure_no <= measureNoEnd; measure_no++ )
            {
                var measure = DMS.SCORE.EditChannel.GetMeasure( measure_no );

                if ( measure == null )
                {
                    continue;
                }

                notePosStart    = ( measure_no == measureNoStart ? sx % Config.System.MeasureNoteNumber : 0 );
                notePosEnd      = ( measure_no == measureNoEnd   ? ex % Config.System.MeasureNoteNumber : Config.System.MeasureNoteNumber - 1 );

                for ( int y = sy; y <= ey; y++ )
                {
				    var midiMap = DMS.SCORE.EditMidiMapSet.DisplayMidiMaps[ y ];
                        
                    if ( midiMap == null )
                    {
                        continue;
                    }

					if ( !measure.NoteLines.TryGetValue( midiMap.MidiMapKey, out var note_line ) )
					{
                        continue;
                    }

                    for ( int note_pos = notePosStart; note_pos <= notePosEnd; note_pos++ )
                    {
                        if ( !note_line.InfoStates.TryGetValue( note_pos, out var info ) )
                        {
                            continue;
                        }

                        rs.SelectNote( info, true );
                    }
                }
            }
        }
        #endregion

        #region Cancel current selected bpm
        {
            foreach ( var info in DMS.SCORE.SysChannel.BpmInfoList.Values )
            {
                if ( info.Selected )
                {
                    rs.SelectBpm( info, false );
                }
            }
        }
        #endregion

        if ( DrawSet.IncludeBpm )
        {
            #region Select bpms within range
            {
                int sx = _NoteRange.StartNotePosX;
                int ex = _NoteRange.EndNotePosX;

                int measureNoStart  = sx / Config.System.MeasureNoteNumber;
                int measureNoEnd    = ex / Config.System.MeasureNoteNumber; // + ( ex % Config.System.MeasureNoteNumber == 0 ? 1 : 0 );

                int notePosStart;
                int notePosEnd;

                for ( int measure_no = measureNoStart; measure_no <= measureNoEnd; measure_no++ )
                {
                    var measure = DMS.SCORE.SysChannel.GetMeasure( measure_no );

                    if ( measure == null )
                    {
                        continue;
                    }

                    notePosStart    = ( measure_no == measureNoStart ? sx % Config.System.MeasureNoteNumber : 0 );
                    notePosEnd      = ( measure_no == measureNoEnd   ? ex % Config.System.MeasureNoteNumber : Config.System.MeasureNoteNumber - 1 );

                    for ( int note_pos = notePosStart; note_pos <= notePosEnd; note_pos++ )
                    {
                        if ( !measure.BpmLine.InfoStates.TryGetValue( note_pos, out var info ) )
                        {
                            continue;
                        }

                        rs.SelectBpm( info, true );
                    }
                }
            }
            #endregion
        }

        if ( rs.Count > 0 )
        {
            _EditResumeMng.ExcuteAndResume( rs );
        }
    }

    /// <summary>
    /// 範囲選択内のノート削除処理。
    /// 範囲選択のクリア処理を実施する為、実行前に _NoteRangeBef の設定が必要。
    /// </summary>
    private void RemoveNoteRange()
    {
        var rs = new ResumeMultiple();

        ClearSelectNoteRange( ref rs );

        foreach ( var info in DMS.SCORE.EditChannel.NoteInfoList.Values )
        {
            if ( info.Selected )
            {
                rs.RemoveNote( info );
            }
        }

        if ( DrawSet.IncludeBpm )
        { 
            foreach ( var info in DMS.SCORE.SysChannel.BpmInfoList.Values )
            {
                if ( info.Selected )
                {
                    rs.RemoveBpm( info );
                }
            }
        }

        if ( rs.Count > 0 )
        {
            _EditResumeMng.ExcuteAndResume( rs );
        }
    }

    /// <summary>
    /// 範囲選択の移動
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <param name="aStart">True:入力開始、False:入力途中/終了</param>
    private void EditMoveNoteRange( Point aMousePos, bool aStart )
    {
        var p = CalcNotePosition( aMousePos );

        if ( aStart )
        {
            _NoteRange.SetStartMovePos( p.X, p.Y );
        }
        else
        {
            _NoteRange.SetEndMovePos( p.X, p.Y );
        }
    }

    /// <summary>
    /// 範囲選択の移動処理
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    private void EditMoveNoteRange( Point aMousePos )
    {
        var rs = new ResumeMultiple();

        int a;
        int measure_no;
        int note_pos;
        int mv_x;
        int mv_y;

        #region NoteRange
        {
            EditMoveNoteRange( aMousePos, false );

            mv_x = _NoteRange.MoveNotePosX;
            mv_y = _NoteRange.MoveNotePosY;

            if ( mv_x == 0 && mv_y == 0 )
            {
                return;
            }

            rs.SelectNoteRange( ref _NoteRange, _NoteRangeBef );
        }
        #endregion

        #region Remove notes before and after the move
        {
            foreach ( var info_old in DMS.SCORE.EditChannel.NoteInfoList.Values )
            {
                if ( info_old.Selected )
                {
                    // Before
                    rs.RemoveNote( info_old );

                    // After
                    a = info_old.AbsoluteNotePos + mv_x;

                    measure_no  = a / Config.System.MeasureNoteNumber;
                    note_pos    = a % Config.System.MeasureNoteNumber;

                    var info_new = DMS.SCORE.EditChannel.GetNote( info_old.MidiMapKey, measure_no, note_pos );

                    if ( info_new != null )
                    {
                        rs.RemoveNote( info_new );
                    }
                }
            }
        }
        #endregion

        #region Add notes to move
        { 
            foreach ( var info_old in DMS.SCORE.EditChannel.NoteInfoList.Values )
            {
                if ( info_old.Selected )
                {
                    a = info_old.AbsoluteNotePos + mv_x;

                    measure_no  = a / Config.System.MeasureNoteNumber;
                    note_pos    = a % Config.System.MeasureNoteNumber;

                    int index = DMS.SCORE.EditMidiMapSet.GetDisplayMidiMapIndex( info_old.MidiMapKey );

                    if ( index == -1 )
                    {
                        continue;
                    }

                    index += mv_y;

                    int key = DMS.SCORE.EditMidiMapSet.GetDisplayMidiMapKey( index );

                    if ( key == -1 )
                    {
                        continue;
                    }

                    rs.AddNote( null, new( info_old.ChannelNo, key, measure_no, note_pos, info_old.Volume, info_old.NoteOn, info_old.NoteOff, true ) );
                }
            }
        }
        #endregion

        #region Remove bpms before and after the move
        {
            foreach ( var info_old in DMS.SCORE.SysChannel.BpmInfoList.Values )
            {
                if ( info_old.Selected )
                {
                    // Before
                    rs.RemoveBpm( info_old );

                    // After
                    a = info_old.AbsoluteNotePos + mv_x;

                    measure_no  = a / Config.System.MeasureNoteNumber;
                    note_pos    = a % Config.System.MeasureNoteNumber;

                    var info_new = DMS.SCORE.SysChannel.GetBpm( measure_no, note_pos );

                    if ( info_new != null )
                    {
                        rs.RemoveBpm( info_new );
                    }
                }
            }
        }
        #endregion

        if ( DrawSet.IncludeBpm )
        {
            #region Add bpms to move
            { 
                foreach ( var info_old in DMS.SCORE.SysChannel.BpmInfoList.Values )
                {
                    if ( info_old.Selected )
                    {
                        a = info_old.AbsoluteNotePos + mv_x;

                        measure_no  = a / Config.System.MeasureNoteNumber;
                        note_pos    = a % Config.System.MeasureNoteNumber;

                        rs.AddBpm( null, new( measure_no, note_pos, info_old.Bpm, true ) );
                    }
                }
            }
            #endregion
        }

        if ( rs.Count > 0 )
        {
            _EditResumeMng.ExcuteAndResume( rs );
        }
    }

    /// <summary>
    /// 範囲選択のコピー処理。
    /// 範囲選択のクリア処理を実施する為、実行前に _NoteRangeBef の設定が必要。
    /// </summary>
    private void CopyNoteRange()
    {
		_CopyNoteList.Clear();
        _CopyBpmList.Clear();

        _CopyNotePositionX = Config.System.NoteCount;

		foreach ( var info in DMS.SCORE.EditChannel.NoteInfoList.Values )
        {
            if ( info.Selected )
            {
				if ( _CopyNotePositionX > info.AbsoluteNotePos )
				{
					_CopyNotePositionX = info.AbsoluteNotePos;
				}

				_CopyNoteList.Add( info.Clone() );
            }
        }

		foreach ( var info in DMS.SCORE.SysChannel.BpmInfoList.Values )
        {
            if ( info.Selected )
            {
				if ( _CopyNotePositionX > info.AbsoluteNotePos )
				{
					_CopyNotePositionX = info.AbsoluteNotePos;
				}

				_CopyBpmList.Add( info.Clone() );
            }
        }

        ClearSelectNoteRange();
    }

    /// <summary>
    /// NOTE/BPM貼付処理。
    /// 範囲選択のクリア処理を実施する為、実行前に _NoteRangeBef の設定が必要。
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    private void PasteNoteRange( Point aMousePos )
    {
        if ( !CheckScoreBodyArea( aMousePos ) )
        {
            return;
        }

		var pos = CalcNotePosition( aMousePos );

        if ( pos.X == -1 || pos.Y == -1 )
        {
            return;
        }

        var rs = new ResumeMultiple();

        ClearSelectNoteRange( ref rs );

        #region Add note
        foreach ( var info in _CopyNoteList )
        {
			int x = pos.X + info.AbsoluteNotePos - _CopyNotePositionX;

			int measure_no = x / Config.System.MeasureNoteNumber;
			int note       = x % Config.System.MeasureNoteNumber;

			var info_old = DMS.SCORE.EditChannel.GetNote( info.MidiMapKey, measure_no, note );
			var info_new = new InfoNote( DMS.SCORE.EditChannelNo, info.MidiMapKey, measure_no, note, info.Volume, info.NoteOn, info.NoteOff );

            rs.AddNote( info_old, info_new );
        }
        #endregion

        #region Add bpm

        if ( DrawSet.IncludeBpm )
        { 
            foreach ( var info in _CopyBpmList )
            {
				int x = pos.X + info.AbsoluteNotePos - _CopyNotePositionX;

				int measure_no = x / Config.System.MeasureNoteNumber;
				int note       = x % Config.System.MeasureNoteNumber;

				var info_old	= DMS.SCORE.SysChannel.GetBpm( measure_no, note );
				var info_new	= new InfoBpm( measure_no, note, info.Bpm );

                rs.AddBpm( info_old, info_new );
            }
        }

        #endregion

        if ( rs.Count > 0 )
        {
            _EditResumeMng.ExcuteAndResume( rs );
        }
    }

    /// <summary>
    /// ノート音量入力クリア
    /// </summary>
    private void ClearVolumeRange() => _VolumeRange.ClearPos();

    /// <summary>
    /// ノート音量入力の編集
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <param name="aStart">True:入力開始、False:入力途中/終了</param>
    private void EditVolumeRange( Point aMousePos, bool aStart )
    {
        if ( !CheckVolumeBodyArea( aMousePos ) )
        {
            return;
        }

		var pos = CalcNoteVolumePosition( aMousePos );

        if ( pos.X == -1 || pos.Y == -1 )
        {
            return;
        }

        if ( aStart )
        {
            _VolumeRange.SetStartPos( pos.X, pos.Y, DrawSet.VolumeEditSelect );
        }
        else
        {
            _VolumeRange.SetEndPos( pos.X, pos.Y );
        }
    }

    /// <summary>
    /// NOTE音量の編集
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
	private void EditNoteVolume( Point aMousePos )
	{
        EditVolumeRange( aMousePos, false );

        var rs = new ResumeMultiple();

        int sn = _VolumeRange.StartNotePosX;
        int en = _VolumeRange.EndNotePosX;

        {
            int measure_no;
            int note_pos;

            for ( int x = sn; x <= en; x++ )
            {
                measure_no  = x / Config.System.MeasureNoteNumber;
                note_pos    = x % Config.System.MeasureNoteNumber;

                var measure = DMS.SCORE.EditChannel.GetMeasure( measure_no );

                if ( measure == null )
                {
                    continue;
                }

				foreach ( var midiMap in DMS.SCORE.EditMidiMapSet.DisplayMidiMaps )
				{
                    if ( !midiMap.Group?.Selected ?? false && !midiMap.Selected )
                    {
                        continue;
                    }

					if ( !measure.NoteLines.TryGetValue( midiMap.MidiMapKey, out var note_line ))
					{
                        continue;
                    }

                    if ( !note_line.InfoStates.TryGetValue( note_pos, out var info_old ) )
                    {
                        continue;
                    }

                    if ( info_old == null || !info_old.NoteOn )
                    {
                        continue;
                    }

                    var info_new = info_old.Clone();

                    switch ( _VolumeRange.EditType )
                    {
                        case ConfigEditer.VolumeEditType.VolumeUpDown:
                            info_new.Volume += _VolumeRange.VolumeList[ x - sn ];
                            break;
                        case ConfigEditer.VolumeEditType.VolumeIntonationHL:
                            info_new.Volume += _VolumeRange.VolumeList[ x - sn ]
                                * ( info_new.Volume < _VolumeRange.StartVolume ? -1 : 1 );
                            break;
                        case ConfigEditer.VolumeEditType.VolumeIntonationH:
                            info_new.Volume += _VolumeRange.VolumeList[ x - sn ]
                                * ( info_new.Volume >= _VolumeRange.StartVolume ? 1 : 0 );
                            break;
                        case ConfigEditer.VolumeEditType.VolumeIntonationL:
                            info_new.Volume += _VolumeRange.VolumeList[ x - sn ]
                                * ( info_new.Volume <= _VolumeRange.StartVolume ? 1 : 0 );
                            break;
                        default:
                            info_new.Volume  = _VolumeRange.VolumeList[ x - sn ];
                            break;
                    }

                    info_new.Volume = Config.Media.CheckMidiVolume( info_new.Volume );

                    rs.SetNoteVolume( info_old, info_new );
				}
            }
        }

        if ( rs.Count == 0 )
        {
            return;
        }

        _EditResumeMng.ExcuteAndResume( rs );

        #region ノート音量表示更新
        {
            int sm = sn / Config.System.MeasureNoteNumber;
            int em = en / Config.System.MeasureNoteNumber;

            for ( int m = sm; m <= em; m++ )
            {
                UpdateNoteVolumeMeasure( m );
            }
        }
        #endregion
    }

    #endregion

    #region Update

    /// <summary>
    /// フレーム処理
    /// </summary>
    public void OnMove()
	{
        #region Resume

        if ( DrawSet.UpdateRedoFlag )
        {
            _EditResumeMng.Redo();
            DrawSet.UpdateRedoFlag = false;
        }

        if ( DrawSet.UpdateUndoFlag )
        {
            _EditResumeMng.Undo();
            DrawSet.UpdateUndoFlag = false;
        }

        if ( DrawSet.UpdateResumeClearFlag )
        {
            _EditResumeMng.Clear();
            DrawSet.UpdateResumeClearFlag = false;
        }

        #endregion

        #region Score size

        if ( DrawSet.UpdateScoreLayoutFlag )
		{
			UpdateScore();
            DrawSet.UpdateScoreLayoutFlag = false;
        }

        #endregion

        #region Sheet potition

        if ( DrawSet.UpdateCameraFlag )
		{
			UpdateSheetPosition();
            DrawSet.UpdateCameraFlag = false;
		}

        #endregion

        #region Line

        if ( DrawSet.UpdateScoreLineFlag )
		{
			UpdateScoreLine();
            DrawSet.UpdateScoreLineFlag = false;
		}

        #endregion

        #region Score header

        if ( DrawSet.UpdateScoreHeaderFlag )
		{
            UpdateMidiMapHeader();
            DrawSet.UpdateScoreHeaderFlag = false;
		}

        #endregion

        #region Score All

        if ( DrawSet.UpdateScoreFlag )
        {
            UpdateBpmMeasure();
            UpdateNoteMeasure();
            UpdateNoteVolumeMeasure();
            UpdateNoteOnOff();

            DrawSet.UpdateScoreBpmMeasureNoList.Clear();
            DrawSet.UpdateScoreNoteMeasureNoList.Clear();
            DrawSet.UpdateScoreNoteVolumeMeasureNoList.Clear();

            DrawSet.UpdateScoreFlag             = false;
            DrawSet.UpdateScoreBpmFlag          = false;
            DrawSet.UpdateScoreNoteFlag         = false;
            DrawSet.UpdateScoreNoteVolumeFlag   = false;
        }

        #endregion

        #region Note

        if ( DrawSet.UpdateScoreNoteFlag )
		{
            foreach ( var measure_no in DrawSet.UpdateScoreNoteMeasureNoList )
            {
                UpdateNoteMeasure( measure_no );
                UpdateNoteVolumeMeasure( measure_no );
            }
            UpdateNoteOnOff();

            DrawSet.UpdateScoreNoteMeasureNoList.Clear();
            DrawSet.UpdateScoreNoteFlag = false;
        }

        #endregion

        #region Note Predict

        if ( DrawSet.UpdateScoreNotePredictFlag )
        {
            // TODO: 機械学習結果反映に使用予定。自動採譜試してみるときに使うかも
            UpdateNotePredictListMeasure();
            DrawSet.UpdateScoreNotePredictFlag = false;
        }

        #endregion

        #region Bgm

        if ( DrawSet.UpdateScoreBgmFlag )
        {
            UpdateScaleBgm();
            DrawSet.UpdateScoreBgmFlag = false;
        }

        if ( DrawSet.UpdateScoreBgmScaleFlag )
        {
            UpdateScaleBgmBitmap();
        }

        #endregion

        #region Bpm

        if ( DrawSet.UpdateScoreBpmFlag )
        {
            foreach ( var measure_no in DrawSet.UpdateScoreBpmMeasureNoList )
            {
                UpdateBpmMeasure( measure_no );
            }
            DrawSet.UpdateScoreBpmMeasureNoList.Clear();
            DrawSet.UpdateScoreBpmFlag = false;
        }

        #endregion

        #region Note Volume

        if ( DrawSet.UpdateScoreNoteVolumeFlag )
        {
            foreach ( var measure_no in DrawSet.UpdateScoreNoteVolumeMeasureNoList )
            { 
                UpdateNoteVolumeMeasure( measure_no );
            }
            DrawSet.UpdateScoreNoteVolumeMeasureNoList.Clear();
            DrawSet.UpdateScoreNoteVolumeFlag = false;
        }

        #endregion

        #region Clear range

        if ( DrawSet.UpdateClearRangeFlag )
        {
			_NoteRangeBef.Set( _NoteRange );

            ClearSelectNoteRange();

            DrawSet.UpdateClearRangeFlag = false;
        }

        #endregion
    }

    /// <summary>
    /// 表示範囲更新
    /// </summary>
    private void UpdateScore()
	{
		// Screen
        _ScreenSize.Height			= Height;
        _ScreenSize.Width			= Width;

		// Infomation
        _InfoRange.X			    = 0;
        _InfoRange.Y			    = 0;
        _InfoRange.Width		    = DrawSet.HeaderWidthSize / 2;
        _InfoRange.Height		    = DrawSet.BpmHeightSize + DrawSet.MeasureNoHeightSize;

		// Bpm header
        _BpmHeadRange.X			    = _InfoRange.Right;
        _BpmHeadRange.Y			    = 0;
        _BpmHeadRange.Width		    = DrawSet.HeaderWidthSize / 2;
        _BpmHeadRange.Height		= DrawSet.BpmHeightSize;

		// Bpm body
        _BpmBodyRange.X			    = _BpmHeadRange.Right;
        _BpmBodyRange.Y			    = _BpmHeadRange.Top;
        _BpmBodyRange.Width		    = _ScreenSize.Width - _BpmHeadRange.Right > 0 ? _ScreenSize.Width - _BpmHeadRange.Right : 0 ;
        _BpmBodyRange.Height		= _BpmHeadRange.Height;

		// Measure number header
        _MeasureNoHeadRange.X		= _InfoRange.Right;
        _MeasureNoHeadRange.Y		= _BpmBodyRange.Bottom;
        _MeasureNoHeadRange.Width	= DrawSet.HeaderWidthSize / 2;
        _MeasureNoHeadRange.Height	= DrawSet.MeasureNoHeightSize;

		// Measure number body
        _MeasureNoBodyRange.X		= _MeasureNoHeadRange.Right;
        _MeasureNoBodyRange.Y		= _MeasureNoHeadRange.Top;
        _MeasureNoBodyRange.Width	= _ScreenSize.Width - _MeasureNoHeadRange.Right > 0 ? _ScreenSize.Width - _MeasureNoHeadRange.Right : 0 ;
        _MeasureNoBodyRange.Height	= _MeasureNoHeadRange.Height;

		// Score sheet header
        _ScoreHeadRange.X			= 0;
        _ScoreHeadRange.Y			= _MeasureNoBodyRange.Bottom;
        _ScoreHeadRange.Width		= DrawSet.HeaderWidthSize;
        _ScoreHeadRange.Height		= DrawSet.ScoreMaxHeight;

        // Score sheet body
        _ScoreBodyRange.X			= _ScoreHeadRange.Right;
        _ScoreBodyRange.Y			= _ScoreHeadRange.Top;
        _ScoreBodyRange.Width		= _ScreenSize.Width - _ScoreHeadRange.Right > 0 ? _ScreenSize.Width - _ScoreHeadRange.Right : 0 ;
        _ScoreBodyRange.Height		= _ScoreHeadRange.Height;

        // Volume header
        _VolumeHeadRange.X			= 0;
        _VolumeHeadRange.Y			= _ScreenSize.Height - DrawSet.VolumeHeightSize > 0 ? _ScreenSize.Height - DrawSet.VolumeHeightSize : 0 ;
        _VolumeHeadRange.Width		= DrawSet.HeaderWidthSize;
        _VolumeHeadRange.Height	    = DrawSet.VolumeHeightSize;

        // Volume body
        _VolumeBodyRange.X			= _VolumeHeadRange.Right;
        _VolumeBodyRange.Y			= _VolumeHeadRange.Top;
        _VolumeBodyRange.Width		= _ScreenSize.Width - _VolumeHeadRange.Right > 0 ? _ScreenSize.Width - _VolumeHeadRange.Right : 0 ;
        _VolumeBodyRange.Height	    = _VolumeHeadRange.Height;

        // Number of notes that can be displayed
        {
            _ScoreViewHeight = _ScoreBodyRange.Height;
                
            int h = _ScreenSize.Height - _ScoreBodyRange.Y - _VolumeBodyRange.Height;

            if ( _ScoreViewHeight > h )
            {
                _ScoreViewHeight = h;
            }
        }

        _SupportLine.SetNoteRect
            ( 
                _MeasureNoBodyRange.X, 
                _MeasureNoBodyRange.Y, 
                DrawSet.NoteWidthSize, 
                _MeasureNoBodyRange.Height + _ScoreBodyRange.Height 
            );

        // Note selection range (1 square on the upper left of the note)
        _NoteRange.SetNoteRect
            ( 
                _ScoreBodyRange.X, 
                _ScoreBodyRange.Y, 
                DrawSet.NoteWidthSize, 
                DrawSet.NoteHeightSize 
            );

        // Volume range
        _VolumeRange.SetBottomPosition( _VolumeBodyRange.X, _VolumeBodyRange.Bottom );

        // Positioning volume
        foreach ( var list in _VolumeList.Values )
        {
            foreach ( var v in list )
            {
                v.Move( _VolumeBodyRange.Bottom );
            }
        }
    }

    /// <summary>
    /// シート位置更新
    /// </summary>
    private void UpdateSheetPosition()
    {
		var body	    = _ScoreBodyRange;
        int view_h      = _ScoreViewHeight;
		var	note_pos    = DrawSet.NotePosition;

        #region Horizontal position check
        {
            int w_cnt = Config.System.NoteCount - body.Width / DrawSet.NoteWidthSize;

            if ( note_pos.X > w_cnt )
            {
                note_pos.X = w_cnt;
            }

            if ( note_pos.X < 0 )
            {
                note_pos.X = 0;
            }
        }
        #endregion

        #region Vertical position check
        {
            int d_cnt = DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount;
            int h_cnt = view_h / DrawSet.NoteHeightSize;

            if ( d_cnt <= h_cnt )
            {
                note_pos.Y = 0;
            }
            else if ( note_pos.Y > d_cnt - h_cnt )
            {
                note_pos.Y = d_cnt - h_cnt;
            }

            if ( note_pos.Y < 0 )
            {
                note_pos.Y = 0;
            }
        }
        #endregion

        // Update sheet position
        DrawSet.NotePosition = new( note_pos.X, note_pos.Y );
    }

    /// <summary>
    /// 小節線表示更新
    /// </summary>
    private void UpdateScoreLine()
	{
        var body        = _ScoreBodyRange;
        var note_num    = Config.System.MeasureNoteNumber;
        var note_width  = DrawSet.NoteWidthSize;

        #region Bar line
        {
            var pens = new Pen[]
                {
                    DrawSet.SheetMeasure001Pen
                ,   DrawSet.SheetMeasure004Pen
                ,   DrawSet.SheetMeasure008Pen
                ,   DrawSet.SheetMeasure016Pen
                ,   DrawSet.SheetMeasure032Pen
                ,   DrawSet.SheetMeasure064Pen
                ,   DrawSet.SheetMeasure128Pen
                };

            _MeasureLineList.Clear();

            int linesize = pens[ 0 ].Width == 0 ? 0 : 1 ;

            if ( linesize == 0 )
            {
                for ( int i = 1; i <= 6; i++ )
                {
                    if ( pens[i].Width != 0 ) 
                    {
                        linesize = (int)Math.Pow( 2, i + 1 ); 
                        break;
                    }
                }
            }

            if ( linesize != 0 )
            {
                Pen? pen = null;

                for ( int i = 0; i < note_num; i += linesize )
                {
                    for ( int x = 6, y = 1; x >= 0; x--, y *= 2 )
                    {
                        pen = pens[ x ];

                        if ( i % ( note_num / y ) == 0 && ( pen.Width != 0 ) )
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
                                body.X + note_width * i, 
                                body.Top, 
                                0, 
                                body.Height, 
                                pen 
                            ) 
                        );

                    pen = null;
                }
            }
        }
        #endregion

        #region Staff
        {
            var pens = new Pen[]
                {
                    DrawSet.SheetStaffGroupPen
                ,   DrawSet.SheetStaffMidiMapPen
                };

            int index = -1;

            for ( int i = 0; i <= 1; i++ )
            {
				if ( pens[i].Width != 0 )
				{
					index = i;
                    break;
				}
            }

            _StaffLineList.Clear();

			if ( index != -1 )
			{
				int x1 = body.X;
				int x2 = body.Width;
				int y  = body.Y;

				for ( int i = 0; i < DMS.SCORE.EditMidiMapSet.DisplayGroupCount; i++ )
				{
    				_StaffLineList.Add( new( x1, y, x2, 0, pens[ index ] ) );

					y += DrawSet.NoteHeightSize;

					{
						int midiMapCount = DMS.SCORE.EditMidiMapSet.DisplayMidiMapCountByGroup[ i ];

                        var pen = pens[ 1 ];

                        if ( pen.Width != 0 )
                        {
							for ( int j = 1; j < midiMapCount; j++ )
							{
								_StaffLineList.Add( new( x1, y, x2, 0, pen ) );

								y += DrawSet.NoteHeightSize;
							}
                        }
                        else
                        {
    						y += DrawSet.NoteHeightSize * ( midiMapCount - 1);
                        }
					}
				}

                _StaffLineList.Add( new( x1, y, x2, 0, pens[ index ] ) );
			}
        }
        #endregion
    }

    /// <summary>
    /// MidiMapGroup/MidiMapヘッダ表示更新
    /// </summary>
    private void UpdateMidiMapHeader()
	{
        var head = _ScoreHeadRange;

        _HeaderList.Clear();

        int tmp_y = head.Y;

        int x;
        int y;
        int w;
        int h;

        #region MidiMapGroup
        {
            x = head.X;
            y = head.Y;
            w = head.Width / 2;

            for ( int index = 0; index < DMS.SCORE.EditMidiMapSet.DisplayGroupCount; index++ )
            {
                var group = DMS.SCORE.EditMidiMapSet.GetDisplayMidiMapGroup( index );

                h = DrawSet.NoteHeightSize * DMS.SCORE.EditMidiMapSet.DisplayMidiMapCountByGroup[ index ];

                if ( group != null )
                { 
                    _HeaderList.Add( new( x, y, w, h, group ) );
                }

                y += h;
            }
        }
        #endregion

        #region MidiMap
        {
            x = head.X + w;
			y = tmp_y;
			w = head.Width - w;
			h = DrawSet.NoteHeightSize;

			foreach ( var midiMap in DMS.SCORE.EditMidiMapSet.DisplayMidiMaps )
			{
                if ( !_MidiMapBrushList.ContainsKey( midiMap.MidiMapKey ) )
                {
                    _MidiMapBrushList.Add( midiMap.MidiMapKey, new( midiMap.Color ) );
                }
                else
                {
                    _MidiMapBrushList[ midiMap.MidiMapKey ].Color = midiMap.Color;
                }

                _HeaderList.Add( new( x, y, w, h, midiMap ) );

				y += h;
			}
        }
        #endregion
    }

    /// <summary>
    /// 小節ノート表示更新
    /// </summary>
    private void UpdateNoteMeasure()
    {
        for ( int measure_no = 0; measure_no <= Config.System.MeasureMaxNumber; measure_no++ )
        {
            UpdateNoteMeasure( measure_no );
        }
    }

    /// <summary>
    /// 小節ノート表示更新
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    private void UpdateNoteMeasure( int aMeasureNo )
    {
        #region Clear
        {
            if ( _NoteList.TryGetValue( aMeasureNo, out var nList ) )
            {
                nList.Clear();
                _NoteList.Remove( aMeasureNo );
            }
        }
        #endregion

        var measure = DMS.SCORE.EditChannel.GetMeasure( aMeasureNo );

        if ( measure == null )
        {
            return;
        }

        var body_s    = _ScoreBodyRange;
        var note_rect = new Rectangle( 0, body_s.Y, DrawSet.NoteWidthSize, DrawSet.NoteHeightSize );

        #region Note
		{
			foreach ( var midiMap in DMS.SCORE.EditMidiMapSet.DisplayMidiMaps )
			{
				if ( measure.NoteLines.TryGetValue( midiMap.MidiMapKey, out var measure_line ) )
				{
					foreach ( var info in measure_line.InfoStates.Values )
					{
						note_rect.X = body_s.X + info.NotePos * note_rect.Width - note_rect.Height / 2;

						if ( !_NoteList.TryGetValue( aMeasureNo, out var lst ) )
						{
							lst = new();
						}
						lst.Add
                            (
							    new
								(
									note_rect.X,
								    note_rect.Y,
								    note_rect.Height,
								    note_rect.Height,
                                    info,
					                _MidiMapBrushList[ midiMap.MidiMapKey ]
                                )
					        );

						_NoteList[ aMeasureNo ] = lst;
					}
				}
				note_rect.Y += note_rect.Height;
			}
		}
        #endregion

        #region Sort
        {
            if ( _NoteList.TryGetValue( aMeasureNo, out var lst ) )
            {
                lst.Sort();
            }
        }
        #endregion
    }

    /// <summary>
    /// ノートON/OFFの関連付け更新
    /// </summary>
    private void UpdateNoteOnOff()
    {
        // TODO: NOTE-OFF の実装方法が微妙、Drumのみであれば問題ないが

        foreach ( var item in _NoteList.Values )
        {
            foreach ( var note in item )
            { 
                if ( note?.InfoNote?.NoteOn ?? false )
                {
                    var distanceToNextNoteOff = DMS.SCORE.EditChannel.GetNotePosDistanceToNextNoteOff( note.InfoNote );
                    note.NoteLength = distanceToNextNoteOff == 0 ? 0 : distanceToNextNoteOff * DrawSet.NoteWidthSize ;
                }
            }
        }

#if false
        var befNoteOff = new Dictionary<int, DmsItemNote>();

        for ( int measure_no = Config.System.MeasureMaxNumber; measure_no >= 0; measure_no-- )
        {
            if ( !_NoteList.TryGetValue( measure_no, out var lst ) )
            {
                continue;
            }

            lst.Reverse();

            foreach ( var item in lst )
            {
                item.NoteLength = 0;
                item.NoteOnItem = null;

                if ( item.InfoNote == null )
                {
                    continue;
                }

                if ( item.InfoNote.NoteOn )
                {
                    if ( befNoteOff.TryGetValue( item.InfoNote.MidiMapKey, out var bef_item ) && bef_item.InfoNote != null )
                    {
                        item.NoteLength = ( bef_item.InfoNote.MeasureNo * DrawSet.MeasureSize + bef_item.NoteRect.Left ) 
                                        - ( item.InfoNote.MeasureNo     * DrawSet.MeasureSize + item.NoteRect.Right );

                        bef_item.NoteOnItem = item;

                        befNoteOff.Remove( item.InfoNote.MidiMapKey );
                    }
                }

                if ( item.InfoNote.NoteOff )
                {
                    befNoteOff[ item.InfoNote.MidiMapKey ] = item;
                }
            }

            lst.Sort();
        }

        befNoteOff.Clear();
#endif
    }

    /// <summary>
    /// 小節ノート予測表示更新
    /// </summary>
    private void UpdateNotePredictListMeasure()
    {
        for ( int measure_no = 0; measure_no <= Config.System.MeasureMaxNumber; measure_no++ )
        {
            UpdateNotePredictListMeasure( measure_no );
        }
    }

    /// <summary>
    /// 小節ノート予測表示更新
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    private void UpdateNotePredictListMeasure( int aMeasureNo )
    {
        #region Clear
        {
            if ( _NotePredictList.TryGetValue( aMeasureNo, out var nList ) )
            {
                nList.Clear();
                _NotePredictList.Remove( aMeasureNo );
            }
        }
        #endregion

        var measure = DMS.SCORE_PREDICT.EditChannel.GetMeasure( aMeasureNo );

        if ( measure == null )
        {
            return;
        }

        var body_s    = _ScoreBodyRange;
        var note_rect = new Rectangle( 0, body_s.Y, DrawSet.NoteWidthSize, DrawSet.NoteHeightSize );

        #region Note
		{
			foreach ( var midiMap in DMS.SCORE.EditMidiMapSet.DisplayMidiMaps )
			{
				if ( measure.NoteLines.TryGetValue( midiMap.MidiMapKey, out var measure_line ) )
				{
					foreach ( var info in measure_line.InfoStates.Values )
					{
						note_rect.X = body_s.X + info.NotePos * note_rect.Width - note_rect.Height / 2;

						if ( !_NotePredictList.TryGetValue( aMeasureNo, out var lst ) )
						{
							lst = new();
						}
						lst.Add
                            (
							    new
								(
									note_rect.X,
								    note_rect.Y,
								    note_rect.Height,
								    note_rect.Height,
                                    info,
					                _MidiMapBrushList[ midiMap.MidiMapKey ]
                                )
					        );

						_NotePredictList[ aMeasureNo ] = lst;
					}
				}
				note_rect.Y += note_rect.Height;
			}
		}
        #endregion

        #region Sort
        {
            if ( _NotePredictList.TryGetValue( aMeasureNo, out var lst ) )
            {
                lst.Sort();
            }
        }
        #endregion
    }

    /// <summary>
    /// 小節ノート音量表示更新
    /// </summary>
    private void UpdateNoteVolumeMeasure()
    {
        for ( int measure_no = 0; measure_no <= Config.System.MeasureMaxNumber; measure_no++ )
        {
            UpdateNoteVolumeMeasure( measure_no );
        }

        DrawSet.VolumeDisplay = DMS.SCORE.EditMidiMapSet.IsSelectMidiMap();
    }

    /// <summary>
    /// 小節ノート音量表示更新
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    private void UpdateNoteVolumeMeasure( int aMeasureNo )
    {
        #region Clear
        {
            if ( _VolumeList.TryGetValue( aMeasureNo, out var nList ) )
            {
                nList.Clear();
                _VolumeList.Remove( aMeasureNo );
            }
        }
        #endregion

        var measure = DMS.SCORE.EditChannel.GetMeasure( aMeasureNo );

        if ( measure == null )
        {
            return;
        }

        var body = _VolumeBodyRange;
        var n_w  = DrawSet.NoteWidthSize;

        #region Note
		{
			foreach ( var midiMap in DMS.SCORE.EditMidiMapSet.DisplayMidiMaps )
			{
                if ( !midiMap.Group?.Selected ?? false && !midiMap.Selected )
                {
                    continue;
                }

				if ( !measure.NoteLines.TryGetValue( midiMap.MidiMapKey, out var measure_line ) )
				{
                    continue;
                }

				foreach ( var info in measure_line.InfoStates.Values )
				{
                    if ( !info.NoteOn )
                    {
                        continue;
                    }

					if ( !_VolumeList.TryGetValue( aMeasureNo, out var lst ) )
					{
						lst = new();
					}
					lst.Add
                        (
                            new
                            (
                                body.X + info.NotePos * n_w - n_w / 2,
                                body.Bottom,
                                info,
                                _MidiMapBrushList[ midiMap.MidiMapKey ]
                            )
                        );

                    _VolumeList[ aMeasureNo ] = lst;
				}
			}
		}
        #endregion
    }

    /// <summary>
    /// 小節BPM表示更新
    /// </summary>
    private void UpdateBpmMeasure()
    {
        for ( int measure_no = 0; measure_no <= Config.System.MeasureMaxNumber; measure_no++ )
        {
            UpdateBpmMeasure( measure_no );
        }
    }

    /// <summary>
    /// 小節BPM表示更新
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    private void UpdateBpmMeasure( int aMeasureNo )
    {
        #region Clear
        {
            if ( _BpmList.TryGetValue( aMeasureNo, out var nList ) )
            {
                nList.Clear();
                _BpmList.Remove( aMeasureNo );
            }
        }
        #endregion

        var body      = _BpmBodyRange;
        var note_rect = new Rectangle( 0, body.Y, DrawSet.NoteWidthSize, DrawSet.BpmHeightSize );

        #region Bpm
        {
            var measure = DMS.SCORE.SysChannel.GetMeasure( aMeasureNo );

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
                note_rect.X = body.X + info.NotePos * DrawSet.NoteWidthSize;

                if ( !_BpmList.TryGetValue( aMeasureNo, out var lst ) )
                {
                    lst = new();
                }

                lst.Add
                    ( 
                        new
                        (
                            note_rect.X,
                            note_rect.Y,
                            note_rect.Width,
                            note_rect.Height,
                            info
                        )
                    );

                _BpmList[ aMeasureNo ] = lst;
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

    /// <summary>
    /// BGM音階表示更新
    /// </summary>
    private void UpdateScaleBgm()
    {
        try
        {
            DrawSet.UpdateScoreBgmScaleFlag = false;

            _ScaleBgm?.Dispose();
            _ScaleBgm = null;

            if ( DMS.SCORE.BgmFilePath.IsExistFile )
            { 
                _ScaleBgm = new NAudioData( DMS.SCORE.BgmFilePath, true );
            }

            DrawSet.UpdateScoreBgmScaleFlag = true;
        }
        catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    /// <summary>
    /// BGM音階表示画像更新
    /// </summary>
    private async void UpdateScaleBgmBitmap()
    {
        await Task.Run
            ( 
                () =>
                    { 
                        if ( _ScaleBgm == null )
                        {
                            _ScaleBitmap?.Dispose();
                            _ScaleBitmap = null;

                            DrawSet.UpdateScoreBgmScaleFlag = false;
                            return;
                        }

                        if ( !_ScaleBgm.IsEnableFFT() )
                        {
                            return;
                        }

                        DrawSet.UpdateScoreBgmScaleFlag = false;

                        try
                        {
                            _ScaleBitmap = CreateScaleBitmap();
                        }
                        catch ( Exception e )
			            {
                            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
			            }
                    } 
            );
    }

    /// <summary>
    /// 音階別周波数表示用Bitmap作成
    /// </summary>
    /// <returns>音階別周波数表示用Bitmap</returns>
    private Bitmap? CreateScaleBitmap()
    {
        if ( !( _ScaleBgm?.IsEnableFFT() ?? false ) )
        {
            return null;
        }

        // TODO: WaveForm 何か案を思いつけば対応

        var t_level = Config.Scale.VolumeLevelTop;
        var h_level = Config.Scale.VolumeLevelHigh;
        var m_level = Config.Scale.VolumeLevelMid;
        var l_level = Config.Scale.VolumeLevelLow;

        if ( t_level - l_level <= 0F )
        {
            return null;
        }

		if ( DMS.SCORE.EditMidiMapSet.DisplayMidiMaps
                .Where( item => Config.Scale.GetScaleListIndex( item.Scale ) != -1 )
                .Count() == 0 )
        {
            return null;
        }

        var bgm_time        = _ScaleBgm.GetDuration();
        var note_pos_x_last = DmsControl.SearchPosition( bgm_time.TotalSeconds + DMS.SCORE.BgmReproducePosition );
        var note_rect       = new Rectangle( 0, 0, 1, 1 );

        var body = new Rectangle
            (
                0,
                0,
                note_rect.Width  * note_pos_x_last,
                note_rect.Height * DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount
            );

        var bmp = new Bitmap( body.Width, body.Height );

        using var g = Graphics.FromImage( bmp );

        // 背景色塗りつぶし
        g.Clear( DrawSet.SheetColor );

        #region 周波数
        {
            for ( int fft_offset = 0; fft_offset < _ScaleBgm.FFTBufferLength0; fft_offset++ )
            {
                var note_pos_time   = _ScaleBgm.GetFFTTime( fft_offset );
                var note_pos_x      = DmsControl.SearchPosition( note_pos_time + DMS.SCORE.BgmReproducePosition );

                var fft         = _ScaleBgm.GetFFTBuffer( fft_offset );
                var hzPerOne    = (float)_ScaleBgm.GetSampleRate() / ( fft.Count * _ScaleBgm.Channels );

                var list = new List<int>();

                var midimap_index = -1;

			    foreach ( var midiMap in DMS.SCORE.EditMidiMapSet.DisplayMidiMaps )
			    {
                    midimap_index++;

                    var index = Config.Scale.GetScaleListIndex( midiMap.Scale );

                    if ( index == -1 )
                    {
                        continue;
                    }

                    var item = Config.Scale.ScaleList[ index ];

                    var fft_index = (int)( item.Hz / hzPerOne ) * _ScaleBgm.Channels;

                    if ( fft_index >= fft.Count )
                    {
                        continue;
                    }

                    var db = fft[ fft_index ];
                    for ( int x = 1; x < _ScaleBgm.Channels; x++ )
                    {
                        db = Math.Max( db, fft[ fft_index + x ] );
                    }

                    db = ( db - l_level ) * ( 1F / ( t_level - l_level ) );

                    if ( db < 0F )
                    {
                        db = 0F;
                    }
                    else if ( db > 1F )
                    {
                        db = 1F;
                    }

                    var alpha   = 255;//155 + (int)( 100 * db );
                    var red     = db >= h_level ? (int)( 255 * db ) : 0;
                    var green   = db >= m_level ? (int)( 255 * db ) : 0;
                    var blue    = db >= l_level ? (int)( 255 * db ) : 0;
                    //var red     = t_level > db && db >= h_level ? (int)( 255 * db ) : 0;
                    //var green   = h_level > db && db >= m_level ? (int)( 255 * db ) : 0;
                    //var blue    = m_level > db && db >= l_level ? (int)( 255 * db ) : 0;

                    if ( db > 0F )
                    {
                        note_rect.X = body.X + note_rect.Width  * note_pos_x;
                        note_rect.Y = body.Y + note_rect.Height * midimap_index;

                        g.FillRectangle( new SolidBrush( Color.FromArgb( alpha, red, green, blue ) ), note_rect );
                    }
                }
            }
        }
        #endregion

        return bmp;
    }

    #endregion

    #region Draw

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphic"></param>
    public void OnDraw( Graphics aGraphic )
    {
        var body            = _ScoreBodyRange;
        var note_pos        = DrawSet.NotePosition;            

        var sheet_pos       = new Point( note_pos.X * DrawSet.NoteWidthSize, note_pos.Y * DrawSet.NoteHeightSize );            
        int measure_size    = DrawSet.MeasureSize;
        int measure_start   = note_pos.X / Config.System.MeasureNoteNumber;
        int measure_end     = ( note_pos.X + body.Width / DrawSet.NoteWidthSize ) / Config.System.MeasureNoteNumber + 1;

        #region WaveForm
        { 
            // TODO: WaveForm 何か案を思いつけば対応

            // 画像表示
            if ( Config.Scale.WaveFormOn && _ScaleBitmap != null )
            {
                // 描画範囲
                var destRect = new Rectangle
                    (
                        body.X,
                        body.Y,
                        body.Width,
                        body.Height
                    );

                // 切取範囲
                var srcRect = new Rectangle
                    (
                        sheet_pos.X     / DrawSet.NoteWidthSize,
                        sheet_pos.Y     / DrawSet.NoteHeightSize,
                        destRect.Width  / DrawSet.NoteWidthSize,
                        destRect.Height / DrawSet.NoteHeightSize
                    );

                var t_level = Config.Scale.VolumeLevelTop;
                var h_level = Config.Scale.VolumeLevelHigh;
                var m_level = Config.Scale.VolumeLevelMid;
                var l_level = Config.Scale.VolumeLevelLow;

                var cm = new ColorMatrix
                        {
                            Matrix00 = Config.Scale.SensitivityLevel >= h_level / 4.0F ? Config.Scale.SensitivityLevel : 0F,
                            Matrix11 = Config.Scale.SensitivityLevel >= m_level / 3.0F ? Config.Scale.SensitivityLevel : 0F,
                            Matrix22 = Config.Scale.SensitivityLevel >= l_level / 2.0F ? Config.Scale.SensitivityLevel : 0F,
                            Matrix33 = 1F,
                            Matrix44 = 1F,
                        };

                // 色補正
                var ia = new ImageAttributes();
                ia.SetColorMatrix( cm );

                // 描画
                aGraphic.DrawImage
                    ( 
                        _ScaleBitmap, 
                        destRect, 
                        srcRect.X, 
                        srcRect.Y, 
                        srcRect.Width, 
                        srcRect.Height, 
                        GraphicsUnit.Pixel, 
                        ia 
                    );
            }
        }
        #endregion

        #region Cursor horizontal line
        {
            if ( _NoteCursorPosition.Y - note_pos.Y >= 0 )
            {
                var r = new Rectangle
                    ( 
                        body.X, 
                        body.Y + ( _NoteCursorPosition.Y - note_pos.Y ) * DrawSet.NoteHeightSize, 
                        body.Width,
                        DrawSet.NoteHeightSize 
                    );

                aGraphic.FillRectangle( DrawSet.SheetCursorLineH, r );
            }
        }
        #endregion

        #region Bar line
        {
            int cnt;
			int diff_x;

            for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
            {
				diff_x = measure_size * measure_no - sheet_pos.X;
				cnt    = _MeasureLineList.Count;

				for ( int index = 0; index < cnt; index++ )
				{
					_MeasureLineList[ index ].Draw( aGraphic, diff_x, -sheet_pos.Y );
                }
            }
        }
        #endregion

		#region Staff
        {
			int cnt = _StaffLineList.Count;

			for ( int index = 0; index < cnt; index++ )
            {
				_StaffLineList[ index ].Draw( aGraphic, 0, -sheet_pos.Y );
            }
        }
        #endregion

        #region Support line
        {
            _SupportLine.Draw( aGraphic, measure_start, measure_end, note_pos.X, -sheet_pos.Y );
        }
        #endregion

        #region Note
        {
            int diff_x;

            for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
            {
                if ( !_NoteList.TryGetValue( measure_no, out var notes ) )
                {
                    continue;
                }

				diff_x = measure_size * measure_no - sheet_pos.X;

				foreach ( var note in notes )
				{
					note.Draw( aGraphic, diff_x, -sheet_pos.Y );
				}
            }
        }
        #endregion

        #region Note predict
        {
            int diff_x;

            for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
            {
                if ( !_NotePredictList.TryGetValue( measure_no, out var notes ) )
                {
                    continue;
                }

				diff_x = measure_size * measure_no - sheet_pos.X;

				foreach ( var note in notes )
				{
					note.DrawPredict( aGraphic, diff_x, -sheet_pos.Y );
				}
            }
        }
        #endregion

        #region Bpm
        {
			#region body
			{
				aGraphic.FillRectangle( DrawSet.BpmBodyBrush   , _BpmBodyRange );
				aGraphic.DrawRectangle( DrawSet.HeaderLinePen  , _BpmBodyRange );

				int diff_x;

				for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
				{
					if ( !_BpmList.TryGetValue( measure_no, out var bpms ) )
					{
						continue;
					}

					diff_x = measure_size * measure_no - sheet_pos.X;

					foreach ( var bpm in bpms )
					{
						bpm.Draw( aGraphic, diff_x,	0 );
					}
				}
			}
	        #endregion
		}
        #endregion

        #region NoteRange
        {
            _NoteRange.Draw( aGraphic, note_pos );
        }
        #endregion

        #region MidiMapGroup / MidiMap
        {
            foreach ( var obj in _HeaderList )
            {
                obj.Draw( aGraphic, 0, -sheet_pos.Y );
            }
        }
        #endregion

        #region Bpm header
        {
			#region header
			{
				aGraphic.FillRectangle( DrawSet.BpmHeadBrush, _BpmHeadRange );

				aGraphic.DrawString
                    ( 
                        "Bpm", 
                        DrawSet.StrFont, 
                        DrawSet.StrBrush, 
                        _BpmHeadRange, 
                        DrawSet.StrFmt 
                    );

				aGraphic.DrawRectangle( DrawSet.HeaderLinePen, _BpmHeadRange );
			}
			#endregion
		}
        #endregion

        #region Volume
        if ( DrawSet.VolumeDisplay && _VolumeList.Count != 0 )
        {
			#region body
			{
                aGraphic.FillRectangle( DrawSet.VolumeBodyBrush, _VolumeBodyRange );
                aGraphic.DrawRectangle( DrawSet.HeaderLinePen  , _VolumeBodyRange );

				int diff_x;

				for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
				{
					if ( !_VolumeList.TryGetValue( measure_no, out var notes ) )
					{
						continue;
					}

					diff_x = measure_size * measure_no - sheet_pos.X;

					foreach ( var note in notes )
					{
						note.Draw( aGraphic, diff_x, 0 );
					}
				}
			}
	        #endregion

            #region Volume range
            {
                _VolumeRange.DrawRange( aGraphic, note_pos.X );
			}
            #endregion

            #region Header
            {
				aGraphic.FillRectangle( DrawSet.VolumeHeadBrush, _VolumeHeadRange );

				aGraphic.DrawString
                    ( 
                        "Volume", 
                        DrawSet.StrFont, 
                        DrawSet.StrBrush, 
                        _VolumeHeadRange, 
                        DrawSet.StrFmt 
                    );

				aGraphic.DrawRectangle( DrawSet.HeaderLinePen, _VolumeHeadRange );
			}
			#endregion
		}
        #endregion

        #region Measure number
        {
			#region body
			{
				aGraphic.FillRectangle( DrawSet.MeasureNoBodyBrush, _MeasureNoBodyRange );

				var r = new Rectangle
					(
						_MeasureNoBodyRange.X,
					    _MeasureNoBodyRange.Y,
					    measure_size,
					    _MeasureNoBodyRange.Height
					);

				for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
				{
					r.X = _MeasureNoBodyRange.X + measure_size * measure_no - sheet_pos.X;

					aGraphic.DrawRectangle( DrawSet.HeaderLinePen, r );

					r.X += 5;

					aGraphic.DrawString
						(
							String.Format( "{0:000}", measure_no ),
							DrawSet.StrFont,
							DrawSet.StrBrush,
							r,
							DrawSet.StrFmt
						);
				}
			}
	        #endregion

			#region header
			{
				aGraphic.FillRectangle( DrawSet.MeasureNoHeadBrush, _MeasureNoHeadRange );
				aGraphic.DrawString( "Measure No", DrawSet.StrFont, DrawSet.StrBrush, _MeasureNoHeadRange, DrawSet.StrFmt );
				aGraphic.DrawRectangle( DrawSet.HeaderLinePen, _MeasureNoHeadRange );
			}
			#endregion
		}
        #endregion

        #region Cursor vertical line
        {
            if ( _NoteCursorPosition.X - note_pos.X >= 0 )
            {
                var x = body.X + ( _NoteCursorPosition.X - note_pos.X ) * DrawSet.NoteWidthSize;

                aGraphic.DrawLine
                    (
                        DrawSet.SheetCursorLineV,
                        x,
                        body.Y,
                        x,
                        body.Bottom
                    );
            }
        }
        #endregion

        #region Info
        {
			#region header
			{
				aGraphic.FillRectangle( DrawSet.InfoHeadBrush, _InfoRange );

				aGraphic.DrawString
					(
						$"{DrawSet.NotePosition.X % Config.System.MeasureNoteNumber}:{DrawSet.NotePosition.Y}\n{_ActionState}",
                        DrawSet.StrFont,
						DrawSet.StrBrush,
						_InfoRange,
						DrawSet.StrFmt
					);

                aGraphic.DrawRectangle( DrawSet.HeaderLinePen, _InfoRange );
			}
			#endregion
        }
        #endregion
    }

    /// <summary>
    /// 描画イベント
    /// </summary>
    /// <param name="ev"></param>
    protected override void OnPaint( PaintEventArgs ev )
    {
		try
		{
			OnMove();
			OnDraw( ev.Graphics );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    /// <summary>
    /// リサイズイベント
    /// </summary>
    /// <param name="ev"></param>
    protected override void OnResize( EventArgs ev )
    {
        try
        {
            Config.EventEditerFormResize();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    #endregion
}
