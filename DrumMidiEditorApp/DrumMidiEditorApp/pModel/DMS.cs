﻿using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;

namespace DrumMidiEditorApp.pModel;

/// <summary>
/// グローバル参照用
/// </summary>
public static class DMS
{
    /// <summary>
    /// アプリケーション起動元のDMSファイルパス
    /// </summary>
    public static GeneralPath AppStartDmsPath { get; set; } = new();

    /// <summary>
    /// 編集中のDMSファイルパス
    /// </summary>
    public static GeneralPath OpenFilePath { get; set; } = new();

    /// <summary>
    /// スコア
    /// </summary>
    public static Score SCORE
    { 
        get => _Score;
        set
        {
            _Score = value;

            // 再生用に譜面情報の参照情報を設定しておく
            Config.Media.SCORE = value;
        }
    }
    private static Score _Score = new();

    /// <summary>
    /// 機械学習用スコア
    /// </summary>
    public static Score SCORE_PREDICT { get; set; } = new();
}
