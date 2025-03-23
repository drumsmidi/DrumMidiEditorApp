using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pIO.pScore.pDtxMania;

/// <summary>
/// Score入出力：DtxMania（非推奨）
/// </summary>
internal partial class ScoreStream : IScoreReader
{
    #region Reader

    public bool Validation( GeneralPath aGeneralPath )
    {
        var errorFlag = false;

        try
        {
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            errorFlag = true;
        }

        return !errorFlag;
    }

    public void Read( GeneralPath aGeneralPath, out Score aScore )
    {
        using var _ = new LogBlock( $"{Log.GetThisMethodName}:load dtx file" );

        aScore = new();

        using var reader = new StreamReader( aGeneralPath.AbsoulteFilePath, Encoding.GetEncoding("UTF-8") );

        var midiMapSet = aScore.Channels[ MidiNet.ChannelDrum ].MidiMapSet;

        AddMidiMapGroup( ref midiMapSet, "11", "HiHatClose" );
        AddMidiMapGroup( ref midiMapSet, "12", "Snare" );
        AddMidiMapGroup( ref midiMapSet, "13", "BassDrum" );
        AddMidiMapGroup( ref midiMapSet, "14", "HighTom" );
        AddMidiMapGroup( ref midiMapSet, "15", "LowTom" );
        AddMidiMapGroup( ref midiMapSet, "16", "Cymbal" );
        AddMidiMapGroup( ref midiMapSet, "17", "FloorTom" );
        AddMidiMapGroup( ref midiMapSet, "18", "HiHatOpen" );
        AddMidiMapGroup( ref midiMapSet, "19", "RideCymbal" );
        AddMidiMapGroup( ref midiMapSet, "1A", "LeftCymbal" );

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
            {
                if ( MyRegex().Match( line ).Success )
                {
                    var zz      = items[ 0 ].Substring( 4, 2 );
                    var name    = items[ 1 ].Trim();

                    wavList [ zz ] = name;

                    continue;
                }
            }
            #endregion

            #region VOLUMEzz
            {
                if ( MyRegex1().Match( line ).Success )
                {
                    var zz      = items[ 0 ].Substring( 4, 2 );
                    var volume  = Convert.ToInt32( items[ 1 ].Trim() );

                    volList [ zz ] = volume;

                    continue;
                }
            }
            #endregion

            #region BPMzz
            {
                if ( MyRegex2().Match( line ).Success )
                {
                    var zz  = items[ 0 ].Substring( 4, 2 );
                    var bpm = Convert.ToDouble( items[ 1 ].Trim() );

                    bpmList [ zz ] = bpm;

                    continue;
                }
            }
            #endregion

            #region NOTE
            {
                if ( MyRegex3().Match( line ).Success )
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
                                volume = 100;
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
            }
            #endregion

            #region BPM
            {
                if ( MyRegex4().Match( line ).Success )
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
            }
            #endregion
        }
    }

    public void Read( GeneralPath aGeneralPath, out MidiMapSet aMidiMapSet ) => aMidiMapSet = new();

    /// <summary>
    /// MidiMapGroupに未設定のMidiMap情報を追加
    /// </summary>
    /// <param name="aMidiMapSet">追加先MidiMapセット</param>
    /// <param name="aGroupName">グループ名</param>
    /// <param name="aMidiMapName">MidiMap名</param>
    private static void AddMidiMapGroup( ref MidiMapSet aMidiMapSet, string aGroupName, string aMidiMapName )
    {
        var group = new MidiMapGroup
        {
            GroupKey    = aMidiMapSet.GetMidiMapGroupNewKey(),
            GroupName   = aGroupName + " " + aMidiMapName,
        };

        var midiMap = new MidiMap
        {
            MidiMapKey  = aMidiMapSet.GetMidiMapNewKey(),
            MidiMapName = aMidiMapName,
        };

        group.AddMidiMap( midiMap );

        aMidiMapSet.AddMidiMapGroup( group );
        aMidiMapSet.UpdateInfo();
    }

    [GeneratedRegex( "^#WAV[0-9A-Z]{2}" )]
    private static partial Regex MyRegex();
    [GeneratedRegex( "^#VOLUME[0-9A-Z]{2}" )]
    private static partial Regex MyRegex1();
    [GeneratedRegex( "^#BPM[0-9A-Z]{2}" )]
    private static partial Regex MyRegex2();
    [GeneratedRegex( "^[#][\\d]{3}[1][1-9A]" )]
    private static partial Regex MyRegex3();
    [GeneratedRegex( "^[#][\\d]{3}08" )]
    private static partial Regex MyRegex4();

    #endregion
}
