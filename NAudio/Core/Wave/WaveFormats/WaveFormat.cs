using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace NAudio.Core.Wave.WaveFormats;

/// <summary>
/// Represents a Wave file format
/// </summary>
[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2 )]
public class WaveFormat
{
    /// <summary>format type</summary>
    protected WaveFormatEncoding waveFormatTag;
    /// <summary>number of channels</summary>
    protected short channels;
    /// <summary>sample rate</summary>
    protected int sampleRate;
    /// <summary>for buffer estimation</summary>
    protected int averageBytesPerSecond;
    /// <summary>block size of data</summary>
    protected short blockAlign;
    /// <summary>number of bits per sample of mono data</summary>
    protected short bitsPerSample;
    /// <summary>number of following bytes</summary>
    protected short extraSize;

    /// <summary>
    /// Creates a new PCM 44.1Khz stereo 16 bit format
    /// </summary>
    public WaveFormat() : this( 44100, 16, 2 )
    {

    }

    /// <summary>
    /// Gets the size of a wave buffer equivalent to the latency in milliseconds.
    /// </summary>
    /// <param name="milliseconds">The milliseconds.</param>
    /// <returns></returns>
    public int ConvertLatencyToByteSize( int milliseconds )
    {
        var bytes = (int) (AverageBytesPerSecond/1000.0*milliseconds);
        if ( bytes % BlockAlign != 0 )
        {
            // Return the upper BlockAligned
            bytes = bytes + BlockAlign - ( bytes % BlockAlign );
        }
        return bytes;
    }

    /// <summary>
    /// Creates a new PCM format with the specified sample rate, bit depth and channels
    /// </summary>
    public WaveFormat( int rate, int bits, int channels )
    {
        if ( channels < 1 )
        {
            throw new ArgumentOutOfRangeException( nameof( channels ), "Channels must be 1 or greater" );
        }
        // minimum 16 bytes, sometimes 18 for PCM
        waveFormatTag           = WaveFormatEncoding.Pcm;
        this.channels           = (short)channels;
        sampleRate              = rate;
        bitsPerSample           = (short)bits;
        extraSize               = 0;

        blockAlign              = (short)( channels * ( bits / 8 ) );
        averageBytesPerSecond   = sampleRate * blockAlign;
    }

    /// <summary>
    /// Creates a new 32 bit IEEE floating point wave format
    /// </summary>
    /// <param name="sampleRate">sample rate</param>
    /// <param name="channels">number of channels</param>
    public static WaveFormat CreateIeeeFloatWaveFormat( int sampleRate, int channels )
    {
        var wf = new WaveFormat
        {
            waveFormatTag   = WaveFormatEncoding.IeeeFloat,
            channels        = (short)channels,
            bitsPerSample   = 32,
            sampleRate      = sampleRate,
            blockAlign      = (short)( 4 * channels )
        };
        wf.averageBytesPerSecond    = sampleRate * wf.blockAlign;
        wf.extraSize                = 0;
        return wf;
    }

    /// <summary>
    /// Helper function to retrieve a WaveFormat structure from a pointer
    /// </summary>
    /// <param name="pointer">WaveFormat structure</param>
    /// <returns></returns>
    public static WaveFormat MarshalFromPtr( nint pointer )
    {
        var waveFormat = Marshal.PtrToStructure<WaveFormat>(pointer);
        switch ( waveFormat.Encoding )
        {
            case WaveFormatEncoding.Pcm:
                // can't rely on extra size even being there for PCM so blank it to avoid reading
                // corrupt data
                waveFormat.extraSize = 0;
                break;
            case WaveFormatEncoding.Extensible:
                waveFormat = Marshal.PtrToStructure<WaveFormatExtensible>( pointer );
                break;
            case WaveFormatEncoding.Adpcm:
                waveFormat = Marshal.PtrToStructure<AdpcmWaveFormat>( pointer );
                break;
            case WaveFormatEncoding.Gsm610:
                waveFormat = Marshal.PtrToStructure<Gsm610WaveFormat>( pointer );
                break;
            default:
                if ( waveFormat.ExtraSize > 0 )
                {
                    waveFormat = Marshal.PtrToStructure<WaveFormatExtraData>( pointer );
                }
                break;
        }
        return waveFormat;
    }

