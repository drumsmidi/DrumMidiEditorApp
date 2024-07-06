using System.IO;

namespace NAudio.Core.FileFormats.SoundFont;

internal class ModulatorBuilder : StructureBuilder<Modulator>
{
    public override Modulator Read( BinaryReader br )
    {
        var m = new Modulator
        {
            SourceModulationData = new ModulatorType( br.ReadUInt16() ),
            DestinationGenerator = (GeneratorEnum)br.ReadUInt16(),
            Amount = br.ReadInt16(),
            SourceModulationAmount = new ModulatorType( br.ReadUInt16() ),
            SourceTransform = (TransformEnum)br.ReadUInt16()
        };
        data.Add( m );
        return m;
    }

    public override void Write( BinaryWriter bw, Modulator o )
    {
        //Zone z = (Zone) o;
        //bw.Write(p.---);
    }

    public override int Length => 10;

    public Modulator [] Modulators => data.ToArray();
}