using Windows.System;
using Windows.UI.Core;

namespace DrumMidiLibrary.pInput;

internal class InputDeviceState
{
    public VirtualKey Key { get; set; }

    public CorePhysicalKeyStatus KeyStatus { get; set; }
}
