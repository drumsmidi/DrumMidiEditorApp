using System.Windows.Forms;

namespace DrumMidiEditor.pView.pPlayer;

/// <summary>
/// プレイヤーサーフェイスインタフェース
/// </summary>
interface IPlayerSurface
{
    /// <summary>
    /// マウスダウンイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    void MouseDown( object sender, MouseEventArgs ev );

    /// <summary>
    /// マウス移動イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    void MouseMove( object sender, MouseEventArgs ev );

    /// <summary>
    /// マウスアップイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    void MouseUp( object sender, MouseEventArgs ev );

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
    bool OnDraw();
}
