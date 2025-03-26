using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pModel;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pScreen.pSongSelect;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class ScreenSongSelect : IScreen
{
    #region Member

    private ScreenSongList _ScreenSongList = new();

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
    protected Score Score = new();

    /// <summary>
    /// スクリーンサイズ
    /// </summary>
    protected Size _ScreenSize = new();

    /// <summary>
    /// ノート位置（絶対値）小数点あり
    /// </summary>
    protected float _SheetPosX = 0;

    /// <summary>
    /// ノート位置（絶対値）
    /// </summary>
    protected int _NotePositionX = 0;

    /// <summary>
    /// 再生開始処理後の現在の再生時間（秒）
    /// </summary>
    protected double _DmsPlayTime = 0.0D;

    #endregion

    #region Request & State

    /// <summary>
    /// リクエスト一覧
    /// </summary>
    public enum Requests : int
    {
        None = 0,
        Load,
        ScoreReSearch,
        SongListInit,
        SongListSelectMode,
        UnLoad,
    }

    /// <summary>
    /// 再生状態
    /// </summary>
    public Requests Request { get; set; } = Requests.Load;

    /// <summary>
    /// 状態一覧
    /// </summary>
    public enum States : int
    {
        None = 0,
        Loading,
        ScoreSearching,
        SongListInitializing,
        SongListSelectMode,
        UnLoading,
    }

    /// <summary>
    /// 再生状態
    /// </summary>
    public States State { get; protected set; } = States.None;

    /// <summary>
    /// 再生要求内容（通常再生、ループ再生の判定に使用）
    /// </summary>
    public States StatePre { get; protected set; } = States.None;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ScreenSongSelect()
    {
    }

    #region Key & Mouse Event

    public virtual void KeyDown( object aSender, KeyRoutedEventArgs aArgs )
    {
        _ScreenSongList.KeyDown( aSender, aArgs );
    }

    public virtual void KeyUp( object aSender, KeyRoutedEventArgs aArgs )
    {
        _ScreenSongList.KeyUp( aSender, aArgs );
    }

    public virtual void MouseDown( object aSender, PointerRoutedEventArgs aArgs )
    {
        _ScreenSongList.MouseDown( aSender, aArgs );
    }

    public virtual void MouseMove( object aSender, PointerRoutedEventArgs aArgs )
    {
        _ScreenSongList.MouseMove( aSender, aArgs );
    }

    public virtual void MouseUp( object aSender, PointerRoutedEventArgs aArgs )
    {
        _ScreenSongList.MouseUp( aSender, aArgs );
    }

    #endregion

    #region Frame処理

    public virtual bool OnMove( double aFrameTime )
    {
        #region リクエスト処理
        {
            var req = Request;

            if (  req != Requests.None )
            {
                // リクエストクリア
                Request = Requests.None;

                // 前回状態
                StatePre = State;

                switch ( req )
                {
                    case Requests.Load:
                        {
                            _ScreenSongList.Request = ScreenSongList.Requests.Load;

                            State = States.Loading;
                        }
                        break;
                    case Requests.ScoreReSearch:
                        {
                            _ScreenSongList.Request = ScreenSongList.Requests.ScoreSearch;

                            State = States.ScoreSearching;
                        }
                        break;
                    case Requests.SongListInit:
                        {
                            _ScreenSongList.Request = ScreenSongList.Requests.SongListInit;

                            State = States.SongListInitializing;
                        }
                        break;
                    case Requests.SongListSelectMode:
                        {
                            _ScreenSongList.Request = ScreenSongList.Requests.SongListSelectMode;

                            State = States.SongListSelectMode;
                        }
                        break;
                }
            }
        }
        #endregion

        #region 子スクリーン フレーム処理
        {
            _ScreenSongList.OnMove( aFrameTime );
        }
        #endregion

        #region 処理状態別 処理
        {
            switch ( State )
            {
                case States.Loading:
                case States.ScoreSearching:
                case States.SongListInitializing:
                    {
                        if ( _ScreenSongList.State == ScreenSongList.States.SongListSelectMode )
                        {
                            State = States.SongListSelectMode;
                        }
                    }
                    break;
            }
        }
        #endregion

        return true;
    }

    #endregion

    #region 描画処理

    public virtual bool OnDraw( CanvasDrawEventArgs aArgs )
    {
        // SwapChainの描画セッション作成時に背景色指定済み
        //aArgs.DrawingSession.Clear( DrawSetCom.SheetColor.Color );

        _ScreenSongList?.OnDraw( aArgs );

        return true;
    }

    #endregion

    #region その他

    public virtual void SetParentScreen( IScreen aScreen )
    {
    }

    #endregion
}
