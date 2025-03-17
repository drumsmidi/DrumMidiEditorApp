using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView;

public sealed partial class PageEditerMain : Page, INotifyPropertyChanged
{
    /// <summary>
    /// プレイヤー設定
    /// </summary>
    private ConfigPlayer ConfigPlayer => Config.Player;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageEditerMain()
    {
        InitializeComponent();

        ControlAccess.PageEditerMain = this;
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadPlayer() => OnPropertyChanged( "ConfigPlayer" );

    #endregion

    /// <summary>
    /// リサイズイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Page_SizeChanged( object sender, SizeChangedEventArgs args )
    {
        try
        {
            EventManage.Event_Window_ResizeWindow();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
