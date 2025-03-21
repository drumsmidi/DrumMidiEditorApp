using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pUtil;
using System;

namespace DrumMidiLibrary.pModel;

/// <summary>
/// 情報ベース
/// </summary>
public abstract class InfoBase : DisposeBaseClass
{
    #region Member

    /// <summary>
    /// 小節番号
    /// </summary>
    public int MeasureNo { get; protected set; } = 0;

    /// <summary>
    /// 小節内ノート位置
    /// </summary>
    public int NotePos { get; protected set; } = 0;

    /// <summary>
    /// 選択状態
    /// </summary>
    public bool Selected { get; set; } = false;

    /// <summary>
    /// ノート位置（絶対値）
    /// </summary>
    public int AbsoluteNotePos
        => ( MeasureNo * ConfigLib.System.MeasureNoteNumber ) + NotePos;

    /// <summary>
    /// 検索キー
    /// </summary>
    public virtual int SearchKey => 0;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private InfoBase()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aSelected">選択状態</param>
    protected InfoBase( int aMeasureNo, int aNotePos, bool aSelected )
    {
        MeasureNo   = aMeasureNo;
        NotePos     = aNotePos;
        Selected    = aSelected;
    }

    /// <summary>
    /// 情報を設定
    /// </summary>
    /// <param name="aInfo">情報</param>
    public virtual void Set( object aInfo )
    {
        if ( aInfo is not InfoBase info )
        {
            throw new InvalidCastException();
        }

        MeasureNo   = info.MeasureNo;
        NotePos     = info.NotePos;
        Selected    = info.Selected;
    }

    /// <summary>
    /// NOTE情報を複製
    /// </summary>
    /// <returns>情報(InfoBase)</returns>
    public virtual object Clone() => new();
}
