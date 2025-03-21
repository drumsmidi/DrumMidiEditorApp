using NAudio.Core.Wave.SampleProviders;
using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.Wave.WaveOutputs;
using NAudio.Core.Wave.WaveStreams;
using NAudio.WinMM;
using System;
using System.Threading;

namespace NAudio;

/// <summary>
/// AudioFileReader simplifies opening an audio file in NAudio
/// Simply pass in the filename, and it will attempt to open the
/// file and set up a conversion path that turns into PCM IEEE float.
/// ACM codecs will be used for conversion.
/// It provides a volume property and implements both WaveStream and
/// ISampleProvider, making it possibly the only stage in your audio
/// pipeline necessary for simple playback scenarios
/// </summary>
public class AudioFileReader : WaveStream, ISampleProvider
{
    private WaveStream _ReaderStream;

    private readonly SampleChannel _SampleChannel;

    private readonly int _DestBytesPerSample;

    private readonly int _SourceBytesPerSample;

    private readonly long _Length;

    private readonly Lock _LockObj = new();

    /// <summary>
    /// Initializes a new instance of AudioFileReader
    /// </summary>
    /// <param name="fileName">The file to open</param>
    public AudioFileReader( string fileName )
    {
        CreateReaderStream( fileName );

        _SourceBytesPerSample = _ReaderStream.WaveFormat.BitsPerSample / 8 * _ReaderStream.WaveFormat.Channels;

        _SampleChannel = new( _ReaderStream, false );

        _DestBytesPerSample = 4 * _SampleChannel.WaveFormat.Channels;

        _Length = SourceToDest( _ReaderStream.Length );
    }

    /// <summary>
    /// Creates the reader stream, supporting all filetypes in the core NAudio library,
    /// and ensuring we are in PCM format
    /// </summary>
    /// <param name="fileName">File Name</param>
    private void CreateReaderStream( string fileName )
    {
        if ( fileName.EndsWith( ".wav", StringComparison.OrdinalIgnoreCase ) )
        {
            _ReaderStream = new WaveFileReader( fileName );

            if ( _ReaderStream.WaveFormat.Encoding is not WaveFormatEncoding.Pcm and not WaveFormatEncoding.IeeeFloat )
            {
                _ReaderStream = WaveFormatConversionStream.CreatePcmStream( _ReaderStream );
                _ReaderStream = new BlockAlignReductionStream( _ReaderStream );
            }
        }
        else if ( fileName.EndsWith( ".mp3", StringComparison.OrdinalIgnoreCase ) )
        {
            _ReaderStream = new Mp3FileReader( fileName );
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// WaveFormat of this stream
    /// </summary>
    public override WaveFormat WaveFormat => _SampleChannel.WaveFormat;

    /// <summary>
    /// Length of this stream (in bytes)
    /// </summary>
    public override long Length => _Length;

    /// <summary>
    /// Position of this stream (in bytes)
    /// </summary>
    public override long Position
    {
        get => SourceToDest( _ReaderStream.Position );
        set
        {
            lock ( _LockObj )
            {
                _ReaderStream.Position = DestToSource( value );
            }
        }
    }

    /// <summary>
    /// Reads from this wave stream
    /// </summary>
    /// <param name="buffer">Audio buffer</param>
    /// <param name="offset">Offset into buffer</param>
    /// <param name="count">Number of bytes required</param>
    /// <returns>Number of bytes read</returns>
    public override int Read( byte [] buffer, int offset, int count )
    {
        var waveBuffer = new WaveBuffer( buffer );

        var samplesRead = Read( waveBuffer.FloatBuffer, offset / 4, count / 4 );

        return samplesRead * 4;
    }

    /// <summary>
    /// Reads audio from this sample provider
    /// </summary>
    /// <param name="buffer">Sample buffer</param>
    /// <param name="offset">Offset into sample buffer</param>
    /// <param name="count">Number of samples required</param>
    /// <returns>Number of samples read</returns>
    public int Read( float [] buffer, int offset, int count )
    {
        lock ( _LockObj )
        {
            return _SampleChannel.Read( buffer, offset, count );
        }
    }

    /// <summary>
    /// Helper to convert source to dest bytes
    /// </summary>
    private long SourceToDest( long sourceBytes ) 
        => _DestBytesPerSample * ( sourceBytes / _SourceBytesPerSample );

    /// <summary>
    /// Helper to convert dest to source bytes
    /// </summary>
    private long DestToSource( long destBytes ) 
        => _SourceBytesPerSample * ( destBytes / _DestBytesPerSample );

    /// <summary>
    /// Disposes this AudioFileReader
    /// </summary>
    /// <param name="disposing">True if called from Dispose</param>
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            if ( _ReaderStream != null )
            {
                _ReaderStream.Dispose();
                _ReaderStream = null;
            }
        }
        base.Dispose( disposing );
    }
}
