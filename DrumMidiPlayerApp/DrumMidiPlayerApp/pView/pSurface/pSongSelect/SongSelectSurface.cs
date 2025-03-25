using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pIO.pDatabase;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pModel;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pSurface.pSongSelect;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class SongSelectSurface : ISurface
{
    #region Member

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


    /// <summary>
    /// リクエスト一覧
    /// </summary>
    private enum SongSelectRequest : int
    {
        None = 0,
        ScoreSearch,
        SongListInit,
        SongListSelectMode,
    }

    /// <summary>
    /// 再生状態
    /// </summary>
    private SongSelectRequest _SongSelectRequest= SongSelectRequest.ScoreSearch;

    /// <summary>
    /// 状態一覧
    /// </summary>
    private enum SongSelectState : int
    {
        None = 0,
        ScoreSearching,
        SongListInitializing,
        SongListSelectMode,
    }

    /// <summary>
    /// 再生状態
    /// </summary>
    private SongSelectState _SongSelectState = SongSelectState.None;

    /// <summary>
    /// 再生要求内容（通常再生、ループ再生の判定に使用）
    /// </summary>
    private SongSelectState _SongSelectStatePre = SongSelectState.None;

    /// <summary>
    /// 停止中にプレイヤーへ描画する画像
    /// </summary>
    private IAsyncOperation<CanvasBitmap>? _StopImage;


    private ItemProcessing? _Processing;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SongSelectSurface()
    {
    }

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

    public virtual bool OnMove( double aFrameTime )
    {
        #region リクエスト処理
        {
            var req = _SongSelectRequest;

            if (  req != SongSelectRequest.None )
            {
                _SongSelectRequest  = SongSelectRequest.None;
                _SongSelectStatePre = _SongSelectState;

                switch ( req )
                {
                    case SongSelectRequest.ScoreSearch:
                        {
                            SearchScoreFiles();

                            FormatRect format = new()
                            {
                                Background = new( HelperColor.GetColor("#AA666666" ) ),
                            };

                            

                            _Processing ??= new ItemProcessing
                            (
                                Config.Panel.BaseScreenSize._width  / 2F,
                                Config.Panel.BaseScreenSize._height / 2F, 
                                30, 
                                format 
                            );

                            _SongSelectState = SongSelectState.ScoreSearching;
                        }
                        break;
                    case SongSelectRequest.SongListInit:
                        {
                            SearchSongList();

                            _SongSelectState = SongSelectState.SongListInitializing;
                        }
                        break;
                    case SongSelectRequest.SongListSelectMode:
                        {
                            _SongSelectState = SongSelectState.SongListSelectMode;
                        }
                        break;
                }
            }
        }
        #endregion

        #region 状態に応じた処理
        {
            switch ( _SongSelectState )
            {
                case SongSelectState.ScoreSearching:
                    {
                        _Processing?.Move( aFrameTime );

                    }
                    break;
                case SongSelectState.SongListInitializing:
                    {
                        _Processing?.Move( aFrameTime );
                    }
                    break;
                case SongSelectState.SongListSelectMode:
                    {
                        _Processing?.Move( aFrameTime );
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
    private async void SearchScoreFiles()
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

                    _SongSelectRequest = SongSelectRequest.SongListInit;
                }
            );
        }
        catch ( Exception e )
        {
            Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );

            _SongSelectRequest = SongSelectRequest.SongListInit;
        }
    }

    /// <summary>
    /// SongList取得
    /// </summary>
    private async void SearchSongList()
    {
        try
        {
            await Task.Run
            (
                () =>
                {
                    var songlist = DBIO.SelectSongList();

                    //songlist.ForEach( song => Log.Info( $"{song.RelativeFilePath}" ) );

                    _SongSelectRequest = SongSelectRequest.SongListSelectMode;
                }
            );
        }
        catch ( Exception e )
        {
            Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );

            _SongSelectRequest = SongSelectRequest.SongListSelectMode;
        }
    }



    public virtual bool OnDraw( CanvasDrawEventArgs aArgs )
    {
        // SwapChainの描画セッション作成時に背景色指定済み
        //aArgs.DrawingSession.Clear( DrawSetCom.SheetColor.Color );

        #region 状態に応じた処理
        {
            switch ( _SongSelectState )
            {
                case SongSelectState.ScoreSearching:
                case SongSelectState.SongListInitializing:
                    {
                        _Processing?.Draw( aArgs.DrawingSession );
                    }
                    break;
                case SongSelectState.SongListSelectMode:
                    {
                        // Effect機能確認用に停止中に画像を表示
                        _StopImage ??= CanvasBitmap.LoadAsync( aArgs.DrawingSession, new Uri( "ms-appx:///Images/stop.jpg" ) );
                        if ( _StopImage.Status == AsyncStatus.Completed )
                        {
                            aArgs.DrawingSession.DrawImage( _StopImage.GetResults() );
                        }

                        _Processing?.Draw( aArgs.DrawingSession );
                    }
                    break;
            }
        }
        #endregion

        return true;
    }
}
