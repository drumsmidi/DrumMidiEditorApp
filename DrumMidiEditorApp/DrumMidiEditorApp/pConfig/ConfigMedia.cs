using System;
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
    public bool UpdateDmsControlBgm { get; set; } = false;

    /// <summary>
    /// ノート再生情報 再読込フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateDmsControlScore { get; set; } = false;

    /// <summary>
    /// MidiMap情報 再読込フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateDmsControlMidiMap { get; set; } = false;

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
    /// ノート音量ランダム
    /// </summary>
    [JsonInclude]
    public int RandomVolume { get; set; } = 0;

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

    /// <summary>
    /// 周波数解析：FFTレングス
    /// </summary>
    [JsonIgnore]
    public int FFTLength { get; private set; }
        = (int)Math.Pow( 2d, 10d );  // 10=1024,11=2048,12=4096,13=8192,14=16394,15=32798;

    /// <summary>
    /// BGM音量チェック
    /// </summary>
    /// <param name="aVolume">音量</param>
    /// <returns>範囲内の音量(0-100)</returns>
    public int CheckBgmVolume( int aVolume )
    {
        if ( aVolume < BgmMinVolume)
        {
            return BgmMinVolume;
        }
        else if ( aVolume > BgmMaxVolume)
        {
            return BgmMaxVolume;
        }
        return aVolume;
    }

    #endregion

    #region Video

    /// <summary>
    /// 動画出力FPS
    /// </summary>
    [JsonInclude]
    public int OutputVideoFps { get; set; } = 60;

    /// <summary>
    /// 動画出力FPS：最大値
    /// </summary>
    [JsonIgnore]
    public int OutputVideoFpsMin { get; set; } = 30;

    /// <summary>
    /// 動画出力FPS：最小値
    /// </summary>
    [JsonIgnore]
    public int OutputVideoFpsMax { get; set; } = 120;

    /// <summary>
    /// MP4出力コーデック
    /// </summary>
    [JsonInclude]
    public string OutputVideoCodec { get; set; } = String.Empty;

    /// <summary>
    /// MP4出力コーデック一覧
    /// </summary>
    [JsonIgnore]
    public List<string> OutputVideoCodecList = new()
    {
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
    };

    #endregion

    #region Midi

    /// <summary>
    /// MIDIインポート時のBPM倍率
    /// </summary>
    [JsonIgnore]
    public float MidiImportZoom { get; set; } = 1;

    /// <summary>
    /// チェンネル番号最小数（0-15）
    /// </summary>
    [JsonIgnore]
    public byte ChannelMinNo { get; private set; } = 0;

    /// <summary>
    /// チャンネル番号最大数（0-15）
    /// </summary>
    [JsonIgnore]
    public byte ChannelMaxNo { get; private set; } = 15;

    /// <summary>
    /// チャンネル番号リスト（0-15）
    /// </summary>
    [JsonIgnore]
    public List<byte> ChannelNoList { get; private set; } = new()
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
    };

    /// <summary>
    /// Drum midiチャンネル
    /// </summary>
    [JsonIgnore]
    public byte ChannelDrum { get; set; } = 0x9;

    /// <summary>
    /// MIDI最小音量
    /// </summary>
    [JsonIgnore]
    public int MidiMinVolume { get; private set; } = 0;

    /// <summary>
    /// MIDI最大音量
    /// </summary>
    [JsonIgnore]
    public int MidiMaxVolume { get; private set; } = 127;

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
    /// MIDIノート番号最小
    /// </summary>
    [JsonIgnore]
    public byte MidiNoteMin { get; private set; } = 0;

    /// <summary>
    /// MIDIノート番号最大
    /// </summary>
    [JsonIgnore]
    public byte MidiNoteMax { get; private set; } = 127;

    /// <summary>
    /// MIDI-OUT遅延時間（秒）
    /// TODO: チャンネル別の設定が必要
    /// </summary>
    [JsonInclude]
    public double MidiOutLatency { get; set; } = 0;

    /// <summary>
    /// MIDIノート番号チェック
    /// </summary>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <returns>範囲内のMIDIノート番号(0-127)</returns>
    public byte CheckMidiNote( int aMidi )
    {
        if ( aMidi < MidiNoteMin )
        {
            return MidiNoteMin;
        }
        else if ( aMidi > MidiNoteMax )
        {
            return MidiNoteMax;
        }
        return (byte)aMidi;
    }

    /// <summary>
    /// MIDI音量チェック
    /// </summary>
    /// <param name="aVolume">音量</param>
    /// <returns>範囲内の音量(0-127)</returns>
    public int CheckMidiVolume( int aVolume )
    {
        if ( aVolume < MidiMinVolume )
        {
            return MidiMinVolume;
        }
        else if ( aVolume > MidiMaxVolume)
        {
            return MidiMaxVolume;
        }
        return aVolume;
    }

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
        else if ( aVolume > MidiAddMaxVolume)
        {
            return MidiAddMaxVolume;
        }
        return aVolume;
    }

    #endregion

    #region Device

    /// <summary>
    /// MIDI-OUTデバイス名
    /// </summary>
    [JsonInclude]
    public string MidiOutDeviceName { get; set; } = String.Empty;

    #endregion
}
