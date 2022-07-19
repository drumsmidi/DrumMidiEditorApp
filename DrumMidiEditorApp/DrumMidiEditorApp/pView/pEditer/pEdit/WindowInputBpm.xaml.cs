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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DrumMidiEditorApp.pView.pEditer.pEdit;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class WindowInputBpm : Window
{
    public WindowInputBpm()
    {
        this.InitializeComponent();
    }

#if false
	/// <summary>
	/// BPM入力フラグ
	/// </summary>
	private DialogResult _Result = DialogResult.None;

	/// <summary>
	/// BPM入力値
	/// </summary>
	public double Bpm
	{
		get => (double)BpmNumericUpDown.Value;
		set => BpmNumericUpDown.Value = (decimal)value;
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public BpmInputForm()
    {
        InitializeComponent();
    }

	/// <summary>
	/// 入力フォーム表示
	/// </summary>
	/// <param name="aBpm">BPM初期値</param>
	/// <returns>入力実施フラグ</returns>
	public DialogResult ShowDialog( double aBpm )
	{
        try
        {
			if ( aBpm <= 0 )
			{
				aBpm = Config.System.DefaultBpm;
			}

			Bpm = aBpm;

			ShowDialog();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}

		return _Result;
	}

	/// <summary>
	/// 入力完了
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void OkButton_Click( object sender, EventArgs ev )
    {			
        try
        {
			_Result = DialogResult.Yes;
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
