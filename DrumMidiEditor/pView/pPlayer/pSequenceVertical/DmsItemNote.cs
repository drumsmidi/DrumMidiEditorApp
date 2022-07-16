using System;
using System.Drawing;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer.pSequenceVertical;

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
	private SizeF _NoteSize = new();

	/// <summary>
	/// ノート背景色
	/// </summary>
	private readonly Color _NoteColor = new();

	/// <summary>
	/// ヒット用背景色
	/// </summary>
	private readonly Color _HitColor = new();

	/// <summary>
	/// MidiMapヘッダアイテム
	/// </summary>
	private DmsItemMidiMap? _DmsItemMidiMap;

	/// <summary>
	/// ノートヒットフラグ
	/// </summary>
	private bool _HitFlag = false;

	/// <summary>
	/// NOTE情報
	/// </summary>
	public InfoNote? InfoNote { get; private set; } = null;

	/// <summary>
	/// NoteOn-Off間の距離（ノートON右端⇔ノートON左端）
	/// </summary>
	public int NoteLength { get; set; } = 0;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aNotePosY">１小節内のノート描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aNoteColor">ノート背景色</param>
	/// <param name="aDmsItemMidiMap">MidiMap描画アイテム</param>
	public DmsItemNote( Graphic2D aGraphics, float aNotePosY, float aWidth, float aHeight, Color aNoteColor, DmsItemMidiMap aDmsItemMidiMap )
    {
        _NotePosY			= aNotePosY;
        _NoteSize.Width		= aWidth;
        _NoteSize.Height	= aHeight;
		_NoteColor			= aNoteColor;
		_HitColor			= Color.FromArgb( Config.Player.Sequence.HeaderGI.BackBrush.Color.A, _NoteColor );
		_DmsItemMidiMap		= aDmsItemMidiMap;

		aGraphics.AddBrush( _NoteColor );
		aGraphics.AddBrush( _HitColor );
	}

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
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
	/// <param name="aDiffY">描画差分Y</param>
	public void Draw( Graphic2D aGraphics, float aDiffX, float aDiffY )
    {
		if ( _DmsItemMidiMap == null )
        {
			return;
        }

		var r = new RectangleF
			(
				_DmsItemMidiMap.Rect.Left,
				_DmsItemMidiMap.Rect.Top,
				_NoteSize.Width,
				_NoteSize.Height
			);

		r.X	+= ( this._DmsItemMidiMap.Rect.Width - r.Width  ) / 2.0F + aDiffX;
		r.Y	+= aDiffY + this._NotePosY;

        aGraphics.FillRoundedRectangle( r, _NoteColor );

		if ( _DmsItemMidiMap != null && !_HitFlag && r.Bottom <= _DmsItemMidiMap.Rect.Top )
        {
			_DmsItemMidiMap.Hit( _HitColor );

			_HitFlag = true;
		}
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
