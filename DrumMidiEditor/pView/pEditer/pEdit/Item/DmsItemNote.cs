using System;
using System.Drawing;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pEditer.pEdit;

/// <summary>
/// エディター描画アイテム：ノート
/// </summary>
public class DmsItemNote : DisposeBaseClass, IComparable, IComparable<DmsItemNote>
{
	/// <summary>
	/// 描画範囲
	/// </summary>
	public Rectangle NoteRect { get; private set; } = new();

	/// <summary>
	/// NOTE情報
	/// </summary>
	public InfoNote? InfoNote { get; private set; } = null;

	/// <summary>
	/// 背景色
	/// </summary>
	private SolidBrush? _Brush = null;

	/// <summary>
	/// NoteOn-Off間の距離（ノートON右端⇔ノートON左端）
	/// </summary>
	public int NoteLength { get; set; } = 0;

	/// <summary>
	/// NoteOffに対応するNoteOnアイテム
	/// </summary>
	public DmsItemNote? NoteOnItem { get; set; } = null;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="aX">描画位置＋１小節内での相対X座標</param>
	/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aInfo">NOTE情報</param>
	/// <param name="aBrush">NOTE背景色</param>
	public DmsItemNote( int aX, int aY, int aWidth, int aHeight, InfoNote aInfo, SolidBrush aBrush )
    {
        NoteRect	= new( aX, aY, aWidth, aHeight );
        InfoNote	= aInfo;
        _Brush		= aBrush;
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				InfoNote	= null;
				_Brush		= null;
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
        if ( InfoNote == null )
        {
            return;
        }

        var r = new Rectangle( NoteRect.Location, NoteRect.Size );
        r.X  += aDiffX;
		r.Y  += aDiffY;

		if ( InfoNote.NoteOn )
		{ 
			if ( _Brush != null )
			{ 
		        aGraphics.FillEllipse( _Brush, r );

				if ( NoteLength > 0 )
				{
					aGraphics.DrawLine
						( 
							new( _Brush ),
							new( r.Right             , r.Y + r.Height / 2 ),
							new( r.Right + NoteLength, r.Y + r.Height / 2 )
						);
				}
			}
		}

		if ( InfoNote.NoteOff )
		{
			Point[] ps = 
				{
					new( r.X - 1          , r.Y + r.Height / 2 ),
					new( r.X + r.Width / 2, r.Y - 1            ),
					new( r.Right + 1      , r.Y + r.Height / 2 ),
					new( r.X + r.Width / 2, r.Bottom + 1       )
				};

			if ( _Brush != null )
			{ 
				aGraphics.DrawPolygon( new( _Brush ), ps );

				if ( NoteOnItem != null && NoteOnItem.NoteLength >= r.X )
				{
					aGraphics.DrawLine
						( 
							new( _Brush ),
							new( r.Left - NoteOnItem.NoteLength, r.Y + r.Height / 2 ),
							new( r.Left                        , r.Y + r.Height / 2 )
						);
				}
			}
		}

		if ( InfoNote.Selected )
		{
			aGraphics.DrawRectangle( Config.Editer.NoteSelectPen, r );
		}
    }

	/// <summary>
	/// 描画（機械学習 予測結果用）
	/// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aDiffX">描画差分X</param>
	/// <param name="aDiffY">描画差分Y</param>
	public void DrawPredict( Graphics aGraphics, int aDiffX, int aDiffY )
    {
        if ( InfoNote == null )
        {
            return;
        }

        var r = new Rectangle( NoteRect.Location - NoteRect.Size / 2, NoteRect.Size * 2 );
        r.X  += aDiffX;
		r.Y  += aDiffY;

        aGraphics.DrawEllipse( Config.Editer.NotePredictPen, r );
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
		else if ( NoteRect.X > aOther.NoteRect.X )
		{
			return 1;
		}
		else if ( NoteRect.X == aOther.NoteRect.X )
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
