using System;
using System.Runtime.InteropServices;
using NAudio.Core;
using NAudio.WinMM.MmeInterop;

// ReSharper disable once CheckNamespace
namespace NAudio.WinMM;

/// <summary>
/// WaveOutUtils
/// </summary>
public static class WaveOutUtils
{
    /// <summary>
    /// Get WaveOut Volume
    /// </summary>
    public static float GetWaveOutVolume( nint hWaveOut, object lockObject )
    {
        int stereoVolume;
        MmResult result;
        lock ( lockObject )
        {
            result = WaveInterop.waveOutGetVolume( hWaveOut, out stereoVolume );
        }
        MmException.Try( result, "waveOutGetVolume" );
        return ( stereoVolume & 0xFFFF ) / (float)0xFFFF;
    }

    /// <summary>
    /// Set WaveOut Volume
    /// </summary>
    public static void SetWaveOutVolume( float value, nint hWaveOut, object lockObject )
    {
        if ( value < 0 )
        {
            throw new ArgumentOutOfRangeException( nameof( value ), "Volume must be between 0.0 and 1.0" );
        }

        if ( value > 1 )
        {
            throw new ArgumentOutOfRangeException( nameof( value ), "Volume must be between 0.0 and 1.0" );
        }

        var left = value;
        var right = value;

        var stereoVolume = (int)(left * 0xFFFF) + ((int)(right * 0xFFFF) << 16);
        MmResult result;
        lock ( lockObject )
        {
            result = WaveInterop.waveOutSetVolume( hWaveOut, stereoVolume );
        }
        MmException.Try( result, "waveOutSetVolume" );
    }

    /// <summary>
    /// Get position in bytes
    /// </summary>
    public static long GetPositionBytes( nint hWaveOut, object lockObject )
    {
        lock ( lockObject )
        {
            var mmTime = new MmTime
            {
                wType = MmTime.TIME_BYTES // request results in bytes, TODO: perhaps make this a little more flexible and support the other types?
            };
            MmException.Try( WaveInterop.waveOutGetPosition( hWaveOut, ref mmTime, Marshal.SizeOf( mmTime ) ), "waveOutGetPosition" );

            return mmTime.wType != MmTime.TIME_BYTES
                ? throw new Exception( string.Format( "waveOutGetPosition: wType -> Expected {0}, Received {1}", MmTime.TIME_BYTES, mmTime.wType ) )
                : mmTime.cb;
        }
    }
}
