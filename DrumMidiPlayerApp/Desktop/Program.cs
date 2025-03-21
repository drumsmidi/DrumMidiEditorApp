using osu.Framework;

namespace DrumMidiPlayerApp.Desktop;

public static class Program
{
    public static void Main()
    {
        using var host = Host.GetSuitableDesktopHost( @"DrumMidiPlayerApp" );
        { 
            using var game = new Game.Game();

            host.Run( game );
        }
    }
}
