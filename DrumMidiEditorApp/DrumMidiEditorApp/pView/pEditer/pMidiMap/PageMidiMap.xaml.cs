using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DrumMidiEditorApp.pView.pEditer.pMidiMap;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PageMidiMap : Page
{
    public PageMidiMap()
    {
        this.InitializeComponent();
    }


	private async void LaunchToolkitButton_Click(object sender, RoutedEventArgs e)
	{
		// Set the recommended app
		var options = new Windows.System.LauncherOptions
		{
			PreferredApplicationPackageFamilyName = "Microsoft.UWPCommunityToolkitSampleApp_8wekyb3d8bbwe",
			PreferredApplicationDisplayName = "Windows Community Toolkit"
		};

		await Windows.System.Launcher.LaunchUriAsync(new Uri("uwpct://controls?sample=datagrid"), options);
	}
}

#if false
/// <summary>
/// 編集中のMidiMapセット
/// </summary>
private MidiMapSet _TmpMidiMapSet = new();

/// <summary>
/// 入力前回値
/// </summary>
    private string _MidiMapSetPreValue = String.Empty;

/// <summary>
/// 選択中のMidiMapGroupインデックス
/// </summary>
private int _MidiMapGroupSelectIndex = Config.System.MidiMapGroupKeyNotSelect;

/// <summary>
/// MidiMapGroup削除後のセル変更イベント制御フラグ
/// </summary>
private bool _MidiMapGroupSelectForce = false;

/// <summary>
/// コンストラクタ
/// </summary>
public MidiMapSetControl()
{
	InitializeComponent();

        #region ToolTip

        ToolTip.SetToolTip( UpdateButton			, "Update MidiMapSet for this channel" );
	ToolTip.SetToolTip( ExportButton			, "Export MidiMapSet for this channel" );
	ToolTip.SetToolTip( ImportButton			, "Import MidiMapSet for this channel" );
	ToolTip.SetToolTip( MidiMapGroupAddButton	, "Add MidiMapGroup at the end" );
	ToolTip.SetToolTip( MidiMapGroupRemoveButton, "Delete selected MidiMapGroup" );
	ToolTip.SetToolTip( MidiMapGroupUpButton	, "Move selected MidiMapGroup up" );
	ToolTip.SetToolTip( MidiMapGroupDownButton	, "Move selected MidiMapGroup down" );
	ToolTip.SetToolTip( MidiMapAddButton		, "Add MidiMap at the end" );
	ToolTip.SetToolTip( MidiMapRemoveButton		, "Delete selected MidiMap" );
	ToolTip.SetToolTip( MidiMapUpButton			, "Move selected MidiMap up" );
	ToolTip.SetToolTip( MidiMapDownButton		, "Move selected MidiMap down" );
	ToolTip.SetToolTip( KeyChangeButton			, "Change MidiMapKey" );

	#endregion
}

#region MidiMapSet

/// <summary>
/// MidiMapセットの情報をGridViewへ反映
/// </summary>
/// <param name="aMidiMapSet">入力MidiMapセット</param>
public void LoadMidiMapSet( MidiMapSet aMidiMapSet )
    {
	SuspendLayout();

        try
        {
		_TmpMidiMapSet.Dispose();
		_TmpMidiMapSet = aMidiMapSet.Clone();

		_MidiMapGroupSelectIndex = Config.System.MidiMapGroupKeyNotSelect;
		_MidiMapGroupSelectForce = false;

		MidiMapGroupDataGridView.Rows.Clear();
		MidiMapDataGridView.Rows.Clear();

		foreach ( var group in _TmpMidiMapSet.MidiMapGroups )
		{
			var row = new DataGridViewRow();
			row.CreateCells( MidiMapGroupDataGridView );
			row.Cells[ MidiMapGroupDisplayColumn.Index   ].Value = group.Display;
			row.Cells[ MidiMapGroupKeyColumn.Index       ].Value = group.GroupKey;
			row.Cells[ MidiMapGroupNameColumn.Index      ].Value = group.GroupName;
			row.Cells[ MidiMapGroupVolumeColumn.Index    ].Value = group.VolumeAdd.ToString();

			MidiMapGroupDataGridView.Rows.Add( row );
		}

		UpdateButton.Visible = false;

		LoadSource();
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}

	ResumeLayout( false );
}

