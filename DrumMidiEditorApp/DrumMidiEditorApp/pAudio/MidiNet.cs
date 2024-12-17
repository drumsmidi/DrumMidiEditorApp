using System;
using System.Collections.Generic;
using System.Threading;
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
    private static readonly Lock _LockObj = new();

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

    #endregion

    #region Midi device

    /// <summary>
    /// MIDI-OUTデバイスウオッチ開始
    /// </summary>
    public static void MidiOutDeviceWatcher()
    {
        _MidiOutDeviceWatcher?.Dispose();
        _MidiOutDeviceWatcher = new();
        _MidiOutDeviceWatcher.DeviceStart();
    }

    /// <summary>
    /// MIDI-OUTデバイス初期化
    /// </summary>
    /// <param name="aMidiOutDeviceName">MIDI-OUTデバイス名</param>
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
    /// <param name="aChannel">チャンネル<(0-15)/param>
    /// <param name="aNote">MIDIノート番号</param>
    /// <param name="aVolume">音量(127基準)</param>
    public static void NoteOn( byte aChannel, byte aNote, byte aVolume )
        => MidiOutShortMsg( new MidiNoteOnMessage( Convert.ToByte( aChannel ), Convert.ToByte( aNote ), Convert.ToByte( aVolume ) ) );

    /// <summary>
    /// ノートOFFイベント
    /// </summary>
    /// <param name="aChannel">チャンネル(0-15)</param>
    /// <param name="aNote">MIDIノート番号</param>
    public static void NoteOff( byte aChannel, byte aNote )
        => MidiOutShortMsg( new MidiNoteOffMessage( Convert.ToByte( aChannel ), Convert.ToByte( aNote ), Convert.ToByte( 0 ) ) );

    /// <summary>
    /// ポリフォニック キーの圧力を指定する MIDI メッセージ
    /// </summary>
    /// <param name="aChannel">チャンネル(0-15)</param>
    /// <param name="aNote">MIDIノート番号</param>
    /// <param name="aPressure">ポリフォニック キー圧力(127基準)</param>
    public static void PolyphonicKeyPressure( byte aChannel, byte aNote, byte aPressure )
        => MidiOutShortMsg( new MidiPolyphonicKeyPressureMessage( Convert.ToByte( aChannel ), Convert.ToByte( aNote ), Convert.ToByte( aPressure ) ) );

    /// <summary>
    /// コントロールの変更を指定する MIDI メッセージ
    /// </summary>
    /// <param name="aChannel">チャンネル(0-15)</param>
    /// <param name="aController">コントローラー(0-127)</param>
    /// <param name="aControlValue">コントローラーに適用する値(0-127)</param>
    public static void ControlChange( byte aChannel, byte aController, byte aControlValue )
        => MidiOutShortMsg( new MidiControlChangeMessage( Convert.ToByte( aChannel ), Convert.ToByte( aController ), Convert.ToByte( aControlValue ) ) );


    /// <summary>
    /// プログラムの変更を指定する MIDI メッセージ
    /// </summary>
    /// <param name="aChannel">チャンネル(0-15)</param>
    /// <param name="aProgram">プログラムNO(0-127)</param>
    public static void ProgramChange( byte aChannel, byte aProgram )
        => MidiOutShortMsg( new MidiProgramChangeMessage( Convert.ToByte( aChannel ), Convert.ToByte( aProgram ) ) );


    /// <summary>
    /// チャネル圧力を指定する MIDI メッセージ
    /// </summary>
    /// <param name="aChannel">チャンネル(0-15)</param>
    /// <param name="aPressure">圧力(0-127)</param>
    public static void ChannelPressure( byte aChannel, byte aPressure )
        => MidiOutShortMsg( new MidiChannelPressureMessage( Convert.ToByte( aChannel ), Convert.ToByte( aPressure ) ) );

    /// <summary>
    /// ピッチベンドの変更を指定する MIDI メッセージ
    /// </summary>
    /// <param name="aChannel">チャンネル(0-15)</param>
    /// <param name="aBend">0 から 16383 の 14 ビット値として指定されたピッチ ベンド値</param>
    public static void PitchBendChange( byte aChannel, byte aBend )
        => MidiOutShortMsg( new MidiPitchBendChangeMessage( Convert.ToByte( aChannel ), Convert.ToByte( aBend ) ) );

    /// <summary>
    /// システム排他メッセージを指定する MIDI メッセージ
    /// </summary>
    /// <param name="aSysExMessage">システムメッセージ</param>
    public static void SystemExclusive( string aSysExMessage )
    {
        var sysExMessageLength = aSysExMessage.Length;

        // Do not send a blank SysEx message
        if ( sysExMessageLength == 0 )
        {
            return;
        }

        var dataWriter = new DataWriter();

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
    /// タイム コードを指定する MIDI メッセージ
    /// </summary>
    /// <param name="aFrameType">フレームの種類(0-7)</param>
    /// <param name="aValues">時刻コード(0-15)</param>
    public static void MidiTimeCode( byte aFrameType, byte aValues )
        => MidiOutShortMsg( new MidiTimeCodeMessage( Convert.ToByte( aFrameType ), Convert.ToByte( aValues ) ) );

    /// <summary>
    /// 曲の位置ポインターを指定する MIDI メッセージ
    /// </summary>
    /// <param name="aBeats">0 から 16383 の 14 ビット値でエンコードされた曲の位置</param>
    public static void SongPositionPointer( ushort aBeats )
        => MidiOutShortMsg( new MidiSongPositionPointerMessage( Convert.ToUInt16( aBeats ) ) );

    /// <summary>
    /// 選択した曲を指定する MIDI メッセージ
    /// </summary>
    /// <param name="aSong">曲選択(0-127)</param>
    public static void SongSelect( byte aSong )
        => MidiOutShortMsg( new MidiSongSelectMessage( Convert.ToByte( aSong ) ) );

    /// <summary>
    /// チューニング要求を指定する MIDI メッセージ
    /// </summary>
    public static void TuneRequest()
        => MidiOutShortMsg( new MidiTuneRequestMessage() );

    /// <summary>
    /// タイミング クロックを指定する MIDI メッセージ
    /// </summary>
    public static void TimingClock()
        => MidiOutShortMsg( new MidiTimingClockMessage() );

    /// <summary>
    /// 開始メッセージを指定する MIDI メッセージ
    /// </summary>
    public static void Start()
        => MidiOutShortMsg( new MidiStartMessage() );

    /// <summary>
    /// 続行メッセージを指定する MIDI メッセージ
    /// </summary>
    public static void Continue()
        => MidiOutShortMsg( new MidiContinueMessage() );

    /// <summary>
    /// 停止メッセージを指定する MIDI メッセージ
    /// </summary>
    public static void Stop()
        => MidiOutShortMsg( new MidiStopMessage() );

    /// <summary>
    /// アクティブなセンシングを指定する MIDI メッセージ
    /// </summary>
    public static void ActiveSensing()
        => MidiOutShortMsg( new MidiActiveSensingMessage() );

    /// <summary>
    /// システム リセットを指定する MIDI メッセージ
    /// </summary>
    public static void SystemReset()
        => MidiOutShortMsg( new MidiSystemResetMessage() );

    /// <summary>
    /// MIDI-OUT ショートメッセージ
    /// </summary>
    /// <param name="aMidiMessage">MIDIメッセージ</param>
    private static void MidiOutShortMsg( IMidiMessage aMidiMessage )
        => _CurrentMidiOutputDevice?.SendMessage( aMidiMessage );

    #endregion
}
