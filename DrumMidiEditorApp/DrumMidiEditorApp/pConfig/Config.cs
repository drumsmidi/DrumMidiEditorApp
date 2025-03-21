﻿using DrumMidiLibrary.pConfig;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// 設定情報
/// </summary>
public static class Config
{
    /// <summary>
    /// システム設定
    /// </summary>
    public static ConfigSystem System
    { 
        get => ConfigLib.System;
        set => ConfigLib.System = value;
    }

    /// <summary>
    /// Audio/Midi/Video設定
    /// </summary>
    public static ConfigMedia Media
    {
        get => ConfigLib.Media;
        set => ConfigLib.Media = value;
    }

    /// <summary>
    /// File設定
    /// </summary>
    public static ConfigFile File { get; set; } = new();

    /// <summary>
    /// Editerタブ設定
    /// </summary>
    public static ConfigEditer Editer { get; set; } = new();

    /// <summary>
    /// イコライザ設定
    /// </summary>
    public static ConfigEqualizer Equalizer { get; set; } = new();

    /// <summary>
    /// 機械学習用設定
    /// </summary>
    public static ConfigMachine Machine { get; set; } = new();

    /// <summary>
    /// Scoreタブ設定
    /// </summary>
    public static ConfigScore Score { get; set; } = new();

    /// <summary>
    /// Scaleタブ設定
    /// </summary>
    public static ConfigScale Scale { get; set; } = new();

    /// <summary>
    /// Window設定
    /// </summary>
    public static ConfigWindow Window { get; set; } = new();

    /// <summary>
    /// Player設定
    /// </summary>
    public static ConfigPlayer Player { get; set; } = new();
}
