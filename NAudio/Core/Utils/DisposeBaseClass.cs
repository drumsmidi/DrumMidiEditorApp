using System;

namespace NAudio.Core.Utils;

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
    /// <param name="aDisposing"></param>
    protected virtual void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                //component.Dispose();
            }

            // Dispose unmanaged resources.
            //WinApi.CloseHandle( handle );
            //handle = IntPtr.Zero;

            // Note disposing has been done.
            _Disposed = true;
        }
    }
    private bool _Disposed = false;
}
