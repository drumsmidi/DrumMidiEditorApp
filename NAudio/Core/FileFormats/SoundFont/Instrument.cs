namespace NAudio.Core.FileFormats.SoundFont;

/// <summary>
/// SoundFont instrument
/// </summary>
public class Instrument
{
    internal ushort startInstrumentZoneIndex;
    internal ushort endInstrumentZoneIndex;

    /// <summary>
    /// instrument name
    /// </summary>
    public string Name
    {
        get; set;
    }

    /// <summary>
    /// Zones
    /// </summary>
    public Zone [] Zones
    {
        get; set;
    }

    /// <summary>
    /// <see cref="object.ToString"/>
    /// </summary>
    public override string ToString() => Name;
}