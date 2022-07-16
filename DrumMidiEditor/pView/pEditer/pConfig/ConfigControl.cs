using System;
using System.Drawing;
using System.Windows.Forms;

using OpenCvSharp;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pAudio;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pEditer.pConfig;

/// <summary>
/// Scoreタブコントロール
/// </summary>
public partial class ConfigControl : UserControl
{
	/// <summary>
	/// コンストラクタ
	/// </summary>
	public ConfigControl()
	{
		InitializeComponent();

		InitUpdate();
    }

	/// <summary>
	/// 初期設定
	/// </summary>
	private void InitUpdate()
	{
		SuspendLayout();
		{
			#region Device
			{
				var list = MidiNet.GetDeviceList();

				for ( int index = 0; index < list.Count; index++ )
				{
					var name = list[ index ];

					MidiDeviceListBox.Items.Add( name );

					if ( Config.Media.MidiOutDeviceName.Equals( name ) )
					{
						MidiDeviceListBox.SelectedIndex = index;
					}
				}

				MidiOutLatencyNumericUpDown.Value = (decimal)Config.Media.MidiOutLatency * 1000;
			}
			#endregion

			#region Video
			{
				var codec = FourCC.FromInt32( Config.Media.OutputVideoCodec );
				foreach ( var item in CodecComboBox.Items )
				{ 
					if ( FourCC.FromString( item?.ToString() ?? String.Empty ) == codec )
					{ 
						CodecComboBox.SelectedItem = item;
						break;
					}
				}

				FpsNumericUpDown.Value = (decimal)Config.Media.OutputVideoFps;
			}
			#endregion

			#region ToolTip

			//ToolTip.SetToolTip( VolumeSizeCheckBox			, "Change note volume size on/off" );

			#endregion

		}
		ResumeLayout( false );
	}

	#region Device

	/// <summary>
	/// MIDI-OUTデバイス選択
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void MidiDeviceListBox_SelectedIndexChanged( object sender, EventArgs ev )
    {
		try
		{ 
			var index = MidiDeviceListBox.SelectedIndex;

			Config.Media.MidiOutDeviceName	= String.Empty;

			if ( index != -1 )
			{
				Config.Media.MidiOutDeviceName = MidiDeviceListBox.Items[ index ]?.ToString() ?? String.Empty;
			}

			if ( !MidiNet.InitDevice() )
			{
				Log.Error( $"Initialization failure... Midi device" );
				return;
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// MIDI-OUT遅延時間変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
    private void MidiOutLatencyNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{ 
			Config.Media.MidiOutLatency = (double)MidiOutLatencyNumericUpDown.Value / 1000D;

			Config.EventReloadBgm();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    #endregion

    #region Video

    private void CodecComboBox_SelectedIndexChanged( object sender, EventArgs ev )
    {
		try
		{
			var index = CodecComboBox.SelectedIndex;

			if ( index != -1 )
			{
				Config.Media.OutputVideoCodec = FourCC.FromString( CodecComboBox.Items[ index ].ToString() ?? String.Empty );
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}

    }

	/// <summary>
	/// Fps設定
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void FpsNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			Config.Media.OutputVideoFps = (int)FpsNumericUpDown.Value;
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    #endregion
}
