using System;

namespace NAudio.Core.FileFormats.Mp3;

/// <summary>
/// Represents a Xing VBR header
/// </summary>
public class XingHeader
{
    [Flags]
    private enum XingHeaderOptions
    {
        Frames = 1,
        Bytes = 2,
        Toc = 4,
        VbrScale = 8
    }

    private static int ReadBigEndian( byte [] buffer, int offset )
    {
        int x;
        // big endian extract
        x = buffer [ offset + 0 ];
        x <<= 8;
        x |= buffer [ offset + 1 ];
        x <<= 8;
        x |= buffer [ offset + 2 ];
        x <<= 8;
        x |= buffer [ offset + 3 ];

        return x;
    }

    /// <summary>
    /// Load Xing Header
    /// </summary>
    /// <param name="frame">Frame</param>
    /// <returns>Xing Header</returns>
    public static XingHeader LoadXingHeader( Mp3Frame frame )
    {
        var xingHeader = new XingHeader
        {
            Mp3Frame = frame
        };
        int offset;
        if ( frame.MpegVersion == MpegVersion.Version1 )
        {
            offset = frame.ChannelMode != ChannelMode.Mono ? 32 + 4 : 17 + 4;
        }
        else if ( frame.MpegVersion == MpegVersion.Version2 )
        {
            offset = frame.ChannelMode != ChannelMode.Mono ? 17 + 4 : 9 + 4;
        }
        else
        {
            return null;
            // throw new FormatException("Unsupported MPEG Version");
        }

        if ( frame.RawData [ offset + 0 ] == 'X' &&
             frame.RawData [ offset + 1 ] == 'i' &&
             frame.RawData [ offset + 2 ] == 'n' &&
             frame.RawData [ offset + 3 ] == 'g' )
        {
            offset += 4;
        }
        else if ( frame.RawData [ offset + 0 ] == 'I' &&
                  frame.RawData [ offset + 1 ] == 'n' &&
                  frame.RawData [ offset + 2 ] == 'f' &&
                  frame.RawData [ offset + 3 ] == 'o' )
        {
            offset += 4;
        }
        else
        {
            return null;
        }

        var flags = (XingHeaderOptions)ReadBigEndian(frame.RawData, offset);
        offset += 4;

        if ( ( flags & XingHeaderOptions.Frames ) != 0 )
        {
            offset += 4;
        }
        if ( ( flags & XingHeaderOptions.Bytes ) != 0 )
        {
            offset += 4;
        }
        if ( ( flags & XingHeaderOptions.Toc ) != 0 )
        {
            offset += 100;
        }
        if ( ( flags & XingHeaderOptions.VbrScale ) != 0 )
        {
            xingHeader.VbrScale = ReadBigEndian( frame.RawData, offset );
            //offset += 4;
        }
        return xingHeader;
    }

    /// <summary>
    /// Sees if a frame contains a Xing header
    /// </summary>
    private XingHeader()
    {
    }

    /// <summary>
    /// VBR Scale property
    /// </summary>
    public int VbrScale { get; private set; } = -1;

    /// <summary>
    /// The MP3 frame
    /// </summary>
    public Mp3Frame Mp3Frame
    {
        get;
        private set;
    }
}
