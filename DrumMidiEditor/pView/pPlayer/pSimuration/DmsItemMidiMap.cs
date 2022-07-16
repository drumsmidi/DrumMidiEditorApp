using System;
using System.Drawing;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer.pSimuration;

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
    private readonly Color _TextColor = new();

    /// <summary>
    /// ラインカラー
    /// </summary>
    private readonly Color _LineColor = new();

    /// <summary>
    /// 背景色（ノートヒット時）
    /// </summary>
    private Color _HitColor = Color.Empty;

    /// <summary>
    /// ノートヒット表示時間（秒）
    /// </summary>
    private double _HitTime = 0;

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
        _MidiMapGroup  = aGroup;
        Rect           = new RectangleF( aX, aY, aWidth, aHeight );
        _TextColor     = aGraphicItem.FontBrush.Color;
        _LineColor     = aGraphicItem.ForePen.Color;

        aGraphics.AddBrush( _TextColor );
		aGraphics.AddBrush( _LineColor );
		aGraphics.AddTextFormat( _TextFormatKey, aGraphicItem.Font, aGraphicItem.FontFormat );
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				_MidiMapGroup = null;
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
    /// <param name="aFrameTime"></param>
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
    public void Draw( Graphic2D aGraphics )
    {
        if ( Config.Player.Simuration.HeaderStrOn && _HitColor != Color.Empty )
        { 
            aGraphics.FillEllipse( Rect, _HitColor );
        }

        if ( Config.Player.Simuration.HeaderStrOn )
        { 
            aGraphics.DrawText
                ( 
                    _MidiMapGroup?.GroupName ?? String.Empty, 
                    _TextFormatKey,
                    Rect, 
                    _TextColor 
                );
        }

        aGraphics.DrawEllipse( Rect, _LineColor );
    }
}
