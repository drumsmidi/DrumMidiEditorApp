using System.Collections.Generic;
using System.Text.Json.Serialization;
using DrumMidiEditorApp.pUtil;
using DrumMidiEditorApp.pUtil.pHelper;
using Windows.UI;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// システム設定
/// </summary>
public class ConfigSystem
{
    #region Deault:Score

    /// <summary>
    /// Scoreファイルパス初期値
    /// </summary>
    [JsonIgnore]
    public GeneralPath DefaultScoreFilePath { get; private set; } = new();

    /// <summary>
    /// メモ情報初期値
    /// </summary>
    [JsonInclude]
    public string DefaultScoreInfo { get; private set; } = string.Empty;

    /// <summary>
    /// BGM音量初期値
    /// </summary>
    [JsonInclude]
    public int DefaultBgmVolume { get; private set; } = 100;

    /// <summary>
    /// Bgmファイルパス初期値
    /// </summary>
    [JsonIgnore]
    public GeneralPath DefaultBgmFilePath { get; private set; } = new();

    /// <summary>
    /// BPM初期値
    /// </summary>
    [JsonInclude]
    public double DefaultBpm { get; private set; } = 120;

    /// <summary>
    /// BGM再生位置（秒）初期値
    /// </summary>
    [JsonInclude]
    public double DefaultBgmPlaybackStartPosition { get; private set; } = 0;

    #endregion

    #region Deault:MidiMapGroup

    /// <summary>
    /// MidiMapGroupキー：未選択
    /// </summary>
    [JsonIgnore]
    public int MidiMapGroupKeyNotSelect { get; private set; } = -1;

    /// <summary>
    /// MidiMapGroup初期値：ラベル名称
    /// </summary>
    [JsonInclude]
    public string DefaultMidiMapGroupName { get; private set; } = string.Empty;

    /// <summary>
    /// MidiMapGroup初期値：表示状態
    /// </summary>
    [JsonInclude]
    public bool DefaultMidiMapGroupDisplay { get; private set; } = true;

    /// <summary>
    /// MidiMapGroup初期値：音量増減値
    /// </summary>
    [JsonInclude]
    public int DefaultMidiMapGroupVolumeAdd { get; private set; } = 0;

    /// <summary>
    /// MidiMapGroup初期値：音階キー
    /// </summary>
    [JsonInclude]
    public string DefaultMidiMapGroupScaleKey { get; private set; } = string.Empty;

    /// <summary>
    /// MidiMapGroup初期値：プレイヤー表示位置X座業
    /// </summary>
    [JsonInclude]
    public float DefaultMidiMapGroupPositionX { get; private set; } = 100;

    /// <summary>
    /// MidiMapGroup初期値：プレイヤー表示位置X座業
    /// </summary>
    [JsonInclude]
    public float DefaultMidiMapGroupPositionY { get; private set; } = 100;

    /// <summary>
    /// MidiMapGroup初期値：プレイヤー表示位置X座業
    /// </summary>
    [JsonInclude]
    public float DefaultMidiMapGroupMagnification { get; private set; } = 1;

    #endregion

    #region Deault:MidiMap

    /// <summary>
    /// MidiMapキー：未選択
    /// </summary>
    [JsonIgnore]
    public int MidiMapKeyNotSelect { get; private set; } = -1;

    /// <summary>
    /// MidiMap初期値：ラベル名称
    /// </summary>
    [JsonInclude]
    public string DefaultMidiMapName { get; private set; } = string.Empty;

    /// <summary>
    /// MidiMap初期値：MIDIノート番号
    /// </summary>
    [JsonInclude]
    public byte DefaultMidiMapMidi { get; private set; } = 0;

    /// <summary>
    /// MidiMap初期値：表示状態
    /// </summary>
    [JsonInclude]
    public bool DefaultMidiMapDisplay { get; private set; } = true;

    /// <summary>
    /// MidiMap初期値：音量増減値
    /// </summary>
    [JsonInclude]
    public int DefaultMidiMapVolumeAdd { get; private set; } = 0;

    /// <summary>
    /// MidiMap初期値：ノートの色
    /// </summary>
    [JsonInclude]
    public Color DefaultMidiMapColor { get; private set; } = Color.FromArgb( 255, 255, 255, 255 );

    /// <summary>
    /// MidiMap初期値：音階名称（ピッチ＋音階）
    /// </summary>
    [JsonInclude]
    public string DefaultMidiMapScale { get; private set; } = string.Empty;

    /// <summary>
    /// MidiMap初期値：音階キーテキスト
    /// </summary>
    [JsonInclude]
    public string DefaultMidiMapScaleKey { get; private set; } = string.Empty;

    #endregion

    #region Log

    /// <summary>
    /// ログ出力フォームへ出力するログ行数
    /// </summary>
    [JsonInclude]
    public readonly int LogOutputLineCount = 40;

    #endregion

    #region System

    /// <summary>
    /// アプリケーション名
    /// </summary>
    [JsonIgnore]
    public string AppName { get; private set; } = $"DrumMidiEditor";

    /// <summary>
    /// アプリケーション起動元のDMSファイルパス
    /// </summary>
    [JsonIgnore]
    public GeneralPath AppStartDmsPath { get; set; } = new();

    /// <summary>
    /// エディターウィンドウ初期サイズ：横幅
    /// </summary>
    [JsonInclude]
    public int WindowSizeWidth { get; private set; } = 1960;

    /// <summary>
    /// エディターウィンドウ初期サイズ：高さ
    /// </summary>
    [JsonInclude]
    public int WindowSizeHeight { get; private set; } = 1280;

    /// <summary>
    /// 小節最大数（0から開始）。
    /// 4桁以上に設定すると動かなくなります。
    /// </summary>
    [JsonIgnore]
    public int MeasureMaxNumber { get; private set; } = 999;

