using System;
using System.Collections.Generic;
using System.Linq;
using DrumMidiEditorApp.pView.pConfig;
using DrumMidiEditorApp.pView.pDebug;
using DrumMidiEditorApp.pView.pEditer;
using DrumMidiEditorApp.pView.pMidiMap;
using DrumMidiEditorApp.pView.pML;
using DrumMidiEditorApp.pView.pMusic;
using DrumMidiEditorApp.pView.pScore;
using DrumMidiLibrary.pLog;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace DrumMidiEditorApp.pView.pNavi;

public sealed partial class PageNavigation : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageNavigation()
    {
        InitializeComponent();
    }

    /// <summary>
    /// ページロード完了
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Page_Loaded( object sender, RoutedEventArgs args )
    {
        try
        {
            // 初期ページへ移動
            NavigationView_Navigate( "PageMusic" );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #region ナビゲーションの制御

    /// <summary>
    /// ページ一覧
    /// </summary>
    private readonly List<(string Tag, Type Page)> _Pages =
        [
            ( "PageMusic"    , typeof( PageMusic )   ),
            ( "PageMidiMap"  , typeof( PageMidiMap ) ),
            ( "PageEdit"     , typeof( PageEdit )    ),
            ( "PageScore"    , typeof( PageScore )   ),
            ( "PageML"       , typeof( PageML )      ),
            ( "PageConfig"   , typeof( PageConfig )  ),
            ( "PageDebug"    , typeof( PageDebug )   ),
        ];

    /// <summary>
    /// ナビゲーション 変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void NaviControl_SelectionChanged( NavigationView sender, NavigationViewSelectionChangedEventArgs args )
    {
        try
        {
            var navItemTag = "";

            if ( args.IsSettingsSelected )
            {
                // 未使用
                navItemTag = "settings";
            }
            else if ( args.SelectedItemContainer != null )
            {
                navItemTag = args.SelectedItemContainer.Tag.ToString() ?? string.Empty;
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
            var item = _NaviControl.MenuItems
                .FirstOrDefault( item =>
                    item.GetType() == typeof( NavigationViewItem ) &&
                        ( (NavigationViewItem)item ).Tag.ToString() == aNaviTagName );

            // 対象NaviItemが選択されていない場合、SelectionChangedイベントを発生させる
            // （プログラム上から変更する場合、NaviItemが選択状態にならない）
            if ( !object.Equals( _NaviControl.SelectedItem, item ) )
            {
                _NaviControl.SelectedItem = item;
            }
            // ページ切替実施
            else
            {
                // ページ切替効果
                //   ページ更新　　：EntranceNavigationTransitionInfo
                //   ドリル　　　　：DrillInNavigationTransitionInfo
                //   横方向スライド：SlideNavigationTransitionInfo
                //   無効化    　　：SuppressNavigationTransitionInfo

                _ = _ContentFrame.Navigate( page_after, null, new SuppressNavigationTransitionInfo() );
            }
        }
    }

    #endregion
}
