using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace DrumMidiEditor.pGeneralFunction.pWinApi;

internal class WinApi
{
    #region Window

    [StructLayout(LayoutKind.Sequential)]
	public struct Message
	{
		public IntPtr	Hwnd;
		public uint		Msg;
		public IntPtr	wParam;
		public IntPtr	lParam;
		public uint		Time;
		public Point	p;
	}

	[SuppressUnmanagedCodeSecurity]
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool PeekMessage( 
		out Message aMsg, IntPtr aHwnd, uint aMsgFilterMin, uint aMsgFilterMax, uint aFlags );

    #endregion

    #region MIDI

    [DllImport("Winmm.dll")]
    public static extern uint midiOutGetNumDevs();

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public struct MidiOutCaps
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
    public static extern uint midiOutGetDevCaps(
        uint aDeviceID, out MidiOutCaps aMidiOutCaps, int aMidiOutCapsLength );

    [DllImport("Winmm.dll")]
    public static extern uint midiOutOpen(
        out IntPtr aHmo, int aDeviceID, MidiOutProc? aCallback, int aCallbackInstance, uint aFlags );

    public delegate void MidiOutProc( IntPtr aHmo, uint aHwnd, int aInstance, int aParam1, int aParam2 );

    [DllImport("Winmm.dll")]
    public static extern uint midiOutShortMsg( IntPtr aHmo, int aMsg );

    [DllImport("Winmm.dll")]
    public static extern uint midiOutReset( IntPtr aHandle );

    [DllImport("Winmm.dll")]
    public static extern uint midiOutClose( IntPtr aHmo );

    #endregion
}
