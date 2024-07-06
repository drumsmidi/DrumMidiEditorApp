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
public class RiffChunk( int identifier, int length, long streamPosition )
{

    /// <summary>
    /// The chunk identifier
    /// </summary>
    public int Identifier
    {
        get;
    } = identifier;

    /// <summary>
    /// The chunk identifier converted to a string
    /// </summary>
    public string IdentifierAsString 
        => Encoding.UTF8.GetString( BitConverter.GetBytes( Identifier ) );

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
