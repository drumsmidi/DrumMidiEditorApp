using System.IO;
using NAudio.Core.FileFormats.Mp3;
using NAudio.Core.Wave.WaveFormats;
using NAudio.WinMM;

namespace NAudio.Core.Wave.WaveStreams;

/// <summary>
/// Class for reading from MP3 files
/// </summary>
/// <remarks>Supports opening a MP3 file</remarks>
public class Mp3FileReader( string mp3FileName ) : Mp3FileReaderBase( File.OpenRead( mp3FileName ), CreateAcmFrameDecompressor, true )
{

    /// <summary>
    /// Creates an ACM MP3 Frame decompressor. This is the default with NAudio
    /// </summary>
    /// <param name="mp3Format">A WaveFormat object based </param>
    /// <returns></returns>
    public static IMp3FrameDecompressor CreateAcmFrameDecompressor( WaveFormat mp3Format ) =>
        new AcmMp3FrameDecompressor( mp3Format );
}
