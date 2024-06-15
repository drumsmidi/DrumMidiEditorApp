using DrumMidiClassLibrary.pModel;
using DrumMidiClassLibrary.pUtil;
using DrumMidiEditorApp.pEvent;

namespace DrumMidiEditorApp.pResume;

/// <summary>
/// レジューム：BPM設定
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aInfoBef">変更前 BPM情報</param>
/// <param name="aInfoAft">変更後 BPM情報</param>
internal class ResumeEditAddBpm( InfoBpm? aInfoBef, InfoBpm aInfoAft ) : DisposeBaseClass, IResume
{
    /// <summary>
    /// 変更前 BPM情報
    /// </summary>
    private InfoBpm? _InfoAft = aInfoAft;

    /// <summary>
    /// 変更後 BPM情報
    /// </summary>
    private InfoBpm? _InfoBef = aInfoBef;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _InfoAft = null;
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
        if ( _InfoBef != null )
        {
            DMS.SCORE.SysChannel.AddBpm( _InfoBef );

            Update( _InfoBef.MeasureNo );
        }
        else if ( _InfoAft != null )
        {
            _ = DMS.SCORE.SysChannel.RemoveBpm( _InfoAft );

            Update( _InfoAft.MeasureNo );
        }
    }

    public void Redo()
    {
        if ( _InfoAft == null )
        {
            return;
        }

        DMS.SCORE.SysChannel.AddBpm( _InfoAft );

        Update( _InfoAft.MeasureNo );
    }

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    private static void Update( int aMeasureNo ) => EventManage.EventEditBpm( aMeasureNo );
}
