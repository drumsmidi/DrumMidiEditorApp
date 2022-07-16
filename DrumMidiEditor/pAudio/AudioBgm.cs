using DrumMidiEditor.pConfig;
using DrumMidiEditor.pGeneralFunction.pAudio;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pAudio;

/// <summary>
/// Audio BGM
/// </summary>
internal class AudioBgm : AudioBase
{
    /// <summary>
    /// AudioData
    /// </summary>
	private readonly NAudioData? _AudioData = null;

    /// <summary>
    /// AudioData参照（周波数解析などで使用）
    /// </summary>
    public NAudioData? AudioData => _AudioData;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aFilepath">Audioファイルパス</param>
    public AudioBgm( GeneralPath aFilepath )
    {
        _AudioData = new NAudioData( aFilepath );
    }

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

    public override double GetCurrentTime()
        => _AudioData?.GetCurrentTime() ?? 0 ;

    public override void SetCurrentTime( double aCurrentTime )
        => _AudioData?.SetCurrentTime( aCurrentTime );

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns>音量（100基準）</returns>
    public override int GetVolume()
        => (int)( _AudioData?.GetVolume() ?? 0 * Config.Media.BgmMaxVolume );

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="aVolume">音量（100基準）</param>
    public override void SetVolume( int aVolume )
        => _AudioData?.SetVolume( Config.Media.CheckBgmVolume( aVolume ) / (float)Config.Media.BgmMaxVolume );

    public override void Play() => _AudioData?.Play();

    public override void Pause() => _AudioData?.Pause();

    public override void Stop() => _AudioData?.Stop();
}
