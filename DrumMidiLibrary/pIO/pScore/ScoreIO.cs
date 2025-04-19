using System;
using System.Collections.Generic;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pIO.pScore;

/// <summary>
/// Score入出力
/// </summary>
public static class ScoreIO
{
    /// <summary>
    /// 書込
    /// </summary>
#pragma warning disable CA1859 // 可能な場合は具象型を使用してパフォーマンスを向上させる
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:不要な抑制を削除します", Justification = "<保留中>" )]
    private static readonly IScoreWriter _Writer = new pDms.ScoreStream1000();
#pragma warning restore CA1859 // 可能な場合は具象型を使用してパフォーマンスを向上させる

    /// <summary>
    /// 読込（上から順番に読込できるバージョンで処理）
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:不要な抑制を削除します", Justification = "<保留中>" )]
    private static readonly List<IScoreReader> _Readers =
    [
        new pDms.ScoreStream1000(),
        new pDms.ScoreStream0950(),
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

                aScore.FilePath = aGeneralPath;
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
        => _Writer?.Write( aGeneralPath, aScore );

    /// <summary>
    /// MidiMapSetファイル保存
    /// </summary>
    /// <param name="aGeneralPath">出力ファイルパス</param>
    /// <param name="aMidiMapSet">保存MidiMapSet</param>
    public static void SaveFile( GeneralPath aGeneralPath, MidiMapSet aMidiMapSet )
        => _Writer?.Write( aGeneralPath, aMidiMapSet );

    /// <summary>
    /// Midiファイル保存
    /// </summary>
    /// <param name="aGeneralPath">出力ファイルパス</param>
    /// <param name="aScore">保存スコア</param>
    public static void SaveMidiFile( GeneralPath aGeneralPath, Score aScore )
        => new pMidi.ScoreStream().Write( aGeneralPath, aScore );

    #endregion
}
