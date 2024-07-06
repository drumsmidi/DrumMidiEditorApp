using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NAudio.Core;

namespace NAudio.WinMM.Mixer;

/// <summary>Represents a Windows mixer device</summary>
public class Mixer
{
    private MixerInterop.MIXERCAPS caps;
    private readonly nint mixerHandle;
    private readonly MixerFlags mixerHandleType;

    /// <summary>The number of mixer devices available</summary>	
    public static int NumberOfDevices => MixerInterop.mixerGetNumDevs();

    /// <summary>Connects to the specified mixer</summary>
    /// <param name="mixerIndex">The index of the mixer to use. 
    /// This should be between zero and NumberOfDevices - 1</param>
    public Mixer( int mixerIndex )
    {
        if ( mixerIndex < 0 || mixerIndex >= NumberOfDevices )
        {
            throw new ArgumentOutOfRangeException( "mixerID" );
        }
        caps = new MixerInterop.MIXERCAPS();
        MmException.Try( MixerInterop.mixerGetDevCaps( mixerIndex, ref caps, Marshal.SizeOf( caps ) ), "mixerGetDevCaps" );
        mixerHandle = mixerIndex;
        mixerHandleType = MixerFlags.Mixer;

        // TODO: optionally support really opening the mixer device
        //MmException.Try(MixerInterop.mixerOpen(out mixerHandle, mixerIndex, IntPtr.Zero, IntPtr.Zero, 0), "mixerOpen");
    }

    /// <summary>The number of destinations this mixer supports</summary>
    public int DestinationCount => (int)caps.cDestinations;

    /// <summary>The name of this mixer device</summary>
    public string Name => caps.szPname;

    /// <summary>The manufacturer code for this mixer device</summary>
    public Manufacturers Manufacturer => (Manufacturers)caps.wMid;

    /// <summary>The product identifier code for this mixer device</summary>
    public int ProductID => caps.wPid;

    /// <summary>Retrieve the specified MixerDestination object</summary>
    /// <param name="destinationIndex">The ID of the destination to use.
    /// Should be between 0 and DestinationCount - 1</param>
    public MixerLine GetDestination( int destinationIndex )
    {
        return destinationIndex < 0 || destinationIndex >= DestinationCount
            ? throw new ArgumentOutOfRangeException( "destinationIndex" )
            : new MixerLine( mixerHandle, destinationIndex, mixerHandleType );
    }

    /// <summary>
    /// A way to enumerate the destinations
    /// </summary>
    public IEnumerable<MixerLine> Destinations
    {
        get
        {
            for ( var destination = 0; destination < DestinationCount; destination++ )
            {
                yield return GetDestination( destination );
            }
        }
    }

    /// <summary>
    /// A way to enumerate all available devices
    /// </summary>
    public static IEnumerable<Mixer> Mixers
    {
        get
        {
            for ( var device = 0; device < NumberOfDevices; device++ )
            {
                yield return new Mixer( device );
            }
        }
    }
}
