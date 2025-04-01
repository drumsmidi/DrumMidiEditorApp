using Windows.System;

namespace DrumMidiLibrary.pInput;

public class InputDeviceState
{
    public VirtualKey Key { get; set; }

    public VirtualKey MapKey { get; set; } = VirtualKey.None;

    public bool IsKeyPushed { get; set; } = false;

    public bool IsKeyReleased { get; set; } = false;

    public int RepeatCount { get; set; } = 0;

    public double StartTime { get; set; } = 0;

    public double EndTime { get; set; } = 0;
}
