using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class WindowImportMidi : Window
{
    public WindowImportMidi()
    {
        this.InitializeComponent();
    }

#if false
    /// <summary>
    /// 取込実施フラグ
    /// </summary>
	private DialogResult _Result = DialogResult.None;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ImportMidiForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// フォーム表示
    /// </summary>
    /// <param name="aZoom">BPMズーム初期値</param>
    /// <returns>取込実施フラグ</returns>
	public DialogResult ShowDialog( int aZoom )
	{
        try
        {
            ZoomNumericUpDown.Value = aZoom;

    		ShowDialog();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}

		return _Result;
	}

    /// <summary>
    /// 取込実施
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
	private void OkButton_Click( object sender, EventArgs ev )
    {
        try
        { 
            Config.Media.MidiImportZoom = (int)ZoomNumericUpDown.Value;

			_Result = DialogResult.OK;
            Hide();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    /// <summary>
    /// キャンセル
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void CxlButton_Click( object sender, EventArgs ev )
    {
        try
        { 
			_Result = DialogResult.Cancel;
            Hide();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }
#endif
}
