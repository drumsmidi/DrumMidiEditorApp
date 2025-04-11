using System.Collections.Generic;
using System.Linq;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace DrumMidiPlayerApp.pView.pScreen;

/// <summary>
/// スクリーン基底クラス
/// </summary>
/// <param name="aScreenBaseOption">オプション設定</param>
public abstract class ScreenBase( ScreenBaseOption aScreenBaseOption ) : IScreen
{
    #region Screen情報

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigScreenBase DrawSet => Config.ScreenBase;

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPanel DrawSetPanel => Config.Panel;

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

    public IScreen? GetParentScreen() => _ParentScreen;

    public void SetParentScreen( IScreen aScreen ) => _ParentScreen = aScreen;

    /// <summary>
    /// 子スクリーン
    /// </summary>
    private readonly List<IScreen> _ChildScreenList = [];

    /// <summary>
    /// 子スクリーン追加
    /// </summary>
    /// <param name="aScreen"></param>
    protected void AddChildScreen( IScreen aScreen )
    {
        // 親スクリーンの描画範囲を子スクリーンに設定
        aScreen.SetScreenDrawRect( ScreenDrawRect );

        // 親スクリーンを設定
        aScreen.SetParentScreen( this );

        _ChildScreenList.Add( aScreen );
    }

    public void SetScreenDrawRect( RectClass aRectClass )
    {
        ScreenDrawRect.X        = aRectClass.X;
        ScreenDrawRect.Y        = aRectClass.Y;
        ScreenDrawRect.Width    = aRectClass.Width;
        ScreenDrawRect.Height   = aRectClass.Height;
    }

    /// <summary>
    /// スクリーン描画範囲
    /// </summary>
    protected RectClass ScreenDrawRect { get; private set; } = new( 0, 0, 0, 0 );

    /// <summary>
    /// 処理中描画アイテム表示フラグ
    /// </summary>
    private readonly bool _ProcessingEnable = aScreenBaseOption.Processing;

    /// <summary>
    /// 処理中描画アイテム
    /// </summary>
    private ItemProcessing? _Processing = null;

    #endregion

    #region Load/UnLoad処理

    /// <summary>
    /// ロード処理
    /// </summary>
    private void OnLoad()
    {
        // スクリーン描画範囲の設定（パネルサイズを初期設定）
        ScreenDrawRect.Width  = DrawSetPanel.BaseScreenSize.Width;
        ScreenDrawRect.Height = DrawSetPanel.BaseScreenSize.Height;

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
    /// ロード処理（子クラス用）
    /// </summary>
    protected virtual void OnLoadSelf()
    {
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
    /// ロード完了処理（子クラス用）
    /// </summary>
    /// <returns>true:処理完了、false:処理中</returns>
    protected virtual bool OnLoadedSelf()
    {
        return true;
    }

    /// <summary>
    /// アンロード処理
    /// </summary>
    private void OnUnLoad()
    {
        // 子クラスのアンロード処理
        OnUnLoadSelf();

        // 入力マップ設定クリア
        _InputMap.KeyMap.Clear();

        // 親クラスのアンロード処理
        _Processing?.Dispose();
    }

    /// <summary>
    /// アンロード処理（子クラス用）
    /// </summary>
    protected virtual void OnUnLoadSelf()
    {
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
    /// アンロード完了処理（子クラス用）
    /// </summary>
    /// <returns>true:処理完了、false:処理中</returns>
    protected virtual bool OnUnLoadedSelf()
    {
        return true;
    }

    #endregion

    #region State

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

    /// <summary>
    /// 親クラスロード完了状態
    /// </summary>
    public bool IsLoaded => State == States.Loaded;

    /// <summary>
    /// 親クラスアンロード完了状態
    /// </summary>
    public bool IsUnLoaded => State == States.UnLoaded;

    #endregion

    #region Request

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
    /// リクエスト処理
    /// </summary>
    private void OnRequest()
    {
        // 親クラスのリクエスト処理
        var req = Request;

        if ( req != Requests.None )
        {
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

        // 子クラスのリクエスト処理
        OnRequestSelf();
    }

    /// <summary>
    /// ロード処理（子クラス用）
    /// </summary>
    protected virtual void OnRequestSelf()
    {
    }

    #endregion

    #region Activate処理

    /// <summary>
    /// アクティブ化状態フラグ
    /// </summary>
    protected bool IsActivate { get; private set; } = false;

    public void OnActivate( bool aActivate )
    {
        IsActivate = aActivate;

        OnActivateSelf( aActivate );

        if ( !aActivate )
        {
            // 親スクリーンをアクティブ化
            GetParentScreen()?.OnActivate( !aActivate );
        }
    }

    /// <summary>
    /// アクティブ化／非アクティブ化 イベント（子クラス用）
    /// </summary>
    protected virtual void OnActivateSelf( bool aActivate )
    {
    }

    #endregion

    #region Input処理

    /// <summary>
    /// 入力変換マップ
    /// </summary>
    protected InputMap _InputMap = new();

    /// <summary>
    /// 入力イベント処理
    /// </summary>
    private void OnInputEvent()
    {
        if ( IsActivate )
        {
            // 子クラスの入力イベント処理
            OnInputEventSelf( _InputMap );
        }
    }

    /// <summary>
    /// 入力イベント処理（子クラス用）
    /// </summary>
    protected virtual void OnInputEventSelf( InputMap aInputMap )
    {
    }

    #endregion

    #region Frame処理

    public void OnMove( double aFrameTime )
    {
        // 親子クラスのリクエスト処理
        OnRequest();

        // 親子クラスの入力イベント処理
        OnInputEvent();

        // 親クラスのフレーム処理
        switch ( State )
        {
            case States.Loading:
                {
                    if ( _ProcessingEnable )
                    {
                        _Processing?.Move( aFrameTime );
                    }

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
                    if ( _ProcessingEnable )
                    {
                        _Processing?.Draw( aArgs.DrawingSession );
                    }
                }
                break;

            case States.Loaded:
                {
                    // 子クラスの描画処理
                    if ( OnDrawSelf( aArgs ) )
                    {
                        // 子スクリーンの描画処理
                        _ChildScreenList.ForEach( screen => screen.OnDraw( aArgs ) );
                    }
                }
                break;
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
