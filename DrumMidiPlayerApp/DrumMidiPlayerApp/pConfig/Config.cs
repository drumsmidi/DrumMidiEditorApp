﻿using DrumMidiLibrary.pConfig;

namespace DrumMidiPlayerApp.pConfig;

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
    /// Log設定
    /// </summary>
    public static ConfigLog Log
    {
        get => ConfigLib.Log;
        set => ConfigLib.Log = value;
    }

    /// <summary>
    /// Panel設定
    /// </summary>
    public static ConfigPanel Panel { get; set; } = new();

    /// <summary>
    /// File設定
    /// </summary>
    public static ConfigFile File { get; set; } = new();

    /// <summary>
    /// Window設定
    /// </summary>
    public static ConfigWindow Window { get; set; } = new();

    /// <summary>
    /// Player設定
    /// </summary>
    public static ConfigPlayer Player { get; set; } = new();

    /// <summary>
    /// ScreenBase設定
    /// </summary>
    public static ConfigScreenBase ScreenBase { get; set; } = new();

    /// <summary>
    /// ScreenSongList設定
    /// </summary>
    public static ConfigScreenSongList ScreenSongList { get; set; } = new();

    /// <summary>
    /// ConfigScreenOptionMain設定
    /// </summary>
    public static ConfigScreenOptionMain ScreenOptionMain { get; set; } = new();


    
}
