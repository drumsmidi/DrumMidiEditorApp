using Microsoft.UI.Xaml;

using DrumMidiEditorApp.pView;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DrumMidiEditorApp;

/// <summary>
/// デフォルトのApplicationクラスを補足するアプリケーション固有の動作を提供します。
/// </summary>
public partial class App : Application
{
    private Window _MainWindow;

    /// <summary>
    /// シングルトンアプリケーションオブジェクトを初期化します。 
    /// これは、実行される作成済みコードの最初の行であり、
    /// main（）またはWinMain（）と論理的に同等です。
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// アプリケーションがエンドユーザーによって正常に起動されたときに呼び出されます。 
    /// アプリケーションを起動して特定のファイルを開くときなど、他のエントリポイントが使用されます。
    /// </summary>
    /// <param name="args">起動リクエストとプロセスに関する詳細</param>
    protected override void OnLaunched( LaunchActivatedEventArgs args )
    {
        _MainWindow = new WindowEditer();
        _MainWindow.Activate();
    }
}
