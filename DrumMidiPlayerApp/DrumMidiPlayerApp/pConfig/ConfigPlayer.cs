﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Windows.Foundation;
using Windows.UI;
using static DrumMidiLibrary.pConfig.ConfigSystem;

namespace DrumMidiPlayerApp.pConfig;

/// <summary>
/// プレイヤー設定
/// </summary>
public class ConfigPlayer
{
    #region 更新フラグ

    /// <summary>
    /// スコア更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool FlagUpdateScore { get; set; } = false;

    /// <summary>
    /// プレイヤー描画モード更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool FlagUpdateSurfaceModo { get; set; } = false;

    /// <summary>
    /// サイズ更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool FlagUpdateSize { get; set; } = false;

    #endregion

    #region プレイヤー描画モード

    /// <summary>
    /// Player表示フラグ
    /// </summary>
    [JsonIgnore]
    public bool DisplayPlayer { get; set; } = false;

    /// <summary>
    /// Player表示フラグ
    /// </summary>
    [JsonIgnore]
    public Visibility DisplayPlayerVisibility
    {
        get => DisplayPlayer ? Visibility.Visible : Visibility.Collapsed;
        set => DisplayPlayer = value == Visibility.Visible;
    }

    /// <summary>
    /// 編集モード
    /// </summary>
    [JsonIgnore]
    public bool EditModeOn { get; set; } = false;

    /// <summary>
    /// プレイヤー描画モード
    /// </summary>
    [JsonInclude]
    public PlayerSurfaceMode PlayerSurfaceModeSelect = PlayerSurfaceMode.ScoreType2;

    /// <summary>
    /// プレイヤー描画モード
    /// </summary>
    [JsonIgnore]
    public int PlayerSurfaceModeSelectIndex
    {
        get => (int)PlayerSurfaceModeSelect;
        set => PlayerSurfaceModeSelect = (PlayerSurfaceMode)value;
    }

    /// <summary>
    /// プレイヤー描画エフェクトモード
    /// </summary>
    public enum PlayerSurfaceEffectMode : int
    {
        AlphaMaskEffect = 0,
        ArithmeticCompositeEffect,
        AtlasEffect,
        BlendEffect,
        BorderEffect,
        BrightnessEffect,
        ChromaKeyEffect,
        ColorManagementEffect,
        ColorManagementProfile,
        ColorMatrixEffect,
        ColorSourceEffect,
        CompositeEffect,
        ContrastEffect,
        ConvolveMatrixEffect,
        CropEffect,
        CrossFadeEffect,
        DirectionalBlurEffect,
        DiscreteTransferEffect,
        DisplacementMapEffect,
        DistantDiffuseEffect,
        DistantSpecularEffect,
        DpiCompensationEffect,
        EdgeDetectionEffect,
        EffectTransferTable3D,
        EmbossEffect,
        ExposureEffect,
        GammaTransferEffect,
        GaussianBlurEffect,
        GrayscaleEffect,
        HighlightsAndShadowsEffect,
        HueRotationEffect,
        HueToRgbEffect,
        InvertEffect,
        LinearTransferEffect,
        LuminanceToAlphaEffect,
        MorphologyEffect,
        OpacityEffect,
        OpacityMetadataEffect,
        PixelShaderEffect,
        PointDiffuseEffect,
        PointSpecularEffect,
        PosterizeEffect,
        PremultiplyEffect,
        RgbToHueEffect,
        SaturationEffect,
        ScaleEffect,
        SepiaEffect,
        ShadowEffect,
        SharpenEffect,
        SpotDiffuseEffect,
        SpotSpecularEffect,
        StraightenEffect,
        TableTransfer3DEffect,
        TableTransferEffect,
        TemperatureAndTintEffect,
        TileEffect,
        TintEffect,
        Transform2DEffect,
        Transform3DEffect,
        TurbulenceEffect,
        UnPremultiplyEffect,
        VignetteEffect,
    }

    /// <summary>
    /// プレイヤー描画エフェクトモード
    /// </summary>
    [JsonInclude]
    public PlayerSurfaceEffectMode PlayerSurfaceEffectModeSelect = PlayerSurfaceEffectMode.AtlasEffect;

    /// <summary>
    /// プレイヤー描画エフェクトモード
    /// </summary>
    [JsonIgnore]
    public int PlayerSurfaceEffectModeSelectIndex
    {
        get => (int)PlayerSurfaceEffectModeSelect;
        set => PlayerSurfaceEffectModeSelect = (PlayerSurfaceEffectMode)value;
    }

    #endregion

    #region 解像度/FPS

    /// <summary>
    /// FPS
    /// </summary>
    [JsonInclude]
    public float Fps { get; set; } = 60F;

    /// <summary>
    /// 解像度リスト
    /// </summary>
    [JsonInclude]
    public readonly List<Size> ResolutionScreenList =
    [
        new(  1280, 1024 ),
    ];

    /// <summary>
    /// 解像度リスト選択インデックス
    /// </summary>
    [JsonInclude]
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
            Color = Color.FromArgb( 255, 0, 0, 0 ),
        };

    #endregion

    #region 個別設定

    /// <summary>
    /// プレイヤー描画モード別設定
    /// </summary>
    [JsonInclude]
    public ConfigPlayerScoreType2 ScoreType2 = new( true );

    #endregion
}
