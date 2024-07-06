using NAudio.Core.Wave.WaveOutputs;

namespace NAudio.Core.Wave.SampleProviders;

/// <summary>
/// Converts an IWaveProvider containing 8 bit PCM to an
/// ISampleProvider
/// </summary>
/// <remarks>
/// Initialises a new instance of Pcm8BitToSampleProvider
/// </remarks>
/// <param name="source">Source wave provider</param>
public class Pcm8BitToSampleProvider( IWaveProvider source ) : SampleProviderConverterBase( source )
{
    /// <summary>
    /// Reads samples from this sample provider
    /// </summary>
    /// <param name="buffer">Sample buffer</param>
    /// <param name="offset">Offset into sample buffer</param>
    /// <param name="count">Number of samples to read</param>
    /// <returns>Number of samples read</returns>
    public override int Read( float [] buffer, int offset, int count )
    {
        var sourceBytesRequired = count;
        EnsureSourceBuffer( sourceBytesRequired );
        var bytesRead           = source.Read(sourceBuffer, 0, sourceBytesRequired);
        var outIndex            = offset;
        for ( var n = 0; n < bytesRead; n++ )
        {
            buffer [ outIndex++ ] = ( sourceBuffer [ n ] / 128f ) - 1.0f;
        }
        return bytesRead;
    }
}
