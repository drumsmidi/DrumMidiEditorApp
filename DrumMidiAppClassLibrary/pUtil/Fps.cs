using System.Collections.Generic;
using System.Diagnostics;

namespace DrumMidiClassLibrary.pUtil;

/// <summary>
/// FPS制御
/// </summary>
public class Fps
{
    /// <summary>
    /// ストップウォッチ
    /// </summary>
    private readonly Stopwatch _StopWatch = new();

    /// <summary>
    /// FPS制御
    /// </summary>
    private class TimeCount
    {
        /// <summary>
        /// Tick()を実行時の合計時間
        /// </summary>
        public double TotalTime = 0.0d;

        /// <summary>
        /// Tick()を実行時の前回合計時間
        /// </summary>
        public double TotalTimeBef = 0.0d;

        /// <summary>
        /// TickFpsWeight実行時のフレーム時間
        /// </summary>
        public double FrameTime = 0.0d;

        /// <summary>
        /// １フレームの秒数
        /// </summary>
        public double FpsWeight = 0.0d;

        /// <summary>
        /// 次のフレームまでの秒数
        /// </summary>
        public double FpsWeightNext = 0.0d;

        /// <summary>
        /// フレーム更新タイミング判定
        /// </summary>
        public bool TickFpsWeight => TotalTime - TotalTimeBef >= FpsWeightNext;
    }

    /// <summary>
    /// FPS制御ディクショナリ
    /// </summary>
    private readonly Dictionary<int, TimeCount> _TimeCountDic = [];

    /// <summary>
    /// FPS設定
    /// </summary>
    /// <param name="aKey">設定キー</param>
    /// <param name="aFps">FPS</param>
    public void Set( int aKey, double aFps )
    {
        var weight =  aFps <= 0 ? 0 : 1.0d / aFps ;

        var item = new TimeCount()
        {
            FpsWeight       = weight,
            FpsWeightNext   = weight,
        };

        _TimeCountDic [ aKey ] = item;
    }

    /// <summary>
    /// タイマー開始
    /// </summary>
    public void Start() => _StopWatch.Start();

    /// <summary>
    /// タイマーリセット
    /// </summary>
    public void Reset()
    {
        foreach ( var item in _TimeCountDic.Values )
        {
            item.TotalTime = 0;
            item.TotalTimeBef = 0;
            item.FrameTime = 0;
            item.FpsWeightNext = item.FpsWeight;
        }
        _StopWatch.Reset();
    }

    /// <summary>
    /// タイマー停止
    /// </summary>
    public void Stop() => _StopWatch.Stop();

    /// <summary>
    /// 合計時間を刻む
    /// </summary>
    public void Tick()
    {
        var now = NowTotalTime;

        foreach ( var item in _TimeCountDic.Values )
        {
            item.TotalTime = now;
        }
    }

    /// <summary>
    /// 現在の合計時間
    /// </summary>
    public double NowTotalTime
        => _StopWatch.ElapsedTicks / (double)Stopwatch.Frequency;

    /// <summary>
    /// フレーム更新制御
    /// </summary>
    /// <param name="aKey">設定キー</param>
    /// <returns>True:フレーム更新タイミング、False:処理待ち</returns>
    public bool TickFpsWeight( int aKey )
    {
        if ( !_TimeCountDic.TryGetValue( aKey, out var item ) )
        {
            return false;
        }

        if ( !item.TickFpsWeight )
        {
            return false;
        }

        item.FrameTime = item.TotalTime - item.TotalTimeBef;
        item.TotalTimeBef = item.TotalTime;
        item.FpsWeightNext = item.FpsWeight == 0 ? 0 : item.FrameTime % item.FpsWeight;

        return true;
    }

    /// <summary>
    /// フレーム時間（秒）取得
    /// </summary>
    /// <param name="aKey">設定キー</param>
    /// <returns>フレーム時間（秒）</returns>
    public double GetFrameTime( int aKey ) => !_TimeCountDic.TryGetValue( aKey, out var item ) ? 0 : item.FrameTime;
}
