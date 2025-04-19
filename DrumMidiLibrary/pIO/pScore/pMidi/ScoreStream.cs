using System;
using System.Collections.Generic;
using System.IO;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pIO.pScore.pMidi;

/// <summary>
/// Score入出力：MIDIファイル
/// </summary>
internal class ScoreStream : IScoreReader, IScoreWriter
{
    #region Reader

    public bool Validation( GeneralPath aGeneralPath )
    {
        return Log.TryCatch<bool>
        (
            () => true,
            ( e ) => false
        );
    }

    public void Read( GeneralPath aGeneralPath, out Score aScore )
    {
        using var _ = new LogBlock( $"{Log.GetThisMethodName}" );

        aScore = new();

        var pos = 0;

        var buffer = File.ReadAllBytes( aGeneralPath.AbsoluteFilePath  );

        Log.Info( $"---------------------------------" );
        Log.Info( $"File Size : {buffer.Length}" );
        Log.Info( $"Chunk     : {MidiText( buffer, pos + 0, 4 )}" );
        Log.Info( $"Length    : {MidiNumber( buffer, pos + 4, 4 )}" );
        Log.Info( $"Format    : {MidiNumber( buffer, pos + 8, 2 )}" );
        Log.Info( $"TrackNum  : {MidiNumber( buffer, pos + 10, 2 )}" );
        Log.Info( $"TimeUnit  : {MidiNumber( buffer, pos + 12, 2 )}" );

        // Check header chunk
        if ( MidiText( buffer, pos + 0, 4 ) != "MThd" )
        {
            Log.Warning( $"{Log.GetThisMethodName}:header chunk" );
            return;
        }

        // Truck number
        var truck_cnt = MidiNumber( buffer, pos + 10, 2 );

        // Resolution
        var time_unit = MidiNumber( buffer, pos + 12, 2 );
        var time_unit_cal = time_unit * 4;

        pos += 14;

        // TODO：どうしようかね
        var zoom = 1; // Config.Media.MidiImportZoom;

        for ( var track = 0; track < truck_cnt; track++ )
        {
            Log.Info( $"---------------------------------" );
            Log.Info( $"Track     : {track}" );
            Log.Info( $"Chunk     : {MidiText( buffer, pos + 0, 4 )}" );
            Log.Info( $"Length    : {MidiNumber( buffer, pos + 4, 4 )}" );

            // Check truck chunk
            if ( MidiText( buffer, pos + 0, 4 ) != "MTrk" )
            {
                Log.Warning( $"{Log.GetThisMethodName}:truck chunk" );
                break;
            }

            // Truck size
            var truck_len = MidiNumber( buffer, pos + 4, 4 );

            pos += 8;

            int     pos_bef;
            var     delta_sum   = 0;
            byte    command_bef = 0;
            byte    channel_bef = 0;

            for ( var i = 0; i < truck_len; i += pos - pos_bef )
            {
                pos_bef = pos;

                // Delta time
                var vardat_len = MidiVariableTextLength( buffer, pos );

                var delta =  MidiVariableNumber( buffer, pos, vardat_len ) * zoom ;
                delta_sum += delta;
                pos += vardat_len;

                var command = (byte)( ( buffer[ pos ] & 0xF0 ) == 0xF0 ? buffer[ pos ] : buffer[ pos ] & 0xF0 );
                var channel = (byte)( ( buffer[ pos ] & 0xF0 ) == 0xF0 ? 0 : buffer[ pos ] & 0x0F );
                pos += 1;

                if ( command >= 0x80 )
                {
                    command_bef = command;
                    channel_bef = channel;
                }
                else
                {
                    command = command_bef;
                    channel = channel_bef;
                    pos -= 1;
                }

                // Log.Info( $"Delta Time : {delta}/{delta_sum}\tCommand : {Convert.ToString( state, 16 )}" );

                switch ( command )
                {
                    case 0x90:  // NoteOn
                    case 0x80:  // NoteOff
                        {
                            var midi        = buffer[ pos ];
                            var volume      = buffer[ pos + 1 ];
                            pos += 2;

                            var measure_no = delta_sum / time_unit_cal;
                            var note_pos    = ConfigLib.System.MeasureNoteNumber * ( delta_sum % time_unit_cal ) / time_unit_cal;

                            //Log.Info( "----------------------------");
                            //Log.Info( $"Channel    : {channel}"		);
                            //Log.Info( $"Note Number: {midi}"		);
                            //Log.Info( $"Measure No : {measure_no}"	);
                            //Log.Info( $"Note       : {note_pos}"	);
                            //Log.Info( $"Velocity   : {volume}"		);
                            //Log.Info( String.Empty);

                            var channelItem = aScore.Channels[ channel ];

                            if ( channelItem != null )
                            {
                                var midiMapKey = channelItem.MidiMapSet.GetMidiMapKeyForMatchMidi( midi );

                                if ( midiMapKey == ConfigLib.System.MidiMapKeyNotSelect )
                                {
                                    var group = new MidiMapGroup
                                    {
                                        GroupKey = channelItem.MidiMapSet.GetMidiMapGroupNewKey()
                                    };
                                    var midiMap = new MidiMap
                                    {
                                        MidiMapKey  = channelItem.MidiMapSet.GetMidiMapGroupNewKey(),
                                        Midi        = midi
                                    };
                                    group.AddMidiMap( midiMap );
                                    channelItem.MidiMapSet.AddMidiMapGroup( group );
                                    channelItem.MidiMapSet.UpdateInfo();
                                }

                                var noteOn = command == 0x90 && volume != 0;

                                var info = channelItem.GetNote( midiMapKey, measure_no, note_pos );

                                if ( info != null )
                                {
                                    if ( noteOn )
                                    {
                                        info.Volume = volume;
                                        info.NoteOn = true;
                                    }
                                    else if ( delta != 0 )
                                    {
                                        info.NoteOff = true;
                                    }
                                }
                                else
                                {
                                    channelItem.AddNote( midiMapKey, measure_no, note_pos, volume, noteOn, !noteOn, false );
                                }
                            }
                        }
                        break;
                    case 0xFF:  // Meta event
                        {
                            int state = buffer[ pos ];
                            pos += 1;

                            // MidiVariableTextLength( buffer, pos );

                            int vartex_len;
                            switch ( state )
                            {
                                // Fixed length
                                case 0x51:   // Tempo setting			[3byte]
                                    {
                                        vardat_len = 1;
                                        vartex_len = MidiNumber( buffer, pos, vardat_len );
                                        pos += vardat_len;

                                        var measure_no = delta_sum / time_unit_cal;
                                        var note_pos    = ConfigLib.System.MeasureNoteNumber * ( delta_sum % time_unit_cal ) / time_unit_cal;

                                        var bpm = 0D;
                                        bpm += buffer [ pos ] << 16;
                                        bpm += buffer [ pos + 1 ] << 8;
                                        bpm += buffer [ pos + 2 ];

                                        bpm = Math.Round( zoom * 120 * 500000 / bpm, 2 );

                                        //aScore.SysChannel.AddBpm( measure_no, note_pos, bpm, false );

                                        //Log.Info( $" Measure No: {measure_no}"		);
                                        //Log.Info( $"      Note : {note_pos}"			);
                                        //Log.Info( $"       Bpm : {bpm}"				);
                                        //Log.Info( $"    Buffer : {buffer[ pos]		);
                                        //Log.Info( $"    Buffer : {buffer[ pos + 1 ]}" );
                                        //Log.Info( $"    Buffer : {buffer[ pos + 2 ]}" );
                                        //Log.Info( $String.Empty);

                                        pos += vartex_len;
                                    }
                                    break;
                                case 0x00:   // Sequence number
                                case 0x20:   // MIDI channel prefix		[1byte]
                                case 0x21:   // Port specification		[1byte]
                                case 0x2F:   // End of track			[0byte]
                                case 0x54:   // SMPTE offset			[5byte]
                                case 0x58:   // Time signature setting	[4byte]
                                case 0x59:   // Key setting				[2byte]
                                    vardat_len = 1;
                                    vartex_len = MidiNumber( buffer, pos, vardat_len );
                                    pos += vardat_len + vartex_len;
                                    break;

                                // variable length
                                case 0x01:   // Text event
                                case 0x02:   // Copyright notice
                                case 0x03:   // Sequence/truck name
                                case 0x04:   // Instrument name
                                case 0x05:   // lyrics
                                case 0x06:   // marker
                                case 0x07:   // Queue point
                                case 0x08:   // program name
                                case 0x09:   // Device name
                                case 0x7F:   // Sequencer specific meta event
                                    vardat_len = MidiVariableTextLength( buffer, pos );
                                    vartex_len = MidiVariableNumber( buffer, pos, vardat_len );
                                    pos += vardat_len + vartex_len;
                                    break;
                            }
                        }
                        break;

                    // SysEx event 
                    case 0xF0:
                        {
                            int state = buffer[ pos ];
                            pos += 1;

                            switch ( state )
                            {
                                case 0x00:   // Exclusive message
                                case 0x01:   // MIDI timecode
                                case 0x02:   // Song position pointer
                                case 0x03:   // Song select
                                case 0x06:   // Tune request
                                case 0x07:   // Exclusive message
                                case 0x08:   // Timing clock
                                case 0x0A:   // start
                                case 0x0B:   // Continue
                                case 0x0C:   // Stop
                                case 0x0E:   // Active sensing
                                    break;
                            }
                        }
                        break;

                    case 0xF8:  // TimingClock
                    case 0xFA:  // StartSequence
                    case 0xFB:  // ContinueSequence
                    case 0xFC:  // StopSequence
                    case 0xFE:  // AutoSensing
                        pos += 1;
                        break;
                    case 0xA0:  // KeyAfterTouch
                    case 0xB0:  // ControlChange
                    case 0xE0:  // PitchWheelChange
                        pos += 2;
                        break;
                    case 0xC0:  // PatchChange
                        {
                            var program = buffer[ pos ];
                            pos += 1;

                            MidiNet.ProgramChange( channel, program );
                        }
                        break;
                    case 0xD0:  // ChannelAfterTouch
                        pos += 1;
                        break;
                    default:
                        {
                            pos += 1;
                            Log.Warning( $"◆◆◆Unsupported event.System support required◆◆◆" );
                            Log.Warning( $"command: {Convert.ToHexString( [(byte)( command + channel )] )}" );
                        }
                        break;
                }
            }
        }

        aScore.UpdateInfoAllMidiMaps();
    }

