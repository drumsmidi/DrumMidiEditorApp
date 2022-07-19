using System;
using System.Collections.Generic;
using System.Drawing;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pDirectX;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pView.pPlayer.pScore;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class PlayerSurface : PlayerSurfaceBase
{
    #region Member

    /// <summary>
    /// スクリーンサイズ
    /// </summary>
    private Size _ScreenSize = new();

    /// <summary>
    /// セクション範囲
    /// </summary>
	private Rectangle _SectionRange = new();

    /// <summary>
    /// 小節番号行ボディ範囲
    /// </summary>
	private Rectangle _MeasureNoBodyRange = new();

    /// <summary>
    /// ノート行ボディ範囲
    /// </summary>
    private Rectangle _ScoreBodyRange = new();

    /// <summary>
    /// 現在のBPM表示範囲
    /// </summary>
	private Rectangle _NowBpmRange = new();

    /// <summary>
    /// 小節分割線リスト
    /// </summary>
    private readonly List<DmsItemLine> _MeasureLineList = new();

    /// <summary>
    /// MidiMapGroupヘッダリスト（MidiMapGroupキー、横線描画アイテム）
    /// </summary>
    private readonly Dictionary<int, DmsItemLine> _HeaderList = new();

    /// <summary>
    /// NOTEリスト（小節番号、NOTE描画アイテム）
    /// </summary>
    private readonly Dictionary<int,List<DmsItemNote>> _NoteList = new();

    /// <summary>
    /// 小節番号
    /// </summary>
    private DmsItemMeasure? _MeasureNo = null;

    /// <summary>
    /// 現在のBPM
    /// </summary>
    private DmsItemLabel? _NowBpm = null;

    /// <summary>
    /// 現在の再生位置ライン
    /// </summary>
    private DmsItemLine? _NowPosition = null;

    #endregion

    /// <summary>
    /// プレイヤー設定
    /// </summary>
    private static ConfigPlayerScore DrawSet => Config.Player.Score;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aGraphic2D">DirectX2D</param>
    public PlayerSurface( Graphic2D aGraphic2D ) : base( aGraphic2D ) { }

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
        // screen
        _ScreenSize.Height			= Config.Player.ResolutionScreenHeight;
        _ScreenSize.Width			= Config.Player.ResolutionScreenWidth;

        //// bpm
        //_NowBpmRange.X			    = 0;
        //_NowBpmRange.Y			    = 0;
        //_NowBpmRange.Width		    = DrawSet.HeaderWidthSize;
        //_NowBpmRange.Height		    = DrawSet.BpmHeightSize;

        //// measure no body
        //_MeasureNoBodyRange.X		= 20;
        //_MeasureNoBodyRange.Y		= _NowBpmRange.Bottom;
        //_MeasureNoBodyRange.Width	= (int)( ( _ScreenSize.Width - 40 ) / DrawSet.MeasureSize ) * DrawSet.MeasureSize;
        //_MeasureNoBodyRange.Height	= DrawSet.MeasureNoHeightSize;

        //// score body
        //_ScoreBodyRange.X			= _MeasureNoBodyRange.Left;
        //_ScoreBodyRange.Y			= _MeasureNoBodyRange.Bottom;
        //_ScoreBodyRange.Width		= _MeasureNoBodyRange.Width;
        //_ScoreBodyRange.Height		= DrawSet.ScoreMaxHeight;

        // 1小節分の範囲
        _SectionRange.X			    = _MeasureNoBodyRange.X;
        _SectionRange.Y			    = _MeasureNoBodyRange.Y;
        _SectionRange.Width		    = _ScoreBodyRange.Width;
        _SectionRange.Height		= _ScoreBodyRange.Bottom - _MeasureNoBodyRange.Top;
    }

	protected override void UpdateScoreLine()
	{
    //    var body            = _ScoreBodyRange;
    //    int measure_size    = DrawSet.MeasureSize;

    //    #region Now position line

    //    _NowPosition = new
    //        ( 
    //            _Graphic2D, 
    //            body.X, 
    //            body.Top, 
    //            DrawSet.CursolPen.Width, 
    //            body.Height, 
    //            DrawSet.CursolPen.Color 
    //        );

    //    #endregion

    //    #region Measure line
    //    {
    //        var pens = new Pen[]
    //            {
    //                DrawSet.SheetMeasure001Pen,
    //                DrawSet.SheetMeasure004Pen,
    //                DrawSet.SheetMeasure008Pen,
    //                DrawSet.SheetMeasure016Pen,
    //                DrawSet.SheetMeasure032Pen,
    //                DrawSet.SheetMeasure064Pen,
    //                DrawSet.SheetMeasure128Pen,
    //            };

    //        _MeasureLineList.Clear();

    //        int linesize = ( pens[ 0 ].Width == 0 ? 0 : 1 );

    //        if ( linesize == 0 )
    //        {
    //            for ( int i = 1; i < pens.Length; i++ )
    //            {
    //                if ( pens[ i ].Width != 0 ) 
    //                {
    //                    linesize = (int)Math.Pow( 2, i + 1 ); 
    //                    break;
    //                }
    //            }
    //        }

    //        if ( linesize != 0 )
    //        {
    //            Pen? pen = null;

				//int note_num   = Config.System.MeasureNoteNumber;
    //            int note_width = DrawSet.NoteTermWidthSize;

    //            for ( int i = 0; i < note_num; i += linesize )
    //            {
    //                for ( int x = 6, y = 1; x >= 0; x--, y *= 2 )
    //                {
    //                    pen = pens[ x ];

    //                    if ( i % ( note_num / y ) == 0 && ( pen.Width != 0 ) )
    //                    {
    //                        break;
    //                    }
    //                }

    //                if ( pen == null )
    //                {
    //                    continue;
    //                }

    //                _MeasureLineList.Add( new( _Graphic2D, body.X + note_width * i, body.Top, pen.Width, body.Height, pen.Color ) );

    //                pen = null;
    //            }
    //        }
    //    }
    //    #endregion

    //    #region MeasureNo
    //    {
    //        _MeasureNo = new
    //            (
    //                _Graphic2D,
    //                _MeasureNoBodyRange.X,
    //                _MeasureNoBodyRange.Y,
    //                measure_size,
    //                _MeasureNoBodyRange.Height,
    //                DrawSet.MeasureNoGI
    //            );
    //    }
    //    #endregion
    }

    protected override void UpdateScoreHeader()
	{
        _HeaderList.Clear();

        var body = _ScoreBodyRange;

    //    int x = body.X;
    //    int y = body.Y;
    //    int w = body.Width;
    //    int h = DrawSet.NoteTermHeightSize;

    //    #region MidiMapGroup
    //    {
    //        foreach ( var group in DMS.SCORE.EditMidiMapSet.DisplayMidiMapGroups )
    //        { 
    //            if ( group != null )
    //            { 
    //                var obj = new DmsItemLine
				//	    ( 
    //                        this._Graphic2D,
    //                        x,
    //                        y,
    //                        w,
    //                        DrawSet.HeaderGI.ForePen.Width,
				//		    DrawSet.HeaderGI.ForePen.Color
				//	    );

    //                _HeaderList.Add( group.GroupKey, obj );
    //            }

			 //   y += h;
		  //  }
    //    }
    //    #endregion

    //    #region Bpm now
    //    {
    //        _NowBpm = new DmsItemLabel
				//( 
    //                _Graphic2D,
    //                _NowBpmRange.X,
    //                _NowBpmRange.Y,
    //                _NowBpmRange.Width,
    //                _NowBpmRange.Height,
    //                String.Empty,
				//    DrawSet.BpmNowGI
    //            );
    //    }
    //    #endregion
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

        var body_s    = _ScoreBodyRange;
        var note_rect = new RectangleF( 0, 0, DrawSet.NoteWidthSize, DrawSet.NoteHeightSize );

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

                    float volume = 1F;
                    if ( DrawSet.NoteVolumeSizeOn )
                    {
                        volume = (float)( info.Volume + midiMap.VolumeAddIncludeGroup ) / (float)Config.Media.MidiMaxVolume;

                        if ( volume > 1F )
                        {
                            volume = 1F;
                        }
                    }
                    if ( DrawSet.NoteVolumeZeroOn && volume <= 0F )
                    {
                        volume = 1F;
                    }

                    note_rect.X         = body_s.X + info.NotePos * DrawSet.NoteTermWidthSize - ( volume * (float)DrawSet.NoteWidthSize / 2.0F );
                    note_rect.Y         = body_s.Y + idx * DrawSet.NoteTermHeightSize + ( ( (float)DrawSet.NoteTermHeightSize - volume * (float)DrawSet.NoteHeightSize ) / 2.0F );
                    note_rect.Width     = DrawSet.NoteWidthSize  * volume;
                    note_rect.Height    = DrawSet.NoteHeightSize * volume;

                    if ( volume != 0F )
                    {
                        if ( note_rect.Width <= 1F )
                        {
                            note_rect.Width = 2F;
                        }
                        if ( note_rect.Height <= 1F )
                        {
                            note_rect.Height = 2F;
                        }
                    }

     //               var obj = new DmsItemNote
					//	(
     //                       _Graphic2D,
					//		note_rect.X,
					//		note_rect.Y,
					//		note_rect.Width,
					//		note_rect.Height,
     //                       midiMap.Color
     //                   );

					//if ( !_NoteList.TryGetValue( aMeasureNo, out var lst ) )
					//{
					//	lst = new();
					//}
					//lst.Add( obj );

					//_NoteList[ aMeasureNo ] = lst;
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

    public override bool OnDraw()
    {
        if ( !base.OnDraw() )
        {
            return false;
        }

        var section = _SectionRange;

        if ( section.Width <= 0 || section.Height <= 0 )
        {
            return true;
        }

   //     var body            = _ScoreBodyRange;
   //     var note_pos        = _NotePositionX;
   //     int measure_size    = DrawSet.MeasureSize;
   //     int measure_x       = body.Width / measure_size;
   //     int measure_y       = ( _ScreenSize.Height - section.Top ) / section.Height;

   //     if ( measure_x < 0 )
   //     {
   //         measure_x = 0;
   //     }
   //     if ( measure_y < 0 )
   //     {
   //         measure_y = 0;
   //     }

   //     int measure_start   = note_pos / Config.System.MeasureNoteNumber;
   //     int measure_end     = measure_start + measure_x * measure_y - 1;

   //     #region Paint section
   //     {
   //         int     cnt;
			//float   diff_x;
			//float   diff_y;

   //         for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
   //         {
   //             diff_x = measure_size   * (int)( measure_no % measure_x );
   //             diff_y = section.Height * (int)( measure_no / measure_x % measure_y );

   //             #region Paint measure line
   //             { 
   //                 cnt = _MeasureLineList.Count;

			//		for ( int index = 0; index < cnt; index++ )
			//		{
			//		    _MeasureLineList[ index ].Draw( _Graphic2D, diff_x, diff_y );
   //                 }
   //             }
   //             #endregion

   //             #region Paint note
   //             {
   //                 if ( _NoteList.TryGetValue( measure_no, out var notes ) )
   //                 {
			//		    foreach ( var note in notes )
			//		    {
			//			    note.Draw( _Graphic2D, diff_x, diff_y );
   //                     }
   //                 }
   //             }
   //             #endregion

   //             #region Paint horizon line

   //             if ( (int)( measure_no % measure_x ) == 0 )
   //             {
   //                 foreach ( var obj in _HeaderList.Values )
   //                 {
   //                     obj.Draw( _Graphic2D, diff_x, diff_y );
   //                 }
   //             }

   //             #endregion

   //             #region Paint measure number
   //             {
   //                 _MeasureNo?.Draw( _Graphic2D, measure_no, diff_x, diff_y );
			//    }
   //             #endregion
   //         }
   //     }
   //     #endregion

   //     #region Cursol
   //     {
   //         var measure_no = (int)( _SheetPosX / Config.System.MeasureNoteNumber );

   //         var diff_x = measure_size   * (int)( measure_no % measure_x ) + _SheetPosX % Config.System.MeasureNoteNumber * DrawSet.NoteTermWidthSize;
   //         var diff_y = section.Height * (int)( measure_no / measure_x % measure_y );

   //         _NowPosition?.Draw( _Graphic2D, diff_x, diff_y );
   //     }
   //     #endregion

   //     #region Paint now bpm
   //     {
   //         if ( DrawSet.BpmNowDisplay && _NowBpm != null )
   //         { 
   //             _NowBpm.Text = String.Format( "[Bpm:{0, 6:##0.00}]", DmsControl.GetBpm( _NotePositionX ) );
   //             _NowBpm.Draw( _Graphic2D );
   //         }
   //     }
   //     #endregion

        return true;
    }
}
