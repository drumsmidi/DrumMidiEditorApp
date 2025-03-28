using System.Collections.Generic;
using Microsoft.UI.Xaml.Input;

namespace DrumMidiLibrary.pInput;

public class InputConverter
{
    InputMap? _InputMap;

    public void SetInputMap( InputMap aInputMap )
    {
        _InputMap = aInputMap;
    }


    #region デバイス入力状態

    /// <summary>
    /// 入力デバイス（入力デバイスID、入力デバイス）
    /// </summary>
    private readonly Dictionary<string, InputDevice> _InputDeviceDic = [];

    /// <summary>
    /// 入力デバイスの入力状態取得
    /// </summary>
    /// <param name="aDeviceId">入力デバイスID</param>
    /// <returns></returns>
    private InputDevice GetInputDevice( string aDeviceId )
    {
        if ( !_InputDeviceDic.TryGetValue( aDeviceId, out var device ) )
        {
            device = new( aDeviceId );
        }
        return device;
    }

    /// <summary>
    /// キーダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public void KeyDown( object aSender, KeyRoutedEventArgs aArgs )
    {
        var device = GetInputDevice( aArgs.DeviceId );


        Print( aArgs );
    }

    /// <summary>
    /// キーダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public void KeyUp( object aSender, KeyRoutedEventArgs aArgs )
    {
        var device = GetInputDevice( aArgs.DeviceId );

        Print( aArgs );
    }

    /// <summary>
    /// キーイベント文字列取得
    /// </summary>
    /// <param name="aArgs"></param>
    /// <returns></returns>
    public string Print( KeyRoutedEventArgs aArgs )
        => $"DeviceId={aArgs.DeviceId} Key={aArgs.Key} OriginalKey={aArgs.OriginalKey} KeyStatus={aArgs.KeyStatus}";

    #endregion
}
