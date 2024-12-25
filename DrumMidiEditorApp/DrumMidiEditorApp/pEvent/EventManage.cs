using System.Collections.Concurrent;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pView;

namespace DrumMidiEditorApp.pEvent;

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

    #region Event.Score

    /// <summary>
    /// フラグ更新：スコア再読込
    /// </summary>
    public static void Event_Score_ReloadScore()
    {
        // Musicページ再読み込み
        ControlAccess.PageMusic?.ReloadMusicInfo();

        // ベースBPM設定
        Event_Score_EditBaseBpm();

        // チャンネル切替通知
        Event_Score_ChangeChannel();

        // BGM再読み込み
        Event_Score_ReloadBgm();
    }

    /// <summary>
    /// チャンネル切替通知
    /// </summary>
    public static void Event_Score_ChangeChannel() =>
        // MidiMap再読み込みリクエスト
        Event_Score_ReloadMidiMapSet();

    /// <summary>
    /// BGM変更／再読込リクエスト
    /// </summary>
    public static void Event_Score_ReloadBgm()
    {
        // Musicスレッドで更新
        Config.Media.FlagUpdateDmsControlBgm    = true;

        // BGMの再読み込み後、波形を読み込む
        Config.Editer.FlagUpdateScoreBgm        = true;
        Config.Editer.FlagUpdateScoreBgmScale   = false;
    }

    /// <summary>
    /// ベースBPM更新通知
    /// </summary>
    public static void Event_Score_EditBaseBpm()
    {
        // タイムテーブル更新リクエスト
        DmsControl.RefreshTimeTable();

        // BPM変更に伴うスコア更新リクエスト
        Config.Media.FlagUpdateDmsControlScore = true;
    }

    /// <summary>
    /// BGM再生開始位置更新通知
    /// </summary>
    public static void Event_Score_EditBgmPlaybackStartPosition() =>
        // スコア更新リクエスト（専用のフラグを用意していない為）
        Config.Media.FlagUpdateDmsControlScore = true;

    /// <summary>
    /// MidiMapSet再読込通知
    /// </summary>
    public static void Event_Score_ReloadMidiMapSet()
    {
        Config.Media.FlagUpdateDmsControlScore      = true;
        Config.Media.FlagUpdateDmsControlMidiMap    = true;

        // MidiMapSet再読み込み
        ControlAccess.UCMidiMapPanel?.ReloadMidiMapSet();

        // キー変更対象一覧再読み込み
        ControlAccess.UCKeyChangePanel?.ReloadMidiMapNoteList();

        // エディタパネル更新リクエスト
        Event_Editer_UpdateLayout();

        // スコアタブ表示更新リクエスト
        Event_ScoreTab_UpdateScore();

        // プレイヤスコア更新リクエスト
        Event_Player_UpdateScore();
    }

    /// <summary>
    /// MidiMapKey変更通知
    /// </summary>
    public static void Event_Score_ChangeMidiMapKey()
    {
        // スコア更新
        Config.Media.FlagUpdateDmsControlScore = true;

        // キー変更対象一覧再読み込み
        ControlAccess.UCKeyChangePanel?.ReloadMidiMapNoteList();

        // エディタパネル更新リクエスト
        Event_Editer_UpdateLayout();

        // スコアタブ表示更新リクエスト
        Event_ScoreTab_UpdateScore();

        // プレイヤスコア更新リクエスト
        Event_Player_UpdateScore();
    }

    /// <summary>
    /// ノート設定／削除
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    public static void Event_Score_EditNote( int aMeasureNo )
    {
        // 音量変更に伴いスコア再読み込みリクエスト
        Config.Media.FlagUpdateDmsControlScore = true;

        // NOTE表示更新リクエスト
        Config.Editer.FlagUpdateScoreNote = true;

        // 表示更新対象の小節番号を登録
        if ( !Config.Editer.UpdateScoreNoteMeasureNoList.Contains( aMeasureNo ) )
        {
            Config.Editer.UpdateScoreNoteMeasureNoList.Add( aMeasureNo );
        }

        // スコアタブ表示更新リクエスト
        Event_ScoreTab_UpdateScore();

        // プレイヤスコア更新リクエスト
        Event_Player_UpdateScore();
    }

    /// <summary>
    /// BPM設定／削除
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    public static void Event_Score_EditBpm( int aMeasureNo )
    {
        // BPM変更に伴うタイムテーブル再計算およびスコア再読み込みリクエスト
        DmsControl.RefreshTimeTable();
        Config.Media.FlagUpdateDmsControlScore = true;

        // BPM表示更新リクエスト
        Config.Editer.FlagUpdateScoreBpm = true;

        // 表示更新対象の小節番号を登録
        if ( !Config.Editer.UpdateScoreBpmMeasureNoList.Contains( aMeasureNo ) )
        {
            Config.Editer.UpdateScoreBpmMeasureNoList.Add( aMeasureNo );
        }

        // プレイヤスコア更新リクエスト
        Event_Player_UpdateScore();
    }

    /// <summary>
    /// フラグ更新：ノートボリューム更新通知
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    public static void Event_Score_EditNoteVolume( int aMeasureNo )
    {
        // 音量変更に伴いスコア再読み込みリクエスト
        Config.Media.FlagUpdateDmsControlScore = true;

        // ノート音量表示更新リクエスト
        Config.Editer.FlagUpdateScoreNoteVolume = true;

        // 表示更新対象の小節番号を登録
        if ( !Config.Editer.UpdateScoreNoteVolumeMeasureNoList.Contains( aMeasureNo ) )
        {
            Config.Editer.UpdateScoreNoteVolumeMeasureNoList.Add( aMeasureNo );
        }

        // スコアタブ表示更新リクエスト
        Event_ScoreTab_UpdateScore();

        // プレイヤスコア更新リクエスト
        Event_Player_UpdateScore();
    }

    #endregion

    #region Event.Editer

    /// <summary>
    /// レイアウト更新リクエスト
    /// </summary>
    private static void Event_Editer_UpdateLayout()
    {
        // 編集履歴クリア
        Event_Resume_Redo();

        // 範囲選択解除
        Event_Editer_ClearRangeSelect();

        Config.Editer.FlagUpdateScoreLayout         = true;
        Config.Editer.FlagUpdateCamera              = true;
        Config.Editer.FlagUpdateScoreLine           = true;
        Config.Editer.FlagUpdateScoreHeader         = true;
        Config.Editer.FlagUpdateScore               = true;
        Config.Editer.FlagUpdateScoreBgmScale       = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// エディタパネルリサイズ通知
    /// </summary>
    private static void Event_Editer_ResizePanel()
    {
        Config.Editer.FlagUpdateScoreLayout         = true;
        Config.Editer.FlagUpdateCamera              = true;
        Config.Editer.FlagUpdateScoreLine           = true;
        Config.Editer.FlagUpdateScoreHeader         = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// エディタパネル内のアイテムリサイズ通知
    /// </summary>
    public static void Event_Editer_ResizeItem()
    {
        Config.Editer.FlagUpdateScoreLayout         = true;
        Config.Editer.FlagUpdateCamera              = true;
        Config.Editer.FlagUpdateScoreLine           = true;
        Config.Editer.FlagUpdateScoreHeader         = true;
        Config.Editer.FlagUpdateScore               = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// シート移動通知
    /// </summary>
    public static void Event_Editer_UpdateSheetPos()
    {
        Config.Editer.FlagUpdateCamera = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// 範囲選択状態更新通知
    /// </summary>
    /// <param name="aRangeSelect"></param>
    public static void Event_Editer_UpdateRangeSelect( bool aRangeSelect )
    {
        Config.Editer.IsRangeSelect = aRangeSelect;

        ControlAccess.PageEdit?.ReloadConfigEditer();
    }

    /// <summary>
    /// 範囲選択内のノートを左寄せに移動リクエスト
    /// </summary>
    public static void Event_Editer_LeftAlignNoteOfRangeSelect()
    {
        Config.Editer.FlagUpdateLeftAlignNoteOfRangeSelect = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// 範囲選択解除リクエスト
    /// </summary>
    public static void Event_Editer_ClearRangeSelect()
    {
        Config.Editer.FlagUpdateClearRange = true;

        ControlAccess.PageEdit?.ReloadConfigEditer();
        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// 波形表示画像更新リクエスト
    /// </summary>
    public static void Event_Editer_UpdateWaveForm()
    {
        Config.Editer.FlagUpdateScoreBgmScale = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// 音量入力エリア表示更新フラグ
    /// （MidiMapGroup/MidiMap 選択など）
    /// </summary>
    public static void Event_Editer_UpdateDisplayNoteVolume()
    {
        Config.Editer.FlagUpdateScoreNoteVolume = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// 機械学習 予測更新通知
    /// </summary>
    public static void Event_Editer_UpdateScorePredict()
    {
        Config.Editer.FlagUpdateScoreNotePredict = true;

        ControlAccess.PageEdit?.Refresh();
    }

    #endregion

    #region Event.Resume

    /// <summary>
    /// レジュームアクション
    /// </summary>
    public enum EResumeAction : int
    {
        Undo = 0,
        Redo,
        Clear,
    }

    /// <summary>
    /// レジュームリクエストキュー
    /// </summary>
    public static ConcurrentQueue<EResumeAction> ResumeRequestQueue { get; private set; } = new();

    /// <summary>
    /// Undoリクエスト
    /// </summary>
    public static void Event_Resume_Undo()
    {
        ResumeRequestQueue.Enqueue( EResumeAction.Undo );

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// Redoリクエスト
    /// </summary>
    public static void Event_Resume_Redo()
    {
        ResumeRequestQueue.Enqueue( EResumeAction.Redo );

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// Resumeクリアリクエスト
    /// </summary>
    public static void Event_Resume_ClearResume() 
        => ResumeRequestQueue.Enqueue( EResumeAction.Clear );

    #endregion

    #region Event.ScoreTab

    /// <summary>
    /// Scoreタブ更新リクエスト
    /// </summary>
    public static void Event_ScoreTab_UpdateScore()
    {
        Config.Score.FlagUpdateScore = true;

        // 描画時に更新
        ControlAccess.UCScore?.Refresh();
    }

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
        ControlAccess.PagePlayer?.UpdatePageSize();

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

        // メニューバーのプレイヤースイッチ切替
        ControlAccess.PageMenuBar?.ReloadConfigPlayer();

        // プレイヤーコントロールの表示切替
        ControlAccess.PageEditerMain?.ReloadPlayer();
    }

    #endregion

    #region Event.Window

    /// <summary>
    /// Windowリサイズ通知
    /// </summary>
    public static void Event_Window_ResizeWindow()
    {
        // エディタパネルリサイズ
        Event_Editer_ResizePanel();

        // スコアタブ表示更新リクエスト
        Event_ScoreTab_UpdateScore();
    }

    #endregion
}
