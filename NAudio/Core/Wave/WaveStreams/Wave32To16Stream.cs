using System;
using NAudio.Core.Utils;
using NAudio.Core.Wave.WaveFormats;


// ReSharper disable once CheckNamespace
namespace NAudio.Core.Wave.WaveStreams;

/// <summary>
/// WaveStream that converts 32 bit audio back down to 16 bit, clipping if necessary
/// </summary>
public class Wave32To16Stream : WaveStream
{
    private WaveStream sourceStream;
    private readonly WaveFormat waveFormat;
    private readonly long length;
    private long position;
    private readonly object lockObject = new();

    /// <summary>
    /// The <see cref="Read"/> method reuses the same buffer to prevent
    /// unnecessary allocations.
    /// </summary>
    private byte[] sourceBuffer;

    /// <summary>
    /// Creates a new Wave32To16Stream
    /// </summary>
    /// <param name="sourceStream">the source stream</param>
    public Wave32To16Stream( WaveStream sourceStream )
    {
        if ( sourceStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat )
        {
            throw new ArgumentException( "Only 32 bit Floating point supported" );
        }

        if ( sourceStream.WaveFormat.BitsPerSample != 32 )
        {
            throw new ArgumentException( "Only 32 bit Floating point supported" );
        }

        waveFormat = new WaveFormat( sourceStream.WaveFormat.SampleRate, 16, sourceStream.WaveFormat.Channels );
        Volume = 1.0f;
        this.sourceStream = sourceStream;
        length = sourceStream.Length / 2;
        position = sourceStream.Position / 2;
    }

    /// <summary>
    /// Sets the volume for this stream. 1.0f is full scale
    /// </summary>
    public float Volume
    {
        get;
        set;
    }

    /// <summary>
    /// <see cref="WaveStream.BlockAlign"/>
    /// </summary>
    public override int BlockAlign => sourceStream.BlockAlign / 2;


    /// <summary>
    /// Returns the stream length
    /// </summary>
    public override long Length => length;

    /// <summary>
    /// Gets or sets the current position in the stream
    /// </summary>
    public override long Position
    {
        get => position;
        set
        {
            lock ( lockObject )
            {
                // make sure we don't get out of sync
                value -= value % BlockAlign;
                sourceStream.Position = value * 2;
                position = value;
            }
        }
    }

    /// <summary>
    /// Reads bytes from this wave stream
    /// </summary>
    /// <param name="destBuffer">Destination buffer</param>
    /// <param name="offset">Offset into destination buffer</param>
    /// <param name="numBytes"></param>
    /// <returns>Number of bytes read.</returns>
    public override int Read( byte [] destBuffer, int offset, int numBytes )
    {
        lock ( lockObject )
        {
            var count = numBytes*2;
            sourceBuffer = BufferHelpers.Ensure( sourceBuffer, count );
            var bytesRead = sourceStream.Read(sourceBuffer, 0, count);
            Convert32To16( destBuffer, offset, sourceBuffer, bytesRead );
            position += bytesRead / 2;
            return bytesRead / 2;
        }
    }

    /// <summary>
    /// Conversion to 16 bit and clipping
    /// </summary>
    private unsafe void Convert32To16( byte [] destBuffer, int offset, byte [] source, int bytesRead )
    {
        fixed ( byte* pDestBuffer = &destBuffer [ offset ],
            pSourceBuffer = &source [ 0 ] )
        {
            var psDestBuffer = (short*)pDestBuffer;
            var pfSourceBuffer = (float*)pSourceBuffer;

            var samplesRead = bytesRead / 4;
            for ( var n = 0; n < samplesRead; n++ )
            {
                var sampleVal = pfSourceBuffer[n] * Volume;
                if ( sampleVal > 1.0f )
                {
                    psDestBuffer [ n ] = short.MaxValue;
                    Clip = true;
                }
                else if ( sampleVal < -1.0f )
                {
                    psDestBuffer [ n ] = short.MinValue;
                    Clip = true;
                }
                else
                {
                    psDestBuffer [ n ] = (short)( sampleVal * 32767 );
                }
            }
        }
    }

    /// <summary>
    /// <see cref="WaveStream.WaveFormat"/>
    /// </summary>
    public override WaveFormat WaveFormat => waveFormat;

    /// <summary>
    /// Clip indicator. Can be reset.
    /// </summary>
    public bool Clip
    {
        get;
        set;
    }

    /// <summary>
    /// Disposes this WaveStream
    /// </summary>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( sourceStream != null )
            {
                sourceStream.Dispose();
                sourceStream = null;
            }
        }
        base.Dispose( disposing );
    }
}
