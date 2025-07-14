using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pIO.pScore.pDtxMania;

/// <summary>
/// Score入出力：DtxMania（非推奨）
/// </summary>
internal partial class ScoreStream : IScoreReader, IScoreWriter
{
    /// <summary>
    /// DTXのレーン情報
    /// </summary>
    private readonly Dictionary<string, MidiMapGroup> _DtxLaneDic = new()
    {
        { "1A", new() { ScaleKey = "CY", GroupName = "1A:LeftCymbal"       } },
        { "11", new() { ScaleKey = "HH", GroupName = "11:HiHatClose"       } },
        { "18", new() { ScaleKey = "HH", GroupName = "18:HiHatOpen"        } },
        { "1B", new() { ScaleKey = "HH", GroupName = "1B:HiHatPedal"       } },
        { "1C", new() { ScaleKey = "BD", GroupName = "1C:LeftBaseDrum"     } },
        { "12", new() { ScaleKey = "SD", GroupName = "12:Snare"            } },
        { "13", new() { ScaleKey = "BD", GroupName = "13:RightBassDrum"    } },
        { "14", new() { ScaleKey = "TM", GroupName = "14:HighTom"          } },
        { "15", new() { ScaleKey = "TM", GroupName = "15:LowTom"           } },
        { "17", new() { ScaleKey = "TM", GroupName = "17:FloorTom"         } },
        { "16", new() { ScaleKey = "CY", GroupName = "16:RightCymbal"      } },
        { "19", new() { ScaleKey = "RD", GroupName = "19:RideCymbal"       } },
    };

    #region Reader

    public bool Validation( GeneralPath aGeneralPath )
    {
        return true;
    }

