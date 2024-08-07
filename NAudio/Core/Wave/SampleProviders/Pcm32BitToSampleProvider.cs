﻿using NAudio.Core.Wave.WaveOutputs;

namespace NAudio.Core.Wave.SampleProviders;

/// <summary>
/// Converts an IWaveProvider containing 32 bit PCM to an
/// ISampleProvider
/// </summary>
/// <remarks>
/// Initialises a new instance of Pcm32BitToSampleProvider
/// </remarks>
/// <param name="source">Source Wave Provider</param>
public class Pcm32BitToSampleProvider( IWaveProvider source ) : SampleProviderConverterBase( source )
{
    /// <summary>
    /// Reads floating point samples from this sample provider
    /// </summary>
    /// <param name="buffer">sample buffer</param>
    /// <param name="offset">offset within sample buffer to write to</param>
    /// <param name="count">number of samples required</param>
    /// <returns>number of samples provided</returns>
    public override int Read( float [] buffer, int offset, int count )
    {
        var sourceBytesRequired = count*4;
        EnsureSourceBuffer( sourceBytesRequired );
        var bytesRead           = source.Read(sourceBuffer, 0, sourceBytesRequired);
        var outIndex            = offset;
        for ( var n = 0; n < bytesRead; n += 4 )
        {
            buffer [ outIndex++ ] = ( ( (sbyte)sourceBuffer [ n + 3 ] << 24 ) |
                                    ( sourceBuffer [ n + 2 ] << 16 ) |
                                    ( sourceBuffer [ n + 1 ] << 8 ) |
                                      sourceBuffer [ n ] ) / 2147483648f;
        }
        return bytesRead / 4;
    }
}
