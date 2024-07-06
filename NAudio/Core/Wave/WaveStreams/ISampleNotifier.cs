using System;

// ReSharper disable once CheckNamespace
namespace NAudio.Core.Wave.WaveStreams;

/// <summary>
/// An interface for WaveStreams which can report notification of individual samples
/// </summary>
public interface ISampleNotifier
{
    /// <summary>
    /// A sample has been detected
    /// </summary>
    event EventHandler<SampleEventArgs> Sample;
}

/// <summary>
/// Sample event arguments
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
public class SampleEventArgs( float left, float right ) : EventArgs
{
    /// <summary>
    /// Left sample
    /// </summary>
    public float Left
    {
        get; set;
    } = left;
    /// <summary>
    /// Right sample
    /// </summary>
    public float Right
    {
        get; set;
    } = right;
}
