using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pUtil;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：ノート音量
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aX">描画位置＋１小節内での相対X座標</param>
/// <param name="aBottom">描画位置 底位置</param>
/// <param name="aInfo">NOTE情報</param>
/// <param name="aFormatRect">書式設定</param>
public partial class DmsItemNoteVolume( float aX, float aBottom, InfoNote aInfo, FormatRect aFormatRect ) : DisposeBaseClass
{
    /// <summary>
    /// 描画位置
    /// </summary>
    private readonly float _PosX = aX;

    /// <summary>
    /// 描画 底位置
    /// </summary>
    private float _Bottom = aBottom;

    /// <summary>
    /// NOTE情報
    /// </summary>
    private InfoNote? _NoteInfo = aInfo;

    /// <summary>
    /// 書式設定
    /// </summary>
    private FormatRect? _FormatRect = aFormatRect;

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

        var rect = new Rect
            (
                _PosX + aDiffX - 1,
                _Bottom - _NoteInfo.Volume,
                1,
                _NoteInfo.Volume
            );

        // 音量線描画
        HelperXaml.DrawFormatRectFillRectangle( aGraphics, rect, _FormatRect );

        if ( Config.Editer.VolumeValueDisplay )
        {
            // 音量文字表示（手抜き）
            rect.X      -= 10;
            rect.Y      -= 20;
            rect.Width   = 100;
            rect.Height  = 10;

            HelperXaml.DrawFormatRectText( aGraphics, rect, _FormatRect, $"{_NoteInfo.Volume}" );
        }
    }
}
