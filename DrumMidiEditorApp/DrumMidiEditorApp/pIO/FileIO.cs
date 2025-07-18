﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pView;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pIO.pJson;
using DrumMidiLibrary.pIO.pPdf;
using DrumMidiLibrary.pIO.pScore;
using DrumMidiLibrary.pIO.pVideo;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;

namespace DrumMidiEditorApp.pIO;

/// <summary>
/// ファイル入出力
/// </summary>
public static class FileIO
{
    #region Config I/O

    /// <summary>
    /// 設定ファイル読込
    /// </summary>
    /// <returns>Trueのみ</returns>
    public static bool LoadConfig()
    {
        using var _ = new LogBlock( "Load Configs" );

        var path = new GeneralPath( Config.File.FolderConfig );

        // ConfigSystem
        path.FileName    = ConfigFile.FileNameConfigSystem;
        Config.System    = LoadConfig<ConfigSystem>( path ) ?? Config.System;

        // ConfigMedia
        path.FileName    = ConfigFile.FileNameConfigMedia;
        Config.Media     = LoadConfig<ConfigMedia>( path ) ?? Config.Media;

        // ConfigLog
        path.FileName    = ConfigFile.FileNameConfigLog;
        Config.Log       = LoadConfig<ConfigLog>( path ) ?? Config.Log;

        // ConfigEditer
        path.FileName    = ConfigFile.FileNameConfigEditer;
        Config.Editer    = LoadConfig<ConfigEditer>( path ) ?? Config.Editer;

        // ConfigEqualizer
        path.FileName    = ConfigFile.FileNameConfigEqualizer;
        Config.Equalizer = LoadConfig<ConfigEqualizer>( path ) ?? Config.Equalizer;

        // ConfigFile
        path.FileName    = ConfigFile.FileNameConfigFile;
        Config.File      = LoadConfig<ConfigFile>( path ) ?? Config.File;

        // ConfigMachine
        path.FileName    = ConfigFile.FileNameConfigMachine;
        Config.Machine   = LoadConfig<ConfigMachine>( path ) ?? Config.Machine;

        // ConfigPlayer
        path.FileName    = ConfigFile.FileNameConfigPlayer;
        Config.Player    = LoadConfig<ConfigPlayer>( path ) ?? Config.Player;

        // ConfigScale
        path.FileName    = ConfigFile.FileNameConfigScale;
        Config.Scale     = LoadConfig<ConfigScale>( path ) ?? Config.Scale;

        // ConfigScore
        path.FileName    = ConfigFile.FileNameConfigScore;
        Config.Score     = LoadConfig<ConfigScore>( path ) ?? Config.Score;

        // ConfigWindow
        path.FileName    = ConfigFile.FileNameConfigWindow;
        Config.Window    = LoadConfig<ConfigWindow>( path ) ?? Config.Window;

        return true;
    }

    /// <summary>
    /// コンフィグファイル読込
    /// </summary>
    /// <typeparam name="T">Configクラス</typeparam>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <returns>読込済みConfigオブジェクト</returns>
    private static T? LoadConfig<T>( GeneralPath aGeneralPath ) where T : IConfig
    {
        try
        {
            var config = JsonIO.LoadFile<T>( aGeneralPath );
            config?.CheckValidation();

            Log.Info( $"Succeeded in reading [{aGeneralPath.AbsoluteFilePath}]" );

            return config;
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to read [{aGeneralPath.AbsoluteFilePath}]" );
            Log.Error( e );
            return default;
        }
    }

