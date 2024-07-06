using System;
using System.Runtime.InteropServices;
using NAudio.Core;
using NAudio.WinMM.MmeInterop;

// ReSharper disable once CheckNamespace
namespace NAudio.WinMM;

/// <summary>
/// A buffer of Wave samples
/// </summary>
public class WaveInBuffer : IDisposable
{
    private readonly WaveHeader header;
    private GCHandle hBuffer;
    private nint waveInHandle;
    private GCHandle hHeader; // we need to pin the header structure
    private GCHandle hThis; // for the user callback

    /// <summary>
    /// creates a new wavebuffer
    /// </summary>
    /// <param name="waveInHandle">WaveIn device to write to</param>
    /// <param name="bufferSize">Buffer size in bytes</param>
    public WaveInBuffer( nint waveInHandle, int bufferSize )
    {
        BufferSize = bufferSize;
        Data = new byte [ bufferSize ];
        hBuffer = GCHandle.Alloc( Data, GCHandleType.Pinned );
        this.waveInHandle = waveInHandle;

        header = new WaveHeader();
        hHeader = GCHandle.Alloc( header, GCHandleType.Pinned );
        header.dataBuffer = hBuffer.AddrOfPinnedObject();
        header.bufferLength = bufferSize;
        header.loops = 1;
        hThis = GCHandle.Alloc( this );
        header.userData = (nint)hThis;

        MmException.Try( WaveInterop.waveInPrepareHeader( waveInHandle, header, Marshal.SizeOf( header ) ), "waveInPrepareHeader" );
        //MmException.Try(WaveInterop.waveInAddBuffer(waveInHandle, header, Marshal.SizeOf(header)), "waveInAddBuffer");
    }

    /// <summary>
    /// Place this buffer back to record more audio
    /// </summary>
    public void Reuse()
    {
        // TEST: we might not actually need to bother unpreparing and repreparing
        MmException.Try( WaveInterop.waveInUnprepareHeader( waveInHandle, header, Marshal.SizeOf( header ) ), "waveUnprepareHeader" );
        MmException.Try( WaveInterop.waveInPrepareHeader( waveInHandle, header, Marshal.SizeOf( header ) ), "waveInPrepareHeader" );
        //System.Diagnostics.Debug.Assert(header.bytesRecorded == 0, "bytes recorded was not reset properly");
        MmException.Try( WaveInterop.waveInAddBuffer( waveInHandle, header, Marshal.SizeOf( header ) ), "waveInAddBuffer" );
    }

    #region Dispose Pattern

    /// <summary>
    /// Finalizer for this wave buffer
    /// </summary>
    ~WaveInBuffer()
    {
        Dispose( false );
        System.Diagnostics.Debug.Assert( true, "WaveInBuffer was not disposed" );
    }

    /// <summary>
    /// Releases resources held by this WaveBuffer
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize( this );
        Dispose( true );
    }

    /// <summary>
    /// Releases resources held by this WaveBuffer
    /// </summary>
    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            // free managed resources
        }
        // free unmanaged resources
        if ( waveInHandle != nint.Zero )
        {
            _ = WaveInterop.waveInUnprepareHeader( waveInHandle, header, Marshal.SizeOf( header ) );
            waveInHandle = nint.Zero;
        }
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
    }

    #endregion

    /// <summary>
    /// Provides access to the actual record buffer (for reading only)
    /// </summary>
    public byte [] Data
    {
        get;
    }

    /// <summary>
    /// Indicates whether the Done flag is set on this buffer
    /// </summary>
    public bool Done => ( header.flags & WaveHeaderFlags.Done ) == WaveHeaderFlags.Done;


    /// <summary>
    /// Indicates whether the InQueue flag is set on this buffer
    /// </summary>
    public bool InQueue => ( header.flags & WaveHeaderFlags.InQueue ) == WaveHeaderFlags.InQueue;

    /// <summary>
    /// Number of bytes recorded
    /// </summary>
    public int BytesRecorded => header.bytesRecorded;

    /// <summary>
    /// The buffer size in bytes
    /// </summary>
    public int BufferSize
    {
        get;
    }
}
