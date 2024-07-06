using System.Runtime.InteropServices;

namespace NAudio.Core.Utils;

/// <summary>
/// General purpose native methods for internal NAudio use
/// </summary>
public static class NativeMethods
{
    /// <summary>
    /// Loads a DLL
    /// </summary>
    [DllImport( "kernel32.dll" )]
    public static extern nint LoadLibrary( string dllToLoad );

    /// <summary>
    /// Get procedure address
    /// </summary>
    [DllImport( "kernel32.dll" )]
    public static extern nint GetProcAddress( nint hModule, string procedureName );


    /// <summary>
    /// Free a library
    /// </summary>
    [DllImport( "kernel32.dll" )]
    public static extern bool FreeLibrary( nint hModule );
}
