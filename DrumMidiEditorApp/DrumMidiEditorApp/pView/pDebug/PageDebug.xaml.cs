using System;
using System.Collections.Generic;
using System.Linq;
using DrumMidiClassLibrary.pLog;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace DrumMidiEditorApp.pView.pDebug;

public sealed partial class PageDebug : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageDebug()
    {
        // 初期化
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
            NaviLinkListView_Navigate( "PageDebugShell" );
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
            ( "PageDebugShell"  , typeof( PageDebugShell ) ),
            ( "PageDebugStyle"  , typeof( PageDebugStyle ) ),
        ];

    /// <summary>
    /// ナビゲーション 変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void NaviLinkListView_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
        try
        {
            var tag = ( ( sender as ListView )?.SelectedItem as FrameworkElement )?.Tag.ToString() ?? string.Empty ;

            NaviLinkListView_Navigate( tag );
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
    private void NaviLinkListView_Navigate( string aNaviTagName )
    {
        var pageItem    = _Pages.FirstOrDefault( item => item.Tag.Equals( aNaviTagName ) );
        var page_after  = pageItem.Page;
        var page_now    = _ContentFrame.CurrentSourcePageType;

        if ( page_after != null && !Type.Equals( page_after, page_now ) )
        {
            var item = _NaviLinkListView.Items
                .FirstOrDefault( item =>
                    item.GetType() == typeof( TextBlock ) &&
                        ( (TextBlock)item ).Tag.ToString() == aNaviTagName );

            // 対象NaviItemが選択されていない場合、SelectionChangedイベントを発生させる
            // （プログラム上から変更する場合、NaviItemが選択状態にならない）
            if ( !object.Equals( _NaviLinkListView.SelectedItem, item ) )
            {
                _NaviLinkListView.SelectedItem = item;
            }
            // ページ切替実施
            else
            {
                // ページ切替効果の無効化
                //      SuppressNavigationTransitionInfo を
                //      他の NavigationTransitionInfo サブタイプの代わりに使用します。
                // https://docs.microsoft.com/ja-jp/windows/apps/design/motion/page-transitions

                _ = _ContentFrame.Navigate( page_after, null, new SuppressNavigationTransitionInfo() );
            }
        }
    }

    #endregion
}
