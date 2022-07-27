using Microsoft.Graphics.Canvas;
using System;
using Windows.Foundation;

using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface.pSequenceVertical;

/// <summary>
/// プレイヤー描画アイテム：ノート
/// </summary>
internal class DmsItemNote : DisposeBaseClass, IComparable, IComparable<DmsItemNote>
{
	/// <summary>
	/// １小節内のノート描画位置Y座標
	/// </summary>
	private readonly float _NotePosY = 0;

	/// <summary>
	/// ノート描画範囲
	/// </summary>
	private Size _NoteSize = new();

	/// <summary>
	/// 描画書式
	/// </summary>
	private FormatRect? _FormatRect = null;

	/// <summary>
	/// MidiMapヘッダアイテム
	/// </summary>
	private DmsItemMidiMap? _DmsItemMidiMap = null;

	/// <summary>
	/// NOTE情報
	/// </summary>
	public InfoNote? InfoNote { get; private set; } = null;

	/// <summary>
	/// NoteOn-Off間の距離（ノートON右端⇔ノートON左端）
	/// </summary>
	public float NoteLength { get; set; } = 0;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="aNotePosY">１小節内のノート描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
	/// <param name="aDmsItemMidiMap">MidiMap描画アイテム</param>
	public DmsItemNote( float aNotePosY, float aWidth, float aHeight, FormatRect aFormatRect, DmsItemMidiMap aDmsItemMidiMap )
    {
        _NotePosY			= aNotePosY;
        _NoteSize.Width		= aWidth;
        _NoteSize.Height	= aHeight;
		_FormatRect			= aFormatRect;
		_DmsItemMidiMap		= aDmsItemMidiMap;
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
				InfoNote		= null;
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
		if ( _DmsItemMidiMap == null || _FormatRect == null )
        {
			return;
        }

		var rect = new Rect
			( 
				_DmsItemMidiMap.DrawRect.Left, 
				_DmsItemMidiMap.DrawRect.Top, 
				_NoteSize._width, 
				_NoteSize._height 
			);

		rect.X	+= ( _DmsItemMidiMap.DrawRect.Width - rect.Width ) / 2.0F + aDiffX;
		rect.Y	+= aDiffY + _NotePosY;

        // 背景色
        aGraphics.FillRoundedRectangle
            (
				rect,
				_FormatRect.RadiusX,
				_FormatRect.RadiusY,
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
		else if ( _NotePosY > aOther._NotePosY )
		{
			return 1;
		}
		else if ( _NotePosY == aOther._NotePosY )
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
