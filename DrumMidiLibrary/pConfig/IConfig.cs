namespace DrumMidiLibrary.pConfig;

public interface IConfig
{
    /// <summary>
    /// コンフィグファイル読込後の妥当性チェックに使用
    /// JsonInclude対象の項目に対して、値を補正する
    /// </summary>
    void CheckValidation();
}
