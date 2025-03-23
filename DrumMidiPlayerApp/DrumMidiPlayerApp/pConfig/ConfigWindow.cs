using System.Text.Json.Serialization;

namespace DrumMidiPlayerApp.pConfig;

/// <summary>
/// Window関連設定
/// </summary>
public class ConfigWindow
{
    /// <summary>
    /// アプリケーション名
    /// </summary>
    [JsonIgnore]
    public string AppName { get; private set; } = $"DrumMidiPlayer";

    /// <summary>
    /// エディターウィンドウ初期サイズ：横幅
    /// </summary>
    [JsonInclude]
    public int WindowSizeWidth => (int)Config.Player.ResolutionScreenWidth;

    /// <summary>
    /// エディターウィンドウ初期サイズ：高さ
    /// </summary>
    [JsonInclude]
    public int WindowSizeHeight => (int)Config.Player.ResolutionScreenHeight;
}
