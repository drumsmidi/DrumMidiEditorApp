using Microsoft.UI.Xaml.Controls;

using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pConfig;
using System;

namespace DrumMidiEditorApp.pView;

public sealed partial class PageEditerMain : Page
{
    private ConfigPlayer ConfigPlayer => Config.Player;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageEditerMain()
    {
        InitializeComponent();

        ControlAccess.PageEditerMain = this;

        UpdateGridLayout();
    }

    /// <summary>
    /// Gridレイアウト更新
    /// </summary>
    public void UpdateGridLayout()
    {
        try
        {
            int row;
            int rowspan;
            int col;
            int colspan;

            switch ( ConfigPlayer.PlayerLayoutModeSelect )
            {
                case ConfigPlayer.PlayerLayoutMode.None:
                    row     = 4;
                    rowspan = 2;
                    col     = 0;
                    colspan = 2;
                    break;
                case ConfigPlayer.PlayerLayoutMode.Right:
                    row     = 1;
                    rowspan = 2;
                    col     = 1;
                    colspan = 1;
                    break;
                case ConfigPlayer.PlayerLayoutMode.Bottom:
                    row     = 2;
                    rowspan = 1;
                    col     = 0;
                    colspan = 2;
                    break;
                default:
                    return;
            }

            // x:Bind経由では更新できなかったので直接設定
            Grid.SetRowSpan     ( _PageNavigation, rowspan );
            Grid.SetColumnSpan  ( _PageNavigation, colspan );

            Grid.SetRow         ( _PagePlayer, row );
            Grid.SetRowSpan     ( _PagePlayer, rowspan );
            Grid.SetColumn      ( _PagePlayer, col );
            Grid.SetColumnSpan  ( _PagePlayer, colspan );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
