using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;

namespace DrumMidiLibrary.pAudio;

/// <summary>
/// システム音再生
/// </summary>
public static partial class SystemSound
{
    #region 音声ON/OFF切替

    /// <summary>
    /// システム音を再生するウィンドウ
    /// </summary>
    private static Window? _AccessWindow { get; set; } = null;

    /// <summary>
    /// システム音を有効化
    /// </summary>
    /// <param name="aWindow"></param>
    public static void SoundOn( Window aWindow )
    {
        _AccessWindow = aWindow;
        ElementSoundPlayer.State = ElementSoundPlayerState.On;
    }

    /// <summary>
    /// システム音を無効化
    /// </summary>
    public static void SoundOff() 
    {
        _AccessWindow = null;
        ElementSoundPlayer.State = ElementSoundPlayerState.Off;
    }

    #endregion

    #region SoundPlay

    /// <summary>
    /// システム音を再生
    /// </summary>
    /// <param name="aElementSoundKind"></param>
    private static void SoundPlay( ElementSoundKind aElementSoundKind )
    {
        if ( _AccessWindow == null )
        {
            return;
        }

        if ( !HelperXaml.DispatcherQueueHasThreadAccess
                ( _AccessWindow, () => { SoundPlay( aElementSoundKind ); } ) )
        {
            return;
        }

        Log.TryCatch
        (
            () =>
            {
                if ( ElementSoundPlayer.State == ElementSoundPlayerState.On )
                {
                    ElementSoundPlayer.Play( aElementSoundKind );
                }
            }
        );
    }

    public static void SoundPlayMoveNext() => SoundPlay( ElementSoundKind.MoveNext );

    public static void SoundPlayMovePrevious() => SoundPlay( ElementSoundKind.MovePrevious );

    public static void SoundPlayFocus() => SoundPlay( ElementSoundKind.Focus );

    public static void SoundPlayGoBack() => SoundPlay( ElementSoundKind.GoBack );

    #endregion
}
