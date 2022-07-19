using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pGeneralFunction.pAudio;

/// <summary>
/// MIDI操作
/// </summary>
public class MidiNet
{
    /// <summary>
    /// ロック用
    /// </summary>
    private static readonly object _LockObj = new();

    /// <summary>
    /// MIDI-OUTデバイスハンドル
    /// </summary>
    private static IntPtr _MidiHandle = IntPtr.Zero;

    #region Midi device

    /// <summary>
    /// MIDI-OUTデバイス初期化
    /// </summary>
    /// <returns>Trueのみ</returns>
    public static bool InitDevice()
    {
        lock ( _LockObj )
        {
            var index = -1;

			var list = GetDeviceList();

            for ( int i = 0; i < list.Count; i++ )
			{
				var name = list[ i ];

				if ( Config.Media.MidiOutDeviceName.Equals( name ) )
				{
                    index = i;
				}
			}

            _ = midiOutClose( _MidiHandle );

            var ret = midiOutOpen( out _MidiHandle, index, null, 0, 0 );

            if ( ret != 0 )
            {
                var msg = String.Empty;
                switch ( ret )
                {
                    case  2: msg = "MMSYSERR_BADDEVICEID";  break;
                    case  4: msg = "MMSYSERR_ALLOCATED";    break;
                    case  7: msg = "MMSYSERR_NOMEM";        break;
                    case 11: msg = "MMSYSERR_INVALPARAM";   break;
                }

                Log.Warning( $"{Log.GetThisMethodName}:{msg}" );
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

		for ( int i = 0; i < count; i++ )
		{
			list.Add( MidiNet.GetDeviceName( i ) );
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

        return info.Pname ?? String.Empty ;
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
        => MidiOutShortMsg( 0x8, aChannel, aMidi, (byte)0 );

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

    /// <summary>
    /// ドラムプログラムリスト
    /// </summary>
    /// <returns>ドラムプログラムリスト</returns>
    public static Dictionary<byte,string> GetDrumProgramList()
    {
        return new()
        {
            {  0, "Standard" },
            {  9, "Room" },
            { 17, "Power" },
            { 25, "Electronic" },
            { 26, "TR-808" },
            { 33, "Jass" },
            { 41, "Brush" },
            { 49, "Orchestra" },
            { 57, "SFX" },
        };
    }

    #region Windows API

    [DllImport("Winmm.dll")]
    private static extern uint midiOutGetNumDevs();

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private struct MidiOutCaps
	{
		public ushort	Mid;
		public ushort	Pid;
		public uint		DriverVersion;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string	Pname;
		public ushort	Technology;
		public ushort	Voices;
		public ushort	Notes;
		public ushort	ChannelMask;
		public uint		Support;
	}

	[DllImport("Winmm.dll")]
    private static extern uint midiOutGetDevCaps(
        uint aDeviceID, out MidiOutCaps aMidiOutCaps, int aMidiOutCapsLength );

    [DllImport("Winmm.dll")]
    private static extern uint midiOutOpen(
        out IntPtr aHmo, int aDeviceID, MidiOutProc? aCallback, int aCallbackInstance, uint aFlags );

    private delegate void MidiOutProc( IntPtr aHmo, uint aHwnd, int aInstance, int aParam1, int aParam2 );

    [DllImport("Winmm.dll")]
    private static extern uint midiOutShortMsg( IntPtr aHmo, int aMsg );

    [DllImport("Winmm.dll")]
    private static extern uint midiOutReset( IntPtr aHandle );

    [DllImport("Winmm.dll")]
    private static extern uint midiOutClose( IntPtr aHmo );

    #endregion
}
