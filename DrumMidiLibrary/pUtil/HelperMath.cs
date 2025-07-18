﻿using System;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// 数学ユーティリティ
/// </summary>
public static class HelperMath
{
    #region 円

    /// <summary>
    /// 円周の位置取得
    /// </summary>
    /// <param name="aCenterX">基準値X</param>
    /// <param name="aCenterY">基準値Y</param>
    /// <param name="aRadius">半径</param>
    /// <param name="aAngle">角度(360度)</param>
    /// <returns>円周上の位置 (例: X=1.0, Y=0.0)</returns>
    public static (float X, float Y) GetCirclePosition( float aCenterX, float aCenterY, float aRadius, float aAngle )
    {        
        var angle = aAngle / 360F * MathF.Tau;

        return ( aCenterX + aRadius * MathF.Cos( angle ), 
                 aCenterY + aRadius * MathF.Sin( angle ) );
    }
    #endregion

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
        return MathF.Pow( 1 - aWeight, 3 ) * aStartXorY 
            +  3 * MathF.Pow( 1 - aWeight, 2 ) * aWeight * aCtrl1XorY 
            +  3 * ( 1 - aWeight ) * MathF.Pow( aWeight, 2 ) * aCtrl2XorY 
            +  MathF.Pow( aWeight, 3 ) * aEndXorY ;
    }

    #endregion

    /// <summary>
    /// 36進数変換：整数（0-1295）を36進数（00～ZZ）で表現
    /// </summary>
    /// <param name="aValue">整数（0-1295）</param>
    /// <returns>36進数（00～ZZ）</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string ToBase36( int aValue )
    {
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        if ( aValue < 0 || aValue > 1295 )
        {
            throw new ArgumentOutOfRangeException( nameof( aValue ), "範囲は 0～1295 です" );
        }
        return $"{chars [ aValue / 36 ]}{chars [ aValue % 36 ]}";
    }
}
