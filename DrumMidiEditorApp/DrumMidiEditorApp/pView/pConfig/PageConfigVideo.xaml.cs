using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pUtil.pHelper;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigVideo : Page
{
    /// <summary>
    /// Media設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigVideo()
    {
        // 初期化
        InitializeComponent();

        // NumberBox の入力書式設定
        _VideoFpsNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 0, 1 );
    }

    #region Video

    /// <summary>
    /// Fps設定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void VideoFpsNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
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

    #endregion
}
