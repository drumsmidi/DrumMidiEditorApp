using System.Collections.Generic;
using System.Text.Json.Serialization;
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
    public readonly List<(int Width, int Height)> ResolutionScreenList = new()
    {
		new() { Width =  320, Height =  240 },
		new() { Width =  480, Height =  360 },
		new() { Width =  640, Height =  480 },
	//	new() { Width = 1024, Height =  768 },
	//	new() { Width = 1280, Height =  720 },
	//	new() { Width = 1440, Height =  900 },
	//	new() { Width = 1920, Height = 1024 },
    //  new() { Width =  240, Height =  360 },  // MP4出力が正常にできない
    //  new() { Width =  360, Height =  480 },  // MP4出力が正常にできない
        new() { Width =  480, Height =  640 },
    };

    /// <summary>
    /// 解像度リスト選択インデックス
    /// </summary>
    [JsonInclude]
    public int ResolutionScreenIndex { get; set; } = 1;

    /// <summary>
    /// 解像度：横幅
    /// </summary>
	public int ResolutionScreenWidth => ResolutionScreenList[ ResolutionScreenIndex ].Width;

    /// <summary>
    /// 解像度：高さ
    /// </summary>
	public int ResolutionScreenHeight => ResolutionScreenList[ ResolutionScreenIndex ].Height;

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
    public Color SheetColor { get; set; } = Color.FromArgb( 255, 0, 0, 0 );

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
