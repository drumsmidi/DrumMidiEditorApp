using System.Runtime.InteropServices;

namespace NAudio.WinMM.Compression;

/// <summary>
/// Interop structure for ACM driver details (ACMDRIVERDETAILS)
/// http://msdn.microsoft.com/en-us/library/dd742889%28VS.85%29.aspx
/// </summary>
[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2 )]
internal struct AcmDriverDetails
{
    /// <summary>
    /// DWORD cbStruct
    /// </summary>
    public int structureSize;
    /// <summary>
    /// FOURCC fccType
    /// </summary>
    public uint fccType;
    /// <summary>
    /// FOURCC fccComp
    /// </summary>
    public uint fccComp;
    /// <summary>
    /// WORD   wMid; 
    /// </summary>
    public ushort manufacturerId;
    /// <summary>
    /// WORD wPid
    /// </summary>
    public ushort productId;
    /// <summary>
    /// DWORD vdwACM
    /// </summary>
    public uint acmVersion;
    /// <summary>
    /// DWORD vdwDriver
    /// </summary>
    public uint driverVersion;
    /// <summary>
    /// DWORD  fdwSupport;
    /// </summary>
    public AcmDriverDetailsSupportFlags supportFlags;
    /// <summary>
    /// DWORD cFormatTags
    /// </summary>
    public int formatTagsCount;
    /// <summary>
    /// DWORD cFilterTags
    /// </summary>
    public int filterTagsCount;
    /// <summary>
    /// HICON hicon
    /// </summary>
    public nint hicon;
    /// <summary>
    /// TCHAR  szShortName[ACMDRIVERDETAILS_SHORTNAME_CHARS]; 
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ShortNameChars)]
    public string shortName;
    /// <summary>
    /// TCHAR  szLongName[ACMDRIVERDETAILS_LONGNAME_CHARS];
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LongNameChars)]
    public string longName;
    /// <summary>
    /// TCHAR  szCopyright[ACMDRIVERDETAILS_COPYRIGHT_CHARS]; 
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CopyrightChars)]
    public string copyright;
    /// <summary>
    /// TCHAR  szLicensing[ACMDRIVERDETAILS_LICENSING_CHARS]; 
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = LicensingChars)]
    public string licensing;
    /// <summary>
    /// TCHAR  szFeatures[ACMDRIVERDETAILS_FEATURES_CHARS];
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FeaturesChars)]
    public string features;

    /// <summary>
    /// ACMDRIVERDETAILS_SHORTNAME_CHARS
    /// </summary>
    private const int ShortNameChars = 32;
    /// <summary>
    /// ACMDRIVERDETAILS_LONGNAME_CHARS
    /// </summary>
    private const int LongNameChars = 128;
    /// <summary>
    /// ACMDRIVERDETAILS_COPYRIGHT_CHARS
    /// </summary>
    private const int CopyrightChars = 80;
    /// <summary>
    /// ACMDRIVERDETAILS_LICENSING_CHARS 
    /// </summary>
    private const int LicensingChars = 128;
    /// <summary>
    /// ACMDRIVERDETAILS_FEATURES_CHARS
    /// </summary>
    private const int FeaturesChars = 512;
}
