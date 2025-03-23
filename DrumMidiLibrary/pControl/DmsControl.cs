using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel;

namespace DrumMidiLibrary.pControl;

/// <summary>
/// ＢＧＭ／ノートの再生制御を行う
/// </summary>
public static class DmsControl
{
    #region 初期化／終了処理

    /// <summary>
    /// 音楽再生タスク
    /// </summary>
    private static Task? _MusicTask = null;

    /// <summary>
    /// 音楽再生タスク 停止フラグ
    /// </summary>
    private static bool _FlagStopMusicTask = true;

    /// <summary>
    /// 音楽再生タスク開始
    /// </summary>
    public static void Start()
    {
        try
        {
            End();

            _TimeTable.Refresh();
            _TimeTable.Update( _TmpScore );

            _FlagStopMusicTask = false;

            // 粒度の細かいシステムではなく、タスクが長時間実行され、
            // 少量の大きなコンポーネントを含む粒度の粗い操作とすることを指定します。
            // これは、TaskScheduler に対し、オーバーサブスクリプションを許可してもよいことを示します。
            // オーバーサブスクリプションを使用すると、使用可能なハードウェア スレッドよりも多くのスレッドを作成できます。
            // これは、タスクの処理に追加のスレッドが必要になる可能性があるというヒントをタスク スケジューラに提供し、
            // 他のスレッドまたはローカル スレッド プール キューの作業項目の進行をスケジューラがブロックするのを防ぎます。
            _MusicTask = Task.Factory.StartNew( ProcSequenceAsync, TaskCreationOptions.LongRunning );

            //_MusicTask = Task.Run(() => { ProcSequenceAsync(); });

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
        try
        {
            _FlagStopMusicTask = true;

            if ( _MusicTask != null )
            {
                // ロックしないように、10秒のみ待つ
                _ = _MusicTask.Wait( 10000 );
                _MusicTask.Dispose();
                _MusicTask = null;

                Log.Info( $"{Log.GetThisMethodName}:end thread" );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
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
    public static int SearchPosition( double aCurrentTime ) 
        => _TimeTable.SearchPosition( aCurrentTime );

    /// <summary>
    /// 再生開始処理後の現在の再生時間（秒）
    /// </summary>
    public static double PlayTime
        => ( _PlayTimeStopWatch.ElapsedTicks / (double)Stopwatch.Frequency ) + StartPlayTime;

    /// <summary>
    /// 再生開始時間（秒）を取得。
    /// 通常再生は０，ループ再生はループ再生開始時間を返す
    /// </summary>
    public static double StartPlayTime { get; private set; } = 0;

    /// <summary>
    /// 再生終了時間（秒）を取得。
    /// 通常再生は末尾時間，ループ再生はループ再生終了時間を返す
    /// </summary>
    public static double EndPlayTime { get; private set; } = 0;

    /// <summary>
    /// 指定した再生位置のノート位置（絶対値）を取得。
    /// プレイヤー用で、過去に遡っての検索はできません。
    /// 通常再生、ループ再生開始時にリセットされます。
    /// </summary>
    /// <param name="aPlayTime">再生位置の時間（秒）</param>
    /// <returns>ノート位置（絶対値）小数点含む</returns>
    public static float PlayNote( double aPlayTime )
    {
        var max_note = ConfigLib.System.NoteCount - 1;

        if ( _NoteSecPos >= max_note )
        {
            return _NoteSecPos;
        }

        lock ( _TimeTable.LockObj )
        {
            while ( _NoteSecPos < max_note
                && _TimeTable [ _NoteSecPos ] < aPlayTime
                && _TimeTable [ _NoteSecPos + 1 ] <= aPlayTime )
            {
                _NoteSecPos += 1;
            }

            return _NoteSecPos == 0
                ? (float)( aPlayTime / _TimeTable [ _NoteSecPos + 1 ] )
                : (float)( _NoteSecPos + ( ( aPlayTime - _TimeTable [ _NoteSecPos ] )
                    / ( _TimeTable [ _NoteSecPos + 1 ] - _TimeTable [ _NoteSecPos ] ) ) );
        }
    }

    /// <summary>
    /// 指定したノート位置（絶対値）のBPM値を取得
    /// </summary>
    /// <param name="aAbsoluteNotePos">ノート位置（絶対値）</param>
    /// <returns>BPM値</returns>
    public static double GetBpm( int aAbsoluteNotePos ) 
        => _TimeTable.GetBpm( aAbsoluteNotePos );

    /// <summary>
    /// 指定したノート位置（絶対値）の開始時間を取得
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <returns>BPM値</returns>
    public static double GetMeasureStartTime( int aMeasureNo ) 
    {
        if ( aMeasureNo >= ConfigLib.System.MeasureCount )
        {
            return 0;
        }
        return _TimeTable [ aMeasureNo * ConfigLib.System.MeasureNoteNumber ];
    }

    /// <summary>
    /// ＢＧＭデータ取得
    /// </summary>
    public static NAudioData? AudioData => _BgmAudio?.GetAudioData() as NAudioData;

    #endregion

    #region リクエスト

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
    private static DmsState _State = DmsState.DmsState_None;

    /// <summary>
    /// 音楽再生リクエスト
    /// </summary>
    private static DmsState _RequestState = DmsState.DmsState_None;

    /// <summary>
    /// 通常再生リクエスト
    /// </summary>
    public static void PlayPreSequence() => _RequestState = DmsState.DmsState_PrePlay;

    /// <summary>
    /// ループ再生リクエスト
    /// </summary>
    public static void PlayPreLoopSequence() => _RequestState = DmsState.DmsState_PreLoopPlay;

    /// <summary>
    /// 停止リクエスト
    /// </summary>
    public static void StopPreSequence() => _RequestState = DmsState.DmsState_PreStop;

    /// <summary>
    /// レコード再生リクエスト
    /// </summary>
    public static void RecordPreSequence() => _RequestState = DmsState.DmsState_PreRecord;

    #endregion

    #region プレイヤー再生リクエスト通知用

    /// <summary>
    /// プレイヤー再生リクエスト一覧
    /// </summary>
    public enum PlayRequest : int
    {
        None = 0,
        PreStop,
        PrePlay,
        PreLoopPlay,
        PreRecord,
    }

    /// <summary>
    /// プレイヤー再生リクエスト
    /// </summary>
    public static PlayRequest PlayReq { get; set; } = PlayRequest.None;

    #endregion

    #region 同期制御（力技）

    /// <summary>
    /// 描画側準備完了フラグ
    /// </summary>
    private static bool _FlagUpdatePlayer = false;

    /// <summary>
    /// 音楽再生準備完了フラグ
    /// </summary>
    private static bool _FlagUpdateAudio = false;

    /// <summary>
    /// 待機フラグリセット
    /// </summary>
    private static void WaitFlagReset()
    {
        _FlagUpdatePlayer = false;
        _FlagUpdateAudio  = false;
    }

    /// <summary>
    /// 録画準備が整ったら呼び出す
    /// （音楽再生準備の完了を待つ）
    /// </summary>
    public static void WaitRecorder()
    {
        var cnt = 3000;

        while ( !_FlagUpdatePlayer && !_FlagUpdateAudio && cnt-- > 0 )
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
        var cnt = 3000;

        _FlagUpdateAudio = true;

        while ( !_FlagUpdatePlayer && cnt-- > 0 )
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
        var cnt = 3000;

        _FlagUpdatePlayer = true;

        while ( !_FlagUpdateAudio && cnt-- > 0 )
        {
            Thread.Sleep( 1 );
        }
    }

    #endregion

    #region Music sequence Task

    /// <summary>
    /// 非同期処理用スコア情報
    /// </summary>
    private static Score _TmpScore = new();

    /// <summary>
    /// ＢＧＭデータ
    /// </summary>
    private static IAudio? _BgmAudio = null;

    /// <summary>
    /// ＢＧＭ再生開始時間
    /// </summary>
    private static double _BgmStartTime = 0;

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
    private static List<DmsControlNoteInfo> _SequenceInfoList = [];

    /// <summary>
    /// MidiMap情報リスト（チャンネル番号、MidiMapキー、MidiMap情報）
    /// </summary>
    private static Dictionary<int, Dictionary<int, DmsControlMidiMapInfo>> _MidiMapInfoList = [];

    /// <summary>
    /// 音楽再生時間ストップウォッチ
    /// </summary>
    private static readonly Stopwatch _PlayTimeStopWatch = new();

    /// <summary>
    /// 音楽再生処理
    /// </summary>
    private static async void ProcSequenceAsync()
    {
        var     range_play      = false;
        int     loop_start;
        int     loop_end;
        var     bgm_play        = false;
        bool    bgm_play_on;
        bool    note_play_on;
        var     pos             = 0;
        double  playtime;
        var     seq_cnt         = 0;
        var     sounds          = new List<DmsControlNoteInfo>();
        var     sleeptime       = 500;

        DmsControlNoteInfo info;

        try
        {
            // 初期化
            _State = DmsState.DmsState_Stop;

            // スレッドプール
            while ( !_FlagStopMusicTask )
            {
                //Thread.Sleep( sleeptime );
                await Task.Delay( sleeptime );

                try
                {
                    #region リクエスト受付＆初期化

                    if ( _RequestState != DmsState.DmsState_None )
                    {
                        // 初期化＆プレイヤー側へのリクエストを設定
                        // プレイヤー側からリクエストの状態を監視する作りとする
                        switch ( _RequestState )
                        {
                            case DmsState.DmsState_PrePlay:
                                {
                                    WaitFlagReset();
                                    PlayReq = PlayRequest.PrePlay;
                                }
                                break;
                            case DmsState.DmsState_PreLoopPlay:
                                {
                                    WaitFlagReset();
                                    PlayReq = PlayRequest.PreLoopPlay;
                                }
                                break;
                            case DmsState.DmsState_PreStop:
                                {
                                    PlayReq = PlayRequest.PreStop;
                                }
                                break;
                            case DmsState.DmsState_PreRecord:
                                {
                                    WaitFlagReset();
                                    PlayReq = PlayRequest.PreRecord;
                                }
                                break;
                        }

                        // 状態更新
                        _State          = _RequestState;
                        _RequestState   = DmsState.DmsState_None;
                    }

                    #endregion

                    #region 状態別処理

                    switch ( _State )
                    {
                        case DmsState.DmsState_Playing:
                        case DmsState.DmsState_LoopPlayStart:
                            {
                                break;
                            }
                        case DmsState.DmsState_PrePlay:
                            {
                                // スコア情報コピー
                                CloneScore();

                                _TimeTable.Update( _TmpScore );

                                UpdateBgm();
                                UpdateMidiMapSet();
                                UpdateScore();

                                _BgmStartTime   = _TmpScore.BgmPlaybackStartPosition;
                                StartPlayTime   = 0;
                                EndPlayTime     = _TimeTable.EndTime;
                                _NoteSecPos     = 0;
                                pos             = 0;
                                seq_cnt         = _SequenceInfoList.Count;
                                range_play      = false;
                                bgm_play        = false;
                                sleeptime       = 1;

                                MidiNet.SystemReset();
                                _BgmAudio?.Stop();

                                WaitPlayer();

                                _PlayTimeStopWatch.Reset();
                                _PlayTimeStopWatch.Start();

                                _State = DmsState.DmsState_Playing;

                                continue;
                            }
                        case DmsState.DmsState_PreLoopPlay:
                            {
                                loop_start  = ConfigLib.Media.PlayLoopStart;
                                loop_end    = ConfigLib.Media.PlayLoopEnd + 1;

                                if ( loop_start > loop_end )
                                {
                                    _State = DmsState.DmsState_PreStop;
                                    continue;
                                }

                                // スコア情報コピー
                                CloneScore();

                                _TimeTable.Update( _TmpScore );

                                UpdateBgm();
                                UpdateMidiMapSet();
                                UpdateScore();

                                _BgmStartTime   = _TmpScore.BgmPlaybackStartPosition;
                                StartPlayTime   = _TimeTable [ loop_start * ConfigLib.System.MeasureNoteNumber ];
                                EndPlayTime     = _TimeTable [ loop_end   * ConfigLib.System.MeasureNoteNumber + 1 ];
                                _NoteSecPos     = loop_start * ConfigLib.System.MeasureNoteNumber;
                                pos             = 0;
                                seq_cnt         = _SequenceInfoList.Count;
                                range_play      = true;
                                bgm_play        = false;
                                sleeptime       = 1;

                                MidiNet.SystemReset();
                                _BgmAudio?.Stop();

                                // 再生開始位置まで、ノート位置を進める
                                while ( pos < seq_cnt )
                                {
                                    info = _SequenceInfoList [ pos ];

                                    if ( info.PlaySecond > StartPlayTime )
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

                                MidiNet.SystemReset();
                                _BgmAudio?.Stop();

                                _PlayTimeStopWatch.Stop();

                                _State = DmsState.DmsState_Pause;

                                continue;
                            }
                        case DmsState.DmsState_PreRecord:
                            {
                                // スコア情報コピー
                                CloneScore();

                                _TimeTable.Update( _TmpScore );

                                UpdateBgm();
                                UpdateMidiMapSet();
                                UpdateScore();

                                loop_end        = _TmpScore.GetMaxMeasureNo() + 3;

                                _BgmStartTime   = _TmpScore.BgmPlaybackStartPosition;
                                StartPlayTime   = 0;
                                EndPlayTime     = _TimeTable [ loop_end * ConfigLib.System.MeasureNoteNumber + 1 ];
                                _NoteSecPos     = 0;
                                pos             = 0;
                                seq_cnt         = _SequenceInfoList.Count;
                                range_play      = false;
                                bgm_play        = false;
                                sleeptime       = 500;

                                MidiNet.SystemReset();
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

                    #endregion
                }
                catch ( Exception e )
                {
                    Log.Warning( $"{Log.GetThisMethodName}:{e.Message}" );
                    continue;
                }

                // ＢＧＭ・ノート再生ＯＮ／ＯＦＦ取得
                bgm_play_on  = ConfigLib.Media.BgmPlayOn;
                note_play_on = ConfigLib.Media.NotePlayOn;

                // ＢＧＭの音量設定
                _BgmAudio?.SetVolume( bgm_play_on ? ConfigLib.Media.BgmVolume : ConfigLib.Media.BgmMinVolume );

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
                    info = _SequenceInfoList [ pos ];

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
                    if ( EndPlayTime <= playtime )
                    {
                        _State = DmsState.DmsState_PreLoopPlay;

                        PlayReq = PlayRequest.PreLoopPlay;
                    }
                }
                else
                {
                    if ( _TimeTable.EndTime < playtime )
                    {
                        _State = DmsState.DmsState_PreStop;

                        PlayReq = PlayRequest.PrePlay;
                    }
                }

                #endregion
            }
        }
        catch ( Exception )
        {
            _FlagStopMusicTask = true;

            throw;
        }
        finally
        {
            _BgmAudio?.Stop();
            _PlayTimeStopWatch.Stop();
        }
    }

    /// <summary>
    /// スコア情報コピー
    /// </summary>
    private static void CloneScore()
    {
        try
        {
            if ( ConfigLib.Media.FlagUpdateDmsControlBgm        || 
                 ConfigLib.Media.FlagUpdateDmsControlMidiMap    || 
                 ConfigLib.Media.FlagUpdateDmsControlScore      )
            {
                // スコア情報コピー
                _TmpScore.Dispose();
                _TmpScore = ConfigLib.Media.SCORE;

                // TODO: 見直し検討

                // ループ処理内での即時反映用
                ConfigLib.Media.BgmVolume = _TmpScore.BgmVolume;
            }
        }
        catch ( Exception e )
        {
            Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ＢＧＭを更新
    /// </summary>
    private static void UpdateBgm()
    {
        try
        {
            // TODO: 回避できていない
            // 暫定回避策
            // 停止せずに再生終了した場合、再生できなくなる為
            // その場合は再度BGMを読み込むようにする。
            if ( !( ( _BgmAudio?.GetAudioData() as NAudioData )?.IsPlaying() ?? false ) )
            {
                ConfigLib.Media.FlagUpdateDmsControlBgm = true;
            }

            // ＢＧＭ再読み込み
            if ( ConfigLib.Media.FlagUpdateDmsControlBgm )
            {
                ConfigLib.Media.FlagUpdateDmsControlBgm = false;

                AudioFactory.Release( _BgmAudio );

                _BgmAudio = AudioFactory.CreateBgm( _TmpScore.BgmFilePath, _TmpScore.BgmVolume );
            }

            // ＢＧＭへのイコライザ適用
            ApplyEqualizerCallback?.Invoke();
        }
        catch ( Exception e )
        {
            Log.Info( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// イコライザ適用
    /// </summary>
    public delegate void ApplyEqualizer();

    /// <summary>
    /// イコライザ適用コールバック
    /// </summary>
    public static ApplyEqualizer? ApplyEqualizerCallback = null;

    /// <summary>
    /// MidiMap再生情報を更新
    /// </summary>
    private static void UpdateMidiMapSet()
    {
        if ( !ConfigLib.Media.FlagUpdateDmsControlMidiMap )
        {
            return;
        }
        ConfigLib.Media.FlagUpdateDmsControlMidiMap = false;

        #region Create MidiMapInfo

        var dlist = new Dictionary<int, Dictionary<int, DmsControlMidiMapInfo>>();

        foreach ( var channel in _TmpScore.Channels.Values )
        {
            dlist.Add( channel.ChannelNo, [] );

            foreach ( var midiMap in channel.MidiMapSet.DisplayMidiMaps )
            {
                dlist [ channel.ChannelNo ]
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
        if ( !ConfigLib.Media.FlagUpdateDmsControlScore )
        {
            return;
        }
        ConfigLib.Media.FlagUpdateDmsControlScore = false;

        var rnd     = new Random();
        var s_list  = new List<DmsControlNoteInfo>();

        #region Create NoteInfo

        foreach ( var channel in _TmpScore.Channels.Values )
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
                                _TimeTable [ info.AbsoluteNotePos ] - ConfigLib.Media.MidiOutLatency,
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
                                _TimeTable [ info.AbsoluteNotePos ] - ConfigLib.Media.MidiOutLatency,
                                info.Volume + rnd.Next( -ConfigLib.Media.RandomVolume, ConfigLib.Media.RandomVolume ),
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
