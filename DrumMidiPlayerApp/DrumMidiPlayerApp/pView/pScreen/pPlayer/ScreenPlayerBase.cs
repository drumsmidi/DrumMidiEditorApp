using System;
using System.Threading.Tasks;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pIO;
using DrumMidiPlayerApp.pModel;
using Windows.System;

namespace DrumMidiPlayerApp.pView.pScreen.pPlayer;

/// <summary>
/// スクリーン：プレイヤー
/// </summary>
public abstract class ScreenPlayerBase : ScreenBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ScreenPlayerBase() : base( false )
    {
    }


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

    #endregion

    #region Request & State

    /// <summary>
    /// リクエスト一覧
    /// </summary>
    public enum Requests : int
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
    public Requests Request { get; set; } = Requests.None;

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

    /// <summary>
    /// 前回状態
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

        // リクエストクリア
        Request = Requests.None;

        // 前回状態
        StatePre = State;

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
                    DmsControl.StopPreSequence();

                    State = States.UnLoading;
                }
                break;
            case Requests.UnLoad:
                {
                    DmsControl.StopPreSequence();

                    State = States.UnLoading;
                }
                break;
        }
    }

    /// <summary>
    /// ロード処理
    /// </summary>
    private async void ProcLoadAsync()
    {
        try
        {
            await Task.Run
            (
                () =>
                {
                    ConfigLib.Media.FlagUpdateDmsControlBgm     = true;
                    ConfigLib.Media.FlagUpdateDmsControlMidiMap = true;
                    ConfigLib.Media.FlagUpdateDmsControlScore   = true;

                    DmsControl.RefreshTimeTable();

                    DmsControl.PlayPreSequence();

                    Score = DMS.SCORE.Clone();

                    UpdateScore();
                    UpdateScoreLine();
                    UpdateScoreHeader();

                    ClearMeasure();

                    var measureMaxNo = Score.GetMaxMeasureNo();

                    for ( var measure_no = 0; measure_no <= measureMaxNo; measure_no++ )
                    {
                        UpdateBpmMeasure( measure_no );
                        UpdateScoreMeasure( measure_no );
                    }

                    DmsControl.WaitAudio();

                    SheetPosX       = 0;
                    NotePositionX   = 0;
                    DmsPlayTime     = DmsControl.StartPlayTime;

                    Request = Requests.Play;
                }
            );
        }
        catch ( Exception e )
        {
            Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );

            Request = Requests.Play;
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

    #region Input Event

    /// <summary>
    /// 入力イベント処理
    /// </summary>
    protected virtual void ProcInputEvent()
    {
        switch ( State )
        {
            case States.Playing:
                {
                    var inputStateList = InputControl.GetInputState( _InputMap );

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
                                break;
                        }
                    }
                }
                break;
        }
    }

    #endregion

    #region Frame処理

    protected override void OnLoadSelf()
    {
        // 入力マップ設定
        _InputMap.KeyMap.Clear();
        _InputMap.KeyMap.Add( VirtualKey.Up,        VirtualKey.GamepadDPadUp   );
        _InputMap.KeyMap.Add( VirtualKey.Down,      VirtualKey.GamepadDPadDown );
        _InputMap.KeyMap.Add( VirtualKey.Back,      VirtualKey.GamepadA );
        _InputMap.KeyMap.Add( VirtualKey.Enter,     VirtualKey.GamepadB );
        _InputMap.KeyMap.Add( VirtualKey.Escape,    VirtualKey.Escape );
    }

    protected override bool OnLoadedSelf()
    {
        return true;
    }

    protected override void OnUnLoadSelf()
    {
        // 入力マップ設定クリア
        _InputMap.KeyMap.Clear();
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
            case States.Loading:
                {
                }
                break;
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
                }
                break;
            case States.UnLoading:
                {
                }
                break;
        }

        return true;
    }






    #endregion

    #region 描画処理

    //protected override bool OnDrawSelf( CanvasDrawEventArgs aArgs )
    //{
    //    return true;
    //}

    #endregion

}
