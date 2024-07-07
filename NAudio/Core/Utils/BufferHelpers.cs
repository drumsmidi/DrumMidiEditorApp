namespace NAudio.Core.Utils;

/// <summary>
/// Helper methods for working with audio buffers
/// </summary>
public static class BufferHelpers
{
    /// <summary>
    /// Ensures the buffer is big enough
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="bytesRequired"></param>
    /// <returns></returns>
    public static byte [] Ensure( byte [] buffer, int bytesRequired )
    {
        if ( buffer == null || buffer.Length < bytesRequired )
        {
            buffer = new byte [ bytesRequired ];
        }
        return buffer;
    }
}
