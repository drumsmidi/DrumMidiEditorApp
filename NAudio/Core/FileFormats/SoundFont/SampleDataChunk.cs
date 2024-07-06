using System.IO;

namespace NAudio.Core.FileFormats.SoundFont;

internal class SampleDataChunk
{
    public SampleDataChunk( RiffChunk chunk )
    {
        var header = chunk.ReadChunkID();
        if ( header != "sdta" )
        {
            throw new InvalidDataException( $"Not a sample data chunk ({header})" );
        }
        SampleData = chunk.GetData();
    }

    public byte [] SampleData
    {
        get; private set;
    }
}