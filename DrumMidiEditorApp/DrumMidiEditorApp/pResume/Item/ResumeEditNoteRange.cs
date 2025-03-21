using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pView.pEditer;
using DrumMidiLibrary.pUtil;

namespace DrumMidiEditorApp.pResume;

/// <summary>
/// レジューム：ノート範囲設定
/// </summary>
internal partial class ResumeEditNoteRange : DisposeBaseClass, IResume
{
    /// <summary>
    /// Editタブのノート範囲オブジェクトへの参照
    /// </summary>
    private DmsItemNoteRange? _RangeRef;

    /// <summary>
    /// ノート範囲変更後
    /// </summary>
    private DmsItemNoteRange? _RangeAft;

    /// <summary>
    /// ノート範囲変更前
    /// </summary>
    private DmsItemNoteRange? _RangeBef;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aRangeRef">ノート範囲変更後</param>
    /// <param name="aRangeBef">ノート範囲変更前</param>
    public ResumeEditNoteRange( ref DmsItemNoteRange aRangeRef, DmsItemNoteRange aRangeBef )
    {
        _RangeRef = aRangeRef;
        _RangeAft = aRangeRef.Clone();
        _RangeBef = aRangeBef.Clone();
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _RangeRef = null;
                _RangeAft = null;
                _RangeBef = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    public void Undo()
    {
        if ( _RangeBef == null )
        {
            return;
        }

        _RangeRef?.Set( _RangeBef );

        Update();
    }

    public void Redo()
    {
        if ( _RangeAft == null )
        {
            return;
        }

        _RangeRef?.Set( _RangeAft );

        Update();
    }

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    private void Update() 
        => EventManage.Event_Editer_UpdateRangeSelect( _RangeRef?.Selected ?? false );
}
