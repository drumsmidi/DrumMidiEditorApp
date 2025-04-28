using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigPlayerSimuration : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigPlayerSimuration()
    {
        InitializeComponent();

        #region NumberBox の入力書式設定

        //_Line128NumberBox.NumberFormatter
        //	= XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );

        #endregion
    }

    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayerSimuration DrawSet => Config.Player.Simuration;

    #endregion

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
