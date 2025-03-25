using DrumMidiLibrary.pConfig;

namespace DrumMidiEditorApp.pConfig;

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
        AppName                     = $"DrumMidiEditer";
        WindowSizeWidthDpiNoScale   = 1600;
        WindowSizeHeightDpiNoScale  = 900;
    }
}
