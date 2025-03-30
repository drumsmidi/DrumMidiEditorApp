using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;

namespace DrumMidiEditorApp.pResume.pItem;

/// <summary>
/// レジューム：ノートボリューム設定
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aInfoBef">変更前 NOTE情報</param>
/// <param name="aInfoAft">変更後 NOTE情報</param>
internal partial class ResumeEditSetNoteVolume( InfoNote aInfoBef, InfoNote aInfoAft ) : DisposeBaseClass, IResume
{
    /// <summary>
    /// 変更前 NOTE情報
    /// </summary>
    private InfoNote? _InfoBef = aInfoBef;

    /// <summary>
    /// 変更後 NOTE情報
    /// </summary>
    private InfoNote? _InfoAft = aInfoAft;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _InfoBef = null;
                _InfoAft = null;
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
        if ( _InfoAft == null )
        {
            return;
        }

        DMS.SCORE.Channels [ _InfoAft.ChannelNo ].AddNote( _InfoAft );

        Update( _InfoAft.MeasureNo );
    }

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    private static void Update( int aMeasureNo ) 
        => EventManage.Event_Score_EditNoteVolume( aMeasureNo );
}
