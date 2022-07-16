using System;
using System.Drawing;

using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer.pScore;

/// <summary>
/// プレイヤー描画アイテム：ノート
/// </summary>
internal class DmsItemNote : DisposeBaseClass, IComparable, IComparable<DmsItemNote>
{
	/// <summary>
	/// ノート描画範囲
	/// </summary>
    private RectangleF _NoteRect = new();

	/// <summary>
	/// ノート背景色
	/// </summary>
	private readonly Color _NoteColor = new();

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aX">描画位置＋１小節内での相対X座標</param>
	/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aNoteColor">ノート背景色</param>
	public DmsItemNote( Graphic2D aGraphics, float aX, float aY, float aWidth, float aHeight, Color aNoteColor )
    {
        _NoteRect.X			= aX;
        _NoteRect.Y			= aY;
        _NoteRect.Width		= aWidth;
        _NoteRect.Height	= aHeight;
		_NoteColor			= aNoteColor;

		aGraphics.AddBrush( _NoteColor );
	}

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
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
	public void Draw( Graphic2D aGraphics, float aDiffX, float aDiffY )
    {
        var r = new RectangleF( _NoteRect.Location, _NoteRect.Size );
        r.X += aDiffX;
        r.Y += aDiffY;

        aGraphics.FillEllipse( r, _NoteColor );
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
		else if ( _NoteRect.X > aOther._NoteRect.X )
		{
			return 1;
		}
		else if ( _NoteRect.X == aOther._NoteRect.X )
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
