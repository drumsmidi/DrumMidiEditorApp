using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pMidiMap;

public sealed partial class UserControlKeyChangePanel : UserControl, INotifyPropertyChanged
{
    #region Member

    /// <summary>
    /// System設定
    /// </summary>
    private ConfigSystem ConfigSystem => Config.System;

    /// <summary>
    /// Score情報
    /// </summary>
    private Score Score => DMS.SCORE;

    /// <summary>
    /// MidiMap別ノート数リスト
    /// </summary>
	private readonly ObservableCollection<string> _MidiMapNoteList = new();

	#endregion

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

	#region INotifyPropertyChanged

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
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged = delegate { };

	public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
		=> PropertyChanged?.Invoke( this, new( aPropertyName ) );

    #endregion

    /// <summary>
    /// 表示切替
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void DisplayButton_Click( object sender, RoutedEventArgs args )
    {
    	try
    	{
            if ( _HeadGrid.Visibility == Visibility.Visible )
            {
                _DisplayButton.Content = "<";
                _HeadGrid.Visibility = Visibility.Collapsed;
                _BodyGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                _DisplayButton.Content = ">";
                _HeadGrid.Visibility = Visibility.Visible;
                _BodyGrid.Visibility = Visibility.Visible;
            }
        }
        catch ( Exception e )
    	{
    		Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    	}
    }

    /// <summary>
    /// キー変更処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void KeyChangeButton_Click( object sender, RoutedEventArgs args )
    {
    	try
    	{
    		int index = _MidiMapNoteListBox.SelectedIndex;

    		if ( index == -1 )
    		{
    			return;
    		}

            var bef_key = Convert.ToInt32( _MidiMapNoteListBox.SelectedValue.ToString()?.Split(' ')[ 0 ] ?? String.Empty );

            var page = new PageInputMidiMap
            {
                SelectMidiMapKey = bef_key,
            };
            page.LoadMidiMap();

            XamlHelper.InputDialogOkCancelAsync
                (
                    Content.XamlRoot,
                    ResourcesHelper.GetString( "LabelKeyChange" ),
                    page,
                    () =>
                    {
    			        var aft_key = page.SelectMidiMapKey;

                        if ( bef_key == aft_key || aft_key == ConfigSystem.MidiMapKeyNotSelect )
    			        {
    				        return;
    			        }

			            var midiMapName_o = $"{bef_key,-3}{Score.EditMidiMapSet.GetGroupAndMidiMapName( bef_key )}";
			            var midiMapName_n = $"{aft_key,-3}{Score.EditMidiMapSet.GetGroupAndMidiMapName( aft_key )}";

			            XamlHelper.MessageDialogYesNoAsync
				            ( 
					            Content.XamlRoot,
					            ResourcesHelper.GetString( "DialogChangeKey/Title" ),
					            ResourcesHelper.GetString( "DialogChangeKey/Content", midiMapName_o, midiMapName_n ),
					            ResourcesHelper.GetString( "Dialog/Yes" ),
					            ResourcesHelper.GetString( "Dialog/No" ),
					            new( () =>
                                {
                                    lock ( Score.LockObj )
                                    { 
        			                    Score.EditChannel.KeyChange( bef_key, aft_key );
                                    }

   				                    EventManage.EventChangeMidiMapKey();
					            })
				            );
                    }
                );
    	}
        catch ( Exception e )
    	{
    		Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
    	}
    }
}
