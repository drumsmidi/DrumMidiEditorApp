using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pUtil;
using DrumMidiEditorApp.pEvent;

namespace DrumMidiEditorApp.pResume;

/// <summary>
/// レジューム：BPM削除
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aInfoBef">変更前 BPM情報</param>
internal partial class ResumeEditRemoveBpm( InfoBpm aInfoBef ) : DisposeBaseClass, IResume
{
    /// <summary>
    /// 変更前 BPM情報
    /// </summary>
    private InfoBpm? _InfoBef = aInfoBef;

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

        DMS.SCORE.SysChannel.AddBpm( _InfoBef );

        Update( _InfoBef.MeasureNo );
    }

    public void Redo()
    {
        if ( _InfoBef == null )
        {
            return;
        }

        _ = DMS.SCORE.SysChannel.RemoveBpm( _InfoBef );

        Update( _InfoBef.MeasureNo );
    }

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    private static void Update( int aMeasureNo ) => EventManage.EventEditBpm( aMeasureNo );
}
