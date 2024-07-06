// created on 10/12/2002 at 21:11
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NAudio.Core;

namespace NAudio.WinMM.Mixer;

/// <summary>
/// Represents a mixer control
/// </summary>
public abstract class MixerControl
{
    internal MixerInterop.MIXERCONTROL mixerControl;
    internal MixerInterop.MIXERCONTROLDETAILS mixerControlDetails;

    /// <summary>
    /// Mixer Handle
    /// </summary>
    protected nint mixerHandle;

    /// <summary>
    /// Number of Channels
    /// </summary>
    protected int nChannels;

    /// <summary>
    /// Mixer Handle Type
    /// </summary>
    protected MixerFlags mixerHandleType;

    /// <summary>
    /// Gets all the mixer controls
    /// </summary>
    /// <param name="mixerHandle">Mixer Handle</param>
    /// <param name="mixerLine">Mixer Line</param>
    /// <param name="mixerHandleType">Mixer Handle Type</param>
    /// <returns></returns>
    public static IList<MixerControl> GetMixerControls( nint mixerHandle, MixerLine mixerLine,
                                                       MixerFlags mixerHandleType )
    {
        var controls = new List<MixerControl>();
        if ( mixerLine.ControlsCount > 0 )
        {
            var mixerControlSize = Marshal.SizeOf<MixerInterop.MIXERCONTROL>();
            var mlc = new MixerInterop.MIXERLINECONTROLS();
            var pmc = Marshal.AllocHGlobal(mixerControlSize * mixerLine.ControlsCount);
            mlc.cbStruct = Marshal.SizeOf( mlc );
            mlc.dwLineID = mixerLine.LineId;
            mlc.cControls = mixerLine.ControlsCount;
            mlc.pamxctrl = pmc;
            mlc.cbmxctrl = Marshal.SizeOf<MixerInterop.MIXERCONTROL>();
            try
            {
                var err = MixerInterop.mixerGetLineControls(mixerHandle, ref mlc, MixerFlags.All | mixerHandleType);
                if ( err != MmResult.NoError )
                {
                    throw new MmException( err, "mixerGetLineControls" );
                }
                for ( var i = 0; i < mlc.cControls; i++ )
                {
                    var address = pmc.ToInt64() + (mixerControlSize * i);

                    var mc =
                        Marshal.PtrToStructure<MixerInterop.MIXERCONTROL>((nint)address);
                    var mixerControl = GetMixerControl(mixerHandle, mixerLine.LineId, mc.dwControlID, mixerLine.Channels,
                                                                             mixerHandleType);

                    controls.Add( mixerControl );
                }
            }
            finally
            {
                Marshal.FreeHGlobal( pmc );
            }

        }
        return controls;
    }

    /// <summary>
    /// Gets a specified Mixer Control
    /// </summary>
    /// <param name="mixerHandle">Mixer Handle</param>
    /// <param name="nLineId">Line ID</param>
    /// <param name="controlId">Control ID</param>
    /// <param name="nChannels">Number of Channels</param>
    /// <param name="mixerFlags">Flags to use (indicates the meaning of mixerHandle)</param>
    /// <returns></returns>
    public static MixerControl GetMixerControl( nint mixerHandle, int nLineId, int controlId, int nChannels,
                                               MixerFlags mixerFlags )
    {
        var mlc = new MixerInterop.MIXERLINECONTROLS();
        var mc = new MixerInterop.MIXERCONTROL();

        // set up the pointer to a structure
        var pMixerControl = Marshal.AllocCoTaskMem(Marshal.SizeOf(mc));
        //Marshal.StructureToPtr(mc, pMixerControl, false);      

        mlc.cbStruct = Marshal.SizeOf( mlc );
        mlc.cControls = 1;
        mlc.dwControlID = controlId;
        mlc.cbmxctrl = Marshal.SizeOf( mc );
        mlc.pamxctrl = pMixerControl;
        mlc.dwLineID = nLineId;
        var err = MixerInterop.mixerGetLineControls(mixerHandle, ref mlc, MixerFlags.OneById | mixerFlags);
        if ( err != MmResult.NoError )
        {
            Marshal.FreeCoTaskMem( pMixerControl );
            throw new MmException( err, "mixerGetLineControls" );
        }

        // retrieve the structure from the pointer
        mc = Marshal.PtrToStructure<MixerInterop.MIXERCONTROL>( mlc.pamxctrl );
        Marshal.FreeCoTaskMem( pMixerControl );

        if ( IsControlBoolean( mc.dwControlType ) )
        {
            return new BooleanMixerControl( mc, mixerHandle, mixerFlags, nChannels );
        }

        if ( IsControlSigned( mc.dwControlType ) )
        {
            return new SignedMixerControl( mc, mixerHandle, mixerFlags, nChannels );
        }

        if ( IsControlUnsigned( mc.dwControlType ) )
        {
            return new UnsignedMixerControl( mc, mixerHandle, mixerFlags, nChannels );
        }

        return IsControlListText( mc.dwControlType )
            ? new ListTextMixerControl( mc, mixerHandle, mixerFlags, nChannels )
            : IsControlCustom( mc.dwControlType )
            ? (MixerControl)new CustomMixerControl( mc, mixerHandle, mixerFlags, nChannels )
            : throw new InvalidOperationException( $"Unknown mixer control type {mc.dwControlType}" );
    }

