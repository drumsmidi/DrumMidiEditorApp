using System.Collections.Generic;
using Windows.System;

namespace DrumMidiLibrary.pInput;

/// <summary>
/// プレイヤーサーフェイスインタフェース
/// </summary>
public class InputMap
{
    public Dictionary<VirtualKey, VirtualKey> KeyMap = [];        
}
