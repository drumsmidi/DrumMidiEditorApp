using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiLibrary.pLog;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class UserControlPlayerScoreType2ScaleItem : UserControl
{
    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayerScoreType2 DrawSet => Config.Player.ScoreType2SelectType;

    /// <summary>
    /// 編集中のScaleItemリスト
    /// </summary>
    private readonly ObservableCollection<ConfigPlayerScoreType2ScaleItem> _TmpScaleItemList = [];

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlPlayerScoreType2ScaleItem()
    {
        // 初期化
        InitializeComponent();

        // 初回表示時に読み込んでおく
        ReloadScaleItemList();
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// ScaleItemListの再読み込み
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadScaleItemList()
    {
        try
        {
            _TmpScaleItemList.Clear();

            DrawSet.ScaleList.ForEach( _TmpScaleItemList.Add );

            OnPropertyChanged( "_TmpScaleItemList" );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    #region ScaleKey

    /// <summary>
    /// 編集中のScaleItemを主データに反映
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ScaleItemApplyButton_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            List<ConfigPlayerScoreType2ScaleItem> itemList = [];

            foreach ( var item in _TmpScaleItemList )
            {
                itemList.Add( new( item ) );
            }

            DrawSet.UpdateScaleList( itemList );

            EventManage.Event_Player_ReloadScaleItem();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ScaleItem 追加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ScaleItemAddButton_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            _TmpScaleItemList.Add( new() );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ScaleItem 削除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ScaleItemRemoveButton_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            if ( _ScaleItemGridView.SelectedItem is not ConfigPlayerScoreType2ScaleItem item )
            {
                return;
            }

            _TmpScaleItemList.Remove( item );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ScaleItem のドラッグ開始
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ScaleItemGridView_DragItemsStarting( object sender, DragItemsStartingEventArgs args )
    {
        try
        {
            if ( args.Items.Count != 1 || args.Items [ 0 ] is not ConfigPlayerScoreType2ScaleItem item )
            {
                return;
            }

            args.Data.RequestedOperation = DataPackageOperation.Move;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ScaleItem のドラッグ終了
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ScaleItemGridView_DragItemsCompleted( ListViewBase sender, DragItemsCompletedEventArgs args )
    {
        try
        {
            switch ( args.DropResult )
            {
                case DataPackageOperation.Move:
                    {
                        if ( args.Items.Count != 1 || args.Items [ 0 ] is not ConfigPlayerScoreType2ScaleItem item )
                        {
                            return;
                        }
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
}
