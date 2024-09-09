using System;
using System.Text;

// ReSharper disable once CheckNamespace
namespace NAudio.Core.Wave.WaveStreams;

/// <summary>
/// Holds information about a RIFF file chunk
/// </summary>
/// <remarks>
/// Creates a RiffChunk object
/// </remarks>
public class RiffChunk( int length, long streamPosition )
{

    /// <summary>
    /// The chunk length
    /// </summary>
    public int Length
    {
        get; private set;
    } = length;

    /// <summary>
    /// The stream position this chunk is located at
    /// </summary>
    public long StreamPosition
    {
        get; private set;
    } = streamPosition;
}
