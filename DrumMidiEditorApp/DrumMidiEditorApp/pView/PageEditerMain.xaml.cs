using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiLibrary.pLog;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView;

public sealed partial class PageEditerMain : Page, INotifyPropertyChanged
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageEditerMain()
    {
        InitializeComponent();

        ControlAccess.PageEditerMain = this;
    }

    #region member

    /// <summary>
    /// プレイヤー設定
    /// </summary>
    private ConfigPlayer ConfigPlayer => Config.Player;

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadPlayer()
    {
        try
        {
            OnPropertyChanged( "ConfigPlayer" );
}
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    /// <summary>
    /// リサイズイベント
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Page_SizeChanged( object aSender, SizeChangedEventArgs aArgs )
    {
        try
        {
            EventManage.Event_Window_ResizeWindow();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}
