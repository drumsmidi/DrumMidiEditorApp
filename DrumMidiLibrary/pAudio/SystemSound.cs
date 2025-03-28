using Microsoft.UI.Xaml;

namespace DrumMidiLibrary.pAudio;

/// <summary>
/// システム音再生
/// </summary>
public static partial class SystemSound
{
    public static void SoundOn()
        => ElementSoundPlayer.State = ElementSoundPlayerState.On;

    public static void SoundOff() 
        => ElementSoundPlayer.State = ElementSoundPlayerState.Off;

    public static void SoundPlay( ElementSoundKind aElementSoundKind )
        => ElementSoundPlayer.Play( aElementSoundKind );

    public static void SoundPlayMoveNext()
        => SoundPlay( ElementSoundKind.MoveNext );

    public static void SoundPlayMovePrevious()
        => SoundPlay( ElementSoundKind.MovePrevious );

    public static void SoundPlayFocus()
        => SoundPlay( ElementSoundKind.Focus );

    public static void SoundPlayGoBack()
        => SoundPlay( ElementSoundKind.GoBack );
}
