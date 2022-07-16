using System;
using System.Drawing;
using System.Windows.Forms;

using DrumMidiEditor.pGeneralFunction.pLog;

namespace DrumMidiEditor.pGeneralFunction.pUtil;

/// <summary>
/// フォームユーティリティ
/// </summary>
public static class FormUtil
{
    /// <summary>
    /// ファイルを開くダイアログ共通処理
    /// </summary>
    /// <param name="aInitialDirectory">初期ディレクトリ</param>
    /// <param name="aFilter">フィルター設定</param>
    /// <param name="aFilePath">選択したファイルパス名（戻り値）</param>
    /// <returns>True:選択、False:未選択</returns>
    public static bool OpenShowDialog( GeneralPath aInitialDirectory, string aFilter, out GeneralPath aFilePath )
    {
        aFilePath = new();

        try
        {
            using var sfd = new OpenFileDialog
            {
                InitialDirectory    = aInitialDirectory.AbsoulteFolderPath,
                Filter              = aFilter,
                FilterIndex         = 1,
                Multiselect         = false
            };

            if ( sfd.ShowDialog() != DialogResult.OK )
            {
                return false;
            }
            else
            {
                aFilePath = new( sfd.FileName );

                if ( aFilePath.Length == 0 )
                {
                    return false;
                }
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
            return false;
        }
        return true;
    }

    /// <summary>
    /// ファイル保存ダイアログ共通処理
    /// </summary>
    /// <param name="aInitialDirectory">初期ディレクトリ</param>
    /// <param name="aFileName">初期ファイル名</param>
    /// <param name="aFilter">フィルター設定</param>
    /// <param name="aFilePath">保存したファイルパス名（戻り値）</param>
    /// <returns>True:保存、False:未保存</returns>
    public static bool SaveShowDialog( GeneralPath aInitialDirectory, GeneralPath aFileName, string aFilter, out GeneralPath aFilePath )
    {
        aFilePath = new();

        try
        {
            using var sfd = new SaveFileDialog
            {
                InitialDirectory    = aInitialDirectory.AbsoulteFolderPath,
                FileName            = aFileName.FileNameWithoutExtension,
                Filter              = aFilter,
                FilterIndex         = 1
            };
            if ( sfd.ShowDialog() != DialogResult.OK )
            {
                return false;
            }

            aFilePath = new( sfd.FileName );

            if ( aFilePath.Length == 0 )
            {
                return false;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
            return false;
        }

        return true;
    }

    /// <summary>
    /// 色選択ダイアログ共通処理
    /// </summary>
    /// <param name="aColorBef">変更前の色</param>
    /// <param name="aColorAft">変更後の色</param>
    /// <returns>True:選択、False:未選択</returns>
    public static bool SelectColor( Color aColorBef, out Color aColorAft )
	{
        using var cd = new ColorDialog
        {
            CustomColors    = new int[] { aColorBef.ToArgb() },
            Color           = aColorBef,
            AllowFullOpen   = true,
            SolidColorOnly  = false
        };

        if ( cd.ShowDialog() != DialogResult.OK )
        {
            aColorAft = aColorBef;
            return false;
        }
        else
        {
            aColorAft = cd.Color;
            return true;
        }
    }

    /// <summary>
    /// フォント選択ダイアログ共通処理
    /// </summary>
    /// <param name="aFontBef">変更前のフォント</param>
    /// <param name="aColorBef">変更前の文字色</param>
    /// <param name="aFontAft">変更後のフォント</param>
    /// <param name="aColorAft">変更後の文字色</param>
    /// <returns>True:選択、False:未選択</returns>
    public static bool SelectFont( Font aFontBef, Color aColorBef, out Font aFontAft, out Color aColorAft )
	{
        using var fd = new FontDialog
        {
            Font        = aFontBef,
            Color       = aColorBef,
            ShowEffects = true,
            ShowColor   = true,
            ShowHelp    = false
        };

        if ( fd.ShowDialog() != DialogResult.OK )
        {
            aFontAft    = aFontBef;
            aColorAft   = aColorBef;
            return false;
        }
        else
        {
            aFontAft    = fd.Font;
            aColorAft   = fd.Color;
            return true;
        }
    }

    /// <summary>
    /// aMousePos が aRange の範囲内かどうか判定
    /// </summary>
    /// <param name="aMousePos">チェック位置</param>
    /// <param name="aRange">チェック範囲</param>
    /// <returns>True:範囲内にある、False:範囲外にある</returns>
    public static bool CheckRange( in Point aMousePos, in Rectangle aRange )
    {
        return (    aRange.Left <= aMousePos.X && aMousePos.X <= aRange.Right
                 && aRange.Top  <= aMousePos.Y && aMousePos.Y <= aRange.Bottom );
    }

    /// <summary>
    /// aMousePos が aRange の範囲内かどうか判定
    /// </summary>
    /// <param name="aMousePos">チェック位置</param>
    /// <param name="aRange">チェック範囲</param>
    /// <returns>True:範囲内にある、False:範囲外にある</returns>
    public static bool CheckRange( in Point aMousePos, in RectangleF aRange )
    {
        return (    aRange.Left <= aMousePos.X && aMousePos.X <= aRange.Right
                 && aRange.Top  <= aMousePos.Y && aMousePos.Y <= aRange.Bottom );
    }

    /// <summary>
    /// aMousePos が aRange の範囲内に収まるようにPoint調整して返す
    /// </summary>
    /// <param name="aMousePos">チェック位置</param>
    /// <param name="aRange">チェック範囲</param>
    /// <returns>修正後座標</returns>
    public static Point CheckRangeIn( in Point aMousePos, in Rectangle aRange )
    {
        var ret = aMousePos;

        if ( ret.X < aRange.Left )
        {
            ret.X = aRange.Left;
        }
        else if ( ret.X > aRange.Right )
        {
            ret.X = aRange.Right;
        }

        if ( ret.Y < aRange.Top )
        {
            ret.Y = aRange.Top;
        }
        else if ( ret.Y > aRange.Bottom )
        {
            ret.Y = aRange.Bottom;
        }

        return ret;
    }

    /// <summary>
    /// aSelectValue の値を元に ListBox内の表示文字でマッチするアイテムを選択する
    /// </summary>
    /// <param name="aListBox">ListBox</param>
    /// <param name="aSelectValue">選択対象の文字列</param>
    /// <returns>True:選択、False:未選択</returns>
    public static bool SelectListBoxValue( ListBox aListBox, string aSelectValue )
    {
        for ( int i = 0; i < aListBox.Items.Count; i++ )
        {
            if ( aListBox.Items[ i ].ToString() == aSelectValue )
            {
                aListBox.SelectedIndex = i;
                return true;
            }
        }
        return false;
    }
}
