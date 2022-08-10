using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Foundation;
using Windows.Graphics.DirectX;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pDMS;

namespace DrumMidiEditorApp.pView.pScore;

public sealed partial class UserControlScore : UserControl
{
    #region Member

    /// <summary>
    /// スコア設定
    /// </summary>
    private ConfigScore DrawSet => Config.Score;

    /// <summary>
    /// System設定
    /// </summary>
    private ConfigSystem ConfigSystem => Config.System;

    /// <summary>
    /// Media設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// Score情報
    /// </summary>
    private Score Score => DMS.SCORE;

    /// <summary>
    /// スコアBitmap
    /// </summary>
    private CanvasBitmap? _ScoreBitmap = null;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlScore()
    {
        InitializeComponent();

        ControlAccess.UCScore = this;
    }

    #region Move

    /// <summary>
    /// フレーム処理
    /// </summary>
    private void OnMove()
    {
        try
        {
            // スコア更新
            if ( DrawSet.UpdateScoreFlag )
            {
                DrawSet.UpdateScoreFlag = false;

                _ScoreBitmap?.Dispose();
                _ScoreBitmap = CreateScoreBitmap();
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// スコアBitmap作成
    /// </summary>
    /// <returns>スコアBitmap</returns>
    private CanvasBitmap? CreateScoreBitmap()
    {
        var m_max = Score.EditChannel.MaxMeasureNo;
        var w_min = ConfigSystem.MeasureNoteNumber;
        var h_min = Score.EditMidiMapSet.DisplayGroupCount;

        if ( w_min * h_min == 0 )
        {
            return null;
        }

        // 描画範囲：ノートの半サイズ分 外枠に余白を持たせる
        var body = new Rect
            (
                DrawSet.NoteWidthSize,
                DrawSet.NoteHeightSize,
                ActualSize.X - DrawSet.NoteWidthSize  * 2,
                ActualSize.Y - DrawSet.NoteHeightSize * 2
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
        var t_w = (float)w_cnt_max / w_cnt;
        var t_h = (float)h_cnt_max / h_cnt;

        // 縦横 小節サイズ
        var m_w = w_min * t_w;
        var m_h = h_min * t_h;

        #endregion

        var offscreen = new CanvasRenderTarget
            (
                CanvasDevice.GetSharedDevice(),
                ActualSize.X,
                ActualSize.Y,
                96
            );

        using var g = offscreen.CreateDrawingSession();

        #region 描画処理

        g.Clear( DrawSet.SheetColor.Color );

        for ( int measure_no = 0; measure_no <= m_max; measure_no++ )
        {
            #region Set measure line
            {
                g.DrawRectangle
                    (
                        body._x + measure_no % w_cnt * m_w,
                        body._y + measure_no / w_cnt * m_h,
                        m_w,
                        m_h,
                        DrawSet.MeasureLine.LineColor.Color,
                        DrawSet.MeasureLine.LineSize
                    );
            }
            #endregion

            var measure = Score.EditChannel.GetMeasure( measure_no );

            if ( measure == null )
            {
                continue;
            }

            #region Set note
            {
                var note_rect = new Rect();

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

                    var group_index = Score.EditMidiMapSet.GetDisplayMidiMapGroupIndex( midiMap.Group.GroupKey );

                    foreach ( var info in measure_line.InfoStates.Values )
                    {
                        var volume = 1F;

                        if ( DrawSet.NoteVolumeSizeOn )
                        {
                            volume = (float)( info.Volume + midiMap.VolumeAddIncludeGroup ) / (float)ConfigMedia.MidiMaxVolume;

                            if ( volume > 1F )
                            {
                                volume = 1F;
                            }
                        }
                        if ( DrawSet.NoteVolumeZeroOn && volume <= 0F )
                        {
                            volume = 1F;
                        }

                        note_rect.X         = body.X + measure_no % w_cnt * m_w + info.NotePos * t_w;
                        note_rect.Y         = body.Y + measure_no / w_cnt * m_h + group_index  * t_h;
                        note_rect.Width     = DrawSet.NoteWidthSize  * volume / 2;
                        note_rect.Height    = DrawSet.NoteHeightSize * volume / 2;

                        g.FillEllipse
                            ( 
                                note_rect._x,
                                note_rect._y,
                                note_rect._width,
                                note_rect._height,
                                midiMap.Color 
                            );
                    }
                }
            }
            #endregion
        }

        #endregion

        // Bitmap作成
        return CanvasBitmap.CreateFromBytes
            ( 
                g,
                offscreen.GetPixelBytes( 0, 0, (int)offscreen.SizeInPixels.Width, (int)offscreen.SizeInPixels.Height ),
                (int)offscreen.SizeInPixels.Width,
                (int)offscreen.SizeInPixels.Height,
                DirectXPixelFormat.B8G8R8A8UIntNormalizedSrgb,
                96,
                CanvasAlphaMode.Premultiplied
            );
    }

    #endregion

    #region Draw

    /// <summary>
    /// キャンバス再描画リクエスト
    /// </summary>
    public void Refresh() => _ScoreCanvas.Invalidate();

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ScoreCanvas_Draw( CanvasControl sender, CanvasDrawEventArgs args )
    {
        try
        {
            OnMove();

            // 画像表示
            if ( _ScoreBitmap != null )
            {
                args.DrawingSession.DrawImage( _ScoreBitmap, 0, 0 );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// Win2D アンロード処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void UserControl_Unloaded( object sender, RoutedEventArgs args )
    {
        try
        {
            // Win2D アンロード（ページキャッシュしている場合は処理しない）
            //_ScoreCanvas.RemoveFromVisualTree();
            //_ScoreCanvas = null;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
