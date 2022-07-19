using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pAudio;

namespace DrumMidiEditorApp.pAudio;

/// <summary>
/// Audio Midi
/// </summary>
internal class AudioMidi : AudioBase
{
    /// <summary>
    /// MIDIチャンネル(0-15)
    /// </summary>
	private readonly byte _Channel = 0;

    /// <summary>
    /// MIDIノート番号
    /// </summary>
	private readonly byte _Midi = 0;

    /// <summary>
    /// 音量（127基準）
    /// </summary>
	private byte _Volume = 0;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aChannel">MIDIチャンネル(0-15)</param>
    /// <param name="aMidi">MIDIノート番号</param>
    public AudioMidi( byte aChannel, byte aMidi )
    {
        _Channel   = aChannel;
        _Midi      = aMidi;
    }

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns>音量（127基準）</returns>
    public override int GetVolume() => this._Volume;

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="aVolume">音量（127基準）</param>
    public override void SetVolume( int aVolume )
        => _Volume = (byte)Config.Media.CheckMidiVolume( aVolume );

    public override void Play()
        => MidiNet.NoteOn( _Channel, _Midi, _Volume );

    public override void Stop()
        => MidiNet.NoteOff( _Channel, _Midi );
}
