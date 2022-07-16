using System;
using System.Drawing;

using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer;

/// <summary>
/// プレイヤー描画アイテム：BPM
/// </summary>
internal class DmsItemBpm : DisposeBaseClass, IComparable, IComparable<DmsItemBpm>
{
	/// <summary>
	/// BPM描画範囲
	/// </summary>
	private RectangleF _Rect = new();

	/// <summary>
	/// BPM情報
	/// </summary>
	private InfoBpm? _Info = null;

	/// <summary>
	/// TextFormat検索キー（作りが微妙なので必要に応じて作り変えてください）
	/// </summary>
	private readonly int _TextFormatKey	= 2;

	/// <summary>
	/// テキストカラー
	/// </summary>
	private readonly Color _TextColor = new();

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aX">描画位置＋１小節内での相対X座標</param>
	/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aInfo">BPM情報</param>
	/// <param name="aGraphicItem">描画アイテム</param>
	public DmsItemBpm( Graphic2D aGraphics, float aX, float aY, float aWidth, float aHeight, InfoBpm aInfo, GraphicItem aGraphicItem )
    {
        _Rect.X			= aX;
        _Rect.Y			= aY;
        _Rect.Width		= aWidth;
        _Rect.Height	= aHeight;
        _Info			= aInfo;
        _TextColor		= aGraphicItem.FontBrush.Color;

		aGraphics.AddBrush( _TextColor );
		aGraphics.AddTextFormat( _TextFormatKey, aGraphicItem.Font, aGraphicItem.FontFormat );
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
	public void Draw( Graphic2D aGraphics, float aDiffX, float aDiffY )
    {
        var r = new RectangleF( _Rect.Location, _Rect.Size );
        r.X += aDiffX;
        r.Y += aDiffY;

        aGraphics.DrawText
		( 
			_Info?.Bpm.ToString() ?? String.Empty, 
			_TextFormatKey, 
			r, 
			_TextColor 
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
