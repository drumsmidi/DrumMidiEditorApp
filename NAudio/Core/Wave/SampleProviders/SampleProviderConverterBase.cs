using NAudio.Core.Utils;
using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.Wave.WaveOutputs;

namespace NAudio.Core.Wave.SampleProviders;

/// <summary>
/// Helper base class for classes converting to ISampleProvider
/// </summary>
/// <remarks>
/// Initialises a new instance of SampleProviderConverterBase
/// </remarks>
/// <param name="source">Source Wave provider</param>
public abstract class SampleProviderConverterBase( IWaveProvider source ) : ISampleProvider
{
    /// <summary>
    /// Source Wave Provider
    /// </summary>
    protected IWaveProvider source = source;

    /// <summary>
    /// Source buffer (to avoid constantly creating small buffers during playback)
    /// </summary>
    protected byte[] sourceBuffer;

    /// <summary>
    /// Wave format of this wave provider
    /// </summary>
    public WaveFormat WaveFormat
    {
        get;
    } = WaveFormat.CreateIeeeFloatWaveFormat( source.WaveFormat.SampleRate, source.WaveFormat.Channels );

    /// <summary>
    /// Reads samples from the source wave provider
    /// </summary>
    /// <param name="buffer">Sample buffer</param>
    /// <param name="offset">Offset into sample buffer</param>
    /// <param name="count">Number of samples required</param>
    /// <returns>Number of samples read</returns>
    public abstract int Read( float [] buffer, int offset, int count );

    /// <summary>
    /// Ensure the source buffer exists and is big enough
    /// </summary>
    /// <param name="sourceBytesRequired">Bytes required</param>
    protected void EnsureSourceBuffer( int sourceBytesRequired ) 
        => sourceBuffer = BufferHelpers.Ensure( sourceBuffer, sourceBytesRequired );
}
