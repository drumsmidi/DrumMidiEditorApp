using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.DataTransfer;

namespace DrumMidiEditorApp.pView.pMidiMap;

public sealed partial class PageImportMidiMap : Page
{
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
    /// コンストラクタ
    /// </summary>
    public PageImportMidiMap()
    {
        InitializeComponent();
    }

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
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
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
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ImportMidiMapGridView_DragItemsStarting( object sender, DragItemsStartingEventArgs args )
    {
        try
        {
            if ( args.Items [ 0 ] is not ImportMidiMapData data )
            {
                return;
            }

            args.Data.SetData( "midimapdata", data.AfterName );
            args.Data.RequestedOperation = DataPackageOperation.Copy;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ドロップ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private async void TextBlock_Drop( object sender, DragEventArgs args )
    {
        try
        {
            switch ( args.AcceptedOperation )
            {
                case DataPackageOperation.Copy:
                    {
                        var def = args.GetDeferral();

                        var obj = await args.Data.GetView().GetDataAsync( "midimapdata" );

                        var name = ( obj as string ) ?? string.Empty;

                        def.Complete();

                        if ( sender is not TextBlock textblock )
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
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ドラッグ範囲内に入った
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void TextBlock_DragEnter( object sender, DragEventArgs args )
    {
        try
        {
            args.AcceptedOperation = DataPackageOperation.Copy;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ドラッグ範囲外に外れた
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void TextBlock_DragLeave( object sender, DragEventArgs args )
    {
        try
        {
            args.AcceptedOperation = DataPackageOperation.None;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}

internal class ImportMidiMapData
{
    public string BeforeName { get; set; } = string.Empty;
    public string AfterName { get; set; } = string.Empty;
}
