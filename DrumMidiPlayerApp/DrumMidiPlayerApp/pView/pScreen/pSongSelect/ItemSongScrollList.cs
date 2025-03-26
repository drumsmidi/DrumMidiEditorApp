using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pScreen.pSongSelect;

/// <summary>
/// 描画アイテム：処理中
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aX">描画位置＋１小節内での相対X座標</param>
/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
/// <param name="aRadius">半径</param>
/// <param name="aFormatRect">描画書式</param>
internal partial class ItemSongScrollList() : DisposeBaseClass
{

    /// <summary>
    /// 現在のフレーム時間
    /// </summary>
    private double _CurrentFrameTime = 0.0F;

    /// <summary>
    /// フレーム終了時間
    /// </summary>
    private readonly double _EndFrameTime = 5.0F;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = new() {
        Background = new( HelperColor.GetColor("#AA666666" ) ),
        Line        = new( HelperColor.GetColor("#FF000000" ), 1.0F ),
        Text        = new( HelperColor.GetColor("#FFFFFFFF" ),
                            new()
                            {
                                FontFamily          = Config.Media.DefaultFontFamily,
                                FontSize            = 36F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _FormatRect = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// フレーム処理
    /// </summary>
    /// <param name="aFrameTime"></param>
    public void Move( double aFrameTime )
    {
        // フレーム時間計算
        _CurrentFrameTime += aFrameTime;
        _CurrentFrameTime %= _EndFrameTime;
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    public void Draw( CanvasDrawingSession aGraphics )
    {
        if ( _FormatRect == null )
        {
            return;
        }

        var progress = (float)( _CurrentFrameTime / _EndFrameTime );

        var rect = new Rect( 20, 20, 1000, 60 );

        for ( var i = 0; i < 13; i++ )
        {
            HelperWin2D.DrawFormatRect( aGraphics, rect, _FormatRect, " TEST" );

            rect.Y += rect.Height + 20;
        }
    }
}
