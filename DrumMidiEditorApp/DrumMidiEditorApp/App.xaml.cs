using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pIO;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pView;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace DrumMidiEditorApp;

/// <summary>
/// デフォルトのApplicationクラスを補足するアプリケーション固有の動作を提供します。
/// </summary>
public partial class App : Application
{
    private Window? _MainWindow;

    /// <summary>
    /// シングルトンアプリケーションオブジェクトを初期化します。 
    /// これは、実行される作成済みコードの最初の行であり、
    /// main（）またはWinMain（）と論理的に同等です。
    /// </summary>
    public App()
    {
        InitializeComponent();

        Log.SetLogFile( new( "TraceLog.log" ) );

        #region インスタンス管理

        // 既定で、単一インスタンス アプリ なので多重起動の制御は不要

        // GetInstancesに現在のインスタンスを追加と記載があったが
        // 実行しないでも登録されている？
        //AppInstance.GetCurrent();

        // AppInstanceに登録されているインスタンスの一覧
        foreach ( var instance in AppInstance.GetInstances() )
        {
            Log.Info( $"ProcessId={instance.ProcessId}, Key={instance.Key}, IsCurrent={instance.IsCurrent}" );
        }

        #endregion
    }

    /// <summary>
    /// アプリケーションがエンドユーザーによって正常に起動されたときに呼び出されます。 
    /// アプリケーションを起動して特定のファイルを開くときなど、他のエントリポイントが使用されます。
    /// </summary>
    /// <param name="args">起動リクエストとプロセスに関する詳細</param>
    protected override void OnLaunched( LaunchActivatedEventArgs args )
    {
        #region デバッグ設定
#if DEBUG
        // [エラーになる]フレーム レートとフレームごとの CPU 使用率情報を表示するかどうか
        //DebugSettings.EnableFrameRateCounter = true;

        // [エラーになる]XAML エラーがエラーを返すのではなく、すぐに FailFast を発生させるかどうか
        //DebugSettings.FailFastOnErrors = true;

        // バインド トレース機能
        //DebugSettings.IsBindingTracingEnabled = true;

        // テキスト パフォーマンス視覚化機能
        //DebugSettings.IsTextPerformanceVisualizationEnabled = true;
#endif
        #endregion

        // Configファイル読込
        FileIO.LoadConfig();

        #region 起動元ファイルのパス取得
        {
            var active_event_args = AppInstance.GetCurrent().GetActivatedEventArgs();

            if ( active_event_args.Kind == ExtendedActivationKind.File )
            {
                if ( active_event_args.Data is IFileActivatedEventArgs data )
                {
                    if ( data.Files.Count > 0 )
                    {
                        Config.System.AppStartDmsPath = new( data.Files [ 0 ].Path ?? string.Empty );

                        Log.Info( $"起動ファイル={Config.System.AppStartDmsPath.AbsoulteFilePath}" );
                    }
                }
            }
        }
        #endregion

        // メインウィンドウ作成
        _MainWindow = new WindowEditer();
        _MainWindow.Activate();
    }
}
