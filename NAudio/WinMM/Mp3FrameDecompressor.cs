﻿using System;
using NAudio.Core.FileFormats.Mp3;
using NAudio.Core.Wave.WaveFormats;
using NAudio.WinMM.Compression;

namespace NAudio.WinMM;

/// <summary>
/// MP3 Frame Decompressor using ACM
/// </summary>
public partial class AcmMp3FrameDecompressor : IMp3FrameDecompressor
{
    private readonly AcmStream conversionStream;
    private bool disposed;

    /// <summary>
    /// Creates a new ACM frame decompressor
    /// </summary>
    /// <param name="sourceFormat">The MP3 source format</param>
    public AcmMp3FrameDecompressor( WaveFormat sourceFormat )
    {
        OutputFormat = AcmStream.SuggestPcmFormat( sourceFormat );
        try
        {
            conversionStream = new AcmStream( sourceFormat, OutputFormat );
        }
        catch ( Exception )
        {
            disposed = true;
            GC.SuppressFinalize( this );
            throw;
        }
    }

    /// <summary>
    /// Output format (PCM)
    /// </summary>
    public WaveFormat OutputFormat
    {
        get;
    }

    /// <summary>
    /// Decompresses a frame
    /// </summary>
    /// <param name="frame">The MP3 frame</param>
    /// <param name="dest">destination buffer</param>
    /// <param name="destOffset">Offset within destination buffer</param>
    /// <returns>Bytes written into destination buffer</returns>
    public int DecompressFrame( Mp3Frame frame, byte [] dest, int destOffset )
    {
        if ( frame == null )
        {
            throw new ArgumentNullException( nameof( frame ), "You must provide a non-null Mp3Frame to decompress" );
        }
        Array.Copy( frame.RawData, conversionStream.SourceBuffer, frame.FrameLength );
        var converted = conversionStream.Convert(frame.FrameLength, out var sourceBytesConverted);
        if ( sourceBytesConverted != frame.FrameLength )
        {
            throw new InvalidOperationException( string.Format( "Couldn't convert the whole MP3 frame (converted {0}/{1})",
                sourceBytesConverted, frame.FrameLength ) );
        }
        Array.Copy( conversionStream.DestBuffer, 0, dest, destOffset, converted );
        return converted;
    }

    /// <summary>
    /// Resets the MP3 Frame Decompressor after a reposition operation
    /// </summary>
    public void Reset() => conversionStream.Reposition();

    /// <summary>
    /// Disposes of this MP3 frame decompressor
    /// </summary>
    public void Dispose()
    {
        if ( !disposed )
        {
            disposed = true;
            conversionStream?.Dispose();
            GC.SuppressFinalize( this );
        }
    }

    /// <summary>
    /// Finalizer ensuring that resources get released properly
    /// </summary>
    ~AcmMp3FrameDecompressor()
    {
        System.Diagnostics.Debug.Assert( false, "AcmMp3FrameDecompressor Dispose was not called" );
        Dispose();
    }
}
