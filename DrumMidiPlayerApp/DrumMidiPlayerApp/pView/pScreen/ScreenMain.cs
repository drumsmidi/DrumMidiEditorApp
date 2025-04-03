using DrumMidiPlayerApp.pView.pScreen.pSongList;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace DrumMidiPlayerApp.pView.pScreen;

/// <summary>
/// メインスクリーン
/// </summary>
public class ScreenMain : ScreenBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ScreenMain() : base( false )
    {
    }

    #region Member

    /// <summary>
    /// スクリーン：曲選択
    /// </summary>
    private ScreenSongList? _ScreenSongList;

    #endregion

    #region Request & State

    /// <summary>
    /// リクエスト一覧
    /// </summary>
    public enum Requests : int
    {
        None = 0,
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
    }

    #endregion


    #region Input Event

    /// <summary>
    /// 入力イベント処理
    /// </summary>
    private void ProcInputEvent()
    {
    }

    #endregion

    #region Frame処理

    protected override void OnLoadSelf()
    {
        AddChildScreen( _ScreenSongList ?? new ScreenSongList() );
    }

    protected override bool OnLoadedSelf()
    {
        return true;
    }

    protected override void OnUnLoadSelf()
    {
        _ScreenSongList = null;
    }

    protected override bool OnUnLoadedSelf()
    {
        return true;
    }

    protected override bool OnMoveSelf( double aFrameTime )
    {
        // 子クラス リクエスト処理
        ProcRequest();

        // 子クラス 入力イベント処理
        ProcInputEvent();

        // 子クラス 状態別 フレーム処理

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
