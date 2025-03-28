using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pModel;

/// <summary>
/// MidiMapGroupプレイヤー表示情報
/// </summary>
public partial class MidiMapGroupPosition : DisposeBaseClass
{
    /// <summary>
    /// プレイヤー表示位置X座標
    /// </summary>
    public float X { get; set; } = ConfigLib.System.DefaultMidiMapGroupPositionX;

    /// <summary>
    /// プレイヤー表示位置X座標
    /// </summary>
    public float Y { get; set; } = ConfigLib.System.DefaultMidiMapGroupPositionY;

    /// <summary>
    /// プレイヤー表示倍率
    /// </summary>
    public float Magnification { get; set; } = ConfigLib.System.DefaultMidiMapGroupMagnification;

    /// <summary>
    /// 複製
    /// </summary>
    /// <returns>複製データ</returns>
    public MidiMapGroupPosition Clone()
    {
        return new()
        {
            X               = X,
            Y               = Y,
            Magnification   = Magnification,
        };
    }
}