    /// <summary>
    /// Helper function to marshal WaveFormat to an IntPtr
    /// </summary>
    /// <param name="format">WaveFormat</param>
    /// <returns>IntPtr to WaveFormat structure (needs to be freed by callee)</returns>
    public static nint MarshalToPtr( WaveFormat format )
    {
        var formatSize      = Marshal.SizeOf(format);
        var formatPointer   = Marshal.AllocHGlobal(formatSize);
        Marshal.StructureToPtr( format, formatPointer, false );
        return formatPointer;
    }

    /// <summary>
    /// Reads in a WaveFormat (with extra data) from a fmt chunk (chunk identifier and
    /// length should already have been read)
    /// </summary>
    /// <param name="br">Binary reader</param>
    /// <param name="formatChunkLength">Format chunk length</param>
    /// <returns>A WaveFormatExtraData</returns>
    public static WaveFormat FromFormatChunk( BinaryReader br, int formatChunkLength )
    {
        var waveFormat = new WaveFormatExtraData();
        waveFormat.ReadWaveFormat( br, formatChunkLength );
        waveFormat.ReadExtraData( br );
        return waveFormat;
    }

    private void ReadWaveFormat( BinaryReader br, int formatChunkLength )
    {
        if ( formatChunkLength < 16 )
        {
            throw new InvalidDataException( "Invalid WaveFormat Structure" );
        }

        waveFormatTag           = (WaveFormatEncoding)br.ReadUInt16();
        channels                = br.ReadInt16();
        sampleRate              = br.ReadInt32();
        averageBytesPerSecond   = br.ReadInt32();
        blockAlign              = br.ReadInt16();
        bitsPerSample           = br.ReadInt16();
        if ( formatChunkLength > 16 )
        {
            extraSize = br.ReadInt16();
            if ( extraSize != formatChunkLength - 18 )
            {
                Debug.WriteLine( "Format chunk mismatch" );
                extraSize = (short)( formatChunkLength - 18 );
            }
        }
    }

    /// <summary>
    /// Reports this WaveFormat as a string
    /// </summary>
    /// <returns>String describing the wave format</returns>
    public override string ToString()
    {
        return waveFormatTag switch
        {
            WaveFormatEncoding.Pcm or WaveFormatEncoding.Extensible => $"{bitsPerSample} bit PCM: {sampleRate}Hz {channels} channels",// extensible just has some extra bits after the PCM header
            WaveFormatEncoding.IeeeFloat => $"{bitsPerSample} bit IEEFloat: {sampleRate}Hz {channels} channels",
            _ => waveFormatTag.ToString(),
        };
    }

    /// <summary>
    /// Returns the encoding type used
    /// </summary>
    public WaveFormatEncoding Encoding => waveFormatTag;

    /// <summary>
    /// Writes this WaveFormat object to a stream
    /// </summary>
    /// <param name="writer">the output stream</param>
    public virtual void Serialize( BinaryWriter writer )
    {
        writer.Write( 18 + extraSize ); // wave format length
        writer.Write( (short)Encoding );
        writer.Write( (short)Channels );
        writer.Write( SampleRate );
        writer.Write( AverageBytesPerSecond );
        writer.Write( (short)BlockAlign );
        writer.Write( (short)BitsPerSample );
        writer.Write( extraSize );
    }

    /// <summary>
    /// Returns the number of channels (1=mono,2=stereo etc)
    /// </summary>
    public int Channels => channels;

    /// <summary>
    /// Returns the sample rate (samples per second)
    /// </summary>
    public int SampleRate => sampleRate;

    /// <summary>
    /// Returns the average number of bytes used per second
    /// </summary>
    public int AverageBytesPerSecond => averageBytesPerSecond;

    /// <summary>
    /// Returns the block alignment
    /// </summary>
    public virtual int BlockAlign => blockAlign;

    /// <summary>
    /// Returns the number of bits per sample (usually 16 or 32, sometimes 24 or 8)
    /// Can be 0 for some codecs
    /// </summary>
    public int BitsPerSample => bitsPerSample;

    /// <summary>
    /// Returns the number of extra bytes used by this waveformat. Often 0,
    /// except for compressed formats which store extra data after the WAVEFORMATEX header
    /// </summary>
    public int ExtraSize => extraSize;
}
