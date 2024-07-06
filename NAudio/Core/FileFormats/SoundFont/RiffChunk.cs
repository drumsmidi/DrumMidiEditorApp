using System;
using System.IO;
using NAudio.Core.Utils;

namespace NAudio.Core.FileFormats.SoundFont;

internal class RiffChunk
{
    private string chunkID;
    private readonly BinaryReader riffFile;

    public static RiffChunk GetTopLevelChunk( BinaryReader file )
    {
        var r = new RiffChunk(file);
        r.ReadChunk();
        return r;
    }

    private RiffChunk( BinaryReader file )
    {
        riffFile = file;
        chunkID = "????";
        ChunkSize = 0;
        DataOffset = 0;
    }

    /// <summary>
    /// just reads a chunk ID at the current position
    /// </summary>
    /// <returns>chunk ID</returns>
    public string ReadChunkID()
    {
        var cid = riffFile.ReadBytes(4);
        return cid.Length != 4
            ? throw new InvalidDataException( "Couldn't read Chunk ID" )
            : ByteEncoding.Instance.GetString( cid, 0, cid.Length );
    }

    /// <summary>
    /// reads a chunk at the current position
    /// </summary>
    private void ReadChunk()
    {
        chunkID = ReadChunkID();
        ChunkSize = riffFile.ReadUInt32(); //(uint) IPAddress.NetworkToHostOrder(riffFile.ReadUInt32());
        DataOffset = riffFile.BaseStream.Position;
    }

    /// <summary>
    /// creates a new riffchunk from current position checking that we're not
    /// at the end of this chunk first
    /// </summary>
    /// <returns>the new chunk</returns>
    public RiffChunk GetNextSubChunk()
    {
        if ( riffFile.BaseStream.Position + 8 < DataOffset + ChunkSize )
        {
            var chunk = new RiffChunk(riffFile);
            chunk.ReadChunk();
            return chunk;
        }
        //Console.WriteLine("DEBUG Failed to GetNextSubChunk because Position is {0}, dataOffset{1}, chunkSize {2}",riffFile.BaseStream.Position,dataOffset,chunkSize);
        return null;
    }

    public byte [] GetData()
    {
        riffFile.BaseStream.Position = DataOffset;
        var data = riffFile.ReadBytes((int)ChunkSize);
        return data.Length != ChunkSize
            ? throw new InvalidDataException( string.Format( "Couldn't read chunk's data Chunk: {0}, read {1} bytes", this, data.Length ) )
            : data;
    }

    /// <summary>
    /// useful for chunks that just contain a string
    /// </summary>
    /// <returns>chunk as string</returns>
    public string GetDataAsString()
    {
        var data = GetData();
        return data == null ? null : ByteEncoding.Instance.GetString( data, 0, data.Length );
    }

    public T GetDataAsStructure<T>( StructureBuilder<T> s )
    {
        riffFile.BaseStream.Position = DataOffset;
        return s.Length != ChunkSize
            ? throw new InvalidDataException( string.Format( "Chunk size is: {0} so can't read structure of: {1}", ChunkSize, s.Length ) )
            : s.Read( riffFile );
    }

    public T [] GetDataAsStructureArray<T>( StructureBuilder<T> s )
    {
        riffFile.BaseStream.Position = DataOffset;
        if ( ChunkSize % s.Length != 0 )
        {
            throw new InvalidDataException( string.Format( "Chunk size is: {0} not a multiple of structure size: {1}", ChunkSize, s.Length ) );
        }
        var structuresToRead = (int)(ChunkSize / s.Length);
        var a = new T[structuresToRead];
        for ( var n = 0; n < structuresToRead; n++ )
        {
            a [ n ] = s.Read( riffFile );
        }
        return a;
    }

    public string ChunkID
    {
        get => chunkID;
        set
        {
            if ( value == null )
            {
                throw new ArgumentNullException( "ChunkID may not be null" );
            }
            if ( value.Length != 4 )
            {
                throw new ArgumentException( "ChunkID must be four characters" );
            }
            chunkID = value;
        }
    }

    public uint ChunkSize
    {
        get; private set;
    }

    public long DataOffset
    {
        get; private set;
    }

    public override string ToString() => string.Format( "RiffChunk ID: {0} Size: {1} Data Offset: {2}", ChunkID, ChunkSize, DataOffset );

}
