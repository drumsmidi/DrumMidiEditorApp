using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.Wave.WaveOutputs;

namespace NAudio.Core.Wave.SampleProviders;

/// <summary>
/// Very simple sample provider supporting adjustable gain
/// </summary>
/// <remarks>
/// Initializes a new instance of VolumeSampleProvider
/// </remarks>
/// <param name="source">Source Sample Provider</param>
public class VolumeSampleProvider( ISampleProvider source ) : ISampleProvider
{
    private readonly ISampleProvider source = source;

    /// <summary>
    /// WaveFormat
    /// </summary>
    public WaveFormat WaveFormat => source.WaveFormat;

    /// <summary>
    /// Reads samples from this sample provider
    /// </summary>
    /// <param name="buffer">Sample buffer</param>
    /// <param name="offset">Offset into sample buffer</param>
    /// <param name="sampleCount">Number of samples desired</param>
    /// <returns>Number of samples read</returns>
    public int Read( float [] buffer, int offset, int sampleCount )
    {
        var samplesRead = source.Read(buffer, offset, sampleCount);
        if ( Volume != 1f )
        {
            for ( var n = 0; n < sampleCount; n++ )
            {
                buffer [ offset + n ] *= Volume;
            }
        }
        return samplesRead;
    }

    /// <summary>
    /// Allows adjusting the volume, 1.0f = full volume
    /// </summary>
    public float Volume
    {
        get; set;
    } = 1.0f;
}
