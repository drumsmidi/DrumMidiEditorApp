using System;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pPlayer.pScore;

/// <summary>
/// プレイヤー描画アイテム：ノート
/// </summary>
internal class DmsItemNote : DisposeBaseClass, IComparable, IComparable<DmsItemNote>
{
	/// <summary>
	/// 描画範囲
	/// </summary>
	private Rect _DrawRect = new();

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
	/// <param name="aFormatRect">描画書式</param>
	public DmsItemNote( float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect )
    {
        _DrawRect.X         = aX;
        _DrawRect.Y         = aY;
        _DrawRect.Width     = aWidth;
        _DrawRect.Height    = aHeight;
		_FormatRect			= aFormatRect;
	}

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
	/// 描画
	/// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aDiffX">描画差分X</param>
	/// <param name="aDiffY">描画差分Y</param>
	public void Draw( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        if ( _FormatRect == null )
        {
            return;
        }

		var rect = _DrawRect;
		rect.X += aDiffX;
		rect.Y += aDiffY;

		aGraphics.FillEllipse
			(
				rect._x,
				rect._y,
				rect._width,
				rect._height,
				_FormatRect.BackColor
			);
	}

	/// <summary>
	/// ノート描画順 並替用
	/// </summary>
	/// <param name="aOther"></param>
	/// <returns></returns>
	public int CompareTo( DmsItemNote? aOther )
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
	/// ノート描画順 並替用
	/// </summary>
	/// <param name="aOther"></param>
	/// <returns></returns>
	public int CompareTo( object? aOther )
	{
		if ( aOther == null )
		{
			return 1;
		}
		if ( GetType() != aOther.GetType() )
		{
                throw new ArgumentException( "Invalid aOther", nameof( aOther ) );
		}
		return CompareTo( aOther as DmsItemNote );
	}
}
