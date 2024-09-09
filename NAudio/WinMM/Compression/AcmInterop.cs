using System.Runtime.InteropServices;
using NAudio.Core;

namespace NAudio.WinMM.Compression;

/// <summary>
/// Interop definitions for Windows ACM (Audio Compression Manager) API
/// </summary>
internal partial class AcmInterop
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

    // http://msdn.microsoft.com/en-us/library/dd742886%28VS.85%29.aspx
    [LibraryImport( "Msacm32.dll" )]
    internal static partial MmResult acmDriverClose( nint hAcmDriver, int closeFlags );

    [LibraryImport( "Msacm32.dll", EntryPoint = "acmFormatSuggest" )]
    internal static partial MmResult acmFormatSuggest2(
        nint hAcmDriver,
        nint sourceFormatPointer,
        nint destFormatPointer,
        int sizeDestFormat,
        AcmFormatSuggestFlags suggestFlags );

    /// <summary>
    /// A version with pointers for troubleshooting
    /// </summary>
    [DllImport( "Msacm32.dll", EntryPoint = "acmStreamOpen" )]
    internal static extern MmResult acmStreamOpen2(
        out nint hAcmStream,
        nint hAcmDriver,
        nint sourceFormatPointer,
        nint destFormatPointer,
        [In] WaveFilter waveFilter,
        nint callback,
        nint instance,
        AcmStreamOpenFlags openFlags );

    // http://msdn.microsoft.com/en-us/library/dd742923%28VS.85%29.aspx
    [LibraryImport( "Msacm32.dll" )]
    internal static partial MmResult acmStreamClose( nint hAcmStream, int closeFlags );

    // http://msdn.microsoft.com/en-us/library/dd742924%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    internal static extern MmResult acmStreamConvert( nint hAcmStream, [In, Out] AcmStreamHeaderStruct streamHeader, AcmStreamConvertFlags streamConvertFlags );

    // http://msdn.microsoft.com/en-us/library/dd742929%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    internal static extern MmResult acmStreamPrepareHeader( nint hAcmStream, [In, Out] AcmStreamHeaderStruct streamHeader, int prepareFlags );

    // http://msdn.microsoft.com/en-us/library/dd742931%28VS.85%29.aspx
    [LibraryImport( "Msacm32.dll" )]
    internal static partial MmResult acmStreamSize( nint hAcmStream, int inputBufferSize, out int outputBufferSize, AcmStreamSizeFlags flags );

    // http://msdn.microsoft.com/en-us/library/dd742932%28VS.85%29.aspx
    [DllImport( "Msacm32.dll" )]
    internal static extern MmResult acmStreamUnprepareHeader( nint hAcmStream, [In, Out] AcmStreamHeaderStruct streamHeader, int flags );
}
