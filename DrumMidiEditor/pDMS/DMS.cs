using DrumMidiEditor.pView.pEditer;
using DrumMidiEditor.pView.pPlayer;

namespace DrumMidiEditor.pDMS;

/// <summary>
/// グローバル参照用
/// </summary>
public static class DMS
{
	/// <summary>
	/// スコア
	/// </summary>
	public static Score SCORE { get; set; } = new();

	/// <summary>
	/// 機械学習用スコア
	/// </summary>
	public static Score SCORE_PREDICT { get; set; } = new();

	/// <summary>
	/// Editerフォーム
	/// </summary>
	public static EditerForm? EditerForm { get; private set; } = null;

	/// <summary>
	/// Playerフォーム
	/// </summary>
	public static PlayerForm? PlayerForm { get; private set; } = null;

	/// <summary>
	/// Form初期化
	/// </summary>
	public static void InitializeForm()
	{
		EditerForm = new();
		PlayerForm = new();
	}
}
