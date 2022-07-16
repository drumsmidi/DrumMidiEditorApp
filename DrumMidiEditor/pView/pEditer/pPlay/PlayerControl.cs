using System;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pControl;
using DrumMidiEditor.pIO;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pGeneralFunction.pUtil;
using DrumMidiEditor.pView.pEditer.pEdit;

namespace DrumMidiEditor.pView.pEditer.pPlay;

/// <summary>
/// プレイヤーコントロール
/// </summary>
public partial class PlayerControl : UserControl
{
	/// <summary>
	/// コンストラクタ
	/// </summary>
	public PlayerControl()
	{
		InitializeComponent();

		SuspendLayout();
		{
			for ( byte channelNo = Config.Media.ChannelMinNo; channelNo <= Config.Media.ChannelMaxNo; channelNo++ )
            {
				ChannelComboBox.Items.Add( channelNo.ToString() );
			}
			ChannelComboBox.SelectedIndex = Config.Media.ChannelDrum;

			PlayEndMeasureNumericUpDown.Maximum		= Config.System.MeasureMaxNumber;
			PlayEndMeasureNumericUpDown.Value		= Config.System.MeasureMaxNumber;
			PlayStartMeasureNumericUpDown.Maximum	= Config.System.MeasureMaxNumber;
			PlayStartMeasureNumericUpDown.Value		= Config.Media.PlayLoopStart;
			PlayEndMeasureNumericUpDown.Value		= Config.Media.PlayLoopEnd;
			MeasureConnectCheckBox.Checked			= Config.Media.PlayLoopConnectOn;
			MeasureConnectNumericUpDown.Value		= Config.Media.PlayLoopConnect;
			BgmCheckBox.Checked						= Config.Media.BgmPlayOn;
			NoteCheckBox.Checked					= Config.Media.NotePlayOn;

			PlayerCheckBox.Checked					= Config.Player.DisplayPlayer;
			PlayerConfigCheckBox.Checked			= Config.Player.DisplayPlayerConfig;

            #region ToolTip

            ToolTip.SetToolTip( ChannelComboBox					, "Select channel" );
			ToolTip.SetToolTip( BgmCheckBox						, "Change bgm on/off" );
			ToolTip.SetToolTip( NoteCheckBox					, "Change midi-out on/off" );
			ToolTip.SetToolTip( PlayButton						, "Play" );
			ToolTip.SetToolTip( StopButton						, "Stop" );
			ToolTip.SetToolTip( RangePlayButton					, "Range play" );
			ToolTip.SetToolTip( PlayStartMeasureNumericUpDown	, "Range play : start measure no" );
			ToolTip.SetToolTip( PlayEndMeasureNumericUpDown		, "Range play : end measure no" );
			ToolTip.SetToolTip( MeasureConnectCheckBox			, "Range play : connect start - end" );
			ToolTip.SetToolTip( MeasureConnectNumericUpDown		, "Range play : connect number" );
			ToolTip.SetToolTip( PlayerCheckBox					, "Change player on/off" );
			ToolTip.SetToolTip( PlayerConfigCheckBox			, "Change player setting on/off" );

            #endregion
        }
        ResumeLayout( false );
	}

