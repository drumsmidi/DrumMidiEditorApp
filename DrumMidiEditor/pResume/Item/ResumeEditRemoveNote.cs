using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pResume;

/// <summary>
/// レジューム：ノート削除
/// </summary>
internal class ResumeEditRemoveNote : DisposeBaseClass, IResume
{
    /// <summary>
    /// 変更前 NOTE情報
    /// </summary>
    private InfoNote? _InfoBef;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aInfoBef">変更前 NOTE情報</param>
    public ResumeEditRemoveNote( InfoNote aInfoBef )
    {
        _InfoBef = aInfoBef;
    }

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

		DMS.SCORE.Channels[ _InfoBef.ChannelNo ].AddNote( _InfoBef );

        Update( _InfoBef.MeasureNo );
    }

    public void Redo()
    {
        if ( _InfoBef == null )
        {
            return;
        }

		DMS.SCORE.Channels[ _InfoBef.ChannelNo ].RemoveNote( _InfoBef );

        Update( _InfoBef.MeasureNo );
    }

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    private static void Update( int aMeasureNo )
    {
        Config.EventEditNote( aMeasureNo );
    }
}
