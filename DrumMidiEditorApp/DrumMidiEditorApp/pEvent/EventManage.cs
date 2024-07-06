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
    public static void EventUpdateMidiOutLatency() =>
        // Musicスレッドで更新
        Config.Media.UpdateDmsControlScore = true;

    #endregion

    #region Event.

    /// <summary>
    /// フラグ更新：スコア再読込
    /// </summary>
    public static void EventReloadScore()
    {
        // Musicページ再読み込み
        ControlAccess.PageMusic?.ReloadMusicInfo();

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
    public static void EventChangeChannel() =>
        // MidiMap再読み込みリクエスト
        EventReloadMidiMapSet();

    #endregion

    #region Event.MusicInfo

    /// <summary>
    /// BGM変更／再読込リクエスト
    /// </summary>
    public static void EventReloadBgm()
    {
        // Musicスレッドで更新
        Config.Media.UpdateDmsControlBgm = true;

        // BGMの再読み込み後、波形を読み込む
        Config.Editer.UpdateScoreBgmFlag = true;
        Config.Editer.UpdateScoreBgmScaleFlag = false;
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

    /// <summary>
    /// BGM再生開始位置更新通知
    /// </summary>
    public static void EventEditBgmPlaybackStartPosition() =>
        // スコア更新リクエスト（専用のフラグを用意していない為）
        Config.Media.UpdateDmsControlScore = true;

    #endregion

    #region Event.MidiMap

    /// <summary>
    /// MidiMapSet再読込通知
    /// </summary>
    public static void EventReloadMidiMapSet()
    {
        Config.Media.UpdateDmsControlScore = true;
        Config.Media.UpdateDmsControlMidiMap = true;

        // MidiMapSet再読み込み
        ControlAccess.UCMidiMapPanel?.ReloadMidiMapSet();

        // キー変更対象一覧再読み込み
        ControlAccess.UCKeyChangePanel?.ReloadMidiMapNoteList();

        // エディタパネル更新リクエスト
        EventEditUpdateLayout();

        // スコアタブ表示更新リクエスト
        EventScoreUpdateTab();

        // プレイヤスコア更新リクエスト
        EventPlayerUpdateScore();
    }

    /// <summary>
    /// MidiMapKey変更通知
    /// </summary>
    public static void EventChangeMidiMapKey()
    {
        // スコア更新
        Config.Media.UpdateDmsControlScore = true;

        // キー変更対象一覧再読み込み
        ControlAccess.UCKeyChangePanel?.ReloadMidiMapNoteList();

        // エディタパネル更新リクエスト
        EventEditUpdateLayout();

        // スコアタブ表示更新リクエスト
        EventScoreUpdateTab();

        // プレイヤスコア更新リクエスト
        EventPlayerUpdateScore();
    }

    #endregion

    #region Event.Edit

    /// <summary>
    /// レイアウト更新リクエスト
    /// </summary>
    private static void EventEditUpdateLayout()
    {
        // 編集履歴クリア
        EventEditClearResume();

        // 範囲選択解除
        EventEditClearRangeSelect();

        Config.Editer.UpdateScoreLayoutFlag     = true;
        Config.Editer.UpdateCameraFlag          = true;
        Config.Editer.UpdateScoreLineFlag       = true;
        Config.Editer.UpdateScoreHeaderFlag     = true;
        Config.Editer.UpdateScoreFlag           = true;
        Config.Editer.UpdateScoreBgmScaleFlag   = true;

        //Config.Editer.UpdateScoreNoteFlag         = true;
        //Config.Editer.UpdateScoreNotePredictFlag  = true;
        //Config.Editer.UpdateScoreBgmFlag          = true;
        //Config.Editer.UpdateScoreBpmFlag          = true;
        //Config.Editer.UpdateScoreNoteVolumeFlag   = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// エディタパネルリサイズ通知
    /// </summary>
    private static void EventEditPanelResize()
    {
        Config.Editer.UpdateScoreLayoutFlag = true;
        Config.Editer.UpdateCameraFlag = true;
        Config.Editer.UpdateScoreLineFlag = true;
        Config.Editer.UpdateScoreHeaderFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// エディタパネル内のアイテムリサイズ通知
    /// </summary>
    public static void EventEditItemResize()
    {
        Config.Editer.UpdateScoreLayoutFlag = true;
        Config.Editer.UpdateCameraFlag = true;
        Config.Editer.UpdateScoreLineFlag = true;
        Config.Editer.UpdateScoreHeaderFlag = true;
        Config.Editer.UpdateScoreFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// シート移動通知
    /// </summary>
    public static void EventEditUpdateSheetPos()
    {
        Config.Editer.UpdateCameraFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// 範囲選択状態更新通知
    /// </summary>
    /// <param name="aRangeSelect"></param>
    public static void EventEditUpdateRangeSelect( bool aRangeSelect )
    {
        Config.Editer.IsRangeSelect = aRangeSelect;

        ControlAccess.PageEdit?.ReloadConfigEditer();
    }

    /// <summary>
    /// 範囲選択解除リクエスト
    /// </summary>
    public static void EventEditClearRangeSelect()
    {
        Config.Editer.UpdateClearRangeFlag = true;

        ControlAccess.PageEdit?.ReloadConfigEditer();
        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// 波形表示画像更新リクエスト
    /// </summary>
    public static void EventEditUpdateWaveForm()
    {
        Config.Editer.UpdateScoreBgmScaleFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// 音量入力エリア表示更新フラグ
    /// （MidiMapGroup/MidiMap 選択など）
    /// </summary>
    public static void EventEditUpdateDisplayNoteVolume()
    {
        Config.Editer.UpdateScoreNoteVolumeFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// 機械学習 予測更新通知
    /// </summary>
    public static void EventEditUpdateScorePredict()
    {
        Config.Editer.UpdateScoreNotePredictFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// ノート設定／削除
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    public static void EventEditNote( int aMeasureNo )
    {
        // 音量変更に伴いスコア再読み込みリクエスト
        Config.Media.UpdateDmsControlScore = true;

        // NOTE表示更新リクエスト
        Config.Editer.UpdateScoreNoteFlag = true;

        // 表示更新対象の小節番号を登録
        if ( !Config.Editer.UpdateScoreNoteMeasureNoList.Contains( aMeasureNo ) )
        {
            Config.Editer.UpdateScoreNoteMeasureNoList.Add( aMeasureNo );
        }

        // スコアタブ表示更新リクエスト
        EventScoreUpdateTab();

        // プレイヤスコア更新リクエスト
        EventPlayerUpdateScore();
    }

    /// <summary>
    /// BPM設定／削除
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    public static void EventEditBpm( int aMeasureNo )
    {
        // BPM変更に伴うタイムテーブル再計算およびスコア再読み込みリクエスト
        DmsControl.RefreshTimeTable();
        Config.Media.UpdateDmsControlScore = true;

        // BPM表示更新リクエスト
        Config.Editer.UpdateScoreBpmFlag = true;

        // 表示更新対象の小節番号を登録
        if ( !Config.Editer.UpdateScoreBpmMeasureNoList.Contains( aMeasureNo ) )
        {
            Config.Editer.UpdateScoreBpmMeasureNoList.Add( aMeasureNo );
        }

        // プレイヤスコア更新リクエスト
        EventPlayerUpdateScore();
    }

    /// <summary>
    /// フラグ更新：ノートボリューム更新通知
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    public static void EventEditNoteVolume( int aMeasureNo )
    {
        // 音量変更に伴いスコア再読み込みリクエスト
        Config.Media.UpdateDmsControlScore = true;

        // ノート音量表示更新リクエスト
        Config.Editer.UpdateScoreNoteVolumeFlag = true;

        // 表示更新対象の小節番号を登録
        if ( !Config.Editer.UpdateScoreNoteVolumeMeasureNoList.Contains( aMeasureNo ) )
        {
            Config.Editer.UpdateScoreNoteVolumeMeasureNoList.Add( aMeasureNo );
        }

        // スコアタブ表示更新リクエスト
        EventScoreUpdateTab();

        // プレイヤスコア更新リクエスト
        EventPlayerUpdateScore();
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
    public static void EventEditUndo()
    {
        ResumeRequestQueue.Enqueue( EResumeAction.Undo );

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// Redoリクエスト
    /// </summary>
    public static void EventEditRedo()
    {
        ResumeRequestQueue.Enqueue( EResumeAction.Redo );

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// Resumeクリアリクエスト
    /// </summary>
    public static void EventEditClearResume() => ResumeRequestQueue.Enqueue( EResumeAction.Clear );

    #endregion

    #region Event.Score

    /// <summary>
    /// Scoreタブ更新リクエスト
    /// </summary>
    public static void EventScoreUpdateTab()
    {
        Config.Score.UpdateScoreFlag = true;

        // 描画時に更新
        ControlAccess.UCScore?.Refresh();
    }

    #endregion

    #region Event.Player

    /// <summary>
    /// プレイヤー描画モード更新リクエスト
    /// </summary>
    public static void EventPlayerUpdateSufaceMode() =>
        // 描画ループ処理内で更新フラグを見て更新
        Config.Player.UpdateSurfaceModoFlag = true;

    /// <summary>
    /// プレイヤースクリーンサイズ更新通知
    /// </summary>
    public static void EventPlayerUpdateScreenSize()
    {
        // Page側のサイズ更新
        ControlAccess.PagePlayer?.UpdatePageSize();

        // Panel用フラグ更新：描画ループ処理内で更新フラグを見て更新
        Config.Player.UpdateSizeFlag = true;

        // スクリーンサイズ変更に伴う更新
        EventPlayerUpdateScore();
    }

    /// <summary>
    /// Playerスコア更新リクエスト
    /// </summary>
    public static void EventPlayerUpdateScore() => Config.Player.UpdateScoreFlag = true;

    /// <summary>
    /// Player表示切替リクエスト
    /// </summary>
    /// <param name="aDisplayPlayer"></param>
    public static void EventPlayerUpdateDisplay( bool aDisplayPlayer )
    {
        Config.Player.DisplayPlayer = aDisplayPlayer;

        // メニューバーのプレイヤースイッチ切替
        ControlAccess.PageMenuBar?.ReloadConfigPlayer();

        // プレイヤーコントロールの表示切替
        ControlAccess.PageEditerMain?.ReloadPlayer();
    }

    #endregion

    #region Event.Common

    /// <summary>
    /// Windowリサイズ通知
    /// </summary>
    public static void EventResizeWindow()
    {
        // エディタパネルリサイズ
        EventEditPanelResize();

        // スコアタブ表示更新リクエスト
        EventScoreUpdateTab();
    }

    #endregion
}
