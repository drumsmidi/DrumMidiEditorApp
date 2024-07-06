using NAudio.Core.Wave.SampleProviders;
using NAudio.Core.Wave.WaveOutputs;

namespace NAudio.Core.Wave;

/// <summary>
/// Useful extension methods to make switching between WaveAndSampleProvider easier
/// </summary>
public static class WaveExtensionMethods
{
    /// <summary>
    /// Allows sending a SampleProvider directly to an IWavePlayer without needing to convert
    /// back to an IWaveProvider
    /// </summary>
    /// <param name="wavePlayer">The WavePlayer</param>
    /// <param name="sampleProvider"></param>
    public static void Init( this IWavePlayer wavePlayer, ISampleProvider sampleProvider )
    {
        var provider = new SampleToWaveProvider(sampleProvider);
        wavePlayer.Init( provider );
    }
}
