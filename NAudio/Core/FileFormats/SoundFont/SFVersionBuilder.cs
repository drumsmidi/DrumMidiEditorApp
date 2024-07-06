using System.IO;

namespace NAudio.Core.FileFormats.SoundFont;

/// <summary>
/// Builds a SoundFont version
/// </summary>
internal class SFVersionBuilder : StructureBuilder<SFVersion>
{
    /// <summary>
    /// Reads a SoundFont Version structure
    /// </summary>
    public override SFVersion Read( BinaryReader br )
    {
        var v = new SFVersion
        {
            Major = br.ReadInt16(),
            Minor = br.ReadInt16()
        };
        data.Add( v );
        return v;
    }

    /// <summary>
    /// Writes a SoundFont Version structure
    /// </summary>
    public override void Write( BinaryWriter bw, SFVersion v )
    {
        bw.Write( v.Major );
        bw.Write( v.Minor );
    }

    /// <summary>
    /// Gets the length of this structure
    /// </summary>
    public override int Length => 4;
}