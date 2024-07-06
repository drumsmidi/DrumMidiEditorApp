using System.Runtime.InteropServices;
using NAudio.Core;

namespace NAudio.WinMM.Compression;

/// <summary>
/// Interop definitions for Windows ACM (Audio Compression Manager) API
/// </summary>
internal class AcmInterop
{
    // http://msdn.microsoft.com/en-us/library/dd742891%28VS.85%29.aspx
    public delegate bool AcmDriverEnumCallback( nint hAcmDriverId, nint instance, AcmDriverDetailsSupportFlags flags );

    public delegate bool AcmFormatEnumCallback( nint hAcmDriverId, ref AcmFormatDetails formatDetails, nint dwInstance, AcmDriverDetailsSupportFlags flags );

    public delegate bool AcmFormatTagEnumCallback( nint hAcmDriverId, ref AcmFormatTagDetails formatTagDetails, nint dwInstance, AcmDriverDetailsSupportFlags flags );

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/dd742910%28VS.85%29.aspx
    /// UINT ACMFORMATCHOOSEHOOKPROC acmFormatChooseHookProc(
    ///   HWND hwnd,     
    ///   UINT uMsg,     
    ///   WPARAM wParam, 
    ///   LPARAM lParam  
    /// </summary>        
    public delegate bool AcmFormatChooseHookProc( nint windowHandle, int message, nint wParam, nint lParam );

    // not done:
    // acmDriverAdd
    // acmDriverID
    // acmDriverMessage
    // acmDriverRemove
    // acmFilterChoose
    // acmFilterChooseHookProc
    // acmFilterDetails
    // acmFilterEnum -acmFilterEnumCallback
    // acmFilterTagDetails
    // acmFilterTagEnum
    // acmFormatDetails        
    // acmFormatTagDetails
    // acmGetVersion
    // acmStreamMessage

    // http://msdn.microsoft.com/en-us/library/windows/desktop/dd742885%28v=vs.85%29.aspx
    // MMRESULT acmDriverAdd(
    //        LPHACMDRIVERID phadid,
    //        HINSTANCE hinstModule,
    //        LPARAM lParam,
    //        DWORD dwPriority,
    //        DWORD fdwAdd)
    [DllImport( "msacm32.dll" )]
    public static extern MmResult acmDriverAdd( out nint driverHandle,
        nint driverModule,
        nint driverFunctionAddress,
        int priority,
        AcmDriverAddFlags flags );

    // http://msdn.microsoft.com/en-us/library/windows/desktop/dd742897%28v=vs.85%29.aspx
    [DllImport( "msacm32.dll" )]
    public static extern MmResult acmDriverRemove( nint driverHandle,
        int removeFlags );

    // http://msdn.microsoft.com/en-us/library/dd742886%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmDriverClose( nint hAcmDriver, int closeFlags );

    // http://msdn.microsoft.com/en-us/library/dd742890%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmDriverEnum( AcmDriverEnumCallback fnCallback, nint dwInstance, AcmDriverEnumFlags flags );

    // http://msdn.microsoft.com/en-us/library/dd742887%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmDriverDetails( nint hAcmDriver, ref AcmDriverDetails driverDetails, int reserved );

    // http://msdn.microsoft.com/en-us/library/dd742894%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmDriverOpen( out nint pAcmDriver, nint hAcmDriverId, int openFlags );

    // http://msdn.microsoft.com/en-us/library/dd742909%28VS.85%29.aspx
    [DllImport( "Msacm32.dll", EntryPoint = "acmFormatChooseW" )]
    public static extern MmResult acmFormatChoose( ref AcmFormatChoose formatChoose );

