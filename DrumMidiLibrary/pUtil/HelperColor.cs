using System;
using System.Text.RegularExpressions;
using DrumMidiLibrary.pLog;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// 色変換補助
/// </summary>
public static partial class HelperColor
{
    #region 定数

    /// <summary>
    /// 空色
    /// </summary>
    public static Color EmptyColor { get; } = Color.FromArgb( 0, 0, 0, 0 );

    /// <summary>
    /// 黒色
    /// </summary>
    public static Color BlackColor { get; } = Color.FromArgb( 255, 0, 0, 0 );

    /// <summary>
    /// 白色
    /// </summary>
    public static Color WhiteColor { get; } = Color.FromArgb( 255, 255, 255, 255 );

    #endregion

    /// <summary>
    /// #FFFFFF 表記の色、または ARGB値 から Color を返す
    /// </summary>
    /// <param name="aColorText">色を表す文字列 (#RGB, #ARGB, #RRGGBB, #AARRGGBB)</param>
    /// <returns>Color オブジェクト</returns>
    /// <exception cref="ArgumentException">無効な色形式の場合にスローされます</exception>
    public static Color GetColor( string aColorText )
    {
        try
        {
            // 先頭の#を除去。文字列を全角に変換
            var colorText = aColorText.StartsWith( '#' ) ? aColorText[1..].ToUpper() : aColorText.ToUpper();

            if ( !ColorRegex().IsMatch( colorText ) )
            {
                throw new ArgumentException( $"Invalid color format", nameof( aColorText ) );
            }

            // #RGB
            // #ARGB
            // #RRGGBB
            // #AARRGGBB

            var length = colorText.Length;
            var keta   = length / 4;
            var alpha  = length is 4 or 8 ? Convert.ToByte( colorText[ ..keta  ], 16 ) : (byte)255 ;
            var offset = length is 4 or 8 ? keta : 0 ;

            return Color.FromArgb
                (
                    alpha,
                    Convert.ToByte( colorText.Substring( offset            , keta ), 16 ),
                    Convert.ToByte( colorText.Substring( offset + keta     , keta ), 16 ),
                    Convert.ToByte( colorText.Substring( offset + keta * 2 , keta ), 16 )
                );
        }
        catch ( Exception e ) 
        {
            Log.Error( e );

            throw new ArgumentException( $"Color conversion error: {e.Message}", nameof( aColorText ), e );
        }
    }

    [GeneratedRegex( "^(?:[0-9A-F]{3}|[0-9A-F]{4}|[0-9A-F]{6}|[0-9A-F]{8})$" )]
    private static partial Regex ColorRegex();

    /// <summary>
    /// Color オブジェクトを #FFFFFFFF 表記の文字列に変換する
    /// </summary>
    /// <param name="aColor">Color オブジェクト</param>
    /// <returns>#FFFFFF 表記の文字列</returns>
    public static string GetColorText( Color aColor )
        => $"#{aColor.A:X2}{aColor.R:X2}{aColor.G:X2}{aColor.B:X2}";

    /// <summary>
    /// Color オブジェクトから SolidColorBrush を生成する
    /// </summary>
    /// <param name="aColor">Color オブジェクト</param>
    /// <returns>SolidColorBrush オブジェクト</returns>
    public static Brush GetColorBrush( Color aColor )
        => new SolidColorBrush( aColor );

    /// <summary>
    /// Brush から Color を取得する
    /// </summary>
    /// <param name="aBrush">Brush オブジェクト</param>
    /// <returns>Color オブジェクト</returns>
    /// <exception cref="ArgumentException">Brush が SolidColorBrush でない場合にスローされます</exception>
    public static Color GetColor( Brush aBrush )
    {
        if ( aBrush is null )
        {
            throw new ArgumentNullException( nameof( aBrush ), "Brush cannot be null." );
        }
        if ( aBrush is SolidColorBrush solidColorBrush )
        {
            return solidColorBrush.Color;
        }
        throw new ArgumentException( "Invalid brush type. Expected SolidColorBrush.", nameof( aBrush ) );
    }
}
