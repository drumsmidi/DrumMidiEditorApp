using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pAudio;

/// <summary>
/// Audioベースクラス
/// </summary>
internal abstract class AudioBase : DisposeBaseClass, IAudio
{
    public virtual void Release()
    {
    }

    public virtual object? GetAudioData() => null;

    public virtual double GetCurrentTime() => 0;

    public virtual void SetCurrentTime( double aCurrentTime )
    {
    }

    public virtual int GetVolume() => 0;

    public virtual void SetVolume( int aVolume )
    {
    }

    public virtual void Play()
    {
    }

    public virtual void Pause()
    {
    }

    public virtual void Stop()
    {
    }
}
