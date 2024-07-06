using System;
using System.Runtime.InteropServices;
using NAudio.Core;
using NAudio.Core.Wave.WaveFormats;

namespace NAudio.WinMM.Compression;

/// <summary>
/// AcmStream encapsulates an Audio Compression Manager Stream
/// used to convert audio from one format to another
/// </summary>
public class AcmStream : IDisposable
{
    private nint streamHandle;
    private nint driverHandle;
    private AcmStreamHeader streamHeader;
    private readonly WaveFormat sourceFormat;

    /// <summary>
    /// Creates a new ACM stream to convert one format to another. Note that
    /// not all conversions can be done in one step
    /// </summary>
    /// <param name="sourceFormat">The source audio format</param>
    /// <param name="destFormat">The destination audio format</param>
    public AcmStream( WaveFormat sourceFormat, WaveFormat destFormat )
    {
        try
        {
            streamHandle = nint.Zero;
            this.sourceFormat = sourceFormat;
            var sourceBufferSize = Math.Max(65536, sourceFormat.AverageBytesPerSecond);
            sourceBufferSize -= sourceBufferSize % sourceFormat.BlockAlign;
            var sourceFormatPointer = WaveFormat.MarshalToPtr(sourceFormat);
            var destFormatPointer = WaveFormat.MarshalToPtr(destFormat);

            try
            {
                MmException.Try( AcmInterop.acmStreamOpen2( out streamHandle, nint.Zero, sourceFormatPointer, destFormatPointer, null, nint.Zero, nint.Zero, AcmStreamOpenFlags.NonRealTime ), "acmStreamOpen" );
            }
            finally
            {
                Marshal.FreeHGlobal( sourceFormatPointer );
                Marshal.FreeHGlobal( destFormatPointer );

            }

            var destBufferSize = SourceToDest(sourceBufferSize);
            streamHeader = new AcmStreamHeader( streamHandle, sourceBufferSize, destBufferSize );
            driverHandle = nint.Zero;
        }
        catch
        {
            // suppress the finalise and clean up resources
            Dispose();
            throw;
        }
    }

    /// <summary>
    /// Creates a new ACM stream to convert one format to another, using a 
    /// specified driver identifier and wave filter
    /// </summary>
    /// <param name="driverId">the driver identifier</param>
    /// <param name="sourceFormat">the source format</param>
    /// <param name="waveFilter">the wave filter</param>
    public AcmStream( nint driverId, WaveFormat sourceFormat, WaveFilter waveFilter )
    {
        var sourceBufferSize = Math.Max(16384, sourceFormat.AverageBytesPerSecond);
        this.sourceFormat = sourceFormat;
        sourceBufferSize -= sourceBufferSize % sourceFormat.BlockAlign;
        MmException.Try( AcmInterop.acmDriverOpen( out driverHandle, driverId, 0 ), "acmDriverOpen" );

        var sourceFormatPointer = WaveFormat.MarshalToPtr(sourceFormat);
        try
        {
            MmException.Try( AcmInterop.acmStreamOpen2( out streamHandle, driverHandle,
                sourceFormatPointer, sourceFormatPointer, waveFilter, nint.Zero, nint.Zero, AcmStreamOpenFlags.NonRealTime ), "acmStreamOpen" );
        }
        finally
        {
            Marshal.FreeHGlobal( sourceFormatPointer );
        }
        streamHeader = new AcmStreamHeader( streamHandle, sourceBufferSize, SourceToDest( sourceBufferSize ) );
    }


    /// <summary>
    /// Returns the number of output bytes for a given number of input bytes
    /// </summary>
    /// <param name="source">Number of input bytes</param>
    /// <returns>Number of output bytes</returns>
    public int SourceToDest( int source )
    {
        if ( source == 0 ) // zero is an invalid parameter to acmStreamSize
        {
            return 0;
        }

        int convertedBytes;
        var mmResult = AcmInterop.acmStreamSize(streamHandle, source, out convertedBytes, AcmStreamSizeFlags.Source);
        MmException.Try( mmResult, "acmStreamSize" );
        return convertedBytes;
    }

    /// <summary>
    /// Returns the number of source bytes for a given number of destination bytes
    /// </summary>
    /// <param name="dest">Number of destination bytes</param>
    /// <returns>Number of source bytes</returns>
    public int DestToSource( int dest )
    {
        if ( dest == 0 ) // zero is an invalid parameter to acmStreamSize
        {
            return 0;
        }

        int convertedBytes;
        MmException.Try( AcmInterop.acmStreamSize( streamHandle, dest, out convertedBytes, AcmStreamSizeFlags.Destination ), "acmStreamSize" );
        return convertedBytes;
    }

