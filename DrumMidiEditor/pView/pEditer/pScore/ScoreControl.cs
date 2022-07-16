using System;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pGeneralFunction.pLog;

namespace DrumMidiEditor.pView.pEditer.pScore;

/// <summary>
/// Scoreタブコントロール
/// </summary>
public partial class ScoreControl : UserControl
{
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
}
