using System;

// ReSharper disable once CheckNamespace
namespace NAudio.Core.Wave.WaveOutputs;

/// <summary>
/// Stopped Event Args
/// </summary>
/// <remarks>
/// Initializes a new instance of StoppedEventArgs
/// </remarks>
/// <param name="exception">An exception to report (null if no exception)</param>
public class StoppedEventArgs( Exception exception = null ) : EventArgs
{

    /// <summary>
    /// An exception. Will be null if the playback or record operation stopped due to 
    /// the user requesting stop or reached the end of the input audio
    /// </summary>
    public Exception Exception
    {
        get;
    } = exception;
}
