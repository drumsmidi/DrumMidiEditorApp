using System;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Windows.Forms;

using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pConfig;

/// <summary>
/// システム設定
/// </summary>
public class ConfigSystem
{
    #region Setting

    /// <summary>
    /// 編集中のDMSファイルパス
    /// </summary>
    [JsonIgnore]
    public GeneralPath OpenFilePath { get; set; } = new();

    #endregion

    #region Deault:Score

    /// <summary>
    /// Scoreファイルパス初期値
    /// </summary>
    [JsonIgnore]
    public GeneralPath DefaultScoreFilePath { get; private set; } = new();

    /// <summary>
    /// ScoreInfoファイルパス初期値
    /// </summary>
    [JsonInclude]
    public string DefaultScoreInfo { get; private set; } = String.Empty;

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
    /// BGM再生位置初期値
    /// </summary>
    [JsonInclude]
    public double DefaultBgmReproducePosition { get; private set; } = 0;

    #endregion

    #region Deault:MidiMapGroup

    /// <summary>
    /// MidiMapGroupキー：未選択
    /// </summary>
    [JsonIgnore]
    public int MidiMapGroupKeyNotSelect { get; private set; } = -1;

    /// <summary>
    /// MidiMap初期値：ラベル名称
    /// </summary>
    [JsonInclude]
    public string DefaultMidiMapGroupName { get; private set; } = String.Empty;

    /// <summary>
    /// MidiMap初期値：表示状態
    /// </summary>
    [JsonInclude]
    public bool DefaultMidiMapGroupDisplay { get; private set; } = true;

    /// <summary>
    /// MidiMap初期値：音量増減値
    /// </summary>
    [JsonInclude]
    public int DefaultMidiMapGroupVolumeAdd { get; private set; } = 0;

    /// <summary>
    /// MidiMap初期値：プレイヤー表示位置X座業
    /// </summary>
    [JsonInclude]
    public float DefaultMidiMapGroupPositionX { get; private set; } = 100;

    /// <summary>
    /// MidiMap初期値：プレイヤー表示位置X座業
    /// </summary>
    [JsonInclude]
    public float DefaultMidiMapGroupPositionY { get; private set; } = 100;

    /// <summary>
    /// MidiMap初期値：プレイヤー表示位置X座業
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
    public string DefaultMidiMapName { get; private set; } = String.Empty;

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
    public Color DefaultMidiMapColor { get; private set; } = Color.LightGray;

    /// <summary>
    /// MidiMap初期値：音階名称（ピッチ＋音階）
    /// </summary>
    [JsonInclude]
    public string DefaultMidiMapScale { get; private set; } = String.Empty;

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
    /// 小節最大数（0から開始）。
    /// 4桁以上に設定すると動かなくなります。
    /// </summary>
    [JsonIgnore]
    public int MeasureMaxNumber { get; private set; } = 999;

    /// <summary>
    /// 1小節辺りのノート数
    /// </summary>
    [JsonIgnore]
    public int MeasureNoteNumber { get; private set; } = 128;

    /// <summary>
    /// MidiMapGroup最大数
    /// </summary>
    [JsonInclude]
    public int MidiMapGroupMaxNumber { get; private set; } = 256;

    /// <summary>
    /// MidiMap最大数
    /// </summary>
    [JsonInclude]
    public int MidiMapMaxNumber { get; private set; } = 800;

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
    /// アプリケーション起動フォルダパス
    /// </summary>
    [JsonIgnore]
    private static readonly string _FolederBase = Application.StartupPath.Replace('/', '\\');

    /// <summary>
    /// アプリケーション起動フォルダパス
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderApp { get; private set; } = new( _FolederBase );

    /// <summary>
    /// Bgmフォルダ（BGM格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderBgm { get; private set; } = new( _FolederBase + "Bgm\\" );

    /// <summary>
    /// Configフォルダ（アプリの設定ファイル格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderConfig { get; private set; } = new( _FolederBase + "Config\\" );

    /// <summary>
    /// Dmsフォルダ（譜面データ格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderDms { get; private set; } = new( _FolederBase + "Dms\\" );

    /// <summary>
    /// MidiMapSetフォルダ（MidiMapSetテンプレート格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderMidiMapSet { get; private set; } = new( _FolederBase + "MidiMapSet\\" );

    /// <summary>
    /// Midiフォルダ（Midiファイル格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderMidi { get; private set; } = new( _FolederBase + "Midi\\" );

    /// <summary>
    /// Modelフォルダ（機械学習用データ格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderModel { get; private set; } = new( _FolederBase + "Model\\" );

    /// <summary>
    /// TechManiaフォルダ（Techファイル格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderTechMania { get; private set; } = new( _FolederBase + "TechMania\\" );

    /// <summary>
    /// Videoフォルダ（動画ファイル格納）
    /// </summary>
    [JsonIgnore]
    public GeneralPath FolderVideo { get; private set; } = new( _FolederBase + "Video\\" );

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
    /// Bgm開くフィルタ
    /// </summary>
    [JsonIgnore]
    public string SupportBgm { get; private set; } = "BGMファイル|*.mp3;*.wav;";

    /// <summary>
    /// Dms保存フィルタ
    /// </summary>
    [JsonIgnore]
    public string SupportDmsSave { get; private set; } = "DMSファイル|*.dms;";

    /// <summary>
    /// Dms開くフィルタ
    /// </summary>
    [JsonIgnore]
    public string SupportDmsOpen { get; private set; } = "DMS/MIDI/DTXファイル|*.dms;*.mid;*.dtx;";

    /// <summary>
    /// MidiMapSetフィルタ
    /// </summary>
    [JsonIgnore]
    public string SupportMidiMapSet { get; private set; } = "DMSファイル|*.dms;";

    /// <summary>
    /// Midi開くフィルタ
    /// </summary>
    [JsonIgnore]
    public string SupportMidi { get; private set; } = "MIDIファイル (*.mid)|*.mid;";

    /// <summary>
    /// TechMania開くフィルタ
    /// </summary>
    [JsonIgnore]
    public string SupportTechMania { get; private set; } = "Techファイル (*.tech)|*.tech;";

    /// <summary>
    /// Video保存フィルタ
    /// </summary>
    [JsonIgnore]
    public string SupportVideo { get; private set; } = "VIDEOファイル|*.mp4;";

    #endregion
}
