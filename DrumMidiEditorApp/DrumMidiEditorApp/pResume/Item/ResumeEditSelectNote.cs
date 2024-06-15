using DrumMidiClassLibrary.pModel;
using DrumMidiClassLibrary.pUtil;

namespace DrumMidiEditorApp.pResume;

/// <summary>
/// レジューム：ノート選択
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aInfo">NOTE情報</param>
/// <param name="aSelectAft">選択状態</param>
internal class ResumeEditSelectNote( InfoNote aInfo, bool aSelectAft ) : DisposeBaseClass, IResume
{
    /// <summary>
    /// NOTE情報
    /// </summary>
    private InfoNote? _Info = aInfo;

    /// <summary>
    /// 選択状態
    /// </summary>
    private readonly bool _Select = aSelectAft;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _Info = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    public void Undo() => Update( !_Select );

    public void Redo() => Update( _Select );

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    private void Update( bool aSelect )
    {
        if ( _Info == null )
        {
            return;
        }

        var info = DMS.SCORE.Channels[ _Info.ChannelNo ].NoteInfoList[ _Info.SearchKey ];

        info.Selected = aSelect;
    }
}
