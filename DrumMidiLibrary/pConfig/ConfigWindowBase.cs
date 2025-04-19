using System;
using System.Text.Json.Serialization;
using DrumMidiLibrary.pLog;

namespace DrumMidiLibrary.pConfig;

/// <summary>
/// Window関連設定
/// </summary>
public abstract class ConfigWindowBase
{
    /// <summary>
    /// アプリケーション名
    /// </summary>
    [JsonIgnore]
    public string AppName { get; protected set; } = "DrumMidiLibrary";

    /// <summary>
    /// DPI
    /// </summary>
    [JsonInclude]
    public float DefaultDpi { get; set; } = 96;

    /// <summary>
    /// DPIスケール設定（0.1-10.0)
    /// </summary>
    /// <param name="aDpiScale">DPIスケール</param>
    public void SetDpiScale( double aDpiScale )
    {
        var min =  0.1D;
        var max = 10.0D;

        if ( aDpiScale < min || max < aDpiScale )
        {
            Log.Warning( $"Invalid DpiScale: {aDpiScale}" );
        }

        DpiScale = Math.Clamp( aDpiScale, min, max );

        SetWindowSizeDpiNoScale( WindowSizeWidthDpiNoScale, WindowSizeHeightDpiNoScale );
    }

    /// <summary>
    /// DPIスケール
    /// </summary>
    [JsonIgnore]
    public double DpiScale { get; private set; } = 1;

    /// <summary>
    /// ウィンドウ初期サイズ設定
    /// </summary>
    /// <param name="aWidth">横幅</param>
    /// <param name="aHeight">高さ</param>
    public void SetWindowSizeDpiNoScale( int aWidth, int aHeight )
    {
        #region WindowSizeWidthDpiNoScale
        {
            var min = 320;
            var max = 1280;

            if ( aWidth < min || max < aWidth )
            {
                Log.Warning( $"Invalid WindowSizeWidthDpiNoScale: {aWidth}" );
            }

            WindowSizeWidthDpiNoScale = Math.Clamp( aWidth, min, max );
        }
        #endregion

        #region WindowSizeHeightDpiNoScale
        {
            var min = 768;
            var max = 4320;

            if ( aHeight < min || max < aHeight )
            {
                Log.Warning( $"Invalid WindowSizeHeightDpiNoScale: {aHeight}" );
            }

            WindowSizeHeightDpiNoScale = Math.Clamp( aHeight, min, max );
        }
        #endregion

        // ウィンドウ初期サイズ ✕ DPIスケール
        WindowSizeWidthDpiScale  = (int)( WindowSizeWidthDpiNoScale  * DpiScale );
        WindowSizeHeightDpiScale = (int)( WindowSizeHeightDpiNoScale * DpiScale );
    }

    /// <summary>
    /// ウィンドウ初期サイズ：横幅
    /// </summary>
    [JsonIgnore]
    public int WindowSizeWidthDpiNoScale { get; private set; } = 1920;

    /// <summary>
    /// ウィンドウ初期サイズ：高さ
    /// </summary>
    [JsonIgnore]
    public int WindowSizeHeightDpiNoScale { get; private set; } = 1080;

    /// <summary>
    /// ウィンドウ初期サイズ ✕ DPIスケール：横幅
    /// （DpiScale の設定を元に計算）
    /// </summary>
    [JsonIgnore]
    public int WindowSizeWidthDpiScale { get; private set; }

    /// <summary>
    /// ウィンドウ初期サイズ ✕ DPIスケール：高さ
    /// （DpiScale の設定を元に計算）
    /// </summary>
    [JsonIgnore]
    public int WindowSizeHeightDpiScale { get; private set; }
}
