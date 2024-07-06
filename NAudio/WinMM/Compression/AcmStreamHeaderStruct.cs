using System.Runtime.InteropServices;

namespace NAudio.WinMM.Compression;

/// <summary>
/// Interop structure for ACM stream headers.
/// ACMSTREAMHEADER 
/// http://msdn.microsoft.com/en-us/library/dd742926%28VS.85%29.aspx
/// </summary>    
[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = 128 )] // explicit size to make it work for x64
internal class AcmStreamHeaderStruct
{
    public int cbStruct;
    public AcmStreamHeaderStatusFlags fdwStatus = 0;
    public nint userData;
    public nint sourceBufferPointer;
    public int sourceBufferLength;
    public int sourceBufferLengthUsed;
    public nint sourceUserData;
    public nint destBufferPointer;
    public int destBufferLength;
    public int destBufferLengthUsed = 0;
    public nint destUserData;

    // 10 reserved values follow this, we don't need to declare them
    // since we have set the struct size explicitly and don't
    // need to access them in client code (thanks Brian)
    /*public int reserved0;
    public int reserved1;
    public int reserved2;
    public int reserved3;
    public int reserved4;
    public int reserved5;
    public int reserved6;
    public int reserved7;
    public int reserved8;
    public int reserved9;*/
}
