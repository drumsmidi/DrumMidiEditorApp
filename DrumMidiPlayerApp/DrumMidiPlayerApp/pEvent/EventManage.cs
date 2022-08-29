using System.Collections.Concurrent;

using DrumMidiClassLibrary.pConfig;
using DrumMidiClassLibrary.pControl;

using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pView;

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
    public static void EventUpdateMidiOutLatency()
    {
        // Musicスレッドで更新
        Config.Media.UpdateDmsControlScore = true;
    }

    #endregion

    #region Event.

    /// <summary>
    /// フラグ更新：スコア再読込
    /// </summary>
    public static void EventReloadScore()
    {
        // ベースBPM設定
        EventEditBaseBpm();

        // チャンネル切替通知
        EventChangeChannel();

        // BGM再読み込み
        EventReloadBgm();
    }

    /// <summary>
    /// チャンネル切替通知
    /// </summary>
    public static void EventChangeChannel()
    {
        // MidiMap再読み込みリクエスト
        EventReloadMidiMapSet();
    }

    #endregion

    #region Event.MusicInfo

    /// <summary>
    /// BGM変更／再読込リクエスト
    /// </summary>
    public static void EventReloadBgm()
    {
        // Musicスレッドで更新
        Config.Media.UpdateDmsControlBgm = true;
    }

    /// <summary>
    /// ベースBPM更新通知
    /// </summary>
    public static void EventEditBaseBpm()
    {
        // タイムテーブル更新リクエスト
        DmsControl.RefreshTimeTable();

        // BPM変更に伴うスコア更新リクエスト
        Config.Media.UpdateDmsControlScore = true;
    }

    #endregion

    #region Event.MidiMap

    /// <summary>
    /// MidiMapSet再読込通知
    /// </summary>
    public static void EventReloadMidiMapSet()
    {
        Config.Media.UpdateDmsControlScore      = true;
        Config.Media.UpdateDmsControlMidiMap    = true;

        // プレイヤスコア更新リクエスト
        EventPlayerUpdateScore();
    }

    #endregion

    #region Event.Player

    /// <summary>
    /// プレイヤー描画モード更新リクエスト
    /// </summary>
    public static void EventPlayerUpdateSufaceMode()
    {
        // 描画ループ処理内で更新フラグを見て更新
        ConfigLocal.Player.UpdateSurfaceModoFlag = true;
    }

    /// <summary>
    /// プレイヤースクリーンサイズ更新通知
    /// </summary>
    public static void EventPlayerUpdateScreenSize()
    {
        // Page側のサイズ更新
        //ControlAccess.PanelPlayer?.UpdatePanelSize();

        // Panel用フラグ更新：描画ループ処理内で更新フラグを見て更新
        ConfigLocal.Player.UpdateSizeFlag = true;

        // スクリーンサイズ変更に伴う更新
        EventPlayerUpdateScore();
    }

    /// <summary>
    /// Playerスコア更新リクエスト
    /// </summary>
    public static void EventPlayerUpdateScore()
    {
        ConfigLocal.Player.UpdateScoreFlag = true;
    }

    #endregion

    #region Event.Common

    /// <summary>
    /// Windowリサイズ通知
    /// </summary>
    public static void EventResizeWindow()
    {

    }

    #endregion
}
