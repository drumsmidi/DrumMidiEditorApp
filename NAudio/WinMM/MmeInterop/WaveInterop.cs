using System;
using System.Runtime.InteropServices;
using NAudio.Core;
using NAudio.Core.Wave.WaveFormats;

namespace NAudio.WinMM.MmeInterop;

/// <summary>
/// MME Wave function interop
/// </summary>
public class WaveInterop
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
    /// Convert a mmio string to FOURCC
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern int mmioStringToFOURCC( [MarshalAs( UnmanagedType.LPStr )] string s, int flags );

    /// <summary>
    /// Get number of WaveOut devices
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern int waveOutGetNumDevs();
    /// <summary>
    /// Prepare wave out header
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutPrepareHeader( nint hWaveOut, WaveHeader lpWaveOutHdr, int uSize );
    /// <summary>
    /// Unprepare WaveOut header
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutUnprepareHeader( nint hWaveOut, WaveHeader lpWaveOutHdr, int uSize );
    /// <summary>
    /// Write to WaveOut device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutWrite( nint hWaveOut, WaveHeader lpWaveOutHdr, int uSize );

    // http://msdn.microsoft.com/en-us/library/dd743866%28VS.85%29.aspx
    /// <summary>
    /// Open WaveOut Device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutOpen( out nint hWaveOut, nint uDeviceID, WaveFormat lpFormat, WaveCallback dwCallback, nint dwInstance, WaveInOutOpenFlags dwFlags );

    /// <summary>
    /// Open WaveOut Device with window callback
    /// </summary>
    [DllImport( "winmm.dll", EntryPoint = "waveOutOpen" )]
    public static extern MmResult waveOutOpenWindow( out nint hWaveOut, nint uDeviceID, WaveFormat lpFormat, nint callbackWindowHandle, nint dwInstance, WaveInOutOpenFlags dwFlags );


    /// <summary>
    /// Reset WaveOut device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutReset( nint hWaveOut );

    /// <summary>
    /// Close WaveOut device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutClose( nint hWaveOut );

    /// <summary>
    /// Pause WaveOut device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutPause( nint hWaveOut );

    /// <summary>
    /// Restart WaveOut device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutRestart( nint hWaveOut );

    // http://msdn.microsoft.com/en-us/library/dd743863%28VS.85%29.aspx
    /// <summary>
    /// Get WaveOut device position
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutGetPosition( nint hWaveOut, ref MmTime mmTime, int uSize );

    // http://msdn.microsoft.com/en-us/library/dd743874%28VS.85%29.aspx
    /// <summary>
    /// Set WaveOut device volume
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutSetVolume( nint hWaveOut, int dwVolume );

    /// <summary>
    /// Get WaveOut device volume
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveOutGetVolume( nint hWaveOut, out int dwVolume );

    // http://msdn.microsoft.com/en-us/library/dd743857%28VS.85%29.aspx
    /// <summary>
    /// Get WaveOut device capabilities
    /// </summary>
    [DllImport( "winmm.dll", CharSet = CharSet.Auto )]
    public static extern MmResult waveOutGetDevCaps( nint deviceID, out WaveOutCapabilities waveOutCaps, int waveOutCapsSize );

    /// <summary>
    /// Get number of WaveIn devices
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern int waveInGetNumDevs();

    // http://msdn.microsoft.com/en-us/library/dd743841%28VS.85%29.aspx
    /// <summary>
    /// Get WaveIn Device capabilities
    /// </summary>
    [DllImport( "winmm.dll", CharSet = CharSet.Auto )]
    public static extern MmResult waveInGetDevCaps( nint deviceID, out WaveInCapabilities waveInCaps, int waveInCapsSize );

    // http://msdn.microsoft.com/en-us/library/dd743838%28VS.85%29.aspx
    /// <summary>
    /// Add WaveIn Buffer
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveInAddBuffer( nint hWaveIn, WaveHeader pwh, int cbwh );
    /// <summary>
    /// Close WaveIn device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveInClose( nint hWaveIn );

    // http://msdn.microsoft.com/en-us/library/dd743847%28VS.85%29.aspx
    /// <summary>
    /// Open WaveIn Device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveInOpen( out nint hWaveIn, nint uDeviceID, WaveFormat lpFormat, WaveCallback dwCallback, nint dwInstance, WaveInOutOpenFlags dwFlags );

    /// <summary>
    /// Open WaveIn Device with Window callbacks
    /// </summary>
    [DllImport( "winmm.dll", EntryPoint = "waveInOpen" )]
    public static extern MmResult waveInOpenWindow( out nint hWaveIn, nint uDeviceID, WaveFormat lpFormat, nint callbackWindowHandle, nint dwInstance, WaveInOutOpenFlags dwFlags );

    // http://msdn.microsoft.com/en-us/library/dd743848%28VS.85%29.aspx
    /// <summary>
    /// Prepare WaveIn Header
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveInPrepareHeader( nint hWaveIn, WaveHeader lpWaveInHdr, int uSize );

    /// <summary>
    /// Unprepare WaveIn Header
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveInUnprepareHeader( nint hWaveIn, WaveHeader lpWaveInHdr, int uSize );

    // http://msdn.microsoft.com/en-us/library/dd743850%28VS.85%29.aspx
    /// <summary>
    /// Reset WaveIn Device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveInReset( nint hWaveIn );

    // http://msdn.microsoft.com/en-us/library/dd743851%28VS.85%29.aspx
    /// <summary>
    /// Start WaveIn device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveInStart( nint hWaveIn );

    // http://msdn.microsoft.com/en-us/library/dd743852%28VS.85%29.aspx
    /// <summary>
    /// Stop WaveIn Device
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveInStop( nint hWaveIn );

    // https://msdn.microsoft.com/en-us/library/Dd743845(v=VS.85).aspx
    /// <summary>
    /// Get WaveIn Device Position
    /// </summary>
    [DllImport( "winmm.dll" )]
    public static extern MmResult waveInGetPosition( nint hWaveIn, out MmTime mmTime, int uSize );


}
