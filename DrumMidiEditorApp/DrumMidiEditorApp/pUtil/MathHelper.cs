using System;

namespace DrumMidiEditorApp.pUtil;

/// <summary>
/// 数学ユーティリティ
/// </summary>
public static class MathHelper
{
    #region ベジェ曲線

    /// <summary>
    /// ベジェ曲線 位置取得（X,Yそれぞれで計算が必要）
    /// Refer: http://contoracutar.blog.fc2.com/blog-entry-1.html
    /// </summary>
    /// <param name="aStartXorY">開始位置</param>
    /// <param name="aCtrl1XorY">制御位置１</param>
    /// <param name="aCtrl2XorY">制御位置２</param>
    /// <param name="aEndXorY">終了位置</param>
    /// <param name="aWeight">重み（0-1）</param>
    /// <returns>ベジェ曲線曲線 位置</returns>
    public static float GetBezierCurvePosition( float aStartXorY, float aCtrl1XorY, float aCtrl2XorY, float aEndXorY, float aWeight )
    {
        return ( MathF.Pow( 1 - aWeight, 3 ) * aStartXorY )
            + ( 3 * MathF.Pow( 1 - aWeight, 2 ) * aWeight * aCtrl1XorY )
            + ( 3 * ( 1 - aWeight ) * MathF.Pow( aWeight, 2 ) * aCtrl2XorY )
            + ( MathF.Pow( aWeight, 3 ) * aEndXorY );
    }

    #endregion
}
