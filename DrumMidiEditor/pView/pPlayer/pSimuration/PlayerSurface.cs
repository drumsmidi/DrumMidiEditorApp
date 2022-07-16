using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pControl;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer.pSimuration;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class PlayerSurface : PlayerSurfaceBase
{
    #region Member

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

    /// <summary>
    /// スクリーンサイズ
    /// </summary>
    private Size _ScreenSize = new();

    /// <summary>
    /// スコア範囲
    /// </summary>
    private Rectangle _ScoreRange = new();

    /// <summary>
    /// 現在のBPM表示範囲
    /// </summary>
	private Rectangle _NowBpmRange = new();

    /// <summary>
    /// 現在の小節番号範囲
    /// </summary>
    private Rectangle _NowMeasureNoRange = new();

    /// <summary>
    /// MidiMapGroupヘッダリスト（MidiMapGroupキー、MidiMapGroup描画アイテム）
    /// </summary>
    private readonly Dictionary<int,DmsItemMidiMap> _HeaderList = new();

    /// <summary>
    /// NOTEリスト（小節番号、NOTE描画アイテム）
    /// </summary>
    private readonly Dictionary<int,List<DmsItemNote>> _NoteList = new();

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
    /// プレイヤー設定
    /// </summary>
    private static ConfigPlayerSimuration DrawSet => Config.Player.Simuration;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aGraphic2D">DirectX2D</param>
    public PlayerSurface( Graphic2D aGraphic2D ) : base( aGraphic2D ) { }

    #region Mouse Event

    public override void MouseDown( object sender, MouseEventArgs ev )
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
                        _MousePosBef       = ev.Location;
                        _MoveMidiMapKey    = Config.System.MidiMapKeyNotSelect;

                        foreach ( var header in _HeaderList )
                        {
                            if ( FormUtil.CheckRange( ev.Location, header.Value.Rect ) )
                            {
                                _MoveMidiMapKey = header.Key;
                                break;
                            }
                        }

                        if ( _MoveMidiMapKey != Config.System.MidiMapKeyNotSelect )
                        {
                            _ActionState = EActionState.MoveHeader;
                        }
					}
					break;
			}
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            _ActionState = EActionState.None;
        }
	}

	public override void MouseMove( object sender, MouseEventArgs ev )
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
                            case EActionState.MoveHeader:
                                {
                                    _HeaderList[ _MoveMidiMapKey ].SetMovePosition
                                        ( 
                                            ev.Location.X - _MousePosBef.X, 
                                            ev.Location.Y - _MousePosBef.Y
                                        );

                                    _MousePosBef = ev.Location;
                                }
                                break;
    					}
					}
					break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            _ActionState = EActionState.None;
        }
	}

	public override void MouseUp( object sender, MouseEventArgs ev )
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
                            case EActionState.MoveHeader:
                                {
                                    _HeaderList[ _MoveMidiMapKey ].SetMovePosition
                                        ( 
                                            ev.Location.X - _MousePosBef.X, 
                                            ev.Location.Y - _MousePosBef.Y
                                        );

                                    var dgp = DMS.SCORE.EditMidiMapSet
                                        .GetMidiMapGroupPosition( ConfigPlayer.PlayerSurfaceMode.Simuration, _MoveMidiMapKey );

                                    dgp.X = _HeaderList[ _MoveMidiMapKey ].Rect.X;
                                    dgp.Y = _HeaderList[ _MoveMidiMapKey ].Rect.Y;

                                    _MoveMidiMapKey = Config.System.MidiMapKeyNotSelect;
                                }
                                break;
                        }
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

        #region header of MidiMapGroup
        {
            foreach ( var obj in _HeaderList.Values )
            {
                obj.Move( aFrameTime );
            }
        }
        #endregion

        return true;
    }

    protected override void UpdateScore()
	{
        // screen
        _ScreenSize.Height			= Config.Player.ResolutionScreenHeight;
        _ScreenSize.Width			= Config.Player.ResolutionScreenWidth;

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
            foreach ( var group in DMS.SCORE.EditMidiMapSet.DisplayMidiMapGroups )
			{
                if ( group == null )
                {
                    continue;
                }

                var dgp = DMS.SCORE.EditMidiMapSet
                    .GetMidiMapGroupPosition( ConfigPlayer.PlayerSurfaceMode.Simuration, group.GroupKey );

                var obj = new DmsItemMidiMap
					( 
                        _Graphic2D,
                        group,
                        body.X + dgp.X,
                        body.Y + dgp.Y,
                        DrawSet.HeaderSize,
                        DrawSet.HeaderSize,
						DrawSet.HeaderGI
					);

                _HeaderList.Add( group.GroupKey, obj );
			}
        }
        #endregion

        #region Bpm now
        {
            _NowBpm = new DmsItemLabel
				( 
                    _Graphic2D,
                    _NowBpmRange.X,
                    _NowBpmRange.Y,
                    _NowBpmRange.Width,
                    _NowBpmRange.Height,
                    String.Empty,
					DrawSet.BpmNowGI
                );
        }
        #endregion

        #region MeasureNo
        {
            _NowMeasureNo = new
                (
                    _Graphic2D,
                    _NowMeasureNoRange.X,
                    _NowMeasureNoRange.Y,
                    _NowMeasureNoRange.Width,
                    _NowMeasureNoRange.Height,
                    String.Empty,
                    DrawSet.MeasureNoGI
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

        var measure = DMS.SCORE.EditChannel.GetMeasure( aMeasureNo );

        if ( measure == null )
        {
            return;
        }

        #region Set note
        {
			foreach ( var midiMap in DMS.SCORE.EditMidiMapSet.DisplayMidiMaps )
			{
                if ( midiMap.Group == null )
                {
                    continue;
                }

				if ( !measure.NoteLines.TryGetValue( midiMap.MidiMapKey, out var measure_line ) )
				{
                    continue;
                }

                var idx = DMS.SCORE.EditMidiMapSet.GetDisplayMidiMapGroupIndex( midiMap.Group.GroupKey );

				foreach ( var info in measure_line.InfoStates.Values )
				{
                    if ( !info.NoteOn )
                    {
                        continue;
                    }

                    var volume = (float)( info.Volume + midiMap.VolumeAddIncludeGroup ) / (float)Config.Media.MidiMaxVolume;

                    if ( volume > 1F )
                    {
                        volume = 1F;
                    }
                    else if ( volume < 0F )
                    {
                        volume = 0F;
                    }

                    var obj = new DmsItemNote
                        (
                            _Graphic2D,
                            info.NotePos * DrawSet.NoteTermSize,
                            midiMap.Color,
                            volume,
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

    public override bool OnDraw()
    {
        if ( !base.OnDraw() )
        {
            return false;
        }

        var note_pos        = _NotePositionX;            
        var sheet_pos_x     = (float)Math.Round( _SheetPosX * DrawSet.NoteTermSize, 0 );
        int measure_size    = DrawSet.MeasureSize;
        int measure_start   = note_pos / Config.System.MeasureNoteNumber;
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
					note.Draw( _Graphic2D, diff_x );
                }
            }
        }
        #endregion

        #region Paint header of MidiMapGroup
        {
            foreach ( var obj in _HeaderList.Values )
            {
                obj.Draw( _Graphic2D );
            }
        }
        #endregion

        #region Paint now bpm
        {
            if ( DrawSet.BpmNowDisplay && _NowBpm != null )
            { 
                _NowBpm.Text = String.Format("{0, 6:##0.00}", DmsControl.GetBpm( _NotePositionX ) );
                _NowBpm.Draw( _Graphic2D );
            }
        }
        #endregion

        #region Paint measure number
        {
            if ( DrawSet.MeasureNoDisplay && _NowMeasureNo != null )
            { 
                _NowMeasureNo.Text = String.Format( "{0, 3:##0}", measure_start );
                _NowMeasureNo.Draw( _Graphic2D );
            }
		}
        #endregion

        return true;
    }

    #endregion
}
