using System;
using System.Drawing;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer.pSequence;

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
	/// コンストラクタ
	/// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aNotePosX">１小節内のノート描画位置X座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aNoteColor">ノート背景色</param>
	/// <param name="aDmsItemMidiMap">MidiMap描画アイテム</param>
	public DmsItemNote( Graphic2D aGraphics, float aNotePosX, float aWidth, float aHeight, Color aNoteColor, DmsItemMidiMap aDmsItemMidiMap )
    {
        _NotePosX			= aNotePosX;
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
				_DmsItemMidiMap.Rect.Right, 
				_DmsItemMidiMap.Rect.Top, 
				_NoteSize.Width, 
				_NoteSize.Height 
			);

		r.X	+= aDiffX + _NotePosX;
		r.Y	+= ( _DmsItemMidiMap.Rect.Height - r.Height ) / 2.0F + aDiffY;

		aGraphics.FillRoundedRectangle( r, _NoteColor );

		if ( _DmsItemMidiMap != null && !_HitFlag && r.X <= _DmsItemMidiMap.Rect.Right )
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