/// <summary>
/// キー変更用に、MidiMap別にノート入力数をカウントしリストを作成
/// </summary>
public void LoadSource()
{
	SuspendLayout();

        try
	{
		SourceValueListBox.Items.Clear();

		var items = DMS.SCORE.EditChannel.GetNumberOfNoteUsingMidiMap();

		foreach ( var key in DMS.SCORE.EditMidiMapSet.MidiMapKeys )
            {
			if ( !items.TryGetValue( key, out var cnt ) )
                {
				continue;
                }

			var MidiMapName = DMS.SCORE.EditMidiMapSet.GetGroupAndMidiMapName( key );

			SourceValueListBox.Items.Add
				(
					$"{key,-3} {MidiMapName,-30}[{cnt,4}]"
				);
            }

		items.Clear();
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}

	ResumeLayout( false );
}

/// <summary>
/// MidiMapGroup/MidiMapの編集内容を元にMidiMapSetを作成
/// </summary>
/// <returns>MidiMapSet</returns>
private MidiMapSet CreateMidiMapSet()
    {
	UpdateSelectMidiMapGroup( MidiMapGroupDataGridView.SelectedCells[ 0 ].RowIndex );
		
	var midiMapSet = _TmpMidiMapSet.Clone();
	DMS.SCORE.EditMidiMapSet.CopyToMidiMapGroupPosition( ref midiMapSet );

	return midiMapSet;
    }

/// <summary>
/// 指定のMidiMapグループとその配下のMidiMap入力情報を一時保存
/// </summary>
/// <param name="aGroupIndex">MidiMapGroupインデックス番号</param>
private void UpdateSelectMidiMapGroup( int aGroupIndex )
{
	if ( aGroupIndex == Config.System.MidiMapGroupKeyNotSelect )
	{
		return;
	}

	#region Save the MidiMap settings
	{
		_TmpMidiMapSet.RemoveMidiMaps( aGroupIndex );

		foreach ( DataGridViewRow row in MidiMapDataGridView.Rows )
		{
                var midiMap = new MidiMap
                {
                    Display		= Convert.ToBoolean		( row.Cells[ MidiMapDisplayColumn.Index	].Value.ToString() ),
                    MidiMapKey	= Convert.ToInt32		( row.Cells[ MidiMapKeyColumn.Index		].Value.ToString() ),
                    MidiMapName	= (string)				  row.Cells[ MidiMapNameColumn.Index	].Value ?? String.Empty,
                    VolumeAdd	= Convert.ToInt32		( row.Cells[ MidiMapVolumeColumn.Index	].Value.ToString() ),
                    //Color		=						  row.Cells[ MidiMapColorColumn.Index	].Style.BackColor,
                    Midi		= (byte)Convert.ToInt32	( row.Cells[ MidiMapMidiColumn.Index	].Value.ToString() ),
                    Scale		= (string)				  row.Cells[ MidiMapScaleColumn.Index	].Value ?? String.Empty,
			};

                _TmpMidiMapSet.AddMidiMap( aGroupIndex, midiMap );
		}
	}
	#endregion

	#region Save MidiMap group settings
	{
		var row = MidiMapGroupDataGridView.Rows[ aGroupIndex ];

		var group		= _TmpMidiMapSet.MidiMapGroups[ aGroupIndex ];
		group.Display	= Convert.ToBoolean	( row.Cells[ MidiMapGroupDisplayColumn.Index	].Value.ToString()	);
		group.GroupKey	= Convert.ToInt32	( row.Cells[ MidiMapGroupKeyColumn.Index		].Value.ToString()	);
		group.GroupName	= (string)			  row.Cells[ MidiMapGroupNameColumn.Index		].Value ?? String.Empty;
		group.VolumeAdd	= Convert.ToInt32	( row.Cells[ MidiMapGroupVolumeColumn.Index		].Value.ToString()	);

		_TmpMidiMapSet.MidiMapGroups[ aGroupIndex ] = group;
	}
	#endregion

	_TmpMidiMapSet.UpdateInfo();
}

