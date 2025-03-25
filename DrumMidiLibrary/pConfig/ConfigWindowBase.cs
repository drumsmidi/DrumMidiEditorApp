using System.Text.Json.Serialization;

namespace DrumMidiLibrary.pConfig;

/// <summary>
/// Window関連設定
/// </summary>
public abstract class ConfigWindowBase
{
    /// <summary>
    /// アプリケーション名
    /// </summary>
    [JsonIgnore]
    public string AppName { get; protected set; } = $"DrumMidiLibrary";

    /// <summary>
    /// DPI
    /// </summary>
    [JsonInclude]
    public float DefaultDpi { get; set; } = 96;

    /// <summary>
    /// DPIスケール
    /// </summary>
    [JsonIgnore]
    public double DpiScale { get; set; } = 1;

    /// <summary>
    /// ウィンドウ初期サイズ：横幅
    /// </summary>
    [JsonIgnore]
    public int WindowSizeWidthDpiNoScale { get; set; } = 1920;

    /// <summary>
    /// ウィンドウ初期サイズ：高さ
    /// </summary>
    [JsonIgnore]
    public int WindowSizeHeightDpiNoScale { get; set; } = 1080;

    /// <summary>
    /// ウィンドウ初期サイズ ✕ DPIスケール：横幅
    /// </summary>
    [JsonIgnore]
    public int WindowSizeWidthDpiScale => (int)( WindowSizeWidthDpiNoScale * DpiScale );

    /// <summary>
    /// ウィンドウ初期サイズ ✕ DPIスケール：高さ
    /// </summary>
    [JsonIgnore]
    public int WindowSizeHeightDpiScale => (int)( WindowSizeHeightDpiNoScale * DpiScale );
}
