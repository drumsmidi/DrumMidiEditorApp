using System;
using System.Collections.Generic;

using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pModel;
using DrumMidiClassLibrary.pUtil;

namespace DrumMidiClassLibrary.pIO.pScore;

/// <summary>
/// Score入出力
/// </summary>
internal static class ScoreIO
{
    /// <summary>
    /// 書込
    /// </summary>
    private static readonly IScoreWriter _Writer = new p1000.ScoreStream();

    /// <summary>
    /// 読込（上から順番に読込できるバージョンで処理）
    /// </summary>
    private static readonly List<IScoreReader> _Readers =
    [
        new p1000.ScoreStream(),
        new p0950.ScoreStream(),
    ];

    #region Load

    /// <summary>
    /// スコア読込
    /// </summary>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <param name="aScore">出力先スコア</param>
    public static void LoadFile( GeneralPath aGeneralPath, out Score aScore )
    {
        foreach ( var reader in _Readers )
        {
            Log.Info( $"{reader}" );

            if ( reader.Validation( aGeneralPath ) )
            {
                reader.Read( aGeneralPath, out aScore );
                return;
            }
        }

        throw new Exception( $"xml format error..." );
    }

    /// <summary>
    /// MidiMapSet読込
    /// </summary>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <param name="aMidiMapSet">出力先MidiMapSet</param>
    public static void LoadFile( GeneralPath aGeneralPath, out MidiMapSet aMidiMapSet )
    {
        foreach ( var reader in _Readers )
        {
            Log.Info( $"{reader}" );

            if ( reader.Validation( aGeneralPath ) )
            {
                reader.Read( aGeneralPath, out aMidiMapSet );
                return;
            }
        }

        throw new Exception( $"xml format error..." );
    }

    /// <summary>
    /// MIDIファイル読込
    /// </summary>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <param name="aScore">出力先スコア</param>
    public static void LoadMidiFile( GeneralPath aGeneralPath, out Score aScore )
        => new pMidi.ScoreStream().Read( aGeneralPath, out aScore );

    /// <summary>
    /// DTXファイル読込（非推奨）
    /// </summary>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <param name="aScore">出力先スコア</param>
    public static void LoadDtxManiaFile( GeneralPath aGeneralPath, out Score aScore )
        => new pDtxMania.ScoreStream().Read( aGeneralPath, out aScore );

    #endregion

    #region Save

    /// <summary>
    /// Score＋MidiMapSetファイル保存
    /// </summary>
    /// <param name="aGeneralPath">出力ファイルパス</param>
    /// <param name="aScore">保存スコア</param>
    public static void SaveFile( GeneralPath aGeneralPath, Score aScore )
        => _Writer.Write( aGeneralPath, aScore );

    /// <summary>
    /// MidiMapSetファイル保存
    /// </summary>
    /// <param name="aGeneralPath">出力ファイルパス</param>
    /// <param name="aMidiMapSet">保存MidiMapSet</param>
    public static void SaveFile( GeneralPath aGeneralPath, MidiMapSet aMidiMapSet )
        => _Writer.Write( aGeneralPath, aMidiMapSet );

    /// <summary>
    /// Midiファイル保存
    /// </summary>
    /// <param name="aGeneralPath">出力ファイルパス</param>
    /// <param name="aScore">保存スコア</param>
    public static void SaveMidiFile( GeneralPath aGeneralPath, Score aScore )
        => new pMidi.ScoreStream().Write( aGeneralPath, aScore );

    #endregion
}
