namespace DrumMidiLibrary.pConfig;

/// <summary>
/// 設定情報
/// </summary>
public static class ConfigLib
{
    /// <summary>
    /// システム設定
    /// </summary>
    public static ConfigSystem System { get; set; } = new();

    /// <summary>
    /// Audio/Midi/Video設定
    /// </summary>
    public static ConfigMedia Media { get; set; } = new();
}
