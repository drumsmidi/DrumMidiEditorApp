﻿using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;

namespace DrumMidiPlayerApp.pModel;

/// <summary>
/// グローバル参照用
/// </summary>
public static class DMS
{
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
}
