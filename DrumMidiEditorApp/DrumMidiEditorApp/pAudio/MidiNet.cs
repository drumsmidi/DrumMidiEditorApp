using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using DrumMidiEditorApp.pLog;

namespace DrumMidiEditorApp.pAudio;

/// <summary>
/// MIDI操作
/// </summary>
public static partial class MidiNet
{
    /// <summary>
    /// ロック用
    /// </summary>
    private static readonly object _LockObj = new();

    /// <summary>
    /// MIDI-OUTデバイスハンドル
    /// </summary>
    private static IntPtr _MidiHandle = IntPtr.Zero;

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
    /// MIDI-OUTデバイス初期化
    /// </summary>
    /// <param name="aMidiOutDeviceName"></param>
    /// <returns>Trueのみ</returns>
    public static bool InitDevice( string aMidiOutDeviceName )
    {
        lock ( _LockObj )
        {
            var index = -1;

            var list = GetDeviceList();

            for ( var i = 0; i < list.Count; i++ )
            {
                var name = list[ i ];

                if ( aMidiOutDeviceName.Equals( name ) )
                {
                    index = i;
                }
            }

            _ = midiOutClose( _MidiHandle );

            var ret = midiOutOpen( out _MidiHandle, index, null, 0, 0 );

            if ( ret != 0 )
            {
                var msg = string.Empty;
                switch ( ret )
                {
                    case 2:
                        msg = "MMSYSERR_BADDEVICEID";
                        break;
                    case 4:
                        msg = "MMSYSERR_ALLOCATED";
                        break;
                    case 7:
                        msg = "MMSYSERR_NOMEM";
                        break;
                    case 11:
                        msg = "MMSYSERR_INVALPARAM";
                        break;
                }

                Log.Warning( $"{Log.GetThisMethodName}:{msg}", true );
            }
        }
        return true;
    }

    /// <summary>
    /// MIDIデバイスリスト取得
    /// </summary>
    /// <returns>MIDI-OUTデバイスリスト</returns>
    public static List<string> GetDeviceList()
    {
        var list = new List<string>();

        var count = GetDeviceCount();

        for ( var i = 0; i < count; i++ )
        {
            list.Add( GetDeviceName( i ) );
        }

        return list;
    }

    /// <summary>
    /// デバイス名取得
    /// </summary>
    /// <param name="aIndex">MIDIデバイスインデックス</param>
    /// <returns>取得：デバイス名、未取得：空文字</returns>
    public static string GetDeviceName( int aIndex )
    {
        _ = midiOutGetDevCaps( (uint)aIndex, out var info, Marshal.SizeOf( typeof( MidiOutCaps ) ) );

        return info.Pname ?? string.Empty;
    }

    /// <summary>
    /// デバイス数取得
    /// </summary>
    /// <returns>デバイス数</returns>
    public static int GetDeviceCount() => (int)midiOutGetNumDevs();

    #endregion

    #region Midi-Out event

    /// <summary>
    /// ノートONイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <param name="aVolume">音量(127基準)</param>
    public static void NoteOn( byte aChannel, byte aMidi, byte aVolume )
        => MidiOutShortMsg( 0x9, aChannel, aMidi, aVolume );

    /// <summary>
    /// ノートONイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <param name="aVolume">音量(127基準)</param>
    public static void NoteOff( byte aChannel, byte aMidi )
        => MidiOutShortMsg( 0x8, aChannel, aMidi, 0 );

    /// <summary>
    /// プログラムチェンジイベント
    /// </summary>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aProgram">プログラムNO</param>
    public static void ProgramChange( byte aChannel, byte aProgram )
        => MidiOutShortMsg( 0xC, aChannel, aProgram, 0 );

    /// <summary>
    /// MIDI-OUT ショートメッセージ
    /// </summary>
    /// <param name="aStatus">ステータス</param>
    /// <param name="aChannel">チャンネル</param>
    /// <param name="aData1">データ１</param>
    /// <param name="aData2">データ２</param>
    /// <returns>実行結果</returns>
    private static uint MidiOutShortMsg( byte aStatus, byte aChannel, byte aData1, byte aData2 )
        => midiOutShortMsg( _MidiHandle, ( aStatus << 4 ) | aChannel | ( aData1 << 8 ) | ( aData2 << 16 ) );

    /// <summary>
    /// MIDI-OUT リセット
    /// </summary>
    public static void Reset()
        => _ = midiOutReset( _MidiHandle );

    #endregion

    #region Windows API

    [LibraryImport( "Winmm.dll" )]
    private static partial uint midiOutGetNumDevs();

    [StructLayout( LayoutKind.Sequential, CharSet = CharSet.Ansi )]
    private struct MidiOutCaps
    {
        public ushort   Mid;
        public ushort   Pid;
        public uint     DriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string   Pname;
        public ushort   Technology;
        public ushort   Voices;
        public ushort   Notes;
        public ushort   ChannelMask;
        public uint     Support;
    }

    [DllImport( "Winmm.dll" )]
    private static extern uint midiOutGetDevCaps(
        uint aDeviceID, out MidiOutCaps aMidiOutCaps, int aMidiOutCapsLength );

    [LibraryImport( "Winmm.dll" )]
    private static partial uint midiOutOpen(
        out IntPtr aHmo, int aDeviceID, MidiOutProc? aCallback, int aCallbackInstance, uint aFlags );

    private delegate void MidiOutProc( IntPtr aHmo, uint aHwnd, int aInstance, int aParam1, int aParam2 );

    [LibraryImport( "Winmm.dll" )]
    private static partial uint midiOutShortMsg( IntPtr aHmo, int aMsg );

    [LibraryImport( "Winmm.dll" )]
    private static partial uint midiOutReset( IntPtr aHandle );

    [LibraryImport( "Winmm.dll" )]
    private static partial uint midiOutClose( IntPtr aHmo );

    #endregion
}
