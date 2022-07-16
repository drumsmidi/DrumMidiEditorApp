using System;
using System.Drawing;

namespace DrumMidiEditor.pGeneralFunction.pUtil;

/// <summary>
/// 文字列ユーティリティ
/// </summary>
public static class StringUtil
{
    /// <summary>
    /// #FFFFFF 表記の色、または ARGB値 からColorを返す
    /// </summary>
    /// <param name="aValue">テキスト</param>
    /// <returns>Color</returns>
    public static Color GetColor( string aValue )
    {
        Color c;

        try
        {
            c = ColorTranslator.FromHtml( aValue.StartsWith( "#" ) ? aValue : $"#{aValue}" );
        }
        catch ( Exception )
        {
            try
            {
                c = Color.FromArgb( int.Parse( aValue ) );
            }
            catch ( Exception )
            {
                throw;
            }
        }
        return c;
    }

    /// <summary>
    /// #FFFFFF 表記の文字を返す
    /// </summary>
    /// <param name="aColor">Color</param>
    /// <returns>#FFFFFF 表記の文字</returns>
    public static string GetColor( Color aColor )
        => ( $"#{aColor.R:X2}{aColor.G:X2}{aColor.B:X2}" ).ToUpper();
}
