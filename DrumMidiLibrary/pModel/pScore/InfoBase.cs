using System;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pModel.pScore;

/// <summary>
/// 情報ベース
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aMeasureNo">小節番号</param>
/// <param name="aNotePos">小節内ノート位置</param>
/// <param name="aSelected">選択状態</param>
public abstract class InfoBase( int aMeasureNo, int aNotePos, bool aSelected ) : DisposeBaseClass
{
    #region Member

    /// <summary>
    /// 小節番号
    /// </summary>
    public int MeasureNo { get; protected set; } = aMeasureNo;

    /// <summary>
    /// 小節内ノート位置
    /// </summary>
    public int NotePos { get; protected set; } = aNotePos;

    /// <summary>
    /// 選択状態
    /// </summary>
    public bool Selected { get; set; } = aSelected;

    /// <summary>
    /// ノート位置（絶対値）
    /// </summary>
    public int AbsoluteNotePos
        => ( MeasureNo * ConfigLib.System.MeasureNoteNumber ) + NotePos;

    /// <summary>
    /// 検索キー
    /// </summary>
    public abstract int SearchKey { get; }

    #endregion

    /// <summary>
    /// 情報を設定
    /// </summary>
    /// <param name="aInfo">情報</param>
    public virtual void Set( object aInfo )
    {
        if ( aInfo is not InfoBase info )
        {
            throw new ArgumentException( $"Invalid type. Expected {nameof( InfoBase )}, but received {aInfo.GetType().Name}.", nameof( aInfo ) );
        }

        MeasureNo   = info.MeasureNo;
        NotePos     = info.NotePos;
        Selected    = info.Selected;
    }

    /// <summary>
    /// NOTE情報を複製
    /// </summary>
    /// <returns>情報(InfoBase)</returns>
    public abstract object Clone();
}
