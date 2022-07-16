using System;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pEditer.pMusic;

/// <summary>
/// Musicタブ表示
/// </summary>
public partial class MusicControl : UserControl
{
	/// <summary>
	/// コンストラクタ
	/// </summary>
	public MusicControl()
	{
		InitializeComponent();

		SuspendLayout();
        {
			EqualizerOnCheckBox.Checked	= Config.Equalizer.EqualizerOn;
			WaveOnCheckBox.Checked		= Config.Equalizer.WaveFormOn;

            #region ToolTip

            ToolTip.SetToolTip( BgmFilePathTextBox	, "Double-click to open daialog" );
            ToolTip.SetToolTip( EqualizerResetButton, "Equalizer input reset" );
			ToolTip.SetToolTip( EqualizerOnCheckBox	, "Equalizer on/off" );
			ToolTip.SetToolTip( WaveOnCheckBox		, "WaveForm on/off" );

            #endregion
		}
		ResumeLayout( false );
	}

    #region MusicInfo

	/// <summary>
	/// 入力フィールドへスコアの内容を反映
	/// </summary>
    public void ApplyScore()
	{
		this.SuspendLayout();

		try
		{
			InfoTextBox.Text				= DMS.SCORE.Info;
			BgmFilePathTextBox.Text			= DMS.SCORE.BgmFilePath.RelativeFilePath;
			BgmBpmNumericUpDown.Value		= (decimal)DMS.SCORE.Bpm;
			BgmRpmNumericUpDown.Value		= (decimal)( DMS.SCORE.BgmReproducePosition * 1000D );
			BgmVolumeNumericUpDown.Value	= DMS.SCORE.BgmVolume;
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}

		this.ResumeLayout( false );
	}

	/// <summary>
	/// BGM再読込
	/// </summary>
	private static void UpdateMusicInfo() => Config.EventReloadBgm();

	/// <summary>
	/// BGMファイルの選択
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void BgmFilePathTextBox_MouseDoubleClick( object sender, MouseEventArgs ev )
	{
		try
		{
			DMS.PlayerForm?.TemporaryHide();

			if ( !FormUtil.OpenShowDialog( Config.System.FolderBgm, Config.System.SupportBgm, out var filepath ) )
            {
				return;
            }

			BgmFilePathTextBox.Text = filepath.RelativeFilePath ?? String.Empty ;

			DMS.SCORE.BgmFilePath = new( filepath );

            UpdateMusicInfo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
			DMS.PlayerForm?.TemporaryShow();
		}
	}

	/// <summary>
	/// BGMファイルの変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void BgmFilePathTextBox_TextChanged( object sender, EventArgs ev )
	{
		try
		{
			DMS.SCORE.BgmFilePath = new( BgmFilePathTextBox.Text );

            UpdateMusicInfo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// BGM再生開始時間の変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void BgmRpmNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			DMS.SCORE.BgmReproducePosition = (double)BgmRpmNumericUpDown.Value / 1000D;

            UpdateMusicInfo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// BPMの変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void BgmBpmNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			DMS.SCORE.Bpm = (double)BgmBpmNumericUpDown.Value;

            UpdateMusicInfo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// BGM音量の変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void BgmVolumeNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			DMS.SCORE.BgmVolume = (byte)BgmVolumeNumericUpDown.Value;

			// DmsControl内で都度、BGMの音量を変更しているので更新不要
			// UpdateMusicInfo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// メモの変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void InfoTextBox_TextChanged( object sender, EventArgs ev )
	{
		try
		{
			DMS.SCORE.Info = InfoTextBox.Text;

			// 今の所、他の処理には影響しないので更新不要
			// UpdateMusicInfo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	#endregion

	#region Equalizer

	/// <summary>
	/// BGMへイコライザ入力内容を適用
	/// </summary>
	public void ApplyEqulizer() => EqualizerPanel.UpdateEqualizer();

	/// <summary>
	/// イコライザの入力およびBGMのイコライザ設定をリセット
	/// </summary>
	public void ResetEqulizer() 
	{
		EqualizerPanel.ResetEqualizer();
		EqualizerPanel.Refresh();
	}

	/// <summary>
	/// イコライザON/OFFの切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void EqualizerOnCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
		try
		{
            Config.Equalizer.EqualizerOn = EqualizerOnCheckBox.Checked;

			ApplyEqulizer();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// イコライザリセット
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
    private void EqualizerResetButton_Click( object sender, EventArgs ev )
    {
		try
		{
			ResetEqulizer();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// WaveFormON/OFFの切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void WaveOnCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
            Config.Equalizer.WaveFormOn = WaveOnCheckBox.Checked;

			EqualizerPanel.Refresh();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	#endregion
}

