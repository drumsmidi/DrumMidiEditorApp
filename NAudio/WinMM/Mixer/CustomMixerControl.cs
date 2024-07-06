// created on 13/12/2002 at 22:07
namespace NAudio.WinMM.Mixer;

/// <summary>
/// Custom Mixer control
/// </summary>
public class CustomMixerControl : MixerControl
{
    internal CustomMixerControl( MixerInterop.MIXERCONTROL mixerControl, nint mixerHandle, MixerFlags mixerHandleType, int nChannels )
    {
        this.mixerControl = mixerControl;
        this.mixerHandle = mixerHandle;
        this.mixerHandleType = mixerHandleType;
        this.nChannels = nChannels;
        mixerControlDetails = new MixerInterop.MIXERCONTROLDETAILS();
        GetControlDetails();
    }

    /// <summary>
    /// Get the data for this custom control
    /// </summary>
    /// <param name="pDetails">pointer to memory to receive data</param>
    protected override void GetDetails( nint pDetails )
    {
    }

    // TODO: provide a way of getting / setting data
}
