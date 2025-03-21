namespace DrumMidiLibrary.pAudio;

/// <summary>
/// Audioインターフェース
/// </summary>
public interface IAudio
{
    /// <summary>
    /// Audio リリース処理
    /// </summary>
    void Release();

    /// <summary>
    /// AudioData取得
    /// </summary>
    /// <returns>Audioデータ</returns>
    object? GetAudioData();

    /// <summary>
    /// 現在の再生時間（秒）を取得
    /// </summary>
    /// <returns>現在の再生時間（秒）</returns>
    double GetCurrentTime();

    /// <summary>
    /// 現在の再生時間位置（秒）を設定
    /// </summary>
    /// <param name="aCurrentTime">再生時間位置（秒）</param>
    void SetCurrentTime( double aCurrentTime );

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns>音量</returns>
    int GetVolume();

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="aVolume">音量</param>
    void SetVolume( int aVolume );

    /// <summary>
    /// 再生
    /// </summary>
    void Play();

    /// <summary>
    /// 一時停止
    /// </summary>
    void Pause();

    /// <summary>
    /// 停止
    /// </summary>
    void Stop();
}
