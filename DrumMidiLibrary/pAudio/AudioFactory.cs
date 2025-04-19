using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pAudio;

/// <summary>
/// Audioデータ作成支援
/// </summary>
public class AudioFactory
{
    /// <summary>
    /// AudioMidiデータ作成
    /// </summary>
    /// <param name="aChannel">MIDIチェンネル番号(0-15)</param>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <param name="aVolume">音量（127基準）</param>
    /// <returns>Audioデータ</returns>
    public static IAudio CreateMidi( byte aChannel, byte aMidi, int aVolume )
    {
        return Log.TryCatchNotNull<IAudio>
        (
            () =>
            {
                var audio = new AudioMidi( aChannel, aMidi );
                audio.SetVolume( aVolume );

                return audio;
            },
            ( e ) => new AudioNull()
        );
    }

    /// <summary>
    /// AudioBgmデータ作成
    /// </summary>
    /// <param name="aFilePath">Audioファイルパス</param>
    /// <param name="aVolume">音量（100基準）</param>
    /// <returns>Audioデータ</returns>
    public static IAudio CreateBgm( GeneralPath aFilePath, int aVolume )
    {
        return Log.TryCatchNotNull<IAudio>
        (
            () =>
            {
                var audio = new AudioBgm( aFilePath );
                audio.SetVolume( aVolume );

                return audio;
            },
            ( e ) => new AudioNull()
        );
    }

    /// <summary>
    /// Audio リリース処理
    /// </summary>
    /// <param name="aAudio">Audioデータ</param>
    public static void Release( IAudio? aAudio )
    {
        Log.TryCatch
        (
            () => aAudio?.Release()
        );
    }

    /// <summary>
    /// MIDIノート単発再生
    /// </summary>
    /// <param name="aChannel">MIDIチェンネル番号(0-15)</param>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <param name="aVolume">音量（127基準）</param>
    public static void SinglePlay( byte aChannel, byte aMidi, int aVolume )
    {
        Log.TryCatch
        (
            () =>
            {
                using var audio = CreateMidi( aChannel, aMidi, aVolume ) as AudioMidi;

                audio?.Play();
            }
        );
    }
}
