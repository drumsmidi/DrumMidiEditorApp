using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using DrumMidiLibrary.pLog;
using Microsoft.UI.Xaml.Input;
using Windows.Gaming.Input;

namespace DrumMidiLibrary.pInput;

public static class InputControl
{
    /// <summary>
    /// 排他制御
    /// </summary>
    private static readonly Lock _LockObj = new();

    /// <summary>
    /// 入力変換
    /// </summary>
    private static InputConverter _InputConverter = new();

    public static void ResetInputState()
    {
        try
        {
            lock ( _LockObj )
            {
                foreach ( var device in _InputDeviceDic.Values )
                {
                    foreach ( var stateItem in device.InputStateDic )
                    {
                        stateItem.Value.Clear();
                    }
                }
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    public static List<InputDeviceState> GetInputState( InputMap aInputMap )
    {
        var list = new List<InputDeviceState>();

        try
        {
            lock ( _LockObj )
            {
                foreach ( var device in _InputDeviceDic.Values )
                {
                    foreach ( var stateItem in device.InputStateDic )
                    {
                        if ( aInputMap.KeyMap.TryGetValue( stateItem.Key, out var keymap ) )
                        {
                            while ( stateItem.Value.TryDequeue( out var state ) )
                            {
                                state.MapKey = keymap;
                                list.Add( state );
                            }
                        }
                    }
                }
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }

        return list;
    }

    #region 時間計測

    /// <summary>
    /// 時間計測
    /// </summary>
    private static readonly Stopwatch _StopWatch = new();

    public static void StartTime() => _StopWatch.Start();

    public static void StopTime() => _StopWatch.Stop();

    public static void RestartTime() => _StopWatch.Restart();

    /// <summary>
    /// 再生開始処理後の現在の再生時間（秒）
    /// </summary>
    private static double CurrentTime
        => _StopWatch.ElapsedTicks / (double)Stopwatch.Frequency;

    #endregion

    #region 入力デイバイス

    /// <summary>
    /// 入力デバイス（入力デバイスID、入力デバイス）
    /// </summary>
    private static readonly Dictionary<string, InputDevice> _InputDeviceDic = [];

    /// <summary>
    /// 入力デバイスの入力状態取得
    /// </summary>
    /// <param name="aDeviceId">入力デバイスID</param>
    /// <returns></returns>
    private static InputDevice GetInputDevice( string aDeviceId )
    {
        if ( !_InputDeviceDic.TryGetValue( aDeviceId, out var device ) )
        {
            device = new( aDeviceId );

            _InputDeviceDic.Add( aDeviceId, device );
        }
        return device;
    }

    #endregion

    #region イベントキャプチャ：ゲームパッド

    /// <summary>
    /// ゲームパッド監視
    /// 
    /// NOTE:ゲームパッドはWinUI3 未サポート？
    /// </summary>
    public static void SetGamePadWatcher()
    {
        try
        {
            lock ( _LockObj )
            {
                foreach ( var gamepad in Gamepad.Gamepads )
                {
                    PrintGamePadLog( gamepad );
                }

                Gamepad.GamepadAdded += GamePadAdded;
                Gamepad.GamepadRemoved += GamepadRemoved;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ゲームパッド監視解除
    /// </summary>
    public static void ReleaseGamePadWatcher()
    {
        try
        {
            lock ( _LockObj )
            {
                Gamepad.GamepadAdded   -= GamePadAdded;
                Gamepad.GamepadRemoved -= GamepadRemoved;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ゲームパッド追加イベント
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aGamepad"></param>
    private static void GamePadAdded( object? aSender, Gamepad aGamepad )
    {
        try
        {
            lock ( _LockObj )
            {
                PrintGamePadLog( aGamepad );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ゲームパッド削除イベント
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aGamepad"></param>
    private static void GamepadRemoved( object? aSender, Gamepad aGamepad )
    {
        try
        {
            lock ( _LockObj )
            {
                PrintGamePadLog( aGamepad );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// キーイベントログ出力
    /// </summary>
    /// <param name="aArgs"></param>
    /// <returns></returns>
    [Conditional( "DEBUG" )]
    public static void PrintGamePadLog( Gamepad aGamepad )
        => Log.Info( $"AAAAAA={aGamepad.IsWireless}"
            );

    #endregion

    #region イベントキャプチャ：キーボード

    /// <summary>
    /// キーダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public static void KeyDown( object aSender, KeyRoutedEventArgs aArgs )
    {
        try
        {
            lock ( _LockObj )
            {
                var device = GetInputDevice( aArgs.DeviceId );

                device.Key( CurrentTime, aArgs );
            }

            PrintKeyLog( aArgs );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// キーダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public static void KeyUp( object aSender, KeyRoutedEventArgs aArgs )
    {
        try
        {
            lock ( _LockObj )
            {
                var device = GetInputDevice( aArgs.DeviceId );

                device.Key( CurrentTime, aArgs );
            }

            PrintKeyLog( aArgs );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// キーイベントログ出力
    /// </summary>
    /// <param name="aArgs"></param>
    /// <returns></returns>
    [Conditional( "DEBUG" )]
    public static void PrintKeyLog( KeyRoutedEventArgs aArgs )
        => Log.Info( $"DeviceId={aArgs.DeviceId} HandleId={aArgs.Handled}" 
            + $" Key={aArgs.Key} OriginalKey={aArgs.OriginalKey}"
            + $" RepeatCount={aArgs.KeyStatus.RepeatCount} IsKeyReleased={aArgs.KeyStatus.IsKeyReleased}"
            + $" WasKeyDown={aArgs.KeyStatus.WasKeyDown} IsMenuKeyDown={aArgs.KeyStatus.IsMenuKeyDown}"
            + $" IsExtendedKey={aArgs.KeyStatus.IsExtendedKey} ScanCode={aArgs.KeyStatus.ScanCode}"
            );

    #endregion

    #region イベントキャプチャ：マウス

    /// <summary>
    /// マウスダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public static void PointerPressed( object aSender, PointerRoutedEventArgs aArgs )
    {
        try
        {
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// マウス移動処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public static void PointerMoved( object aSender, PointerRoutedEventArgs aArgs )
    {
        try
        {
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// マウスアップ処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public static void PointerReleased( object aSender, PointerRoutedEventArgs aArgs )
    {
        try
        {
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
