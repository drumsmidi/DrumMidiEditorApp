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
        AppName = "DrumMidiEditer";

        SetWindowSizeDpiNoScale( 1600, 900 );
    }
}
