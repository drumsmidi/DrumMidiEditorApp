using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pUtil;
using DrumMidiEditorApp.pEvent;

namespace DrumMidiEditorApp.pResume;

/// <summary>
/// レジューム：MidiMapGroup選択
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aGroup">編集対象のMidiMapGroup</param>
internal partial class ResumeEditSelectMidiMapGroup( MidiMapGroup aGroup ) : DisposeBaseClass, IResume
{
    /// <summary>
    /// 編集対象のMidiMapGroup
    /// </summary>
    private MidiMapGroup? _MidiMapGroup = aGroup;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _MidiMapGroup = null;
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
        if ( _MidiMapGroup == null )
        {
            return;
        }
        _MidiMapGroup.Selected = !_MidiMapGroup.Selected;

        EventManage.EventEditUpdateDisplayNoteVolume();
    }
}
