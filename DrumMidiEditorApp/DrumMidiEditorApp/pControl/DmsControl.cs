using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using DrumMidiEditorApp.pAudio;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pAudio;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pControl;

/// <summary>
/// ＢＧＭ／ノートの再生制御を行う
/// </summary>
public static class DmsControl
{
    #region Member

    /// <summary>
    /// 音楽再生状態 一覧
    /// </summary>
    private enum DmsState : int
	{
		DmsState_None = 0,
		DmsState_PrePlay,
		DmsState_PlayStart,
		DmsState_Playing,
		DmsState_PreLoopPlay,
		DmsState_LoopPlayStart,
		DmsState_LoopPlaying,
		DmsState_PreStop,
		DmsState_Pause,
		DmsState_Stop,
		DmsState_PreRecord,
		DmsState_Recording,
	}

	/// <summary>
	/// 音楽再生状態
	/// </summary>
	private static DmsState	_State = DmsState.DmsState_None;

	/// <summary>
	/// 音楽再生タスク
	/// </summary>
	private static Task? _MusicTask = null;

	/// <summary>
	/// 音楽再生タスク 停止フラグ
	/// </summary>
	private static bool _TaskStop = true;

	/// <summary>
	/// 描画側準備完了フラグ
	/// </summary>
	private static bool	_UpdatePlayer = false;

	/// <summary>
	/// 音楽再生準備完了フラグ
	/// </summary>
	private static bool	_UpdateAudio = false;

	/// <summary>
	/// ＢＧＭデータ
	/// </summary>
	private static IAudio? _BgmAudio = null;

	/// <summary>
	/// ＢＧＭ再生開始時間
	/// </summary>
    private static double _BgmStartTime = 0;

	/// <summary>
	/// 再生開始時間
	/// </summary>
    private static double _RangeStartTime = 0;

	/// <summary>
	/// 再生終了時間
	/// </summary>
    private static double _RangeEndTime = 0;

	/// <summary>
	/// ノート再生位置（絶対値）。
	/// プレイヤー検索用
	/// </summary>
    private static int _NoteSecPos = 0;

	/// <summary>
	/// TimeTable
	/// </summary>
	private static readonly TimeTable _TimeTable = new();

	/// <summary>
	/// ノート再生シーケンスリスト
	/// </summary>
	private static List<DmsControlNoteInfo> _SequenceInfoList = new();

	/// <summary>
	/// MidiMap情報リスト（チャンネル番号、MidiMapキー、MidiMap情報）
	/// </summary>
    private static Dictionary<int,Dictionary<int,DmsControlMidiMapInfo>> _MidiMapInfoList = new();

	/// <summary>
	/// 音楽再生時間ストップウォッチ
	/// </summary>
    private static readonly Stopwatch _PlayTimeStopWatch = new();

	#endregion

	#region 初期化／終了処理

