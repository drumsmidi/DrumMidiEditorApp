using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pUtil;
using DrumMidiEditorApp.pEvent;

namespace DrumMidiEditorApp.pResume;

/// <summary>
/// レジューム：MidiMapGroup ScaleKey選択
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aChannelNo">編集対象のチェンネル番号</param>
/// <param name="aScaleKey">編集対象のMidiMapGroupの音階キー</param>
internal partial class ResumeEditSelectMidiMapGroupScale( byte aChannelNo, string aScaleKey ) : DisposeBaseClass, IResume
{
    /// <summary>
    /// 編集対象のチャンネル番号
    /// </summary>
    private readonly byte _ChannelNo = aChannelNo;

    /// <summary>
    /// 編集対象の音階キー
    /// </summary>
    private readonly string _ScaleKey = aScaleKey;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
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
        foreach ( var group in DMS.SCORE.Channels[ _ChannelNo ].MidiMapSet.MidiMapGroups )
        {
            if ( group.ScaleKey.Equals( _ScaleKey ) )
            {
                group.ScaleKeySelected = !group.ScaleKeySelected;
            }
        }

        EventManage.Event_Editer_UpdateDisplayNoteVolume();
    }
}