    /// <summary>
    /// Gets the control details
    /// </summary>
    protected void GetControlDetails()
    {
        mixerControlDetails.cbStruct = Marshal.SizeOf( mixerControlDetails );
        mixerControlDetails.dwControlID = mixerControl.dwControlID;
        mixerControlDetails.cChannels = IsCustom ? 0 : ( mixerControl.fdwControl & MixerInterop.MIXERCONTROL_CONTROLF_UNIFORM ) != 0 ? 1 : nChannels;


        if ( ( mixerControl.fdwControl & MixerInterop.MIXERCONTROL_CONTROLF_MULTIPLE ) != 0 )
        {
            mixerControlDetails.hwndOwner = (nint)mixerControl.cMultipleItems;
        }
        else if ( IsCustom )
        {
            mixerControlDetails.hwndOwner = nint.Zero; // TODO: special cases
        }
        else
        {
            mixerControlDetails.hwndOwner = nint.Zero;
        }

        if ( IsBoolean )
        {
            mixerControlDetails.cbDetails = Marshal.SizeOf<MixerInterop.MIXERCONTROLDETAILS_BOOLEAN>();
        }
        else if ( IsListText )
        {
            mixerControlDetails.cbDetails = Marshal.SizeOf<MixerInterop.MIXERCONTROLDETAILS_LISTTEXT>();
        }
        else if ( IsSigned )
        {
            mixerControlDetails.cbDetails = Marshal.SizeOf<MixerInterop.MIXERCONTROLDETAILS_SIGNED>();
        }
        else if ( IsUnsigned )
        {
            mixerControlDetails.cbDetails = Marshal.SizeOf<MixerInterop.MIXERCONTROLDETAILS_UNSIGNED>();
        }
        else
        {
            // must be custom
            mixerControlDetails.cbDetails = mixerControl.Metrics.customData;
        }
        var detailsSize = mixerControlDetails.cbDetails*mixerControlDetails.cChannels;
        if ( ( mixerControl.fdwControl & MixerInterop.MIXERCONTROL_CONTROLF_MULTIPLE ) != 0 )
        {
            // fixing issue 16390 - calculating size correctly for multiple items
            detailsSize *= (int)mixerControl.cMultipleItems;
        }
        var buffer = Marshal.AllocCoTaskMem(detailsSize);
        // To copy stuff in:
        // Marshal.StructureToPtr( theStruct, buffer, false );
        mixerControlDetails.paDetails = buffer;
        var err = MixerInterop.mixerGetControlDetails(mixerHandle, ref mixerControlDetails,
                                                           MixerFlags.Value | mixerHandleType);
        // let the derived classes get the details before we free the handle			
        if ( err == MmResult.NoError )
        {
            GetDetails( mixerControlDetails.paDetails );
        }
        Marshal.FreeCoTaskMem( buffer );
        if ( err != MmResult.NoError )
        {
            throw new MmException( err, "mixerGetControlDetails" );
        }
    }

    /// <summary>
    /// Gets the control details
    /// </summary>
    /// <param name="pDetails"></param>
    protected abstract void GetDetails( nint pDetails );

    /// <summary>
    /// Mixer control name
    /// </summary>
    public string Name => mixerControl.szName;

    /// <summary>
    /// Mixer control type
    /// </summary>
    public MixerControlType ControlType => mixerControl.dwControlType;

    /// <summary>
    /// Returns true if this is a boolean control
    /// </summary>
    /// <param name="controlType">Control type</param>
    private static bool IsControlBoolean( MixerControlType controlType )
    {
        return controlType switch
        {
            MixerControlType.BooleanMeter or MixerControlType.Boolean or MixerControlType.Button or MixerControlType.Loudness or MixerControlType.Mono or MixerControlType.Mute or MixerControlType.OnOff or MixerControlType.StereoEnhance or MixerControlType.Mixer or MixerControlType.MultipleSelect or MixerControlType.Mux or MixerControlType.SingleSelect => true,
            _ => false,
        };
    }

    /// <summary>
    /// Is this a boolean control
    /// </summary>
    public bool IsBoolean => IsControlBoolean( mixerControl.dwControlType );

    /// <summary>
    /// Determines whether a specified mixer control type is a list text control
    /// </summary>
    private static bool IsControlListText( MixerControlType controlType )
    {
        return controlType switch
        {
            MixerControlType.Equalizer or MixerControlType.Mixer or MixerControlType.MultipleSelect or MixerControlType.Mux or MixerControlType.SingleSelect => true,
            _ => false,
        };
    }

    /// <summary>
    /// True if this is a list text control
    /// </summary>
    public bool IsListText => IsControlListText( mixerControl.dwControlType );

    private static bool IsControlSigned( MixerControlType controlType )
    {
        return controlType switch
        {
            MixerControlType.PeakMeter or MixerControlType.SignedMeter or MixerControlType.Signed or MixerControlType.Decibels or MixerControlType.Pan or MixerControlType.QSoundPan or MixerControlType.Slider => true,
            _ => false,
        };
    }

    /// <summary>
    /// True if this is a signed control
    /// </summary>
    public bool IsSigned => IsControlSigned( mixerControl.dwControlType );

    private static bool IsControlUnsigned( MixerControlType controlType )
    {
        return controlType switch
        {
            MixerControlType.UnsignedMeter or MixerControlType.Unsigned or MixerControlType.Bass or MixerControlType.Equalizer or MixerControlType.Fader or MixerControlType.Treble or MixerControlType.Volume or MixerControlType.MicroTime or MixerControlType.MilliTime or MixerControlType.Percent => true,
            _ => false,
        };
    }

    /// <summary>
    /// True if this is an unsigned control
    /// </summary>
    public bool IsUnsigned => IsControlUnsigned( mixerControl.dwControlType );

    private static bool IsControlCustom( MixerControlType controlType ) => controlType == MixerControlType.Custom;

    /// <summary>
    /// True if this is a custom control
    /// </summary>
    public bool IsCustom => IsControlCustom( mixerControl.dwControlType );

    /// <summary>
    /// String representation for debug purposes
    /// </summary>
    public override string ToString() => $"{Name} {ControlType}";
}
