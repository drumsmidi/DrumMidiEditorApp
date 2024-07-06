// created on 13/12/2002 at 22:01
using System.Runtime.InteropServices;
using NAudio.Core;

namespace NAudio.WinMM.Mixer;

/// <summary>
/// Represents a signed mixer control
/// </summary>
public class SignedMixerControl : MixerControl
{
    private MixerInterop.MIXERCONTROLDETAILS_SIGNED signedDetails;

    internal SignedMixerControl( MixerInterop.MIXERCONTROL mixerControl, nint mixerHandle, MixerFlags mixerHandleType, int nChannels )
    {
        this.mixerControl = mixerControl;
        this.mixerHandle = mixerHandle;
        this.mixerHandleType = mixerHandleType;
        this.nChannels = nChannels;
        mixerControlDetails = new MixerInterop.MIXERCONTROLDETAILS();
        GetControlDetails();
    }

    /// <summary>
    /// Gets details for this contrl
    /// </summary>
    protected override void GetDetails( nint pDetails ) => signedDetails = Marshal.PtrToStructure<MixerInterop.MIXERCONTROLDETAILS_SIGNED>( mixerControlDetails.paDetails );

    /// <summary>
    /// The value of the control
    /// </summary>
    public int Value
    {
        get
        {
            GetControlDetails();
            return signedDetails.lValue;
        }
        set
        {
            signedDetails.lValue = value;
            mixerControlDetails.paDetails = Marshal.AllocHGlobal( Marshal.SizeOf( signedDetails ) );
            Marshal.StructureToPtr( signedDetails, mixerControlDetails.paDetails, false );
            MmException.Try( MixerInterop.mixerSetControlDetails( mixerHandle, ref mixerControlDetails, MixerFlags.Value | mixerHandleType ), "mixerSetControlDetails" );
            Marshal.FreeHGlobal( mixerControlDetails.paDetails );
        }
    }

    /// <summary>
    /// Minimum value for this control
    /// </summary>
    public int MinValue => mixerControl.Bounds.minimum;

    /// <summary>
    /// Maximum value for this control
    /// </summary>
    public int MaxValue => mixerControl.Bounds.maximum;

    /// <summary>
    /// Value of the control represented as a percentage
    /// </summary>
    public double Percent
    {
        get => 100.0 * ( Value - MinValue ) / ( MaxValue - MinValue );
        set => Value = (int)( MinValue + ( value / 100.0 * ( MaxValue - MinValue ) ) );
    }

    /// <summary>
    /// String Representation for debugging purposes
    /// </summary>
    /// <returns></returns>
    public override string ToString() => string.Format( "{0} {1}%", base.ToString(), Percent );
}
