using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigVideo : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigVideo()
    {
        InitializeComponent();

        #region NumberBox の入力書式設定

        _VideoFpsNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 0, 1 );

        #endregion
    }

    #region member

    /// <summary>
    /// Media設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    #endregion

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
            Log.Error( e );
        }
    }

    #endregion
}
