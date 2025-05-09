﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pLog;

/// <summary>
/// ログ出力
/// </summary>
public static class Log
{
    #region ログ出力

    /// <summary>
    /// ブロック開始／終了間の時間計測用
    /// </summary>
    private static readonly ConcurrentStack<DateTime> _BlockTime = new();

    /// <summary>
    /// 情報ログラベル
    /// </summary>
    private static readonly string _LABEL_INFO = "[INFO]";

    /// <summary>
    /// 警告ログラベル
    /// </summary>
    private static readonly string _LABEL_WARNING = "[WARN]";

    /// <summary>
    /// エラーログラベル
    /// </summary>
    private static readonly string _LABEL_ERROR = "[ERR] ";

    /// <summary>
    /// ブロックラベル開始
    /// </summary>
    private static readonly string _LABEL_BLOCK_START = "[INFO]";

    /// <summary>
    /// ブロックラベル終了
    /// </summary>
    private static readonly string _LABEL_BLOCK_END = "[INFO]";

    /// <summary>
    /// 情報ログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    /// <param name="aCallback">True:通知、False:非通知</param>
    public static void Info( string aText, bool aCallback = false )
    {
        SetLog( $"{_LABEL_INFO} {Log.GetTimeAndThreadInfo} {GetBackMethodName} {aText}" );

        if ( aCallback )
        {
            Callback( 0, aText );
        }
    }

    /// <summary>
    /// 警告ログ
    /// </summary>
    /// <param name="aException">出力内容</param>
    /// <param name="aCallback">True:通知、False:非通知</param>
    public static void Info( Exception aException, bool aCallback = false )
    {
        SetLog( $"{_LABEL_INFO} {Log.GetTimeAndThreadInfo} {GetBackMethodName} {aException.Message}" );
        SetLog( $"{aException.StackTrace}" );

        if ( aCallback )
        {
            Callback( 0, aException.Message );
        }
    }


    /// <summary>
    /// 警告ログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    /// <param name="aCallback">True:通知、False:非通知</param>
    public static void Warning( string aText, bool aCallback = false )
    {
        SetLog( $"{_LABEL_WARNING} {Log.GetTimeAndThreadInfo} {GetBackMethodName} {aText}" );

        if ( aCallback )
        {
            Callback( 1, aText );
        }
    }

    /// <summary>
    /// 警告ログ
    /// </summary>
    /// <param name="aException">出力内容</param>
    /// <param name="aCallback">True:通知、False:非通知</param>
    public static void Warning( Exception aException, bool aCallback = false )
    {
        SetLog( $"{_LABEL_WARNING} {Log.GetTimeAndThreadInfo} {GetBackMethodName} {aException.Message}" );
        SetLog( $"{aException.StackTrace}" );

        if ( aCallback )
        {
            Callback( 1, aException.Message );
        }
    }

    /// <summary>
    /// エラーログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    /// <param name="aCallback">True:通知、False:非通知</param>
    public static void Error( string aText, bool aCallback = false )
    {
        SetLog( $"{_LABEL_ERROR} {Log.GetTimeAndThreadInfo} {GetBackMethodName} {aText}" );

        if ( aCallback )
        {
            Callback( 2, aText );
        }
    }

    /// <summary>
    /// エラーログ
    /// </summary>
    /// <param name="aException">出力内容</param>
    /// <param name="aCallback">True:通知、False:非通知</param>
    public static void Error( Exception aException, bool aCallback = false )
    {
        SetLog( $"{_LABEL_ERROR} {Log.GetTimeAndThreadInfo} {GetBackMethodName} {aException.Message}" );
        SetLog( $"{aException.StackTrace}" );

        if ( aCallback )
        {
            Callback( 2, aException.Message );
        }
    }

    /// <summary>
    /// ブロック開始ログ
    /// </summary>
    /// <param name="aBlockName">ブロック名</param>
    public static void BeginInfo( string aBlockName )
    {
        SetLog( $"{_LABEL_BLOCK_START} {Log.GetTimeAndThreadInfo} {GetBackMethodNameBlockStart} {aBlockName} === Begin ===" );

        _BlockTime.Push( DateTime.Now );
    }

    /// <summary>
    /// ブロック開始ログ
    /// </summary>
    /// <param name="aBlockName">ブロック名</param>
    public static void EndInfo( string aBlockName )
    {
        if ( _BlockTime.TryPop( out var startTime ) )
        {
            var ms = ( DateTime.Now - startTime ).TotalMilliseconds;

            SetLog( $"{_LABEL_BLOCK_END} {Log.GetTimeAndThreadInfo} {GetBackMethodNameBlockEnd} {aBlockName} ===  End  === {ms}ms " );
        }
        else
        {
            SetLog( $"{_LABEL_BLOCK_END} {Log.GetTimeAndThreadInfo} {GetBackMethodNameBlockEnd} {aBlockName} ===  End  === " );
        }
    }


#if false // 使いにくいので削除
    /// <summary>
    /// 共通 例外処理
    /// </summary>
    /// <param name="aAction"></param>
    /// <param name="aFallback"></param>
    /// <param name="aFinally"></param>
    public static void TryCatch( Action aAction, Action<Exception>? aFallback = null, Action? aFinally = null )
    {
        try
        {
            aAction();
        }
        catch ( Exception e )
        {
            Log.Error( $"{GetBackMethodName}:{e.Message}" );
            Log.Error( $"{GetBackMethodName}:{e.StackTrace}" );

            aFallback?.Invoke( e );
        }
        finally
        {
            aFinally?.Invoke();
        }
    }

