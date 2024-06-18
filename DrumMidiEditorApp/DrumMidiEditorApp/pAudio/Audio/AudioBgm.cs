using DrumMidiEditorApp.pUtil;

namespace DrumMidiEditorApp.pAudio;

/// <summary>
/// Audio BGM
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aFilepath">Audioファイルパス</param>
internal class AudioBgm( GeneralPath aFilepath ) : AudioBase
{
    /// <summary>
    /// AudioData
    /// </summary>
	private readonly NAudioData? _AudioData = new( aFilepath );

    public override void Release() => Dispose();

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                Stop();
                _AudioData?.Dispose();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    public override object? GetAudioData() => _AudioData;

    public override double GetCurrentTime()
        => _AudioData?.GetCurrentTime() ?? 0 ;

    public override void SetCurrentTime( double aCurrentTime )
        => _AudioData?.SetCurrentTime( aCurrentTime );

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns>音量（100基準）</returns>
    public override int GetVolume()
        => _AudioData?.GetVolume() ?? NAudioData.MinVolume ;

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="aVolume">音量（100基準）</param>
    public override void SetVolume( int aVolume )
        => _AudioData?.SetVolume( aVolume );

    public override void Play() => _AudioData?.Play();

    public override void Pause() => _AudioData?.Pause();

    public override void Stop() => _AudioData?.Stop();
}
