using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// Audio/Midi設定
/// </summary>
public class ConfigMedia
{
    #region フラグ

    /// <summary>
    /// シーケンス ＢＧＭ再読込フラグ
    /// </summary>
    [JsonIgnore]
    public bool FlagUpdateDmsControlBgm { get; set; } = false;

    /// <summary>
    /// ノート再生情報 再読込フラグ
    /// </summary>
    [JsonIgnore]
    public bool FlagUpdateDmsControlScore { get; set; } = false;

    /// <summary>
    /// MidiMap情報 再読込フラグ
    /// </summary>
    [JsonIgnore]
    public bool FlagUpdateDmsControlMidiMap { get; set; } = false;

    #endregion

    #region Setting

    /// <summary>
    /// ループ再生：小節開始番号
    /// </summary>
    [JsonIgnore]
    public int PlayLoopStart { get; set; } = 0;

    /// <summary>
    /// ループ再生：小節終了番号
    /// </summary>
    [JsonIgnore]
    public int PlayLoopEnd { get; set; } = 5;

    /// <summary>
    /// ループ再生：小節開始／終了番号 接続
    /// </summary>
    [JsonIgnore]
    public bool PlayLoopConnectOn { get; set; } = true;

    /// <summary>
    /// ループ再生：小節開始～終了間の長さ
    /// </summary>
    [JsonIgnore]
    public int PlayLoopConnect { get; set; } = 5;

    /// <summary>
    /// BGM再生ONフラグ
    /// </summary>
    [JsonIgnore]
    public bool BgmPlayOn { get; set; } = true;

    /// <summary>
    /// NOTE再生ONフラグ
    /// </summary>
    [JsonIgnore]
    public bool NotePlayOn { get; set; } = true;

    /// <summary>
    /// BGM音量（Musicスレッド リアルタイム更新用）
    /// </summary>
    [JsonIgnore]
    public int BgmVolume { get; set; } = Config.System.DefaultBgmVolume;

    /// <summary>
    /// ノート音量ランダム
    /// </summary>
    [JsonInclude]
    public int RandomVolume { get; set; } = 0;

    #endregion

    #region Graphic

    /// <summary>
    /// 初期値：フォント
    /// </summary>
    [JsonIgnore]
    public string DefaultFontFamily { get; private set; } = $"system-ui";

    /// <summary>
    /// DPI
    /// </summary>
    [JsonInclude]
    public float DefaultDpi { get; set; } = 96;

    #endregion

    #region Audio

    /// <summary>
    /// BGM最小音量
    /// </summary>
    [JsonIgnore]
    public int BgmMinVolume { get; private set; } = 0;

    /// <summary>
    /// BGM最大音量
    /// </summary>
    [JsonIgnore]
    public int BgmMaxVolume { get; private set; } = 100;

    /// <summary>
    /// BPM最小音量
    /// </summary>
    [JsonIgnore]
    public double BpmMinVolume { get; private set; } = 1;

    /// <summary>
    /// BPM最大音量
    /// </summary>
    [JsonIgnore]
    public double BpmMaxVolume { get; private set; } = 999;

    /// <summary>
    /// BGM再生開始位置（秒）：最小値
    /// </summary>
    [JsonIgnore]
    public double BgmPlaybackStartMinPosition { get; private set; } = -1000;

    /// <summary>
    /// BGM再生開始位置（秒）：最大値
    /// </summary>
    [JsonIgnore]
    public double BgmPlaybackStartMaxPosition { get; private set; } = 1000;

    #endregion

    #region Video

    /// <summary>
    /// 動画出力FPS
    /// </summary>
    [JsonInclude]
    public int OutputVideoFps { get; set; } = 60;

    /// <summary>
    /// 動画出力FPS：最小値
    /// </summary>
    [JsonIgnore]
    public int OutputVideoFpsMin { get; set; } = 30;

    /// <summary>
    /// 動画出力FPS：最大値
    /// </summary>
    [JsonIgnore]
    public int OutputVideoFpsMax { get; set; } = 120;

    /// <summary>
    /// MP4出力コーデック
    /// </summary>
    [JsonInclude]
    public string OutputVideoCodec { get; set; } = string.Empty;

    /// <summary>
    /// MP4出力コーデック一覧
    /// </summary>
    [JsonIgnore]
    public List<string> OutputVideoCodecList =
    [
        "AVC",
        "CVID",
        "DIB",
        "DIV3",
        "DIVX",
        "DV25",
        "DV50",
        "DVC",
        "DVH1",
        "DVHD",
        "DVSD",
        "DVSL",
        "H261",
        "H263",
        "H264",
        "H265",
        "HEVC",
        "I420",
        "IV32",
        "IV41",
        "IV50",
        "IYUB",
        "IYUV",
        "JPEG",
        "M4S2",
        "MJPG",
        "MP42",
        "MP43",
        "MP4S",
        "MP4V",
        "MPG1",
        "MPG2",
        "MPG4",
        "MSS1",
        "MSS2",
        "MSVC",
        "PIM1",
        "WMV1",
        "WMV2",
        "WMV3",
        "WVC1",
        "X264",
        "XVID",
    ];

    #endregion

    #region Midi

    /// <summary>
    /// MIDIインポート時のBPM倍率
    /// </summary>
    [JsonIgnore]
    public float MidiImportZoom { get; set; } = 1;

    /// <summary>
    /// MIDI最小音量増減
    /// </summary>
    [JsonIgnore]
    public int MidiAddMinVolume { get; private set; } = -127;

    /// <summary>
    /// MIDI最大音量増減
    /// </summary>
    [JsonIgnore]
    public int MidiAddMaxVolume { get; private set; } = +127;

    /// <summary>
    /// MIDI音量増減チェック
    /// </summary>
    /// <param name="aVolume">音量</param>
    /// <returns>範囲内の音量(0-127)</returns>
    public int CheckMidiAddVolume( int aVolume )
    {
        if ( aVolume < MidiAddMinVolume )
        {
            return MidiAddMinVolume;
        }
        else if ( aVolume > MidiAddMaxVolume )
        {
            return MidiAddMaxVolume;
        }
        return aVolume;
    }

    /// <summary>
    /// MIDI-OUT遅延時間（秒）
    /// </summary>
    [JsonInclude]
    public double MidiOutLatency { get; set; } = 0;

    #endregion

    #region Device

    /// <summary>
    /// MIDI-OUTデバイス名
    /// </summary>
    [JsonInclude]
    public string MidiOutDeviceName { get; set; } = string.Empty;

    #endregion
}
