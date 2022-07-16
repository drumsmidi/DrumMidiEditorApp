using System;
using System.Drawing;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer.pSimuration;

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
	/// ノート背景色
	/// </summary>
	private readonly Color _NoteColor = new();

	/// <summary>
	/// ヒット用背景色
	/// </summary>
	private readonly Color _HitColor = new();

    /// <summary>
    /// ノート音量(0-1)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0052:読み取られていないプライベート メンバーを削除", Justification = "<保留中>" )]
    private readonly float _Volume = 0;

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
	/// <param name="aNoteColor">ノート背景色</param>
	/// <param name="aVolume">ノート音量(0-1)</param>
	/// <param name="aDmsItemMidiMap">ヘッダアイテム</param>
	public DmsItemNote( Graphic2D aGraphics, float aNotePosX, Color aNoteColor, float aVolume, DmsItemMidiMap aDmsItemMidiMap )
    {
        _NotePosX		= aNotePosX;
		_NoteColor		= aNoteColor;
		_HitColor		= Color.FromArgb( Config.Player.Simuration.HeaderGI.BackBrush.Color.A, _NoteColor );
		_Volume			= aVolume;
		_DmsItemMidiMap	= aDmsItemMidiMap;

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
	public void Draw( Graphic2D aGraphics, float aDiffX )
    {
		var distance = ( _NotePosX + aDiffX ) / 10F;

		if ( !_HitFlag && distance <= 0F )
		{
			_DmsItemMidiMap?.Hit( _HitColor );

			_HitFlag = true;
		}

		if ( distance < 0F && 9F < distance )
		{
			return;
		}

		// 1:0, 10:1
		var sa = 1F + (float)Math.Log10( distance + 1 );

		if ( _DmsItemMidiMap != null )
		{ 
			var r = _DmsItemMidiMap.Rect;
			r.Width		*= sa;
			r.Height	*= sa;
			r.X			-= ( r.Width  - _DmsItemMidiMap.Rect.Width  ) / 2.0F;
			r.Y			-= ( r.Height - _DmsItemMidiMap.Rect.Height ) / 2.0F;

			aGraphics.DrawEllipse( r, _NoteColor );
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
