using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pEditer;

public sealed partial class PageInputBpm : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageInputBpm()
    {
        InitializeComponent();

        #region NumberBox の入力書式設定

        _BpmNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 2, 0.01 );

        #endregion
    }

    #region member

    /// <summary>
    /// メディア設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// エディター設定
    /// </summary>
    private ConfigEditer ConfigEditer => Config.Editer;

    /// <summary>
    /// BPM入力値
    /// </summary>
    public double Bpm { get; set; } = Config.System.DefaultBpm;

    #endregion

    /// <summary>
    /// ＢＰＭ変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void BpmNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ＢＰＭ変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void BpmRadioButtons_SelectionChanged( object aSender, SelectionChangedEventArgs aArgs )
    {
        try
        {
            var obj = aSender as RadioButtons;
            ConfigEditer.BpmChangeIndex = obj?.SelectedIndex ?? 0;

            double s, l;
            switch ( ConfigEditer.BpmChangeIndex )
            {
                case 0:
                    s = 1;
                    l = 10;
                    break;
                case 1:
                    s = 0.1;
                    l = 1;
                    break;
                case 2:
                    s = 0.01;
                    l = 0.1;
                    break;
                default:
                    s = 1;
                    l = 10;
                    break;
            }

            _BpmNumberBox.SmallChange = s;
            _BpmNumberBox.LargeChange = l;
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}
