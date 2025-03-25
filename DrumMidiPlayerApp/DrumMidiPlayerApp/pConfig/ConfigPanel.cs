using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;
using DrumMidiLibrary.pUtil;
using Windows.Foundation;
using Windows.UI;

namespace DrumMidiPlayerApp.pConfig;

/// <summary>
/// プレイヤー設定
/// </summary>
public class ConfigPanel
{
    #region 更新フラグ

    /// <summary>
    /// スコア更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool FlagUpdateScore { get; set; } = true;

    /// <summary>
    /// プレイヤー描画モード更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool FlagUpdateSurfaceModo { get; set; } = true;

    /// <summary>
    /// サイズ更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool FlagUpdateSize { get; set; } = false;

    #endregion

    #region プレイヤー描画モード

    /// <summary>
    /// 描画モード
    /// </summary>
    public enum SurfaceMode : int
    {
        SongSelect = 0,
        Player_ScoreType2,
    }

    /// <summary>
    /// 描画モード
    /// </summary>
    [JsonIgnore]
    public SurfaceMode SurfaceModeSelect = SurfaceMode.SongSelect;

    #endregion

    #region 解像度/FPS

    /// <summary>
    /// FPS
    /// </summary>
    [JsonInclude]
    public float Fps { get; set; } = 60F;

    /// <summary>
    /// ベーススクリーンサイズ
    /// </summary>
    [JsonIgnore]
    public readonly Size BaseScreenSize = new( 1920, 1080 );

    /// <summary>
    /// スクリーン倍率
    /// </summary>
    [JsonIgnore]
    public Vector2 ScreenMagnification 
        => new( ResolutionScreenWidth / BaseScreenSize._width, ResolutionScreenHeight / BaseScreenSize._height );

    /// <summary>
    /// 解像度リスト
    /// chatgptよりサイズ情報を抽出
    /// </summary>
    [JsonIgnore]
    public readonly List<Size> ResolutionScreenList =
    [
        new( 1920, 1080 ), // フルHD        : 最も一般的な解像度。多くのディスプレイやテレビで使用されています
        new( 1366,  768 ), // HD            : よくノートパソコンに使用される解像度。省電力で基本的な用途に適しています。
        new( 1600,  900 ), // HD+           : 少し広めの画面解像度。1366x768よりも広い作業領域を提供します。
        new( 2560, 1440 ), // QHD           : 高解像度ディスプレイの一つ。多くの中〜高級ディスプレイやゲーミングモニターで使用されます。
        new( 3840, 2160 ), // 4K UHD        : 4K 解像度。高精細な表示や大きなスクリーンで使用されることが多い
        new( 3200, 1800 ), // QHD+          : 高解像度であり、特にラップトップにおいて人気のある解像度
        new( 2880, 1620 ), //               : 高解像度のスクリーンやゲーミングモニターで見かけることがある解像度
        new( 4096, 2160 ), // DCI 4K        : 映画や高品質なビデオ制作で使用される4Kの標準解像度
        new( 1360,  768 ), // WXGA          : より小型のスクリーンやモバイル機器で見られる解像度
        new( 1280, 1024 ), // SXGA          : 古いスクリーンやモニターでよく使用されていた解像度。スクエア型のディスプレイに最適
        new( 2560, 1080 ), // UltraWide     : ウルトラワイドモニターに適した解像度。横に広い作業スペースを提供します
        new( 3440, 1440 ), // UltraWide QHD : ウルトラワイドQHDディスプレイで使用され、映画やゲームで広い視野を提供します
        new( 5120, 1440 ), // 5K UltraWide  : 高解像度のウルトラワイドディスプレイ。広範な作業スペースを提供するプロフェッショナルな用途で使用されます。
        new( 7680, 4320 ), // 8K UHD        : 8K 解像度。非常に高精細な表示を提供しますが、一般的にはまだ普及していません
    ];

    /// <summary>
    /// 解像度リスト選択インデックス
    /// </summary>
    [JsonIgnore]
    public int ResolutionScreenIndex { get; set; } = 0;

    /// <summary>
    /// 解像度：横幅
    /// </summary>
    [JsonIgnore]
    public float ResolutionScreenWidth => ResolutionScreenList [ ResolutionScreenIndex ]._width;

    /// <summary>
    /// 解像度：高さ
    /// </summary>
    [JsonIgnore]
    public float ResolutionScreenHeight => ResolutionScreenList [ ResolutionScreenIndex ]._height;

    #endregion

    #region Sheet

    /// <summary>
    /// 背景色
    /// </summary>
    [JsonIgnore]
    public FormatColor SheetColor
    {
        get; set;
    } = new()
    {
        Color = Color.FromArgb( 255, 30, 30, 30 ),
    };

    #endregion

}
