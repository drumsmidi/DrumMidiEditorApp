using DrumMidiClassLibrary.pConfig;
using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pModel;
using DrumMidiClassLibrary.pUtil;

using DrumMidiPlayerApp.pConfig;

namespace DrumMidiPlayerApp.pIO;

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

        // ConfigPlayer
        path.FileName = Config.System.FileNameConfigPlayer;
        ConfigLocal.Player = LoadConfig<ConfigPlayer>( path ) ?? ConfigLocal.Player ;

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

        // ConfigPlayer
        path.FileName = Config.System.FileNameConfigPlayer;
        SaveConfig<ConfigPlayer>( path, ConfigLocal.Player );

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
}
