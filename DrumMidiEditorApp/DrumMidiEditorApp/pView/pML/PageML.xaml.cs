using System;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pConfig;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DrumMidiEditorApp.pMachineLearning;

namespace DrumMidiEditorApp.pView.pML;

public sealed partial class PageML : Page
{
    /// <summary>
    /// スコア設定
    /// </summary>
    private ConfigMachine DrawSet => Config.Machine;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageML()
    {
        InitializeComponent();

        ControlAccess.PageML = this;
    }

    /// <summary>
    /// 再生停止
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MachineLearning_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            MLControl.Start();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
