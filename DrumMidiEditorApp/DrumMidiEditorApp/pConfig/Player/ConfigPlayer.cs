using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Windows.Foundation;
using Windows.UI;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// プレイヤー設定
/// </summary>
public class ConfigPlayer
{
    #region プレイヤー描画モード

    /// <summary>
    /// 編集モード
    /// </summary>
    [JsonIgnore]
    public bool EditModeOn { get; set; } = false;

    /// <summary>
    /// プレイヤー描画モード
    /// </summary>
    public enum PlayerSurfaceMode : int
    {
        Sequence = 0,
        SequenceVertical,
        Score,
        Simuration,
    }

    /// <summary>
    /// プレイヤー描画モード
    /// </summary>
    [JsonInclude]
    public PlayerSurfaceMode PlayerSurfaceModeSelect = PlayerSurfaceMode.Sequence;

    #endregion

    #region 解像度/FPS

    /// <summary>
    /// FPS
    /// </summary>
    [JsonInclude]
    public float Fps { get; set; } = 90F;

    /// <summary>
    /// 解像度リスト
    /// </summary>
    [JsonInclude]
    public readonly List<Size> ResolutionScreenList = new()
    {
        new(  320,  240 ),
        new(  480,  360 ),
        new(  640,  480 ),
	//	new( 1024,  768 ),
	//	new( 1280,  720 ),
	//	new( 1440,  900 ),
	//	new( 1920, 1024 ),
    //  new(  240,  360 ),  // MP4出力が正常にできない
    //  new(  360,  480 ),  // MP4出力が正常にできない
        new(  480,  640 ),
    };

    /// <summary>
    /// 解像度リスト選択インデックス
    /// </summary>
    [JsonInclude]
    public int ResolutionScreenIndex { get; set; } = 1;

    /// <summary>
    /// 解像度：横幅
    /// </summary>
	public float ResolutionScreenWidth => ResolutionScreenList[ ResolutionScreenIndex ]._width;

    /// <summary>
    /// 解像度：高さ
    /// </summary>
	public float ResolutionScreenHeight => ResolutionScreenList[ ResolutionScreenIndex ]._height;

    #endregion

    #region フラグ

    /// <summary>
    /// PlayerConfig表示フラグ
    /// </summary>
    [JsonIgnore]
    public bool DisplayPlayerConfig { get; set; } = false;

    /// <summary>
    /// Player表示フラグ
    /// </summary>
    [JsonIgnore]
    public bool DisplayPlayer { get; set; } = false;

    /// <summary>
    /// スコア更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreFlag { get; set; } = false;

    #endregion

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

    #region 再生リクエスト

    /// <summary>
    /// プレイヤー再生リクエスト一覧
    /// </summary>
    public enum PlayRequest : int
    {
        None = 0
    ,   PreStop
    ,   PrePlay
    ,   PreLoopPlay
    ,   PreRecord
    }

    /// <summary>
    /// プレイヤー再生リクエスト
    /// </summary>
    [JsonIgnore]
    public PlayRequest PlayReq { get; set; } = PlayRequest.None;

    #endregion

    /// <summary>
    /// プレイヤー描画モード別設定
    /// </summary>
    [JsonInclude]
    public ConfigPlayerSequence Sequence { get; set; } = new();

    /// <summary>
    /// プレイヤー描画モード別設定
    /// </summary>
    [JsonInclude]
    public ConfigPlayerScore Score { get; set; } = new();

    /// <summary>
    /// プレイヤー描画モード別設定
    /// </summary>
    [JsonInclude]
    public ConfigPlayerSimuration Simuration { get; set; } = new();

    /// <summary>
    /// プレイヤー描画モード別設定
    /// </summary>
    [JsonInclude]
    public ConfigPlayerSequenceVertical Vertical { get; set; } = new();
}
