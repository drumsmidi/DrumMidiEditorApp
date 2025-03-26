using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace DrumMidiPlayerApp.pView.pScreen;

/// <summary>
/// プレイヤーサーフェイスインタフェース
/// </summary>
public interface IScreen
{
    /// <summary>
    /// キーダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    void KeyDown( object aSender, KeyRoutedEventArgs aArgs );

    /// <summary>
    /// キーアップ処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    void KeyUp( object aSender, KeyRoutedEventArgs aArgs );

    /// <summary>
    /// マウスダウンイベント
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    void MouseDown( object aSender, PointerRoutedEventArgs aArgs );

    /// <summary>
    /// マウス移動イベント
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    void MouseMove( object aSender, PointerRoutedEventArgs aArgs );

    /// <summary>
    /// マウスアップイベント
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    void MouseUp( object aSender, PointerRoutedEventArgs aArgs );

    /// <summary>
    /// フレーム処理
    /// </summary>
    /// <param name="aFrameTime">フレーム時間（秒）</param>
    /// <returns>True:処理続行、False:処理中断</returns>
	bool OnMove( double aFrameTime );

    /// <summary>
    /// 描画
    /// </summary>
    /// <returns>True:処理続行、False:処理中断</returns>
    bool OnDraw( CanvasDrawEventArgs aArgs );

    /// <summary>
    /// 親スクリーンのインスタンスを設定
    /// </summary>
    void SetParentScreen( IScreen aScreen );
}
