using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PageScore : Page
{
    public PageScore()
    {
        InitializeComponent();
    }

    private void Page_Unloaded( object sender, RoutedEventArgs args )
    {
        try
        {
            _ScoreCanvas.RemoveFromVisualTree();
            _ScoreCanvas = null;
        }
        catch ( Exception e )
        {

        }
    }


    private void CanvasControl_Draw( CanvasControl sender, CanvasDrawEventArgs args )
    {
        try
        {
            args.DrawingSession.DrawText("Hello, World!", new(100,100), Color.FromArgb( 255, 33, 33, 33) );
            args.DrawingSession.DrawCircle(125, 125, 100, Colors.Green);
            args.DrawingSession.DrawLine(0, 0, 50, 200, Colors.Red);
        }
        catch ( Exception e )
        {

        }
    }


#if false





	/// <summary>
	/// プレイヤー描画設定
	/// </summary>
	private static ConfigScore DrawSet => Config.Score;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public ScoreControl()
	{
		InitializeComponent();

		SuspendLayout();
		{
            #region Sequence

			// Note
			VolumeSizeCheckBox.Checked			= DrawSet.NoteVolumeSizeOn;
			VolumeZeroCheckBox.Checked			= DrawSet.NoteVolumeZeroOn;
			NoteHeightNumericUpDown.Value		= (Decimal)DrawSet.NoteHeightSize;
			NoteWidthNumericUpDown.Value		= (Decimal)DrawSet.NoteWidthSize;

			#endregion

			#region ToolTip

			ToolTip.SetToolTip( VolumeSizeCheckBox			, "Change note volume size on/off" );
			ToolTip.SetToolTip( VolumeZeroCheckBox			, "Change note volume zero on/off" );
			ToolTip.SetToolTip( NoteHeightNumericUpDown		, "Note height" );
			ToolTip.SetToolTip( NoteWidthNumericUpDown		, "Note width" );

			#endregion
		}
		ResumeLayout( false );
    }

	#region Note

	/// <summary>
	/// 音量サイズ変更フラグ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void VolumeSizeCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSet.NoteVolumeSizeOn = VolumeSizeCheckBox.Checked;

			UpdateScore();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// 音量０表示フラグ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void VolumeZeroCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSet.NoteVolumeZeroOn = VolumeZeroCheckBox.Checked;

			UpdateScore();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

	/// <summary>
	/// ノートサイズ：縦　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void NoteHeightNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSet.NoteHeightSize = (int)NoteHeightNumericUpDown.Value;

			UpdateScore();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// ノートサイズ：横　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void NoteWidthNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSet.NoteWidthSize = (int)NoteWidthNumericUpDown.Value;

			UpdateScore();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	#endregion

	/// <summary>
	/// スコア更新フラグ設定
	/// </summary>
	private void UpdateScore()
	{
		Config.EventUpdateScoreTab();
		ScorePanel.Refresh();
	}

	/// <summary>
	/// リサイズ
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
    private void ScorePanel_Resize( object sender, EventArgs ev )
    {
		try
		{
			Config.EventEditerFormResize();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }
#endif

}
