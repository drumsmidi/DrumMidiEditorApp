using DrumMidiLibrary.pModel;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;
using System;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pPlayer.pSurface;

/// <summary>
/// プレイヤー描画アイテム：BPM
/// </summary>
internal partial class DmsItemBpm : DisposeBaseClass, IComparable, IComparable<DmsItemBpm>
{
    /// <summary>
    /// 描画範囲
    /// </summary>
    private Rect _DrawRect = new();

    /// <summary>
    /// BPM情報
    /// </summary>
    private InfoBpm? _Info = null;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aX">描画位置＋１小節内での相対X座標</param>
    /// <param name="aY">描画位置＋１小節内での相対Y座標</param>
    /// <param name="aWidth">横幅</param>
    /// <param name="aHeight">高さ</param>
    /// <param name="aInfo">BPM情報</param>
    /// <param name="aFormatRect">描画書式</param>
    public DmsItemBpm( float aX, float aY, float aWidth, float aHeight, InfoBpm aInfo, FormatRect aFormatRect )
    {
        _DrawRect.X         = aX;
        _DrawRect.Y         = aY;
        _DrawRect.Width     = aWidth;
        _DrawRect.Height    = aHeight;
        _Info               = aInfo;
        _FormatRect         = aFormatRect;
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _Info       = null;
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
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    public void Draw( CanvasDrawingSession aGraphics )
        => Draw( aGraphics, 0, 0 );

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        var rect = _DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

        HelperWin2D.DrawFormatRect( aGraphics, rect, _FormatRect, _Info?.Bpm.ToString() ?? string.Empty );
    }

    /// <summary>
    /// BPM描画順 並替用
    /// </summary>
    /// <param name="aOther"></param>
    /// <returns></returns>
    public int CompareTo( DmsItemBpm? aOther )
    {
        if ( aOther == null )
        {
            return 1;
        }
        else if ( _DrawRect.X > aOther._DrawRect.X )
        {
            return 1;
        }
        else if ( _DrawRect.X == aOther._DrawRect.X )
        {
            return 0;
        }

        return -1;
    }

    /// <summary>
    /// BPM描画順 並替用
    /// </summary>
    /// <param name="aOther"></param>
    /// <returns></returns>
    public int CompareTo( object? aOther )
    {
        return aOther == null
            ? 1
            : GetType() != aOther.GetType()
            ? throw new ArgumentException( "Invalid aOther", nameof( aOther ) )
            : CompareTo( aOther as DmsItemBpm );
    }
}
