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
        AppName = "DrumMidiPlayer";

        SetWindowSizeDpiNoScale( (int)Config.Panel.ResolutionScreenWidth, (int)Config.Panel.ResolutionScreenHeight );
    }
}
