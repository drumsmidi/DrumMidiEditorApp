using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DrumMidiClassLibrary.pConfig;
using DrumMidiClassLibrary.pControl;
using DrumMidiClassLibrary.pIO.pVideo;
using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pModel;
using DrumMidiClassLibrary.pUtil;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pView;

namespace DrumMidiEditerApp.pIO;

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
        ConfigLocal.Editer = LoadConfig<ConfigEditer>( path ) ?? ConfigLocal.Editer ;

        // ConfigPlayer
        path.FileName = Config.System.FileNameConfigPlayer;
        ConfigLocal.Player = LoadConfig<ConfigPlayer>( path ) ?? ConfigLocal.Player ;

        // ConfigScore
        path.FileName = Config.System.FileNameConfigScore;
        ConfigLocal.Score = LoadConfig<ConfigScore>( path ) ?? ConfigLocal.Score ;

        // ConfigEqualizer
        path.FileName = Config.System.FileNameConfigEqualizer;
        ConfigLocal.Equalizer = LoadConfig<ConfigEqualizer>( path ) ?? ConfigLocal.Equalizer ;

        // ConfigScale
        path.FileName = Config.System.FileNameConfigScale;
        ConfigLocal.Scale = LoadConfig<ConfigScale>( path ) ?? ConfigLocal.Scale;

        return true;
    }

    /// <summary>
    /// コンフィグファイル読込
    /// </summary>
    /// <typeparam name="T">Configクラス</typeparam>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <returns>読込済みConfigオブジェクト</returns>
    private static T? LoadConfig<T>( GeneralPath aGeneralPath ) where T : class
        => DrumMidiClassLibrary.pIO.FileIO.LoadConfig<T>( aGeneralPath );

    /// <summary>
    /// 設定ファイル保存
    /// </summary>
    /// <returns>Trueのみ</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0017:オブジェクトの初期化を簡略化します", Justification = "<保留中>")]
    public static bool SaveConfig()
    {
		using var _ = new LogBlock( Log.GetThisMethodName );

        // サブフォルダ作成
        DrumMidiClassLibrary.pIO.FileIO.DocumentFolderStructure();

        var path = new GeneralPath( Config.System.FolderConfig );

        // ConfigSystem
        path.FileName = Config.System.FileNameConfigSystem;
        SaveConfig<ConfigSystem>( path, Config.System );

        // ConfigMedia
        path.FileName = Config.System.FileNameConfigMedia;
        SaveConfig<ConfigMedia>( path, Config.Media );

        // ConfigEditer
        path.FileName = Config.System.FileNameConfigEditer;
        SaveConfig<ConfigEditer>( path, ConfigLocal.Editer );

        // ConfigPlayer
        path.FileName = Config.System.FileNameConfigPlayer;
        SaveConfig<ConfigPlayer>( path, ConfigLocal.Player );

        // ConfigScore
        path.FileName = Config.System.FileNameConfigScore;
        SaveConfig<ConfigScore>( path, ConfigLocal.Score );

        // ConfigEqualizer
        path.FileName = Config.System.FileNameConfigEqualizer;
        SaveConfig<ConfigEqualizer>( path, ConfigLocal.Equalizer );

        // ConfigScale
        path.FileName = Config.System.FileNameConfigScale;
        SaveConfig<ConfigScale>( path, ConfigLocal.Scale );

        return true;
    }

    /// <summary>
    /// コンフィグファイル出力
    /// </summary>
    /// <typeparam name="T">Configクラス</typeparam>
    /// <param name="aGeneralPath">出力ファイルパス</param>
    /// <param name="aGeneralPath">出力Configオブジェクト</param>
    private static void SaveConfig<T>( GeneralPath aGeneralPath, T aConfig ) where T : class
        => DrumMidiClassLibrary.pIO.FileIO.SaveConfig<T>( aGeneralPath, aConfig );

    #endregion

    /// <summary>
    /// スコア保存
    /// </summary>
    /// <param name="aFilePath">スコアファイルパス</param>
    /// <param name="aScore">出力スコア</param>
    /// <returns>True:読込成功、False:読込失敗</returns>
    public static bool LoadScore( GeneralPath aFilePath, out Score aScore )
        => DrumMidiClassLibrary.pIO.FileIO.LoadScore( aFilePath, out aScore );

    /// <summary>
    /// スコア保存
    /// </summary>
    /// <param name="aFilePath">出力先ファイルパス</param>
    /// <param name="aScore">保存スコア</param>
    /// <returns>True:保存成功、False:保存失敗</returns>
    public static bool SaveScore( GeneralPath aFilePath, Score aScore )
        => DrumMidiClassLibrary.pIO.FileIO.SaveScore( aFilePath, aScore );

    /// <summary>
    /// スコア - Midi保存
    /// </summary>
    /// <param name="aFilePath">出力先ファイルパス</param>
    /// <param name="aScore">保存スコア</param>
    /// <returns>True:保存成功、False:保存失敗</returns>
    public static bool SaveMidi( GeneralPath aFilePath, Score aScore )
        => DrumMidiClassLibrary.pIO.FileIO.SaveMidi( aFilePath, aScore );

    /// <summary>
    /// スコア - Video保存
    /// </summary>
    /// <param name="aFilePath">出力先ファイルパス</param>
    /// <returns>True:保存成功、False:保存失敗</returns>
    public static async void SaveVideoAsync( GeneralPath aFilePath )
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

            var log_cnt = 0;

            await Task.Run
                (
                    () => 
                    { 
                        for ( var time = 0D; time <= DmsControl.EndPlayTime; time += frameTime )
                        {
                            if ( log_cnt++ % fps == 0 )
                            { 
                                ControlAccess.PageStatusBar?.ReloadProgressBar( time * 100 / DmsControl.EndPlayTime );
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

                        ControlAccess.PageStatusBar?.ReloadProgressBar( 0 );

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
        => DrumMidiClassLibrary.pIO.FileIO.LoadMidiMapSet( aFilePath, out aMidiMapSet );

    /// <summary>
    /// MidiMapセットテンプレートの保存
    /// </summary>
    /// <param name="aFilePath">テンプレートファイルパス</param>
    /// <param name="aMidiMapSet">保存MidiMapセット</param>
    /// <returns>True:保存成功、False:保存失敗</returns>
    public static bool SaveMidiMapSet( GeneralPath aFilePath, MidiMapSet aMidiMapSet )
        => DrumMidiClassLibrary.pIO.FileIO.SaveMidiMapSet( aFilePath, aMidiMapSet );

    /// <summary>
    /// スコア情報のインポート
    /// </summary>
    /// <param name="aFilePath">スコアファイルパス</param>
    /// <returns>True:インポート成功、False:インポート失敗</returns>
    public static bool ImportScore( GeneralPath aFilePath )
        => DrumMidiClassLibrary.pIO.FileIO.ImportScore( aFilePath );
}
