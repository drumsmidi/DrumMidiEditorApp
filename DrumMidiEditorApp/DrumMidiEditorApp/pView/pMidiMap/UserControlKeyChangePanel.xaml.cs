using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pMidiMap;

public sealed partial class UserControlKeyChangePanel : UserControl, INotifyPropertyChanged
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlKeyChangePanel()
    {
        InitializeComponent();

        ControlAccess.UCKeyChangePanel = this;

        // 初回表示時に読み込んでおく
        ReloadMidiMapNoteList();
    }

    #region Member

    /// <summary>
    /// Score情報
    /// </summary>
    private static Score Score => DMS.SCORE;

    /// <summary>
    /// MidiMap別ノート数リスト
    /// </summary>
	private readonly ObservableCollection<string> _MidiMapNoteList = [];

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// MidiMap別ノートリスト再読み込み
    /// 
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadMidiMapNoteList()
    {
        try
        {
            _MidiMapNoteList.Clear();

            var items = Score.EditChannel.GetNumberOfNoteUsingMidiMap();

            foreach ( var key in Score.EditMidiMapSet.MidiMapKeys )
            {
                if ( !items.TryGetValue( key, out var cnt ) )
                {
                    continue;
                }

                var MidiMapName = Score.EditMidiMapSet.GetGroupAndMidiMapName( key );

                _MidiMapNoteList.Add( $"{key,-3} {MidiMapName,-30} [{cnt,4}]" );
            }

            items.Clear();

            OnPropertyChanged( "_MidiMapNoteList" );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    /// <summary>
    /// 表示切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void DisplayButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( _HeadGrid.Visibility == Visibility.Visible )
            {
                _DisplayButton.Content  = "<";
                _HeadGrid.Visibility    = Visibility.Collapsed;
                _BodyGrid.Visibility    = Visibility.Collapsed;
            }
            else
            {
                _DisplayButton.Content  = ">";
                _HeadGrid.Visibility    = Visibility.Visible;
                _BodyGrid.Visibility    = Visibility.Visible;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// キー変更処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void KeyChangeButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            var index = _MidiMapNoteListBox.SelectedIndex;

            if ( index == -1 )
            {
                return;
            }

            var bef_key = Convert.ToInt32( _MidiMapNoteListBox.SelectedValue.ToString()?.Split(' ')[ 0 ] ?? string.Empty );

            var page = new PageInputMidiMap
            {
                SelectMidiMapKey = bef_key,
            };
            page.LoadMidiMap();

            HelperXaml.InputDialogOkCancelAsync
                (
                    Content.XamlRoot,
                    HelperResources.GetString( "LabelKeyChange" ),
                    page,
                    () =>
                    {
                        var aft_key = page.SelectMidiMapKey;

                        if ( bef_key == aft_key || aft_key == Config.System.MidiMapKeyNotSelect )
                        {
                            return;
                        }

                        var midiMapName_o = $"{bef_key,-3}{Score.EditMidiMapSet.GetGroupAndMidiMapName( bef_key )}";
                        var midiMapName_n = $"{aft_key,-3}{Score.EditMidiMapSet.GetGroupAndMidiMapName( aft_key )}";

                        HelperXaml.MessageDialogYesNoAsync
                            (
                                Content.XamlRoot,
                                HelperResources.GetString( "DialogChangeKey/Title" ),
                                HelperResources.GetString( "DialogChangeKey/Content", midiMapName_o, midiMapName_n ),
                                HelperResources.GetString( "Dialog/Yes" ),
                                HelperResources.GetString( "Dialog/No" ),
                                new( () =>
                                {
                                    lock ( Score.LockObj )
                                    {
                                        Score.EditChannel.KeyChange( bef_key, aft_key );
                                    }

                                    EventManage.Event_Score_ChangeMidiMapKey();
                                } )
                            );
                    }
                );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}
