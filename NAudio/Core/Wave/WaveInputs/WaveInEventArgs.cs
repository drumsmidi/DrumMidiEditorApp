using System;

namespace NAudio.Core.Wave.WaveInputs;

/// <summary>
/// Event Args for WaveInStream event
/// </summary>
/// <remarks>
/// Creates new WaveInEventArgs
/// </remarks>
public class WaveInEventArgs( byte [] buffer, int bytes ) : EventArgs
{

    /// <summary>
    /// Buffer containing recorded data. Note that it might not be completely
    /// full. <seealso cref="BytesRecorded"/>
    /// </summary>
    public byte [] Buffer
    {
        get;
    } = buffer;

    /// <summary>
    /// The number of recorded bytes in Buffer. <seealso cref="Buffer"/>
    /// </summary>
    public int BytesRecorded
    {
        get;
    } = bytes;
}
