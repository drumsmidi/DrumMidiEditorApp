using Microsoft.UI.Xaml;
using System.Diagnostics;
using Windows.ApplicationModel;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pView;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DrumMidiEditorApp;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private static Window _MainWindow;

    public static WindowEditer? MainWindow => _MainWindow as WindowEditer;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        // TODO: 多重起動の制御とか終了処理の仕方がいまいちわからない
        // https://docs.microsoft.com/ja-jp/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/applifecycle

        #region アプリ多重起動防止
        {
            var app = AppInstance.FindOrRegisterInstanceForKey( Config.System.AppName );

			if ( !app.IsCurrentInstance )
			{
				Trace.WriteLine( $"Preventing multiple launches of apps" );

				app.RedirectActivationTo();
				Exit();
				return;
			}
		}
		#endregion

        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched( LaunchActivatedEventArgs args )
    {
        _MainWindow = new WindowEditer();
        _MainWindow.Activate();
    }
}
