using System;
using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pUtil;

namespace DrumMidiClassLibrary.pAudio;

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
        IAudio audio;

        try
        {
            audio = new AudioMidi( aChannel, aMidi );
            audio.SetVolume( aVolume );
        }
        catch ( Exception e )
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}" );

            audio = new AudioNull();
        }
        return audio;
    }

    /// <summary>
    /// AudioBgmデータ作成
    /// </summary>
    /// <param name="aFilePath">Audioファイルパス</param>
    /// <param name="aVolume">音量（127基準）</param>
    /// <returns>Audioデータ</returns>
    public static IAudio CreateBgm( GeneralPath aFilePath, int aVolume )
    {
        IAudio audio;

        try
        {
            audio = new AudioBgm( aFilePath );
            audio.SetVolume( aVolume );
        }
        catch ( Exception e )
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}" );

            audio = new AudioNull();
        }
        return audio;
    }

    /// <summary>
    /// Audio リリース処理
    /// </summary>
    /// <param name="aAudio">Audioデータ</param>
    public static void Release( IAudio? aAudio )
    {
        try
        {
            ( aAudio as AudioBase )?.Dispose();
        }
        catch ( Exception e )
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MIDIノート単発再生
    /// </summary>
    /// <param name="aChannel">MIDIチェンネル番号(0-15)</param>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <param name="aVolume">音量（127基準）</param>
    public static void SinglePlay( byte aChannel, byte aMidi, int aVolume )
    {
        try
        {
            using var audio = CreateMidi( aChannel, aMidi, aVolume ) as AudioBase;

            audio?.Play();
        }
        catch ( Exception e )
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
