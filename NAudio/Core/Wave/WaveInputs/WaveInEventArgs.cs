using System;

namespace NAudio.Core.Wave.WaveInputs;

/// <summary>
/// Event Args for WaveInStream event
/// </summary>
public class WaveInEventArgs : EventArgs
{

    /// <summary>
    /// Creates new WaveInEventArgs
    /// </summary>
    public WaveInEventArgs( byte [] buffer, int bytes )
    {
        Buffer = buffer;
        this.BytesRecorded = bytes;
    }

    /// <summary>
    /// Buffer containing recorded data. Note that it might not be completely
    /// full. <seealso cref="BytesRecorded"/>
    /// </summary>
    public byte [] Buffer
    {
        get;
    }

    /// <summary>
    /// The number of recorded bytes in Buffer. <seealso cref="Buffer"/>
    /// </summary>
    public int BytesRecorded
    {
        get;
    }
}
