// created on 10/12/2002 at 20:37
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NAudio.Core;

namespace NAudio.WinMM.Mixer;

/// <summary>
/// Represents a mixer line (source or destination)
/// </summary>
public class MixerLine
{
    private MixerInterop.MIXERLINE mixerLine;
    private readonly nint mixerHandle;
    private readonly MixerFlags mixerHandleType;

    /// <summary>
    /// Creates a new mixer destination
    /// </summary>
    /// <param name="mixerHandle">Mixer Handle</param>
    /// <param name="destinationIndex">Destination Index</param>
    /// <param name="mixerHandleType">Mixer Handle Type</param>
    public MixerLine( nint mixerHandle, int destinationIndex, MixerFlags mixerHandleType )
    {
        this.mixerHandle = mixerHandle;
        this.mixerHandleType = mixerHandleType;
        mixerLine = new MixerInterop.MIXERLINE();
        mixerLine.cbStruct = Marshal.SizeOf( mixerLine );
        mixerLine.dwDestination = destinationIndex;
        MmException.Try( MixerInterop.mixerGetLineInfo( mixerHandle, ref mixerLine, mixerHandleType | MixerFlags.GetLineInfoOfDestination ), "mixerGetLineInfo" );
    }

    /// <summary>
    /// Creates a new Mixer Source For a Specified Source
    /// </summary>
    /// <param name="mixerHandle">Mixer Handle</param>
    /// <param name="destinationIndex">Destination Index</param>
    /// <param name="sourceIndex">Source Index</param>
    /// <param name="mixerHandleType">Flag indicating the meaning of mixerHandle</param>
    public MixerLine( nint mixerHandle, int destinationIndex, int sourceIndex, MixerFlags mixerHandleType )
    {
        this.mixerHandle = mixerHandle;
        this.mixerHandleType = mixerHandleType;
        mixerLine = new MixerInterop.MIXERLINE();
        mixerLine.cbStruct = Marshal.SizeOf( mixerLine );
        mixerLine.dwDestination = destinationIndex;
        mixerLine.dwSource = sourceIndex;
        MmException.Try( MixerInterop.mixerGetLineInfo( mixerHandle, ref mixerLine, mixerHandleType | MixerFlags.GetLineInfoOfSource ), "mixerGetLineInfo" );
    }

    /// <summary>
    /// Creates a new Mixer Source
    /// </summary>
    /// <param name="waveInDevice">Wave In Device</param>
    public static int GetMixerIdForWaveIn( int waveInDevice )
    {
        int mixerId;
        MmException.Try( MixerInterop.mixerGetID( waveInDevice, out mixerId, MixerFlags.WaveIn ), "mixerGetID" );
        return mixerId;
    }

    /// <summary>
    /// Mixer Line Name
    /// </summary>
    public string Name => mixerLine.szName;

    /// <summary>
    /// Mixer Line short name
    /// </summary>
    public string ShortName => mixerLine.szShortName;

    /// <summary>
    /// The line ID
    /// </summary>
    public int LineId => mixerLine.dwLineID;

    /// <summary>
    /// Component Type
    /// </summary>
    public MixerLineComponentType ComponentType => mixerLine.dwComponentType;

    /// <summary>
    /// Mixer destination type description
    /// </summary>
    public string TypeDescription => mixerLine.dwComponentType switch
    {
        // destinations
        MixerLineComponentType.DestinationUndefined => "Undefined Destination",
        MixerLineComponentType.DestinationDigital => "Digital Destination",
        MixerLineComponentType.DestinationLine => "Line Level Destination",
        MixerLineComponentType.DestinationMonitor => "Monitor Destination",
        MixerLineComponentType.DestinationSpeakers => "Speakers Destination",
        MixerLineComponentType.DestinationHeadphones => "Headphones Destination",
        MixerLineComponentType.DestinationTelephone => "Telephone Destination",
        MixerLineComponentType.DestinationWaveIn => "Wave Input Destination",
        MixerLineComponentType.DestinationVoiceIn => "Voice Recognition Destination",
        // sources
        MixerLineComponentType.SourceUndefined => "Undefined Source",
        MixerLineComponentType.SourceDigital => "Digital Source",
        MixerLineComponentType.SourceLine => "Line Level Source",
        MixerLineComponentType.SourceMicrophone => "Microphone Source",
        MixerLineComponentType.SourceSynthesizer => "Synthesizer Source",
        MixerLineComponentType.SourceCompactDisc => "Compact Disk Source",
        MixerLineComponentType.SourceTelephone => "Telephone Source",
        MixerLineComponentType.SourcePcSpeaker => "PC Speaker Source",
        MixerLineComponentType.SourceWaveOut => "Wave Out Source",
        MixerLineComponentType.SourceAuxiliary => "Auxiliary Source",
        MixerLineComponentType.SourceAnalog => "Analog Source",
        _ => "Invalid Component Type",
    };

    /// <summary>
    /// Number of channels
    /// </summary>
    public int Channels => mixerLine.cChannels;

    /// <summary>
    /// Number of sources
    /// </summary>
    public int SourceCount => mixerLine.cConnections;

    /// <summary>
    /// Number of controls
    /// </summary>
    public int ControlsCount => mixerLine.cControls;

    /// <summary>
    /// Is this destination active
    /// </summary>
    public bool IsActive => ( mixerLine.fdwLine & MixerInterop.MIXERLINE_LINEF.MIXERLINE_LINEF_ACTIVE ) != 0;

    /// <summary>
    /// Is this destination disconnected
    /// </summary>
    public bool IsDisconnected => ( mixerLine.fdwLine & MixerInterop.MIXERLINE_LINEF.MIXERLINE_LINEF_DISCONNECTED ) != 0;

    /// <summary>
    /// Is this destination a source
    /// </summary>
    public bool IsSource => ( mixerLine.fdwLine & MixerInterop.MIXERLINE_LINEF.MIXERLINE_LINEF_SOURCE ) != 0;

    /// <summary>
    /// Gets the specified source
    /// </summary>
    public MixerLine GetSource( int sourceIndex )
    {
        return sourceIndex < 0 || sourceIndex >= SourceCount
            ? throw new ArgumentOutOfRangeException( "sourceIndex" )
            : new MixerLine( mixerHandle, mixerLine.dwDestination, sourceIndex, mixerHandleType );
    }

    /// <summary>
    /// Enumerator for the controls on this Mixer Limne
    /// </summary>
    public IEnumerable<MixerControl> Controls => MixerControl.GetMixerControls( mixerHandle, this, mixerHandleType );

    /// <summary>
    /// Enumerator for the sources on this Mixer Line
    /// </summary>
    public IEnumerable<MixerLine> Sources
    {
        get
        {
            for ( var source = 0; source < SourceCount; source++ )
            {
                yield return GetSource( source );
            }
        }
    }

    /// <summary>
    /// The name of the target output device
    /// </summary>
    public string TargetName => mixerLine.szPname;

    /// <summary>
    /// Describes this Mixer Line (for diagnostic purposes)
    /// </summary>
    public override string ToString()
    {
        return string.Format( "{0} {1} ({2} controls, ID={3})",
            Name, TypeDescription, ControlsCount, mixerLine.dwLineID );
    }
}

