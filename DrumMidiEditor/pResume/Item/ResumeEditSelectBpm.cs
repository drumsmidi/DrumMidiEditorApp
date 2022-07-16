using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pResume;

/// <summary>
/// レジューム：BPM選択
/// </summary>
internal class ResumeEditSelectBpm : DisposeBaseClass, IResume
{
    /// <summary>
    /// BPM情報
    /// </summary>
    private InfoBpm? _Info;

    /// <summary>
    /// 選択状態
    /// </summary>
    private readonly bool _Select;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aInfo">BPM情報</param>
    /// <param name="aSelectAft">選択状態</param>
    public ResumeEditSelectBpm( InfoBpm aInfo, bool aSelectAft )
    {
        _Info      = aInfo;
        _Select    = aSelectAft;
    }

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

    public void Undo()
    {
        Update( !_Select );
    }

    public void Redo()
    {
        Update( _Select );
    }

    /// <summary>
    /// Undo/Redo共通処理
    /// </summary>
    private void Update( bool aSelect )
    {
        if ( _Info == null )
        {
            return;
        }

        var info = DMS.SCORE.SysChannel.BpmInfoList[ _Info.SearchKey ];

        info.Selected = aSelect;
    }
}
