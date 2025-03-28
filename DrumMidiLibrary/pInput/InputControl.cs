using System;
using System.Diagnostics;
using System.Threading;
using DrumMidiLibrary.pLog;
using Microsoft.UI.Xaml.Input;

namespace DrumMidiLibrary.pInput;

public static class InputControl
{
    /// <summary>
    /// 排他制御
    /// </summary>
    private static Lock _LockObj = new();

    /// <summary>
    /// 入力変換
    /// </summary>
    private static InputConverter _InputConverter = new();

    #region イベントキャプチャ

    /// <summary>
    /// キーダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    public static void KeyDown( object aSender, KeyRoutedEventArgs aArgs )
    {
        try
        {
            //_InputConverter.KeyDown( aSender, aArgs );

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
            //_InputConverter.KeyDown( aSender, aArgs );

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
        => Log.Info( $"DeviceId={aArgs.DeviceId} Key={aArgs.Key} OriginalKey={aArgs.OriginalKey} KeyStatus={aArgs.KeyStatus}" );

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
