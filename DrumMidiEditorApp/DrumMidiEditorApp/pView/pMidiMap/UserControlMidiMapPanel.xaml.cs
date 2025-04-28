using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pIO;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

namespace DrumMidiEditorApp.pView.pMidiMap;

public sealed partial class UserControlMidiMapPanel : UserControl, INotifyPropertyChanged
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlMidiMapPanel()
    {
        InitializeComponent();

        ControlAccess.UCMidiMapPanel = this;

        // 初回表示時に読み込んでおく
        ReloadMidiMapSet();
    }

    #region Member

    /// <summary>
    /// Score情報
    /// </summary>
    private static Score Score => DMS.SCORE;

    /// <summary>
    /// 編集中のMidiMapセット
    /// </summary>
    private MidiMapSet _TmpMidiMapSet = new();

    /// <summary>
    /// 選択中のMidiMapGroupインデックス
    /// </summary>
    private int _MidiMapGroupSelectIndex = -1;

    /// <summary>
    /// GridView 移動前のインデックス位置
    /// </summary>
    private int _BeforeMoveIndex = -1;

    /// <summary>
    /// 編集中のMidiMapGroupリスト
    /// </summary>
    private readonly ObservableCollection<MidiMapGroup> _TmpMidiMapGroupList = [];

    /// <summary>
    /// 編集中のMidiMapリスト
    /// </summary>
	private readonly ObservableCollection<MidiMap> _TmpMidiMapList = [];

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// MidiMapSetの再読み込み
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadMidiMapSet()
    {
        try
        {
            // 正本のMidiMapSetの情報を一時データとしてコピー
            _TmpMidiMapSet.Dispose();
            _TmpMidiMapSet = Score.EditMidiMapSet.Clone();

            // 先頭のMidiMapGroupを選択
            _MidiMapGroupSelectIndex = _TmpMidiMapSet.MidiMapGroups.Count > 0 ? 0 : -1;

            ReloadMidiMapGroup();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MidiMapGroupの再読み込み
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadMidiMapGroup()
    {
        try
        {
            // 再読み込み前に先頭の行を選択していた場合、通知が無視されるので
            // 未選択状態でリセットしておく
            var tmp = _MidiMapGroupSelectIndex;

            _MidiMapGroupSelectIndex = -1;
            OnPropertyChanged( "_MidiMapGroupSelectIndex" );

            // GridViewのItemSources データ作成
            _TmpMidiMapGroupList.Clear();

            _TmpMidiMapSet.MidiMapGroups
                .ForEach( _TmpMidiMapGroupList.Add );

            OnPropertyChanged( "_TmpMidiMapGroupList" );

            _MidiMapGroupSelectIndex = tmp;
            OnPropertyChanged( "_MidiMapGroupSelectIndex" );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MidiMapの再読み込み
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadMidiMap()
    {
        try
        {
            _TmpMidiMapList.Clear();

            if ( _MidiMapGroupSelectIndex != -1
                && _MidiMapGroupSelectIndex < _TmpMidiMapSet.MidiMapGroups.Count )
            {
                _TmpMidiMapSet.MidiMapGroups [ _MidiMapGroupSelectIndex ].MidiMaps
                    .ForEach( _TmpMidiMapList.Add );
            }

            OnPropertyChanged( "_TmpMidiMapList" );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    #region Command

    /// <summary>
    /// 編集中のMidiMapSetを主データに反映
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ApplyButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            lock ( Score.LockObj )
            {
                Score.EditMidiMapSet = CreateMidiMapSet();
                Score.EditMidiMapSet.ClearSelect();
                Score.EditMidiMapSet.UpdateInfo();
            }

            EventManage.Event_Score_ReloadMidiMapSet();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// インポート
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ImportButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

        try
        {
            HelperXaml.OpenDialogAsync
                (
                    ControlAccess.MainWindow,
                    Config.File.SupportMidiMapSet,
                    PickerLocationId.DocumentsLibrary,
                    ConfigFile.FolderMidiMapSet,
                    ( filepath ) =>
                    {
                        if ( !FileIO.LoadMidiMapSet( filepath, out var midiMapSet ) )
                        {
                            return;
                        }

                        var page = new PageImportMidiMap();
                        page.LoadMidiMap( midiMapSet );

                        HelperXaml.InputDialogOkCancelAsync
                            (
                                Content.XamlRoot,
                                HelperResources.GetString( "LabelImportMidiMap" ),
                                page,
                                () =>
                                {
                                    var keyChangeDic = page.GetChangeKeys();

                                    lock ( Score.LockObj )
                                    {
                                        foreach ( var item in keyChangeDic )
                                        {
                                            Score.EditChannel.KeyChange( item.Key, -item.Value );
                                        }

                                        foreach ( var item in keyChangeDic )
                                        {
                                            Score.EditChannel.KeyChange( -item.Value, item.Value );
                                        }

                                        Score.EditMidiMapSet = midiMapSet;
                                        Score.EditMidiMapSet.UpdateInfo();
                                    }

                                    EventManage.Event_Score_ReloadMidiMapSet();
                                }
                            );
                    }
                );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// エクスポート
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ExportButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

        try
        {
            HelperXaml.SaveDialogAsync
                (
                    ControlAccess.MainWindow,
                    Config.File.SupportMidiMapSet,
                    string.Empty,
                    PickerLocationId.DocumentsLibrary,
                    ConfigFile.FolderMidiMapSet,
                    ( filepath ) =>
                        {
                            filepath.Extension = ConfigFile.ExtentionDms;

                            var midiMapSet = CreateMidiMapSet();

                            if ( !FileIO.SaveMidiMapSet( filepath, midiMapSet ) )
                            {
                                return;
                            }
                        }
                );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    #region MidiMapSet

    /// <summary>
    /// MidiMapGroup/MidiMapの編集内容を元にMidiMapSetを作成
    /// </summary>
    /// <returns>MidiMapSet</returns>
    private MidiMapSet CreateMidiMapSet()
    {
        var midiMapSet = _TmpMidiMapSet.Clone();

        Score.EditMidiMapSet.CopyToMidiMapGroupPosition( ref midiMapSet );

        return midiMapSet;
    }

    #endregion

    #region MidiMapGroup

    /// <summary>
    /// MidiMapGroupのアイテム変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiMapGroupGridView_SelectionChanged( object aSender, SelectionChangedEventArgs aArgs )
    {
        try
        {
            ReloadMidiMap();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MidiMapGroup追加
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiMapGroupAddButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            var new_key = _TmpMidiMapSet.GetMidiMapGroupNewKey();

            if ( new_key == Config.System.MidiMapGroupKeyNotSelect )
            {
                return;
            }

            var group = new MidiMapGroup
            {
                GroupKey = new_key
            };
            _TmpMidiMapSet.AddMidiMapGroup( group );
            _TmpMidiMapSet.UpdateInfo();

            // 新規追加したアイテムを選択
            _MidiMapGroupSelectIndex = _TmpMidiMapSet.MidiMapGroups.Count - 1;

            ReloadMidiMapGroup();

            if ( _TmpMidiMapList.Count == 0 )
            {
                MidiMapAddButton_Click( aSender, aArgs );
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MidiMapGroup削除
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiMapGroupRemoveButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( _MidiMapGroupSelectIndex == -1 )
            {
                return;
            }

            _TmpMidiMapSet.RemoveMidiMapGroup( _MidiMapGroupSelectIndex );
            _TmpMidiMapSet.UpdateInfo();

            // 同じ行にあるアイテムを選択
            if ( _TmpMidiMapSet.MidiMapGroups.Count == 0 )
            {
                _MidiMapGroupSelectIndex = -1;
            }
            else if ( _TmpMidiMapSet.MidiMapGroups.Count <= _MidiMapGroupSelectIndex )
            {
                _MidiMapGroupSelectIndex = _TmpMidiMapSet.MidiMapGroups.Count - 1;
            }

            ReloadMidiMapGroup();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MidiMapGroup のドラッグ開始
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiMapGroupGridView_DragItemsStarting( object aSender, DragItemsStartingEventArgs aArgs )
    {
        try
        {
            if ( aArgs.Items.Count != 1 )
            {
                return;
            }

            if ( aArgs.Items [ 0 ] is MidiMapGroup group )
            {
                _BeforeMoveIndex = _TmpMidiMapGroupList.IndexOf( group );

                aArgs.Data.RequestedOperation = DataPackageOperation.Move;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MidiMapGroup のドラッグ終了
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiMapGroupGridView_DragItemsCompleted( ListViewBase aSender, DragItemsCompletedEventArgs aArgs )
    {
        try
        {
            switch ( aArgs.DropResult )
            {
                case DataPackageOperation.Move:
                    {
                        if ( aArgs.Items.Count != 1 )
                        {
                            return;
                        }

                        if ( aArgs.Items [ 0 ] is not MidiMapGroup group )
                        {
                            return;
                        }

                        var afterMoveIndex = _TmpMidiMapGroupList.IndexOf( group );

                        if ( afterMoveIndex == -1 )
                        {
                            return;
                        }

                        _TmpMidiMapSet.MoveMidiMapGroup( _BeforeMoveIndex, afterMoveIndex - _BeforeMoveIndex );
                        _TmpMidiMapSet.UpdateInfo();

                        // 選択中のアイテムをドラッグ＆ドロップすると
                        // SelectionChanged が先に呼ばれ
                        // 移動前の情報が読み込まれてしまうので
                        // ここでも再読み込みしておく
                        if ( _MidiMapGroupSelectIndex == afterMoveIndex )
                        {
                            ReloadMidiMap();
                        }
                    }
                    break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    #region MidiMap

    /// <summary>
    /// MidiMap追加
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiMapAddButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( _MidiMapGroupSelectIndex == -1 )
            {
                return;
            }

            var new_key = _TmpMidiMapSet.GetMidiMapNewKey();

            if ( new_key == Config.System.MidiMapKeyNotSelect )
            {
                return;
            }

            var midiMap = new MidiMap
            {
                MidiMapKey = new_key
            };
            _TmpMidiMapSet.AddMidiMap( _MidiMapGroupSelectIndex, midiMap );
            _TmpMidiMapSet.UpdateInfo();

            ReloadMidiMap();

            // 新規追加したアイテムを選択
            _MidiMapGridView.SelectedIndex = _MidiMapGridView.Items.Count - 1;
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MidiMap削除
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiMapRemoveButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( _MidiMapGroupSelectIndex == -1 || _MidiMapGridView.SelectedIndex == -1 )
            {
                return;
            }

            var index = _MidiMapGridView.SelectedIndex;

            _TmpMidiMapSet.RemoveMidiMap( _MidiMapGroupSelectIndex, index );
            _TmpMidiMapSet.UpdateInfo();

            ReloadMidiMap();

            // 同じ行にあるアイテムを選択
            if ( _MidiMapGridView.Items.Count == 0 )
            {
                MidiMapAddButton_Click( aSender, aArgs );
                return;
            }
            else if ( _MidiMapGridView.Items.Count <= index )
            {
                index = _MidiMapGridView.Items.Count - 1;
            }

            _MidiMapGridView.SelectedIndex = index;
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MidiMap ドラッグ開始
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiMapGridView_DragItemsStarting( object aSender, DragItemsStartingEventArgs aArgs )
    {
        try
        {
            if ( aArgs.Items.Count != 1 )
            {
                return;
            }

            if ( aArgs.Items [ 0 ] is MidiMap midiMap )
            {
                _BeforeMoveIndex = _TmpMidiMapList.IndexOf( midiMap );

                aArgs.Data.RequestedOperation = DataPackageOperation.Move;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MidiMap ドラッグ終了
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiMapGridView_DragItemsCompleted( ListViewBase aSender, DragItemsCompletedEventArgs aArgs )
    {
        try
        {
            switch ( aArgs.DropResult )
            {
                case DataPackageOperation.Move:
                    {
                        if ( aArgs.Items.Count != 1 )
                        {
                            return;
                        }

                        if ( aArgs.Items [ 0 ] is not MidiMap midiMap )
                        {
                            return;
                        }

                        var afterMoveIndex = _TmpMidiMapList.IndexOf( midiMap );

                        if ( afterMoveIndex == -1 )
                        {
                            return;
                        }

                        _TmpMidiMapSet.MoveMidiMap( _MidiMapGroupSelectIndex, _BeforeMoveIndex, afterMoveIndex - _BeforeMoveIndex );
                        _TmpMidiMapSet.UpdateInfo();
                    }
                    break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    #region GridItem イベント

    /// <summary>
    /// ボリューム増減入力チェック
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void VolumeAddNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            aSender.Value = Config.Media.CheckMidiAddVolume( (int)aArgs.NewValue );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MIDI入力チェック
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            aSender.Value = MidiNet.CheckMidiNote( (int)aArgs.NewValue );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// 色選択
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ColorButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( aSender is Button item )
            {
                HelperXaml.ColorDialog
                    (
                        item,
                        ( item.Background as SolidColorBrush )?.Color ?? Config.System.DefaultMidiMapColor,
                        ( color ) =>
                        {
                            item.Background = new SolidColorBrush( color );
                        }
                    );
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion
}
