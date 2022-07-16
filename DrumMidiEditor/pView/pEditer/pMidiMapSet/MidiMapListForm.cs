using System;
using System.Windows.Forms;

using DrumMidiEditor.pAudio;
using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pLog;

namespace DrumMidiEditor.pView.pEditer.pMidiMapSet;

/// <summary>
/// MidiMapリストフォーム
/// </summary>
public partial class MidiMapListForm : Form
{
	/// <summary>
	/// 選択したMidiMapキー
	/// </summary>
	private int SelectKey = Config.System.MidiMapKeyNotSelect;

	/// <summary>
	/// コンストラクタ
	/// </summary>
    public MidiMapListForm()
    {
        InitializeComponent();
    }

	/// <summary>
	/// フォーム表示
	/// </summary>
	/// <param name="aMidiMapKey">編集対象のMidiMapキー</param>
	/// <returns></returns>
	public int ShowDialog( int aMidiMapKey )
    {
        try
        {
			MidiMapListBox.Items.Clear();

			foreach ( var group in DMS.SCORE.EditMidiMapSet.MidiMapGroups )
			{
				foreach ( var midiMap in group.MidiMaps )
				{
					MidiMapListBox.Items
						.Add( $"{midiMap.MidiMapKey,-3} {DMS.SCORE.EditMidiMapSet.GetGroupAndMidiMapName( midiMap.MidiMapKey )}" ); 
				}
			}

			NoSelectButton.Text	= $"{aMidiMapKey,-3} {DMS.SCORE.EditMidiMapSet.GetGroupAndMidiMapName( aMidiMapKey )}";
			SelectKey			= aMidiMapKey;

			Location = Cursor.Position;
			ShowDialog();
			Owner.Focus();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}

        return SelectKey;
    }

	/// <summary>
	/// キャンセル
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void NoSelectButton_MouseClick( object sender, MouseEventArgs ev )
	{
        try
        {
			Hide();
        }
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// 選択したMidiMap音の再生
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void MidiMapListBox_MouseClick( object sender, MouseEventArgs ev )
	{
        try
        {
			int index = MidiMapListBox.SelectedIndex;

			if ( index == -1 )
			{
				return;
			}

			var midiMap = DMS.SCORE.EditMidiMapSet.MidiMaps[ index ];

			AudioFactory.SinglePlay( DMS.SCORE.EditChannelNo, midiMap.Midi, (byte)Config.Media.MidiMaxVolume );
        }
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// キー変更確定
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void MidiMapListBox_MouseDoubleClick( object sender, MouseEventArgs ev )
	{
        try
        {
			int index = MidiMapListBox.SelectedIndex;

			if ( index == -1 )
			{
				return;
			}

			var midiMap = DMS.SCORE.EditMidiMapSet.MidiMaps[ index ];

			var midiMapName_o = DMS.SCORE.EditMidiMapSet.GetGroupAndMidiMapName( SelectKey );
			var midiMapName_n = DMS.SCORE.EditMidiMapSet.GetGroupAndMidiMapName( midiMap.MidiMapKey );

			if ( MessageBox.Show
					(
						this,
						$"Change OK ?\n"
							+ $"  before:  {SelectKey,-3}{midiMapName_o}\n"
							+ $"  after :  {midiMap.MidiMapKey,-3}{midiMapName_n}",
						"Key Change",
						MessageBoxButtons.OKCancel
					) != DialogResult.OK )
			{
				return;
			}

            SelectKey = midiMap.MidiMapKey;

            Hide();
        }
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}
}
