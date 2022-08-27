using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pIO.pScore;

/// <summary>
/// Score出力
/// </summary>
internal interface IScoreWriter
{
	/// <summary>
	/// Score＋MidiMapSet保存
	/// </summary>
	/// <param name="aGeneralPath">出力ファイルパス</param>
	/// <param name="aScore">保存スコア</param>
	void Write( GeneralPath aGeneralPath, Score aScore );

	/// <summary>
	/// MidiMapSet保存
	/// </summary>
	/// <param name="aGeneralPath">出力ファイルパス</param>
	/// <param name="aMidiMapSet">保存MidiMapSet</param>
	void Write( GeneralPath aGeneralPath, MidiMapSet aMidiMapSet );
}
