using DrumMidiLibrary.pInput;
using DrumMidiPlayerApp.pView.pScreen.pSongList;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace DrumMidiPlayerApp.pView.pScreen;

/// <summary>
/// スクリーン：メイン
/// </summary>
public class ScreenMain() : ScreenBase( false )
{
    #region Screen情報

    /// <summary>
    /// スクリーン：曲選択
    /// </summary>
    private ScreenSongList? _ScreenSongList;

    #endregion

    #region Load/UnLoad処理

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

    #endregion

    #region State

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

    #endregion

    #region Request

    /// <summary>
    /// リクエスト一覧
    /// </summary>
    private enum Requests : int
    {
        None = 0,
    }

    /// <summary>
    /// 再生状態
    /// </summary>
    private Requests Request { get; set; } = Requests.None;

    protected override void OnRequestSelf()
    {
        var req = Request;

        if ( req == Requests.None )
        {
            return;
        }

        // リクエストクリア
        Request = Requests.None;
    }

    #endregion

    #region Activate処理

    protected override void OnActivateSelf( bool aActivate )
    {
    }

    #endregion

    #region Input Event

    protected override void OnInputEventSelf( InputMap aInputMap )
    {
    }

    #endregion

    #region Frame処理

    protected override bool OnMoveSelf( double aFrameTime )
    {
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
