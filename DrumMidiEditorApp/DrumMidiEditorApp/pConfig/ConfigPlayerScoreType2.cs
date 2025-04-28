using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// プレイヤー設定
/// </summary>
public class ConfigPlayerScoreType2 : IConfig
{
    public void CheckValidation()
    {
        BpmHeightSize        = Math.Max( BpmHeightSize          , 0 );
        BpmWidthSize         = Math.Max( BpmWidthSize           , 0 );
        MeasureNoHeightSize  = Math.Max( MeasureNoHeightSize    , 0 );
        NoteHeightSize       = Math.Max( NoteHeightSize         , 0 );
        NoteWidthSize        = Math.Max( NoteWidthSize          , 0 );
        NoteTermWidthSize    = Math.Max( NoteTermWidthSize      , 0.1F );
        NoteTermHeightSize   = Math.Max( NoteTermHeightSize     , 0.1F );
    }

    #region Sheet

    /// <summary>
    /// 背景色
    /// </summary>
    [JsonInclude]
    public FormatColor SheetColor { get; set; } = new()
    {
        Color = Color.FromArgb( 255, 0, 0, 0 ),
    };

    #endregion

    #region Bpm

    /// <summary>
    /// BPM行の高さ
    /// </summary>
    [JsonInclude]
    public float BpmHeightSize { get; set; } = 18F;

    /// <summary>
    /// BPMテキスト表示横幅
    /// </summary>
    [JsonInclude]
    public float BpmWidthSize { get; set; } = 100F;

    /// <summary>
    /// 現在のBPM値表示フラグ
    /// </summary>
    [JsonInclude]
    public bool BpmNowDisplay { get; set; } = true;

