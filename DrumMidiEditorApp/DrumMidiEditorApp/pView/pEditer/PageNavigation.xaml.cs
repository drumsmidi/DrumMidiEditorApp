using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pView.pEditer.pEdit;
using DrumMidiEditorApp.pView.pEditer.pConfig;
using DrumMidiEditorApp.pView.pEditer.pScore;

namespace DrumMidiEditorApp.pView.pEditer;

public sealed partial class PageNavigation : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageNavigation()
    {
        InitializeComponent();
    }

    #region ナビゲーションの制御

    /// <summary>
    /// ページ一覧
    /// </summary>
    private readonly List<(string Tag, Type Page)> _Pages = new()
        {
            ( "PageMusic"    , typeof( PageMusic )   ),
            ( "PageMidiMap"  , typeof( PageMidiMap ) ),
            ( "PageEdit"     , typeof( PageEdit )    ),
            ( "PageScore"    , typeof( PageScore )   ),
            ( "PageConfig"   , typeof( PageConfig )  ),
        };

	/// <summary>
	/// ナビゲーション ロード
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
    private void PageNaviControl_Loaded( object sender, RoutedEventArgs args )
    {
        try
        {
			// 初期ページへ移動
            NavigationView_Navigate( "PageScore" );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// ナビゲーション 変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void PageNaviControl_SelectionChanged( NavigationView sender, NavigationViewSelectionChangedEventArgs args )
    {
        try
        {
            var navItemTag = "";

            if ( args.IsSettingsSelected )
            {
                navItemTag = "settings";
            }
            else if ( args.SelectedItemContainer != null )
            {
                navItemTag = args.SelectedItemContainer.Tag.ToString() ?? String.Empty ;
            }

            NavigationView_Navigate( navItemTag );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// ナビゲーション ページ切替
	/// </summary>
	/// <param name="aNaviTagName"></param>
    private void NavigationView_Navigate( string aNaviTagName )
    {
        var pageItem    = _Pages.FirstOrDefault( item => item.Tag.Equals( aNaviTagName ) );
        var page_after  = pageItem.Page;
        var page_now    = _ContentFrame.CurrentSourcePageType;

        if ( page_after != null && !Type.Equals( page_after, page_now ) )
        {
            var item = _PageNaviControl.MenuItems
                .FirstOrDefault( item =>
                    item.GetType() == typeof( NavigationViewItem ) &&
                        ( (NavigationViewItem)item ).Tag.ToString() == aNaviTagName );

            // 対象NaviItemが選択されていない場合、SelectionChangedイベントを発生させる
            // （プログラム上から変更する場合、NaviItemが選択状態にならない）
            if ( !Object.Equals( _PageNaviControl.SelectedItem, item ) )
            { 
                _PageNaviControl.SelectedItem = item;
            }
            // ページ切替実施
            else
            {
                // https://docs.microsoft.com/ja-jp/windows/apps/design/motion/page-transitions
                // ページ切替効果の無効化
                //      SuppressNavigationTransitionInfo を
                //      他の NavigationTransitionInfo サブタイプの代わりに使用します。

                _ContentFrame.Navigate( page_after, null, new SuppressNavigationTransitionInfo() );
            }
        }
    }

    #endregion
}
