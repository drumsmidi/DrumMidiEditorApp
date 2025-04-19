using System;
using System.Collections.Generic;
using System.Threading;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pModel.pScore;

namespace DrumMidiLibrary.pControl;

/// <summary>
/// ノート位置の再生時間やＢＰＭ値 計算表
/// </summary>
internal class TimeTable
{
    /// <summary>
    /// 排他制御用ロックオブジェクト
    /// </summary>
    public readonly Lock LockObj = new();

    /// <summary>
    /// ノート位置のBPM値検索用
    /// ＜ノート位置（絶対値）、Bpm入力値＞
    /// </summary>
    private readonly Dictionary<int, double> _BpmDic = [];

    /// <summary>
    /// ベースBPM
    /// </summary>
    private double _BaseBpm = ConfigLib.System.DefaultBpm;

    /// <summary>
    /// TimeTable
    /// </summary>
    private double[] _TimeTables = new double[ 10 ];

    /// <summary>
    /// TimeTableへのアクセス。＜排他制御なし＞
    /// 指定したノート位置（絶対値）の時間（秒）を取得
    /// </summary>
    /// <param name="aIndex">ノート位置（絶対値）</param>
    /// <returns>時間（秒）</returns>
    public double this [ int aIndex ] => _TimeTables [ aIndex ];

    /// <summary>
    /// TimeTable末尾の時間（秒）を取得
    /// </summary>
    public double EndTime
    {
        get
        {
            lock ( LockObj )
            {
                return _TimeTables [ ConfigLib.System.NoteCount ];
            }
        }
    }

    /// <summary>
    /// 1ノート辺りの時間（秒）を取得。
    /// （ConfigLib.System.DefaultBpmのBPM値基準）
    /// </summary>
    public double NoteTime
    {
        get
        {
            lock ( LockObj )
            {
                return 240.0d / ( _BaseBpm * ConfigLib.System.MeasureNoteNumber );
            }
        }
    }

    /// <summary>
    /// 指定したノート位置（絶対値）のBPM値を取得
    /// </summary>
    /// <param name="aAbsoluteNotePos">ノート位置（絶対値）</param>
    /// <returns>BPM値</returns>
    public double GetBpm( int aAbsoluteNotePos )
    {
        lock ( LockObj )
        {
            if ( _BpmDic.TryGetValue( aAbsoluteNotePos, out var bpm ) )
            {
                return bpm;
            }

            var closestBpm = _BpmDic[ 0 ];
            var closestPos = 0;

            foreach ( var item in _BpmDic )
            {
                if ( item.Key < aAbsoluteNotePos && item.Key > closestPos )
                {
                    closestBpm = item.Value;
                    closestPos = item.Key;
                }
            }

            return closestBpm;
        }
    }

    /// <summary>
    /// 指定した時間のノート位置（絶対値）を取得
    /// </summary>
    /// <param name="aCurrentTime">秒数</param>
    /// <returns>ノート位置（絶対値）0～</returns>
    public int SearchPosition( double aCurrentTime )
    {
        lock ( LockObj )
        {
            for ( var measure_no = 0; measure_no <= ConfigLib.System.MeasureMaxNumber; measure_no++ )
            {
                var note_pos_s = measure_no * ConfigLib.System.MeasureNoteNumber;

                if ( _TimeTables [ note_pos_s ] > aCurrentTime )
                {
                    for ( var note_pos = 1; note_pos <= ConfigLib.System.MeasureNoteNumber; note_pos++ )
                    {
                        if ( _TimeTables [ note_pos_s - note_pos ] == aCurrentTime )
                        {
                            return note_pos_s - note_pos;
                        }
                        if ( _TimeTables [ note_pos_s - note_pos ] < aCurrentTime )
                        {
                            return note_pos_s - note_pos + 1;
                        }
                    }
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// TimeTable更新フラグ
    /// </summary>
    private bool _FlagUpdate = true;

    /// <summary>
    /// TimeTable更新フラグを立てます。
    /// TimeTable.Update実行時にTimeTableを更新します。
    /// </summary>
    public void Refresh() => _FlagUpdate = true;

    /// <summary>
    /// TimeTableを更新します。
    /// Musicタブで入力したBPM値とEditタブ画面で入力したBPM値を元に
    /// 各ノート位置の再生時間を算出します。
    /// </summary>
    /// <param name="aScore"></param>
    public void Update( Score aScore )
    {
        if ( !_FlagUpdate )
        {
            return;
        }
        _FlagUpdate = false;

        lock ( LockObj )
        {
            _BaseBpm = aScore.Bpm;

            _BpmDic.Clear();
            _BpmDic [ 0 ] = _BaseBpm;

            _TimeTables = new double [ ConfigLib.System.NoteCount + 2 ];
            _TimeTables [ 0 ] = 0;

            var note_time = NoteTime;

            var indexCnt = 0;

            for ( var measure_no = 0; measure_no <= ConfigLib.System.MeasureMaxNumber; measure_no++ )
            {
                var measure = aScore.SysChannel.GetMeasure( measure_no );

                if ( measure == null )
                {
                    for ( var note_pos = 0; note_pos < ConfigLib.System.MeasureNoteNumber; note_pos++ )
                    {
                        _TimeTables [ indexCnt + 1 ] = note_time + _TimeTables [ indexCnt ];

                        indexCnt++;
                    }
                }
                else
                {
                    for ( var note_pos = 0; note_pos < ConfigLib.System.MeasureNoteNumber; note_pos++ )
                    {
                        if ( measure.BpmLine.InfoStates.TryGetValue( note_pos, out var info ) )
                        {
                            note_time = 240.0d / ( info.Bpm * ConfigLib.System.MeasureNoteNumber );

                            _BpmDic [ info.AbsoluteNotePos ] = info.Bpm;
                        }

                        _TimeTables [ indexCnt + 1 ] = note_time + _TimeTables [ indexCnt ];

                        indexCnt++;
                    }
                }
            }

            for ( var index = 0; index < indexCnt; index++ )
            {
                _TimeTables [ index ] = Math.Round( _TimeTables [ index ], 8 );
            }
        }
    }
}
