using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pUtil;

namespace DrumMidiEditorApp.pIO.pScore;

/// <summary>
/// Score読取
/// </summary>
internal interface IScoreReader
{
    /// <summary>
    /// XML妥当性チェック
    /// </summary>
    /// <param name="aGeneralPath">チェック対象のXMLファイルパス</param>
    /// <returns>True:OK, False:NG</returns>
    bool Validation( GeneralPath aGeneralPath );

    /// <summary>
    /// スコア読込
    /// </summary>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <param name="aScore">出力先スコア</param>
    void Read( GeneralPath aGeneralPath, out Score aScore );

    /// <summary>
    /// MidiMapSet読込
    /// </summary>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <param name="aMidiMapSet">出力先MidiMapSet</param>
    void Read( GeneralPath aGeneralPath, out MidiMapSet aMidiMapSet );
}
