using DrumMidiClassLibrary.pUtil;

namespace DrumMidiClassLibrary.pModel;

/// <summary>
/// グローバル参照用
/// </summary>
public static class DMS
{
	/// <summary>
	/// 編集中のDMSファイルパス
	/// </summary>
	public static GeneralPath OpenFilePath { get; set; } = new();

	/// <summary>
	/// スコア
	/// </summary>
	public static Score SCORE { get; set; } = new();

	/// <summary>
	/// 機械学習用スコア
	/// </summary>
	public static Score SCORE_PREDICT { get; set; } = new();
}
