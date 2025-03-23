using DrumMidiLibrary.pControl;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pView;
using System.Collections.Concurrent;

namespace DrumMidiPlayerApp.pEvent;

/// <summary>
/// イベント管理（簡易）
/// </summary>
public static class EventManage
{
    #region Event.Device

    /// <summary>
    /// MIDI-OUT 遅延時間更新通知
    /// </summary>
    public static void Event_Device_UpdateMidiOutLatency() =>
        // Musicスレッドで更新
        Config.Media.FlagUpdateDmsControlScore = true;

    #endregion

    #region Event.Player

    /// <summary>
    /// プレイヤー描画モード更新リクエスト
    /// </summary>
    public static void Event_Player_UpdateSufaceMode() =>
        // 描画ループ処理内で更新フラグを見て更新
        Config.Player.FlagUpdateSurfaceModo = true;

    /// <summary>
    /// プレイヤースクリーンサイズ更新通知
    /// </summary>
    public static void Event_Player_UpdateScreenSize()
    {
        // Page側のサイズ更新
        //ControlAccess.PagePlayer?.UpdatePageSize();

        // Panel用フラグ更新：描画ループ処理内で更新フラグを見て更新
        Config.Player.FlagUpdateSize = true;

        // スクリーンサイズ変更に伴う更新
        Event_Player_UpdateScore();
    }

    /// <summary>
    /// Playerスコア更新リクエスト
    /// </summary>
    public static void Event_Player_UpdateScore() 
        => Config.Player.FlagUpdateScore = true;

    /// <summary>
    /// Player表示切替リクエスト
    /// </summary>
    /// <param name="aDisplayPlayer"></param>
    public static void Event_Player_ChangeDisplay( bool aDisplayPlayer )
    {
        Config.Player.DisplayPlayer = aDisplayPlayer;

        // プレイヤーコントロールの表示切替
        //ControlAccess.PageEditerMain?.ReloadPlayer();
    }

    /// <summary>
    /// ScaleItem再読込通知
    /// </summary>
    public static void Event_Player_ReloadScaleItem()
    {
        // プレイヤスコア更新リクエスト
        Event_Player_UpdateScore();
    }

    #endregion

    #region Event.Window

    /// <summary>
    /// Windowリサイズ通知
    /// </summary>
    public static void Event_Window_ResizeWindow()
    {
        // エディタパネルリサイズ
        //Event_Editer_ResizePanel();

        // スコアタブ表示更新リクエスト
        //Event_ScoreTab_UpdateScore();
    }

    #endregion
}
