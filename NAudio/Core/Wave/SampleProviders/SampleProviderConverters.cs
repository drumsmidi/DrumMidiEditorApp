using System;
using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.Wave.WaveOutputs;

namespace NAudio.Core.Wave.SampleProviders;

/// <summary>
/// Utility class for converting to SampleProvider
/// </summary>
internal static class SampleProviderConverters
{
    /// <summary>
    /// Helper function to go from IWaveProvider to a SampleProvider
    /// Must already be PCM or IEEE float
    /// </summary>
    /// <param name="waveProvider">The WaveProvider to convert</param>
    /// <returns>A sample provider</returns>
    public static ISampleProvider ConvertWaveProviderIntoSampleProvider( IWaveProvider waveProvider )
    {
        ISampleProvider sampleProvider;
        if ( waveProvider.WaveFormat.Encoding == WaveFormatEncoding.Pcm )
        {
            // go to float
            if ( waveProvider.WaveFormat.BitsPerSample == 8 )
            {
                sampleProvider = new Pcm8BitToSampleProvider( waveProvider );
            }
            else if ( waveProvider.WaveFormat.BitsPerSample == 16 )
            {
                sampleProvider = new Pcm16BitToSampleProvider( waveProvider );
            }
            else
            {
                sampleProvider = waveProvider.WaveFormat.BitsPerSample == 24
                    ? new Pcm24BitToSampleProvider( waveProvider )
                    : waveProvider.WaveFormat.BitsPerSample == 32
                                    ? (ISampleProvider)new Pcm32BitToSampleProvider( waveProvider )
                                    : throw new InvalidOperationException( "Unsupported bit depth" );
            }
        }
        else
        {
            sampleProvider = waveProvider.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat
                ? waveProvider.WaveFormat.BitsPerSample == 64
                            ? new WaveToSampleProvider64( waveProvider )
                            : new WaveToSampleProvider( waveProvider )
                : throw new ArgumentException( "Unsupported source encoding" );
        }
        return sampleProvider;
    }
}
