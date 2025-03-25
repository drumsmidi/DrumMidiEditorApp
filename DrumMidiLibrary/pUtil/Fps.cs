using System.Diagnostics;

namespace DrumMidiLibrary.pUtil;

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
    /// FPS制御
    /// </summary>
    private TimeCount? _TimeCount;

    /// <summary>
    /// FPS設定
    /// </summary>
    /// <param name="aFps">FPS</param>
    public void Set( double aFps )
    {
        var weight =  aFps <= 0 ? 0 : 1.0d / aFps ;

        _TimeCount = new()
        {
            FpsWeight       = weight,
            FpsWeightNext   = weight,
        };
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
        if ( _TimeCount != null )
        {
            _TimeCount.TotalTime      = 0;
            _TimeCount.TotalTimeBef   = 0;
            _TimeCount.FrameTime      = 0;
            _TimeCount.FpsWeightNext  = _TimeCount.FpsWeight;
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
        if ( _TimeCount != null )
        {
            _TimeCount.TotalTime = NowTotalTime;
        }
    }

    /// <summary>
    /// 現在の合計時間
    /// </summary>
    public double NowTotalTime => _StopWatch.ElapsedTicks / (double)Stopwatch.Frequency;

    /// <summary>
    /// フレーム更新制御
    /// </summary>
    /// <param name="aKey">設定キー</param>
    /// <returns>True:フレーム更新タイミング、False:処理待ち</returns>
    public bool TickFpsWeight()
    {
        Tick();

        if ( _TimeCount == null || !_TimeCount.TickFpsWeight )
        {
            return false;
        }

        _TimeCount.FrameTime      = _TimeCount.TotalTime - _TimeCount.TotalTimeBef;
        _TimeCount.TotalTimeBef   = _TimeCount.TotalTime;
        _TimeCount.FpsWeightNext  = _TimeCount.FpsWeight == 0 ? 0 : _TimeCount.FrameTime % _TimeCount.FpsWeight;

        return true;
    }

    /// <summary>
    /// フレーム時間（秒）取得
    /// </summary>
    /// <returns>フレーム時間（秒）</returns>
    public double GetFrameTime() => _TimeCount?.FrameTime ?? 0 ;
}
