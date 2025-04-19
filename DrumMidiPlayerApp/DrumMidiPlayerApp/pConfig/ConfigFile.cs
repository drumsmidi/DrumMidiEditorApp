using DrumMidiLibrary.pUtil;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DrumMidiPlayerApp.pConfig;

/// <summary>
/// ディレクトリ・ファイル関連設定
/// </summary>
public class ConfigFile
{
    #region Folder Path

    /// <summary>
    /// ドキュメントベースフォルダパス
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderDocBase { get; private set; } = new( $"{HelperAppDirectory.LocalFolder.Path}\\" );

    /// <summary>
    /// Configフォルダ（アプリの設定ファイル格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderConfig
     // => new( FolderDocBase.AbsoulteFolderPath );
        => new( FolderDocBase.CombineWithSubFolder( "Config\\" ) );

    /// <summary>
    /// DMSフォルダパス
    /// </summary>
    [JsonIgnore]
    public List<GeneralPath> FolderDms { get; private set; } =
    [
        new( $"{HelperAppDirectory.MyDocumentsDirectory}\\" ),
    ];




    #endregion

    #region Filename

    /// <summary>
    /// 設定ファイル名：システム設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigSystem = "ConfigSystem.json";

    /// <summary>
    /// 設定ファイル名：メディア設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigMedia = "ConfigMedia.json";

    /// <summary>
    /// 設定ファイル名：ファイル設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigFile = "ConfigFile.json";

    /// <summary>
    /// 設定ファイル名：Window設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigWindow = "ConfigWindow.json";

    /// <summary>
    /// 設定ファイル名：Player設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigPlayer = "ConfigPlayer.json";

    #endregion

    #region Extention

    /// <summary>
    /// 拡張子：DMS
    /// </summary>
    [JsonIgnore]
    public static readonly string ExtentionDms = ".dms";

    #endregion
}