/// <summary>
/// GridViewへ指定のMidiMapグループ配下のMidiMap情報を設定
/// </summary>
/// <param name="aGroupIndex">MidiMapGroupインデックス番号</param>
private void UpdateDisplayMidiMapGroup( int aGroupIndex )
{
	if ( aGroupIndex == Config.System.MidiMapGroupKeyNotSelect )
	{
		return;
	}

	if (   aGroupIndex == Config.System.MidiMapGroupKeyNotSelect
		|| aGroupIndex >= _TmpMidiMapSet.MidiMapGroups.Count )
	{
		return;
	}

	SuspendLayout();
	{ 
		MidiMapDataGridView.Rows.Clear();

		var group = _TmpMidiMapSet.MidiMapGroups[ aGroupIndex ];

		foreach ( var midiMap in group.MidiMaps )
		{
			var row = new DataGridViewRow();
			row.CreateCells( MidiMapDataGridView );
			row.Cells[ MidiMapDisplayColumn.Index	].Value				= midiMap.Display;
			row.Cells[ MidiMapKeyColumn.Index		].Value				= midiMap.MidiMapKey.ToString();
			row.Cells[ MidiMapNameColumn.Index		].Value				= midiMap.MidiMapName;
			row.Cells[ MidiMapVolumeColumn.Index	].Value				= midiMap.VolumeAdd.ToString();
			//row.Cells[ MidiMapColorColumn.Index		].Style.BackColor	= midiMap.Color;
			row.Cells[ MidiMapMidiColumn.Index		].Value				= midiMap.Midi.ToString();
			row.Cells[ MidiMapScaleColumn.Index		].Value				= midiMap.Scale;

			MidiMapDataGridView.Rows.Add( row );
		}
	}
	ResumeLayout( false );
}

