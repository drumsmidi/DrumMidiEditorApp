using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pScore;

public sealed partial class PageScore : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageScore()
    {
        InitializeComponent();

        #region NumberBox の入力書式設定

        _NoteHeightNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _NoteWidthNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );

        #endregion
    }

    #region member

    /// <summary>
    /// スコア設定
    /// </summary>
    private ConfigScore DrawSet => Config.Score;

    #endregion

    /// <summary>
    /// スコア更新フラグ設定
    /// </summary>
    private static void UpdateScore() => EventManage.Event_ScoreTab_UpdateScore();

    /// <summary>
    /// 共通：トグル切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ToggleSwitch_Toggled( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// 共通：数値変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void NumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}
