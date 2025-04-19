using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pLog;

/// <summary>
/// ログブロック出力
/// 使い方：using var _ = LogBlock( "ブロック名" );
/// </summary>
public partial class LogBlock : DisposeBaseClass
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
        if ( _Disposed )
        {
            return;
        }
        // マネージドリソースの解放
        if ( aDisposing )
        {
            Log.EndInfo( _BlockName );
        }

        // アンマネージドリソースの解放
        {
        }

        _Disposed = true;

        base.Dispose( aDisposing );
    }
    private bool _Disposed = false;
}
