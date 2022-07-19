using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pDMS;

/// <summary>
/// 情報ベース
/// </summary>
public class InfoBase : DisposeBaseClass
{ 
    /// <summary>
    /// 小節番号
    /// </summary>
    public int MeasureNo { get; protected set; }

    /// <summary>
    /// 小節内ノート位置
    /// </summary>
    public int NotePos { get; protected set; }

    /// <summary>
    /// 選択状態
    /// </summary>
    public bool Selected { get; set; } = false;

    /// <summary>
    /// ノート位置（絶対値）
    /// </summary>
    public int AbsoluteNotePos
        => MeasureNo * Config.System.MeasureNoteNumber + NotePos;

    /// <summary>
    /// 検索キー
    /// </summary>
    public virtual int SearchKey => 0;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private InfoBase() {}

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aSelected">選択状態</param>
    protected InfoBase( int aMeasureNo, int aNotePos, bool aSelected )
    {
        MeasureNo  = aMeasureNo;
        NotePos    = aNotePos;
        Selected   = aSelected;
    }

    /// <summary>
    /// 情報を設定
    /// </summary>
    /// <param name="aInfo">情報</param>
    public virtual void Set( InfoBase aInfo )
    {
        MeasureNo  = aInfo.MeasureNo;
        NotePos    = aInfo.NotePos;
        Selected   = aInfo.Selected;
    }
}