/// <summary>
/// MidiMapSetを正本に反映
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void UpdateButton_Click( object sender, EventArgs ev )
{
        try
        {
		DMS.SCORE.EditMidiMapSet = CreateMidiMapSet();
		DMS.SCORE.EditMidiMapSet.ClearSelect();
		DMS.SCORE.EditMidiMapSet.UpdateInfo();

		Config.EventReloadMidiMapSet();

		UpdateButton.Visible = false;
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// MidiMapSetテンプレートの保存
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void ExportButton_Click( object sender, EventArgs ev )
{
        try
        {
		//DMS.PlayerForm?.TemporaryHide();

		//if ( !FormUtil.SaveShowDialog( Config.System.FolderMidiMapSet, new(), Config.System.SupportMidiMapSet, out var filepath ) )
   //         {
		//	return;
   //         }

		//var midiMapSet = CreateMidiMapSet();

		//FileIO.SaveMidiMapSet( filepath, midiMapSet );
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
	finally
        {
		//DMS.PlayerForm?.TemporaryShow();
	}
}

/// <summary>
/// MidiMapSetテンプレートのインポート。
/// インポート実施後、MidiMapSetを正本に反映。
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void ImportButton_Click( object sender, EventArgs ev )
{
        try
        {
		//DMS.PlayerForm?.TemporaryHide();

		//if ( !FormUtil.OpenShowDialog( Config.System.FolderMidiMapSet, Config.System.SupportMidiMapSet, out var filepath ) )
   //         {
		//	return;
   //         }

		//if ( !FileIO.LoadMidiMapSet( filepath, out var midiMapSet ) )
   //         {
		//	return;
   //         }

   //         using var fm = new ImportMidiMapSetForm
   //         {
   //             Owner = ParentForm
   //         };

   //         if ( fm.Display( midiMapSet ) )
   //         {
		//	DMS.SCORE.EditMidiMapSet = midiMapSet;
		//	DMS.SCORE.EditMidiMapSet.UpdateInfo();

		//	LoadMidiMapSet( midiMapSet );
   //             LoadSource();

		//	Config.EventReloadMidiMapSet();

		//	UpdateButton.Visible = false;
		//}
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
	finally
        {
		//DMS.PlayerForm?.TemporaryShow();
	}
}

#endregion

#region MidiMapGroup

/// <summary>
/// 最後尾に新規MidiMapGroup追加
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapGroupAddButton_Click( object sender, EventArgs ev )
{
        try
        {
		int new_key = _TmpMidiMapSet.GetMidiMapGroupNewKey();

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

		SuspendLayout();
		{ 
			var row = new DataGridViewRow();
			row.CreateCells( MidiMapGroupDataGridView );
			row.Cells[ MidiMapGroupDisplayColumn.Index	].Value	= group.Display;
			row.Cells[ MidiMapGroupKeyColumn.Index		].Value	= group.GroupKey.ToString();
			row.Cells[ MidiMapGroupNameColumn.Index		].Value	= group.GroupName;
			row.Cells[ MidiMapGroupVolumeColumn.Index	].Value	= group.VolumeAdd.ToString();

			MidiMapGroupDataGridView.Rows.Add( row );

			UpdateButton.Visible = true;
		}
		ResumeLayout( false );
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// 選択したMidiMapGroup削除
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapGroupRemoveButton_Click( object sender, EventArgs ev )
{
        try
        {
            if ( MidiMapGroupDataGridView.SelectedCells.Count == 0 )
            {
                return;
            }

            int col_index = MidiMapGroupDataGridView.SelectedCells[ 0 ].ColumnIndex;
            int row_index = MidiMapGroupDataGridView.SelectedCells[ 0 ].RowIndex;
		int key       = Convert.ToInt32( MidiMapGroupDataGridView.Rows[ row_index ].Cells[ MidiMapGroupKeyColumn.Index ].Value.ToString() );

		_TmpMidiMapSet.RemoveMidiMapGroup( row_index );
		_TmpMidiMapSet.UpdateInfo();

		SuspendLayout();
		{ 
			MidiMapDataGridView.Rows.Clear();
			MidiMapGroupDataGridView.Rows.RemoveAt( row_index );

			int cnt = MidiMapGroupDataGridView.Rows.Count;

			_MidiMapGroupSelectIndex = ( row_index >= cnt ) ? cnt - 1 : row_index ;
			_MidiMapGroupSelectForce = true;

			UpdateDisplayMidiMapGroup( _MidiMapGroupSelectIndex );

			if ( _MidiMapGroupSelectIndex != -1 )
			{
				MidiMapGroupDataGridView.Rows[ _MidiMapGroupSelectIndex ].Cells[ col_index ].Selected = true;
			}

			UpdateButton.Visible = true;
		}
		ResumeLayout( false );
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// 選択したMidiMapGroupの位置を一つ上げる
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapGroupUpButton_Click( object sender, EventArgs ev )
{
        try
        {
            if ( MidiMapGroupDataGridView.SelectedCells.Count == 0 )
            {
                return;
            }

            int col_index = MidiMapGroupDataGridView.SelectedCells[ 0 ].ColumnIndex;
            int row_index = MidiMapGroupDataGridView.SelectedCells[ 0 ].RowIndex;

            if ( row_index <= 0 )
            {
                return;
            }

		_MidiMapGroupSelectIndex = MidiMapGroupMoveRow( row_index, -1 );

		SuspendLayout();
		{ 
			MidiMapGroupDataGridView.Rows[ _MidiMapGroupSelectIndex ].Cells[ col_index ].Selected = true;

			UpdateButton.Visible = true;
		}
		ResumeLayout( false );
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// 選択したMidiMapGroupの位置を１つ下げる
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapGroupDownButton_Click( object sender, EventArgs ev )
{
	try
        {
            if ( MidiMapGroupDataGridView.SelectedCells.Count == 0 )
            {
                return;
            }

            int col_index = MidiMapGroupDataGridView.SelectedCells[ 0 ].ColumnIndex;
            int row_index = MidiMapGroupDataGridView.SelectedCells[ 0 ].RowIndex;

            if ( row_index >= MidiMapGroupDataGridView.Rows.Count - 1 )
            {
                return;
            }

            _MidiMapGroupSelectIndex = MidiMapGroupMoveRow( row_index, 1 );

		SuspendLayout();
		{ 
            MidiMapGroupDataGridView.Rows[ _MidiMapGroupSelectIndex ].Cells[ col_index ].Selected = true;

			UpdateButton.Visible = true;
		}
		ResumeLayout( false );
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// MidiMapGroup位置の移動処理
/// </summary>
/// <param name="aBeforeIndex">移動前のインデックス番号</param>
/// <param name="aMove">移動量</param>
/// <returns>移動後のMidiMapGroupインデックス番号</returns>
private int MidiMapGroupMoveRow( int aBeforeIndex, int aMove )
    {
        int bef = aBeforeIndex;
        int aft	= aBeforeIndex + aMove;
	int cnt = MidiMapGroupDataGridView.Rows.Count;

	if ( aft < 0 )
	{
		aft = 0;
	}
	if ( aft > cnt - 1 )
	{
		aft = cnt - 1;
	}

	if ( aft != bef )
	{
		SuspendLayout();
		{ 
			var row = MidiMapGroupDataGridView.Rows[ bef ];

                // Removeイベント後のSelectionChangedイベントでエラーが発生する為、回避
#pragma warning disable CS8622
                MidiMapGroupDataGridView.SelectionChanged -= new EventHandler( MidiMapGroupDataGridView_SelectionChanged );
                MidiMapGroupDataGridView.Rows.Remove( row );
			MidiMapGroupDataGridView.SelectionChanged += new EventHandler( MidiMapGroupDataGridView_SelectionChanged );
#pragma warning restore CS8622

			MidiMapGroupDataGridView.Rows.Insert( aft, row );

			_TmpMidiMapSet.MoveMidiMapGroup( aBeforeIndex, aMove );
			_TmpMidiMapSet.UpdateInfo();
		}
		ResumeLayout( false );
	}

	return aft;
    }

/// <summary>
/// MidiMapGroupのセル選択時に、MidiMapの一覧表示を更新
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapGroupDataGridView_SelectionChanged( object sender, EventArgs ev )
{
	try
	{
		if ( MidiMapGroupDataGridView.SelectedCells.Count == 0 )
		{
			MidiMapDataGridView.Rows.Clear();
			return;
		}

		int row_index = MidiMapGroupDataGridView.SelectedCells[ 0 ].RowIndex;

		if ( row_index == _MidiMapGroupSelectIndex && !_MidiMapGroupSelectForce )
		{
			return;
		}
		_MidiMapGroupSelectForce = false;

		// Save the value before change
		UpdateSelectMidiMapGroup( _MidiMapGroupSelectIndex );

		// Set the changed value
		_MidiMapGroupSelectIndex = row_index;

		UpdateDisplayMidiMapGroup( row_index );
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// MidiMapGroupセル選択
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapGroupDataGridView_CellEnter( object sender, DataGridViewCellEventArgs ev )
{
	try
	{
		if  ( MidiMapGroupDataGridView[ ev.ColumnIndex, ev.RowIndex ].GetType().Equals( typeof( DataGridViewComboBoxCell ) ) )
		{
			MidiMapGroupDataGridView.BeginEdit( false );

			( (DataGridViewComboBoxEditingControl)MidiMapGroupDataGridView.EditingControl ).DroppedDown = true;
		}
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// MidiMapGroupセル内のコンテンツクリック
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapGroupDataGridView_CellContentClick( object sender, DataGridViewCellEventArgs ev )
{
	try
	{
		if ( ev.RowIndex == -1 )
		{
			return;
		}

		if  ( ev.ColumnIndex == MidiMapGroupDisplayColumn.Index )
		{
			UpdateButton.Visible = true;
		}
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// テキスト入力前の値を保持
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapGroupDataGridView_CellValidating( object sender, DataGridViewCellValidatingEventArgs ev )
{
        try
        {
		if ( ev.RowIndex == -1 )
		{
			return;
		}

		if	(	ev.ColumnIndex == MidiMapGroupNameColumn.Index
			||	ev.ColumnIndex == MidiMapGroupVolumeColumn.Index
			)
		{
			_MidiMapSetPreValue = (string)MidiMapGroupDataGridView[ ev.ColumnIndex, ev.RowIndex ].Value ?? String.Empty ;
		}
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// テキスト入力後の検証
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapGroupDataGridView_CellValidated( object sender, DataGridViewCellEventArgs ev )
{
	bool rollback = false;

        try
        {
		if ( ev.RowIndex == -1 )
		{
			return;
		}

		if	(	ev.ColumnIndex == MidiMapGroupNameColumn.Index
			||	ev.ColumnIndex == MidiMapGroupVolumeColumn.Index
			)
		{
			var value = (string)MidiMapGroupDataGridView[ ev.ColumnIndex, ev.RowIndex ].Value ?? String.Empty ;

			if ( _MidiMapSetPreValue.Equals( value ) )
			{
				return;
			}

			#region Check

			if ( ev.ColumnIndex == MidiMapGroupVolumeColumn.Index )
			{		                
				if ( !int.TryParse( value, out var volume ) )
				{
					rollback = true;
				}
				else if ( volume < Config.Media.MidiAddMinVolume || Config.Media.MidiAddMaxVolume < volume )
				{
					rollback = true;
				}
			}

			#endregion

			SuspendLayout();
			{ 
				if ( rollback )
				{
					MidiMapGroupDataGridView[ ev.ColumnIndex, ev.RowIndex ].Value = _MidiMapSetPreValue;
				}
				else
				{
					UpdateButton.Visible = true;
				}
			}
			ResumeLayout(false);
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
/// 最後尾に新規MidiMap追加
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapAddButton_Click( object sender, EventArgs ev )
    {
	try
        {
		if ( _MidiMapGroupSelectIndex == -1 )
		{
			return;
		}

		int new_key = _TmpMidiMapSet.GetMidiMapNewKey();

		if ( new_key == Config.System.MidiMapKeyNotSelect )
		{
			return;
		}

            var MidiMap = new MidiMap
            {
                MidiMapKey = new_key
            };
		_TmpMidiMapSet.AddMidiMap( _MidiMapGroupSelectIndex, MidiMap );
		_TmpMidiMapSet.UpdateInfo();

            SuspendLayout();
		{ 
			var row = new DataGridViewRow();
			row.CreateCells( MidiMapDataGridView );
			row.Cells[ MidiMapDisplayColumn.Index	].Value						= MidiMap.Display;
			row.Cells[ MidiMapKeyColumn.Index		].Value						= MidiMap.MidiMapKey.ToString();
			row.Cells[ MidiMapNameColumn.Index		].Value						= MidiMap.MidiMapName;
			row.Cells[ MidiMapVolumeColumn.Index	].Value						= MidiMap.VolumeAdd.ToString();
			//row.Cells[ MidiMapColorColumn.Index		].Style.BackColor			= MidiMap.Color;
			//row.Cells[ MidiMapColorColumn.Index		].Style.SelectionBackColor	= MidiMap.Color;
			row.Cells[ MidiMapMidiColumn.Index		].Value						= MidiMap.Midi.ToString();
			row.Cells[ MidiMapScaleColumn.Index		].Value						= MidiMap.Scale;

			MidiMapDataGridView.Rows.Add( row );

			UpdateButton.Visible = true;
		}
		ResumeLayout( false );
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

/// <summary>
/// 選択したMidiMap削除
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapRemoveButton_Click( object sender, EventArgs ev )
    {
        try
        {
            if ( MidiMapDataGridView.SelectedCells.Count == 0 )
            {
                return;
            }

            int MidiMap_index	= MidiMapDataGridView.SelectedCells[ 0 ].RowIndex;
		int MidiMap_key		= Convert.ToInt32( MidiMapDataGridView.Rows[ MidiMap_index ].Cells[ MidiMapKeyColumn.Index ].Value.ToString() );

		_TmpMidiMapSet.RemoveMidiMap( _MidiMapGroupSelectIndex, MidiMap_index );
		_TmpMidiMapSet.UpdateInfo();

		SuspendLayout();
		{ 
            MidiMapDataGridView.Rows.RemoveAt( MidiMap_index );

			UpdateButton.Visible = true;
		}
		ResumeLayout( false );
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

/// <summary>
/// 選択したMidiMapの位置を一つ上げる
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapUpButton_Click( object sender, EventArgs ev )
    {
        try
        {
            if ( MidiMapDataGridView.SelectedCells.Count == 0 )
            {
                return;
            }

            int col_index = MidiMapDataGridView.SelectedCells[ 0 ].ColumnIndex;
            int row_index = MidiMapDataGridView.SelectedCells[ 0 ].RowIndex;

            if ( row_index <= 0 )
            {
                return;
            }

		int aft_index = MidiMapMoveRow( row_index, -1 );

		SuspendLayout();
		{ 
            MidiMapDataGridView.Rows[ aft_index ].Cells[ col_index ].Selected = true;

			UpdateButton.Visible = true;
		}
		ResumeLayout( false );
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

/// <summary>
/// 選択したMidiMapの位置を１つ下げる
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapDownButton_Click( object sender, EventArgs ev )
    {
	try
        {
            if ( MidiMapDataGridView.SelectedCells.Count == 0 )
            {
                return;
            }

            int col_index = MidiMapDataGridView.SelectedCells[ 0 ].ColumnIndex;
            int row_index = MidiMapDataGridView.SelectedCells[ 0 ].RowIndex;

            if ( row_index >= MidiMapDataGridView.Rows.Count - 1 )
            {
                return;
            }

		int aft_index = MidiMapMoveRow( row_index, 1 );

		SuspendLayout();
		{ 
            MidiMapDataGridView.Rows[ aft_index ].Cells[ col_index ].Selected = true;

			UpdateButton.Visible = true;
		}
		ResumeLayout( false );
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

/// <summary>
/// MidiMap位置の移動処理
/// </summary>
/// <param name="aBeforeIndex">移動前のインデックス番号</param>
/// <param name="aMove">移動量</param>
/// <returns>移動後のドラムインデックス番号</returns>
private int MidiMapMoveRow( int aBeforeIndex, int aMove )
    {
        var bef = aBeforeIndex;
        var aft	= aBeforeIndex + aMove;
	var cnt = MidiMapDataGridView.Rows.Count;

	if ( aft < 0 )
	{
		aft = 0;
	}
	if ( aft > cnt - 1 )
	{
		aft = cnt - 1;
	}

	if ( aft != bef )
	{
		var row = MidiMapDataGridView.Rows[ bef ];

		SuspendLayout();
		{ 
			MidiMapDataGridView.Rows.Remove( row );
			MidiMapDataGridView.Rows.Insert( aft, row );

			_TmpMidiMapSet.MoveMidiMap( _MidiMapGroupSelectIndex, aBeforeIndex, aMove );
			_TmpMidiMapSet.UpdateInfo();
		}
		ResumeLayout( false );
	}

	return aft;
    }

/// <summary>
/// MidiMapセル選択
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapDataGridView_CellEnter( object sender, DataGridViewCellEventArgs ev )
{
	try
	{
		if  ( MidiMapDataGridView[ ev.ColumnIndex, ev.RowIndex ].GetType().Equals( typeof( DataGridViewComboBoxCell ) ) )
		{
			MidiMapDataGridView.BeginEdit( false );
			( (DataGridViewComboBoxEditingControl)MidiMapDataGridView.EditingControl ).DroppedDown = true;
		}
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// MidiMapセル内のコンテンツクリック
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MidiMapDataGridView_CellContentClick( object sender, DataGridViewCellEventArgs ev )
    {
        try
        {
		if ( ev.RowIndex == -1 )
		{
			return;
		}

		if ( ev.ColumnIndex == MidiMapDisplayColumn.Index )
            {
			UpdateButton.Visible = true;
		}
		else if ( ev.ColumnIndex == MidiMapColorColumn.Index )
		{
			var cell = MidiMapDataGridView.Rows[ ev.RowIndex ].Cells[ MidiMapColorColumn.Index ];

			try
			{ 
				//DMS.PlayerForm?.TemporaryHide();

				if ( FormUtil.SelectColor( cell.Style.SelectionBackColor, out var c ) )
				{
					cell.Style.BackColor			= c;
					cell.Style.SelectionBackColor	= c;

					UpdateButton.Visible = true;
				}
			}
			finally
                {
				//DMS.PlayerForm?.TemporaryShow();
			}
		}
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// テキスト入力前の値を保持
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
    private void MidiMapDataGridView_CellValidating( object sender, DataGridViewCellValidatingEventArgs ev )
    {
        try
        {
		if ( ev.RowIndex == -1 )
		{
			return;
		}

		if  (	ev.ColumnIndex == MidiMapNameColumn.Index
			||	ev.ColumnIndex == MidiMapVolumeColumn.Index
			||	ev.ColumnIndex == MidiMapMidiColumn.Index
			)
		{
			_MidiMapSetPreValue = (string)MidiMapDataGridView[ ev.ColumnIndex, ev.RowIndex ].Value ?? String.Empty ;
		}
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

/// <summary>
/// テキスト入力後の検証
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
    private void MidiMapDataGridView_CellValidated( object sender, DataGridViewCellEventArgs ev )
    {
	bool rollback = false;

        try
        {
		if ( ev.RowIndex == -1 )
		{
			return;
		}

		if  (	ev.ColumnIndex == MidiMapNameColumn.Index
			||	ev.ColumnIndex == MidiMapVolumeColumn.Index
			||	ev.ColumnIndex == MidiMapMidiColumn.Index
			)
		{
			var value = (string)MidiMapDataGridView[ ev.ColumnIndex, ev.RowIndex ].Value ?? String.Empty ;

			if ( _MidiMapSetPreValue.Equals( value ) )
			{
				return;
			}

			#region Check

			if ( ev.ColumnIndex == MidiMapVolumeColumn.Index )
			{
				if ( !int.TryParse( value, out var volume ) )
				{
					rollback = true;
				}
				else if ( volume < Config.Media.MidiAddMinVolume || Config.Media.MidiAddMaxVolume < volume )
				{
					rollback = true;
				}
			}
			else if ( ev.ColumnIndex == MidiMapMidiColumn.Index )
			{	                
				if ( !int.TryParse( value, out var midi ) )
				{
					rollback = true;
				}
				else if ( midi < Config.Media.MidiNoteMin || midi > Config.Media.MidiNoteMax )
				{
					rollback = true;
				}
			}

			#endregion

			SuspendLayout();
			{
				if ( rollback )
				{
					MidiMapDataGridView[ ev.ColumnIndex, ev.RowIndex ].Value = _MidiMapSetPreValue;
				}
				else
				{
					UpdateButton.Visible = true;
				}
			}
			ResumeLayout( false );
		}
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

    #endregion

    #region KeyChange

/// <summary>
/// キー変更処理
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
    private void KeyChangeButton_Click( object sender, EventArgs ev )
{
        try
        {
		//DMS.PlayerForm?.TemporaryHide();

		int index = SourceValueListBox.SelectedIndex;

		if ( index == -1 )
		{
			return;
		}

            using( var fm = new MidiMapListForm
			{
				Owner = ParentForm
			})
            {
			int bef_key = Convert.ToInt32( SourceValueListBox.Items[ index ].ToString()?.Split(' ')[ 0 ] ?? String.Empty );
			int aft_key = fm.ShowDialog( bef_key );

			if ( bef_key == aft_key )
			{
				return;
			}

			DMS.SCORE.EditChannel.KeyChange( bef_key, aft_key );

                Config.EventChangeMidiMapKey();
		}

		LoadSource();
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
	finally
        {
		//DMS.PlayerForm?.TemporaryShow();
	}
}

    #endregion
#endif
