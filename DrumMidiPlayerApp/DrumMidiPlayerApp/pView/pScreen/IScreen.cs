using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace DrumMidiPlayerApp.pView.pScreen;

/// <summary>
/// プレイヤーサーフェイスインタフェース
/// </summary>
public interface IScreen
{
    /// <summary>
    /// アクティブ化／非アクティブ化
    /// </summary>
    /// <param name="aActivate">true:アクティブ化、false:非アクティブ化</param>
    void OnActivate( bool aActivate );

    /// <summary>
    /// フレーム処理
    /// </summary>
    /// <param name="aFrameTime">フレーム時間（秒）</param>
    /// <returns>True:処理続行、False:処理中断</returns>
	void OnMove( double aFrameTime );

    /// <summary>
    /// 描画
    /// </summary>
    /// <returns>True:処理続行、False:処理中断</returns>
    void OnDraw( CanvasDrawEventArgs aArgs );

    /// <summary>
    /// ロード完了フラグ
    /// </summary>
    bool IsLoaded { get; }

    /// <summary>
    /// アンロード完了フラグ
    /// </summary>
    bool IsUnLoaded { get; }

    /// <summary>
    /// 親スクリーンのインスタンスを設定
    /// </summary>
    void SetParentScreen( IScreen aScreen );

    /// <summary>
    /// 親スクリーンのインスタンスを取得
    /// </summary>
    IScreen? GetParentScreen();

    /// <summary>
    /// 親スクリーンのインスタンスを取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T? GetParentScreen<T>();

    /// <summary>
    /// スクリーン描画範囲設定
    /// </summary>
    void SetScreenDrawRect( RectClass aRectClass );
}
