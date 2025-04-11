using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pModel.pOption;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.System;

namespace DrumMidiPlayerApp.pView.pScreen.pOption;

/// <summary>
/// スクリーン：オプション選択
/// </summary>
public class ScreenOptionMain() : ScreenBase( new(){ Processing = true } )
{
    #region Screen情報

    /// <summary>
    /// オプションリストスクロールリスト
    /// </summary>
    private ItemOptionScrollList? _OptionScrollList;

    #endregion

    #region Load/UnLoad処理

    protected override void OnLoadSelf()
    {
        // パッティング設定
        ScreenDrawRect.Trimming( 120 );

        // アイテム：オプションスクロールリスト作成
        _OptionScrollList ??= new();
        _OptionScrollList.DrawRect.X = ScreenDrawRect.X + 20;
        _OptionScrollList.DrawRect.Y = ScreenDrawRect.Y + 20;

        var optionList = new OptionList( "Option" );
        optionList.Add( new( "Option/OptionA" ) );
        optionList.Add( new( "Option/OptionB" ) );
        optionList.Add( new( "Option/OptionB/OptionB1" ) );
        optionList.Add( new( "Option/OptionB/OptionB2" ) );
        optionList.Add( new( "Option/OptionC" ) );

        _OptionScrollList.SetSongList( optionList );
        _OptionScrollList.GoSongList();

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
        _OptionScrollList?.Dispose();
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
                        // オプションスクロールリスト選択状態
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
                                    _OptionScrollList?.PreviewSongList();

                                    SystemSound.SoundPlayMovePrevious();
                                }
                                break;
                            case VirtualKey.GamepadDPadDown:
                                {
                                    _OptionScrollList?.NextSongList();

                                    SystemSound.SoundPlayMoveNext();
                                }
                                break;
                            case VirtualKey.GamepadA:
                                {
                                    _OptionScrollList?.GoBackSongList();

                                    SystemSound.SoundPlayGoBack();
                                }
                                break;
                            case VirtualKey.GamepadB:
                                {
                                    var item = _OptionScrollList?.GoSongList();
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
                    _OptionScrollList?.Move( aFrameTime );
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
                    // 背景描画
                    HelperWin2D.DrawFormatRect
                    ( 
                        aArgs.DrawingSession, 
                        ScreenDrawRect.GetRect(), 
                        Config.ScreenOptionMain.BackgroundRect
                    );

                    // オプションスクロールリスト描画
                    _OptionScrollList?.Draw( aArgs.DrawingSession );
                }
                break;
        }
        return true;
    }

    #endregion

    #region 個別処理

    #endregion
}
