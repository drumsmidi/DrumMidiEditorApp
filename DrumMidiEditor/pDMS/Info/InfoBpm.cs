namespace DrumMidiEditor.pDMS;

/// <summary>
/// BPM情報
/// </summary>
public class InfoBpm : InfoBase
{
    /// <summary>
    /// BPM
    /// </summary>
    public double Bpm { get; set; } = 0;

    /// <summary>
    /// 検索キー（小節番号✕1,000＋小節内ノート位置）
    /// </summary>
    public override int SearchKey 
        => base.MeasureNo * 1000 + base.NotePos;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private InfoBpm() 
        : base( 0, 0, false ) 
    { 
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    public InfoBpm( int aMeasureNo, int aNotePos )
        : base( aMeasureNo, aNotePos, false )
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aBpm">BPM</param>
    public InfoBpm( int aMeasureNo, int aNotePos, double aBpm )
        : base( aMeasureNo, aNotePos, false )
    {
        Bpm = aBpm;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aBpm">BPM</param>
    /// <param name="aSelected">選択状態</param>
    public InfoBpm( int aMeasureNo, int aNotePos, double aBpm, bool aSelected )
        : base( aMeasureNo, aNotePos, aSelected )
    {
        Bpm = aBpm;
    }

    /// <summary>
    /// BPM情報を設定
    /// </summary>
    /// <param name="aInfo">BPM情報</param>
    public void Set( InfoBpm aInfo )
    {
        base.Set( aInfo );

        Bpm = aInfo.Bpm;
    }

    /// <summary>
    /// BPM情報を複製
    /// </summary>
    /// <returns>BPM情報</returns>
    public InfoBpm Clone()
    {
        return new()
            {
                MeasureNo   = base.MeasureNo,
                NotePos     = base.NotePos,
                Selected    = base.Selected,
                Bpm         = this.Bpm,
            };
    }
}
