using System;
using System.Runtime.InteropServices;

namespace NAudio.Core.Wave.WaveFormats;

/// <summary>
/// WaveFormatExtensible
/// http://www.microsoft.com/whdc/device/audio/multichaud.mspx
/// </summary>
[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2 )]
public class WaveFormatExtensible : WaveFormat
{
    private readonly short wValidBitsPerSample; // bits of precision, or is wSamplesPerBlock if wBitsPerSample==0
    private readonly int dwChannelMask; // which channels are present in stream

    /// <summary>
    /// SubFormat (may be one of AudioMediaSubtypes)
    /// </summary>
    public Guid SubFormat
    {
        get;
    }

    /// <summary>
    /// Serialize
    /// </summary>
    /// <param name="writer"></param>
    public override void Serialize( System.IO.BinaryWriter writer )
    {
        base.Serialize( writer );
        writer.Write( wValidBitsPerSample );
        writer.Write( dwChannelMask );
        var guid = SubFormat.ToByteArray();
        writer.Write( guid, 0, guid.Length );
    }

    /// <summary>
    /// String representation
    /// </summary>
    public override string ToString()
    {
        return $"WAVE_FORMAT_EXTENSIBLE {AudioMediaSubtypes.GetAudioSubtypeName( SubFormat )} " +
            $"{SampleRate}Hz {Channels} channels {BitsPerSample} bit";
    }
}
