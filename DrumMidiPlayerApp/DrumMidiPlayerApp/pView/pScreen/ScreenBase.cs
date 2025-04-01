using System.Collections.Generic;
using System.Linq;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace DrumMidiPlayerApp.pView.pScreen;

/// <summary>
/// スクリーンベース
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
public abstract class ScreenBase( bool aProcessing ) : IScreen
{

    #region Screen情報

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigScreenBase DrawSet => Config.ScreenBase;

    /// <summary>
    /// 親スクリーン
    /// </summary>
    private IScreen? _ParentScreen = null;

    public T? GetParentScreen<T>()
    {
        if ( _ParentScreen == null )
        {
            return default;
        }
        if ( _ParentScreen is not T obj )
        {
            return default;
        }

        return obj;
    }

    public void SetParentScreen( IScreen aScreen )
        => _ParentScreen = aScreen;

    /// <summary>
    /// 子スクリーン
    /// </summary>
    private readonly List<IScreen> _ChildScreenList = [];

    public void AddChildScreen( IScreen aScreen )
    {
        aScreen.SetParentScreen( this );
        _ChildScreenList.Add( aScreen );
    }

    /// <summary>
    /// スクリーン描画範囲
    /// </summary>
    public RectClass ScreenDrawRect { get; set; } 
        = new( 0, 0, Config.Panel.BaseScreenSize.Width, Config.Panel.BaseScreenSize.Height );

    /// <summary>
    /// 処理中描画アイテム
    /// </summary>
    private readonly bool _ProcessingEnable = aProcessing;

    /// <summary>
    /// 処理中描画アイテム
    /// </summary>
    private ItemProcessing? _Processing = null;

    #endregion

    #region Request & State

    /// <summary>
    /// リクエスト一覧
    /// </summary>
    private enum Requests : int
    {
        None = 0,
        Load,
        UnLoad,
    }

    /// <summary>
    /// リクエスト
    /// </summary>
    private Requests Request { get; set; } = Requests.Load;

    /// <summary>
    /// リクエスト：アンロード
    /// </summary>
    protected void RequestUnLoad() => Request = Requests.UnLoad;

    /// <summary>
    /// 状態一覧
    /// </summary>
    private enum States : int
    {
        None = 0,
        Loading,
        Loaded,
        UnLoading,
        UnLoaded,
    }

    /// <summary>
    /// 状態
    /// </summary>
    private States State { get; set; } = States.None;

    public bool IsLoaded => State == States.Loaded;

    public bool IsUnLoaded => State == States.UnLoaded;

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

        // リクエスト処理
        switch ( req )
        {
            case Requests.Load:
                {
                    OnLoad();

                    State = States.Loading;
                }
                break;
            case Requests.UnLoad:
                {
                    OnUnLoad();

                    State = States.UnLoading;
                }
                break;
        }
    }

    /// <summary>
    /// ロード処理
    /// </summary>
    private void OnLoad()
    {
        // 子クラスのロード処理
        OnLoadSelf();

        // 親クラスのロード処理
        if ( _ProcessingEnable )
        {
            _Processing ??= new
                (
                    ScreenDrawRect._x + ScreenDrawRect._width  / 2F,
                    ScreenDrawRect._y + ScreenDrawRect._height / 2F,
                    DrawSet.ProcessingRadius,
                    DrawSet.ProcessingRect
                );
        }
    }

    /// <summary>
    /// ロード処理
    /// </summary>
    protected virtual void OnLoadSelf()
    {        
    }

    /// <summary>
    /// アンロード処理
    /// </summary>
    private void OnUnLoad()
    {
        // 子クラスのアンロード処理
        OnUnLoadSelf();

        // 親クラスのアンロード処理
        _Processing?.Dispose();
    }

    /// <summary>
    /// アンロード処理
    /// </summary>
    protected virtual void OnUnLoadSelf()
    {
    }

    #endregion

    #region Input Event

    /// <summary>
    /// 入力変換マップ
    /// </summary>
    protected InputMap _InputMap = new();

    /// <summary>
    /// 入力イベント処理
    /// </summary>
    private void ProcInputEvent()
    {
    }

    #endregion

    #region Frame処理

    public void OnMove( double aFrameTime )
    {
        // 親クラスのリクエスト処理
        ProcRequest();

        // 親クラスの入力イベント処理
        ProcInputEvent();

        // 親クラスのフレーム処理
        switch ( State )
        {
            case States.Loading:
                {
                    _Processing?.Move( aFrameTime );

                    if ( OnLoaded() )
                    {
                        State = States.Loaded;
                    }
                }
                break;
            case States.UnLoading:
                {
                    if ( OnUnLoaded() )
                    {
                        State = States.UnLoaded;
                    }
                }
                break;
        }

        // 子クラスのフレーム処理
        if ( OnMoveSelf( aFrameTime ) )
        {
            // 子スクリーンのフレーム処理
            _ChildScreenList.ForEach( screen => screen.OnMove( aFrameTime ) );
        }

        // キーイベントリセット処理
        if ( _ParentScreen == null )
        {
            InputControl.ResetInputState();
        }
    }

    /// <summary>
    /// ロード完了処理
    /// </summary>
    /// <returns>true:処理完了、false:処理中</returns>
    private bool OnLoaded()
    {
        // 子スクリーンのロード完了チェック
        var flag = _ChildScreenList.Count == 0 || _ChildScreenList.All( screen => screen.IsLoaded == true );

        // 子クラスと子スクリーンのロード完了チェック
        return OnLoadedSelf() && flag;
    }

    /// <summary>
    /// ロード完了処理
    /// </summary>
    /// <returns>true:処理完了、false:処理中</returns>
    protected virtual bool OnLoadedSelf()
    {
        return true;
    }

    /// <summary>
    /// アンロード完了処理
    /// </summary>
    /// <returns>true:処理完了、false:処理中</returns>
    private bool OnUnLoaded()
    {
        // 子スクリーンのアンロード完了チェック
        if ( _ChildScreenList.All( screen => screen.IsUnLoaded == true ) )
        {
            _ChildScreenList.Clear();
        }
        // 子クラスのアンロード完了チェック
        return OnUnLoadedSelf();
    }

    /// <summary>
    /// アンロード完了処理
    /// </summary>
    /// <returns>true:処理完了、false:処理中</returns>
    protected virtual bool OnUnLoadedSelf()
    {
        return true;
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    /// <param name="aFrameTime"></param>
    /// <returns>true:子スクリーンフレーム処理ON、false:子スクリーンフレーム処理OFF</returns>
    protected virtual bool OnMoveSelf( double aFrameTime )
    {
        return true;
    }

    #endregion

    #region 描画処理

    public void OnDraw( CanvasDrawEventArgs aArgs )
    {
        // 親クラスの描画処理
        switch ( State )
        {
            case States.Loading:
                {
                    if ( !_ProcessingEnable )
                    {
                        break;
                    }
                    _Processing?.Draw( aArgs.DrawingSession );
                }
                return;

            case States.UnLoading:
            case States.UnLoaded:
                return;
        }

        // 子クラスの描画処理
        if ( OnDrawSelf( aArgs ) )
        {
            // 子スクリーンの描画処理
            _ChildScreenList.ForEach( screen => screen.OnDraw( aArgs ) );
        }
    }

    /// <summary>
    /// 描画処理
    /// </summary>
    /// <param name="aArgs"></param>
    /// <returns>true:子スクリーン描画ON、false:子スクリーン描画OFF</returns>
    protected virtual bool OnDrawSelf( CanvasDrawEventArgs aArgs )
    {
        return true;
    }

    #endregion
}
