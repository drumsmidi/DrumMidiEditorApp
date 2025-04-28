using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pConfig;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigSystem : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigSystem()
    {
        InitializeComponent();
    }

    #region Member

    /// <summary>
    /// システム設定
    /// </summary>
    private ConfigSystem ConfigSystem => Config.System;

    /// <summary>
    /// ログ設定
    /// </summary>
    private ConfigLog ConfigLog => Config.Log;

    #endregion
}
