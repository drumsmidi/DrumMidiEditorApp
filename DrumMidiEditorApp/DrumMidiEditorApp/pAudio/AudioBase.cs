using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pAudio;

/// <summary>
/// Audioベースクラス
/// </summary>
internal class AudioBase : DisposeBaseClass, IAudio
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    protected AudioBase() { }

    public virtual double GetCurrentTime() => 0;

    public virtual void SetCurrentTime( double aCurrentTime ) { }

    public virtual int GetVolume() => 0;

    public virtual void SetVolume( int aVolume ) { }

    public virtual void Play() { }

    public virtual void Pause() { }

    public virtual void Stop() { }
}
