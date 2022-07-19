using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pDirectX;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pView.pPlayer.pSequence;

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
    /// ノート行ヘッダ範囲
    /// </summary>
    private Rectangle _ScoreHeadRange = new();

    /// <summary>
    /// ノート行ボディ範囲
    /// </summary>
    private Rectangle _ScoreBodyRange = new();

    /// <summary>
    /// 小節分割線リスト
    /// </summary>
    private readonly List<DmsItemLine> _MeasureLineList = new();

    /// <summary>
    /// MidiMapGroupヘッダリスト（MidiMapGroupキー、MidiMapGroup描画アイテム）
    /// </summary>
    private readonly Dictionary<int,DmsItemMidiMap> _HeaderGroupList = new();

    /// <summary>
    /// MidiMapヘッダリスト（MidiMapキー、MidiMap描画アイテム）
    /// </summary>
    private readonly Dictionary<int, DmsItemMidiMap> _HeaderMidiMapList = new();

    /// <summary>
    /// BPMリスト（小節番号、BPM描画アイテム）
    /// </summary>
    private readonly Dictionary<int,List<DmsItemBpm>> _BpmList = new();

    /// <summary>
    /// NOTEリスト（小節番号、NOTE描画アイテム）
    /// </summary>
    private readonly Dictionary<int,List<DmsItemNote>> _NoteList = new();

    /// <summary>
    /// NOTE-OFFリスト（NOTE描画アイテム、小節番号開始、終了）
    /// </summary>
    private readonly Dictionary<DmsItemNote, Point> _NoteOffList = new();

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
    /// プレイヤー設定
    /// </summary>
    private static ConfigPlayerSequence DrawSet => Config.Player.Sequence;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aGraphic2D">DirectX2D</param>
    public PlayerSurface( Graphic2D aGraphic2D ) 
        : base( aGraphic2D ) 
    { 
    }

	public override bool OnMove( double aFrameTime )
    {
        if ( !base.OnMove( aFrameTime ) )
        {
            return false;
        }

        #region header of MidiMapGroup
        {
            foreach ( var obj in _HeaderGroupList.Values )
            {
                obj.Move( aFrameTime );
            }
        }
        #endregion

        #region header of MidiMap
        {
            foreach ( var obj in _HeaderMidiMapList.Values )
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

		// bpm header
  //      _BpmHeadRange.X			    = 0;
  //      _BpmHeadRange.Y			    = 0;
  //      _BpmHeadRange.Width		    = DrawSet.HeaderTotalWidthSize;
  //      _BpmHeadRange.Height		= DrawSet.BpmHeightSize;

		//// bpm body
  //      _BpmBodyRange.X			    = _BpmHeadRange.Right;
  //      _BpmBodyRange.Y			    = _BpmHeadRange.Top;
  //      _BpmBodyRange.Width		    = _ScreenSize.Width - _BpmHeadRange.Right;
  //      _BpmBodyRange.Height		= _BpmHeadRange.Height;

		//// measure no header
  //      _MeasureNoHeadRange.X		= 0;
  //      _MeasureNoHeadRange.Y		= _BpmBodyRange.Bottom;
  //      _MeasureNoHeadRange.Width	= DrawSet.HeaderTotalWidthSize;
  //      _MeasureNoHeadRange.Height	= DrawSet.MeasureNoHeightSize;

  //      // measure no body
  //      _MeasureNoBodyRange.X		= _MeasureNoHeadRange.Right;
  //      _MeasureNoBodyRange.Y		= _MeasureNoHeadRange.Top;
  //      _MeasureNoBodyRange.Width	= _ScreenSize.Width - _MeasureNoHeadRange.Right;
  //      _MeasureNoBodyRange.Height	= _MeasureNoHeadRange.Height;

		//// score header
  //      _ScoreHeadRange.X			= 0;
  //      _ScoreHeadRange.Y			= _MeasureNoBodyRange.Bottom;
  //      _ScoreHeadRange.Width		= DrawSet.HeaderTotalWidthSize;
  //      _ScoreHeadRange.Height		= DrawSet.ScoreMaxHeight;

  //      // score body
  //      _ScoreBodyRange.X			= _ScoreHeadRange.Right;
  //      _ScoreBodyRange.Y			= _ScoreHeadRange.Top;
  //      _ScoreBodyRange.Width		= _ScreenSize.Width - _ScoreHeadRange.Right;
  //      _ScoreBodyRange.Height		= _ScoreHeadRange.Height;
    }

	protected override void UpdateScoreLine()
	{
    //    var body            = _ScoreBodyRange;
    //    int measure_size    = DrawSet.MeasureSize;

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

    //                _MeasureLineList.Add
    //                    ( 
    //                        new
    //                        ( 
    //                            _Graphic2D, 
    //                            body.X + note_width * i - pen.Width / 2F, 
    //                            body.Top, 
    //                            pen.Width, 
    //                            body.Height, 
    //                            pen.Color 
    //                        ) 
    //                    );

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
        _HeaderGroupList.Clear();
        _HeaderMidiMapList.Clear();

        var head = _ScoreHeadRange;

   //     int x = head.X;
   //     int y = head.Y;
   //     int w = DrawSet.HeaderGroupOn ? head.Width : DrawSet.HeaderGroupWidthSize ;
   //     int h = DrawSet.HeaderGroupOn ? DrawSet.NoteTermHeightSize : 0 ;

   //     #region MidiMapGroup
   //     {
   //         int index = 0;

   //         foreach ( var group in DMS.SCORE.EditMidiMapSet.DisplayMidiMapGroups )
		 //   {
   //             if ( !DrawSet.HeaderGroupOn )
   //             { 
   //                 h = DrawSet.NoteTermHeightSize * DMS.SCORE.EditMidiMapSet.DisplayMidiMapCountByGroup[ index ];

   //                 index++;
   //             }

   //             if ( group != null )
   //             {
   //                 var obj = new DmsItemMidiMap
			//		    ( 
   //                         _Graphic2D,
   //                         group,
   //                         x,
   //                         y,
   //                         w,
   //                         h,
			//			    DrawSet.HeaderGI
			//		    );

   //                 _HeaderGroupList.Add( group.GroupKey, obj );
   //             }

			//	y += h;
		 //   }
   //     }
   //     #endregion

   //     #region MidiMap
   //     if ( !DrawSet.HeaderGroupOn )
   //     {
   //         x = head.X + w;
   //         y = head.Y;
   //         w = DrawSet.HeaderWidthSize;
   //         h = DrawSet.NoteTermHeightSize;

   //         foreach ( var midiMap in DMS.SCORE.EditMidiMapSet.DisplayMidiMaps )
			//{
   //             if ( midiMap != null )
   //             {
   //                 var obj = new DmsItemMidiMap
			//		    ( 
   //                         _Graphic2D,
   //                         midiMap,
   //                         x,
   //                         y,
   //                         w,
   //                         h,
			//			    DrawSet.HeaderGI
			//		    );

   //                 _HeaderMidiMapList.Add( midiMap.MidiMapKey, obj );
   //             }

			//    y += h;
			//}
   //     }
   //     #endregion

   //     #region Bpm now
   //     {
   //         _NowBpm = new DmsItemLabel
			//	( 
   //                 _Graphic2D,
   //                 _BpmHeadRange.X,
   //                 _BpmHeadRange.Y,
   //                 _BpmHeadRange.Width,
   //                 _BpmHeadRange.Height,
   //                 String.Empty,
			//		DrawSet.BpmNowGI
   //             );
   //     }
   //     #endregion
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
                    _NoteOffList.Remove( item );
                }

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

                var idx = DrawSet.HeaderGroupOn ? DMS.SCORE.EditMidiMapSet.GetDisplayMidiMapGroupIndex( midiMap.Group.GroupKey )
                                                : DMS.SCORE.EditMidiMapSet.GetDisplayMidiMapIndex( midiMap.MidiMapKey ) ;

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

                    note_rect.Width  = DrawSet.NoteWidthSize  * volume;
                    note_rect.Height = DrawSet.NoteHeightSize * volume;

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

                    int distanceToNextNoteOff = DMS.SCORE.EditChannel.GetNotePosDistanceToNextNoteOff( info );

                    float distanceToNextNoteOffWidth = distanceToNextNoteOff * DrawSet.NoteTermWidthSize;

                    note_rect.X = info.NotePos * DrawSet.NoteTermWidthSize - note_rect.Width / 2.0F;

                    note_rect.Width += distanceToNextNoteOffWidth;

     //               var obj = new DmsItemNote
					//	(
     //                       _Graphic2D,
					//		note_rect.X,
					//		note_rect.Width,
					//		note_rect.Height,
     //                       midiMap.Color,
     //                       DrawSet.HeaderGroupOn ? _HeaderGroupList[ midiMap.Group.GroupKey ] 
     //                                             : _HeaderMidiMapList[ midiMap.MidiMapKey ]
     //                   );

     //               // NoteOn描画補助
     //               if ( !_NoteList.TryGetValue( aMeasureNo, out var lst ) )
					//{
					//	lst = new();
					//}
					//lst.Add( obj );

					//_NoteList[ aMeasureNo ] = lst;

     //               // NoteOff描画補助
     //               int measureMax = ( info.NotePos + distanceToNextNoteOff ) / Config.System.MeasureNoteNumber;

     //               if ( aMeasureNo < measureMax )
     //               { 
					//    _NoteOffList[ obj ] = new( aMeasureNo, measureMax );
     //               }
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
                _BpmList.Remove( aMeasureNo );
            }
        }
        #endregion

   //     var body      = _BpmBodyRange;
   //     var note_rect = new Rectangle( body.X, body.Y, DrawSet.BpmWidthSize, DrawSet.BpmHeightSize );

   //     #region Set bpm
   //     {
   //         var measure = DMS.SCORE.SysChannel.GetMeasure( aMeasureNo );

   //         if ( measure == null )
   //         {
   //             return;
   //         }

   //         var bpm_line = measure.BpmLine;

   //         if ( bpm_line == null )
   //         {
			//    return;
			//}

   //         foreach ( var info in bpm_line.InfoStates.Values )
   //         {
   //             note_rect.X = body.X + info.NotePos * DrawSet.NoteTermWidthSize;

   //             var obj = new DmsItemBpm
   //                 (
   //                     _Graphic2D,
   //                     note_rect.X,
   //                     note_rect.Y,
   //                     note_rect.Width,
   //                     note_rect.Height,
   //                     info,
   //                     DrawSet.BpmGI
   //                 );

   //             if ( !_BpmList.TryGetValue( aMeasureNo, out var lst ) )
   //             {
   //                 lst = new();                
   //             }
   //             lst.Add( obj );

   //             _BpmList[ aMeasureNo ] = lst;
   //         }
   //     }
   //     #endregion

   //     #region Sort
   //     {
			//if ( _BpmList.TryGetValue( aMeasureNo, out var lst ) )
   //         {
   //             lst.Sort();
   //         }
   //     }
   //     #endregion
    }

    public override bool OnDraw()
    {
        if ( !base.OnDraw() )
        {
            return false;
        }

        var head            = _ScoreHeadRange;
        var body            = _ScoreBodyRange;
        var note_pos        = _NotePositionX;            
        var sheet_pos_x     = (float)Math.Round( _SheetPosX * DrawSet.NoteTermWidthSize, 0 );
  //      int measure_size    = DrawSet.MeasureSize;
  //      int measure_start   = ( note_pos - head.Width / DrawSet.NoteTermWidthSize ) / Config.System.MeasureNoteNumber - 1;
  //      int measure_end     = ( note_pos + body.Width / DrawSet.NoteTermWidthSize ) / Config.System.MeasureNoteNumber + 1;

  //      if ( measure_start < 0 )
  //      {
  //          measure_start = 0;
  //      }
  //      if ( measure_end > Config.System.MeasureMaxNumber )
  //      {
  //          measure_end = Config.System.MeasureMaxNumber;
  //      }

  //      #region Paint measure line
  //      {
  //          int     cnt;
		//	float   diff_x;

  //          for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
  //          {
		//		diff_x = measure_size * measure_no - sheet_pos_x;
		//		cnt    = _MeasureLineList.Count;

		//		for ( int index = 0; index < cnt; index++ )
		//		{
		//			_MeasureLineList[ index ].Draw( _Graphic2D, diff_x, 0 );
  //              }
  //          }
  //      }
  //      #endregion

  //      #region Paint note
  //      {
  //          float diff_x;

  //          for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
  //          {
  //              if ( !_NoteList.TryGetValue( measure_no, out var notes ) )
  //              {
  //                  continue;
  //              }

		//		diff_x = measure_size * measure_no - sheet_pos_x;

		//		foreach ( var note in notes )
		//		{
		//			note.Draw( _Graphic2D, diff_x, 0 );
  //              }
  //          }

  //          // 描画範囲外のノート描画
  //          foreach ( var item in _NoteOffList.Where( c => c.Value.X < measure_start && measure_start <= c.Value.Y ) )
  //          {
		//		diff_x = measure_size * item.Value.X - sheet_pos_x;

  //  			item.Key.Draw( _Graphic2D, diff_x, 0 );
  //          }
  //      }
  //      #endregion

  //      #region Paint header of MidiMapGroup / MidiMap
  //      {
  //          foreach ( var obj in _HeaderGroupList.Values )
  //          {
  //              obj.Draw( _Graphic2D, 0, 0 );
  //          }

  //          foreach ( var obj in _HeaderMidiMapList.Values )
  //          {
  //              obj.Draw( _Graphic2D, 0, 0 );
  //          }
  //      }
  //      #endregion

  //      #region Paint bpm
  //      {
  //          #region Paint body
  //          {
  //              float diff_x;

		//			for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
		//			{
		//				if ( !_BpmList.TryGetValue( measure_no, out var bpms ) )
		//				{
		//					continue;
		//				}

		//				diff_x = measure_size * measure_no - sheet_pos_x;

		//				foreach ( var bpm in bpms )
		//				{
		//					bpm.Draw( _Graphic2D, diff_x,	0 );
		//				}
		//			}
		//		}
  //          #endregion

  //          #region Paint bpm now
  //          {
  //              if ( DrawSet.BpmNowDisplay && _NowBpm != null )
  //              { 
  //                  _NowBpm.Text = String.Format( "[Bpm:{0, 6:##0.00}]", DmsControl.GetBpm( _NotePositionX ) );
  //                  _NowBpm.Draw( _Graphic2D );
  //              }
  //          }
  //          #endregion
  //      }
  //      #endregion

  //      #region Paint measure number
  //      {
  //          #region Paint body
  //          {
  //              float diff_x;

  //              for ( int measure_no = measure_start; measure_no <= measure_end; measure_no++ )
		//		{
  //                  diff_x = measure_size * measure_no - sheet_pos_x;

  //                  _MeasureNo?.Draw( _Graphic2D, measure_no, diff_x, 0 );
		//		}
		//	}
  //          #endregion
		//}
  //      #endregion

        return true;
    }
}
