using System;
using System.Runtime.InteropServices;
using NAudio.Core;

namespace NAudio.WinMM.Compression;

internal partial class AcmStreamHeader : IDisposable
{
    private readonly AcmStreamHeaderStruct streamHeader;
    private GCHandle hSourceBuffer;
    private GCHandle hDestBuffer;
    private readonly nint streamHandle;
    private bool firstTime;

    public AcmStreamHeader( nint streamHandle, int sourceBufferLength, int destBufferLength )
    {
        streamHeader = new AcmStreamHeaderStruct();
        SourceBuffer = new byte [ sourceBufferLength ];
        hSourceBuffer = GCHandle.Alloc( SourceBuffer, GCHandleType.Pinned );

        DestBuffer = new byte [ destBufferLength ];
        hDestBuffer = GCHandle.Alloc( DestBuffer, GCHandleType.Pinned );

        this.streamHandle = streamHandle;
        firstTime = true;
        //Prepare();
    }

    private void Prepare()
    {
        streamHeader.cbStruct = Marshal.SizeOf( streamHeader );
        streamHeader.sourceBufferLength = SourceBuffer.Length;
        streamHeader.sourceBufferPointer = hSourceBuffer.AddrOfPinnedObject();
        streamHeader.destBufferLength = DestBuffer.Length;
        streamHeader.destBufferPointer = hDestBuffer.AddrOfPinnedObject();
        MmException.Try( AcmInterop.acmStreamPrepareHeader( streamHandle, streamHeader, 0 ), "acmStreamPrepareHeader" );
    }

    private void Unprepare()
    {
        streamHeader.sourceBufferLength = SourceBuffer.Length;
        streamHeader.sourceBufferPointer = hSourceBuffer.AddrOfPinnedObject();
        streamHeader.destBufferLength = DestBuffer.Length;
        streamHeader.destBufferPointer = hDestBuffer.AddrOfPinnedObject();

        var result = AcmInterop.acmStreamUnprepareHeader(streamHandle, streamHeader, 0);
        if ( result != MmResult.NoError )
        {
            //if (result == MmResult.AcmHeaderUnprepared)
            throw new MmException( result, "acmStreamUnprepareHeader" );
        }
    }

    public void Reposition() => firstTime = true;

    public int Convert( int bytesToConvert, out int sourceBytesConverted )
    {
        Prepare();
        try
        {
            streamHeader.sourceBufferLength = bytesToConvert;
            streamHeader.sourceBufferLengthUsed = bytesToConvert;
            var flags = firstTime ? AcmStreamConvertFlags.Start | AcmStreamConvertFlags.BlockAlign : AcmStreamConvertFlags.BlockAlign;
            MmException.Try( AcmInterop.acmStreamConvert( streamHandle, streamHeader, flags ), "acmStreamConvert" );
            firstTime = false;
            System.Diagnostics.Debug.Assert( streamHeader.destBufferLength == DestBuffer.Length, "Codecs should not change dest buffer length" );
            sourceBytesConverted = streamHeader.sourceBufferLengthUsed;
        }
        finally
        {
            Unprepare();
        }

        return streamHeader.destBufferLengthUsed;
    }

    public byte [] SourceBuffer
    {
        get; private set;
    }

    public byte [] DestBuffer
    {
        get; private set;
    }

    #region IDisposable Members

    private bool disposed = false;

    public void Dispose()
    {
        GC.SuppressFinalize( this );
        Dispose( true );
    }

    protected virtual void Dispose( bool disposing )
    {
        if ( !disposed )
        {
            //Unprepare();
            SourceBuffer = null;
            DestBuffer = null;
            hSourceBuffer.Free();
            hDestBuffer.Free();
        }
        disposed = true;
    }

    ~AcmStreamHeader()
    {
        System.Diagnostics.Debug.Assert( false, "AcmStreamHeader dispose was not called" );
        Dispose( false );
    }
    #endregion
}
