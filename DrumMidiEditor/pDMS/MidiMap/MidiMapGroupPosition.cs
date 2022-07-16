using DrumMidiEditor.pConfig;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pDMS;

/// <summary>
/// MidiMapGroupプレイヤー表示情報
/// </summary>
public class MidiMapGroupPosition : DisposeBaseClass
{
	/// <summary>
	/// プレイヤー表示位置X座標
	/// </summary>
	public float X { get; set; } = Config.System.DefaultMidiMapGroupPositionX;

	/// <summary>
	/// プレイヤー表示位置X座標
	/// </summary>
	public float Y { get; set; } = Config.System.DefaultMidiMapGroupPositionY;

	/// <summary>
	/// プレイヤー表示倍率
	/// </summary>
	public float Magnification { get; set; } = Config.System.DefaultMidiMapGroupMagnification;

	/// <summary>
	/// 複製
	/// </summary>
	/// <returns>複製データ</returns>
	public MidiMapGroupPosition Clone()
    {
		return new()
            {
				X				= this.X,
				Y				= this.Y,
				Magnification	= this.Magnification,
            };
    }
}
