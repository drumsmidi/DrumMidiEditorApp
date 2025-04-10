using System;
using System.Threading.Tasks;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pIO.pDatabase;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pIO;
using DrumMidiPlayerApp.pModel;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.System;

namespace DrumMidiPlayerApp.pView.pScreen.pOption;

/// <summary>
/// スクリーン：曲選択
/// </summary>
public class ScreenOptionMain() : ScreenBase( true )
{
    #region Screen情報

    /// <summary>
    /// 曲リストスクロールリスト
    /// </summary>
    private ItemOptionScrollList? _SongScrollList;

    #endregion

    #region Load/UnLoad処理

    protected override void OnLoadSelf()
    {
        // スクリーンサイズ設定
        ScreenDrawRect.X      = Config.Panel.BaseScreenSize.Width / 2D;
        ScreenDrawRect.Width  = Config.Panel.BaseScreenSize.Width / 2D;
        ScreenDrawRect.Height = Config.Panel.BaseScreenSize.Height;

        // アイテム：曲スクロールリスト作成
        _SongScrollList ??= new();

        // 入力マップ設定
        _InputMap.KeyMap.Clear();
        _InputMap.KeyMap.Add( VirtualKey.Up     , VirtualKey.GamepadDPadUp );
        _InputMap.KeyMap.Add( VirtualKey.Down   , VirtualKey.GamepadDPadDown );
        _InputMap.KeyMap.Add( VirtualKey.Back   , VirtualKey.GamepadA );
        _InputMap.KeyMap.Add( VirtualKey.Enter  , VirtualKey.GamepadB );
        _InputMap.KeyMap.Add( VirtualKey.Escape , VirtualKey.Escape );
    }

    protected override bool OnLoadedSelf()
    {
        return true;
    }

    protected override void OnUnLoadSelf()
    {
        // アイテム：曲スクロールリスト破棄
        _SongScrollList?.Dispose();
    }

    #endregion

    #region State

    /// <summary>
    /// 状態一覧
    /// </summary>
    public enum States : int
    {
        None = 0,
        OptionSelectMode,
    }

    /// <summary>
    /// 状態
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
        OptionSelectMode,
        Close,
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

        // リクエスト処理
        switch ( req )
        {
            case Requests.OptionSelectMode:
                {
                    OnActivate( true );

                    State = States.OptionSelectMode;
                }
                break;
            case Requests.Close:
                {
                    OnActivate( false );

                    State = States.None;
                }
                break;
        }
    }

    #endregion

    #region Activate処理

    protected override void OnActivateSelf( bool aActivate )
    {
        switch ( State )
        {
            case States.None:
                {
                    if ( aActivate )
                    {
                        // 曲スクロールリスト選択状態
                        Request = Requests.OptionSelectMode;
                    }
                }
                break;
        }
    }

    #endregion

    #region Input Event

    protected override void OnInputEventSelf( InputMap aInputMap )
    {
        switch ( State )
        {
            case States.OptionSelectMode:
                {
                    var inputStateList = InputControl.GetInputState( aInputMap );

                    foreach ( var state in inputStateList )
                    {
                        switch ( state.MapKey )
                        {
                            case VirtualKey.GamepadDPadUp:
                                {
                                    _SongScrollList?.PreviewSongList();

                                    SystemSound.SoundPlayMovePrevious();
                                }
                                break;
                            case VirtualKey.GamepadDPadDown:
                                {
                                    _SongScrollList?.NextSongList();

                                    SystemSound.SoundPlayMoveNext();
                                }
                                break;
                            case VirtualKey.GamepadA:
                                {
                                    _SongScrollList?.GoBackSongList();

                                    SystemSound.SoundPlayGoBack();
                                }
                                break;
                            case VirtualKey.GamepadB:
                                {
                                    var item = _SongScrollList?.GoSongList();

                                    SystemSound.SoundPlayFocus();

                                }
                                break;
                            case VirtualKey.Escape:
                                {
                                    Request = Requests.Close;
                                }
                                return;
                        }
                    }
                }
                break;
        }
    }

    #endregion

    #region Frame処理

    protected override bool OnMoveSelf( double aFrameTime )
    {
        // 子クラス 状態別 フレーム処理
        switch ( State )
        {
            case States.OptionSelectMode:
                {
                    _SongScrollList?.Move( aFrameTime );
                }
                break;
        }

        return true;
    }

    #endregion

    #region 描画処理

    protected override bool OnDrawSelf( CanvasDrawEventArgs aArgs )
    {
        switch ( State )
        {
            case States.OptionSelectMode:
                {
                    HelperWin2D.DrawFormatRect
                    ( 
                        aArgs.DrawingSession, 
                        ScreenDrawRect.GetRect(), 
                        Config.ScreenBase.ProcessingRect,
                        string.Empty
                    );

                    _SongScrollList?.Draw( aArgs.DrawingSession );
                }
                break;
        }
        return true;
    }

    #endregion

    #region 個別処理

    #endregion
}
