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
    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public UserControlPlayerScoreType2ScaleItem()
    {
        InitializeComponent();

        // ����\�����ɓǂݍ���ł���
        ReloadScaleItemList();
    }

    #region Member

    /// <summary>
    /// �`��ݒ�
    /// </summary>
    private ConfigPlayerScoreType2 DrawSet => Config.Player.ScoreType2;

    /// <summary>
    /// �ҏW����ScaleItem���X�g
    /// </summary>
    private readonly ObservableCollection<ConfigPlayerScoreType2ScaleItem> _TmpScaleItemList = [];

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// ScaleItemList�̍ēǂݍ���
    /// x:Bind OneWay/TwoWay �ēǂݍ���
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
            Log.Error( e );
        }
    }

    #endregion

    #region ScaleKey

    /// <summary>
    /// �ҏW����ScaleItem����f�[�^�ɔ��f
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ScaleItemApplyButton_Click( object aSender, RoutedEventArgs aArgs )
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
            Log.Error( e );
        }
    }

    /// <summary>
    /// ScaleItem �ǉ�
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ScaleItemAddButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            _TmpScaleItemList.Add( new() );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ScaleItem �폜
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ScaleItemRemoveButton_Click( object aSender, RoutedEventArgs aArgs )
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
            Log.Error( e );
        }
    }

    /// <summary>
    /// ScaleItem �̃h���b�O�J�n
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ScaleItemGridView_DragItemsStarting( object aSender, DragItemsStartingEventArgs aArgs )
    {
        try
        {
            if ( aArgs.Items.Count != 1 || aArgs.Items [ 0 ] is not ConfigPlayerScoreType2ScaleItem item )
            {
                return;
            }

            aArgs.Data.RequestedOperation = DataPackageOperation.Move;
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ScaleItem �̃h���b�O�I��
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ScaleItemGridView_DragItemsCompleted( ListViewBase aSender, DragItemsCompletedEventArgs aArgs )
    {
        try
        {
            switch ( aArgs.DropResult )
            {
                case DataPackageOperation.Move:
                    {
                        if ( aArgs.Items.Count != 1 || aArgs.Items [ 0 ] is not ConfigPlayerScoreType2ScaleItem item )
                        {
                            return;
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
}
