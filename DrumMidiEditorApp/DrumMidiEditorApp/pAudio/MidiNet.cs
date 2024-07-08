using System;
using System.Collections.Generic;
using DrumMidiEditorApp.pLog;
using Windows.Devices.Midi;
using Windows.Storage.Streams;

namespace DrumMidiEditorApp.pAudio;

/// <summary>
/// MIDI操作
/// </summary>
internal static partial class MidiNet
{
    /// <summary>
    /// ロック用
    /// </summary>
    private static readonly object _LockObj = new();

    /// <summary>
    /// MIDI-OUTデバイス
    /// </summary>
    private static MidiOutDeviceWatcher? _MidiOutDeviceWatcher;

    /// <summary>
    /// MIDI-OUTデバイス
    /// </summary>
    private static IMidiOutPort? _CurrentMidiOutputDevice;


    #region Midi define

    /// <summary>
    /// チェンネル番号最小数（0-15）
    /// </summary>
    public static byte ChannelMinNo { get; private set; } = 0;

    /// <summary>
    /// チャンネル番号最大数（0-15）
    /// </summary>
    public static byte ChannelMaxNo { get; private set; } = 15;

    /// <summary>
    /// チャンネル番号リスト（0-15）
    /// </summary>
    public static List<byte> ChannelNoList
    {
        get; private set;
    } =
    [
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
    ];

    /// <summary>
    /// MIDI最小音量
    /// </summary>
    public static int MidiMinVolume { get; private set; } = 0;

    /// <summary>
    /// MIDI最大音量
    /// </summary>
    public static int MidiMaxVolume { get; private set; } = 127;

    /// <summary>
    /// MIDIノート番号最小
    /// </summary>
    public static byte MidiNoteMin { get; private set; } = 0;

    /// <summary>
    /// MIDIノート番号最大
    /// </summary>
    public static byte MidiNoteMax { get; private set; } = 127;

    /// <summary>
    /// MIDIノート番号チェック
    /// </summary>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <returns>範囲内のMIDIノート番号(0-127)</returns>
    public static byte CheckMidiNote( int aMidi )
    {
        if ( aMidi < MidiNoteMin )
        {
            return MidiNoteMin;
        }
        else if ( aMidi > MidiNoteMax )
        {
            return MidiNoteMax;
        }
        return (byte)aMidi;
    }

    /// <summary>
    /// MIDI音量チェック
    /// </summary>
    /// <param name="aVolume">音量</param>
    /// <returns>範囲内の音量(0-127)</returns>
    public static int CheckMidiVolume( int aVolume )
    {
        if ( aVolume < MidiMinVolume )
        {
            return MidiMinVolume;
        }
        else if ( aVolume > MidiMaxVolume )
        {
            return MidiMaxVolume;
        }
        return aVolume;
    }

    /// <summary>
    /// Drum midiチャンネル
    /// </summary>
    public static byte ChannelDrum { get; set; } = 0x9;

    /// <summary>
    /// ドラムプログラムリスト
    /// </summary>
    /// <returns>ドラムプログラムリスト</returns>
    public static Dictionary<byte, string> DrumProgramList
    {
        get; private set;
    } = new()
        {
            {  0, "Standard"    },
            {  9, "Room"        },
            { 17, "Power"       },
            { 25, "Electronic"  },
            { 26, "TR-808"      },
            { 33, "Jass"        },
            { 41, "Brush"       },
            { 49, "Orchestra"   },
            { 57, "SFX"         },
        };

    #endregion

    #region Midi device

    /// <summary>
    /// MIDI-OUTデバイスウオッチ開始
    /// </summary>
    /// <returns>Trueのみ</returns>
    public static void MidiOutDeviceWatcher()
    {
        _MidiOutDeviceWatcher?.Dispose();
        _MidiOutDeviceWatcher = new();
        _MidiOutDeviceWatcher.DeviceStart();
    }

    /// <summary>
    /// MIDI-OUTデバイス初期化
    /// </summary>
    /// <param name="aMidiOutDeviceName"></param>
    /// <returns>Trueのみ</returns>
    public static async void InitDeviceAsync( string aMidiOutDeviceName )
    {
        if ( _MidiOutDeviceWatcher == null )
        {
            return;
        }
        var port = await _MidiOutDeviceWatcher.GetMidiOutPortAsync( aMidiOutDeviceName );

        lock ( _LockObj )
        {
            SystemReset();
            _CurrentMidiOutputDevice?.Dispose();
            _CurrentMidiOutputDevice = port;
        }
    }

    /// <summary>
    /// MIDIデバイスリスト取得
    /// </summary>
    /// <returns>MIDI-OUTデバイスリスト</returns>
    public static List<string> GetDeviceList()
    {
        var list = new List<string>();

        if ( _MidiOutDeviceWatcher != null )
        {
            foreach ( var device in _MidiOutDeviceWatcher.MidiOutDeviceDic.Values )
            {
                list.Add( device.Name );

                Log.Info( $"{device.Id}:{device.Name}", false );
            }
        }
        return list;
    }

    #endregion

    #region Midi-Out event

    /// <summary>
    /// ノートONイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <param name="aVolume">音量(127基準)</param>
    public static void NoteOn( byte aChannel, byte aMidi, byte aVolume )
        => MidiOutShortMsg( new MidiNoteOnMessage( Convert.ToByte( aChannel ), Convert.ToByte( aMidi ), Convert.ToByte( aVolume ) ) );

