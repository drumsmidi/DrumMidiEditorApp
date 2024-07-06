﻿using System;
using System.IO;
using NAudio.Core.Utils;
using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.Wave.WaveStreams;

namespace NAudio.Core.Wave.WaveOutputs;

/// <summary>
/// This class writes audio data to a .aif file on disk
/// </summary>
public class AiffFileWriter : Stream
{
    private Stream outStream;
    private readonly BinaryWriter writer;
    private long dataSizePos;
    private long commSampleCountPos;
    private long dataChunkSize = 8;

    /// <summary>
    /// Creates an Aiff file by reading all the data from a WaveProvider
    /// BEWARE: the WaveProvider MUST return 0 from its Read method when it is finished,
    /// or the Aiff File will grow indefinitely.
    /// </summary>
    /// <param name="filename">The filename to use</param>
    /// <param name="sourceProvider">The source WaveProvider</param>
    public static void CreateAiffFile( string filename, WaveStream sourceProvider )
    {
        using var writer = new AiffFileWriter( filename, sourceProvider.WaveFormat );
        var buffer = new byte[16384];

        while ( sourceProvider.Position < sourceProvider.Length )
        {
            var count = Math.Min((int)(sourceProvider.Length - sourceProvider.Position), buffer.Length);
            var bytesRead = sourceProvider.Read(buffer, 0, count);

            if ( bytesRead == 0 )
            {
                // end of source provider
                break;
            }

            writer.Write( buffer, 0, bytesRead );
        }
    }

    /// <summary>
    /// AiffFileWriter that actually writes to a stream
    /// </summary>
    /// <param name="outStream">Stream to be written to</param>
    /// <param name="format">Wave format to use</param>
    public AiffFileWriter( Stream outStream, WaveFormat format )
    {
        this.outStream = outStream;
        WaveFormat = format;
        writer = new BinaryWriter( outStream, System.Text.Encoding.UTF8 );
        writer.Write( System.Text.Encoding.UTF8.GetBytes( "FORM" ) );
        writer.Write( 0 ); // placeholder
        writer.Write( System.Text.Encoding.UTF8.GetBytes( "AIFF" ) );

        CreateCommChunk();
        WriteSsndChunkHeader();
    }

    /// <summary>
    /// Creates a new AiffFileWriter
    /// </summary>
    /// <param name="filename">The filename to write to</param>
    /// <param name="format">The Wave Format of the output data</param>
    public AiffFileWriter( string filename, WaveFormat format )
        : this( new FileStream( filename, FileMode.Create, FileAccess.Write, FileShare.Read ), format )
    {
        this.Filename = filename;
    }

    private void WriteSsndChunkHeader()
    {
        writer.Write( System.Text.Encoding.UTF8.GetBytes( "SSND" ) );
        dataSizePos = outStream.Position;
        writer.Write( 0 );  // placeholder
        writer.Write( 0 );  // zero offset
        writer.Write( SwapEndian( WaveFormat.BlockAlign ) );
    }

    private byte [] SwapEndian( short n ) => new byte [] { (byte)( n >> 8 ), (byte)( n & 0xff ) };

    private byte [] SwapEndian( int n ) => new byte [] { (byte)( ( n >> 24 ) & 0xff ), (byte)( ( n >> 16 ) & 0xff ), (byte)( ( n >> 8 ) & 0xff ), (byte)( n & 0xff ), };

    private void CreateCommChunk()
    {
        writer.Write( System.Text.Encoding.UTF8.GetBytes( "COMM" ) );
        writer.Write( SwapEndian( 18 ) );
        writer.Write( SwapEndian( (short)WaveFormat.Channels ) );
        commSampleCountPos = outStream.Position;
        ;
        writer.Write( 0 );  // placeholder for total number of samples
        writer.Write( SwapEndian( (short)WaveFormat.BitsPerSample ) );
        writer.Write( IEEE.ConvertToIeeeExtended( WaveFormat.SampleRate ) );
    }

