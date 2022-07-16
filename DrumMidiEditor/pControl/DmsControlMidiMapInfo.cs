using System;

using DrumMidiEditor.pAudio;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pControl;

/// <summary>
/// DmsControlNoteInfo⇒DmsControlMidiMapInfoを通してMIDI再生
/// </summary>
internal class DmsControlMidiMapInfo : DisposeBaseClass
{
	/// <summary>
	/// ノート再生
	/// </summary>
	private readonly IAudio? _Audio = null;

	/// <summary>
	/// 音量増減値
	/// </summary>
	private readonly int _VolumeAdd = 0;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="aChannel">MIDIチャンネル番号(0-15)</param>
	/// <param name="aMidi">MIDIノート番号</param>
	/// <param name="aVolumeAdd">音量増減値</param>
	public DmsControlMidiMapInfo( byte aChannel, byte aMidi, int aVolumeAdd )
	{
		_Audio = AudioFactory.CreateMidi( aChannel, aMidi, 0 );

		_VolumeAdd = aVolumeAdd;
	}

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				AudioFactory.Release( _Audio );
			}

			// Dispose unmanaged resources.

			_Disposed = true;

			// Note disposing has been done.
			base.Dispose( aDisposing );
		}
	}
	private bool _Disposed = false;

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
