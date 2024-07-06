// created on 13/12/2002 at 22:04
using System.Runtime.InteropServices;
using NAudio.Core;

namespace NAudio.WinMM.Mixer;

/// <summary>
/// Represents an unsigned mixer control
/// </summary>
public class UnsignedMixerControl : MixerControl
{
    private MixerInterop.MIXERCONTROLDETAILS_UNSIGNED[] unsignedDetails;

    internal UnsignedMixerControl( MixerInterop.MIXERCONTROL mixerControl, nint mixerHandle, MixerFlags mixerHandleType, int nChannels )
    {
        this.mixerControl = mixerControl;
        this.mixerHandle = mixerHandle;
        this.mixerHandleType = mixerHandleType;
        this.nChannels = nChannels;
        mixerControlDetails = new MixerInterop.MIXERCONTROLDETAILS();
        GetControlDetails();
    }

    /// <summary>
    /// Gets the details for this control
    /// </summary>
    protected override void GetDetails( nint pDetails )
    {
        unsignedDetails = new MixerInterop.MIXERCONTROLDETAILS_UNSIGNED [ nChannels ];
        for ( var channel = 0; channel < nChannels; channel++ )
        {
            unsignedDetails [ channel ] = Marshal.PtrToStructure<MixerInterop.MIXERCONTROLDETAILS_UNSIGNED>( mixerControlDetails.paDetails );
        }
    }

    /// <summary>
    /// The control value
    /// </summary>
    public uint Value
    {
        get
        {
            GetControlDetails();
            return unsignedDetails [ 0 ].dwValue;
        }
        set
        {
            var structSize = Marshal.SizeOf(unsignedDetails[0]);

            mixerControlDetails.paDetails = Marshal.AllocHGlobal( structSize * nChannels );
            for ( var channel = 0; channel < nChannels; channel++ )
            {
                unsignedDetails [ channel ].dwValue = value;
                var pointer = mixerControlDetails.paDetails.ToInt64() + (structSize * channel);
                Marshal.StructureToPtr( unsignedDetails [ channel ], (nint)pointer, false );
            }
            MmException.Try( MixerInterop.mixerSetControlDetails( mixerHandle, ref mixerControlDetails, MixerFlags.Value | mixerHandleType ), "mixerSetControlDetails" );
            Marshal.FreeHGlobal( mixerControlDetails.paDetails );
        }
    }

    /// <summary>
    /// The control's minimum value
    /// </summary>
    public uint MinValue => (uint)mixerControl.Bounds.minimum;

    /// <summary>
    /// The control's maximum value
    /// </summary>
    public uint MaxValue => (uint)mixerControl.Bounds.maximum;

    /// <summary>
    /// Value of the control represented as a percentage
    /// </summary>
    public double Percent
    {
        get => 100.0 * ( Value - MinValue ) / ( MaxValue - MinValue );
        set => Value = (uint)( MinValue + ( value / 100.0 * ( MaxValue - MinValue ) ) );
    }

    /// <summary>
    /// String Representation for debugging purposes
    /// </summary>
    public override string ToString() => string.Format( "{0} {1}%", base.ToString(), Percent );
}
