﻿using DrumMidiLibrary.pUtil;
using System.Text.Json.Serialization;
using Windows.UI;

namespace DrumMidiLibrary.pConfig;

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
    [JsonIgnore]
    public string DefaultScoreInfo { get; private set; } = string.Empty;

    /// <summary>
    /// BGM音量初期値
    /// </summary>
    [JsonIgnore]
    public int DefaultBgmVolume { get; private set; } = 100;

    /// <summary>
    /// Bgmファイルパス初期値
    /// </summary>
    [JsonIgnore]
    public GeneralPath DefaultBgmFilePath { get; private set; } = new();

    /// <summary>
    /// BPM初期値
    /// </summary>
    [JsonIgnore]
    public double DefaultBpm { get; private set; } = 120;

    /// <summary>
    /// BGM再生位置（秒）初期値
    /// </summary>
    [JsonIgnore]
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
    [JsonIgnore]
    public string DefaultMidiMapGroupName { get; private set; } = string.Empty;

    /// <summary>
    /// MidiMapGroup初期値：表示状態
    /// </summary>
    [JsonIgnore]
    public bool DefaultMidiMapGroupDisplay { get; private set; } = true;

    /// <summary>
    /// MidiMapGroup初期値：音量増減値
    /// </summary>
    [JsonIgnore]
    public int DefaultMidiMapGroupVolumeAdd { get; private set; } = 0;

    /// <summary>
    /// MidiMapGroup初期値：音階キー
    /// </summary>
    [JsonIgnore]
    public string DefaultMidiMapGroupScaleKey { get; private set; } = string.Empty;

    /// <summary>
    /// MidiMapGroup初期値：プレイヤー表示位置X座標
    /// </summary>
    [JsonIgnore]
    public float DefaultMidiMapGroupPositionX { get; private set; } = 100;

    /// <summary>
    /// MidiMapGroup初期値：プレイヤー表示位置Y座標
    /// </summary>
    [JsonIgnore]
    public float DefaultMidiMapGroupPositionY { get; private set; } = 100;

    /// <summary>
    /// MidiMapGroup初期値：プレイヤー表示倍率
    /// </summary>
    [JsonIgnore]
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
    [JsonIgnore]
    public string DefaultMidiMapName { get; private set; } = string.Empty;

    /// <summary>
    /// MidiMap初期値：MIDIノート番号
    /// </summary>
    [JsonIgnore]
    public byte DefaultMidiMapMidi { get; private set; } = 0;

    /// <summary>
    /// MidiMap初期値：表示状態
    /// </summary>
    [JsonIgnore]
    public bool DefaultMidiMapDisplay { get; private set; } = true;

    /// <summary>
    /// MidiMap初期値：音量増減値
    /// </summary>
    [JsonIgnore]
    public int DefaultMidiMapVolumeAdd { get; private set; } = 0;

    /// <summary>
    /// MidiMap初期値：ノートの色
    /// </summary>
    [JsonIgnore]
    public Color DefaultMidiMapColor { get; private set; } = HelperColor.WhiteColor;

    /// <summary>
    /// MidiMap初期値：音階名称（ピッチ＋音階）
    /// </summary>
    [JsonIgnore]
    public string DefaultMidiMapScale { get; private set; } = string.Empty;

    /// <summary>
    /// MidiMap初期値：音階キーテキスト
    /// </summary>
    [JsonIgnore]
    public string DefaultMidiMapScaleKey { get; private set; } = string.Empty;

    #endregion

    #region System

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

    #region エディター内のプレイヤー描画モード

    /// <summary>
    /// プレイヤー描画モード
    /// 本来は、エディターの設定に記述するべきだが、ファイル入出力時に使用しているので、ここに記述している。
    /// </summary>
    public enum PlayerSurfaceMode : int
    {
        Sequence = 0,
        Simuration,
        ScoreType2,
    }

    #endregion
}
