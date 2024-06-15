using System;
using DrumMidiClassLibrary.pConfig;
using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pWinUI;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pMenuBar;

public sealed partial class PageImportMidi : Page
{
    /// <summary>
    /// BPM取込倍率
    /// </summary>
    public float BpmZoom { get; set; } = Config.Media.MidiImportZoom;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageImportMidi()
    {
        InitializeComponent();

        // NumberBox の入力書式設定
        _BpmZoomNumberBox.NumberFormatter
            = XamlHelper.CreateNumberFormatter( 1, 4, 0.0125 );
    }

    /// <summary>
    /// ＢＰＭ取込倍率変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void BpmZoomNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
        try
        {
            // 必須入力チェック
            if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
                return;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
