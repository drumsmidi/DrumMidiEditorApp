using DrumMidiClassLibrary.pUtil;

namespace DrumMidiClassLibrary.pLog;

/// <summary>
/// ログブロック出力
/// 使い方：using var _ = LogBlock( "ブロック名" );
/// </summary>
public class LogBlock : DisposeBaseClass
{
    /// <summary>
    /// ブロック名
    /// </summary>
    private readonly string _BlockName;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aBlockName">ブロック名</param>
    public LogBlock( string aBlockName )
    {
        _BlockName = aBlockName;

        Log.BeginInfo( _BlockName );
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
                // Dispose managed resources.
                Log.EndInfo( _BlockName );
            }

            // Dispose unmanaged resources.

            _Disposed = true;

			// Note disposing has been done.
			base.Dispose( aDisposing );
		}
	}
    private bool _Disposed = false;
}
