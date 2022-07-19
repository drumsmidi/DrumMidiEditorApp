using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pResume;

/// <summary>
/// レジューム：ノート設定
/// </summary>
internal class ResumeEditAddNote : DisposeBaseClass, IResume
{
    /// <summary>
    /// 変更前 NOTE情報
    /// </summary>
    private InfoNote? _InfoBef;

    /// <summary>
    /// 変更後 NOTE情報
    /// </summary>
    private InfoNote? _InfoAft;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aInfoBef">変更前 NOTE情報</param>
    /// <param name="aInfoAft">変更後 NOTE情報</param>
    public ResumeEditAddNote( InfoNote? aInfoBef, InfoNote aInfoAft )
    {
        _InfoBef = aInfoBef;
        _InfoAft = aInfoAft;
    }

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
        if ( _InfoBef != null )
        {
			DMS.SCORE.Channels[ _InfoBef.ChannelNo ].AddNote( _InfoBef );

            Update( _InfoBef.MeasureNo );
        }
        else if ( _InfoAft != null )
        {
			DMS.SCORE.Channels[ _InfoAft.ChannelNo ].RemoveNote( _InfoAft );

            Update( _InfoAft.MeasureNo );
        }
    }

    public void Redo()
    {
        if ( _InfoAft == null )
        {
            return;
        }

		DMS.SCORE.Channels[ _InfoAft.ChannelNo ].AddNote( _InfoAft );

        Update( _InfoAft.MeasureNo );
    }

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    private static void Update( int aMeasureNo )
    {
        Config.EventEditNote( aMeasureNo );
    }
}
