using System;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// IDisposableベースクラス
/// </summary>
public abstract class DisposeBaseClass : IDisposable
{
    /// <summary>
    /// デストラクタ
    /// </summary>
    ~DisposeBaseClass()
    {
        Dispose( false );
    }

    /// <summary>
    /// 破棄（共通）
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    /// 破棄（継承先で必要に応じて定義）
    /// </summary>
    /// <param name="aDisposing">true:マネージドリソースの解放, false:アンマネージドリソースの解放</param>
    protected virtual void Dispose( bool aDisposing )
    {
        if ( _Disposed )
        {
            return;
        }

        // マネージドリソースの解放
        if ( aDisposing )
        {
            //component.Dispose();
        }

        // アンマネージドリソースの解放
        {
            //WinApi.CloseHandle( handle );
            //handle = IntPtr.Zero;
        }

        _Disposed = true;
    }
    private bool _Disposed = false;
}