    /// <summary>
    /// 現在のBPM値描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect BpmNowRect { get; set; } = new()
    {
        Background  = new( Color.FromArgb(   0,   0,   0,   0 ) ),
        Line        = new( Color.FromArgb(   0,   0,   0,   0 ), 0F ),
        Text        = new( Color.FromArgb( 255, 255, 255, 255 ),
                            new()
                            {
                                FontFamily          = Config.Media.DefaultFontFamily,
                                FontSize            = 14F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    #endregion

    #region Measure number

    /// <summary>
    /// 小節番号行の高さ
    /// </summary>
    [JsonInclude]
    public float MeasureNoHeightSize { get; set; } = 18F;

    /// <summary>
    /// 小節番号描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect MeasureNoRect { get; set; } = new()
    {
        Background  = new( Color.FromArgb( 255, 150, 150, 150 ) ),
        Line        = new( Color.FromArgb( 255,   0,   0,   0 ), 1.0F ),
        Text        = new( Color.FromArgb( 255,   0,   0,   0 ),
                            new()
                            {
                                FontFamily          = Config.Media.DefaultFontFamily,
                                FontSize            = 14F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// 小節番号描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect MeasureNoSelectRect { get; set; } = new()
    {
        Background  = new( Color.FromArgb( 255, 255, 255, 255 ) ),
        Line        = new( Color.FromArgb( 255,   0,   0,   0 ), 1.0F ),
        Text        = new( Color.FromArgb( 255,   0,   0,   0 ),
                            new()
                            {
                                FontFamily          = Config.Media.DefaultFontFamily,
                                FontSize            = 14F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    #endregion

    #region Header

    /// <summary>
    /// ヘッダー描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatLine HeaderLineA { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 255, 255, 255 ) ),
        LineSize    = 1.0F,
    };

    /// <summary>
    /// ヘッダー描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatLine HeaderLineB { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 170, 170, 170 ) ),
        LineSize    = 0.4F,
    };

    #endregion

    #region Note

    /// <summary>
    /// NOTE描画アイテム
    /// </summary>
    [JsonInclude]
    public FormatRect NoteRect { get; set; } = new()
    {
        Background  = new( Color.FromArgb( 255, 255, 255, 255 ) ),
        Line        = new( Color.FromArgb( 255, 255, 255, 255 ), 3.0F ),
        Text        = new( Color.FromArgb( 255, 255, 255, 255 ),
                            new()
                            {
                                FontFamily          = Config.Media.DefaultFontFamily,
                                FontSize            = 24F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Center,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// ノート高さ
    /// </summary>
    [JsonInclude]
    public float NoteHeightSize { get; set; } = 10F;

    /// <summary>
    /// ノート横幅
    /// </summary>
    [JsonInclude]
    public float NoteWidthSize { get; set; } = 10F;

    /// <summary>
    /// 音量ゼロノート表示フラグ
    /// </summary>
    [JsonInclude]
    public bool NoteVolumeZeroOn { get; set; } = false;

    /// <summary>
    /// 音量サイズ変更フラグ
    /// </summary>
    [JsonInclude]
    public bool NoteVolumeSizeOn { get; set; } = true;

    /// <summary>
    /// ノートテキスト表示フラグ
    /// </summary>
    [JsonInclude]
    public bool NoteTextOn { get; set; } = true;

    /// <summary>
    /// ノート間隔：横
    /// </summary>
    [JsonInclude]
    public float NoteTermWidthSize { get; set; } = 2.34F;

    /// <summary>
    /// ノート間隔：縦
    /// </summary>
    [JsonInclude]
    public float NoteTermHeightSize { get; set; } = 18.2F;

    #endregion

    #region Line

    /// <summary>
    /// カーソルペン
    /// </summary>
    [JsonInclude]
    public FormatLine CursorLine { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 255, 255, 255 ) ),
        LineSize    = 1.0F,
    };

    /// <summary>
    /// カーソル位置より前の塗りつぶり用
    /// </summary>
    [JsonInclude]
    public FormatRect CursorRect { get; set; } = new()
    {
        Background  = new( Color.FromArgb( 160,   0,   0,   0 ) ),
        Line        = new( Color.FromArgb(   0, 255, 255, 255 ), 0.0F ),
        Text        = new( Color.FromArgb(   0, 255, 255, 255 ),
                            new()
                            {
                                FontFamily          = Config.Media.DefaultFontFamily,
                                FontSize            = 24F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Center,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    /// <summary>
    /// カーソル位置より前の塗りつぶり表示フラグ
    /// </summary>
    [JsonInclude]
    public bool CursorRectOn { get; set; } = true;

    /// <summary>
    /// 小節128分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure128Line { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 255, 255, 255 ) ),
        LineSize    = 1.0F,
    };

    /// <summary>
    /// 小節64分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure064Line { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 255, 255, 255 ) ),
        LineSize    = 0.5F,
    };

    /// <summary>
    /// 小節32分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure032Line { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 255, 255, 255 ) ),
        LineSize    = 0.3F,
    };

    /// <summary>
    /// 小節16分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure016Line { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 200, 200, 200 ) ),
        LineSize    = 0.0F,
    };

    /// <summary>
    /// 小節8分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure008Line { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 190, 190, 190 ) ),
        LineSize    = 0.0F,
    };

    /// <summary>
    /// 小節4分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure004Line { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 180, 180, 180 ) ),
        LineSize    = 0.0F,
    };

    /// <summary>
    /// 小節1分間隔の線ペン
    /// </summary>
    [JsonInclude]
    public FormatLine SheetMeasure001Line { get; set; } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 170, 170, 170 ) ),
        LineSize    = 0.0F,
    };

    #endregion

    /// <summary>
    /// スコア最大高さ
    /// </summary>
    [JsonIgnore]
    public float ScoreMaxHeight => NoteTermHeightSize * ScaleList.Count;

    /// <summary>
    /// １小節の横幅
    /// </summary>
    [JsonIgnore]
    public float MeasureSize => NoteTermWidthSize * Config.System.MeasureNoteNumber;

    #region 音階

    /// <summary>
    /// 音階リスト
    /// </summary>
    [JsonInclude]
    public List<ConfigPlayerScoreType2ScaleItem> ScaleList =
    [
        new ( "DUMMY", "", true    ),
        new ( "CY"   , "", false   ),
        new ( "RD"   , "", true    ),
        new ( "HH"   , "", false   ),
        new ( "SD"   , "", false   ),
        new ( "TM"   , "", false   ),
      //new ( "HT"   , "", true    ),
      //new ( "MT"   , "", false   ),
      //new ( "LT"   , "", false   ),
      //new ( "FT1"  , "", false   ),
      //new ( "FT2"  , "", true    ),
        new ( "BD"   , "", true    ),
        new ( "PC"   , "", false   ),
    ];

    /// <summary>
    /// 音階リスト更新
    /// </summary>
    public void UpdateScaleList( List<ConfigPlayerScoreType2ScaleItem> aScaleList )
    {
        lock ( ScaleList )
        {
            ScaleList.Clear();
            aScaleList.ForEach( item => ScaleList.Add( new( item ) ) );
        }
    }

    /// <summary>
    /// 音階リストのインデックス番号取得
    /// </summary>
    /// <param name="aScaleKey">[音階キー] ( 例: "CY" )</param>
    /// <param name="aScaleKeyText">[音階テキスト] ( 例: "1" )</param>
    /// <returns>階リストのインデックス番号</returns>
    public (int, string) GetScaleListIndex( string aScaleKey, string aScaleKeyText )
    {
        if ( aScaleKey.Length == 0 )
        {
            return ( -1, string.Empty );
        }

        var index = -1;

        lock ( ScaleList )
        {
            foreach ( var item in ScaleList )
            {
                index++;

                if ( item.ScaleKey.Equals( aScaleKey ) )
                {
                    return ( index, aScaleKeyText );
                }
            }
        }

        return ( -1, string.Empty );
    }

    #endregion
}