    public void Read( GeneralPath aGeneralPath, out Score aScore )
    {
        using var _ = new LogBlock( $"{Log.GetThisMethodName}:load dtx file" );

        aScore = new();

        using var reader = new StreamReader( aGeneralPath.AbsoluteFilePath , Encoding.GetEncoding("UTF-8") );

        var midiMapSet = aScore.Channels[ MidiNet.ChannelDrum ].MidiMapSet;

        foreach ( var item in _DtxLaneDic )
        {
            var group = item.Value.Clone();
            group.GroupKey = midiMapSet.GetMidiMapGroupNewKey();

            var midiMap = new MidiMap
            {
                MidiMapKey  = midiMapSet.GetMidiMapNewKey(),
                MidiMapName = "Dummy",
            };
            group.AddMidiMap( midiMap );
            midiMapSet.AddMidiMapGroup( group );
            midiMapSet.UpdateInfo();
        }

        midiMapSet.UpdateInfo();

        var wavList = new Dictionary<string, string>();
        var volList = new Dictionary<string, int>();
        var bpmList = new Dictionary<string, double>();

        string? line;

        while ( ( line = reader.ReadLine() ) != null )
        {
            var items = line.Split( [':', ';'] );

            if ( items.Length < 2 )
            {
                continue;
            }

            #region WAVzz
            if ( RegexWAV().Match( line ).Success )
            {
                var zz      = items[ 0 ].Substring( 4, 2 );
                var name    = items[ 1 ].Trim();

                wavList [ zz ] = name;

                continue;
            }
            #endregion
            #region VOLUMEzz
            else if ( RegexVOLUME().Match( line ).Success )
            {
                var zz      = items[ 0 ].Substring( 7, 2 );
                var volume  = Convert.ToInt32( items[ 1 ].Trim() );

                volList [ zz ] = volume;

                continue;
            }
            #endregion
            #region BPMzz
            else if ( RegexBASEBPM().Match( line ).Success )
            {
                var zz  = items[ 0 ].Substring( 4, 2 );
                var bpm = Convert.ToDouble( items[ 1 ].Trim() );

                bpmList [ zz ] = bpm;

                continue;
            }
            #endregion
            #region BPMTOP
            else if ( RegexBPMTOP().Match( line ).Success )
            {
                var bpm = Convert.ToDouble( items[ 1 ].Trim() );

                aScore.Bpm = bpm;

                continue;
            }
            #endregion
            #region BPM
            else if ( RegexBPM().Match( line ).Success )
            {
                var measure_no  = Convert.ToInt32( items[ 0 ].Substring( 1, 3 ) );
                var notes       = items[ 1 ].Trim();

                var bunkainou = notes.Length / 2;

                for ( var n = 0; n < bunkainou; n++ )
                {
                    var note = notes.Substring( n * 2, 2 );

                    if ( note.Equals( "00" ) )
                    {
                        continue;
                    }

                    if ( !bpmList.TryGetValue( note, out var bpm ) )
                    {
                        bpm = ConfigLib.System.DefaultBpm;
                    }

                    aScore.SysChannel.AddBpm( measure_no, n * ConfigLib.System.MeasureNoteNumber / bunkainou, bpm, false );
                }
                continue;
            }
            #endregion
            #region NOTE
            else if ( RegexNOTE().Match( line ).Success )
            {
                var measure_no  = Convert.ToInt32( items[ 0 ].Substring( 1, 3 ) );
                var chnl        = items[ 0 ].Substring( 4, 2 );
                var notes       = items[ 1 ].Trim();

                foreach ( var group in midiMapSet.MidiMapGroups )
                {
                    if ( !group.GroupName [ ..2 ].Equals( chnl ) )
                    {
                        continue;
                    }

                    var bunkainou = notes.Length / 2;

                    for ( var n = 0; n < bunkainou; n++ )
                    {
                        var note = notes.Substring( n * 2, 2 );

                        if ( note.Equals( "00" ) )
                        {
                            continue;
                        }

                        if ( !wavList.TryGetValue( note, out var wavNm ) )
                        {
                            wavNm = note;
                        }

                        if ( !volList.TryGetValue( note, out var volume ) )
                        {
                            volume = ConfigLib.Media.BgmMaxVolume;
                        }

                        var key = -1;
                        foreach ( var midiMap in group.MidiMaps )
                        {
                            if ( midiMap.MidiMapName.Equals( wavNm ) )
                            {
                                key = midiMap.MidiMapKey;
                                break;
                            }
                        }

                        if ( key == -1 )
                        {
                            key = midiMapSet.GetMidiMapNewKey();

                            var midiMap = new MidiMap
                            {
                                MidiMapKey  = key,
                                MidiMapName = wavNm,
                            };

                            group.AddMidiMap( midiMap );
                            midiMapSet.UpdateInfo();
                        }

                        aScore.Channels [ MidiNet.ChannelDrum ]
                            .AddNoteOn( key, measure_no, n * ConfigLib.System.MeasureNoteNumber / bunkainou, volume, false );
                    }
                    break;
                }

                continue;
            }
            #endregion
        }
    }

    public void Read( GeneralPath aGeneralPath, out MidiMapSet aMidiMapSet ) => aMidiMapSet = new();

    [GeneratedRegex( "^#WAV[0-9A-Z]{2}" )]
    private static partial Regex RegexWAV();

    [GeneratedRegex( "^#VOLUME[0-9A-Z]{2}" )]
    private static partial Regex RegexVOLUME();

    [GeneratedRegex( "^#BPM[0-9A-Z]{2}" )]
    private static partial Regex RegexBASEBPM();

    [GeneratedRegex( "^#BPM" )]
    private static partial Regex RegexBPMTOP();

    [GeneratedRegex( "^[#][\\d]{3}[1][1-9A]" )]
    private static partial Regex RegexNOTE();

    [GeneratedRegex( "^[#][\\d]{3}08" )]
    private static partial Regex RegexBPM();

    #endregion

    #region Writer

