using System;
using System.Runtime.InteropServices;
using NAudio.Core.Utils;
using NAudio.Core;
using NAudio.Core.Wave.WaveOutputs;
using NAudio.WinMM.MmeInterop;

namespace NAudio.WinMM;

/// <summary>
/// A buffer of Wave samples for streaming to a Wave Output device
/// </summary>
public partial class WaveOutBuffer : DisposeBaseClass
{
    private readonly WaveHeader header;
    private readonly byte[] buffer;
    private readonly IWaveProvider waveStream;
    private readonly object waveOutLock;
    private GCHandle hBuffer;
    private nint hWaveOut;
    private GCHandle hHeader; // we need to pin the header structure
    private GCHandle hThis; // for the user callback

    /// <summary>
    /// creates a new wavebuffer
    /// </summary>
    /// <param name="hWaveOut">WaveOut device to write to</param>
    /// <param name="bufferSize">Buffer size in bytes</param>
    /// <param name="bufferFillStream">Stream to provide more data</param>
    /// <param name="waveOutLock">Lock to protect WaveOut API's from being called on >1 thread</param>
    public WaveOutBuffer( nint hWaveOut, int bufferSize, IWaveProvider bufferFillStream, object waveOutLock )
    {
        BufferSize          = bufferSize;
        buffer              = new byte [ bufferSize ];
        hBuffer             = GCHandle.Alloc( buffer, GCHandleType.Pinned );
        this.hWaveOut       = hWaveOut;
        waveStream          = bufferFillStream;
        this.waveOutLock    = waveOutLock;

        header              = new WaveHeader();
        hHeader             = GCHandle.Alloc( header, GCHandleType.Pinned );
        header.dataBuffer   = hBuffer.AddrOfPinnedObject();
        header.bufferLength = bufferSize;
        header.loops        = 1;
        hThis               = GCHandle.Alloc( this );
        header.userData     = (nint)hThis;

        lock ( waveOutLock )
        {
            MmException.Try( WaveInterop.waveOutPrepareHeader( hWaveOut, header, Marshal.SizeOf( header ) ), "waveOutPrepareHeader" );
        }
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                //component.Dispose();
            }

            // Dispose unmanaged resources.
            // free unmanaged resources
            if ( hHeader.IsAllocated )
            {
                hHeader.Free();
            }

            if ( hBuffer.IsAllocated )
            {
                hBuffer.Free();
            }

            if ( hThis.IsAllocated )
            {
                hThis.Free();
            }

            if ( hWaveOut != nint.Zero )
            {
                lock ( waveOutLock )
                {
                    _ = WaveInterop.waveOutUnprepareHeader( hWaveOut, header, Marshal.SizeOf( header ) );
                }
                hWaveOut = nint.Zero;
            }

            // Note disposing has been done.
            _Disposed = true;
        }
    }
    private bool _Disposed = false;

    /// this is called by the WAVE callback and should be used to refill the buffer
    public bool OnDone()
    {
        int bytes;
        lock ( waveStream )
        {
            bytes = waveStream.Read( buffer, 0, buffer.Length );
        }
        if ( bytes == 0 )
        {
            return false;
        }
        for ( var n = bytes; n < buffer.Length; n++ )
        {
            buffer [ n ] = 0;
        }
        WriteToWaveOut();
        return true;
    }

    /// <summary>
    /// Whether the header's in queue flag is set
    /// </summary>
    public bool InQueue => ( header.flags & WaveHeaderFlags.InQueue ) == WaveHeaderFlags.InQueue;

    /// <summary>
    /// The buffer size in bytes
    /// </summary>
    public int BufferSize
    {
        get;
    }

    private void WriteToWaveOut()
    {
        MmResult result;

        lock ( waveOutLock )
        {
            result = WaveInterop.waveOutWrite( hWaveOut, header, Marshal.SizeOf( header ) );
        }
        if ( result != MmResult.NoError )
        {
            throw new MmException( result, "waveOutWrite" );
        }

        GC.KeepAlive( this );
    }
}
