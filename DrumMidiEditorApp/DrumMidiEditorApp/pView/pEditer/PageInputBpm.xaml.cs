using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pUtil.pHelper;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pEditer;

public sealed partial class PageInputBpm : Page
{
    /// <summary>
    /// メディア設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// BPM入力値
    /// </summary>
    public double Bpm { get; set; } = Config.System.DefaultBpm;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageInputBpm()
    {
        InitializeComponent();

        // NumberBox の入力書式設定
        _BpmNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 2, 0.01 );
    }

    /// <summary>
    /// ＢＰＭ変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void BpmNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( sender, args ) )
            {
                return;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
