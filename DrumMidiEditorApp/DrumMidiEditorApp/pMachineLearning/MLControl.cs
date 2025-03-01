using System;
using System.Threading.Tasks;
using DrumMidiEditorApp.pLog;

namespace DrumMidiEditorApp.pMachineLearning;

/// <summary>
/// ML.NET 機械学習
/// </summary>
public static class MLControl
{
    /// <summary>
    /// 処理中フラグ
    /// </summary>
    private static bool _Processing = false;

    /// <summary>
    /// 機械学習＆予測処理（試作）
    /// TODO: お試しで作成したもの、今のところ実装予定なし
    /// </summary>
    public static async void Start()
    {
        if ( _Processing )
        {
            return;
        }

        _Processing = true;

        try
        {
            await Task.Run( () =>
                {
                    var proc = new MLProcFFT();
                    proc.Start();
                } );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
        finally
        {
            _Processing = false;
        }
    }
}
