using DrumMidiPlayerApp.pView.pPlayer;

namespace DrumMidiPlayerApp.pView;

public static class ControlAccess
{
    public static WindowPlayer? MainWindow { get; set; } = null;

    public static UserControlPlayerPanel2? UCPlayerPanel { get; set; } = null;
}