    /// <summary>
    /// 小節番号書式フォーマット
    /// </summary>
    [JsonIgnore]
    public string MeasureNumberFormat { get; private set; } = "{0:000}";

    /// <summary>
    /// 1小節辺りのノート数
    /// </summary>
    [JsonIgnore]
    public int MeasureNoteNumber { get; private set; } = 128;

    /// <summary>
    /// MidiMapGroup最大数
    /// </summary>
    [JsonIgnore]
    public int MidiMapGroupMaxNumber { get; private set; } = 256;

    /// <summary>
    /// MidiMap最大数
    /// </summary>
    [JsonIgnore]
    public int MidiMapMaxNumber { get; private set; } = 500;

    /// <summary>
    /// 小節最大数
    /// </summary>
    [JsonIgnore]
    public int MeasureCount => MeasureMaxNumber + 1;

    /// <summary>
    /// １行辺りの全小節分のノート数
    /// </summary>
    [JsonIgnore]
    public int NoteCount => MeasureCount * MeasureNoteNumber;

    #endregion

    #region Folder Path

    /// <summary>
    /// ドキュメントベースフォルダパス
    /// </summary>
    [JsonInclude]
    public GeneralPath FolderDocBase { get; private set; } = new( $"{HelperAppDirectory.LocalFolder.Path}\\" );

    /// <summary>
    /// Configフォルダ（アプリの設定ファイル格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderConfig
        => new( FolderDocBase.AbsoulteFolderPath );
     // => new ( FolderDocBase.GetAbsoulteFolderPathCombineSubFolder( "Config\\" ) );

    /// <summary>
    /// Modelフォルダ（機械学習用データ格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderModel
        => new( FolderDocBase.AbsoulteFolderPath );
     // => new (FolderDocBase.GetAbsoulteFolderPathCombineSubFolder( "Model\\" ) );

    /// <summary>
    /// Bgmフォルダ（BGM格納）
    /// </summary>
    [JsonIgnore]
    public string FolderBgm = "DrumMidiEditorBgm";

    /// <summary>
    /// Dmsフォルダ（譜面データ格納）
    /// </summary>
    [JsonIgnore]
    public string FolderDms = "DrumMidiEditorDms";

    /// <summary>
    /// MidiMapSetフォルダ（MidiMapSetテンプレート格納）
    /// </summary>
    [JsonIgnore]
    public string FolderMidiMapSet = "DrumMidiEditorMidiMapSet";

    /// <summary>
    /// Midiフォルダ（Midiデータ格納）
    /// </summary>
    [JsonIgnore]
    public string FolderMidi = "DrumMidiEditorMidi";

    /// <summary>
    /// Exportフォルダ
    /// </summary>
    [JsonIgnore]
    public string FolderExport = "DrumMidiEditorExport";

    #endregion

    #region Filename

    /// <summary>
    /// 設定ファイル名：Editerタブ設定
    /// </summary>
    [JsonIgnore]
    public string FileNameConfigEditer { get; private set; } = "ConfigEditer.json";

    /// <summary>
    /// 設定ファイル名：イコライザ設定
    /// </summary>
    [JsonIgnore]
    public string FileNameConfigEqualizer { get; private set; } = "ConfigEqualizer.json";

    /// <summary>
    /// 設定ファイル名：メディア設定
    /// </summary>
    [JsonIgnore]
    public string FileNameConfigMedia { get; private set; } = "ConfigMedia.json";

    /// <summary>
    /// 設定ファイル名：Player設定
    /// </summary>
    [JsonIgnore]
    public string FileNameConfigPlayer { get; private set; } = "ConfigPlayer.json";

    /// <summary>
    /// 設定ファイル名：Scaleタブ設定
    /// </summary>
    [JsonIgnore]
    public string FileNameConfigScale { get; private set; } = "ConfigScale.json";

    /// <summary>
    /// 設定ファイル名：Scoreタブ設定
    /// </summary>
    [JsonIgnore]
    public string FileNameConfigScore { get; private set; } = "ConfigScore.json";

    /// <summary>
    /// 設定ファイル名：システム設定
    /// </summary>
    [JsonIgnore]
    public string FileNameConfigSystem { get; private set; } = "ConfigSystem.json";

    #endregion

    #region Extention

    /// <summary>
    /// 拡張子：DMS
    /// </summary>
    [JsonIgnore]
    public string ExtentionDms { get; private set; } = ".dms";

    /// <summary>
    /// 拡張子：MIDI
    /// </summary>
    [JsonIgnore]
    public string ExtentionMidi { get; private set; } = ".mid";

    /// <summary>
    /// 拡張子：VIDEO
    /// </summary>
    [JsonIgnore]
    public string ExtentionVideo { get; private set; } = ".mp4";

    /// <summary>
    /// Bgm開くフィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportBgm
    {
        get; private set;
    } = [
            ".mp3",
            ".wav",
        ];

    /// <summary>
    /// Dms保存フィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportDmsSave
    {
        get; private set;
    } = [
            ".dms",
        ];

    /// <summary>
    /// Dms開くフィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportDmsOpen
    {
        get; private set;
    } = [
            ".dms",
            ".mid",
            ".dtx",
        ];

    /// <summary>
    /// MidiMapSetフィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportMidiMapSet
    {
        get; private set;
    } = [
            ".dms",
        ];

    /// <summary>
    /// Midi開くフィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportMidi
    {
        get; private set;
    } = [
            ".mid",
        ];

    /// <summary>
    /// Video保存フィルタ
    /// </summary>
    [JsonIgnore]
    public List<string> SupportVideo
    {
        get; private set;
    } = [
            ".mp4",
        ];

    #endregion
}
