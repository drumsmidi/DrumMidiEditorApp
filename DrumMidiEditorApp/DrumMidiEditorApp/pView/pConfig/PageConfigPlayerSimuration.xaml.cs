using DrumMidiEditorApp.pConfig;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigPlayerSimuration : Page
{
    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayerSimuration DrawSet => ConfigLocal.Player.Simuration;

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