	/// <summary>
	/// スコアをシステム全体に反映
	/// </summary>
    private void ApplyScore()
    {
        try
        {
			Config.EventReloadScore();

			if ( DMS.EditerForm != null )
			{
				DMS.SCORE.EditChannelNo = (byte)ChannelComboBox.SelectedIndex;

				DMS.EditerForm.MusicCtl.ApplyScore();
				DMS.EditerForm.MusicCtl.ApplyEqulizer();
				DMS.EditerForm.MidiMapSetCtl.LoadMidiMapSet( DMS.SCORE.EditMidiMapSet );
				DMS.EditerForm.Refresh();

				DMS.EditerForm.Text = $"{Config.System.AppName} [{Config.System.OpenFilePath.AbsoulteFilePath}]";
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	#region Menu

	/// <summary>
	/// 再生停止＆プレイヤーフォーム一時非表示
	/// </summary>
	private static void PlayerStop()
    {
		DmsControl.StopPreSequence();

		DMS.PlayerForm?.TemporaryHide();
	}

	/// <summary>
	/// プレイヤーフォーム再表示
	/// </summary>
	private static void PlayerPlay()
	{
		DMS.PlayerForm?.TemporaryShow();
	}

	/// <summary>
	/// メニュー：DMS新規作成
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void NewFileToolStripMenuItem_Click( object sender, EventArgs ev )
    {
        try
        {
            PlayerStop();

			var ds = MessageBox.Show( "The file you are editing will not be saved.\nAre you sure you want to create a new file?", "Confirmation", MessageBoxButtons.YesNo );
            if ( ds != DialogResult.Yes )
            {
				return;
            }

			DMS.SCORE = new();

            Config.System.OpenFilePath = new();

			ApplyScore();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
            PlayerPlay();
		}
	}

	/// <summary>
	/// メニュー：DMS開く
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void OpenFileToolStripMenuItem_Click( object sender, EventArgs ev )
    {
		try
		{
            PlayerStop();

			if ( !FormUtil.OpenShowDialog( Config.System.FolderDms, Config.System.SupportDmsOpen, out var filepath ) )
            {
				return;
            }

            if ( !FileIO.LoadScore( filepath, out var score ) )
            {
				return;
            }

			DMS.SCORE = score;

            Config.System.OpenFilePath = filepath;

            ApplyScore();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
		{
            PlayerPlay();
		}
	}

	/// <summary>
	/// メニュー：DMS上書保存
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void SaveFileToolStripMenuItem_Click( object sender, EventArgs ev )
    {
		try
        {
            PlayerStop();

			var filepath = Config.System.OpenFilePath;

			if ( !filepath.IsExistFile )
			{ 
				if ( !FormUtil.SaveShowDialog( Config.System.FolderDms, Config.System.OpenFilePath, Config.System.SupportDmsSave, out filepath ) )
				{
					return;
				}
			}

			filepath.Extension = Config.System.ExtentionDms;

            if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
            {
                return;
            }

            Config.System.OpenFilePath = filepath;

			ApplyScore();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
		{
            PlayerPlay();
		}
	}

	/// <summary>
	/// メニュー：DMS別名保存
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void SaveAsFileToolStripMenuItem_Click( object sender, EventArgs ev )
    {
        try
        {
            PlayerStop();

			if ( !FormUtil.SaveShowDialog( Config.System.FolderDms, Config.System.OpenFilePath, Config.System.SupportDmsSave, out var filepath ) )
			{
				return;
			}

			filepath.Extension = Config.System.ExtentionDms;

			if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
            {
                return;
            }

            Config.System.OpenFilePath = filepath;

			ApplyScore();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
		{
            PlayerPlay();
		}
	}

	/// <summary>
	/// メニュー：エクスポートMidi
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void OutputMidiToolStripMenuItem_Click( object sender, EventArgs ev )
	{
		try
		{
            PlayerStop();

			if ( !FormUtil.SaveShowDialog( Config.System.FolderMidi, Config.System.OpenFilePath, Config.System.SupportMidi, out var filepath ) )
			{
				return;
			}

			if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
			{
				return;
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
		{
            PlayerPlay();
		}
	}

	/// <summary>
	/// メニュー：エクスポートVideo
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void OutputVideoToolStripMenuItem_Click( object sender, EventArgs ev )
	{
		try
		{
            PlayerStop();

			if ( !FormUtil.SaveShowDialog( Config.System.FolderVideo, Config.System.OpenFilePath, Config.System.SupportVideo, out var filepath ) )
			{
				return;
			}

			if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
			{
				return;
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
		{
            PlayerPlay();
		}
	}

	/// <summary>
	/// メニュー：エクスポートTechMania
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void OutputTechManiaToolStripMenuItem_Click( object sender, EventArgs ev )
    {
		try
		{
            PlayerStop();

			if ( !FormUtil.SaveShowDialog( Config.System.FolderTechMania, Config.System.OpenFilePath, Config.System.SupportTechMania, out var filepath ) )
			{
				return;
			}

			if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
			{
				return;
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
		{
            PlayerPlay();
		}
    }

	/// <summary>
	/// メニュー：インポートMIDI
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void ImportMidiToolStripMenuItem_Click( object sender, EventArgs ev )
	{
		try
        {
            PlayerStop();

			if ( !FormUtil.OpenShowDialog( Config.System.FolderMidi, Config.System.SupportMidi, out var filepath ) )
            {
				return;
            }

			using var fm = new ImportMidiForm();
                
			if ( fm.ShowDialog( Config.Media.MidiImportZoom ) != DialogResult.OK )
            {
				return;
            }

			var score = DMS.SCORE;

            if ( !FileIO.ImportScore( filepath, ref score ) )
            {
				return;
            }

			DMS.SCORE = score;

            ApplyScore();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
		{
            PlayerPlay();
		}
	}

	#endregion

	#region Function

	/// <summary>
	/// チャンネル切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void ChannelComboBox_SelectedIndexChanged( object sender, EventArgs ev )
	{
		try
		{
			DMS.SCORE.EditChannelNo = (byte)Convert.ToInt32( ChannelComboBox.SelectedItem.ToString() ?? "0" );

			Config.EventChangeChannel();

			if ( DMS.EditerForm != null )
			{ 
				DMS.EditerForm.MidiMapSetCtl.LoadMidiMapSet( DMS.SCORE.EditMidiMapSet );
				DMS.EditerForm.Refresh();
			}

		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// BGM再生切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void BgmCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
            Config.Media.BgmPlayOn = BgmCheckBox.Checked;
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// NOTE再生切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void NoteCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
            Config.Media.NotePlayOn = NoteCheckBox.Checked;
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// 通常再生
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void PlayButton_Click( object sender, EventArgs ev )
	{
		try
		{
			DmsControl.PlayPreSequence();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ループ再生
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void RangePlayButton_Click( object sender, EventArgs ev )
	{
		try
		{
			DmsControl.PlayPreLoopSequence();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// 再生停止
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void StopButton_Click( object sender, EventArgs ev )
	{
		try
		{
			DmsControl.StopPreSequence();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ループ再生：小節開始番号変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void PlayStartMeasureNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			MeasureLoopSetting();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ループ再生：小節終了番号変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void PlayEndMeasureNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			MeasureLoopSetting();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ループ再生：小節番号接続切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void MeasureConnectCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
            Config.Media.PlayLoopConnectOn = MeasureConnectCheckBox.Checked;

			MeasureLoopSetting();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ループ再生：小節番号接続数変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void MeasureConnectNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			MeasureLoopSetting();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ループ再生：小節開始／終了番号設定
	/// </summary>
	private void MeasureLoopSetting()
	{
		SuspendLayout();
		{ 
			int start	= (int)PlayStartMeasureNumericUpDown.Value;
			int end		= (int)PlayEndMeasureNumericUpDown.Value;
			int conn	= (int)MeasureConnectNumericUpDown.Value;
			int min		= (int)PlayStartMeasureNumericUpDown.Minimum;
			int max		= (int)PlayEndMeasureNumericUpDown.Maximum;

			if ( start < min )
			{
				start = min;
			}
			if ( start > max )
			{
				start = max;
			}

			if ( Config.Media.PlayLoopConnectOn )
			{
				end = start + conn;
			}
			else if ( start > end )
			{
				end = start;
			}

			if ( end > max )
			{
				end = max;
			}

			PlayStartMeasureNumericUpDown.Value	= (decimal)start;
			PlayEndMeasureNumericUpDown.Value	= (decimal)end;

			Config.Media.PlayLoopStart	= start;
			Config.Media.PlayLoopEnd	= end;
		}
		ResumeLayout( false );
	}

	/// <summary>
	/// Playerフォーム表示切替
	/// </summary>
	public void DoPlayerCheckBox()
    {
		try
		{
			PlayerCheckBox.Checked = !PlayerCheckBox.Checked;
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// プレイヤーフォーム表示切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void PlayerCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
		try
		{
            Config.Player.DisplayPlayer = PlayerCheckBox.Checked;
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// プレイヤー設定フォーム表示切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void PlayerConfigCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
		try
		{
            Config.Player.DisplayPlayerConfig = PlayerConfigCheckBox.Checked;
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

    #endregion
}
