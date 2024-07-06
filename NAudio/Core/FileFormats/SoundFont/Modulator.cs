namespace NAudio.Core.FileFormats.SoundFont;

/// <summary>
/// Transform Types
/// </summary>
public enum TransformEnum
{
    /// <summary>
    /// Linear
    /// </summary>
    Linear = 0
}

/// <summary>
/// Modulator
/// </summary>
public class Modulator
{

    /// <summary>
    /// Source Modulation data type
    /// </summary>
    public ModulatorType SourceModulationData
    {
        get; set;
    }

    /// <summary>
    /// Destination generator type
    /// </summary>
    public GeneratorEnum DestinationGenerator
    {
        get; set;
    }

    /// <summary>
    /// Amount
    /// </summary>
    public short Amount
    {
        get; set;
    }

    /// <summary>
    /// Source Modulation Amount Type
    /// </summary>
    public ModulatorType SourceModulationAmount
    {
        get; set;
    }

    /// <summary>
    /// Source Transform Type
    /// </summary>
    public TransformEnum SourceTransform
    {
        get; set;
    }

    /// <summary>
    /// <see cref="object.ToString"/>
    /// </summary>
    public override string ToString()
    {
        return string.Format( "Modulator {0} {1} {2} {3} {4}",
            SourceModulationData, DestinationGenerator,
            Amount, SourceModulationAmount, SourceTransform );
    }

}