    /// <summary>
    /// 共通 例外処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="aAction"></param>
    /// <param name="aFallback"></param>
    /// <param name="aFinally"></param>
    /// <returns></returns>
    public static T? TryCatch<T>( Func<T> aAction, Action<Exception>? aFallback = null, Action? aFinally = null )
    {
        try
        {
            return aAction();
        }
        catch ( Exception e )
        {
            Log.Error( $"{GetMethodName( 2 )}:{e.Message}" );
            Log.Error( $"{GetMethodName( 2 )}:{e.StackTrace}" );

            aFallback?.Invoke( e );
            return default;
        }
        finally
        {
            aFinally?.Invoke();
        }
    }

    /// <summary>
    /// 共通 例外処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="aAction"></param>
    /// <param name="aFallback"></param>
    /// <param name="aFinally"></param>
    /// <returns></returns>
    public static T? TryCatch<T>( Func<T> aAction, Func<Exception, T>? aFallback = null, Action? aFinally = null )
    {
        try
        {
            return aAction();
        }
        catch ( Exception e )
        {
            Log.Error( $"{GetBackMethodName}:{e.Message}" );
            Log.Error( $"{GetBackMethodName}:{e.StackTrace}" );

            if ( aFallback == null )
            {
                return default;
            }
            return aFallback( e );
        }
        finally
        {
            aFinally?.Invoke();
        }
    }

    /// <summary>
    /// 共通 例外処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="aAction"></param>
    /// <param name="aFallback"></param>
    /// <param name="aFinally"></param>
    /// <returns></returns>
    public static T TryCatchNotNull<T>( Func<T> aAction, Func<Exception, T> aFallback, Action? aFinally = null )
    {
        try
        {
            return aAction();
        }
        catch ( Exception e )
        {
            Log.Error( $"{GetBackMethodName}:{e.Message}" );
            Log.Error( $"{GetBackMethodName}:{e.StackTrace}" );

            return aFallback( e );
        }
        finally
        {
            aFinally?.Invoke();
        }
    }
#endif

    /// <summary>
    /// ログ出力
    /// </summary>
    /// <param name="aText">出力内容</param>
    private static void SetLog( string aText ) => Trace.WriteLine( aText );

    /// <summary>
    /// 現在実行中のメソッド名を取得
    /// </summary>
    public static string GetThisMethodName => GetMethodName( 1 );

    /// <summary>
    /// 呼び出し元のメソッド名を取得
    /// </summary>
    private static string GetBackMethodName => GetMethodName( 3 );

    /// <summary>
    /// 呼び出し元のメソッド名を取得
    /// </summary>
    private static string GetBackMethodNameBlockStart => GetMethodName( 4 );

    /// <summary>
    /// 呼び出し元のメソッド名を取得
    /// </summary>
    private static string GetBackMethodNameBlockEnd => GetMethodName( 5 );

    /// <summary>
    /// メソッド内を取得
    /// </summary>
    /// <param name="aSkipFrame"></param>
    /// <returns></returns>
    private static string GetMethodName( int aSkipFrame )
    {
        var prevFrame   = new StackFrame( aSkipFrame, false );

        var className   = prevFrame.GetMethod()?.ReflectedType?.Name  ?? string.Empty ;
        var methodName  = prevFrame.GetMethod()?.Name                 ?? string.Empty ;

        return $"{className}.{methodName}()";
    }

    /// <summary>
    /// 現在時刻とスレッド情報取得
    /// </summary>
    private static string GetTimeAndThreadInfo
    {
        get
        {
            var threadId = Environment.CurrentManagedThreadId;
            var threadNm = Thread.CurrentThread.Name;
            return $"{DateTime.Now:HH:mm:ss.ff} [" + ( string.IsNullOrEmpty( threadNm ) ? $"{threadId:00}" : threadNm ) + "]";
        }
    }

    #endregion

    #region ログ出力通知

    /// <summary>
    /// ログ出力コールバック
    /// </summary>
    /// <param name="aLevel">0:Info, 1:Warning, 2:Error</param>
    /// <param name="aText">出力内容</param>
    public delegate void LogOutput( int aLevel, string aText );

    /// <summary>
    /// ログ出力コールバック
    /// </summary>
    public static readonly ConcurrentQueue<LogOutput> LogOutputCallback = new();

    /// <summary>
    /// ログ出力通知
    /// </summary>
    /// <param name="aLevel">0:Info, 1:Warning, 2:Error</param>
    /// <param name="aText">出力内容</param>
    private static void Callback( int aLevel, string aText )
    {
        foreach ( var callback in LogOutputCallback )
        {
            try
            {
                callback( aLevel, aText );
            }
            catch ( Exception e )
            {
                Log.Warning( e );
            }
        }
    }

    private static GeneralPath _TraceLogPath = new();

    /// <summary>
    /// ログファイルへTraceログを出力するように設定
    /// </summary>
    public static void SetLogFile( GeneralPath aLogPath )    
    {
        _TraceLogPath = aLogPath;

        var log_file = File.Create( _TraceLogPath.AbsoluteFilePath );
        _ = Trace.Listeners.Add( new TextWriterTraceListener( log_file ) );
        Trace.AutoFlush = true;
    }

    /// <summary>
    /// ログファイルを開く
    /// </summary>
    public static void OpenLogFile()
    {
        try
        {
            Process.Start( "EXPLORER.EXE", _TraceLogPath.AbsoluteFilePath );
        }
        catch ( Exception )
        {            
        }
    }

    #endregion
}
