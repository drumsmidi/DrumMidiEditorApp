namespace DrumMidiPlayerApp.pConfig;

/// <summary>
/// 音階アイテム
/// </summary>
public class ConfigPlayerScoreType2ScaleItem
{
    /// <summary>
    /// 音階キー
    /// </summary>
    public string ScaleKey { get; set; } = string.Empty;

    /// <summary>
    /// ラベル
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// ライン描画フラグ
    /// </summary>
    public bool LineDrawFlag { get; set; } = false;

    /// <summary>
    /// 音階キー＋ラベル
    /// </summary>
    public string ScaleKeyLabel => $"{ScaleKey}-{Label}";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ConfigPlayerScoreType2ScaleItem()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aScaleKey">音階キー</param>
    /// <param name="aLabel">ラベル</param>
    /// <param name="aLineDrawFlag">ライン描画フラグ</param>
    public ConfigPlayerScoreType2ScaleItem( string aScaleKey, string aLabel, bool aLineDrawFlag )
    {
        ScaleKey        = aScaleKey;
        Label           = aLabel;
        LineDrawFlag    = aLineDrawFlag;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aScaleItem">コピー元</param>
    public ConfigPlayerScoreType2ScaleItem( ConfigPlayerScoreType2ScaleItem aScaleItem )
    {
        ScaleKey        = aScaleItem.ScaleKey;
        Label           = aScaleItem.Label;
        LineDrawFlag    = aScaleItem.LineDrawFlag;
    }
}
