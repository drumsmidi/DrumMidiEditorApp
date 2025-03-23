using DrumMidiPlayerApp.pConfig;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pIO.pJson;
using DrumMidiLibrary.pIO.pScore;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel;
using DrumMidiLibrary.pUtil;
using System;
using System.Collections.Generic;

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
    public static bool LoadConfig()
    {
        using var _ = new LogBlock( Log.GetThisMethodName );

#pragma warning disable IDE0017 // オブジェクトの初期化を簡略化します
        var path = new GeneralPath( Config.File.FolderConfig );
#pragma warning restore IDE0017 // オブジェクトの初期化を簡略化します

        // ConfigSystem
        path.FileName    = ConfigFile.FileNameConfigSystem;
        Config.System    = LoadConfig<ConfigSystem>( path ) ?? Config.System;

        // ConfigMedia
        path.FileName    = ConfigFile.FileNameConfigMedia;
        Config.Media     = LoadConfig<ConfigMedia>( path ) ?? Config.Media;

        // ConfigFile
        path.FileName    = ConfigFile.FileNameConfigFile;
        Config.File      = LoadConfig<ConfigFile>( path ) ?? Config.File;

        // ConfigWindow
        path.FileName    = ConfigFile.FileNameConfigWindow;
        Config.Window    = LoadConfig<ConfigWindow>( path ) ?? Config.Window;

        // ConfigPlayer
        path.FileName    = ConfigFile.FileNameConfigPlayer;
        Config.Player    = LoadConfig<ConfigPlayer>( path ) ?? Config.Player;

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
    public static bool SaveConfig()
    {
        using var _ = new LogBlock( Log.GetThisMethodName );

        // サブフォルダ作成
        DocumentFolderStructure();

#pragma warning disable IDE0017 // オブジェクトの初期化を簡略化します
        var path = new GeneralPath( Config.File.FolderConfig );
#pragma warning restore IDE0017 // オブジェクトの初期化を簡略化します

        // ConfigSystem
        path.FileName = ConfigFile.FileNameConfigSystem;
        SaveConfig( path, Config.File );

        // ConfigMedia
        path.FileName = ConfigFile.FileNameConfigMedia;
        SaveConfig( path, Config.Media );

        // ConfigFile
        path.FileName = ConfigFile.FileNameConfigFile;
        SaveConfig( path, Config.File );

        // ConfigWindow
        path.FileName = ConfigFile.FileNameConfigWindow;
        SaveConfig( path, Config.Window );

        // ConfigPlayer
        path.FileName = ConfigFile.FileNameConfigPlayer;
        SaveConfig( path, Config.Player );

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

    #region Create sub folder

    /// <summary>
    /// ドキュメントフォルダを構築
    /// </summary>
    public static void DocumentFolderStructure()
    {
        var folderList = new List<GeneralPath>()
        {
            Config.File.FolderConfig,
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

            _ = aFolderPath.CreateDirectory();

            Log.Info( $"Folder creation successful [{aFolderPath.AbsoulteFolderPath}]" );
        }
        catch ( Exception e )
        {
            Log.Error( $"Folder creation failure [{aFolderPath.AbsoulteFolderPath}]" );
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
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
        using var _ = new LogBlock( Log.GetThisMethodName );

        aScore = new();

        try
        {
            var ext = aFilePath.Extension.ToLower();

            if ( ext.Equals( ConfigFile.ExtentionDms ) )
            {
                ScoreIO.LoadFile( aFilePath, out aScore );
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
}
