using System;
using DrumMidiLibrary.pConfig;

namespace DrumMidiLibrary.pModel.pScore;

/// <summary>
/// BPM情報
/// </summary>
public partial class InfoBpm : InfoBase
{
    #region コンストラクタ

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

    #endregion

    #region Member

    /// <summary>
    /// BPM
    /// </summary>
    public double Bpm { get; set; } = ConfigLib.System.DefaultBpm;

    /// <summary>
    /// 検索キー（小節番号✕1,000＋小節内ノート位置）
    /// </summary>
    public override int SearchKey
        => ( MeasureNo * 1000 ) + NotePos;

    #endregion

    /// <summary>
    /// BPM情報を設定
    /// </summary>
    /// <param name="aInfo">BPM情報</param>
    public override void Set( object aInfo )
    {
        base.Set( aInfo );

        if ( aInfo is not InfoBpm info )
        {
            throw new ArgumentException( $"Invalid type. Expected {nameof( InfoBpm )}, but received {aInfo.GetType().Name}.", nameof( aInfo ) );
        }

        Bpm = info.Bpm;
    }

    /// <summary>
    /// BPM情報を複製
    /// </summary>
    /// <returns>BPM情報(InfoBpm)</returns>
    public override object Clone()
    {
        return new InfoBpm()
        {
            MeasureNo   = MeasureNo,
            NotePos     = NotePos,
            Selected    = Selected,
            Bpm         = Bpm,
        };
    }
}
