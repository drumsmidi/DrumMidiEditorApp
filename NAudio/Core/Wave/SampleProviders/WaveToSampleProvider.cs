﻿using System;
using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.Wave.WaveOutputs;

namespace NAudio.Core.Wave.SampleProviders;

/// <summary>
/// Helper class turning an already 32 bit floating point IWaveProvider
/// into an ISampleProvider - hopefully not needed for most applications
/// </summary>
public class WaveToSampleProvider : SampleProviderConverterBase
{
    /// <summary>
    /// Initializes a new instance of the WaveToSampleProvider class
    /// </summary>
    /// <param name="source">Source wave provider, must be IEEE float</param>
    public WaveToSampleProvider( IWaveProvider source )
        : base( source )
    {
        if ( source.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat )
        {
            throw new ArgumentException( "Must be already floating point" );
        }
    }

    /// <summary>
    /// Reads from this provider
    /// </summary>
    public override int Read( float [] buffer, int offset, int count )
    {
        var bytesNeeded = count * 4;
        EnsureSourceBuffer( bytesNeeded );
        var bytesRead   = source.Read(sourceBuffer, 0, bytesNeeded);
        var samplesRead = bytesRead / 4;
        var outputIndex = offset;
        unsafe
        {
            fixed ( byte* pBytes = &sourceBuffer [ 0 ] )
            {
                var pFloat = (float*)pBytes;
                for ( int n = 0, i = 0; n < bytesRead; n += 4, i++ )
                {
                    buffer [ outputIndex++ ] = *( pFloat + i );
                }
            }
        }
        return samplesRead;
    }
}
