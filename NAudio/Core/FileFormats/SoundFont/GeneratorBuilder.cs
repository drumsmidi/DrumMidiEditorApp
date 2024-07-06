using System.IO;

namespace NAudio.Core.FileFormats.SoundFont;

internal class GeneratorBuilder : StructureBuilder<Generator>
{
    public override Generator Read( BinaryReader br )
    {
        var g = new Generator
        {
            GeneratorType = (GeneratorEnum)br.ReadUInt16(),
            UInt16Amount = br.ReadUInt16()
        };
        data.Add( g );
        return g;
    }

    public override void Write( BinaryWriter bw, Generator o )
    {
        //Zone z = (Zone) o;
        //bw.Write(p.---);
    }

    public override int Length => 4;

    public Generator [] Generators => data.ToArray();

    public void Load( Instrument [] instruments )
    {
        foreach ( var g in Generators )
        {
            if ( g.GeneratorType == GeneratorEnum.Instrument )
            {
                g.Instrument = instruments [ g.UInt16Amount ];
            }
        }
    }

    public void Load( SampleHeader [] sampleHeaders )
    {
        foreach ( var g in Generators )
        {
            if ( g.GeneratorType == GeneratorEnum.SampleID )
            {
                g.SampleHeader = sampleHeaders [ g.UInt16Amount ];
            }
        }
    }
}