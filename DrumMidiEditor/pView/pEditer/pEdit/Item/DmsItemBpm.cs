using System;
using System.Drawing;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pEditer.pEdit;

/// <summary>
/// エディター描画アイテム：BPM
/// </summary>
public class DmsItemBpm : DisposeBaseClass, IComparable, IComparable<DmsItemBpm>
{
	/// <summary>
	/// 描画範囲
	/// </summary>
	private Rectangle _Rect = new();

	/// <summary>
	/// BPM情報
	/// </summary>
	private InfoBpm? _Info = null;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="aX">描画位置＋１小節内での相対X座標</param>
	/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aInfo">BPM情報</param>
	public DmsItemBpm( int aX, int aY, int aWidth, int aHeight, InfoBpm aInfo )
    {
        _Rect.X			= aX;
        _Rect.Y			= aY;
        _Rect.Width		= aWidth;
        _Rect.Height	= aHeight;
        _Info			= aInfo;
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				_Info = null;
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
	public void Draw( Graphics aGraphics, int aDiffX, int aDiffY )
    {
        if ( _Info == null )
        {
            return;
        }

        var r = new Rectangle( _Rect.Location, _Rect.Size );
        r.X  += aDiffX - r.Width / 2;
        r.Y  += aDiffY;

        aGraphics.FillRectangle
			( 
				_Info.Selected ? Config.Editer.BpmSelectBrush : Config.Editer.BpmNonSelectBrush, 
				r 
			);

		r.X		+= r.Width + 1;
		r.Width += 100;

		aGraphics.DrawString
			( 
				_Info.Bpm.ToString(), 
				Config.Editer.StrFont,
				Config.Editer.StrBrush, 
				r, 
				Config.Editer.StrFmt
			);
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
		else if ( _Rect.X > aOther._Rect.X )
		{
			return 1;
		}
		else if ( _Rect.X == aOther._Rect.X )
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
		if ( aOther == null )
		{
			return 1;
		}
		if ( GetType() != aOther.GetType() )
		{
            throw new ArgumentException( "Invalid aOther", nameof( aOther ) );
		}
		return CompareTo( aOther as DmsItemBpm );
	}
}
