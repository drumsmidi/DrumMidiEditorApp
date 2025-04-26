using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace DrumMidiEditorApp.pView.pMidiMap;

public sealed partial class PageImportMidiMap : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageImportMidiMap()
    {
        InitializeComponent();
    }

    #region Member

    /// <summary>
    /// Score情報
    /// </summary>
    private Score Score => DMS.SCORE;

    /// <summary>
    /// 既存のMidiMapSetで、ノート登録があるMidiMapリスト
    /// </summary>
    private readonly ObservableCollection<ImportMidiMapData> _ChangeKeyDataList = [];

    /// <summary>
    /// インポート対象のMidiMapSet
    /// </summary>
    private readonly ObservableCollection<ImportMidiMapData> _ImportDataList = [];

    #endregion

    /// <summary>
    /// MidiMapリスト読込
    /// </summary>
    public void LoadMidiMap( MidiMapSet aMidiMapSet )
    {
        try
        {
            #region 既存のMidiMapSetで、ノート登録があるMidiMap
            {
                var items = Score.EditChannel.GetNumberOfNoteUsingMidiMap();

                foreach ( var midiMap in Score.EditMidiMapSet.MidiMaps )
                {
                    if ( !items.TryGetValue( midiMap.MidiMapKey, out var cnt ) )
                    {
                        continue;
                    }

                    var before = $"{midiMap.MidiMapKey,-3} {Score.EditMidiMapSet.GetGroupAndMidiMapName( midiMap.MidiMapKey ),-30} [{cnt,4}]";
                    var after  = string.Empty;

                    var midiMapKey_new = aMidiMapSet.GetMidiMapKeyForMatchMidi( midiMap.Midi );

                    if ( midiMapKey_new != Config.System.MidiMapKeyNotSelect )
                    {
                        after = $"{midiMapKey_new,-3} {aMidiMapSet.GetGroupAndMidiMapName( midiMapKey_new ),-30}";
                    }

                    _ChangeKeyDataList.Add
                        (
                            new()
                            {
                                BeforeName  = before,
                                AfterName   = after,
                            }
                        );
                }

                items.Clear();
            }
            #endregion

            #region インポート対象のMidiMapSet
            {
                foreach ( var group in aMidiMapSet.MidiMapGroups )
                {
                    foreach ( var midiMap in group.MidiMaps )
                    {
                        var name = aMidiMapSet.GetGroupAndMidiMapName( midiMap.MidiMapKey );

                        _ImportDataList.Add
                            (
                                new()
                                {
                                    AfterName = $"{midiMap.MidiMapKey,-3} {name}",
                                }
                            );
                    }
                }
            }
            #endregion
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// キー変換リスト
    /// </summary>
    /// <returns>変換前MidiMapKey, 変換後MidiMapKey</returns>
    public Dictionary<int, int> GetChangeKeys()
    {
        var changeKeyDic = new Dictionary<int, int>();

        foreach ( var item in _ChangeKeyDataList )
        {
            var midiMapKeyBef = item.BeforeName.Split( ' ' )[ 0 ];
            var midiMapKeyAft = item.AfterName.Split( ' ' )[ 0 ];

            if ( int.TryParse( midiMapKeyAft, out _ ) )
            {
                changeKeyDic.Add( Convert.ToInt32( midiMapKeyBef ), Convert.ToInt32( midiMapKeyAft ) );
            }
        }
        return changeKeyDic;
    }

    /// <summary>
    /// ドラッグ
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ImportMidiMapGridView_DragItemsStarting( object aSender, DragItemsStartingEventArgs aArgs )
    {
        try
        {
            if ( aArgs.Items [ 0 ] is ImportMidiMapData data )
            {
                aArgs.Data.SetData( "midimapdata", data.AfterName );
                aArgs.Data.RequestedOperation = DataPackageOperation.Copy;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ドロップ
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private async void TextBlock_Drop( object aSender, DragEventArgs aArgs )
    {
        try
        {
            switch ( aArgs.AcceptedOperation )
            {
                case DataPackageOperation.Copy:
                    {
                        var def = aArgs.GetDeferral();

                        var obj = await aArgs.Data.GetView().GetDataAsync( "midimapdata" );

                        var name = ( obj as string ) ?? string.Empty;

                        def.Complete();

                        if ( aSender is not TextBlock textblock )
                        {
                            return;
                        }

                        textblock.Text = name;
                    }
                    break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ドラッグ範囲内に入った
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void TextBlock_DragEnter( object aSender, DragEventArgs aArgs )
    {
        try
        {
            aArgs.AcceptedOperation = DataPackageOperation.Copy;
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ドラッグ範囲外に外れた
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void TextBlock_DragLeave( object aSender, DragEventArgs aArgs )
    {
        try
        {
            aArgs.AcceptedOperation = DataPackageOperation.None;
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}

internal class ImportMidiMapData
{
    public string BeforeName { get; set; } = string.Empty;
    public string AfterName { get; set; } = string.Empty;
}
