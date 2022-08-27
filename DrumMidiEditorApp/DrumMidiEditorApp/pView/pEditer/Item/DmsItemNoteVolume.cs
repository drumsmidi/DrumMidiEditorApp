using Microsoft.Graphics.Canvas;
using Windows.Foundation;

using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：ノート音量
/// </summary>
public class DmsItemNoteVolume : DisposeBaseClass
{
    /// <summary>
    /// 描画位置
    /// </summary>
    private readonly float _PosX = 0;

    /// <summary>
    /// 描画 底位置
    /// </summary>
    private float _Bottom = 0;

    /// <summary>
    /// NOTE情報
    /// </summary>
    private InfoNote? _NoteInfo = null;

    /// <summary>
    /// 書式設定
    /// </summary>
    private FormatRect? _FormatRect = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aX">描画位置＋１小節内での相対X座標</param>
    /// <param name="aBottom">描画位置 底位置</param>
    /// <param name="aInfo">NOTE情報</param>
    /// <param name="aFormatRect">書式設定</param>
    public DmsItemNoteVolume( float aX, float aBottom, InfoNote aInfo, FormatRect aFormatRect )
    {
        _PosX      		= aX;
        _Bottom         = aBottom;
        _NoteInfo  		= aInfo;
        _FormatRect     = aFormatRect;
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				_NoteInfo   = null;
                _FormatRect = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

			// Note disposing has been done.
			base.Dispose( aDisposing );
		}
	}
    private bool _Disposed = false;

    /// <summary>
    /// 表示位置の調整
    /// </summary>
    /// <param name="aBottom">底位置</param>
    public void Move( float aBottom ) => _Bottom = aBottom;

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX )
    {
        if ( _NoteInfo == null || _FormatRect == null )
        {
            return;
        }

        // TODO: 音量の幅調整が必要かも
        var rect = new Rect
            ( 
                _PosX + aDiffX - 1,
                _Bottom - _NoteInfo.Volume,
                2,
                _NoteInfo.Volume
            );

        // 音量線描画
        aGraphics.FillRectangle( rect, _FormatRect.Background.Color );

        // 音量文字表示（手抜き）
        rect.X		 -=  10;
        rect.Y		 -=  10;
        rect.Width    = 100;
        rect.Height   =  10;

        aGraphics.DrawText
            (
                $"{_NoteInfo.Volume}",
                rect,
                _FormatRect.Text.TextColor.Color,
                _FormatRect.Text.TextFormat
            );
    }
} 
