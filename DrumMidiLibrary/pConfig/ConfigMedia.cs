using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DrumMidiLibrary.pModel.pScore;

namespace DrumMidiLibrary.pConfig;

/// <summary>
/// Audio/Midi/Video設定
/// </summary>
public class ConfigMedia : IConfig
{
    public void CheckValidation()
    {
        OutputVideoFps      = CheckOutputVideoFps( OutputVideoFps );
        OutputVideoCodec    = OutputVideoCodecList.Find( val => val.Equals( OutputVideoCodec ) ) ?? string.Empty ;
        MidiOutLatency      = CheckMidiOutLatency( MidiOutLatency );
    }

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

    #region スコア

    /// <summary>
    /// 非同期処理用スコア情報
    /// </summary>
    [JsonIgnore]
    private Score _Score = new();

    /// <summary>
    /// 非同期処理用スコア情報
    /// </summary>
    [JsonIgnore]
    public Score SCORE
    {
        get
        {
            lock ( _Score.LockObj )
            {
                return _Score.Clone();
            }
        }
        set
        {
            lock ( _Score.LockObj )
            {
                lock ( value.LockObj )
                {
                    _Score = value;
                }
            }
        }
    }

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
    public int BgmVolume { get; set; } = ConfigLib.System.DefaultBgmVolume;

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
    public string DefaultFontFamily { get; private set; } = "system-ui";

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
    /// BPM最小値
    /// </summary>
    [JsonIgnore]
    public double BpmMin { get; private set; } = 1;

    /// <summary>
    /// BPM最大値
    /// </summary>
    [JsonIgnore]
    public double BpmMax { get; private set; } = 999;

    /// <summary>
    /// 小節番号書式フォーマット
    /// （MeasureMaxNumber の設定を元に計算）
    /// </summary>
    [JsonIgnore]
    public string BpmNumberFormat { get; private set; } = $"{0:000.00}";

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

    /// <summary>
    /// BGM再生バッファの遅延時間（秒）
    /// </summary>
    [JsonIgnore]
    public double BgmDesiredLatency { get; set; } = 0.200D;

    /// <summary>
    /// BGM再生バッファの遅延時間（秒）：最小値
    /// </summary>
    [JsonIgnore]
    public double BgmDesiredLatencyMin { get; private set; } = 0.05D;

    /// <summary>
    /// BGM再生バッファの遅延時間（秒）：最大値
    /// </summary>
    [JsonIgnore]
    public double BgmDesiredLatencyMax { get; private set; } = 0.50D;

    /// <summary>
    /// BGM再生バッファの遅延時間（秒）チェック
    /// </summary>
    /// <param name="aBgmDesiredLatency">BGM再生バッファの遅延時間（秒）</param>
    /// <returns>範囲内のBGM再生バッファの遅延時間（秒）</returns>
    public double CheckBgmDesiredLatency( double aBgmDesiredLatency )
        => Math.Clamp( aBgmDesiredLatency, BgmDesiredLatencyMin, BgmDesiredLatencyMax );

    /// <summary>
    /// BGM再生バッファ数
    /// </summary>
    [JsonIgnore]
    public int BgmNumberOfBuffers { get; set; } = 2;

    /// <summary>
    /// BGM再生バッファ数：最小値
    /// </summary>
    [JsonIgnore]
    public int BgmNumberOfBuffersMin { get; private set; } = 2;

    /// <summary>
    /// BGM再生バッファ数：最大値
    /// </summary>
    [JsonIgnore]
    public int BgmNumberOfBuffersMax { get; private set; } = 4;

    /// <summary>
    /// BGM再生バッファ数チェック
    /// </summary>
    /// <param name="aBgmNumberOfBuffers">BGM再生バッファ数</param>
    /// <returns>範囲内のBGM再生バッファ数</returns>
    public int CheckBgmNumberOfBuffers( int aBgmNumberOfBuffers )
        => Math.Clamp( aBgmNumberOfBuffers, BgmNumberOfBuffersMin, BgmNumberOfBuffersMax );

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
    public int OutputVideoFpsMin { get; private set; } = 30;

    /// <summary>
    /// 動画出力FPS：最大値
    /// </summary>
    [JsonIgnore]
    public int OutputVideoFpsMax { get; private set; } = 120;

    /// <summary>
    /// 動画出力FPSチェック
    /// </summary>
    /// <param name="aOutputVideoFps">動画出力FPS</param>
    /// <returns>範囲内の音量(30-120)</returns>
    public int CheckOutputVideoFps( int aOutputVideoFps )
        => Math.Clamp( aOutputVideoFps, OutputVideoFpsMin, OutputVideoFpsMax );

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
    /// TODO: MIDIインポート時のBPM倍率 仮作成
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
        => Math.Clamp( aVolume, MidiAddMinVolume, MidiAddMaxVolume );

    /// <summary>
    /// MIDI-OUT遅延時間（秒）
    /// </summary>
    [JsonInclude]
    public double MidiOutLatency { get; set; } = 0;

    /// <summary>
    /// MIDI-OUT遅延時間（秒）：最小値
    /// </summary>
    [JsonIgnore]
    public double MidiOutLatencyMin { get; private set; } = 0;

    /// <summary>
    /// MIDI-OUT遅延時間（秒）：最大値
    /// </summary>
    [JsonIgnore]
    public double MidiOutLatencyMax { get; private set; } = 4;

    /// <summary>
    /// MIDI-OUT遅延時間（秒）チェック
    /// </summary>
    /// <param name="aMidiOutLatency">音量</param>
    /// <returns>範囲内の音量(0-127)</returns>
    public double CheckMidiOutLatency( double aMidiOutLatency )
        => Math.Clamp( aMidiOutLatency, MidiOutLatencyMin, MidiOutLatencyMax );

    #endregion

    #region Device

    /// <summary>
    /// MIDI-OUTデバイス名
    /// </summary>
    [JsonInclude]
    public string MidiOutDeviceName { get; set; } = string.Empty;

    #endregion
}
