using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pIO.pJson;
using DrumMidiEditorApp.pIO.pScore;
using DrumMidiEditorApp.pView;

namespace DrumMidiEditorApp.pIO;

/// <summary>
/// ファイル入出力
/// </summary>
public static class FileIO
{
    #region Create sub folder

    /// <summary>
    /// ドキュメントフォルダを構築
    /// </summary>
    private static void DocumentFolderStructure()
    {
        var folderList = new List<GeneralPath>()
        {
            Config.System.FolderConfig,
            Config.System.FolderModel,
        };

        var targetFolderList = new List<GeneralPath>();

        try
        {
            // フォルダ存在チェック
            foreach ( var folder in folderList )
            {
                if ( !folder.IsExistDirectory )
                {
                    targetFolderList.Add( folder );
                }
            }

            if ( targetFolderList.Count == 0 )
            {
                return;
            }

            // フォルダ作成
            targetFolderList.ForEach( folder => CreateFolder( folder ) );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// フォルダ作成
    /// </summary>
    /// <param name="aFolderPath"></param>
    private static void CreateFolder( GeneralPath aFolderPath )
    {
        try
        {
            if ( aFolderPath.Length == 0 || aFolderPath.IsExistDirectory )
            {
                return;
            }

            aFolderPath.CreateDirectory();

            Log.Info( $"Folder creation successful [{aFolderPath.AbsoulteFolderPath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Folder creation failure [{aFolderPath.AbsoulteFolderPath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    #region Config I/O

    /// <summary>
    /// 設定ファイル読込
    /// </summary>
    /// <returns>Trueのみ</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:オブジェクトの初期化を簡略化します", Justification = "<保留中>")]
    public static bool LoadConfig()
    {
		using var _ = new LogBlock( Log.GetThisMethodName );

        var path = new GeneralPath( Config.System.FolderConfig );

        // ConfigSystem
        path.FileName = Config.System.FileNameConfigSystem;
        Config.System = LoadConfig<ConfigSystem>( path ) ?? Config.System ;

        // ConfigMedia
        path.FileName = Config.System.FileNameConfigMedia;
        Config.Media = LoadConfig<ConfigMedia>( path ) ?? Config.Media ;

        // ConfigEditer
        path.FileName = Config.System.FileNameConfigEditer;
        Config.Editer = LoadConfig<ConfigEditer>( path ) ?? Config.Editer ;

        // ConfigPlayer
        path.FileName = Config.System.FileNameConfigPlayer;
        Config.Player = LoadConfig<ConfigPlayer>( path ) ?? Config.Player ;

        // ConfigScore
        path.FileName = Config.System.FileNameConfigScore;
        Config.Score = LoadConfig<ConfigScore>( path ) ?? Config.Score ;

        // ConfigEqualizer
        path.FileName = Config.System.FileNameConfigEqualizer;
        Config.Equalizer = LoadConfig<ConfigEqualizer>( path ) ?? Config.Equalizer ;

        // ConfigScale
        path.FileName = Config.System.FileNameConfigScale;
        Config.Scale = LoadConfig<ConfigScale>( path ) ?? Config.Scale;

        return true;
    }

    /// <summary>
    /// コンフィグファイル読込
    /// </summary>
    /// <typeparam name="T">Configクラス</typeparam>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <returns>読込済みConfigオブジェクト</returns>
    private static T? LoadConfig<T>( GeneralPath aGeneralPath ) where T : class
    {
        T? config = null;

        try
        {
            config = JsonIO.LoadFile<T>( aGeneralPath );

            Log.Info( $"Succeeded in reading [{aGeneralPath.AbsoulteFilePath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to read [{aGeneralPath.AbsoulteFilePath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
        return config;
    }


    /// <summary>
    /// 設定ファイル保存
    /// </summary>
    /// <returns>Trueのみ</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:オブジェクトの初期化を簡略化します", Justification = "<保留中>")]
    public static bool SaveConfig()
    {
		using var _ = new LogBlock( Log.GetThisMethodName );

        // サブフォルダ作成
        FileIO.DocumentFolderStructure();

        var path = new GeneralPath( Config.System.FolderConfig );

        // ConfigSystem
        path.FileName = Config.System.FileNameConfigSystem;
        SaveConfig<ConfigSystem>( path, Config.System );

        // ConfigMedia
        path.FileName = Config.System.FileNameConfigMedia;
        SaveConfig<ConfigMedia>( path, Config.Media );

        // ConfigEditer
        path.FileName = Config.System.FileNameConfigEditer;
        SaveConfig<ConfigEditer>( path, Config.Editer );

        // ConfigPlayer
        path.FileName = Config.System.FileNameConfigPlayer;
        SaveConfig<ConfigPlayer>( path, Config.Player );

        // ConfigScore
        path.FileName = Config.System.FileNameConfigScore;
        SaveConfig<ConfigScore>( path, Config.Score );

        // ConfigEqualizer
        path.FileName = Config.System.FileNameConfigEqualizer;
        SaveConfig<ConfigEqualizer>( path, Config.Equalizer );

        // ConfigScale
        path.FileName = Config.System.FileNameConfigScale;
        SaveConfig<ConfigScale>( path, Config.Scale );

        return true;
    }

    /// <summary>
    /// コンフィグファイル出力
    /// </summary>
    /// <typeparam name="T">Configクラス</typeparam>
    /// <param name="aGeneralPath">出力ファイルパス</param>
    /// <param name="aGeneralPath">出力Configオブジェクト</param>
    private static void SaveConfig<T>( GeneralPath aGeneralPath, T aConfig ) where T : class
    {
        try
        {
            JsonIO.SaveFile<T>( aConfig, aGeneralPath );

            Log.Info( $"Succeeded in writing [{aGeneralPath.AbsoulteFilePath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aGeneralPath.AbsoulteFilePath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    /// <summary>
    /// スコア保存
    /// </summary>
    /// <param name="aFilePath">スコアファイルパス</param>
    /// <param name="aScore">出力スコア</param>
    /// <returns>True:読込成功、False:読込失敗</returns>
    public static bool LoadScore( GeneralPath aFilePath, out Score aScore )
    {
		using var _ = new LogBlock( Log.GetThisMethodName );

        aScore = new();

        try
        {
            var ext = aFilePath.Extension.ToLower();

            if ( ext.Equals( ".dms" ) )
            {
                ScoreIO.LoadFile( aFilePath, out aScore );
            }
            else if ( ext.Equals( ".mid" ) )
            {
                ScoreIO.LoadMidiFile( aFilePath, out aScore );
            }
            else if ( ext.Equals( ".dtx" ) )
            {
                // not recommended
                ScoreIO.LoadDtxManiaFile( aFilePath, out aScore );
            }
            else
            {
                throw new NotSupportedException( $"Extension {ext} is not supported" );
            }

            Log.Info( $"Succeeded in reading [{aFilePath.AbsoulteFilePath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to read [{aFilePath.AbsoulteFilePath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
            return false;
        }
        return true;
    }

    /// <summary>
    /// スコア保存
    /// </summary>
    /// <param name="aFilePath">出力先ファイルパス</param>
    /// <param name="aScore">保存スコア</param>
    /// <returns>True:保存成功、False:保存失敗</returns>
    public static bool SaveScore( GeneralPath aFilePath, Score aScore )
    {
		using var _ = new LogBlock( Log.GetThisMethodName );

        try
        {
            ScoreIO.SaveFile( aFilePath, aScore );

            Log.Info( $"Succeeded in writing [{aFilePath.AbsoulteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoulteFilePath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
            return false;
        }
        return true;
    }

    /// <summary>
    /// スコア - Midi保存
    /// </summary>
    /// <param name="aFilePath">出力先ファイルパス</param>
    /// <param name="aScore">保存スコア</param>
    /// <returns>True:保存成功、False:保存失敗</returns>
    public static bool SaveMidi( GeneralPath aFilePath, Score aScore )
    {
		using var _ = new LogBlock( Log.GetThisMethodName );

        try
        {
            ScoreIO.SaveMidiFile( aFilePath, aScore );

            Log.Info( $"Succeeded in writing [{aFilePath.AbsoulteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoulteFilePath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
            return false;
        }
        return true;
    }

    /// <summary>
    /// スコア - Video保存
    /// </summary>
    /// <param name="aFilePath">出力先ファイルパス</param>
    /// <param name="aScore">保存スコア</param>
    /// <returns>True:保存成功、False:保存失敗</returns>
    public static async void SaveVideoAsync( GeneralPath aFilePath, Score aScore )
    {
	    using var _ = new LogBlock( Log.GetThisMethodName );

        try
        {
            var mp4_codec = Config.Media.OutputVideoCodec;
            var fps       = Config.Media.OutputVideoFps;

            ControlAccess.UCPlayerPanel?.GetFrameStart();

            using var frameSize = ControlAccess.UCPlayerPanel?.GetFrame( 0 );

            if ( frameSize == null )
            {
                Log.Error( $"get frame failure.", true );
                return;
            }

            using var mp4 = new Mp4IO();

            var bmp = mp4.Open
                ( 
                    aFilePath, 
                    mp4_codec, 
                    fps, 
                    (int)frameSize.Size.Width,
                    (int)frameSize.Size.Height
                );

            if ( bmp == null )
            {
                Log.Error( $"open video file failure.", true );
                return;
            }

            var frameTime = 1d / fps;

            int log_cnt = 0;

            await Task.Run
                (
                    () => 
                    { 
                        for ( var time = 0D; time <= DmsControl.EndPlayTime; time += frameTime )
                        {
                            if ( log_cnt++ % fps == 0 )
                            { 
                                Config.System.ProgressBarValue = time * 100 / DmsControl.EndPlayTime;
                                ControlAccess.PageStatusBar?.ReloadProgressBar();
                                Log.Info( $"{(int)time}/{(int)DmsControl.EndPlayTime}({Math.Round( time*100/DmsControl.EndPlayTime, 2 )}%)", true );
                            }

                            using var frame = ControlAccess.UCPlayerPanel?.GetFrame( time );

                            if ( frame == null )
                            {
                                Log.Error( $"frame read error.[{time}]" );
                                continue;
                            }

                            var buffer = frame.GetPixelBytes();

                            var bmpData = bmp.LockBits
                            (
                                new( 0, 0, bmp.Width, bmp.Height ),
                                ImageLockMode.WriteOnly,
                                bmp.PixelFormat
                            );

                            Marshal.Copy( buffer, 0, bmpData.Scan0, buffer.Length );

                            bmp.UnlockBits( bmpData );

                            mp4.AddFrame();
                        }

                        Log.Info( $"Succeeded in writing [{aFilePath.AbsoulteFilePath}]", true );

                        Config.System.ProgressBarValue = 0;
                        ControlAccess.PageStatusBar?.ReloadProgressBar();

                        ControlAccess.UCPlayerPanel?.GetFrameEnd();
                    }
                );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoulteFilePath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            ControlAccess.UCPlayerPanel?.GetFrameEnd();
        }
    }

    /// <summary>
    /// MidiMapセットテンプレートの読込
    /// </summary>
    /// <param name="aFilePath">テンプレートファイルパス</param>
    /// <param name="aMidiMapSet">出力MidiMapセット</param>
    /// <returns>True:読込成功、False:読込失敗</returns>
    public static bool LoadMidiMapSet( GeneralPath aFilePath, out MidiMapSet aMidiMapSet )
    {
		using var _ = new LogBlock( Log.GetThisMethodName );

        try
        {
            ScoreIO.LoadFile( aFilePath, out aMidiMapSet );

            Log.Info( $"Succeeded in reading [{aFilePath.AbsoulteFilePath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to read [{aFilePath.AbsoulteFilePath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            aMidiMapSet = new();
            return false;
        }
        return true;
    }

    /// <summary>
    /// MidiMapセットテンプレートの保存
    /// </summary>
    /// <param name="aFilePath">テンプレートファイルパス</param>
    /// <param name="aMidiMapSet">保存MidiMapセット</param>
    /// <returns>True:保存成功、False:保存失敗</returns>
    public static bool SaveMidiMapSet( GeneralPath aFilePath, MidiMapSet aMidiMapSet )
    {
		using var _ = new LogBlock( Log.GetThisMethodName );

        try
        {
            ScoreIO.SaveFile( aFilePath, aMidiMapSet );

            Log.Info( $"Succeeded in writing [{aFilePath.AbsoulteFilePath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoulteFilePath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
            return false;
        }
        return true;
    }

    /// <summary>
    /// スコア情報のインポート
    /// </summary>
    /// <param name="aFilePath">スコアファイルパス</param>
    /// <param name="aScore">出力先スコア</param>
    /// <returns>True:インポート成功、False:インポート失敗</returns>
    public static bool ImportScore( GeneralPath aFilePath, ref Score aScore )
    {
        // TODO: この辺は改良の余地あり。

		using var _ = new LogBlock( Log.GetThisMethodName );

        try
        {
            var ext = aFilePath.Extension.ToLower();

            //if ( ext.Equals( ".mid" ) )
            //{
            //    ScoreIO.LoadMidiFile( aFilePath, out aScore );
            //}
            //else
            {
                throw new NotSupportedException( $"Extension {ext} is not supported" );
            }

            //Log.Info( $"Succeeded in reading [{aFilePath.AbsoulteFilePath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to read [{aFilePath.AbsoulteFilePath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
            return false;
        }
        //return true;
    }
}
