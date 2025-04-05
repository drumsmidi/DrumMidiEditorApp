using Microsoft.UI.Xaml;

namespace DrumMidiLibrary.pAudio;

/// <summary>
/// システム音再生
/// </summary>
public static partial class SystemSound
{
    /// <summary>
    /// システム音を有効化
    /// </summary>
    public static void SoundOn()
        => ElementSoundPlayer.State = ElementSoundPlayerState.On;

    /// <summary>
    /// システム音を無効化
    /// </summary>
    public static void SoundOff() 
        => ElementSoundPlayer.State = ElementSoundPlayerState.Off;

    /// <summary>
    /// システム音を再生
    /// </summary>
    /// <param name="aElementSoundKind"></param>
    private static void SoundPlay( ElementSoundKind aElementSoundKind )
    {
        // NOTE:非同期処理中は実行できない。DispatcherQueue的な対応が必要そう
        // ElementSoundPlayer.Play( aElementSoundKind );
    }

    public static void SoundPlayMoveNext() => SoundPlay( ElementSoundKind.MoveNext );

    public static void SoundPlayMovePrevious() => SoundPlay( ElementSoundKind.MovePrevious );

    public static void SoundPlayFocus() => SoundPlay( ElementSoundKind.Focus );

    public static void SoundPlayGoBack() => SoundPlay( ElementSoundKind.GoBack );
}
