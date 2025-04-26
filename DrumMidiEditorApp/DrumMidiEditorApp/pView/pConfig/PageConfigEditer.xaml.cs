using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigEditer : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigEditer()
    {
        InitializeComponent();
    }

    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigEditer DrawSet => Config.Editer;

    #endregion

    /// <summary>
    /// ページロード完了後処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Page_Loaded( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            #region NumberBox の入力書式設定

            _Line128NumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
            _Line064NumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
            _Line032NumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
            _Line016NumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
            _Line008NumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
            _Line004NumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
            _Line001NumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );

            #endregion
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// 共通：トグル切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ToggleSwitch_Toggled( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ノートサイズ変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void SizeNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
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
    /// 色選択
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ColorButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( aSender is Button item )
            {
                HelperXaml.ColorDialog
                    (
                        item,
                        ( item.Background as SolidColorBrush )?.Color ?? HelperColor.EmptyColor,
                        ( color ) =>
                        {
                            item.Background = new SolidColorBrush( color );
                        }
                    );
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}