    /// <summary>
    /// The aiff file name or null if not applicable
    /// </summary>
    public string Filename
    {
        get;
    }

    /// <summary>
    /// Number of bytes of audio in the data chunk
    /// </summary>
    public override long Length => dataChunkSize;

    /// <summary>
    /// WaveFormat of this aiff file
    /// </summary>
    public WaveFormat WaveFormat
    {
        get;
    }

    /// <summary>
    /// Returns false: Cannot read from a AiffFileWriter
    /// </summary>
    public override bool CanRead => false;

    /// <summary>
    /// Returns true: Can write to a AiffFileWriter
    /// </summary>
    public override bool CanWrite => true;

    /// <summary>
    /// Returns false: Cannot seek within a AiffFileWriter
    /// </summary>
    public override bool CanSeek => false;

    /// <summary>
    /// Read is not supported for a AiffFileWriter
    /// </summary>
    public override int Read( byte [] buffer, int offset, int count ) => throw new InvalidOperationException( "Cannot read from an AiffFileWriter" );

    /// <summary>
    /// Seek is not supported for a AiffFileWriter
    /// </summary>
    public override long Seek( long offset, SeekOrigin origin ) => throw new InvalidOperationException( "Cannot seek within an AiffFileWriter" );

    /// <summary>
    /// SetLength is not supported for AiffFileWriter
    /// </summary>
    /// <param name="value"></param>
    public override void SetLength( long value ) => throw new InvalidOperationException( "Cannot set length of an AiffFileWriter" );

    /// <summary>
    /// Gets the Position in the AiffFile (i.e. number of bytes written so far)
    /// </summary>
    public override long Position
    {
        get => dataChunkSize;
        set => throw new InvalidOperationException( "Repositioning an AiffFileWriter is not supported" );
    }

    /// <summary>
    /// Appends bytes to the AiffFile (assumes they are already in the correct format)
    /// </summary>
    /// <param name="data">the buffer containing the wave data</param>
    /// <param name="offset">the offset from which to start writing</param>
    /// <param name="count">the number of bytes to write</param>
    public override void Write( byte [] data, int offset, int count )
    {
        var swappedData = new byte[data.Length];

        var align = WaveFormat.BitsPerSample / 8;

        for ( var i = 0; i < data.Length; i++ )
        {
            var pos = ((int)Math.Floor((double)i / align) * align) + (align - (i % align) - 1);
            swappedData [ i ] = data [ pos ];
        }

        outStream.Write( swappedData, offset, count );
        dataChunkSize += count;
    }

    private readonly byte [] value24 = new byte[3]; // keep this around to save us creating it every time

    /// <summary>
    /// Writes a single sample to the Aiff file
    /// </summary>
    /// <param name="sample">the sample to write (assumed floating point with 1.0f as max value)</param>
    public void WriteSample( float sample )
    {
        if ( WaveFormat.BitsPerSample == 16 )
        {
            writer.Write( SwapEndian( (short)( short.MaxValue * sample ) ) );
            dataChunkSize += 2;
        }
        else if ( WaveFormat.BitsPerSample == 24 )
        {
            var value = BitConverter.GetBytes((int)(int.MaxValue * sample));
            value24 [ 2 ] = value [ 1 ];
            value24 [ 1 ] = value [ 2 ];
            value24 [ 0 ] = value [ 3 ];
            writer.Write( value24 );
            dataChunkSize += 3;
        }
        else if ( WaveFormat.BitsPerSample == 32 && WaveFormat.Encoding == WaveFormatEncoding.Extensible )
        {
            writer.Write( SwapEndian( ushort.MaxValue * (int)sample ) );
            dataChunkSize += 4;
        }
        else
        {
            throw new InvalidOperationException( "Only 16, 24 or 32 bit PCM or IEEE float audio data supported" );
        }
    }

