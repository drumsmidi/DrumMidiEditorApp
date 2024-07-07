using System;
using System.IO;

namespace NAudio.Core.FileFormats.Mp3;

/// <summary>
/// An ID3v2 Tag
/// </summary>
public class Id3v2Tag
{
    private readonly long tagStartPosition;
    private readonly long tagEndPosition;

    /// <summary>
    /// Reads an ID3v2 tag from a stream
    /// </summary>
    public static Id3v2Tag ReadTag( Stream input )
    {
        try
        {
            return new Id3v2Tag( input );
        }
        catch ( FormatException )
        {
            return null;
        }
    }

    private Id3v2Tag( Stream input )
    {
        tagStartPosition = input.Position;
        var reader = new BinaryReader(input);
        var headerBytes = reader.ReadBytes(10);
        if ( headerBytes.Length >= 3 &&
             headerBytes [ 0 ] == (byte)'I' &&
             headerBytes [ 1 ] == (byte)'D' &&
             headerBytes [ 2 ] == '3' )
        {

            // http://www.id3.org/develop.html
            // OK found an ID3 tag
            // bytes 3 & 4 are ID3v2 version

            if ( ( headerBytes [ 5 ] & 0x40 ) == 0x40 )
            {
                // extended header present
                var extendedHeader = reader.ReadBytes(4);
                var extendedHeaderLength = extendedHeader[0] * (1 << 21);
                extendedHeaderLength += extendedHeader [ 1 ] * ( 1 << 14 );
                _ = extendedHeader [ 2 ] * ( 1 << 7 );
                _ = extendedHeader [ 3 ];
            }

            // synchsafe
            var dataLength = headerBytes[6] * (1 << 21);
            dataLength += headerBytes [ 7 ] * ( 1 << 14 );
            dataLength += headerBytes [ 8 ] * ( 1 << 7 );
            dataLength += headerBytes [ 9 ];

            _ = reader.ReadBytes( dataLength );

            if ( ( headerBytes [ 5 ] & 0x10 ) == 0x10 )
            {
                // footer present
                _ = reader.ReadBytes( 10 );
            }
        }
        else
        {
            input.Position = tagStartPosition;
            throw new FormatException( "Not an ID3v2 tag" );
        }
        tagEndPosition = input.Position;
        input.Position = tagStartPosition;
        RawData = reader.ReadBytes( (int)( tagEndPosition - tagStartPosition ) );

    }

    /// <summary>
    /// Raw data from this tag
    /// </summary>
    public byte [] RawData
    {
        get;
    }
}
