using DrumMidiEditorApp.pView.pEditer;
using DrumMidiEditorApp.pView.pMidiMap;
using DrumMidiEditorApp.pView.pMusic;
using DrumMidiEditorApp.pView.pStatusBar;

namespace DrumMidiEditorApp.pView;

public static class ControlAccess
{
    public static WindowEditer? MainWindow { get; set; } = null;

    public static PageEditerMain? PageEditerMain { get; set; } = null;

    public static PageMusic? PageMusic { get; set; } = null;

    public static PageEdit? PageEdit { get; set; } = null;

    public static PageStatusBar? PageStatusBar { get; set; } = null;

    public static UserControlEqualizer? UCEqualizer { get; set; } = null;

    public static UserControlMidiMapPanel? UCMidiMapPanel { get; set; } = null;

    public static UserControlKeyChangePanel? UCKeyChangePanel { get; set; } = null;
}