    /// <summary>
    /// Writes 32 bit floating point samples to the Aiff file
    /// They will be converted to the appropriate bit depth depending on the WaveFormat of the AIF file
    /// </summary>
    /// <param name="samples">The buffer containing the floating point samples</param>
    /// <param name="offset">The offset from which to start writing</param>
    /// <param name="count">The number of floating point samples to write</param>
    public void WriteSamples( float [] samples, int offset, int count )
    {
        for ( var n = 0; n < count; n++ )
        {
            WriteSample( samples [ offset + n ] );
        }
    }

    /// <summary>
    /// Writes 16 bit samples to the Aiff file
    /// </summary>
    /// <param name="samples">The buffer containing the 16 bit samples</param>
    /// <param name="offset">The offset from which to start writing</param>
    /// <param name="count">The number of 16 bit samples to write</param>
    public void WriteSamples( short [] samples, int offset, int count )
    {
        // 16 bit PCM data
        if ( WaveFormat.BitsPerSample == 16 )
        {
            for ( var sample = 0; sample < count; sample++ )
            {
                writer.Write( SwapEndian( samples [ sample + offset ] ) );
            }
            dataChunkSize += count * 2;
        }
        // 24 bit PCM data
        else if ( WaveFormat.BitsPerSample == 24 )
        {
            byte[] value;
            for ( var sample = 0; sample < count; sample++ )
            {
                value = BitConverter.GetBytes( ushort.MaxValue * samples [ sample + offset ] );
                value24 [ 2 ] = value [ 1 ];
                value24 [ 1 ] = value [ 2 ];
                value24 [ 0 ] = value [ 3 ];
                writer.Write( value24 );
            }
            dataChunkSize += count * 3;
        }
        // 32 bit PCM data
        else if ( WaveFormat.BitsPerSample == 32 && WaveFormat.Encoding == WaveFormatEncoding.Extensible )
        {
            for ( var sample = 0; sample < count; sample++ )
            {
                writer.Write( SwapEndian( ushort.MaxValue * samples [ sample + offset ] ) );
            }
            dataChunkSize += count * 4;
        }
        else
        {
            throw new InvalidOperationException( "Only 16, 24 or 32 bit PCM audio data supported" );
        }
    }

    /// <summary>
    /// Ensures data is written to disk
    /// </summary>
    public override void Flush() => writer.Flush();

    #region IDisposable Members

    /// <summary>
    /// Actually performs the close,making sure the header contains the correct data
    /// </summary>
    /// <param name="disposing">True if called from <see>Dispose</see></param>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( outStream != null )
            {
                try
                {
                    UpdateHeader( writer );
                }
                finally
                {
                    // in a finally block as we don't want the FileStream to run its disposer in
                    // the GC thread if the code above caused an IOException (e.g. due to disk full)
                    outStream.Dispose(); // will close the underlying base stream
                    outStream = null;
                }
            }
        }
    }

    /// <summary>
    /// Updates the header with file size information
    /// </summary>
    protected virtual void UpdateHeader( BinaryWriter writer )
    {
        Flush();
        _ = writer.Seek( 4, SeekOrigin.Begin );
        writer.Write( SwapEndian( (int)( outStream.Length - 8 ) ) );
        UpdateCommChunk( writer );
        UpdateSsndChunk( writer );
    }

    private void UpdateCommChunk( BinaryWriter writer )
    {
        _ = writer.Seek( (int)commSampleCountPos, SeekOrigin.Begin );
        writer.Write( SwapEndian( (int)( dataChunkSize * 8 / WaveFormat.BitsPerSample / WaveFormat.Channels ) ) );
    }

    private void UpdateSsndChunk( BinaryWriter writer )
    {
        _ = writer.Seek( (int)dataSizePos, SeekOrigin.Begin );
        writer.Write( SwapEndian( (int)dataChunkSize ) );
    }

    /// <summary>
    /// Finaliser - should only be called if the user forgot to close this AiffFileWriter
    /// </summary>
    ~AiffFileWriter()
    {
        System.Diagnostics.Debug.Assert( false, "AiffFileWriter was not disposed" );
        Dispose( false );
    }

    #endregion
}
