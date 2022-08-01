using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pIO;

namespace DrumMidiEditorApp.pView.pMidiMap;

public sealed partial class UserControlMidiMapPanel : UserControl, INotifyPropertyChanged
{
    #region Member

    /// <summary>
    /// Player設定
    /// </summary>
    private ConfigPlayer DrawSet => Config.Player;

    /// <summary>
    /// System設定
    /// </summary>
    private ConfigSystem ConfigSystem => Config.System;

    /// <summary>
    /// Scale設定
    /// </summary>
    private ConfigScale ConfigScale => Config.Scale;

    /// <summary>
    /// Media設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// Score情報
    /// </summary>
    private Score Score => DMS.SCORE;

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
    private readonly ObservableCollection<MidiMapGroup> _TmpMidiMapGroupList = new();

    /// <summary>
    /// 編集中のMidiMapリスト
    /// </summary>
	private readonly ObservableCollection<MidiMap> _TmpMidiMapList = new();

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlMidiMapPanel()
    {
        InitializeComponent();

		ControlAccess.UCMidiMapPanel = this;
	}

	#region INotifyPropertyChanged

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
            _MidiMapGroupSelectIndex = _TmpMidiMapSet.MidiMapGroups.Count > 0 ? 0 : -1 ;

            ReloadMidiMapGroup();
        }
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
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
            // GridViewのItemSources データ作成
            _TmpMidiMapGroupList.Clear();

            _TmpMidiMapSet.MidiMapGroups
                .ForEach( midiMap => _TmpMidiMapGroupList.Add( midiMap ) );

            OnPropertyChanged( "_TmpMidiMapGroupList" );
            OnPropertyChanged( "_MidiMapGroupSelectIndex" );
        }
        catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
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

            if (   _MidiMapGroupSelectIndex != -1
    		    && _MidiMapGroupSelectIndex < _TmpMidiMapSet.MidiMapGroups.Count )
    	    {
                _TmpMidiMapSet.MidiMapGroups[ _MidiMapGroupSelectIndex ].MidiMaps
                    .ForEach( midiMap => _TmpMidiMapList.Add( midiMap ) );
    	    }

            OnPropertyChanged( "_TmpMidiMapList" );
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged = delegate { };

	public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
		=> PropertyChanged?.Invoke( this, new( aPropertyName ) );

    #endregion

    #region Command

    /// <summary>
    /// 編集中のMidiMapSetを主データに反映
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ApplyButton_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            Score.EditMidiMapSet = CreateMidiMapSet();
            Score.EditMidiMapSet.ClearSelect();
            Score.EditMidiMapSet.UpdateInfo();

            Config.EventReloadMidiMapSet();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// インポート
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ImportButton_Click( object sender, RoutedEventArgs args )
    {
        try
        {
			XamlHelper.OpenDialogAsync
				(
					ControlAccess.MainWindow,
					ConfigSystem.SupportMidiMapSet,
					PickerLocationId.DocumentsLibrary,
					ConfigSystem.FolderMidiMapSet,
					( filepath ) =>
                    {
                        if ( !FileIO.LoadMidiMapSet( filepath, out var midiMapSet ) )
                        {
            	            return;
                        }

                        var page = new PageImportMidiMap();
                        page.LoadMidiMap( midiMapSet );

                        XamlHelper.InputDialogOkCancelAsync
                            (
                                Content.XamlRoot,
                                ResourcesHelper.GetString( "LabelImportMidiMap" ),
                                page,
                                () =>
                                {
                                    Score.EditMidiMapSet = midiMapSet;
                                    Score.EditMidiMapSet.UpdateInfo();

                                    Config.EventReloadMidiMapSet();
                                }
                            );
					}
				);
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// エクスポート
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ExportButton_Click( object sender, RoutedEventArgs args )
    {
        try
        {
			XamlHelper.SaveDialogAsync
				(
					ControlAccess.MainWindow,
					ConfigSystem.SupportMidiMapSet,
					"",
					PickerLocationId.DocumentsLibrary,
					ConfigSystem.FolderMidiMapSet,
					( filepath ) =>
                    {
						filepath.Extension = ConfigSystem.ExtentionDms;

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
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
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
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiMapGroupGridView_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
        try
        {
            ReloadMidiMap();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MidiMapGroup追加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiMapGroupAddButton_Click( object sender, RoutedEventArgs args )
    {
        try
        {
    		int new_key = _TmpMidiMapSet.GetMidiMapGroupNewKey();

    		if ( new_key == ConfigSystem.MidiMapGroupKeyNotSelect )
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
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MidiMapGroup削除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiMapGroupRemoveButton_Click( object sender, RoutedEventArgs args )
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
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MidiMapGroup のドラッグ開始
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiMapGroupGridView_DragItemsStarting( object sender, DragItemsStartingEventArgs args )
    {
		try
		{
            if ( args.Items.Count != 1 )
            {
                return;
            }

            if ( args.Items[ 0 ] is not MidiMapGroup group )
            {
                return;
            }

            _BeforeMoveIndex = _TmpMidiMapGroupList.IndexOf( group );

            args.Data.RequestedOperation = DataPackageOperation.Move;
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MidiMapGroup のドラッグ終了
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiMapGroupGridView_DragItemsCompleted( ListViewBase sender, DragItemsCompletedEventArgs args )
    {
		try
		{
            switch ( args.DropResult )
            {
                case DataPackageOperation.Move:
                    {
                        if ( args.Items.Count != 1 )
                        {
                            return;
                        }

                        if ( args.Items[ 0 ] is not MidiMapGroup group )
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
                    }
                    break;
            }
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    #region MidiMap

    /// <summary>
    /// MidiMap追加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiMapAddButton_Click( object sender, RoutedEventArgs args )
    {
        try
        {
    		if ( _MidiMapGroupSelectIndex == -1 )
    		{
    			return;
    		}

    		int new_key = _TmpMidiMapSet.GetMidiMapNewKey();

    		if ( new_key == ConfigSystem.MidiMapKeyNotSelect )
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
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MidiMap削除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiMapRemoveButton_Click( object sender, RoutedEventArgs args )
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
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MidiMap ドラッグ開始
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiMapGridView_DragItemsStarting( object sender, DragItemsStartingEventArgs args )
    {
		try
		{
            if ( args.Items.Count != 1 )
            {
                return;
            }

            if ( args.Items[ 0 ] is not MidiMap midiMap )
            {
                return;
            }

            _BeforeMoveIndex = _TmpMidiMapList.IndexOf( midiMap );

            args.Data.RequestedOperation = DataPackageOperation.Move;
        }
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MidiMap ドラッグ終了
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiMapGridView_DragItemsCompleted( ListViewBase sender, DragItemsCompletedEventArgs args )
    {
		try
		{
            switch ( args.DropResult )
            {
                case DataPackageOperation.Move:
                    {
                        if ( args.Items.Count != 1 )
                        {
                            return;
                        }

                        if ( args.Items[ 0 ] is not MidiMap midiMap )
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
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    #region GridItem イベント

    /// <summary>
    /// ボリューム増減入力チェック
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void VolumeAddNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

            sender.Value = ConfigMedia.CheckMidiAddVolume( (int)sender.Value );
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MIDI入力チェック
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

            sender.Value = ConfigMedia.CheckMidiNote( (int)sender.Value );
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// 色選択
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ColorButton_Click( object sender, RoutedEventArgs args )
    {
		try
		{
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
