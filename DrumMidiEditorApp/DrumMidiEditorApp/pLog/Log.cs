using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;

using DrumMidiEditorApp.pUtil;

namespace DrumMidiEditorApp.pLog;

/// <summary>
/// ログ出力
/// </summary>
public static class Log
{
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
    /// 情報ログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    public static void Info( string aText ) => Info( aText, false );

    /// <summary>
    /// 情報ログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    /// <param name="aCallback">True:通知、False:非通知</param>
    public static void Info( string aText, bool aCallback )
    {
        SetLog( $"{_LABEL_INFO} {Log.GetTimeAndThreadInfo} {aText}" );

        if ( aCallback )
        {
            Callback( 0, aText );
        }
    }

    /// <summary>
    /// 警告ログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    public static void Warning( string aText ) => Warning( aText, false );

    /// <summary>
    /// 警告ログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    /// <param name="aCallback">True:通知、False:非通知</param>
    public static void Warning( string aText, bool aCallback )
    {
        SetLog( $"{_LABEL_WARNING} {Log.GetTimeAndThreadInfo} {aText}" );

        if ( aCallback )
        {
            Callback( 1, aText );
        }
    }

    /// <summary>
    /// エラーログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    [Conditional( "DEBUG" )]
    public static void Error( Exception aException ) => SetLog( $"{aException.StackTrace}" );

    /// <summary>
    /// エラーログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    public static void Error( string aText ) => Error( aText, true );

    /// <summary>
    /// エラーログ
    /// </summary>
    /// <param name="aText">出力内容</param>
    /// <param name="aCallback">True:通知、False:非通知</param>
    public static void Error( string aText, bool aCallback )
    {
        SetLog( $"{_LABEL_ERROR} {Log.GetTimeAndThreadInfo} {aText}" );

        if ( aCallback )
        {
            Callback( 2, aText );
        }
    }

    /// <summary>
    /// ブロック開始ログ
    /// </summary>
    /// <param name="aBlockName">ブロック名</param>
    public static void BeginInfo( string aBlockName )
    {
        SetLog( $"{_LABEL_INFO} {Log.GetTimeAndThreadInfo} {aBlockName} === Begin ===" );

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

            SetLog( $"{_LABEL_INFO} {Log.GetTimeAndThreadInfo} {aBlockName} ===  End  === {ms}ms " );
        }
        else
        {
            SetLog( $"{_LABEL_INFO} {Log.GetTimeAndThreadInfo} {aBlockName} ===  End  === " );
        }
    }

    /// <summary>
    /// ログ出力
    /// </summary>
    /// <param name="aText">出力内容</param>
    private static void SetLog( string aText ) => Trace.WriteLine( aText );

    /// <summary>
    /// 現在実行中のメソッド名を取得
    /// </summary>
    public static string GetThisMethodName
    {
        get
        {
            var prevFrame   = new StackFrame( 1, false );

            var className   = prevFrame.GetMethod()?.ReflectedType?.Name  ?? string.Empty ;
            var methodName  = prevFrame.GetMethod()?.Name                 ?? string.Empty ;

            return $"{className}.{methodName}()";
        }
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
            callback( aLevel, aText );
        }
    }

    /// <summary>
    /// ログファイルへTraceログを出力するように設定
    /// </summary>
    public static void SetLogFile( GeneralPath aLogPath )
    {
        var log_file = File.Create( aLogPath.AbsoulteFilePath );

        _ = Trace.Listeners.Add( new TextWriterTraceListener( log_file ) );
        Trace.AutoFlush = true;
    }

    #endregion
}
