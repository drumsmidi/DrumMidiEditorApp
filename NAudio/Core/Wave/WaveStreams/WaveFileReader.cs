using System;
using System.Collections.Generic;
using System.IO;
using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.FileFormats.Wav;
using System.Threading;

namespace NAudio.Core.Wave.WaveStreams;

/// <summary>This class supports the reading of WAV files,
/// providing a repositionable WaveStream that returns the raw data
/// contained in the WAV file
/// </summary>
public class WaveFileReader : WaveStream
{
    private readonly WaveFormat waveFormat;
    private readonly bool ownInput;
    private readonly long dataPosition;
    private readonly long dataChunkLength;
    private readonly Lock lockObject = new();
    private Stream waveStream;

    /// <summary>Supports opening a WAV file</summary>
    /// <remarks>The WAV file format is a real mess, but we will only
    /// support the basic WAV file format which actually covers the vast
    /// majority of WAV files out there. For more WAV file format information
    /// visit www.wotsit.org. If you have a WAV file that can't be read by
    /// this class, email it to the NAudio project and we will probably
    /// fix this reader to support it
    /// </remarks>
    public WaveFileReader( string waveFile ) :
        this( File.OpenRead( waveFile ), true )
    {
    }

    private WaveFileReader( Stream inputStream, bool ownInput )
    {
        waveStream = inputStream;

        var chunkReader = new WaveFileChunkReader();
        try
        {
            chunkReader.ReadWaveHeader( inputStream );
            waveFormat      = chunkReader.WaveFormat;
            dataPosition    = chunkReader.DataChunkPosition;
            dataChunkLength = chunkReader.DataChunkLength;
            ExtraChunks     = chunkReader.RiffChunks;
        }
        catch
        {
            if ( ownInput )
            {
                inputStream.Dispose();
            }

            throw;
        }

        Position        = 0;
        this.ownInput   = ownInput;
    }

    /// <summary>
    /// Gets a list of the additional chunks found in this file
    /// </summary>
    public List<RiffChunk> ExtraChunks
    {
        get;
    }

    /// <summary>
    /// Cleans up the resources associated with this WaveFileReader
    /// </summary>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            // Release managed resources.
            if ( waveStream != null )
            {
                // only dispose our source if we created it
                if ( ownInput )
                {
                    waveStream.Dispose();
                }
                waveStream = null;
            }
        }
        else
        {
            System.Diagnostics.Debug.Assert( false, "WaveFileReader was not disposed" );
        }
        // Release unmanaged resources.
        // Set large fields to null.
        // Call Dispose on your base class.
        base.Dispose( disposing );
    }

    /// <summary>
    /// <see cref="WaveStream.WaveFormat"/>
    /// </summary>
    public override WaveFormat WaveFormat => waveFormat;

    /// <summary>
    /// This is the length of audio data contained in this WAV file, in bytes
    /// (i.e. the byte length of the data chunk, not the length of the WAV file itself)
    /// <see cref="WaveStream.WaveFormat"/>
    /// </summary>
    public override long Length => dataChunkLength;

    /// <summary>
    /// Position in the WAV data chunk.
    /// <see cref="Stream.Position"/>
    /// </summary>
    public override long Position
    {
        get => waveStream.Position - dataPosition;
        set
        {
            lock ( lockObject )
            {
                value = Math.Min( value, Length );
                // make sure we don't get out of sync
                value -= value % waveFormat.BlockAlign;
                waveStream.Position = value + dataPosition;
            }
        }
    }

    /// <summary>
    /// Reads bytes from the Wave File
    /// <see cref="Stream.Read"/>
    /// </summary>
    public override int Read( byte [] array, int offset, int count )
    {
        if ( count % waveFormat.BlockAlign != 0 )
        {
            throw new ArgumentException(
                $"Must read complete blocks: requested {count}, block align is {WaveFormat.BlockAlign}" );
        }
        lock ( lockObject )
        {
            // sometimes there is more junk at the end of the file past the data chunk
            if ( Position + count > dataChunkLength )
            {
                count = (int)( dataChunkLength - Position );
            }
            return waveStream.Read( array, offset, count );
        }
    }
}
