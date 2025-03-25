using DrumMidiLibrary.pConfig;

namespace DrumMidiPlayerApp.pConfig;

/// <summary>
/// Window関連設定
/// </summary>
public class ConfigWindow : ConfigWindowBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ConfigWindow() : base() 
    {
        AppName                     = $"DrumMidiPlayer";
        WindowSizeWidthDpiNoScale   = (int)Config.Panel.ResolutionScreenWidth;
        WindowSizeHeightDpiNoScale  = (int)Config.Panel.ResolutionScreenHeight;
    }
}