    // http://msdn.microsoft.com/en-us/library/dd742914%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmFormatEnum( nint hAcmDriver, ref AcmFormatDetails formatDetails, AcmFormatEnumCallback callback, nint instance, AcmFormatEnumFlags flags );

#if NET35
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/dd742916%28VS.85%29.aspx
    /// MMRESULT acmFormatSuggest(
    /// HACMDRIVER had,          
    /// LPWAVEFORMATEX pwfxSrc,  
    /// LPWAVEFORMATEX pwfxDst,  
    /// DWORD cbwfxDst,          
    /// DWORD fdwSuggest);
    /// </summary>
    [DllImport("Msacm32.dll")]
    public static extern MmResult acmFormatSuggest(
        IntPtr hAcmDriver,
        [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "NAudio.Wave.WaveFormatCustomMarshaler")] 
        WaveFormat sourceFormat,
        [In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "NAudio.Wave.WaveFormatCustomMarshaler")] 
        WaveFormat destFormat, 
        int sizeDestFormat, 
        AcmFormatSuggestFlags suggestFlags);
#endif

    [DllImport( "Msacm32.dll", EntryPoint = "acmFormatSuggest" )]
    public static extern MmResult acmFormatSuggest2(
        nint hAcmDriver,
        nint sourceFormatPointer,
        nint destFormatPointer,
        int sizeDestFormat,
        AcmFormatSuggestFlags suggestFlags );

    // http://msdn.microsoft.com/en-us/library/dd742919%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmFormatTagEnum( nint hAcmDriver, ref AcmFormatTagDetails formatTagDetails, AcmFormatTagEnumCallback callback, nint instance, int reserved );

    // http://msdn.microsoft.com/en-us/library/dd742922%28VS.85%29.aspx
    // this version of the prototype is for metrics that output a single integer
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmMetrics( nint hAcmObject, AcmMetrics metric, out int output );

#if NET35
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/dd742928%28VS.85%29.aspx
    /// MMRESULT acmStreamOpen(
    ///   LPHACMSTREAM    phas,       
    ///   HACMDRIVER      had,        
    ///   LPWAVEFORMATEX  pwfxSrc,    
    ///   LPWAVEFORMATEX  pwfxDst,    
    ///   LPWAVEFILTER    pwfltr,     
    ///   DWORD_PTR       dwCallback, 
    ///   DWORD_PTR       dwInstance, 
    ///   DWORD           fdwOpen     
    /// </summary>
    [DllImport("Msacm32.dll")]
    public static extern MmResult acmStreamOpen(
        out IntPtr hAcmStream, 
        IntPtr hAcmDriver, 
        [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "NAudio.Wave.WaveFormatCustomMarshaler")] 
        WaveFormat sourceFormat,
        [In, MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "NAudio.Wave.WaveFormatCustomMarshaler")] 
        WaveFormat destFormat, 
        [In] WaveFilter waveFilter, 
        IntPtr callback, 
        IntPtr instance, 
        AcmStreamOpenFlags openFlags);
#endif

    /// <summary>
    /// A version with pointers for troubleshooting
    /// </summary>
    [DllImport( "Msacm32.dll", EntryPoint = "acmStreamOpen" )]
    public static extern MmResult acmStreamOpen2(
        out nint hAcmStream,
        nint hAcmDriver,
        nint sourceFormatPointer,
        nint destFormatPointer,
        [In] WaveFilter waveFilter,
        nint callback,
        nint instance,
        AcmStreamOpenFlags openFlags );

    // http://msdn.microsoft.com/en-us/library/dd742923%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmStreamClose( nint hAcmStream, int closeFlags );

    // http://msdn.microsoft.com/en-us/library/dd742924%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmStreamConvert( nint hAcmStream, [In, Out] AcmStreamHeaderStruct streamHeader, AcmStreamConvertFlags streamConvertFlags );

    // http://msdn.microsoft.com/en-us/library/dd742929%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmStreamPrepareHeader( nint hAcmStream, [In, Out] AcmStreamHeaderStruct streamHeader, int prepareFlags );

    // http://msdn.microsoft.com/en-us/library/dd742929%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmStreamReset( nint hAcmStream, int resetFlags );

    // http://msdn.microsoft.com/en-us/library/dd742931%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmStreamSize( nint hAcmStream, int inputBufferSize, out int outputBufferSize, AcmStreamSizeFlags flags );

    // http://msdn.microsoft.com/en-us/library/dd742932%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    public static extern MmResult acmStreamUnprepareHeader( nint hAcmStream, [In, Out] AcmStreamHeaderStruct streamHeader, int flags );
}