    /// <summary>
    /// ノートONイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <param name="aVolume">音量(127基準)</param>
    public static void NoteOff( byte aChannel, byte aMidi )
        => MidiOutShortMsg( new MidiNoteOffMessage( Convert.ToByte( aChannel ), Convert.ToByte( aMidi ), Convert.ToByte( 0 ) ) );

    /// <summary>
    /// ノートONイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <param name="aVolume">音量(127基準)</param>
    public static void PolyphonicKeyPressure( byte aChannel, byte aNote, byte aPressure )
        => MidiOutShortMsg( new MidiPolyphonicKeyPressureMessage( Convert.ToByte( aChannel ), Convert.ToByte( aNote ), Convert.ToByte( aPressure ) ) );

    /// <summary>
    /// ノートONイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <param name="aVolume">音量(127基準)</param>
    public static void ControlChange( byte aChannel, byte aController, byte aControlValue )
        => MidiOutShortMsg( new MidiControlChangeMessage( Convert.ToByte( aChannel ), Convert.ToByte( aController ), Convert.ToByte( aControlValue ) ) );


    /// <summary>
    /// プログラムチェンジイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aProgram">プログラムNO</param>
    public static void ProgramChange( byte aChannel, byte aProgram )
        => MidiOutShortMsg( new MidiProgramChangeMessage( Convert.ToByte( aChannel ), Convert.ToByte( aProgram ) ) );


    /// <summary>
    /// プログラムチェンジイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aProgram">プログラムNO</param>
    public static void ChannelPressure( byte aChannel, byte aPressure )
        => MidiOutShortMsg( new MidiChannelPressureMessage( Convert.ToByte( aChannel ), Convert.ToByte( aPressure ) ) );

    /// <summary>
    /// プログラムチェンジイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aProgram">プログラムNO</param>
    public static void PitchBendChange( byte aChannel, byte aBend )
        => MidiOutShortMsg( new MidiPitchBendChangeMessage( Convert.ToByte( aChannel ), Convert.ToByte( aBend ) ) );

    /// <summary>
    /// プログラムチェンジイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aProgram">プログラムNO</param>
    public static void SystemExclusive( string aSysExMessage )
    {
        var dataWriter = new DataWriter();
        var sysExMessageLength = aSysExMessage.Length;

        // Do not send a blank SysEx message
        if ( sysExMessageLength == 0 )
        {
            return;
        }

        // SysEx messages are two characters long with 1-character space in between them
        // So we add 1 to the message length, so that it is perfectly divisible by 3
        // The loop count tracks the number of individual message pieces
        var loopCount = (sysExMessageLength + 1) / 3;

        // Expecting a string of format "F0 NN NN NN NN.... F7", where NN is a byte in hex
        for ( var i = 0; i < loopCount; i++ )
        {
            var messageString = aSysExMessage.Substring(3 * i, 2);
            var messageByte = Convert.ToByte(messageString, 16);
            dataWriter.WriteByte( messageByte );
        }
        MidiOutShortMsg( new MidiSystemExclusiveMessage( dataWriter.DetachBuffer() ) );
    }

    /// <summary>
    /// ノートONイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    public static void MidiTimeCode( byte aFrameType, byte aValues )
        => MidiOutShortMsg( new MidiTimeCodeMessage( Convert.ToByte( aFrameType ), Convert.ToByte( aValues ) ) );

    /// <summary>
    /// ノートONイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    public static void SongPositionPointer( ushort aBeats )
        => MidiOutShortMsg( new MidiSongPositionPointerMessage( Convert.ToUInt16( aBeats ) ) );

    /// <summary>
    /// ノートONイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    public static void SongSelect( byte aSong )
        => MidiOutShortMsg( new MidiSongSelectMessage( Convert.ToByte( aSong ) ) );

    /// <summary>
    /// MIDI-OUT リセット
    /// </summary>
    public static void TuneRequest()
        => MidiOutShortMsg( new MidiTuneRequestMessage() );

    /// <summary>
    /// MIDI-OUT リセット
    /// </summary>
    public static void TimingClock()
        => MidiOutShortMsg( new MidiTimingClockMessage() );

    /// <summary>
    /// MIDI-OUT リセット
    /// </summary>
    public static void Start()
        => MidiOutShortMsg( new MidiStartMessage() );

    /// <summary>
    /// MIDI-OUT リセット
    /// </summary>
    public static void Continue()
        => MidiOutShortMsg( new MidiContinueMessage() );

    /// <summary>
    /// MIDI-OUT リセット
    /// </summary>
    public static void Stop()
        => MidiOutShortMsg( new MidiStopMessage() );

    /// <summary>
    /// MIDI-OUT リセット
    /// </summary>
    public static void ActiveSensing()
        => MidiOutShortMsg( new MidiActiveSensingMessage() );

    /// <summary>
    /// MIDI-OUT リセット
    /// </summary>
    public static void SystemReset()
        => MidiOutShortMsg( new MidiSystemResetMessage() );

    /// <summary>
    /// MIDI-OUT ショートメッセージ
    /// </summary>
    /// <param name="aStatus">ステータス</param>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aData1">データ１</param>
    /// <param name="aData2">データ２</param>
    /// <returns>実行結果</returns>
    private static void MidiOutShortMsg( IMidiMessage aMidiMessage )
        => _CurrentMidiOutputDevice?.SendMessage( aMidiMessage );

    #endregion
}