    /// <summary>
    /// Suggests an appropriate PCM format that the compressed format can be converted
    /// to in one step
    /// </summary>
    /// <param name="compressedFormat">The compressed format</param>
    /// <returns>The PCM format</returns>
    public static WaveFormat SuggestPcmFormat( WaveFormat compressedFormat )
    {
        // create a PCM format
        var suggestedFormat = new WaveFormat(compressedFormat.SampleRate, 16, compressedFormat.Channels);
        //MmException.Try(AcmInterop.acmFormatSuggest(IntPtr.Zero, compressedFormat, suggestedFormat, Marshal.SizeOf(suggestedFormat), AcmFormatSuggestFlags.FormatTag), "acmFormatSuggest");


        var suggestedFormatPointer = WaveFormat.MarshalToPtr(suggestedFormat);
        var compressedFormatPointer = WaveFormat.MarshalToPtr(compressedFormat);
        try
        {
            var result = AcmInterop.acmFormatSuggest2(nint.Zero, compressedFormatPointer,
                suggestedFormatPointer, Marshal.SizeOf(suggestedFormat), AcmFormatSuggestFlags.FormatTag);
            suggestedFormat = WaveFormat.MarshalFromPtr( suggestedFormatPointer );
            MmException.Try( result, "acmFormatSuggest" );
        }
        finally
        {
            Marshal.FreeHGlobal( suggestedFormatPointer );
            Marshal.FreeHGlobal( compressedFormatPointer );
        }
        return suggestedFormat;
    }

    /// <summary>
    /// Returns the Source Buffer. Fill this with data prior to calling convert
    /// </summary>
    public byte [] SourceBuffer => streamHeader.SourceBuffer;

    /// <summary>
    /// Returns the Destination buffer. This will contain the converted data
    /// after a successful call to Convert
    /// </summary>
    public byte [] DestBuffer => streamHeader.DestBuffer;

    /// <summary>
    /// Report that we have repositioned in the source stream
    /// </summary>
    public void Reposition() => streamHeader.Reposition();

    /// <summary>
    /// Converts the contents of the SourceBuffer into the DestinationBuffer
    /// </summary>
    /// <param name="bytesToConvert">The number of bytes in the SourceBuffer
    /// that need to be converted</param>
    /// <param name="sourceBytesConverted">The number of source bytes actually converted</param>
    /// <returns>The number of converted bytes in the DestinationBuffer</returns>
    public int Convert( int bytesToConvert, out int sourceBytesConverted )
    {
        if ( bytesToConvert % sourceFormat.BlockAlign != 0 )
        {
            System.Diagnostics.Debug.WriteLine( string.Format( "Not a whole number of blocks: {0} ({1})", bytesToConvert, sourceFormat.BlockAlign ) );
            bytesToConvert -= bytesToConvert % sourceFormat.BlockAlign;
        }

        return streamHeader == null
            ? throw new ObjectDisposedException( "AcmStream has already been disposed" )
            : streamHeader.Convert( bytesToConvert, out sourceBytesConverted );
    }

    /// <summary>
    /// Converts the contents of the SourceBuffer into the DestinationBuffer
    /// </summary>
    /// <param name="bytesToConvert">The number of bytes in the SourceBuffer
    /// that need to be converted</param>
    /// <returns>The number of converted bytes in the DestinationBuffer</returns>
    [Obsolete( "Call the version returning sourceBytesConverted instead" )]
    public int Convert( int bytesToConvert )
    {
        int sourceBytesConverted;
        var destBytes = Convert(bytesToConvert, out sourceBytesConverted);
        return sourceBytesConverted != bytesToConvert
            ? throw new MmException( MmResult.NotSupported, "AcmStreamHeader.Convert didn't convert everything" )
            : destBytes;
    }

    /* Relevant only for async conversion streams
    public void Reset()
    {
        MmException.Try(AcmInterop.acmStreamReset(streamHandle,0),"acmStreamReset");
    }
    */

    #region IDisposable Members

    /// <summary>
    /// Frees resources associated with this ACM Stream
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// Frees resources associated with this ACM Stream
    /// </summary>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            // Free other state (managed objects).
            if ( streamHeader != null )
            {
                streamHeader.Dispose();
                streamHeader = null;
            }
        }

        // Free your own state (unmanaged objects).

        if ( streamHandle != nint.Zero )
        {
            var result = AcmInterop.acmStreamClose(streamHandle, 0);
            streamHandle = nint.Zero;
            if ( result != MmResult.NoError )
            {
                throw new MmException( result, "acmStreamClose" );
            }

        }
        // Set large fields to null.
        if ( driverHandle != nint.Zero )
        {
            _ = AcmInterop.acmDriverClose( driverHandle, 0 );
            driverHandle = nint.Zero;
        }
    }

    /// <summary>
    /// Frees resources associated with this ACM Stream
    /// </summary>
    ~AcmStream()
    {
        // Simply call Dispose(false).
        System.Diagnostics.Debug.Assert( false, "AcmStream Dispose was not called" );
        Dispose( false );
    }

    #endregion
}
