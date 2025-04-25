using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pMenuBar;

public sealed partial class PageImportMidi : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageImportMidi()
    {
        InitializeComponent();

        // NumberBox の入力書式設定
        _BpmZoomNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 4, 0.0125 );
    }

    #region member

    /// <summary>
    /// BPM取込倍率
    /// </summary>
    public float BpmZoom { get; set; } = Config.Media.MidiImportZoom;

    #endregion

    /// <summary>
    /// ＢＰＭ取込倍率変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void BpmZoomNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}
