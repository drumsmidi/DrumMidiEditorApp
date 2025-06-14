using System;
using System.Threading.Tasks;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pIO.pDatabase;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pIO;
using DrumMidiPlayerApp.pModel;
using DrumMidiPlayerApp.pView.pScreen.pOption;
using DrumMidiPlayerApp.pView.pScreen.pPlayer;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.System;

namespace DrumMidiPlayerApp.pView.pScreen.pSongList;

/// <summary>
/// スクリーン：曲選択
/// </summary>
public class ScreenSongList() : ScreenBase( new(){ Processing = true } )
{
    #region Screen情報

    /// <summary>
    /// スクリーン：曲再生
    /// </summary>
    private ScreenPlayer? _ScreenPlayer;

    /// <summary>
    /// スクリーン：曲再生
    /// </summary>
    private ScreenPlayerMobile? _ScreenPlayerMobile;

    /// <summary>
    /// スクリーン：オプション
    /// </summary>
    private ScreenOptionMain? _ScreenOptionMain;

    /// <summary>
    /// 曲リストスクロールリスト
    /// </summary>
    private ItemSongScrollList? _SongScrollList;

    #endregion

    #region Load/UnLoad処理

    protected override void OnLoadSelf()
    {
        // スクリーンサイズ設定
        ScreenDrawRect.Width  = Config.Panel.BaseScreenSize.Width;
        ScreenDrawRect.Height = Config.Panel.BaseScreenSize.Height;

        // アイテム：曲スクロールリスト作成
        _SongScrollList ??= new();

        // 子スクリーン作成
        AddChildScreen( _ScreenPlayer       ??= new() );
    //  AddChildScreen( _ScreenPlayerMobile ??= new() );
        AddChildScreen( _ScreenOptionMain   ??= new() );

        // 入力マップ設定
        _InputMap.KeyMap.Clear();
        _InputMap.KeyMap.Add( VirtualKey.Up     , VirtualKey.GamepadDPadUp );
        _InputMap.KeyMap.Add( VirtualKey.Down   , VirtualKey.GamepadDPadDown );
        _InputMap.KeyMap.Add( VirtualKey.Back   , VirtualKey.GamepadA );
        _InputMap.KeyMap.Add( VirtualKey.Enter  , VirtualKey.GamepadB );
        _InputMap.KeyMap.Add( VirtualKey.M      , VirtualKey.GamepadMenu );

        // スコア検索
        Request = Requests.ScoreSearch;
    }

    protected override bool OnLoadedSelf()
    {
        return State == States.SongListSelectMode;
    }

    protected override void OnUnLoadSelf()
    {
        _ScreenPlayer       = null;
        _ScreenPlayerMobile = null;
        _ScreenOptionMain   = null;

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
        ScoreSearching,
        SongListInitializing,
        SongListSelectMode,
        PlayerMode,
        OptionMode,
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
        ScoreSearch,
        SongListInit,
        SongListSelectMode,
        PlayerMode,
        OptionMode,
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
            case Requests.ScoreSearch:
                {
                    // スコア検索
                    SearchScoreFilesAsync();

                    State = States.ScoreSearching;
                }
                break;
            case Requests.SongListInit:
                {
                    // SongList検索
                    SearchSongList();

                    State = States.SongListInitializing;
                }
                break;
            case Requests.SongListSelectMode:
                {
                    OnActivate( true );

                    State = States.SongListSelectMode;
                }
                break;
            case Requests.PlayerMode:
                {
                    OnActivate( false );

                    _ScreenPlayer?.OnActivate( true );

                    _ScreenPlayerMobile?.OnActivate( true );

                    State = States.PlayerMode;
                }
                break;
            case Requests.OptionMode:
                {
                    OnActivate( false );

                    _ScreenOptionMain?.OnActivate( true );

                    State = States.OptionMode;
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
            case States.PlayerMode:
            case States.OptionMode:
                {
                    if ( aActivate )
                    {
                        // 曲スクロールリスト選択状態
                        Request = Requests.SongListSelectMode;
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
            case States.SongListSelectMode:
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

                                    // 曲選択
                                    if ( item != null && item.FilePath != null )
                                    {
                                        if ( FileIO.LoadScore( item.FilePath, out var score ) )
                                        {
                                            // NOTE:見直しが必要
                                            score.BgmFilePath = new( score.BgmFilePath.RelativeFilePath, score.FilePath.AbsoluteFolderPath );
                                            score.BgmFilePath.PrintPath();

                                            DMS.SCORE = score;

                                            Request = Requests.PlayerMode;
                                            return;
                                        }
                                    }
                                }
                                break;
                            case VirtualKey.GamepadMenu:
                                {
                                    // オプション選択
                                    Request = Requests.OptionMode;
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
            case States.SongListSelectMode:
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
            case States.SongListSelectMode:
            case States.OptionMode:
                {
                    _SongScrollList?.Draw( aArgs.DrawingSession );
                }
                break;
        }
        return true;
    }

    #endregion

    #region 個別処理

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
            songlist.Sort();

            _SongScrollList?.SetSongList( songlist );

            Request = Requests.SongListSelectMode;
        }
        catch ( Exception e )
        {
            Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );

            Request = Requests.SongListSelectMode;
        }
    }

    #endregion
}
