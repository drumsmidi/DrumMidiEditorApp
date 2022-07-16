using System;
using System.Drawing;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer;

/// <summary>
/// プレイヤー描画アイテム：MidiMapヘッダ
/// </summary>
internal class DmsItemMidiMap : DisposeBaseClass
{
    /// <summary>
    /// 描画対象のMidiMapGroup
    /// </summary>
	private MidiMapGroup? _MidiMapGroup = null;

    /// <summary>
    /// 描画対象のMidiMap
    /// </summary>
	private MidiMap? _MidiMap = null;

    /// <summary>
    /// 描画範囲
    /// </summary>
    public RectangleF Rect { get; private set; } = new();

    /// <summary>
    /// TextFormat検索キー（作りが微妙なので必要に応じて作り変えてください）
    /// </summary>
    private readonly int _TextFormatKey = 3;

    /// <summary>
    /// テキストカラー
    /// </summary>
    private Color _TextColor = new();

    /// <summary>
    /// ラインカラー
    /// </summary>
    private Color _LineColor = new();

    /// <summary>
    /// 背景色
    /// </summary>
    private Color _BackColor = new();

    /// <summary>
    /// 背景色（ノートヒット時）
    /// </summary>
    private Color _HitColor = Color.Empty;

    /// <summary>
    /// ノートヒット表示時間（秒）
    /// </summary>
    private double _HitTime = 0;

    /// <summary>
    /// ラインサイズ
    /// </summary>
    private float _LineWidth = 0;

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aGraphics">グラフィック</param>
    /// <param name="aGroup">MidiMapGroup</param>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aGraphicItem">描画アイテム</param>
    public DmsItemMidiMap( Graphic2D aGraphics, MidiMapGroup aGroup, float aX, float aY, float aWidth, float aHeight, GraphicItem aGraphicItem )
    {
        _MidiMapGroup = aGroup;

        Initialize( aGraphics, aX, aY, aWidth, aHeight, aGraphicItem );
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aGraphics">グラフィック</param>
    /// <param name="aMidiMap">MidiMap</param>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aGraphicItem">描画アイテム</param>
    public DmsItemMidiMap( Graphic2D aGraphics, MidiMap aMidiMap, float aX, float aY, float aWidth, float aHeight, GraphicItem aGraphicItem )
    {
        _MidiMap = aMidiMap;

        Initialize( aGraphics, aX, aY, aWidth, aHeight, aGraphicItem );
    }

    /// <summary>
    /// 初期設定
    /// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aGraphicItem">描画アイテム</param>
    private void Initialize( Graphic2D aGraphics, float aX, float aY, float aWidth, float aHeight, GraphicItem aGraphicItem )
    {
        Rect            = new( aX, aY, aWidth, aHeight );
        _TextColor      = aGraphicItem.FontBrush.Color;
        _LineColor      = aGraphicItem.ForePen.Color;
        _BackColor		= aGraphicItem.BackBrush.Color;
        _LineWidth      = aGraphicItem.ForePen.Width;

        aGraphics.AddBrush( _TextColor );
		aGraphics.AddBrush( _LineColor );
		aGraphics.AddBrush( _BackColor );
		aGraphics.AddTextFormat( _TextFormatKey, aGraphicItem.Font, aGraphicItem.FontFormat );
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				_MidiMapGroup  = null;
                _MidiMap       = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

			// Note disposing has been done.
			base.Dispose( aDisposing );
		}
	}
    private bool _Disposed = false;

    /// <summary>
    /// 位置設定
    /// </summary>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
    public void SetMovePosition( float aMoveX, float aMoveY )
    {
        Rect = new 
            ( 
                Rect.X + aMoveX,
                Rect.Y + aMoveY, 
                Rect.Width, 
                Rect.Height 
            );
    }

    /// <summary>
    /// ノートヒット
    /// </summary>
    /// <param name="aColor">ノート色</param>
    public void Hit( Color aColor )
    {
        _HitColor  = aColor;
        _HitTime   = 0.1d;
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    /// <param name="aFrameTime">フレーム時間（秒）</param>
    public void Move( double aFrameTime )
    {
        _HitTime -= aFrameTime;

        if ( _HitTime < 0 )
        {
            _HitColor  = Color.Empty;
            _HitTime   = 0;
        }
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( Graphic2D aGraphics, int aDiffX, int aDiffY )
    {
        var r = Rect;
        r.X += aDiffX;
        r.Y += aDiffY;

        aGraphics.FillRectangle
            ( 
                r,
                Config.Player.Sequence.HeaderEffectOn && _HitColor != Color.Empty ? _HitColor : _BackColor
            );

        aGraphics.DrawText
            ( 
                _MidiMapGroup?.GroupName ?? _MidiMap?.MidiMapName ?? String.Empty, 
                _TextFormatKey, 
                r, 
                _TextColor 
            );

        if ( _LineWidth > 0F )
        { 
            aGraphics.DrawRectangle( r, _LineWidth, _LineColor );
        }
    }
}
