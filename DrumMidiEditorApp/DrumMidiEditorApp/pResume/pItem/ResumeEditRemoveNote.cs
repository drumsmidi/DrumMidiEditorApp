using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;

namespace DrumMidiEditorApp.pResume.pItem;

/// <summary>
/// レジューム：ノート削除
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aInfoBef">変更前 NOTE情報</param>
internal partial class ResumeEditRemoveNote( InfoNote aInfoBef ) : DisposeBaseClass, IResume
{
    /// <summary>
    /// 変更前 NOTE情報
    /// </summary>
    private InfoNote? _InfoBef = aInfoBef;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _InfoBef = null;
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
        if ( _InfoBef == null )
        {
            return;
        }

        DMS.SCORE.Channels [ _InfoBef.ChannelNo ].AddNote( _InfoBef );

        Update( _InfoBef.MeasureNo );
    }

    public void Redo()
    {
        if ( _InfoBef == null )
        {
            return;
        }

        _ = DMS.SCORE.Channels [ _InfoBef.ChannelNo ].RemoveNote( _InfoBef );

        Update( _InfoBef.MeasureNo );
    }

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    private static void Update( int aMeasureNo ) => EventManage.Event_Score_EditNote( aMeasureNo );
}