    public void Read( GeneralPath aGeneralPath, out MidiMapSet aMidiMapSet ) 
        => aMidiMapSet = new();

    /// <summary>
    /// MIDI数値取得
    /// </summary>
    /// <param name="aBuffer">バッファ</param>
    /// <param name="aStartPos">開始位置</param>
    /// <param name="aLength">長さ</param>
    /// <returns>取得数値</returns>
    private static int MidiNumber( byte [] aBuffer, int aStartPos, int aLength )
    {
        var num = 0;

        for ( var i = 0; i < aLength; i++ )
        {
            num <<= 8;
            num |= aBuffer [ aStartPos + i ];
        }
        return num;
    }

    /// <summary>
    /// MIDIテキスト取得
    /// </summary>
    /// <param name="aBuffer">バッファ</param>
    /// <param name="aStartPos">開始位置</param>
    /// <param name="aLength">長さ</param>
    /// <returns>テキスト数値</returns>
    private static string MidiText( byte [] aBuffer, int aStartPos, int aLength )
    {
        var tex = string.Empty;

        for ( var i = 0; i < aLength; i++ )
        {
            tex += char.ToString( (char)aBuffer [ aStartPos + i ] );
        }

        return tex;
    }

    /// <summary>
    /// MIDI可変長数値取得
    /// </summary>
    /// <param name="aBuffer">バッファ</param>
    /// <param name="aStartPos">開始位置</param>
    /// <param name="aLength">長さ</param>
    /// <returns>取得数値</returns>
    private static int MidiVariableNumber( byte [] aBuffer, int aStartPos, int aLength )
    {
        var num = 0;

        for ( var i = 0; i < aLength; i++ )
        {
            num <<= 7;
            num |= aBuffer [ aStartPos + i ] & 0x7F;
        }

        return num;
    }

