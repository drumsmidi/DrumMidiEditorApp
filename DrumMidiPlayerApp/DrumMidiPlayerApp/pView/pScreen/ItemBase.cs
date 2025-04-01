using DrumMidiLibrary.pUtil;

namespace DrumMidiPlayerApp.pView.pScreen;

/// <summary>
/// 描画アイテム：処理中
/// </summary>
internal partial class ItemBase : DisposeBaseClass
{
    /// <summary>
    /// 現在のフレーム時間
    /// </summary>
    private double _CurrentFrameTime = 0.0F;

    /// <summary>
    /// フレーム終了時間
    /// </summary>
    protected double EndFrameTime { get; set; } = 0.0F;

    /// <summary>
    /// フレーム進捗率（0～1)
    /// </summary>
    public float FrameProgress
        => EndFrameTime <= 0 ? 1 : (float)( _CurrentFrameTime / EndFrameTime );

    /// <summary>
    /// フレーム処理
    /// </summary>
    /// <param name="aFrameTime"></param>
    public virtual void Move( double aFrameTime )
    {
        // フレーム時間計算
        _CurrentFrameTime += aFrameTime;
        _CurrentFrameTime %= EndFrameTime;
    }
}