    /// <summary>
    /// 設定ファイル保存
    /// </summary>
    /// <returns>Trueのみ</returns>
    public static bool SaveConfig()
    {
        using var _ = new LogBlock( "Save Configs" );

        // サブフォルダ作成
        DocumentFolderStructure();

        var path = new GeneralPath( Config.File.FolderConfig );

        // ConfigSystem
        path.FileName = ConfigFile.FileNameConfigSystem;
        SaveConfig( path, Config.File );

        // ConfigMedia
        path.FileName = ConfigFile.FileNameConfigMedia;
        SaveConfig( path, Config.Media );

        // ConfigLog
        path.FileName = ConfigFile.FileNameConfigLog;
        SaveConfig( path, Config.Log );

        // ConfigEditer
        path.FileName = ConfigFile.FileNameConfigEditer;
        SaveConfig( path, Config.Editer );

        // ConfigEqualizer
        path.FileName = ConfigFile.FileNameConfigEqualizer;
        SaveConfig( path, Config.Equalizer );

        // ConfigFile
        path.FileName = ConfigFile.FileNameConfigFile;
        SaveConfig( path, Config.File );

        // ConfigMachine
        path.FileName = ConfigFile.FileNameConfigMachine;
        SaveConfig( path, Config.Machine );

        // ConfigPlayer
        path.FileName = ConfigFile.FileNameConfigPlayer;
        SaveConfig( path, Config.Player );

        // ConfigScale
        path.FileName = ConfigFile.FileNameConfigScale;
        SaveConfig( path, Config.Scale );

        // ConfigScore
        path.FileName = ConfigFile.FileNameConfigScore;
        SaveConfig( path, Config.Score );

        // ConfigWindow
        path.FileName = ConfigFile.FileNameConfigWindow;
        SaveConfig( path, Config.Window );

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

            Log.Info( $"Succeeded in writing [{aGeneralPath.AbsoluteFilePath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aGeneralPath.AbsoluteFilePath}]" );
            Log.Error( e );
        }
    }

    #endregion

    /// <summary>
    /// スコア - Video保存
    /// </summary>
    /// <param name="aFilePath">出力先ファイルパス</param>
    /// <param name="aCancellationToken">キャンセルトークン</param>
    /// <returns></returns>
    public static async Task SaveVideoAsync( GeneralPath aFilePath, CancellationToken aCancellationToken )
    {
        using var _a = new LogBlock( "Save Video" );

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

            var log_cnt = 0;

            await Task.Run
                ( 
                    () =>
                    {
                        for ( var time = 0D; time <= DmsControl.EndPlayTime; time += frameTime )
                        {
                            // キャンセルチェック
                            aCancellationToken.ThrowIfCancellationRequested();

                            if ( log_cnt++ % fps == 0 )
                            {
                                ControlAccess.PageStatusBar?.ReloadProgressBar( time * 100 / DmsControl.EndPlayTime );
                                Log.Info( $"{(int)time}/{(int)DmsControl.EndPlayTime}({Math.Round( time * 100 / DmsControl.EndPlayTime, 2 )}%)", true );
                            }

                            #region Frame描画

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

                            _ = mp4.AddFrame();

                            #endregion
                        }

                        Log.Info( $"Succeeded in writing [{aFilePath.AbsoluteFilePath}]", true );
                    },
                    aCancellationToken
                );
        }
        catch ( OperationCanceledException )
        {
            Log.Info( $"Cancel in writing [{aFilePath.AbsoluteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoluteFilePath}]", true );
            Log.Error( e );
        }
        finally
        {
            ControlAccess.PageStatusBar?.ReloadProgressBar( 0 );
            ControlAccess.UCPlayerPanel?.GetFrameEnd();
        }
    }

    /// <summary>
    /// スコア - Pdf保存
    /// </summary>
    /// <param name="aFilePath">出力先ファイルパス</param>
    /// <returns></returns>
    public static void SavePdf( GeneralPath aFilePath )
    {
        using var _a = new LogBlock( "Save Pdf" );

        try
        {
            ControlAccess.UCPlayerPanel?.GetFrameStart();

            using var frameSize = ControlAccess.UCPlayerPanel?.GetFrame( 0 );

            if ( frameSize == null )
            {
                Log.Error( $"get frame failure.", true );
                return;
            }

            // NOTEが存在する最大小節番号
            var measureMaxNo = DMS.SCORE.GetMaxMeasureNo();

            // 1ページ当たりの小節描画数
            var measurePerPage = ControlAccess.UCPlayerPanel?.GetNumberOfMeasureNoPerPage() ?? 0 ;
            if ( measurePerPage == 0 )
            {
                Log.Error( $"no measures to draw.", true );
                return;
            }

            // 最終ページ
            var pageMax = measureMaxNo / measurePerPage + 1;

            using var pdf = new PdfIO();

            var bmp = pdf.Open
                (
                    aFilePath,
                    pageMax,
                    (int)frameSize.Size.Width,
                    (int)frameSize.Size.Height
                );

            if ( bmp == null )
            {
                Log.Error( $"open video file failure.", true );
                return;
            }

            // NOTE: page=0から処理すると小節番号=000が2回処理されてしまう。
            // NOTE: 描画の初期化が上手くいっていない？

            for ( var page = 1; page <= pageMax; page++ )
            {
                var frametime = DmsControl.GetMeasureStartTime( page * measurePerPage );

                #region Frame描画

                using var frame = ControlAccess.UCPlayerPanel?.GetFrame( frametime );

                if ( frame == null )
                {
                    Log.Error( $"frame read error.[{page}]" );
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

                _ = pdf.AddFrame();

                #endregion
            }

            Log.Info( $"Succeeded in writing [{aFilePath.AbsoluteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoluteFilePath}]", true );
            Log.Error( e );
        }
        finally
        {
            ControlAccess.UCPlayerPanel?.GetFrameEnd();
        }
    }

    #region Create sub folder

    /// <summary>
    /// ドキュメントフォルダを構築
    /// </summary>
    private static void DocumentFolderStructure()
    {
        var folderList = new List<GeneralPath>()
        {
            Config.File.FolderConfig,
            Config.File.FolderModel,
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
            targetFolderList.ForEach( CreateFolder );
        }
        catch ( Exception e )
        {
            Log.Error( e );
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

            _ = aFolderPath.CreateDirectory();

            Log.Info( $"Folder creation successful [{aFolderPath.AbsoluteFolderPath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Folder creation failure [{aFolderPath.AbsoluteFolderPath}]" );
            Log.Error( e );
        }
    }

    #endregion

    /// <summary>
    /// スコア読込
    /// </summary>
    /// <param name="aFilePath">スコアファイルパス</param>
    /// <param name="aScore">出力スコア</param>
    /// <returns>True:読込成功、False:読込失敗</returns>
    public static bool LoadScore( GeneralPath aFilePath, out Score aScore )
    {
        using var _ = new LogBlock( "Load Score" );

        aScore = new();

        try
        {
            var ext = aFilePath.Extension.ToLower();

            if ( ext.Equals( ConfigFile.ExtentionDms ) )
            {
                ScoreIO.LoadFile( aFilePath, out aScore );
            }
            else if ( ext.Equals( ConfigFile.ExtentionMidi ) )
            {
                ScoreIO.LoadMidiFile( aFilePath, out aScore );
            }
            else if ( ext.Equals( ConfigFile.ExtentionDtx ) )
            {
                ScoreIO.LoadDtxManiaFile( aFilePath, out aScore );
            }
            else
            {
                throw new NotSupportedException( $"Extension {ext} is not supported" );
            }

            Log.Info( $"Succeeded in reading [{aFilePath.AbsoluteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to read [{aFilePath.AbsoluteFilePath}]", true );
            Log.Error( e );
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
        using var _ = new LogBlock( "Save Score" );

        try
        {
            ScoreIO.SaveFile( aFilePath, aScore );

            Log.Info( $"Succeeded in writing [{aFilePath.AbsoluteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoluteFilePath}]", true );
            Log.Error( e );
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
        using var _ = new LogBlock( "Save Midi" );

        try
        {
            ScoreIO.SaveMidiFile( aFilePath, aScore );

            Log.Info( $"Succeeded in writing [{aFilePath.AbsoluteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoluteFilePath}]", true );
            Log.Error( e );
            return false;
        }
        return true;
    }

    /// <summary>
    /// スコア - Dtx保存
    /// </summary>
    /// <param name="aFilePath">出力先ファイルパス</param>
    /// <param name="aScore">保存スコア</param>
    /// <returns>True:保存成功、False:保存失敗</returns>
    public static bool SaveDtx( GeneralPath aFilePath, Score aScore )
    {
        using var _ = new LogBlock( "Save Dtx" );

        try
        {
            ScoreIO.SaveDtxFile( aFilePath, aScore );

            Log.Info( $"Succeeded in writing [{aFilePath.AbsoluteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoluteFilePath}]", true );
            Log.Error( e );
            return false;
        }
        return true;
    }


    /// <summary>
    /// MidiMapセットテンプレートの読込
    /// </summary>
    /// <param name="aFilePath">テンプレートファイルパス</param>
    /// <param name="aMidiMapSet">出力MidiMapセット</param>
    /// <returns>True:読込成功、False:読込失敗</returns>
    public static bool LoadMidiMapSet( GeneralPath aFilePath, out MidiMapSet aMidiMapSet )
    {
        using var _ = new LogBlock( "Load MidiMapSet" );

        try
        {
            ScoreIO.LoadFile( aFilePath, out aMidiMapSet );

            Log.Info( $"Succeeded in reading [{aFilePath.AbsoluteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to read [{aFilePath.AbsoluteFilePath}]", true );
            Log.Error( e );

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
        using var _ = new LogBlock( "Save MidiMapSet" );

        try
        {
            ScoreIO.SaveFile( aFilePath, aMidiMapSet );

            Log.Info( $"Succeeded in writing [{aFilePath.AbsoluteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to write [{aFilePath.AbsoluteFilePath}]", true );
            Log.Error( e );
            return false;
        }
        return true;
    }

    /// <summary>
    /// スコア情報のインポート
    /// </summary>
    /// <param name="aFilePath">スコアファイルパス</param>
    /// <returns>True:インポート成功、False:インポート失敗</returns>
    public static bool ImportScore( GeneralPath aFilePath )
    {
        // TASK: 放置中

        using var _ = new LogBlock( "Import Midi" );

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

            //Log.Info( $"Succeeded in reading [{aFilePath.AbsoulteFilePath}]", true );
        }
        catch ( Exception e )
        {
            Log.Error( $"Failed to read [{aFilePath.AbsoluteFilePath}]", true );
            Log.Error( e );
            return false;
        }
        //return true;
    }
}
