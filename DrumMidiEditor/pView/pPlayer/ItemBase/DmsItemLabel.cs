using System;
using System.Drawing;

using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer;

/// <summary>
/// プレイヤー描画アイテム：ラベル
/// </summary>
internal class DmsItemLabel : DisposeBaseClass
{
    /// <summary>
    /// 描画範囲
    /// </summary>
    private RectangleF _Rect = new();

    /// <summary>
    /// 描画テキスト
    /// </summary>
    public string Text { get; set; } = String.Empty;

    /// <summary>
    /// TextFormat検索キー（作りが微妙なので必要に応じて作り変えてください）
    /// </summary>
	private readonly int _TextFormatKey = 4;

    /// <summary>
    /// テキストカラー
    /// </summary>
    private readonly Color _TextColor = new();

    /// <summary>
    /// ラインカラー
    /// </summary>
    private readonly Color _LineColor = new();

    /// <summary>
    /// 背景色
    /// </summary>
    private readonly Color _BackColor = new();

    /// <summary>
    /// ラインサイズ
    /// </summary>
    private readonly float _LineWidth = 0;

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
    /// <param name="aText">描画テキスト</param>
	/// <param name="aGraphicItem">描画アイテム</param>
    public DmsItemLabel( Graphic2D aGraphics, float aX, float aY, float aWidth, float aHeight, string aText, GraphicItem aGraphicItem )
    {
        _Rect.X         = aX;
        _Rect.Y         = aY;
        _Rect.Width     = aWidth;
        _Rect.Height    = aHeight;
        Text		    = aText;
        _TextColor      = aGraphicItem.FontBrush.Color;
        _LineColor      = aGraphicItem.ForePen.Color;
        _BackColor		= aGraphicItem.BackBrush.Color;
        _LineWidth      = aGraphicItem.ForePen.Width;

        aGraphics.AddBrush( _TextColor );
		aGraphics.AddBrush( _LineColor );
		aGraphics.AddBrush( _BackColor );
		aGraphics.AddTextFormat( _TextFormatKey, aGraphicItem.Font, aGraphicItem.FontFormat );
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    public void Draw( Graphic2D aGraphics )
    {
        aGraphics.FillRectangle( _Rect, _BackColor );

        aGraphics.DrawText
            ( 
                Text ?? String.Empty, 
                _TextFormatKey, 
                _Rect, 
                _TextColor 
            );

        if ( _LineWidth > 0F )
        { 
            aGraphics.DrawRectangle( _Rect, _LineWidth, _LineColor );
        }
    }
}
