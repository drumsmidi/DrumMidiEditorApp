using System;
using System.Threading.Tasks;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pModel;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Devices.Display.Core;
using Windows.Foundation;
using Windows.System;

namespace DrumMidiPlayerApp.pView.pScreen.pPlayer;

/// <summary>
/// スクリーン：プレイヤー
/// </summary>
public abstract class ScreenPlayerBase() : ScreenBase( new() )
{
    #region Screen情報

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

    /// <summary>
    /// スコア
    /// </summary>
    protected Score Score { get; private set; } = new();

    /// <summary>
    /// ノート位置（絶対値）小数点あり
    /// </summary>
    protected float SheetPosX { get; private set; } = 0;

    /// <summary>
    /// ノート位置（絶対値）
    /// </summary>
    protected int NotePositionX { get; private set; } = 0;

    /// <summary>
    /// 再生開始処理後の現在の再生時間（秒）
    /// </summary>
    protected double DmsPlayTime { get; private set; } = 0.0D;

    /// <summary>
    /// 小節開始番号
    /// </summary>
    private int _MeasureNoStart = 0;

    /// <summary>
    /// 小節終了番号
    /// </summary>
    private int _MeasureNoEnd = 0;

    /// <summary>
    /// ロード中に描画する画像
    /// </summary>
    private IAsyncOperation<CanvasBitmap>? _LoadingImage;

    #endregion

    #region Load/UnLoad処理

    protected override void OnLoadSelf()
    {
        // スクリーンサイズ設定
        //ScreenDrawRect.X      = 0;
        //ScreenDrawRect.Y      = 0;        
        //ScreenDrawRect.Width  = Config.Panel.BaseScreenSize.Width;
        //ScreenDrawRect.Height = Config.Panel.BaseScreenSize.Height;

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
        // ロード中画像を閉じる
        _LoadingImage?.Close();
    }

    #endregion

    #region State

    /// <summary>
    /// 状態一覧
    /// </summary>
    public enum States : int
    {
        None = 0,
        Loading,
        Playing,
        UnLoading,
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
        Load,
        Play,
        PlayEnd,
        UnLoad,
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
            case Requests.Load:
                {
                    ProcLoadAsync();

                    State = States.Loading;
                }
                break;
            case Requests.Play:
                {
                    State = States.Playing;
                }
                break;
            case Requests.PlayEnd:
                {
                    Request = Requests.UnLoad;
                }
                break;
            case Requests.UnLoad:
                {
                    DmsControl.StopPreSequence();

                    OnActivate( false );

                    State = States.UnLoading;
                }
                break;
        }
    }

    #endregion

    #region Activate処理

    protected override void OnActivateSelf( bool aActivate )
    {
        if ( aActivate )
        {
            // ロードリクエスト
            Request = Requests.Load;
        }
    }

    #endregion

    #region Input Event

    protected override void OnInputEventSelf( InputMap aInputMap )
    {
        switch ( State )
        {
            case States.Playing:
                {
                    var inputStateList = InputControl.GetInputState( aInputMap );

                    foreach ( var state in inputStateList )
                    {
                        switch ( state.MapKey )
                        {
                            case VirtualKey.GamepadDPadUp:
                                {
                                }
                                break;
                            case VirtualKey.GamepadDPadDown:
                                {
                                }
                                break;
                            case VirtualKey.GamepadA:
                                {
                                }
                                break;
                            case VirtualKey.GamepadB:
                                {
                                }
                                break;
                            case VirtualKey.Escape:
                                {
                                    Request = Requests.UnLoad;
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
            case States.Playing:
                {
                    // Calc sheet position
                    DmsPlayTime = DmsControl.PlayTime;

                    var note_pos = DmsControl.PlayNote( DmsPlayTime );

                    SheetPosX = note_pos;

                    var limit_width = ConfigSystem.NoteCount;

                    if ( SheetPosX > limit_width )
                    {
                        SheetPosX = limit_width;
                    }

                    NotePositionX = (int)note_pos;

                    if ( SheetPosX > _MeasureNoEnd * ConfigSystem.MeasureNoteNumber )
                    {
                        Request = Requests.UnLoad;
                    }
                }
                break;
            case States.UnLoading:
                {
                    State = States.None;
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
            case States.Loading:
                {
                    // ロード中画像を表示
                    _LoadingImage ??= CanvasBitmap.LoadAsync( aArgs.DrawingSession, new Uri( "ms-appx:///Images/ScreenPlayerBase_Loading.png" ) );

                    if ( _LoadingImage.Status == AsyncStatus.Completed )
                    {
                        aArgs.DrawingSession.DrawImage( _LoadingImage.GetResults(), ScreenDrawRect.GetRect() );
                    }
                }
                return false;
        }
        return true;
    }

    #endregion

    #region 更新処理

    /// <summary>
    /// ロード処理
    /// </summary>
    private async void ProcLoadAsync()
    {
        try
        {
            await Task.Run
            (
                async () =>
                {
                    #region DmsControlへのリクエスト

                    // DmsControl用のフラグを設定
                    ConfigLib.Media.FlagUpdateDmsControlBgm     = true;
                    ConfigLib.Media.FlagUpdateDmsControlMidiMap = true;
                    ConfigLib.Media.FlagUpdateDmsControlScore   = true;

                    DmsControl.RefreshTimeTable();
                    DmsControl.PlayPreSequence();

                    #endregion

                    // スコア情報をコピー
                    Score = DMS.SCORE.Clone();

                    UpdateScore();
                    UpdateScoreLine();
                    UpdateScoreHeader();

                    _MeasureNoStart = 0;
                    _MeasureNoEnd   = Score.GetMaxMeasureNo();

                    ClearMeasure();

                    for ( var measure_no = _MeasureNoStart; measure_no <= _MeasureNoEnd; measure_no++ )
                    {
                        UpdateBpmMeasure( measure_no );
                        UpdateScoreMeasure( measure_no );
                    }

                    #region DmsControlの完了待

                    while ( DmsControl.PlayerReq != DmsControl.PlayerRequest.PrePlay )
                    {
                        await Task.Delay( 1 );
                    }
                    DmsControl.PlayerReq = DmsControl.PlayerRequest.None;

                    DmsControl.WaitAudio();

                    #endregion

                    SheetPosX = 0;
                    NotePositionX   = 0;
                    DmsPlayTime     = DmsControl.StartPlayTime;

                    Request = Requests.Play;

                    Log.Info( $"{Log.GetThisMethodName}:Load OK" );
                }
            );
        }
        catch ( Exception e )
        {
            Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );

            Request = Requests.UnLoad;
        }
    }

    /// <summary>
    /// スコア範囲設定更新
    /// </summary>
    protected virtual void UpdateScore()
    {
    }

    /// <summary>
    /// 小節線表示更新
    /// </summary>
	protected virtual void UpdateScoreLine()
    {
    }

    /// <summary>
    /// ヘッダ表示更新
    /// </summary>
    protected virtual void UpdateScoreHeader()
    {
    }

    /// <summary>
    /// 小節NOTE／BPM表示設定クリア
    /// </summary>
    protected virtual void ClearMeasure()
    {
    }

    /// <summary>
    /// 小節NOTE表示更新
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    protected virtual void UpdateScoreMeasure( int aMeasureNo )
    {
    }

    /// <summary>
    /// 小節BPM表示更新
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    protected virtual void UpdateBpmMeasure( int aMeasureNo )
    {
    }

    #endregion
}
