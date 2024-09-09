using System;
using System.Runtime.InteropServices;
using NAudio.Core;
using NAudio.Core.Wave.WaveFormats;

namespace NAudio.WinMM.MmeInterop;

/// <summary>
/// MME Wave function interop
/// </summary>
public partial class WaveInterop
{
    /// <summary>
    /// WaveInOut Open Flags
    /// </summary>
    [Flags]
    public enum WaveInOutOpenFlags
    {
        /// <summary>
        /// CALLBACK_NULL
        /// No callback
        /// </summary>
        CallbackNull = 0,
        /// <summary>
        /// CALLBACK_FUNCTION
        /// dwCallback is a FARPROC 
        /// </summary>
        CallbackFunction = 0x30000,
        /// <summary>
        /// CALLBACK_EVENT
        /// dwCallback is an EVENT handle 
        /// </summary>
        CallbackEvent = 0x50000,
        /// <summary>
        /// CALLBACK_WINDOW
        /// dwCallback is a HWND 
        /// </summary>
        CallbackWindow = 0x10000,
        /// <summary>
        /// CALLBACK_THREAD
        /// callback is a thread ID 
        /// </summary>
        CallbackThread = 0x20000,
        /*
        WAVE_FORMAT_QUERY = 1,
        WAVE_MAPPED = 4,
        WAVE_FORMAT_DIRECT = 8*/
    }

    //public const int TIME_MS = 0x0001;  // time in milliseconds 
    //public const int TIME_SAMPLES = 0x0002;  // number of wave samples 
    //public const int TIME_BYTES = 0x0004;  // current byte offset 

    /// <summary>
    /// Wave Message
    /// </summary>
    public enum WaveMessage
    {
        /// <summary>
        /// WIM_OPEN
        /// </summary>
        WaveInOpen = 0x3BE,
        /// <summary>
        /// WIM_CLOSE
        /// </summary>
        WaveInClose = 0x3BF,
        /// <summary>
        /// WIM_DATA
        /// </summary>
        WaveInData = 0x3C0,

        /// <summary>
        /// WOM_CLOSE
        /// </summary>
        WaveOutClose = 0x3BC,
        /// <summary>
        /// WOM_DONE
        /// </summary>
        WaveOutDone = 0x3BD,
        /// <summary>
        /// WOM_OPEN
        /// </summary>
        WaveOutOpen = 0x3BB
    }

    // use the userdata as a reference
    // WaveOutProc http://msdn.microsoft.com/en-us/library/dd743869%28VS.85%29.aspx
    // WaveInProc http://msdn.microsoft.com/en-us/library/dd743849%28VS.85%29.aspx
    /// <summary>
    /// Wave Callback
    /// </summary>
    public delegate void WaveCallback( nint hWaveOut, WaveMessage message, nint dwInstance, WaveHeader wavhdr, nint dwReserved );

    /// <summary>
    /// Prepare wave out header
    /// </summary>
    [DllImport( "winmm.dll" )]
    internal static extern MmResult waveOutPrepareHeader( nint hWaveOut, WaveHeader lpWaveOutHdr, int uSize );
    /// <summary>
    /// Unprepare WaveOut header
    /// </summary>
    [DllImport( "winmm.dll" )]
    internal static extern MmResult waveOutUnprepareHeader( nint hWaveOut, WaveHeader lpWaveOutHdr, int uSize );
    /// <summary>
    /// Write to WaveOut device
    /// </summary>
    [DllImport( "winmm.dll" )]
    internal static extern MmResult waveOutWrite( nint hWaveOut, WaveHeader lpWaveOutHdr, int uSize );

    /// <summary>
    /// Open WaveOut Device with window callback
    /// </summary>
    [DllImport( "winmm.dll", EntryPoint = "waveOutOpen" )]
    internal static extern MmResult waveOutOpenWindow( out nint hWaveOut, nint uDeviceID, WaveFormat lpFormat, nint callbackWindowHandle, nint dwInstance, WaveInOutOpenFlags dwFlags );

    /// <summary>
    /// Reset WaveOut device
    /// </summary>
    [LibraryImport( "winmm.dll" )]
    internal static partial MmResult waveOutReset( nint hWaveOut );

    /// <summary>
    /// Close WaveOut device
    /// </summary>
    [LibraryImport( "winmm.dll" )]
    internal static partial MmResult waveOutClose( nint hWaveOut );

    /// <summary>
    /// Pause WaveOut device
    /// </summary>
    [LibraryImport( "winmm.dll" )]
    internal static partial MmResult waveOutPause( nint hWaveOut );

    /// <summary>
    /// Restart WaveOut device
    /// </summary>
    [LibraryImport( "winmm.dll" )]
    internal static partial MmResult waveOutRestart( nint hWaveOut );

    // http://msdn.microsoft.com/en-us/library/dd743863%28VS.85%29.aspx
    /// <summary>
    /// Get WaveOut device position
    /// </summary>
    [LibraryImport( "winmm.dll" )]
    internal static partial MmResult waveOutGetPosition( nint hWaveOut, ref MmTime mmTime, int uSize );

    // http://msdn.microsoft.com/en-us/library/dd743874%28VS.85%29.aspx
    /// <summary>
    /// Set WaveOut device volume
    /// </summary>
    [LibraryImport( "winmm.dll" )]
    internal static partial MmResult waveOutSetVolume( nint hWaveOut, int dwVolume );

    /// <summary>
    /// Get WaveOut device volume
    /// </summary>
    [LibraryImport( "winmm.dll" )]
    internal static partial MmResult waveOutGetVolume( nint hWaveOut, out int dwVolume );
}