    /// <summary>
    /// MIDI可変長テキスト長さ取得
    /// </summary>
    /// <param name="aBuffer">バッファ</param>
    /// <param name="aStartPos">開始位置</param>
    /// <returns>テキスト長さ</returns>
    private static int MidiVariableTextLength( byte [] aBuffer, int aStartPos )
    {
        var num = 0;
        var len = aBuffer.Length;

        for ( var i = 0; i < len; i++ )
        {
            // If the 1st bit is 1, the reading ends
            if ( ( aBuffer [ aStartPos + i ] & 0x80 ) != 0x80 )
            {
                num = i + 1;
                break;
            }
        }

        return num;
    }

    #endregion

    #region Writer

    public void Write( GeneralPath aGeneralPath, Score aScore )
    {
        using var writer = new FileStream( aGeneralPath.AbsoluteFilePath, FileMode.Create );

        var data_list = new List<byte>();

        {
            var  count  = 0;
            byte channelNo;
            byte midi;
            byte volume;
            var  bpm = (int)( 120F * 500000F / (float)aScore.Bpm );

            var bpmBytes = BitConverter.GetBytes( bpm );

            data_list.Add( 0x00 );
            data_list.Add( 0xFF );
            data_list.Add( 0x51 );
            data_list.Add( 0x03 );
            data_list.Add( bpmBytes.Length >= 3 ? bpmBytes [ 2 ] : (byte)0x00 );
            data_list.Add( bpmBytes.Length >= 2 ? bpmBytes [ 1 ] : (byte)0x00 );
            data_list.Add( bpmBytes.Length >= 1 ? bpmBytes [ 0 ] : (byte)0x00 );

            var measureNoMax = aScore.GetMaxMeasureNo();

            for ( var measure_no = 0; measure_no <= measureNoMax; measure_no++ )
            {
                for ( var note_pos = 0; note_pos < ConfigLib.System.MeasureNoteNumber; note_pos++ )
                {
                    #region bpm
                    {
                        var bpmInfo = aScore.SysChannel.GetBpm( measure_no, note_pos );

                        if ( bpmInfo != null )
                        {
                            bpm = (int)( 120F * 500000F / (float)bpmInfo.Bpm );

                            bpmBytes = BitConverter.GetBytes( bpm );

                            // delta time
                            var val = 1920 * count / ConfigLib.System.MeasureNoteNumber;

                            if ( val < 128 )
                            {
                                data_list.Add( (byte)val );
                            }
                            else if ( val < 16384 )
                            {
                                data_list.Add( (byte)( ( val >> 7 ) | 0x80 ) );
                                data_list.Add( (byte)( val & 0x7F ) );
                            }
                            else if ( val < 2097152 )
                            {
                                data_list.Add( (byte)( ( val >> 14 ) | 0x80 ) );
                                data_list.Add( (byte)( ( val >> 7 ) | 0x80 ) );
                                data_list.Add( (byte)( val & 0x7F ) );
                            }
                            else if ( val < 268435456 )
                            {
                                data_list.Add( (byte)( ( val >> 21 ) | 0x80 ) );
                                data_list.Add( (byte)( ( val >> 14 ) | 0x80 ) );
                                data_list.Add( (byte)( ( val >> 7 ) | 0x80 ) );
                                data_list.Add( (byte)( val & 0x7F ) );
                            }

                            data_list.Add( 0xFF );  // Meta event
                            data_list.Add( 0x51 );    // Tempo setting
                            data_list.Add( 0x03 );
                            data_list.Add( bpmBytes.Length >= 3 ? bpmBytes [ 2 ] : (byte)0x00 );
                            data_list.Add( bpmBytes.Length >= 2 ? bpmBytes [ 1 ] : (byte)0x00 );
                            data_list.Add( bpmBytes.Length >= 1 ? bpmBytes [ 0 ] : (byte)0x00 );

                            count = 0;
                        }
                    }
                    #endregion

                    #region note

                    foreach ( var channel in aScore.Channels.Values )
                    {
                        var measure = channel.GetMeasure( measure_no );

                        if ( measure == null )
                        {
                            continue;
                        }

                        foreach ( var midiMap in channel.MidiMapSet.MidiMaps )
                        {
                            if ( !measure.NoteLines.TryGetValue( midiMap.MidiMapKey, out var measure_line ) )
                            {
                                continue;
                            }

                            if ( !measure_line.InfoStates.TryGetValue( note_pos, out var note ) )
                            {
                                continue;
                            }

                            channelNo = channel.ChannelNo;
                            midi = midiMap.Midi;
                            volume = (byte)MidiNet.CheckMidiVolume( note.Volume + midiMap.VolumeAddIncludeGroup );

                            // delta time
                            var val = 1920 * count / ConfigLib.System.MeasureNoteNumber;

                            if ( val < 128 )
                            {
                                data_list.Add( (byte)val );
                            }
                            else if ( val < 16384 )
                            {
                                data_list.Add( (byte)( ( val >> 7 ) | 0x80 ) );
                                data_list.Add( (byte)( val & 0x7F ) );
                            }
                            else if ( val < 2097152 )
                            {
                                data_list.Add( (byte)( ( val >> 14 ) | 0x80 ) );
                                data_list.Add( (byte)( ( val >> 7 ) | 0x80 ) );
                                data_list.Add( (byte)( val & 0x7F ) );
                            }
                            else if ( val < 268435456 )
                            {
                                data_list.Add( (byte)( ( val >> 21 ) | 0x80 ) );
                                data_list.Add( (byte)( ( val >> 14 ) | 0x80 ) );
                                data_list.Add( (byte)( ( val >> 7 ) | 0x80 ) );
                                data_list.Add( (byte)( val & 0x7F ) );
                            }

                            if ( note.NoteOff )
                            {
                                data_list.Add( (byte)( 0x80 | channelNo ) );  // NoteOff
                                data_list.Add( midi );
                                data_list.Add( 0x00 );

                                count = 0;
                            }

                            if ( note.NoteOn )
                            {
                                if ( note.NoteOff )
                                {
                                    data_list.Add( 0x00 );  // Delta time
                                }

                                data_list.Add( (byte)( 0x90 | channelNo ) );  // NoteOn
                                data_list.Add( midi );
                                data_list.Add( volume );

                                if ( channelNo == MidiNet.ChannelDrum )
                                {
                                    data_list.Add( 0x00 );  // Delta time
                                    data_list.Add( (byte)( 0x80 | channelNo ) );  // NoteOff
                                    data_list.Add( midi );
                                    data_list.Add( 0x00 );
                                }

                                count = 0;
                            }
                        }
                    }
                    #endregion

                    count++;
                }
            }
        }

        #region Midi Header
        {
            // chunk type
            writer.WriteByte( 0x4D );
            writer.WriteByte( 0x54 );
            writer.WriteByte( 0x68 );
            writer.WriteByte( 0x64 );
            // data length
            writer.WriteByte( 0x00 );
            writer.WriteByte( 0x00 );
            writer.WriteByte( 0x00 );
            writer.WriteByte( 0x06 );
            // format
            writer.WriteByte( 0x00 );
            writer.WriteByte( 0x00 );
            // truck count
            writer.WriteByte( 0x00 );
            writer.WriteByte( 0x01 );
            // time unit
            writer.WriteByte( 0x01 );
            writer.WriteByte( 0xE0 );
        }
        #endregion

        #region Midi Track
        {
            // chunk type
            writer.WriteByte( 0x4D );
            writer.WriteByte( 0x54 );
            writer.WriteByte( 0x72 );
            writer.WriteByte( 0x6B );

            // data length
            {
                var bs = BitConverter.GetBytes( data_list.Count + 4 );
                writer.WriteByte( bs [ 3 ] );
                writer.WriteByte( bs [ 2 ] );
                writer.WriteByte( bs [ 1 ] );
                writer.WriteByte( bs [ 0 ] );
            }

            foreach ( var b in data_list )
            {
                // data
                writer.WriteByte( b );
            }
            // end truck
            writer.WriteByte( 0x00 );
            writer.WriteByte( 0xFF );
            writer.WriteByte( 0x2F );
            writer.WriteByte( 0x00 );
        }
        #endregion
    }

    public void Write( GeneralPath aGeneralPath, MidiMapSet aMidiMapSet )
    {
    }

    #endregion
}
