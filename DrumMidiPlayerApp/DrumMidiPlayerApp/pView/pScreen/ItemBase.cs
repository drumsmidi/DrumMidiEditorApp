using System;
using DrumMidiLibrary.pUtil;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pScreen;

/// <summary>
/// 描画アイテム：処理中
/// </summary>
/// <param name="aLayer">描画優先順</param>
/// <param name="aX">描画位置</param>
/// <param name="aY">描画位置</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
internal partial class ItemBase( int aLayer, float aX, float aY, float aWidth, float aHeight )
    : DisposeBaseClass, IComparable, IComparable<ItemBase>
{
    #region 描画関連

    /// <summary>
    /// 描画優先順
    /// </summary>
    public int Layer = aLayer;

    /// <summary>
    /// 描画範囲
    /// </summary>
    public Rect DrawRect = new( aX, aY, aWidth, aHeight );

    /// <summary>
    /// 描画順 並替用
    /// </summary>
    /// <param name="aOther"></param>
    /// <returns></returns>
    public int CompareTo( ItemBase? aOther )
    {
        if ( aOther == null )
        {
            return 1;
        }
        else if ( Layer < aOther.Layer )
        {
            return -1;
        }
        else if ( Layer > aOther.Layer )
        {
            return 1;
        }
        else if ( DrawRect.X < aOther.DrawRect.X )
        {
            return -1;
        }
        else if ( DrawRect.X > aOther.DrawRect.X )
        {
            return 1;
        }
        else if ( DrawRect.Y < aOther.DrawRect.Y )
        {
            return -1;
        }
        else if ( DrawRect.Y > aOther.DrawRect.Y )
        {
            return 1;
        }
        else if ( DrawRect.X == aOther.DrawRect.X )
        {
            return 0;
        }
        return -1;
    }

    /// <summary>
    /// 描画順 並替用
    /// </summary>
    /// <param name="aOther"></param>
    /// <returns></returns>
    public int CompareTo( object? aOther )
        => aOther == null ? 1 : GetType() != aOther.GetType()
            ? throw new ArgumentException( "Invalid aOther", nameof( aOther ) )
            : CompareTo( aOther as ItemBase );

    #endregion

    #region フレーム関連

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

    #endregion
}
