using System;
using DrumMidiEditorApp.pMachineLearning;
using DrumMidiLibrary.pLog;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pML;

public sealed partial class PageML : Page
{
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
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>" )]
    private void MachineLearning_Click( object aSender, RoutedEventArgs aArgs )
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
