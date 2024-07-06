using System;
using NAudio.Core.Wave.WaveOutputs;

namespace NAudio.Core.Wave.SampleProviders;

/// <summary>
/// Converts an IWaveProvider containing 16 bit PCM to an
/// ISampleProvider
/// </summary>
/// <remarks>
/// Initialises a new instance of Pcm16BitToSampleProvider
/// </remarks>
/// <param name="source">Source wave provider</param>
public class Pcm16BitToSampleProvider( IWaveProvider source ) : SampleProviderConverterBase( source )
{
    /// <summary>
    /// Reads samples from this sample provider
    /// </summary>
    /// <param name="buffer">Sample buffer</param>
    /// <param name="offset">Offset into sample buffer</param>
    /// <param name="count">Samples required</param>
    /// <returns>Number of samples read</returns>
    public override int Read( float [] buffer, int offset, int count )
    {
        var sourceBytesRequired = count * 2;
        EnsureSourceBuffer( sourceBytesRequired );
        var bytesRead           = source.Read(sourceBuffer, 0, sourceBytesRequired);
        var outIndex            = offset;
        for ( var n = 0; n < bytesRead; n += 2 )
        {
            buffer [ outIndex++ ] = BitConverter.ToInt16( sourceBuffer, n ) / 32768f;
        }
        return bytesRead / 2;
    }
}
