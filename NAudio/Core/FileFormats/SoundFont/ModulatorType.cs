namespace NAudio.Core.FileFormats.SoundFont;

/// <summary>
/// Controller Sources
/// </summary>
public enum ControllerSourceEnum
{
    /// <summary>
    /// No Controller
    /// </summary>
    NoController = 0,
    /// <summary>
    /// Note On Velocity
    /// </summary>
    NoteOnVelocity = 2,
    /// <summary>
    /// Note On Key Number
    /// </summary>
    NoteOnKeyNumber = 3,
    /// <summary>
    /// Poly Pressure
    /// </summary>
    PolyPressure = 10,
    /// <summary>
    /// Channel Pressure
    /// </summary>
    ChannelPressure = 13,
    /// <summary>
    /// Pitch Wheel
    /// </summary>
    PitchWheel = 14,
    /// <summary>
    /// Pitch Wheel Sensitivity
    /// </summary>
    PitchWheelSensitivity = 16
}

/// <summary>
/// Source Types
/// </summary>
public enum SourceTypeEnum
{
    /// <summary>
    /// Linear
    /// </summary>
    Linear,
    /// <summary>
    /// Concave
    /// </summary>
    Concave,
    /// <summary>
    /// Convex
    /// </summary>
    Convex,
    /// <summary>
    /// Switch
    /// </summary>
    Switch
}

/// <summary>
/// Modulator Type
/// </summary>
public class ModulatorType
{
    private readonly bool polarity;
    private readonly bool direction;
    private readonly bool midiContinuousController;
    private readonly ControllerSourceEnum controllerSource;
    private readonly SourceTypeEnum sourceType;
    private readonly ushort midiContinuousControllerNumber;

    internal ModulatorType( ushort raw )
    {
        // TODO: map this to fields
        polarity = ( raw & 0x0200 ) == 0x0200;
        direction = ( raw & 0x0100 ) == 0x0100;
        midiContinuousController = ( raw & 0x0080 ) == 0x0080;
        sourceType = (SourceTypeEnum)( ( raw & 0xFC00 ) >> 10 );

        controllerSource = (ControllerSourceEnum)( raw & 0x007F );
        midiContinuousControllerNumber = (ushort)( raw & 0x007F );

    }

    /// <summary>
    /// <see cref="object.ToString"/>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return midiContinuousController
            ? string.Format( "{0} CC{1}", sourceType, midiContinuousControllerNumber )
            : string.Format( "{0} {1}", sourceType, controllerSource );
    }

}