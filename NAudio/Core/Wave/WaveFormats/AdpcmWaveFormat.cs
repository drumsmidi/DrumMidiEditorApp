using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace NAudio.Core.Wave.WaveFormats;

/// <summary>
/// Microsoft ADPCM
/// See http://icculus.org/SDL_sound/downloads/external_documentation/wavecomp.htm
/// </summary>
[StructLayout( LayoutKind.Sequential, Pack = 2 )]
public class AdpcmWaveFormat : WaveFormat
{
    private readonly short samplesPerBlock;
    private readonly short numCoeff;
    // 7 pairs of coefficients
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    private readonly short[] coefficients;

    /// <summary>
    /// Empty constructor needed for marshalling from a pointer
    /// </summary>
    private AdpcmWaveFormat() : this( 8000, 1 )
    {
    }

    /// <summary>
    /// Samples per block
    /// </summary>
    public int SamplesPerBlock => samplesPerBlock;

    /// <summary>
    /// Number of coefficients
    /// </summary>
    public int NumCoefficients => numCoeff;

    /// <summary>
    /// Coefficients
    /// </summary>
    public short [] Coefficients => coefficients;

    /// <summary>
    /// Microsoft ADPCM  
    /// </summary>
    /// <param name="sampleRate">Sample Rate</param>
    /// <param name="channels">Channels</param>
    public AdpcmWaveFormat( int sampleRate, int channels ) :
        base( sampleRate, 0, channels )
    {
        waveFormatTag = WaveFormatEncoding.Adpcm;

        // TODO: validate sampleRate, bitsPerSample
        extraSize = 32;
        blockAlign = this.sampleRate switch
        {
            8000 or 11025 => 256,
            22050 => 512,
            _ => 1024,
        };
        bitsPerSample = 4;
        samplesPerBlock = (short)( ( ( blockAlign - ( 7 * channels ) ) * 8 / ( bitsPerSample * channels ) ) + 2 );
        averageBytesPerSecond =
            SampleRate * blockAlign / samplesPerBlock;

        // samplesPerBlock = blockAlign - (7 * channels)) * (2 / channels) + 2;


        numCoeff = 7;
        coefficients = [
            256,0,512,-256,0,0,192,64,240,0,460,-208,392,-232
        ];
    }

    /// <summary>
    /// Serializes this wave format
    /// </summary>
    /// <param name="writer">Binary writer</param>
    public override void Serialize( System.IO.BinaryWriter writer )
    {
        base.Serialize( writer );
        writer.Write( samplesPerBlock );
        writer.Write( numCoeff );
        foreach ( var coefficient in coefficients )
        {
            writer.Write( coefficient );
        }
    }

    /// <summary>
    /// String Description of this WaveFormat
    /// </summary>
    public override string ToString()
    {
        return string.Format( "Microsoft ADPCM {0} Hz {1} channels {2} bits per sample {3} samples per block",
            SampleRate, channels, bitsPerSample, samplesPerBlock );
    }
}