    public void Write( GeneralPath aGeneralPath, Score aScore )
    {
        using var writer = File.CreateText( aGeneralPath.AbsoluteFilePath );

        writer.WriteLine( "; Created by DrumMidiEditorApp" );

        #region タイトルと製作者とコメントその他

        writer.WriteLine();
        writer.WriteLine( "#TITLE: (no title)" );
        //writer.WriteLine( "#ARTIST: " );
        //writer.WriteLine( "#COMMENT: " );
        //writer.WriteLine( "#PANEL: " );
        //writer.WriteLine( "#PREVIEW: " );
        //writer.WriteLine( "#PREIMAGE: " );
        //writer.WriteLine( "#STAGEFILE: " );
        //writer.WriteLine( "#BACKGROUND: " );
        //writer.WriteLine( "#RESULTIMAGE: " );
        writer.WriteLine( $"#BPM: {string.Format( " " + $"{{0:000.00}}", aScore.Bpm )}" );
        //writer.WriteLine( "#DLEVEL: " );
        //writer.WriteLine( "#GLEVEL: " );
        //writer.WriteLine( "#BLEVEL: " );
        //writer.WriteLine( "#PATH_WAV: " );
        //writer.WriteLine( "#DTXVPLAYSPEED: " );

        #endregion

        #region WAVリスト
        {
            foreach ( var midiMap in aScore.EditChannel.MidiMapSet.MidiMaps )
            {
                var dtxKey = midiMap.Group?.GroupName[ ..2 ] ?? string.Empty;

                if ( !_DtxLaneDic.ContainsKey( dtxKey ) )
                {
                    continue;
                }

                writer.WriteLine( $"#WAV{HelperMath.ToBase36( midiMap.MidiMapKey )}: \t;{midiMap.MidiMapName}" );
                //writer.WriteLine( $"#VOLUME{midiMap.MidiMapKey:X}: {xxx}" );
                //writer.WriteLine( $"#PAN{0}: {1}" );
            }

            // BGM設定
            writer.WriteLine( $"#WAVFF: {aScore.BgmFilePath.FileName}" );
            writer.WriteLine( $"#VOLUMEFF: {aScore.BgmVolume}" );
            writer.WriteLine( $"#BGMWAV: FF" );
        }
        #endregion

        writer.WriteLine();

        var data_list = new List<byte>();

        {
            var measureNoMax = aScore.GetMaxMeasureNo();
            var bpm_cnt = 1;

            for ( var measure_no = 0; measure_no <= measureNoMax; measure_no++ )
            {
                #region bpm
                {
                    var measure = aScore.SysChannel.GetMeasure( measure_no );

                    if ( measure == null )
                    {
                        continue;
                    }

                    var bpm_line = measure.BpmLine;

                    if ( bpm_line == null )
                    {
                        continue;
                    }

                    var val = string.Concat( Enumerable.Repeat( "00", ConfigLib.System.MeasureNoteNumber ) );

                    foreach ( var info in bpm_line.InfoStates )
                    {
                        var bpmKey = HelperMath.ToBase36( bpm_cnt++ );
                        var bpm    = info.Value.Bpm;

                        writer.WriteLine( $"#BPM{bpmKey}: {bpm}" );

                        val = val
                            .Remove( info.Key * 2, 2 )
                            .Insert( info.Key * 2, $"{bpmKey}" );
                    }

                    writer.WriteLine( $"#{measure_no}08: {val}" );
                }
                #endregion
            }

            for ( var measure_no = 0; measure_no <= measureNoMax; measure_no++ )
            {

                #region note
                {
                    var measure = aScore.EditChannel.GetMeasure( measure_no );

                    if ( measure == null )
                    {
                        continue;
                    }

                    foreach ( var midiMap in aScore.EditChannel.MidiMapSet.MidiMaps )
                    {
                        var dtxKey = midiMap.Group?.GroupName[ ..2 ] ?? string.Empty;

                        if ( !_DtxLaneDic.ContainsKey( dtxKey ) )
                        {
                            continue;
                        }

                        var measure_line = measure.NoteLine( midiMap.MidiMapKey );

                        if ( measure_line == null )
                        {
                            continue;
                        }

                        var val = string.Concat( Enumerable.Repeat( "00", ConfigLib.System.MeasureNoteNumber ) );
                        
                        foreach ( var info in measure_line.InfoStates )
                        {
                            val= val
                                .Remove( info.Key * 2, 2 )
                                .Insert( info.Key * 2, $"{HelperMath.ToBase36( info.Value.MidiMapKey )}" );
                        }

                        writer.WriteLine( $"#{measure_no:D3}{dtxKey}: {val}" );
                    }
                }
                #endregion
            }
        }
    }

    public void Write( GeneralPath aGeneralPath, MidiMapSet aMidiMapSet )
    {
    }

#endregion
}
