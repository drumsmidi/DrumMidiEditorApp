using System;
using System.IO;
using System.Text;

namespace NAudio.Core.FileFormats.SoundFont;

/// <summary>
/// Class to read the SoundFont file presets chunk
/// </summary>
public class PresetsChunk
{
    private readonly PresetBuilder presetHeaders = new();
    private readonly ZoneBuilder presetZones = new();
    private readonly ModulatorBuilder presetZoneModulators = new();
    private readonly GeneratorBuilder presetZoneGenerators = new();
    private readonly InstrumentBuilder instruments = new();
    private readonly ZoneBuilder instrumentZones = new();
    private readonly ModulatorBuilder instrumentZoneModulators = new();
    private readonly GeneratorBuilder instrumentZoneGenerators = new();
    private readonly SampleHeaderBuilder sampleHeaders = new();

    internal PresetsChunk( RiffChunk chunk )
    {
        var header = chunk.ReadChunkID();
        if ( header != "pdta" )
        {
            throw new InvalidDataException( string.Format( "Not a presets data chunk ({0})", header ) );
        }

        RiffChunk c;
        while ( ( c = chunk.GetNextSubChunk() ) != null )
        {
            switch ( c.ChunkID )
            {
                case "PHDR":
                case "phdr":
                    c.GetDataAsStructureArray( presetHeaders );
                    break;
                case "PBAG":
                case "pbag":
                    c.GetDataAsStructureArray( presetZones );
                    break;
                case "PMOD":
                case "pmod":
                    c.GetDataAsStructureArray( presetZoneModulators );
                    break;
                case "PGEN":
                case "pgen":
                    c.GetDataAsStructureArray( presetZoneGenerators );
                    break;
                case "INST":
                case "inst":
                    c.GetDataAsStructureArray( instruments );
                    break;
                case "IBAG":
                case "ibag":
                    c.GetDataAsStructureArray( instrumentZones );
                    break;
                case "IMOD":
                case "imod":
                    c.GetDataAsStructureArray( instrumentZoneModulators );
                    break;
                case "IGEN":
                case "igen":
                    c.GetDataAsStructureArray( instrumentZoneGenerators );
                    break;
                case "SHDR":
                case "shdr":
                    c.GetDataAsStructureArray( sampleHeaders );
                    break;
                default:
                    throw new InvalidDataException( String.Format( "Unknown chunk type {0}", c.ChunkID ) );
            }
        }

        // now link things up
        instrumentZoneGenerators.Load( sampleHeaders.SampleHeaders );
        instrumentZones.Load( instrumentZoneModulators.Modulators, instrumentZoneGenerators.Generators );
        instruments.LoadZones( instrumentZones.Zones );
        presetZoneGenerators.Load( instruments.Instruments );
        presetZones.Load( presetZoneModulators.Modulators, presetZoneGenerators.Generators );
        presetHeaders.LoadZones( presetZones.Zones );
        sampleHeaders.RemoveEOS();
    }

    /// <summary>
    /// The Presets contained in this chunk
    /// </summary>
    public Preset [] Presets => presetHeaders.Presets;

    /// <summary>
    /// The instruments contained in this chunk
    /// </summary>
    public Instrument [] Instruments => instruments.Instruments;

    /// <summary>
    /// The sample headers contained in this chunk
    /// </summary>
    public SampleHeader [] SampleHeaders => sampleHeaders.SampleHeaders;

    /// <summary>
    /// <see cref="object.ToString"/>
    /// </summary>
    public override string ToString()
    {
        var sb = new StringBuilder();
        _ = sb.Append( "Preset Headers:\r\n" );
        foreach ( var p in presetHeaders.Presets )
        {
            _ = sb.AppendFormat( "{0}\r\n", p );
        }
        _ = sb.Append( "Instruments:\r\n" );
        foreach ( var i in instruments.Instruments )
        {
            _ = sb.AppendFormat( "{0}\r\n", i );
        }
        return sb.ToString();
    }
}