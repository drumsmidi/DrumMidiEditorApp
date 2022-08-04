using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pView;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// 設定情報
/// </summary>
public static class Config
{
    /// <summary>
    /// システム設定
    /// </summary>
    public static ConfigSystem System { get; set; } = new();

    /// <summary>
    /// 機械学習用設定
    /// </summary>
    public static ConfigMachine Machine { get; set; } = new();

    /// <summary>
    /// Audio/Midi/Video設定
    /// </summary>
    public static ConfigMedia Media { get; set; } = new();

    /// <summary>
    /// イコライザ設定
    /// </summary>
    public static ConfigEqualizer Equalizer { get; set; } = new();

    /// <summary>
    /// Editerタブ設定
    /// </summary>
    public static ConfigEditer Editer { get; set; } = new();

    /// <summary>
    /// Player設定
    /// </summary>
    public static ConfigPlayer Player { get; set; } = new();

    /// <summary>
    /// Scoreタブ設定
    /// </summary>
    public static ConfigScore Score { get; set; } = new();

    /// <summary>
    /// Scaleタブ設定
    /// </summary>
    public static ConfigScale Scale { get; set; } = new();

    #region Event

    /// <summary>
    /// フラグ更新：スコア再読込
    /// </summary>
    public static void EventReloadScore()
    {
        Media.UpdateDmsControlBgm			= true;
        Media.UpdateDmsControlScore		    = true;
        Media.UpdateDmsControlMidiMap		= true;
        DmsControl.RefreshTimeTable();

        Editer.UpdateCameraFlag             = true;
		Editer.UpdateScoreLayoutFlag        = true;
        Editer.UpdateScoreLineFlag		    = true;
        Editer.UpdateScoreHeaderFlag		= true;
        Editer.UpdateScoreBgmFlag		    = true;
        Editer.UpdateScoreBgmScaleFlag      = false;
        Editer.UpdateScoreBpmFlag		    = true;
		Editer.UpdateScoreFlag              = true;
		Player.UpdateScoreFlag			    = true;
		Score.UpdateScoreFlag			    = true;

        ControlAccess.PageMusic?.ReloadMusicInfo();
        ControlAccess.UCMidiMapPanel?.ReloadMidiMapSet();
        ControlAccess.UCKeyChangePanel?.ReloadMidiMapNoteList();
    }

    /// <summary>
    /// フラグ更新：チャンネル切替
    /// </summary>
    public static void EventChangeChannel()
    {
        Editer.UpdateCameraFlag             = true;
		Editer.UpdateScoreLayoutFlag        = true;
        Editer.UpdateScoreLineFlag		    = true;
        Editer.UpdateScoreHeaderFlag		= true;
        Editer.UpdateScoreBgmFlag		    = false;
        Editer.UpdateScoreBgmScaleFlag      = true;
        Editer.UpdateScoreFlag              = true;
		Player.UpdateScoreFlag			    = true;
		Score.UpdateScoreFlag			    = true;
    }

    /// <summary>
    /// フラグ更新：MidiMapセット再読込
    /// </summary>
    public static void EventReloadMidiMapSet()
    {
        Media.UpdateDmsControlScore		    = true;
        Media.UpdateDmsControlMidiMap		= true;
        Editer.UpdateCameraFlag             = true;
		Editer.UpdateScoreLayoutFlag        = true;
        Editer.UpdateScoreLineFlag		    = true;
        Editer.UpdateScoreHeaderFlag		= true;
		Editer.UpdateScoreFlag              = true;
		Player.UpdateScoreFlag			    = true;
		Score.UpdateScoreFlag			    = true;

        ControlAccess.UCMidiMapPanel?.ReloadMidiMapSet();
        ControlAccess.UCKeyChangePanel?.ReloadMidiMapNoteList();

        // 履歴クリア
        EventClearEditerRangeSelect();
    }

    /// <summary>
    /// フラグ更新：MidiMapKey変更
    /// </summary>
    public static void EventChangeMidiMapKey()
    {
        Media.UpdateDmsControlScore		    = true;
        Editer.UpdateScoreFlag		        = true;
		Player.UpdateScoreFlag			    = true;
		Score.UpdateScoreFlag			    = true;

        ControlAccess.UCKeyChangePanel?.ReloadMidiMapNoteList();
    }

    /// <summary>
    /// フラグ更新：BGM変更／再読込
    /// </summary>
    public static void EventReloadBgm()
    {
        Media.UpdateDmsControlBgm	    = true;
        Media.UpdateDmsControlScore	    = true;
        DmsControl.RefreshTimeTable();

        Editer.UpdateScoreBgmFlag       = true;
        Editer.UpdateScoreBgmScaleFlag  = false;
    }

    /// <summary>
    /// フラグ更新：機械学習 予測
    /// </summary>
    public static void EventUpdateScorePredict()
    {
        Editer.UpdateScoreNotePredictFlag = true;
    }

