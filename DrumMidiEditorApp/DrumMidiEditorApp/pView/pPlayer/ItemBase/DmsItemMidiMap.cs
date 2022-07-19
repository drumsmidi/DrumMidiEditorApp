using Microsoft.Graphics.Canvas;
using System;
using Windows.Foundation;
using Windows.UI;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pPlayer;

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
    public Rect DrawRect { get; private set; } = new();

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = null;

    /// <summary>
    /// ノートヒットカラー
    /// </summary>
    private Color _HitColor = ColorHelper.EmptyColor;

    /// <summary>
    /// ノートヒット表示時間（秒）
    /// </summary>
    private double _HitTime = 0;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aGroup">MidiMapGroup</param>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
    public DmsItemMidiMap( MidiMapGroup aGroup, float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect )
    {
        _MidiMapGroup   = aGroup;
        DrawRect        = new( aX, aY, aWidth, aHeight );
		_FormatRect     = aFormatRect;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aMidiMap">MidiMap</param>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
    public DmsItemMidiMap( MidiMap aMidiMap, float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect )
    {
        _MidiMap    = aMidiMap;
        DrawRect    = new( aX, aY, aWidth, aHeight );
        _FormatRect = aFormatRect;
    }

    protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				_MidiMapGroup   = null;
                _MidiMap        = null;
                _FormatRect     = null;
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
        DrawRect = new 
            (
                DrawRect.X + aMoveX,
                DrawRect.Y + aMoveY,
                DrawRect.Width,
                DrawRect.Height 
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
            _HitColor  = ColorHelper.EmptyColor;
            _HitTime   = 0;
        }
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        if ( _FormatRect == null )
        {
            return;
        }

        var rect = DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

        // 背景色
        aGraphics.FillRectangle
            ( 
                rect,
                Config.Player.Sequence.HeaderEffectOn && _HitColor != ColorHelper.EmptyColor
                    ? _HitColor : _FormatRect.BackColor 
            );

        // テキスト描画
        aGraphics.DrawText
            (
                _MidiMapGroup?.GroupName ?? _MidiMap?.MidiMapName ?? String.Empty,
                rect,
                _FormatRect.TextColor,
                _FormatRect.TextFormat
            );

        // 外枠
        if ( _FormatRect.LineSize > 0 )
        { 
            aGraphics.DrawRectangle
                (
                    rect,
                    _FormatRect.LineColor,
                    _FormatRect.LineSize
                );
        }
    }
}
