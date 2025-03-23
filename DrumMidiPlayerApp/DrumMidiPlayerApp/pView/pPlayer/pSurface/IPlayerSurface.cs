using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace DrumMidiPlayerApp.pView.pPlayer.pSurface;

/// <summary>
/// プレイヤーサーフェイスインタフェース
/// </summary>
internal interface IPlayerSurface
{
    /// <summary>
    /// マウスダウンイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void MouseDown( object sender, PointerRoutedEventArgs args );

    /// <summary>
    /// マウス移動イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void MouseMove( object sender, PointerRoutedEventArgs args );

    /// <summary>
    /// マウスアップイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void MouseUp( object sender, PointerRoutedEventArgs args );

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
    bool OnDraw( CanvasDrawEventArgs args );

    /// <summary>
    /// 1ページ当たりの小節描画数取得
    /// </summary>
    /// <returns>1ページ当たりの小節描画数</returns>
    int GetNumberOfMeasureNoPerPage();
}
