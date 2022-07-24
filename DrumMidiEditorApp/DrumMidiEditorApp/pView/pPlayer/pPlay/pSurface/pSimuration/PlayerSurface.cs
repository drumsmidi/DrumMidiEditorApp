using System;
using System.Collections.Generic;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pDirectX;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pPlayer.pPlay.pSurface.pSimuration;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class PlayerSurface : PlayerSurfaceBase
{
    #region Member

    /// <summary>
    /// プレイヤー設定
    /// </summary>
    private static ConfigPlayerSimuration DrawSet => Config.Player.Simuration;

    /// <summary>
    /// スコア範囲
    /// </summary>
    private Rect _ScoreRange = new();

    /// <summary>
    /// 現在のBPM表示範囲
    /// </summary>
	private Rect _NowBpmRange = new();

    /// <summary>
    /// 現在の小節番号範囲
    /// </summary>
    private Rect _NowMeasureNoRange = new();

    /// <summary>
    /// MidiMapGroupヘッダリスト（MidiMapGroupキー、MidiMapGroup描画アイテム）
    /// </summary>
    private readonly Dictionary<int,DmsItemMidiMap> _HeaderList = new();

    /// <summary>
    /// NOTEリスト（小節番号、NOTE描画アイテム）
    /// </summary>
    private readonly Dictionary<int,List<DmsItemNote>> _NoteList = new();

    /// <summary>
    /// ノート背景色リスト＜MidiMapKey、背景色＞
    /// </summary>
    private readonly Dictionary<int, FormatRect> _MidiMapNoteFormatList = new();

    /// <summary>
    /// 小節番号
    /// </summary>
    private DmsItemLabel? _NowMeasureNo = null;

    /// <summary>
    /// 現在のBPM
    /// </summary>
    private DmsItemLabel? _NowBpm = null;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PlayerSurface() : base() { }

    #region Mouse Event

    /// <summary>
    /// マウスアクション
    /// </summary>
    private enum EActionState
    {
        None = 0,
        MoveHeader,
    }

    /// <summary>
    /// マウスアクション
    /// </summary>
    private EActionState _ActionState = EActionState.None;

    /// <summary>
    /// 移動中のMidiMapキー
    /// </summary>
    private int _MoveMidiMapKey = Config.System.MidiMapKeyNotSelect;

    /// <summary>
    /// マウス移動前座標
    /// </summary>
    private Point _MousePosBef = new();

    public override void MouseDown( object sender, PointerRoutedEventArgs args )
	{
        if ( _ActionState != EActionState.None )
        {
            return;
        }

        try
        {
            var p = args.GetCurrentPoint( sender as FrameworkElement );

            if ( p.Properties.IsLeftButtonPressed)
			{
                _MousePosBef       = p.Position;
                _MoveMidiMapKey    = ConfigSystem.MidiMapKeyNotSelect;

                foreach ( var header in _HeaderList )
                {
                    if ( XamlHelper.CheckRange( p.Position, header.Value.DrawRect ) )
                    {
                        _MoveMidiMapKey = header.Key;
                        break;
                    }
                }

                if ( _MoveMidiMapKey != ConfigSystem.MidiMapKeyNotSelect )
                {
                    _ActionState = EActionState.MoveHeader;
                }
			}
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            _ActionState = EActionState.None;
        }
	}

	public override void MouseMove( object sender, PointerRoutedEventArgs args )
	{
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
            var p = args.GetCurrentPoint( sender as FrameworkElement );

            if ( p.Properties.IsLeftButtonPressed)
			{
				switch ( _ActionState )
				{
                    case EActionState.MoveHeader:
                        {
                            _HeaderList[ _MoveMidiMapKey ].SetMovePosition
                                ( 
                                    (float)( p.Position.X - _MousePosBef.X ),
                                    (float)( p.Position.Y - _MousePosBef.Y )
                                );

                            _MousePosBef = p.Position;
                        }
                        break;
    			}
			}
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            _ActionState = EActionState.None;
        }
	}

	public override void MouseUp( object sender, PointerRoutedEventArgs args )
	{
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
            var p = args.GetCurrentPoint( sender as FrameworkElement );

            switch ( _ActionState )
            {
                case EActionState.MoveHeader:
                    {
                        _HeaderList[ _MoveMidiMapKey ].SetMovePosition
                            ( 
                                (float)( p.Position.X - _MousePosBef.X ),
                                (float)( p.Position.Y - _MousePosBef.Y )
                            );

                        var dgp = Score.EditMidiMapSet
                            .GetMidiMapGroupPosition( ConfigPlayer.PlayerSurfaceMode.Simuration, _MoveMidiMapKey );

                        dgp.X = _HeaderList[ _MoveMidiMapKey ].DrawRect._x;
                        dgp.Y = _HeaderList[ _MoveMidiMapKey ].DrawRect._y;

                        _MoveMidiMapKey = ConfigSystem.MidiMapKeyNotSelect;
                    }
                    break;
            }
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

    #region Update

    public override bool OnMove( double aFrameTime )
    {
        if ( !base.OnMove( aFrameTime ) )
        {
            return false;
        }

        return true;
    }

    protected override void UpdateScore()
	{
        base.UpdateScore();

        // bpm
        _NowBpmRange.X			    = 0;
        _NowBpmRange.Y			    = 0;
        _NowBpmRange.Width		    = DrawSet.BpmNowWidthSize;
        _NowBpmRange.Height		    = DrawSet.BpmNowHeightSize;

		// measure no
        _NowMeasureNoRange.X		= _ScreenSize.Width - DrawSet.MeasureNoWidthSize;
        _NowMeasureNoRange.Y		= 0;
        _NowMeasureNoRange.Width	= DrawSet.MeasureNoWidthSize;
        _NowMeasureNoRange.Height	= DrawSet.MeasureNoHeightSize;

        // score
        _ScoreRange.X			    = 0;
        _ScoreRange.Y			    = 0;
        _ScoreRange.Width		    = _ScreenSize.Width;
        _ScoreRange.Height		    = _ScreenSize.Height;
    }

    protected override void UpdateScoreHeader()
	{
        _HeaderList.Clear();

        var body = _ScoreRange;

        #region MidiMapGroup
        {
            foreach ( var group in Score.EditMidiMapSet.DisplayMidiMapGroups )
			{
                if ( group == null )
                {
                    continue;
                }

                var dgp = Score.EditMidiMapSet
                    .GetMidiMapGroupPosition( ConfigPlayer.PlayerSurfaceMode.Simuration, group.GroupKey );

                var obj = new DmsItemMidiMap
					( 
                        group,
                        (float)( body.X + dgp.X ),
                        (float)( body.Y + dgp.Y ),
                        DrawSet.HeaderSize,
                        DrawSet.HeaderSize,
						DrawSet.HeaderRect
					);

                _HeaderList.Add( group.GroupKey, obj );
			}
        }
        #endregion

        #region MidiMap
        {
			foreach ( var midiMap in Score.EditMidiMapSet.DisplayMidiMaps )
			{
                // ノート描画用の書式を登録
                if ( !_MidiMapNoteFormatList.ContainsKey( midiMap.MidiMapKey ) )
                {
                    // TODO: 線の色とか情報追加が必要
                    var formatRect = new FormatRect
                    {
                        BackColor = midiMap.Color
                    };

                    _MidiMapNoteFormatList.Add( midiMap.MidiMapKey, formatRect );
                }
                else
                {
                    _MidiMapNoteFormatList[ midiMap.MidiMapKey ].BackColor = midiMap.Color;
                }
			}
        }
        #endregion

        #region Bpm now
        {
            _NowBpm = new DmsItemLabel
				( 
                    _NowBpmRange._x,
                    _NowBpmRange._y,
                    _NowBpmRange._width,
                    _NowBpmRange._height,
                    String.Empty,
					DrawSet.BpmNowRect
                );
        }
        #endregion

        #region MeasureNo
        {
            _NowMeasureNo = new
                (
                    _NowMeasureNoRange._x,
                    _NowMeasureNoRange._y,
                    _NowMeasureNoRange._width,
                    _NowMeasureNoRange._height,
                    String.Empty,
                    DrawSet.MeasureNoRect
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
    }

    protected override void UpdateScoreMeasure( int aMeasureNo )
    {
        #region Clear note
        {
            if ( _NoteList.TryGetValue( aMeasureNo, out var nList ) )
            {
                nList.Clear();
                _NoteList.Remove( aMeasureNo );
            }
        }
        #endregion

        var measure = Score.EditChannel.GetMeasure( aMeasureNo );

        if ( measure == null )
        {
            return;
        }

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

                var idx = Score.EditMidiMapSet.GetDisplayMidiMapGroupIndex( midiMap.Group.GroupKey );

				foreach ( var info in measure_line.InfoStates.Values )
				{
                    if ( !info.NoteOn )
                    {
                        continue;
                    }

                    var obj = new DmsItemNote
                        (
                            info.NotePos * DrawSet.NoteTermSize,
                            _MidiMapNoteFormatList[ midiMap.MidiMapKey ],
                            _HeaderList[ midiMap.Group.GroupKey ]
                        );

                    if ( !_NoteList.TryGetValue( aMeasureNo, out var lst ) )
                    {
                        lst = new();
                    }
                    lst.Add( obj );

                    _NoteList[ aMeasureNo ] = lst;
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

    #endregion

    #region Draw

    public override bool OnDraw( CanvasControl sender, CanvasDrawEventArgs args )
    {
        if ( !base.OnDraw( sender, args ) )
        {
            return false;
        }

        var note_pos        = _NotePositionX;            
        var sheet_pos_x     = (float)Math.Round( _SheetPosX * DrawSet.NoteTermSize, 0 );
        var measure_size    = DrawSet.MeasureSize;
        int measure_start   = note_pos / ConfigSystem.MeasureNoteNumber;
        int measure_end     = measure_start + DrawSet.DrawMeasureCount - 1;

        #region Paint note
        {
            float diff_x;

            for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
            {
                if ( !_NoteList.TryGetValue( measure_no, out var notes ) )
                {
                    continue;
                }

				diff_x = measure_size * measure_no - sheet_pos_x;

				foreach ( var note in notes )
				{
					note.Draw( args.DrawingSession, diff_x );
                }
            }
        }
        #endregion

        #region Paint header of MidiMapGroup
        {
            foreach ( var obj in _HeaderList.Values )
            {
                obj.Draw( args.DrawingSession );
            }
        }
        #endregion

        #region Paint now bpm
        {
            if ( DrawSet.BpmNowDisplay && _NowBpm != null )
            { 
                _NowBpm.Text = String.Format("{0, 6:##0.00}", DmsControl.GetBpm( _NotePositionX ) );
                _NowBpm.Draw( args.DrawingSession );
            }
        }
        #endregion

        #region Paint measure number
        {
            if ( DrawSet.MeasureNoDisplay && _NowMeasureNo != null )
            { 
                _NowMeasureNo.Text = String.Format( "{0, 3:##0}", measure_start );
                _NowMeasureNo.Draw( args.DrawingSession );
            }
		}
        #endregion

        return true;
    }

    #endregion
}
