using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Windows.Foundation;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pEditer.pEdit;

/// <summary>
/// エディター描画アイテム：ノート
/// </summary>
public class DmsItemNote : DisposeBaseClass, IComparable, IComparable<DmsItemNote>
{
	/// <summary>
	/// 描画範囲
	/// </summary>
	public Rect DrawRect { get; private set; } = new();

	/// <summary>
	/// NOTE情報
	/// </summary>
	public InfoNote? InfoNote { get; private set; } = null;

	/// <summary>
	/// 描画書式
	/// </summary>
	private FormatRect? _FormatRect = null;

	/// <summary>
	/// NoteOn-Off間の距離（ノートON右端⇔ノートON左端）
	/// </summary>
	public float NoteLength { get; set; } = 0;

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
	/// <param name="aFormatRect">描画書式</param>
	public DmsItemNote( float aX, float aY, float aWidth, float aHeight, InfoNote aInfo, FormatRect aFormatRect )
    {
		DrawRect	= new( aX, aY, aWidth, aHeight );
        InfoNote	= aInfo;
		_FormatRect = aFormatRect;
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				InfoNote	= null;
				_FormatRect = null;
				NoteOnItem	= null;
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
	public void Draw( CanvasControl sender, CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        if ( InfoNote == null || _FormatRect == null )
        {
            return;
        }

		// 描画範囲の左上の座標基準
		var rect = DrawRect;
		rect.X  += aDiffX;
		rect.Y  += aDiffY;

		if ( InfoNote.NoteOn )
		{ 
			// ノートON描画
		    aGraphics.FillEllipse
				( 
					(float)( rect.X + rect.Width  / 2 ),
					(float)( rect.Y + rect.Height / 2 ),
					rect._width,
					rect._height,
					_FormatRect.BackColor
				);

			// ノートON-OFF間の線を描画
			if ( NoteLength > 0 && _FormatRect.LineSize > 0 )
			{
				aGraphics.DrawLine
					(
						(float)( rect.Right ),
						(float)( rect.Y + rect.Height / 2F ),
						(float)( rect.Right + NoteLength ),
						(float)( rect.Y + rect.Height / 2F ),
						_FormatRect.LineColor,
						_FormatRect.LineSize
					);
			}
		}

		if ( !InfoNote.NoteOn && InfoNote.NoteOff )
		{
            Vector2[] ps =
                {
                    new( rect._x							, (float)( rect.Y + rect.Height / 2 )	),
                    new( (float)( rect.X + rect.Width / 2 )	, rect._y								),
                    new( (float)( rect.Right )				, (float)( rect.Y + rect.Height / 2 )	),
                    new( (float)( rect.X + rect.Width / 2 ) , (float)( rect.Bottom )				)
                };

			// ノートOFF描画
			aGraphics.DrawGeometry
					(
						CanvasGeometry.CreatePolygon( sender, ps ),
						_FormatRect.LineColor,
						_FormatRect.LineSize
					);
		}

		// ノートON-OFF間の線を描画
		if ( NoteOnItem != null )
        {
            aGraphics.DrawLine
                ( 
                	(float)( rect.X - NoteOnItem.NoteLength ), 
					(float)( rect.Y + rect.Height / 2 ),
                	(float)( rect.X ), 
					(float)( rect.Y + rect.Height / 2 ),
					_FormatRect.LineColor,
					_FormatRect.LineSize
				);
        }

		if ( InfoNote.Selected )
		{
			var format = Config.Editer.NoteSelectLine;

			rect.X -= rect.Width  / 2;
			rect.Y -= rect.Height / 2;

			// ノート選択時の外枠を描画
			aGraphics.DrawRectangle
				( 
					rect,
					format.LineColor,
					format.LineSize
				);
		}
    }

	/// <summary>
	/// 描画（機械学習 予測結果用）
	/// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aDiffX">描画差分X</param>
	/// <param name="aDiffY">描画差分Y</param>
	public void DrawPredict( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        if ( InfoNote == null )
        {
            return;
        }

		var format = Config.Editer.NotePredictLine;

		var rect = new Rect
			(
				DrawRect.X + aDiffX,
				DrawRect.Y + aDiffY,
				DrawRect.Width  * 2d,
				DrawRect.Height * 2d
			);

		aGraphics.DrawEllipse
			( 
				rect._x,
				rect._y,
				rect._width,
				rect._height,
				format.LineColor,
				format.LineSize
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
		else if ( DrawRect.X > aOther.DrawRect.X )
		{
			return 1;
		}
		else if ( DrawRect.X == aOther.DrawRect.X )
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
