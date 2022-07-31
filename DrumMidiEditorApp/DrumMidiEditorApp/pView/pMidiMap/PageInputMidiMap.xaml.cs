using Microsoft.UI.Xaml.Controls;
using System;

using DrumMidiEditorApp.pAudio;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pView.pMidiMap;

public sealed partial class PageInputMidiMap : Page
{
    #region Member

    /// <summary>
    /// System設定
    /// </summary>
    private ConfigSystem ConfigSystem => Config.System;

	/// <summary>
	/// Media設定
	/// </summary>
	private ConfigMedia ConfigMedia => Config.Media;

	/// <summary>
	/// Score情報
	/// </summary>
	private Score Score => DMS.SCORE;

	/// <summary>
	/// 選択したMidiMapキー
	/// </summary>
	public int SelectMidiMapKey { get; set; } = Config.System.MidiMapKeyNotSelect;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageInputMidiMap()
    {
		InitializeComponent();
	}

	/// <summary>
	/// MidiMapリスト作成
	/// </summary>
	public void LoadMidiMap()
    {
		foreach ( var group in Score.EditMidiMapSet.MidiMapGroups )
		{
			foreach ( var midiMap in group.MidiMaps )
			{
				var name = $"{midiMap.MidiMapKey,-3} {Score.EditMidiMapSet.GetGroupAndMidiMapName( midiMap.MidiMapKey )}";

				_MidiMapListBox.Items.Add( name );

				if ( midiMap.MidiMapKey == SelectMidiMapKey )
                {
					_MidiMapListBox.SelectedIndex	= _MidiMapListBox.Items.Count - 1;
					_MidiMapBeforeTextBlock.Text	= name;
				}
			}
		}
    }

	/// <summary>
	/// 選択変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void MidiMapListBox_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
        try
        {
			var index = _MidiMapListBox.SelectedIndex;

			if ( index == -1 )
			{
				SelectMidiMapKey = ConfigSystem.MidiMapKeyNotSelect;
				return;
			}

			var midiMap = Score.EditMidiMapSet.MidiMaps[ index ];

			SelectMidiMapKey = midiMap.MidiMapKey;

			AudioFactory.SinglePlay( Score.EditChannelNo, midiMap.Midi, ConfigMedia.MidiMaxVolume );
        }
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }
}
