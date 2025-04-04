﻿using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pMidiMap;

public sealed partial class PageInputMidiMap : Page
{
    #region Member

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
                    _MidiMapListBox.SelectedIndex = _MidiMapListBox.Items.Count - 1;
                    _MidiMapBeforeTextBlock.Text  = name;
                }
            }
        }
    }

    /// <summary>
    /// 選択変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>" )]
    private void MidiMapListBox_SelectionChanged( object aSender, SelectionChangedEventArgs aArgs )
    {
        try
        {
            var index = _MidiMapListBox.SelectedIndex;

            if ( index == -1 )
            {
                SelectMidiMapKey = Config.System.MidiMapKeyNotSelect;
                return;
            }

            var midiMap = Score.EditMidiMapSet.MidiMaps[ index ];

            SelectMidiMapKey = midiMap.MidiMapKey;

            AudioFactory.SinglePlay( Score.EditChannelNo, midiMap.Midi, MidiNet.MidiMaxVolume );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
