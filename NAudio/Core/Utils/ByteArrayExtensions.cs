using System;
using System.Text;

namespace NAudio.Core.Utils;

/// <summary>
/// these will become extension methods once we move to .NET 3.5
/// </summary>
public static class ByteArrayExtensions
{
    /// <summary>
    /// Concatenates the given arrays into a single array.
    /// </summary>
    /// <param name="byteArrays">The arrays to concatenate</param>
    /// <returns>The concatenated resulting array.</returns>
    public static byte [] Concat( params byte [] [] byteArrays )
    {
        var size = 0;
        foreach ( var btArray in byteArrays )
        {
            size += btArray.Length;
        }

        if ( size <= 0 )
        {
            return [];
        }

        var result = new byte[size];
        var idx = 0;
        foreach ( var btArray in byteArrays )
        {
            Array.Copy( btArray, 0, result, idx, btArray.Length );
            idx += btArray.Length;
        }

        return result;
    }
}
