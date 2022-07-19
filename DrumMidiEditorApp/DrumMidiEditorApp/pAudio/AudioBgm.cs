using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pAudio;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pAudio;

/// <summary>
/// Audio BGM
/// </summary>
internal class AudioBgm : AudioBase
{
    /// <summary>
    /// AudioData
    /// </summary>
	public NAudioData? AudioData { get; private set; } = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aFilepath">Audioファイルパス</param>
    public AudioBgm( GeneralPath aFilepath )
    {
        AudioData = new NAudioData( aFilepath );
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				Stop();
                AudioData?.Dispose();
                AudioData = null;
            }

			// Dispose unmanaged resources.

			_Disposed = true;

			// Note disposing has been done.
			base.Dispose( aDisposing );
		}
	}
    private bool _Disposed = false;

    public override double GetCurrentTime()
        => AudioData?.GetCurrentTime() ?? 0 ;

    public override void SetCurrentTime( double aCurrentTime )
        => AudioData?.SetCurrentTime( aCurrentTime );

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns>音量（100基準）</returns>
    public override int GetVolume()
        => (int)( AudioData?.GetVolume() ?? 0 * Config.Media.BgmMaxVolume );

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="aVolume">音量（100基準）</param>
    public override void SetVolume( int aVolume )
        => AudioData?.SetVolume( Config.Media.CheckBgmVolume( aVolume ) / (float)Config.Media.BgmMaxVolume );

    public override void Play() => AudioData?.Play();

    public override void Pause() => AudioData?.Pause();

    public override void Stop() => AudioData?.Stop();
}