	/// <summary>
	/// 音楽再生タスク開始
	/// </summary>
	public static void Start()
	{
		try
		{
			_State = DmsState.DmsState_Stop;

			_TimeTable.Refresh();
			_TimeTable.Update();

			_TaskStop = false;

			_MusicTask = Task.Run( () => { ProcSequence(); } );

            Log.Info( $"{Log.GetThisMethodName}:start thread" );
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// 音楽再生タスク終了
	/// </summary>
	public static void End()
	{
		_TaskStop = true;

		// ロックしないように、10秒のみ待つ
		_MusicTask?.Wait( 10000 );

		Log.Info( $"{Log.GetThisMethodName}:end thread" );
	}

    #endregion

    #region 情報取得

    /// <summary>
    /// TimeTable更新フラグを設定
    /// </summary>
    public static void RefreshTimeTable() => _TimeTable.Refresh();

	/// <summary>
	/// 指定した時間のノート位置（絶対値）を取得
	/// </summary>
	/// <param name="aCurrentTime">秒数</param>
	/// <returns>ノート位置（絶対値）0～</returns>
	public static int SearchPosition( double aCurrentTime ) => _TimeTable.SearchPosition( aCurrentTime );

	/// <summary>
	/// 1ノート辺りの時間（秒）を取得。
	/// （Musicタブで入力したBPM値基準）
	/// </summary>
	public static double NoteTime => TimeTable.NoteTime;

	/// <summary>
	/// 再生開始処理後の現在の再生時間（秒）
	/// </summary>
	public static double PlayTime 
		=> (double)_PlayTimeStopWatch.ElapsedTicks / (double)Stopwatch.Frequency + _RangeStartTime;

	/// <summary>
	/// 再生開始時間（秒）を取得。
	/// 通常再生は０，ループ再生はループ再生開始時間を返す
	/// </summary>
	public static double StartPlayTime => _RangeStartTime;

	/// <summary>
	/// 再生終了時間（秒）を取得。
	/// 通常再生は末尾時間，ループ再生はループ再生終了時間を返す
	/// </summary>
	public static double EndPlayTime => _RangeEndTime;

	/// <summary>
	/// 指定した再生位置のノート位置（絶対値）を取得。
	/// プレイヤー用で、過去に遡っての検索はできません。
	/// 通常再生、ループ再生開始時にリセットされます。
	/// </summary>
	/// <param name="aPlayTime">再生位置の時間（秒）</param>
	/// <returns>ノート位置（絶対値）小数点含む</returns>
	public static float PlayNote( float aPlayTime )
	{
		int	max_note = Config.System.NoteCount - 1;

		if ( _NoteSecPos >= max_note )
		{
			return _NoteSecPos;
		}

		while ( _NoteSecPos	<  max_note
			&& _TimeTable[ _NoteSecPos ]     <  aPlayTime
			&& _TimeTable[ _NoteSecPos + 1 ] <= aPlayTime )
		{
			_NoteSecPos += 1;
		}

		if ( _NoteSecPos == 0 )
		{
			return (float)( aPlayTime / _TimeTable[ _NoteSecPos + 1 ] );
		}
		else
		{
			return (float)( _NoteSecPos + ( aPlayTime - _TimeTable[ _NoteSecPos ] )
				/ ( _TimeTable[ _NoteSecPos + 1 ] - _TimeTable[ _NoteSecPos ] ) );
		}
	}

	/// <summary>
	/// 指定したノート位置（絶対値）のBPM値を取得
	/// </summary>
	/// <param name="aAbsoluteNotePos">ノート位置（絶対値）</param>
	/// <returns>BPM値</returns>
	public static double GetBpm( int aAbsoluteNotePos ) => _TimeTable.GetBpm( aAbsoluteNotePos );

	/// <summary>
	/// ＢＧＭデータ取得
	/// </summary>
	public static NAudioData? AudioData => ( _BgmAudio as AudioBgm )?.AudioData;

    #endregion

    #region リクエスト

    /// <summary>
    /// 通常再生リクエスト
    /// </summary>
    public static void PlayPreSequence()
	{
		_UpdatePlayer	= false;
		_UpdateAudio	= false;

		_State = DmsState.DmsState_PrePlay;

		SetPlayerRequest( ConfigPlayer.PlayRequest.PrePlay );
	}

	/// <summary>
	/// ループ再生リクエスト
	/// </summary>
	public static void PlayPreLoopSequence()
	{
		_UpdatePlayer	= false;
		_UpdateAudio	= false;

		_State = DmsState.DmsState_PreLoopPlay;

		SetPlayerRequest( ConfigPlayer.PlayRequest.PreLoopPlay );
	}

	/// <summary>
	/// 停止リクエスト
	/// </summary>
	public static void StopPreSequence()
	{
		_State = DmsState.DmsState_PreStop;

		SetPlayerRequest( ConfigPlayer.PlayRequest.PreStop );
	}

	/// <summary>
	/// レコード再生リクエスト
	/// </summary>
	public static void RecordPreSequence()
	{
		_UpdatePlayer	= false;
		_UpdateAudio	= false;

		_State = DmsState.DmsState_PreRecord;

		SetPlayerRequest( ConfigPlayer.PlayRequest.PreRecord );
	}

	/// <summary>
	/// プレイヤーフォームへのリクエスト設定
	/// </summary>
	/// <param name="aRequest">リクエスト</param>
	private static void SetPlayerRequest( ConfigPlayer.PlayRequest aRequest )
		=> Config.Player.PlayReq = aRequest;

    #endregion

    #region 同期処理

    /// <summary>
    /// 録画準備が整ったら呼び出す
    /// （音楽再生準備の完了を待つ）
    /// </summary>
    public static void WaitRecorder()
    {
		int cnt = 3000;

		while ( !_UpdatePlayer && !_UpdateAudio && cnt-- > 0 )
		{
			Thread.Sleep( 1 );
		}
    }

	/// <summary>
	/// 再生準備が整ったら呼び出す。
	/// （プレイヤー再生準備の完了を待つ）
	/// </summary>
	private static void WaitPlayer()
	{
		int cnt = 3000;

		_UpdateAudio = true;

		while ( !_UpdatePlayer && cnt-- > 0 )
		{
			Thread.Sleep( 1 );
		}
	}

	/// <summary>
	/// プレイヤーの準備が整ったら呼び出す
	/// （音楽再生準備の完了を待つ）
	/// </summary>
	public static void WaitAudio()
	{
		int cnt = 3000;

		_UpdatePlayer = true;

		while ( !_UpdateAudio && cnt-- > 0 )
		{
			Thread.Sleep( 1 );
		}
	}

    #endregion

    /// <summary>
    /// 音楽再生処理
    /// </summary>
    private static void ProcSequence()
	{
		bool	range_play		= false;
		int		loop_start;
		int		loop_end;
		bool	bgm_play        = false;
		bool	bgm_play_on;
		bool	note_play_on;
		int		pos				= 0;
		double	playtime;
		int		seq_cnt			= 0;
		var		sounds			= new List<DmsControlNoteInfo>();
		int		sleeptime		= 1;

		DmsControlNoteInfo info;

		try
		{
			while ( !_TaskStop )
			{
				Thread.Sleep( sleeptime );

				try
				{
					switch ( _State )
					{
						case DmsState.DmsState_Playing:
						case DmsState.DmsState_LoopPlayStart:
							{
								break;
							}
						case DmsState.DmsState_PrePlay:
							{
								_TimeTable.Update();

								UpdateBgm();
								UpdateMidiMapSet();
								UpdateScore();

								_BgmStartTime	= DMS.SCORE.BgmPlaybackStartPosition;
								_RangeStartTime	= 0;
								_RangeEndTime	= _TimeTable.EndTime;
								_NoteSecPos		= 0;
								pos				= 0;
								seq_cnt			= _SequenceInfoList.Count;
								range_play		= false;
								bgm_play		= false;
								sleeptime		= 1;

								MidiNet.Reset();
								_BgmAudio?.Stop();

								WaitPlayer();

								_PlayTimeStopWatch.Reset();
								_PlayTimeStopWatch.Start();

								_State = DmsState.DmsState_Playing;

								continue;
							}
						case DmsState.DmsState_PreLoopPlay:
							{
								loop_start	= Config.Media.PlayLoopStart;
								loop_end	= Config.Media.PlayLoopEnd + 1;

								if ( loop_start > loop_end )
								{
									_State = DmsState.DmsState_PreStop;
									continue;
								}

								_TimeTable.Update();

								UpdateBgm();
								UpdateMidiMapSet();
								UpdateScore();

								_BgmStartTime	= DMS.SCORE.BgmPlaybackStartPosition;
                                _RangeStartTime = _TimeTable[ ( loop_start == 0 ) ? 0 : loop_start * Config.System.MeasureNoteNumber ];
                                _RangeEndTime	= _TimeTable[ ( loop_end   == 0 ) ? 0 : loop_end   * Config.System.MeasureNoteNumber + 1 ];
                                _NoteSecPos		= loop_start * Config.System.MeasureNoteNumber;
								pos				= 0;
								seq_cnt			= _SequenceInfoList.Count;
								range_play		= true;
								bgm_play		= false;
								sleeptime		= 1;

								MidiNet.Reset();
								_BgmAudio?.Stop();

								// 再生開始位置まで、ノート位置を進める
								while ( pos < seq_cnt )
								{
									info = _SequenceInfoList[ pos ];

									if ( info.PlaySecond > _RangeStartTime )
									{
										break;
									}

									pos++;
								}

								WaitPlayer();

								_PlayTimeStopWatch.Reset();
								_PlayTimeStopWatch.Start();

								_State = DmsState.DmsState_LoopPlayStart;

								continue;
							}
						case DmsState.DmsState_PreStop:
							{
								sleeptime = 500;

								MidiNet.Reset();
								_BgmAudio?.Stop();

								_PlayTimeStopWatch.Stop();

								_State = DmsState.DmsState_Pause;

								continue;
							}
						case DmsState.DmsState_PreRecord:
							{
								_TimeTable.Update();

								UpdateBgm();
								UpdateMidiMapSet();
								UpdateScore();

								loop_end = DMS.SCORE.GetMaxMeasureNo() + 3;

								_BgmStartTime	= DMS.SCORE.BgmPlaybackStartPosition;
								_RangeStartTime = 0;
								_RangeEndTime	= _TimeTable[ loop_end * Config.System.MeasureNoteNumber + 1 ];
								_NoteSecPos		= 0;
								pos				= 0;
								seq_cnt			= _SequenceInfoList.Count;
								range_play		= false;
								bgm_play		= false;
								sleeptime		= 100;

								MidiNet.Reset();
								_BgmAudio?.Pause();

								WaitPlayer();

								_PlayTimeStopWatch.Stop();

								_State = DmsState.DmsState_Recording;

								continue;
							}
						default:
							{		
								continue;
							}
					}
				}
				catch ( Exception e )
				{
					Log.Warning( $"{Log.GetThisMethodName}:{e.Message}" );
					continue;
				}

				// ＢＧＭ・ノート再生ＯＮ／ＯＦＦ取得
				bgm_play_on  = Config.Media.BgmPlayOn;
				note_play_on = Config.Media.NotePlayOn;

				// ＢＧＭの音量設定
				_BgmAudio?.SetVolume( bgm_play_on ? DMS.SCORE.BgmVolume : 0 );

				// 現在の再生時間（秒）を取得
				playtime = PlayTime;

				#region ＢＧＭ再生開始

				if ( !bgm_play )
				{
					if ( _BgmStartTime < 0 || _BgmStartTime <= playtime )
					{
						_BgmAudio?.SetCurrentTime( playtime - _BgmStartTime );
						_BgmAudio?.Play();

						bgm_play = true;
					}
				}

                #endregion

                #region 前回位置から今回位置間で再生対象のノートを抽出

                while ( pos < seq_cnt )
				{
					info = _SequenceInfoList[ pos ];

					if ( info.PlaySecond > playtime )
					{
						break;
					}
					sounds.Add( info );

					pos++;
				}

                #endregion

                #region ノート再生

				// TODO: チャンネル別再生ON/OFFなど対応が必要
                if ( note_play_on )
				{
					foreach ( var seq_info in sounds )
					{
						seq_info.Play();
					}
				}
				sounds.Clear();

                #endregion

                #region 再生終了判定

                if ( range_play )
				{
					if ( _RangeEndTime <= playtime )
					{
						_State = DmsState.DmsState_PreLoopPlay;

						SetPlayerRequest( ConfigPlayer.PlayRequest.PreLoopPlay );
					}
				}
				else
				{
					if ( _TimeTable.EndTime < playtime )
					{
						_State = DmsState.DmsState_PreStop;

						SetPlayerRequest( ConfigPlayer.PlayRequest.PrePlay );
					}
				}

                #endregion
            }
        }
		catch ( Exception )
        {
			_TaskStop = true;

            throw;
        }
		finally
		{
			_BgmAudio?.Stop();
			_PlayTimeStopWatch.Stop();
		}
	}

    #region 更新処理

    /// <summary>
    /// ＢＧＭを更新
    /// </summary>
    private static void UpdateBgm()
	{
		try
		{
			// TODO: BGM再生終了後の再生ができない。NAudioの使い方に問題がある？

			// ＢＧＭ再読み込み
			//if ( Config.Media.UpdateDmsControlBgm )
			{
				Config.Media.UpdateDmsControlBgm = false;

				AudioFactory.Release( _BgmAudio );

				_BgmAudio = AudioFactory.CreateBgm( DMS.SCORE.BgmFilePath, DMS.SCORE.BgmVolume );
			}

			// TODO: イコライザ設定が必要

			// ＢＧＭへのイコライザ適用
			//DMS.EditerForm?.MusicCtl.ApplyEqulizer();
		}
		catch ( Exception e )
		{
			Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// MidiMap再生情報を更新
	/// </summary>
	private static void UpdateMidiMapSet()
	{
		if ( !Config.Media.UpdateDmsControlMidiMap )
		{
			return;
		}
        Config.Media.UpdateDmsControlMidiMap = false;

		#region Create MidiMapInfo

		var dlist = new Dictionary<int, Dictionary<int, DmsControlMidiMapInfo>>();

		foreach ( var channel in DMS.SCORE.Channels.Values )
		{
			dlist.Add( channel.ChannelNo, new() );

			foreach ( var midiMap in channel.MidiMapSet.DisplayMidiMaps )
			{
				dlist[ channel.ChannelNo ]
					.Add( midiMap.MidiMapKey, new( channel.ChannelNo, midiMap.Midi, midiMap.VolumeAddIncludeGroup ) );
			}
		}

		#endregion

		#region Release old MidiMapInfo

		foreach ( var infos in _MidiMapInfoList.Values )
		{ 
			var e = infos.GetEnumerator();

			while ( e.MoveNext() )
			{			
				e.Current.Value.Dispose();
			}
			infos.Clear();
		}
		_MidiMapInfoList.Clear();

		#endregion

		_MidiMapInfoList = dlist;
	}

	/// <summary>
	/// ノート再生情報を更新
	/// </summary>
	private static void UpdateScore()
	{
		if ( !Config.Media.UpdateDmsControlScore )
		{
			return;
		}
        Config.Media.UpdateDmsControlScore = false;

		var rnd		= new Random();
		var s_list	= new List<DmsControlNoteInfo>();

		#region Create NoteInfo

		foreach ( var channel in DMS.SCORE.Channels.Values )
		{
			if ( !_MidiMapInfoList.TryGetValue( channel.ChannelNo, out var midiMapInfoList ) )
            {
				continue;
            }
		
			foreach ( var info in channel.NoteInfoList.Values )
			{
				if ( !midiMapInfoList.TryGetValue( info.MidiMapKey, out var midiMapInfo ) )
				{
					continue;
				}

				if ( info.NoteOff )
				{ 
					s_list.Add
						( 
							new
							(
								_TimeTable[ info.AbsoluteNotePos ] - Config.Media.MidiOutLatency,
								0,
								false,
								ref midiMapInfo
							)
						);
				}

				if ( info.NoteOn )
				{ 
					s_list.Add
						( 
							new
							(
								_TimeTable[ info.AbsoluteNotePos ] - Config.Media.MidiOutLatency,
								info.Volume + rnd.Next( -Config.Media.RandomVolume, Config.Media.RandomVolume ),
								true,
								ref midiMapInfo
							)
						);
				}
			}
		}

		s_list.Sort();

		#endregion

		#region Release old NoteInfo
        {
            var e = _SequenceInfoList.GetEnumerator();

			while ( e.MoveNext() )
			{
				e.Current.Dispose();
			} 

			_SequenceInfoList.Clear();
		}
		#endregion

        _SequenceInfoList = s_list;
	}

    #endregion
}
