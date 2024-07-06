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
    /// Parameterless constructor for marshalling
    /// </summary>
    private WaveFormatExtensible()
    {
    }

    /// <summary>
    /// Creates a new WaveFormatExtensible for PCM or IEEE
    /// </summary>
    public WaveFormatExtensible( int rate, int bits, int channels )
        : base( rate, bits, channels )
    {
        waveFormatTag       = WaveFormatEncoding.Extensible;
        extraSize           = 22;
        wValidBitsPerSample = (short)bits;
        for ( var n = 0; n < channels; n++ )
        {
            dwChannelMask |= 1 << n;
        }
        if ( bits == 32 )
        {
            // KSDATAFORMAT_SUBTYPE_IEEE_FLOAT
            SubFormat = AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT;
        }
        else
        {
            // KSDATAFORMAT_SUBTYPE_PCM
            SubFormat = AudioMediaSubtypes.MEDIASUBTYPE_PCM;
        }
    }

    /// <summary>
    /// WaveFormatExtensible for PCM or floating point can be awkward to work with
    /// This creates a regular WaveFormat structure representing the same audio format
    /// Returns the WaveFormat unchanged for non PCM or IEEE float
    /// </summary>
    /// <returns></returns>
    public WaveFormat ToStandardWaveFormat()
    {
        return SubFormat == AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT && bitsPerSample == 32
            ? CreateIeeeFloatWaveFormat( sampleRate, channels )
            : SubFormat == AudioMediaSubtypes.MEDIASUBTYPE_PCM 
            ? new WaveFormat( sampleRate, bitsPerSample, channels ) 
            : this;
        //throw new InvalidOperationException("Not a recognised PCM or IEEE float format");
    }

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
