using System.Collections.Generic;
using System.Text.Json.Serialization;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pUtil;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// ディレクトリ・ファイル関連設定
/// </summary>
public class ConfigFile : IConfig
{
    public void CheckValidation()
    {
        // 何もしない
    }

    #region Folder

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
        => new( FolderDocBase.CombineWithSubFolder( "Config\\" ) );

    /// <summary>
    /// Modelフォルダ（機械学習用データ格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderModel
        => new( FolderDocBase.CombineWithSubFolder( "Model\\" ) );

    /// <summary>
    /// FilePicker用：Bgmフォルダ（BGM格納）
    /// </summary>
    [JsonIgnore]
    public static readonly string FolderBgm = "DrumMidiEditorBgm";

    /// <summary>
    /// FilePicker用：Dmsフォルダ（譜面データ格納）
    /// </summary>
    [JsonIgnore]
    public static readonly string FolderDms = "DrumMidiEditorDms";

    /// <summary>
    /// FilePicker用：MidiMapSetフォルダ（MidiMapSetテンプレート格納）
    /// </summary>
    [JsonIgnore]
    public static readonly string FolderMidiMapSet = "DrumMidiEditorMidiMapSet";

    /// <summary>
    /// FilePicker用：Midiフォルダ（Midiデータ格納）
    /// </summary>
    [JsonIgnore]
    public static readonly string FolderMidi = "DrumMidiEditorMidi";

    /// <summary>
    /// FilePicker用：Exportフォルダ
    /// </summary>
    [JsonIgnore]
    public static readonly string FolderExport = "DrumMidiEditorExport";

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
    /// 設定ファイル名：ログ設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigLog = "ConfigLog.json";

    /// <summary>
    /// 設定ファイル名：Editerタブ設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigEditer = "ConfigEditer.json";

    /// <summary>
    /// 設定ファイル名：イコライザ設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigEqualizer = "ConfigEqualizer.json";

    /// <summary>
    /// 設定ファイル名：ファイル設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigFile = "ConfigFile.json";

    /// <summary>
    /// 設定ファイル名：機械学習設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigMachine = "ConfigMachine.json";

    /// <summary>
    /// 設定ファイル名：Scaleタブ設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigScale = "ConfigScale.json";

    /// <summary>
    /// 設定ファイル名：Scoreタブ設定
    /// </summary>
    [JsonIgnore]
    public static readonly string FileNameConfigScore = "ConfigScore.json";

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

    /// <summary>
    /// 拡張子：DMS
    /// </summary>
    [JsonIgnore]
    public static readonly string ExtentionDtx = ".dtx";

    /// <summary>
    /// 拡張子：MIDI
    /// </summary>
    [JsonIgnore]
    public static readonly string ExtentionMidi = ".mid";

    /// <summary>
    /// 拡張子：MP3
    /// </summary>
    [JsonIgnore]
    public static readonly string ExtentionMp3 = ".mp3";

    /// <summary>
    /// 拡張子：VIDEO
    /// </summary>
    [JsonIgnore]
    public static readonly string ExtentionMp4 = ".mp4";

    /// <summary>
    /// 拡張子：PDF
    /// </summary>
    [JsonIgnore]
    public static readonly string ExtentionPdf = ".pdf";

    /// <summary>
    /// 拡張子：MP3
    /// </summary>
    [JsonIgnore]
    public static readonly string ExtentionWav = ".wav";

    /// <summary>
    /// Bgm開くフィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportBgm { get; private set; } = 
    [
        ExtentionMp3,
        ExtentionWav,
    ];

    /// <summary>
    /// Dms保存フィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportDmsSave { get; private set; } = 
    [
        ExtentionDms,
    ];

    /// <summary>
    /// Dms開くフィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportDmsOpen { get; private set; } = 
    [
        ExtentionDms,
        ExtentionMidi,
        ExtentionDtx,
    ];

    /// <summary>
    /// MidiMapSetフィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportMidiMapSet { get; private set; } = 
    [
        ExtentionDms,
    ];

    /// <summary>
    /// Midi開くフィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportMidi { get; private set; } = 
    [
        ExtentionMidi,
    ];

    /// <summary>
    /// Pdf保存フィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportPdf { get; private set; } = 
    [
        ExtentionPdf,
    ];

    /// <summary>
    /// Video保存フィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportVideo { get; private set; } = 
    [
        ExtentionMp4,
    ];

    #endregion
}
