using System.Collections.Generic;
using DrumMidiLibrary.pLog;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace DrumMidiLibrary.pInput;

internal class InputDevice( string aDeviceId )
{
    public string DeviceId { get; private set; } = aDeviceId;

    /// <summary>
    /// 入力状態（仮想キー、入力状態）
    /// </summary>
    public readonly Dictionary<VirtualKey, Queue<InputDeviceState>> InputStateDic = [];

    /// <summary>
    /// 入力状態取得
    /// </summary>
    /// <param name="aKey">仮想キー</param>
    /// <returns>入力状態</returns>
    private Queue<InputDeviceState> GetInputState( VirtualKey aKey )
    {
        if ( !InputStateDic.TryGetValue( aKey, out var stateQue ) )
        {
            stateQue = new();
            InputStateDic.Add( aKey, stateQue );
        }
        return stateQue;
    }

    /// <summary>
    /// キー処理
    /// </summary>
    /// <param name="aCurrentTime"></param>
    /// <param name="aArgs"></param>
    public void Key( double aCurrentTime, KeyRoutedEventArgs aArgs )
    {
        var stateQue = GetInputState( aArgs.Key );

        // キューがない または 前回キーリリースされた場合
        if ( !stateQue.TryPeek( out var state ) || state.IsKeyReleased )
        {
            // キー押下前に、キーリリースされた場合、無視する
            if ( aArgs.KeyStatus.IsKeyReleased )
            {
                return;
            }

            state = new()
            {
                Key             = aArgs.Key,
                IsKeyPushed     = true,
                IsKeyReleased   = false,
                RepeatCount     = (int)aArgs.KeyStatus.RepeatCount ,
                StartTime       = aCurrentTime,
                EndTime         = aCurrentTime,
            };

            stateQue.Enqueue( state );
        }
        // 前回キーが押されていた
        else if ( state.IsKeyPushed )
        {
            if ( aArgs.KeyStatus.IsKeyReleased )
            {
                state.IsKeyPushed   = false;
                state.IsKeyReleased = true;
                state.EndTime       = aCurrentTime;
            }
            else
            {
                state.RepeatCount   += (int)aArgs.KeyStatus.RepeatCount ;
                state.EndTime        = aCurrentTime;
            }
        }
    }
}
