using Microsoft.Graphics.Canvas;
using System;

using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pPlayer.pSimuration;

/// <summary>
/// プレイヤー描画アイテム：ノート
/// </summary>
internal class DmsItemNote : DisposeBaseClass, IComparable, IComparable<DmsItemNote>
{
	/// <summary>
	/// １小節内のノート描画位置X座標
	/// </summary>
	private readonly float _NotePosX = 0;

	/// <summary>
	/// 描画書式
	/// </summary>
	private FormatRect? _FormatRect = null;

	/// <summary>
	/// MidiMapヘッダアイテム
	/// </summary>
	private DmsItemMidiMap? _DmsItemMidiMap = null;

	/// <summary>
	/// ノートヒットフラグ
	/// </summary>
	private bool _HitFlag = false;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="aNotePosX">１小節内のノート描画位置X座標</param>
	/// <param name="aFormatRect">描画書式</param>
	/// <param name="aDmsItemMidiMap">ヘッダアイテム</param>
	public DmsItemNote( float aNotePosX, FormatRect aFormatRect, DmsItemMidiMap aDmsItemMidiMap )
    {
        _NotePosX		= aNotePosX;
		_FormatRect		= aFormatRect;
		_DmsItemMidiMap	= aDmsItemMidiMap;
	}

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				_FormatRect		= null;
				_DmsItemMidiMap = null;
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
	public void Draw( CanvasDrawingSession aGraphics, float aDiffX )
    {
		if ( _DmsItemMidiMap == null || _FormatRect == null )
        {
			return;
        }

		var distance = ( _NotePosX + aDiffX ) / 10F;

		if ( !_HitFlag && distance <= 0F )
		{
			_DmsItemMidiMap.Hit( _FormatRect.BackColor );

			_HitFlag = true;
		}

		if ( distance < 0F && 9F < distance )
		{
			return;
		}

		// 1:0, 10:1
		var sa = 1F + (float)Math.Log10( distance + 1 );

		var rect = _DmsItemMidiMap.DrawRect;
		rect.Width	*= sa;
		rect.Height	*= sa;
		rect.X		-= ( rect.Width  - _DmsItemMidiMap.DrawRect.Width  ) / 2.0F;
		rect.Y		-= ( rect.Height - _DmsItemMidiMap.DrawRect.Height ) / 2.0F;

        // 背景色
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
		else if ( _NotePosX > aOther._NotePosX )
		{
			return 1;
		}
		else if ( _NotePosX == aOther._NotePosX )
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
