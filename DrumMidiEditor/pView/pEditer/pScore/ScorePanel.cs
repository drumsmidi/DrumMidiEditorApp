using System;
using System.Drawing;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pLog;

namespace DrumMidiEditor.pView.pEditer.pScore;

/// <summary>
/// スコアパネル
/// </summary>
public class ScorePanel : Panel
{   
    /// <summary>
    /// スコアBitmap
    /// </summary>
    private Bitmap? _ScoreBitmap = null;

    /// <summary>
    /// Scoreタブ設定
    /// </summary>
    private static ConfigScore DrawSet => Config.Score;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ScorePanel()
    {
        SetStyle( ControlStyles.ResizeRedraw,           false	);
        SetStyle( ControlStyles.OptimizedDoubleBuffer,  true	);
        SetStyle( ControlStyles.UserPaint,              true	);
        SetStyle( ControlStyles.AllPaintingInWmPaint,   true	);
        SetStyle( ControlStyles.DoubleBuffer,           true	); 
        SetStyle( ControlStyles.Opaque,                 true	);

		SuspendLayout();
		{
			Margin     = new Padding( 0 );
			Padding    = new Padding( 0 );
        }
        ResumeLayout( false );
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    private void OnMove()
    {
        // スコア更新
        if ( DrawSet.UpdateScoreFlag )
        {
            DrawSet.UpdateScoreFlag = false;

            _ScoreBitmap = CreateScoreBitmap();
        }
    }

    /// <summary>
    /// スコアBitmap作成
    /// </summary>
    /// <returns>スコアBitmap</returns>
    private Bitmap? CreateScoreBitmap()
    {
        var m_max = DMS.SCORE.EditChannel.MaxMeasureNo;
        var w_min = Config.System.MeasureNoteNumber;
        var h_min = DMS.SCORE.EditMidiMapSet.DisplayGroupCount;

        if ( w_min * h_min == 0 )
        {
            return null;
        }

        var bmp = new Bitmap( Width, Height );

        // 描画範囲：ノートの半サイズ分 外枠に余白を持たせる
        var body = new Rectangle
            (
                DrawSet.NoteWidthSize,
                DrawSet.NoteHeightSize,
                bmp.Width  - DrawSet.NoteWidthSize  * 2,
                bmp.Height - DrawSet.NoteHeightSize * 2
            );

        #region 縦横 小節表示数 算出

        // 縦横 小節表示可能最大数
        var w_cnt_max = (int)( body.Width  / w_min );
        var h_cnt_max = (int)( body.Height / h_min );

        // 縦横 小節表示数
        int w_cnt = 0;
        int h_cnt = 0;

        for ( int h = 1; h <= h_cnt_max; h++ )
        { 
            for ( int w = 1; w <= w_cnt_max; w++ )
            { 
                if ( w * h >= m_max )
                {
                    w_cnt = w;
                    h_cnt = h;
                    break;
                }
            }

            if ( w_cnt != 0 && h_cnt != 0 )
            {
                break;
            }
        }

        if ( w_cnt == 0 || h_cnt == 0 )
        {
            w_cnt = w_cnt_max;
            h_cnt = h_cnt_max;
        }

        // 縦横 ノート間隔
        var t_w = (int)( w_cnt_max / w_cnt );
        var t_h = (int)( h_cnt_max / h_cnt );

        // 縦横 小節サイズ
        var m_w = w_min * t_w;
        var m_h = h_min * t_h;

        #endregion

        using var g = Graphics.FromImage( bmp );

        g.Clear( DrawSet.SheetColor );

        for ( int measure_no = 0; measure_no <= m_max; measure_no++ )
        {
            #region Set measure line
            {
                g.DrawRectangle
                    (
                        DrawSet.MeasureLinePen,
                        body.X + measure_no % w_cnt * m_w,
                        body.Y + measure_no / w_cnt * m_h,
                        m_w,
                        m_h
                    );
            }
            #endregion

            var measure = DMS.SCORE.EditChannel.GetMeasure( measure_no );

            if ( measure == null )
            {
                continue;
            }

            #region Set note
            {
                var note_rect = new RectangleF();

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

                    var group_index = DMS.SCORE.EditMidiMapSet.GetDisplayMidiMapGroupIndex( midiMap.Group.GroupKey );

                    var brush = new SolidBrush( midiMap.Color );

					foreach ( var info in measure_line.InfoStates.Values )
					{
                        var volume = 1F;
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

                        note_rect.X         = body.X
                                            + measure_no % w_cnt * m_w 
                                            + info.NotePos * t_w 
                                            - ( volume * (float)DrawSet.NoteWidthSize / 2.0F );
                        note_rect.Y         = body.Y
                                            + measure_no / w_cnt * m_h 
                                            + group_index * t_h 
                                            + ( ( (float)t_h - volume * (float)DrawSet.NoteHeightSize ) / 2.0F );
                        note_rect.Width     = DrawSet.NoteWidthSize  * volume;
                        note_rect.Height    = DrawSet.NoteHeightSize * volume;

                        if ( volume != 0F )
                        {
                            if ( note_rect.Width <= 1 )
                            {
                                note_rect.Width = 2;
                            }
                            if ( note_rect.Height <= 1 )
                            {
                                note_rect.Height = 2;
                            }
                        }

                        g.FillEllipse( brush, note_rect );
					}
				}
			}
            #endregion
        }

        return bmp;
    }

    /// <summary>
    /// 描画処理
    /// </summary>
    /// <param name="aGraphic">グラフィック</param>
    public void OnDraw( Graphics aGraphic )
    {
        if ( _ScoreBitmap != null )
        {
            aGraphic.DrawImage( _ScoreBitmap as Image, new Point( 0, 0 ) );
        }
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
}
