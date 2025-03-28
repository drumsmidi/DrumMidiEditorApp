using System.Collections.Generic;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace DrumMidiLibrary.pInput;

internal class InputDevice( string aDeviceId )
{
    public string DeviceId { get; private set; } = aDeviceId;

    /// <summary>
    /// 入力状態（仮想キー、入力状態）
    /// </summary>
    private readonly Dictionary<VirtualKey, InputDeviceState> _InputStateDic = [];

    /// <summary>
    /// 入力状態取得
    /// </summary>
    /// <param name="aKey">仮想キー</param>
    /// <returns>入力状態</returns>
    private InputDeviceState GetInputState( VirtualKey aKey )
    {
        if ( !_InputStateDic.TryGetValue( aKey, out var state ) )
        {
            state = new()
            {
                Key = aKey,
            };
        }
        return state;
    }

    /// <summary>
    /// キーダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public void KeyDown( object aSender, KeyRoutedEventArgs aArgs )
    {
        var state = GetInputState( aArgs.Key );

        state.KeyStatus = aArgs.KeyStatus;
    }

    /// <summary>
    /// キーダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public void KeyUp( object aSender, KeyRoutedEventArgs aArgs )
    {
        var state = GetInputState( aArgs.Key );

        state.KeyStatus = aArgs.KeyStatus;
    }
}
