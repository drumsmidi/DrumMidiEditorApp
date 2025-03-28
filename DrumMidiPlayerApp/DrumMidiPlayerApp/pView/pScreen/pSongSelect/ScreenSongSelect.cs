using DrumMidiLibrary.pConfig;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace DrumMidiPlayerApp.pView.pScreen.pSongSelect;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class ScreenSongSelect : ScreenBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ScreenSongSelect() : base( false )
    {
    }

    #region Member

    private ScreenSongList? _ScreenSongList;

    /// <summary>
    /// プレイヤー設定（共通）
    /// </summary>
    protected ConfigPlayer DrawSetCom => Config.Player;

    /// <summary>
    /// システム設定
    /// </summary>
    protected ConfigSystem ConfigSystem => Config.System;

    /// <summary>
    /// メディア設定
    /// </summary>
    protected ConfigMedia ConfigMedia => Config.Media;

    #endregion

    #region Request & State

    /// <summary>
    /// リクエスト一覧
    /// </summary>
    public enum Requests : int
    {
        None = 0,
        ScoreReSearch,
        SongListInit,
        SongListSelectMode,
    }

    /// <summary>
    /// 再生状態
    /// </summary>
    public Requests Request { get; set; } = Requests.None;

    /// <summary>
    /// 状態一覧
    /// </summary>
    public enum States : int
    {
        None = 0,
        ScoreSearching,
        SongListInitializing,
        SongListSelectMode,
    }

    /// <summary>
    /// 再生状態
    /// </summary>
    public States State { get; protected set; } = States.None;

    /// <summary>
    /// 再生要求内容（通常再生、ループ再生の判定に使用）
    /// </summary>
    public States StatePre { get; protected set; } = States.None;

    /// <summary>
    /// リクエスト処理
    /// </summary>
    private void ProcRequest()
    {
        var req = Request;

        if ( req == Requests.None )
        {
            return;
        }

        // 前回状態
        StatePre = State;

        // リクエスト処理
        switch ( req )
        {
            case Requests.ScoreReSearch:
                {
                    //_ScreenSongList?.Request = ScreenSongList.Requests.ScoreSearch;

                    State = States.ScoreSearching;
                }
                break;
            case Requests.SongListInit:
                {
                    //_ScreenSongList?.Request = ScreenSongList.Requests.SongListInit;

                    State = States.SongListInitializing;
                }
                break;
            case Requests.SongListSelectMode:
                {
                    if ( _ScreenSongList != null )
                    {
                       // _ScreenSongList.Request = ScreenSongList.Requests.SongListSelectMode;

                        State = States.SongListSelectMode;
                    }
                }
                break;
        }
    }

    #endregion


    #region Input Event

    /// <summary>
    /// 入力イベント処理
    /// </summary>
    private void ProcInputEvent()
    {
        switch ( State )
        {
            case States.SongListSelectMode:
                {
                }
                break;
        }
    }

    #endregion

    #region Frame処理

    protected override void OnLoadSelf()
    {
        AddChildScreen( _ScreenSongList ??= new() );
    }

    protected override void OnUnLoadSelf()
    {
        _ScreenSongList = null;
    }

    protected override bool OnLoadedSelf()
    {
        Request = Requests.SongListSelectMode;

        return true;
    }

    protected override bool OnMoveSelf( double aFrameTime )
    {
        // 子クラス リクエスト処理
        ProcRequest();

        // 子クラス 入力イベント処理
        ProcInputEvent();

        // 子クラス 状態別 フレーム処理
        switch ( State )
        {
            case States.SongListSelectMode:
                {
                    if ( _ScreenSongList?.State == ScreenSongList.States.SongListSelectMode )
                    {
                        State = States.SongListSelectMode;
                    }
                }
                break;
        }

        return true;
    }

    #endregion

    #region 描画処理

    protected override bool OnDrawSelf( CanvasDrawEventArgs aArgs )
    {
        return true;
    }

    #endregion
}
