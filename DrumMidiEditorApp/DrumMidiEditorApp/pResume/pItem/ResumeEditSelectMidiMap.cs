using DrumMidiEditorApp.pEvent;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;

namespace DrumMidiEditorApp.pResume.pItem;

/// <summary>
/// レジューム：MidiMap選択
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aMidiMap">編集対象のMidiMap</param>
internal partial class ResumeEditSelectMidiMap( MidiMap aMidiMap ) : DisposeBaseClass, IResume
{
    /// <summary>
    /// 編集対象のMidiMap
    /// </summary>
    private MidiMap? _MidiMap = aMidiMap;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _MidiMap = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    public void Undo() => Update();

    public void Redo() => Update();

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    private void Update()
    {
        if ( _MidiMap == null )
        {
            return;
        }
        _MidiMap.Selected = !_MidiMap.Selected;

        EventManage.Event_Editer_UpdateDisplayNoteVolume();
    }
}
