namespace DrumMidiEditorApp.pAudio;

/// <summary>
/// Audio Midi
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aChannel">MIDIチャンネル(0-15)</param>
/// <param name="aMidi">MIDIノート番号</param>
internal partial class AudioMidi( byte aChannel, byte aMidi ) : AudioBase
{
    /// <summary>
    /// MIDIチャンネル(0-15)
    /// </summary>
	private readonly byte _Channel = aChannel;

    /// <summary>
    /// MIDIノート番号
    /// </summary>
	private readonly byte _Midi = aMidi;

    /// <summary>
    /// 音量（127基準）
    /// </summary>
	private byte _Volume = (byte)MidiNet.MidiMinVolume;

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns>音量（127基準）</returns>
    public override int GetVolume() => _Volume;

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="aVolume">音量（127基準）</param>
    public override void SetVolume( int aVolume )
        => _Volume = (byte)MidiNet.CheckMidiVolume( aVolume );

    public override void Play()
        => MidiNet.NoteOn( _Channel, _Midi, _Volume );

    public override void Stop()
        => MidiNet.NoteOff( _Channel, _Midi );
}
