﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pStatusBar;

public sealed partial class PageStatusBar : Page, INotifyPropertyChanged
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageStatusBar()
    {
        InitializeComponent();

        ControlAccess.PageStatusBar = this;

        // プログレスバーの初期値を設定
        _ProgressBar.Minimum = Config.System.ProgressBarMinValue;
        _ProgressBar.Maximum = Config.System.ProgressBarMaxValue;

        // ログ出力の通知を受け取る
        Log.LogOutputCallback.Enqueue( SetStatusText );
    }

    #region member

    /// <summary>
    /// 進捗バー
    /// </summary>
    private double _ProgressBarValue = 0;

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// プログレスバー再読み込み
    /// 
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    /// <param name="aProgressBarValue">進捗バー（０～１００）</param>
    public void ReloadProgressBar( double aProgressBarValue )
    {
        try
        {
            if ( !HelperXaml.DispatcherQueueHasThreadAccess( this, () => ReloadProgressBar( aProgressBarValue ) ) )
            {
                return;
            }

            _ProgressBarValue = Math.Clamp( aProgressBarValue, _ProgressBar.Minimum, _ProgressBar.Maximum );

            OnPropertyChanged( "_ProgressBarValue" );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    #region InfoBar

    /// <summary>
    /// ステータスバーへのログ出力
    /// </summary>
    /// <param name="aLevel">0:Info, 1:Warning, 2:Error</param>
    /// <param name="aText">出力内容</param>
    private void SetStatusText( int aLevel, string aText )
    {
        try
        {
            switch ( aLevel )
            {
                case 0:
                    SetStatusText( "Info", aText, InfoBarSeverity.Informational );
                    break;
                case 1:
                    SetStatusText( "Warn", aText, InfoBarSeverity.Warning );
                    break;
                case 2:
                    SetStatusText( "Error", aText, InfoBarSeverity.Error );
                    break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ステータスバーテキスト出力
    /// </summary>
    /// <param name="aTitle">タイトル</param>
    /// <param name="aContent">出力内容</param>
    /// <param name="aSeverity"></param>
    private void SetStatusText( string aTitle, string aContent, InfoBarSeverity aSeverity )
    {
        try
        {
            if ( !HelperXaml.DispatcherQueueHasThreadAccess( this, () => SetStatusText( aTitle, aContent, aSeverity ) ) )
            {
                return;
            }

            _InfoBar.Title      = aTitle;
            _InfoBar.Message    = aContent;
            _InfoBar.Severity   = aSeverity;
            _InfoBar.IsOpen     = true;
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion
}
