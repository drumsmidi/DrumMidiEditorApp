using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pUtil;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigPlayerSimuration : Page
{
    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayerSimuration DrawSet => Config.Player.Simuration;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigPlayerSimuration()
    {
        // 初期化
        InitializeComponent();

        #region NumberBox の入力書式設定

        //_Line128NumberBox.NumberFormatter
        //	= XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );

        #endregion
    }

    /// <summary>
    /// 色選択
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ColorButton_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            if ( sender is not Button item )
            {
                return;
            }

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
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
