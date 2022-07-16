using System;
using System.Diagnostics;
using System.Windows.Forms;

using DrumMidiEditor.pControl;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pAudio;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pIO;

namespace DrumMidiEditor;

/// <summary>
/// プログラムメイン
/// </summary>
public static class Program
{
	/// <summary>
	/// アプリ起動
	/// </summary>
	[STAThread]
    public static void Main()
    {
		try
        {
			// DpiUnaware			: アプリケーション ウィンドウは DPI の変更に合わせて拡大縮小されず、常に 100 % の倍率が想定されます。
			// DpiUnawareGdiScaled	: DpiUnaware に似ていますが、GDI / GDI + ベースのコンテンツの品質が向上します。
			// PerMonitor			: このウィンドウでは、作成時に DPI が確認され、DPI が変化したときに倍率が調整されます。
			// PerMonitorV2			: PerMonitor に似ていますが、子ウィンドウの DPI 変更通知、comctl32.dll コントロールの拡大縮小の改善
			//						  およびダイアログの拡大縮小を使用できます。
			// SystemAware			: このウィンドウでは、プライマリ モニターの DPI が 1 回照会され、
			//						  それがすべてのモニターのアプリケーションに対して使用されます。
			Application.SetHighDpiMode( HighDpiMode.DpiUnaware );
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );

			#region アプリ多重起動防止
			{
				var procName = Process.GetCurrentProcess().ProcessName;

				if ( Process.GetProcessesByName( procName ).Length > 1 ) 
				{
					Log.Info( $"Preventing multiple launches of apps" );
					return;
				}
			}
			#endregion

			#region アプリ初期化処理
			{
				Log.Info( $"Current directory:{Application.StartupPath}" );

				FileIO.LoadConfig();

				if ( !MidiNet.InitDevice() )
				{
					Log.Warning( $"Initialization failure... Midi device" );
					return;
				}

				DmsControl.Start();

				DMS.InitializeForm();
			}
			#endregion

			// メインフォーム起動
			Application.Run( DMS.EditerForm );

			#region アプリ終了処理
			{
				DmsControl.StopPreSequence();
				DmsControl.End();

				FileIO.SaveConfig();
			}
			#endregion
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}
}
