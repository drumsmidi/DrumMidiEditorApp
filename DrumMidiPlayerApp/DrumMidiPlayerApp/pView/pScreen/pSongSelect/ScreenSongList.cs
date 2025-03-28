using System;
using System.Threading.Tasks;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pIO.pDatabase;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace DrumMidiPlayerApp.pView.pScreen.pSongSelect;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class ScreenSongList : ScreenBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ScreenSongList() : base( true )
    {
    }


    #region Screen情報

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
        ScoreSearch,
        SongListInit,
        SongListSelectMode,
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
        ScoreSearching,
        SongListInitializing,
        SongListSelectMode,
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

        // 前回状態
        StatePre = State;

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
                    State = States.SongListSelectMode;
                }
                break;
        }
    }

    #endregion

    #region Input Event

    /// <summary>
    /// 入力イベント処理
    /// </summary>
    private void ProcInputEvent()
    {
        switch ( State )
        {
            case States.SongListSelectMode:
                {
                    //_SongScrollList?.NextSongList();

                    //SystemSound.SoundPlayMoveNext();
                }
                break;
        }
    }

    #endregion

    #region Frame処理

    protected override void OnLoadSelf()
    {
        // スクリーンサイズ設定
        //ScreenDrawRect.Width  = Config.Panel.BaseScreenSize.Width  / 2D;
        //ScreenDrawRect.Height = Config.Panel.BaseScreenSize.Height / 2D;

        // アイテム：曲スクロールリスト作成
        _SongScrollList ??= new();

        // スコア検索
        SearchScoreFilesAsync();
    }

    protected override bool OnLoadedSelf()
    {
        return State == States.SongListSelectMode;
    }

    protected override void OnUnLoadSelf()
    {
        _SongScrollList?.Dispose();
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
                    _SongScrollList?.Move( aFrameTime );
                }
                break;
        }

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

    #region 描画処理

    protected override bool OnDrawSelf( CanvasDrawEventArgs aArgs )
    {
        switch ( State )
        {
            case States.SongListSelectMode:
                {
                    _SongScrollList?.Draw( aArgs.DrawingSession );
                }
                break;
        }
        return true;
    }

    #endregion

}