    /// <summary>
    /// フラグ更新：ノートボリューム変更
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    public static void EventEditNoteVolume( int aMeasureNo )
    {
        Media.UpdateDmsControlScore		  = true;
        Editer.UpdateScoreNoteVolumeFlag  = true;
        Player.UpdateScoreFlag            = true;
        Score.UpdateScoreFlag             = true;

        if ( !Editer.UpdateScoreNoteVolumeMeasureNoList.Contains( aMeasureNo ) )
        {
            Editer.UpdateScoreNoteVolumeMeasureNoList.Add( aMeasureNo );
        }
    }

    /// <summary>
    /// フラグ更新：ノート設定／削除
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    public static void EventEditNote( int aMeasureNo )
    {
        Media.UpdateDmsControlScore		 = true;
        Editer.UpdateScoreNoteFlag       = true;
        Player.UpdateScoreFlag           = true;
        Score.UpdateScoreFlag            = true;

        if ( !Editer.UpdateScoreNoteMeasureNoList.Contains( aMeasureNo ) )
        {
            Editer.UpdateScoreNoteMeasureNoList.Add( aMeasureNo );
        }
    }

    /// <summary>
    /// フラグ更新：BPM設定／削除
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    public static void EventEditBpm( int aMeasureNo )
    {
        Media.UpdateDmsControlScore		= true;
        DmsControl.RefreshTimeTable();

        Editer.UpdateScoreBpmFlag       = true;
        Player.UpdateScoreFlag          = true;
        Score.UpdateScoreFlag           = true;

        if ( !Editer.UpdateScoreBpmMeasureNoList.Contains( aMeasureNo ) )
        {
            Editer.UpdateScoreBpmMeasureNoList.Add( aMeasureNo );
        }
    }

    /// <summary>
    /// フラグ更新：MidiMapGroup/MidiMap 選択
    /// </summary>
    public static void EventUpdateEditerDisplayNoteVolume()
    {
        Editer.UpdateScoreNoteVolumeFlag = true;
    }

    /// <summary>
    /// フラグ更新：Editerタブ　ライン更新
    /// </summary>
    public static void EventUpdateEditerLine()
    {
        Editer.UpdateScoreLineFlag = true;
    }

    /// <summary>
    /// フラグ更新：Windowリサイズ
    /// </summary>
    public static void EventWindowResize()
    {
        Editer.UpdateScoreLayoutFlag    = true;
        Editer.UpdateScoreLineFlag      = true;
        Score.UpdateScoreFlag           = true;
    }

    #region Event.Edit

    /// <summary>
    /// フラグ更新：Editerタブ　シート移動
    /// </summary>
    public static void EventUpdateEditerSheetPos()
    {
        Editer.UpdateCameraFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// フラグ更新：Editerタブ　ヘッダ更新／ノート間隔更新
    /// </summary>
    public static void EventUpdateEditerSize()
    {
        Editer.UpdateScoreLayoutFlag    = true;
        Editer.UpdateScoreLineFlag      = true;
        Editer.UpdateScoreHeaderFlag    = true;
        Editer.UpdateScoreFlag          = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// フラグ更新：Editerタブ 波形表示画像更新
    /// </summary>
    public static void EventUpdateEditerWaveForm()
    {
        Editer.UpdateScoreBgmScaleFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// フラグ更新：Editerタブ　範囲選択クリア
    /// </summary>
    public static void EventUpdateEditerRangeSelect()
    {
        ControlAccess.PageEdit?.ReloadConfigEditer();
    }

    /// <summary>
    /// フラグ更新：Editerタブ　範囲選択クリア
    /// </summary>
    public static void EventClearEditerRangeSelect()
    {
        Editer.UpdateClearRangeFlag = true;

        ControlAccess.PageEdit?.ReloadConfigEditer();
        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// フラグ更新：Editerタブ　Undo
    /// </summary>
    public static void EventEditerUndo()
    {
        Editer.UpdateUndoFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    /// <summary>
    /// フラグ更新：Editerタブ　Redo
    /// </summary>
    public static void EventEditerRedo()
    {
        Editer.UpdateRedoFlag = true;

        ControlAccess.PageEdit?.Refresh();
    }

    #endregion

    #region Event.Score

    /// <summary>
    /// Scoreタブ更新
    /// </summary>
    public static void EventUpdateScoreTab()
    {
        Score.UpdateScoreFlag = true;

        ControlAccess.UCScore?.Refresh();
    }

    #endregion

    #region Event.Player

    /// <summary>
    /// フラグ更新：Player　更新
    /// </summary>
    public static void EventUpdatePlayer()
    {
        Player.UpdateScoreFlag = true;
    }

    /// <summary>
    /// Player表示切替通知
    /// </summary>
    public static void EventDisplayPlayer()
    {
        ControlAccess.PageMenuBar?.ReloadConfigPlayer();
        ControlAccess.PageEditerMain?.ReloadPlayer();
    }

    #endregion

    #endregion
}
