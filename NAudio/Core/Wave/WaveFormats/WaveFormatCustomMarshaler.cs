using System;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace NAudio.Core.Wave.WaveFormats;

/// <summary>
/// Custom marshaller for WaveFormat structures
/// </summary>
public sealed class WaveFormatCustomMarshaler : ICustomMarshaler
{
    private static WaveFormatCustomMarshaler marshaler = null;

    /// <summary>
    /// Gets the instance of this marshaller
    /// </summary>
    /// <param name="cookie"></param>
    /// <returns></returns>
    public static ICustomMarshaler GetInstance( string cookie )
    {
        marshaler ??= new WaveFormatCustomMarshaler();
        return marshaler;
    }

    /// <summary>
    /// Clean up managed data
    /// </summary>
    public void CleanUpManagedData( object ManagedObj )
    {

    }

    /// <summary>
    /// Clean up native data
    /// </summary>
    /// <param name="pNativeData"></param>
    public void CleanUpNativeData( nint pNativeData ) => Marshal.FreeHGlobal( pNativeData );

    /// <summary>
    /// Get native data size
    /// </summary>        
    public int GetNativeDataSize() => throw new NotImplementedException();

    /// <summary>
    /// Marshal managed to native
    /// </summary>
    public nint MarshalManagedToNative( object ManagedObj ) => WaveFormat.MarshalToPtr( (WaveFormat)ManagedObj );

    /// <summary>
    /// Marshal Native to Managed
    /// </summary>
    public object MarshalNativeToManaged( nint pNativeData ) => WaveFormat.MarshalFromPtr( pNativeData );
}
