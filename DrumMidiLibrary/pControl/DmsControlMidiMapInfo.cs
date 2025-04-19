using System;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pControl;

/// <summary>
/// DmsControlNoteInfo⇒DmsControlMidiMapInfoを通してMIDI再生
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aChannel">MIDIチャンネル番号(0-15)</param>
/// <param name="aMidi">MIDIノート番号</param>
/// <param name="aVolumeAdd">音量増減値</param>
internal partial class DmsControlMidiMapInfo( byte aChannel, byte aMidi, int aVolumeAdd ) : DisposeBaseClass
{
    protected override void Dispose( bool aDisposing )
    {
        if ( _Disposed )
        {
            return;
        }

        // マネージドリソースの解放
        if ( aDisposing )
        {
            AudioFactory.Release( _Audio );
        }

        // アンマネージドリソースの解放
        {
        }

        _Disposed = true;

        base.Dispose( aDisposing );
    }
    private bool _Disposed = false;

    #region member

    /// <summary>
    /// ノート再生
    /// </summary>
    private readonly IAudio? _Audio = AudioFactory.CreateMidi( aChannel, aMidi, 0 );

    /// <summary>
    /// 音量増減値
    /// </summary>
    private readonly int _VolumeAdd = aVolumeAdd;

    #endregion

    /// <summary>
    /// MidiMapに設定されている音量増減値を加算して再生
    /// </summary>
    /// <param name="aVolume">音量(127基準)</param>
    public void Play( int aVolume )
    {
        try
        {
            _Audio?.SetVolume( _VolumeAdd + aVolume );
            _Audio?.Play();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// 再生停止
    /// </summary>
    public void Stop()
    {
        try
        {
            _Audio?.Stop();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
