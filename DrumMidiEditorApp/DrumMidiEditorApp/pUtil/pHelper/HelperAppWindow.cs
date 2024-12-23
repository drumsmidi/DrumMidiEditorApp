using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using WinRT.Interop;

namespace DrumMidiEditorApp.pUtil.pHelper;

/// <summary>
/// Window操作拡張
/// </summary>
public static class HelperAppWindow
{
    /// <summary>
    /// AppWindow取得
    /// </summary>
    /// <param name="aWindow">Windowインスタンス</param>
    /// <returns>AppWindow</returns>
    public static AppWindow GetAppWindow( this Window aWindow )
    {
        var hwnd = WindowNative.GetWindowHandle( aWindow );
        return GetAppWindowFromWindowHandle( hwnd );
    }

    /// <summary>
    /// AppWindow取得
    /// </summary>
    /// <param name="aHwnd"></param>
    /// <returns></returns>
    private static AppWindow GetAppWindowFromWindowHandle( nint aHwnd )
    {
        var windowId = Win32Interop.GetWindowIdFromWindow( aHwnd );

        return AppWindow.GetFromWindowId( windowId );
    }

    /// <summary>
    /// Windowリサイズ
    /// </summary>
    /// <param name="aAppWindow"></param>
    /// <param name="aWidth"></param>
    /// <param name="aHeight"></param>
    public static void ResizeWindow( AppWindow aAppWindow, int aWidth, int aHeight )
        => aAppWindow.Resize( new( aWidth, aHeight ) );

    #region Presenter

    /// <summary>
    /// ウィンドウのプレゼンター設定
    /// （通常のウィンドウ）
    /// </summary>
    /// <param name="aAppWindow"></param>
    public static void SetPresenterNormalWindow( AppWindow aAppWindow )
        => SetPresenter( aAppWindow, true, true, true, false, false, true, true );

    /// <summary>
    /// ウィンドウのプレゼンター設定
    /// （プレイヤー用）
    /// </summary>
    /// <param name="aAppWindow"></param>
    public static void SetPresenterPlayerWindow( AppWindow aAppWindow )
        => SetPresenter( aAppWindow, false, false, true, true, false, false, false );

    /// <summary>
    /// ウィンドウのプレゼンター設定
    /// （サイズ変更可：ダイアログ）
    /// </summary>
    /// <param name="aAppWindow"></param>
    public static void SetPresenterSizableDialog( AppWindow aAppWindow )
        => SetPresenter( aAppWindow, false, false, true, true, true, true, true );

    /// <summary>
    /// ウィンドウのプレゼンター設定
    /// （サイズ変更不可：ダイアログ）
    /// </summary>
    /// <param name="aAppWindow"></param>
    public static void SetPresenterFixedDialog( AppWindow aAppWindow )
    {
        var op = CompactOverlayPresenter.Create();
        op.InitialSize = CompactOverlaySize.Small;

        aAppWindow.SetPresenter( op );

        // OverlappedPresenter で設定しようとすると、IsAlwaysOnTop=true でエラーになる
        // SetPresenter( aAppWindow, false, false, false, true, true, true, true );
    }

    /// <summary>
    /// ウィンドウのプレゼンター設定
    /// </summary>
    /// <param name="aAppWindow"></param>
    /// <param name="aMaximizable"></param>
    /// <param name="aMinimizable"></param>
    /// <param name="aResizable"></param>
    /// <param name="aAlwaysOnTop"></param>
    /// <param name="aModal"></param>
    /// <param name="aHasBorder"></param>
    /// <param name="aHasTitleBar"></param>
    public static void SetPresenter( AppWindow aAppWindow, bool aMaximizable, bool aMinimizable, bool aResizable, bool aAlwaysOnTop, bool aModal, bool aHasBorder, bool aHasTitleBar )
    {
        // 参考URL
        // https://tera1707.com/entry/2022/04/24/220519

        var op = OverlappedPresenter.Create();
        op.IsMaximizable = aMaximizable;
        op.IsMinimizable = aMinimizable;
        op.IsResizable = aResizable;
        op.IsAlwaysOnTop = aAlwaysOnTop;
        op.IsModal = aModal;
        op.SetBorderAndTitleBar( aHasBorder, aHasTitleBar );

        aAppWindow.SetPresenter( op );
    }

    #endregion
}
