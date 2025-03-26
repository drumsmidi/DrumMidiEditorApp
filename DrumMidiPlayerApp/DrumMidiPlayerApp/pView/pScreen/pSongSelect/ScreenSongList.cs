using System;
using System.Threading.Tasks;
using DrumMidiLibrary.pIO.pDatabase;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pScreen.pSongSelect;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class ScreenSongList : IScreen
{
    #region Member

    /// <summary>
    /// 親スクリーン
    /// </summary>
    private ScreenSongSelect? _ParentScreen;

    /// <summary>
    /// スクリーン位置：X座標
    /// </summary>
    public float ScreenPosX { get; set; } = 0;

    /// <summary>
    /// スクリーン位置：Y座標
    /// </summary>
    public float ScreenPosY { get; set; } = 0;

    /// <summary>
    /// スクリーンサイズ
    /// </summary>
    public Size ScreenSize { get; set; } = new( Config.Panel.BaseScreenSize.Width / 2F, Config.Panel.BaseScreenSize.Height );

    /// <summary>
    /// 処理中アイテム
    /// </summary>
    private ItemProcessing? _Processing;

    /// <summary>
    /// 曲リストスクロールリスト
    /// </summary>
    private ItemSongScrollList? _SongScrollList;

    #endregion

    #region Request & State

    /// <summary>
    /// リクエスト一覧
    /// </summary>
    public enum Requests : int
    {
        None = 0,
        Load,
        ScoreSearch,
        SongListInit,
        SongListSelectMode,
        UnLoad,
    }

    /// <summary>
    /// 再生状態
    /// </summary>
    public Requests Request { get; set; } = Requests.ScoreSearch;

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
    /// 状態
    /// </summary>
    public States State { get; protected set; } = States.None;

    /// <summary>
    /// 前回状態
    /// </summary>
    public States StatePre { get; protected set; } = States.None;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ScreenSongList()
    {
    }

    #region Key & Mouse Event

    public virtual void KeyDown( object aSender, KeyRoutedEventArgs aArgs )
    {
    }

    public virtual void KeyUp( object aSender, KeyRoutedEventArgs aArgs )
    {
    }

    public virtual void MouseDown( object aSender, PointerRoutedEventArgs aArgs )
    {
    }

    public virtual void MouseMove( object aSender, PointerRoutedEventArgs aArgs )
    {
    }

    public virtual void MouseUp( object aSender, PointerRoutedEventArgs aArgs )
    {
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
                            // スコア検索
                            SearchScoreFilesAsync();

                            var format = new FormatRect()
                            {
                                Background = new( HelperColor.GetColor("#AA666666" ) ),
                            };

                            // 処理中アイテム
                            _Processing ??= new
                            (
                                ScreenSize._width  / 2F,
                                ScreenSize._height / 2F, 
                                30, 
                                format
                            );

                            // 
                            _SongScrollList ??= new();

                            State = States.ScoreSearching;
                        }
                        break;
                    case Requests.SongListInit:
                        {
                            // SongList検索
                            SearchSongList();

                            State = States.SongListInitializing;

                            Request = Requests.SongListSelectMode;
                        }
                        break;
                    case Requests.SongListSelectMode:
                        {
                            State = States.SongListSelectMode;
                        }
                        break;
                }
            }
        }
        #endregion

        #region Itemフレーム処理
        {
            _Processing?.Move( aFrameTime );
            _SongScrollList?.Move( aFrameTime );
        }
        #endregion

        #region 状態に応じた処理
        {
            switch ( State )
            {
                case States.ScoreSearching:
                    {
                    }
                    break;
                case States.SongListInitializing:
                    {
                    }
                    break;
                case States.SongListSelectMode:
                    {
                    }
                    break;
            }
        }
        #endregion

        return true;
    }

    /// <summary>
    /// スコア検索
    /// </summary>
    private async void SearchScoreFilesAsync()
    {
        try
        {
            await Task.Run
            ( 
                () =>
                {
                    var list = new SourceList();

                    // ファイル検索
                    Config.File.FolderDms.ForEach
                    ( 
                        baseFolderPath => 
                        { 
                            list.SearchSource( baseFolderPath, "*.dms" );

                            // DB登録
                            DBIO.MeargeSongList( list.Sources );
                        }
                    );

                    Request = Requests.SongListInit;
                }
            );
        }
        catch ( Exception e )
        {
            Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );

            Request = Requests.SongListInit;
        }
    }

    /// <summary>
    /// SongList取得
    /// </summary>
    private void SearchSongList()
    {
        try
        {
            var songlist = DBIO.SelectSongList();

            //songlist.ForEach( song => Log.Info( $"{song.RelativeFilePath}" ) );
        }
        catch ( Exception e )
        {
            Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    #region 描画処理

    public virtual bool OnDraw( CanvasDrawEventArgs aArgs )
    {
        // SwapChainの描画セッション作成時に背景色指定済み
        //aArgs.DrawingSession.Clear( DrawSetCom.SheetColor.Color );

        #region 状態に応じた処理
        {
            switch ( State )
            {
                case States.Loading:
                case States.ScoreSearching:
                case States.SongListInitializing:
                    {
                        _Processing?.Draw( aArgs.DrawingSession );
                    }
                    break;
                case States.SongListSelectMode:
                    {
                        _SongScrollList?.Draw( aArgs.DrawingSession );
                    }
                    break;
            }
        }
        #endregion

        return true;
    }

    #endregion

    #region その他

    public virtual void SetParentScreen( IScreen aScreen )
    {
        if ( aScreen is ScreenSongSelect screen )
        {
            _ParentScreen = screen;
        }
    }

    #endregion
}
