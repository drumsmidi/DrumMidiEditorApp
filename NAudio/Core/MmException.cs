using System;

namespace NAudio.Core;

/// <summary>
/// Summary description for MmException.
/// </summary>
/// <remarks>
/// Creates a new MmException
/// </remarks>
/// <param name="result">The result returned by the Windows API call</param>
/// <param name="function">The name of the Windows API that failed</param>
public class MmException( MmResult result, string function ) : Exception( ErrorMessage( result, function ) )
{
    private static string ErrorMessage( MmResult result, string function ) => $"{result} calling {function}";

    /// <summary>
    /// Helper function to automatically raise an exception on failure
    /// </summary>
    /// <param name="result">The result of the API call</param>
    /// <param name="function">The API function name</param>
    public static void Try( MmResult result, string function )
    {
        if ( result != MmResult.NoError )
        {
            throw new MmException( result, function );
        }
    }

    /// <summary>
    /// Returns the Windows API result
    /// </summary>
    public MmResult Result
    {
        get;
    } = result;

    /// <summary>
    /// The function being called
    /// </summary>
    public string Function
    {
        get;
    } = function;

}
