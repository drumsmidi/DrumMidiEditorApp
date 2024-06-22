using DrumMidiEditorApp.pConfig;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigPlayerSimuration : Page
{
    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
#pragma warning disable IDE0051 // 使用されていないプライベート メンバーを削除する
    private ConfigPlayerSimuration DrawSet => Config.Player.Simuration;
#pragma warning restore IDE0051 // 使用されていないプライベート メンバーを削除する

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
}
