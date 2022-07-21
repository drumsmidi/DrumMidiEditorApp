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
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DrumMidiEditorApp.pView.pEditer.pEdit;

public sealed partial class PageEdit : Page
{
    /// <summary>
    /// Editerタブ設定
    /// </summary>
    private ConfigEditer DrawSet => Config.Editer;

    /// <summary>
    /// System設定
    /// </summary>
    private ConfigSystem ConfSystem => Config.System;

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

	private ObservableCollection<CollectionItem> _MeasureNoList = new();

	public PageEdit()
    {
        InitializeComponent();

		int keta = ConfSystem.MeasureMaxNumber.ToString().Length;

		for ( int measure_no = 0; measure_no <= ConfSystem.MeasureMaxNumber; measure_no++ )
		{
			_MeasureNoList.Add( new( measure_no.ToString().PadLeft( keta, '0' ) ) );
		}


    }

    private void _MeasureNoGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void _MeasureNoGridView_ItemClick(object sender, ItemClickEventArgs e)
    {

    }
}


#if false
/// <summary>
/// シート移動タイマー
/// </summary>
private readonly Timer _MoveTimer = new();

/// <summary>
/// シート移動 マウス開始位置
/// </summary>
    private Point _MoveSheetPos = new();

/// <summary>
/// Editerタブ設定
/// </summary>
private static ConfigEditer DrawSet => Config.Editer;

/// <summary>
/// コンストラクタ
/// </summary>
public EditerControl()
{
	InitializeComponent();

	InitUpdate();

#region ToolTip

	ToolTip.SetToolTip( MeasureNoListBox		, "Move measure position" );
	ToolTip.SetToolTip( NoteHeightNumericUpDown	, "Note height" );
	ToolTip.SetToolTip( NoteWidthNumericUpDown	, "Note width" );
        ToolTip.SetToolTip( VolumeGroupBox			, "Input volume" );
        ToolTip.SetToolTip( EditVolumeListBox		, "Volume edit line" );

        ToolTip.SetToolTip( WaveFormGroupBox		, "Under consideration" );
        ToolTip.SetToolTip( VolumeLowNumericUpDown	, "Low db" );
        ToolTip.SetToolTip( VolumeMidNumericUpDown	, "Mid db" );
        ToolTip.SetToolTip( VolumeHighNumericUpDown	, "High db" );
        ToolTip.SetToolTip( VolumeTopNumericUpDown	, "Top db" );
        ToolTip.SetToolTip( ScaleSensitivityTrackBar, "Sensitivity" );

	ToolTip.SetToolTip( MoveSheetPictureBox		, "Move sheet position" );

	ToolTip.SetToolTip( NoteOnCheckBox			, "Input note on/off" );
	ToolTip.SetToolTip( DivisionLineListBox		, "Input note. Number of divisions within a measure." );
	ToolTip.SetToolTip( SelectRangeListBox		, "Selection type" );
	ToolTip.SetToolTip( ClearRangeCheckBox		, "Cancel selection" );
	ToolTip.SetToolTip( BpmCheckBox				, "Include BPM in selection" );

	ToolTip.SetToolTip( RedoButton				, "Redo" );
	ToolTip.SetToolTip( UndoButton				, "Undu" );

#endregion

	_MoveTimer.Tick		+= new EventHandler( ProcMoveSheet );
	_MoveTimer.Interval	 = 100;
	_MoveTimer.Enabled	 = false;
}

/// <summary>
/// 初期設定
/// </summary>
    private void InitUpdate()
    {
	SuspendLayout();
        {
			
            var ss = DrawSet;

            // Note
            //NoteWidthNumericUpDown.Value	= ss.NoteWidthSize;                
            //NoteHeightNumericUpDown.Value	= ss.NoteHeightSize;

		// Volume
		Volume100RadioButton.Checked = true;

		// Edit
		FormUtil.SelectListBoxValue( DivisionLineListBox, ss.SheetDivisionLine.ToString() );

		EditVolumeListBox.SelectedIndex		= (int)DrawSet.VolumeEditSelect;
		SelectRangeListBox.SelectedIndex	= (int)DrawSet.RangeSelect;
		NoteOnCheckBox.Checked				= DrawSet.NoteOn;

		// Vertical scroll bar
		EditVScrollBar.Value	= 0;
		EditVScrollBar.Minimum	= 0;

		// Horizontal scroll bar
		EditHScrollBar.Value	= 0;
		EditHScrollBar.Minimum	= 0;

		UpdateScrollBar();

		SetVolumeLevel
			(
				Config.Scale.VolumeLevelTop,
				Config.Scale.VolumeLevelHigh,
				Config.Scale.VolumeLevelMid,
				Config.Scale.VolumeLevelLow
			);

		ScaleSensitivityTrackBar.Value = (int)( Config.Scale.SensitivityLevel * ScaleSensitivityTrackBar.Maximum );

	}
	ResumeLayout( false );
    }

/// <summary>
/// EditerPanel描画更新
/// </summary>
public void RefreshScreen() => EditerPanel.Invalidate( true );

#region SheetMove

/// <summary>
/// シート縦スクロール
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void EditVScrollBar_Scroll( object sender, ScrollEventArgs ev )
{
        try
        {
		MoveSheetDiff( 0, ev.NewValue - ev.OldValue );
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// シート横スクロール
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void EditHScrollBar_Scroll( object sender, ScrollEventArgs ev )
{
        try
        {
		MoveSheetDiff( ev.NewValue - ev.OldValue,	0 );
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// 選択した小節番号へシート位置移動
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MeasureNoListBox_SelectedIndexChanged( object sender, EventArgs ev )
{
	try
	{
		if ( MeasureNoListBox.SelectedIndex == -1 )
		{
			return;
		}

		JumpMeasure( MeasureNoListBox.SelectedIndex );
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// シートマウス移動開始
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MoveSheetPictureBox_MouseDown( object sender, MouseEventArgs ev )
{
        try
        {
		if ( !_MoveTimer.Enabled )
		{
			_MoveSheetPos		= Cursor.Position;
			_MoveTimer.Enabled	= true;
		}
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// シートマウス移動終了
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void MoveSheetPictureBox_MouseUp( object sender, MouseEventArgs ev )
{
        try
        {
        _MoveTimer.Enabled = false;
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// シートマウス移動
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void ProcMoveSheet( object? sender, EventArgs ev )
    {
        try
        {
		SuspendLayout();

		if ( _MoveTimer.Enabled )
		{
    //            MoveSheetDiff
    //                (
    //                    ( Cursor.Position.X - _MoveSheetPos.X ) / DrawSet.NoteWidthSize
    //                ,   ( Cursor.Position.Y - _MoveSheetPos.Y ) / DrawSet.NoteHeightSize
    //                );

			//EditHScrollBar.Value = DrawSet.NotePosition.X;
			//EditVScrollBar.Value = DrawSet.NotePosition.Y;
		}
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
	finally
        {
		ResumeLayout( false );
	}
}

/// <summary>
/// シート移動処理
/// </summary>
/// <param name="aDiffX">移動ノート数X座標</param>
/// <param name="aDiffY">移動ノート数Y座標</param>
private void MoveSheetDiff( int aDiffX, int aDiffY )
    {
	if ( aDiffX == 0 && aDiffY == 0 )
	{
		return;
	}

	EditVScrollBar.Maximum = DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount - 1 >= 0 
		? DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount - 1 : 0;

	//DrawSet.NotePosition = FormUtil.CheckRangeIn
	//	( 
	//		new
	//		(
	//			DrawSet.NotePosition.X + aDiffX,
	//			DrawSet.NotePosition.Y + aDiffY
	//		),
	//		new
	//		(
	//			EditHScrollBar.Minimum,
	//			EditVScrollBar.Minimum,
	//			EditHScrollBar.Maximum,
	//			EditVScrollBar.Maximum
	//		)
	//	);

	Config.EventUpdateEditerSheetPos();

	RefreshScreen();
}

/// <summary>
/// シート位置　指定の小節番号へ移動
/// </summary>
/// <param name="aMeasureNo">小節番号</param>
private void JumpMeasure( int aMeasureNo )
    {
	//DrawSet.NotePosition = new Point( aMeasureNo * Config.System.MeasureNoteNumber, DrawSet.NotePosition.Y );

	Config.EventUpdateEditerSheetPos();

	RefreshScreen();
    }

#endregion

#region Note

/// <summary>
/// ノート間隔：横　変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void NoteWidthNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
	try
	{
		DrawSet.NoteWidthSize = (int)NoteWidthNumericUpDown.Value;

		Config.EventUpdateEditerSize();

		RefreshScreen();
	}
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

/// <summary>
/// ノート間隔：縦　変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void NoteHeightNumericUpDown_ValueChanged( object sender, EventArgs ev )
{
	try
	{
		DrawSet.NoteHeightSize = (int)NoteHeightNumericUpDown.Value;

		Config.EventUpdateEditerSize();

		RefreshScreen();
	}
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
}

#endregion

#region Volume

/// <summary>
/// 音量入力変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void VolumeNumericUpDown_ValueChanged( object sender, EventArgs ev )
{
	try
	{
		if ( VolumeRadioButton.Checked )
		{ 
			DrawSet.NoteSelectVolume = (int)VolumeNumericUpDown.Value;
		}
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// 音量入力変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void VolumeRadioButton_CheckedChanged( object sender, EventArgs ev )
{
	try
	{
		int volume = 0;

		if		( Volume127RadioButton.Checked	)	{ volume = 127; }
		else if ( Volume125RadioButton.Checked	)	{ volume = 125; }
		else if ( Volume120RadioButton.Checked	)	{ volume = 120; }
		else if ( Volume115RadioButton.Checked	)	{ volume = 115; }
		else if ( Volume110RadioButton.Checked	)	{ volume = 110; }
		else if ( Volume105RadioButton.Checked	)	{ volume = 105; }
		else if ( Volume100RadioButton.Checked	)	{ volume = 100; }
		else if ( Volume95RadioButton.Checked	)	{ volume =  95; }
		else if ( Volume90RadioButton.Checked	)	{ volume =  90; }
		else if ( Volume85RadioButton.Checked	)	{ volume =  85; }
		else if ( Volume80RadioButton.Checked	)	{ volume =  80; }
		else if ( Volume75RadioButton.Checked	)	{ volume =  75; }
		else if ( Volume70RadioButton.Checked	)	{ volume =  70; }
		else if ( Volume65RadioButton.Checked	)	{ volume =  65; }
		else if ( Volume60RadioButton.Checked	)	{ volume =  60; }
		else if ( Volume55RadioButton.Checked	)	{ volume =  55; }
		else if ( Volume50RadioButton.Checked	)	{ volume =  50; }
		else if ( Volume45RadioButton.Checked	)	{ volume =  45; }
		else if ( Volume40RadioButton.Checked	)	{ volume =  40; }
		else if ( Volume35RadioButton.Checked	)	{ volume =  35; }
		else if ( Volume30RadioButton.Checked	)	{ volume =  30; }
		else if ( Volume25RadioButton.Checked	)	{ volume =  25; }
		else if ( Volume20RadioButton.Checked	)	{ volume =  20; }
		else if ( Volume15RadioButton.Checked	)	{ volume =  15; }
		else if ( Volume10RadioButton.Checked	)	{ volume =  10; }
		else if ( Volume5RadioButton.Checked	)	{ volume =   5; }
		else if ( Volume0RadioButton.Checked	)	{ volume =   0; }
		else if ( VolumeRadioButton.Checked		)   { volume = (int)VolumeNumericUpDown.Value; }

		DrawSet.NoteSelectVolume = volume;
        }
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

#endregion

#region Resume

/// <summary>
/// Undo実行
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
    private void UndoButton_Click( object sender, EventArgs ev )
    {
	try
	{
            Config.EventEditerUndo();

		RefreshScreen();
	}
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

/// <summary>
/// Redo実行
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void RedoButton_Click( object sender, EventArgs ev )
    {
	try
	{
		Config.EventEditerRedo();

		RefreshScreen();
	}
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

#endregion

/// <summary>
/// スクロールバー最大値更新
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
    private void EditerControl_Enter( object sender, EventArgs ev )
    {
	try
	{
		UpdateScrollBar();
	}
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

/// <summary>
/// スクロールバー最大値更新
/// </summary>
    private void UpdateScrollBar()
    {
	// Vertical scroll bar
	EditVScrollBar.Maximum = DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount - 1 >= 0
		? DMS.SCORE.EditMidiMapSet.DisplayMidiMapAllCount - 1 : 0 ;

	// Horizontal scroll bar
	EditHScrollBar.Maximum = Config.System.NoteCount - 1 >= 0 
		? Config.System.NoteCount - 1 : 0;
    }

#region Edit

/// <summary>
/// 分割入力単位変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
    private void DivisionLineListBox_SelectedIndexChanged( object sender, EventArgs ev )
    {
	try
	{
		DrawSet.SheetDivisionLine = Convert.ToInt32( DivisionLineListBox.SelectedItem.ToString() );

		RefreshScreen();
	}
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

/// <summary>
/// 音量編集タイプ変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void EditVolumeListBox_SelectedIndexChanged( object sender, EventArgs ev )
{
	try
	{
		DrawSet.VolumeEditSelect = (ConfigEditer.VolumeEditType)EditVolumeListBox.SelectedIndex;
	}
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
}

/// <summary>
/// 範囲選択タイプ変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
    private void SelectRangeListBox_SelectedIndexChanged( object sender, EventArgs ev )
    {
	try
	{
		DrawSet.RangeSelect = (ConfigEditer.RangeSelectType)SelectRangeListBox.SelectedIndex;
	}
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

/// <summary>
/// 範囲選択チェック状態変更
/// </summary>
/// <param name="aCheck">True:チェックON、False:チェックOFF</param>
public void DoClearRangeCheckBox( bool aCheck )
{
	try
	{
		ClearRangeCheckBox.Checked = aCheck;
	}
	catch ( Exception e )
	{
		Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// 範囲選択クリア
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void ClearRangeCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
	try
	{
		if ( !ClearRangeCheckBox.Checked )
		{ 
			Config.EventClearEditerRange();

			RefreshScreen();
		}
	}
	catch ( Exception e )
	{
		Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
}

/// <summary>
/// 範囲選択にBPMを含めるフラグ変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void BpmCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
	try
	{
		DrawSet.IncludeBpm = BpmCheckBox.Checked;
	}
	catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

/// <summary>
/// NoteOn/Off入力切替
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
    private void NoteOnCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
	try
	{
		DrawSet.NoteOn = NoteOnCheckBox.Checked;
	}
	catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

#endregion

#region WaveForm

    /// <summary>
    /// 音量TOP変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void VolumeTopNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
	try
	{
		var t = (float)VolumeTopNumericUpDown.Value;
		var h = (float)VolumeHighNumericUpDown.Value;
		var m = (float)VolumeMidNumericUpDown.Value;
		var l = (float)VolumeLowNumericUpDown.Value;

		if ( t < h )
            {
			h = t;

			if ( h < m )
                {
				m = h;

				if ( m < l )
                    {
					l = m;
                    }
                }
            }

		SetVolumeLevel( t, h, m, l );
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

/// <summary>
/// 音量HIGT変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void VolumeHighNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
	try
	{
		var t = (float)VolumeTopNumericUpDown.Value;
		var h = (float)VolumeHighNumericUpDown.Value;
		var m = (float)VolumeMidNumericUpDown.Value;
		var l = (float)VolumeLowNumericUpDown.Value;

		if ( h < Config.Scale.VolumeLevelHigh )
		{ 
			if ( h < m )
			{
				m = h;

				if ( m < l )
				{
					l = m;
				}
			}
		}
		else
		{ 
			if ( h > t )
			{
				t = h;
			}
		}

		SetVolumeLevel( t, h, m, l );
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

/// <summary>
/// 音量MID変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void VolumeMidNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
	try
	{
		var t = (float)VolumeTopNumericUpDown.Value;
		var h = (float)VolumeHighNumericUpDown.Value;
		var m = (float)VolumeMidNumericUpDown.Value;
		var l = (float)VolumeLowNumericUpDown.Value;

		if ( m < Config.Scale.VolumeLevelMid )
		{ 
			if ( m < l )
			{
				l = m;
			}
		}
		else
		{ 
			if ( m > h )
			{
				h = m;

				if ( h > t )
				{
					t = h;
				}
			}
		}

		SetVolumeLevel( t, h, m, l );
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

/// <summary>
/// 音量LOW変更
/// </summary>
/// <param name="sender"></param>
/// <param name="ev"></param>
private void VolumeLowNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
	try
	{
		var t = (float)VolumeTopNumericUpDown.Value;
		var h = (float)VolumeHighNumericUpDown.Value;
		var m = (float)VolumeMidNumericUpDown.Value;
		var l = (float)VolumeLowNumericUpDown.Value;

		if ( l > m )
            {
			m = l;

			if ( m > h )
                {
				h = m;

				if ( h > t )
                    {
					t = h;
                    }
                }
            }

		SetVolumeLevel( t, h, m, l );
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

/// <summary>
/// 音量レベル設定
/// </summary>
/// <param name="aTop">音量TOP</param>
/// <param name="aHigh">音量HIGH</param>
/// <param name="aMid">音量MID</param>
/// <param name="aLow">音量LOW</param>
private void SetVolumeLevel( float aTop, float aHigh, float aMid, float aLow )
    {
	VolumeTopNumericUpDown.Value	= (decimal)aTop;
	VolumeHighNumericUpDown.Value	= (decimal)aHigh;
	VolumeMidNumericUpDown.Value	= (decimal)aMid;
	VolumeLowNumericUpDown.Value	= (decimal)aLow;

	Config.Scale.VolumeLevelTop		= aTop;
	Config.Scale.VolumeLevelHigh	= aHigh;
	Config.Scale.VolumeLevelMid		= aMid;
	Config.Scale.VolumeLevelLow		= aLow;

	Config.EventUpdateEditerWaveForm();

	RefreshScreen();
}

/// <summary>
/// 感度設定
/// </summary>
/// <param name="sender"></param>
/// <param name="ex"></param>
private void ScaleSensitivityTrackBar_Scroll( object sender, EventArgs ex )
    {
	try
	{
		Config.Scale.SensitivityLevel = (float)ScaleSensitivityTrackBar.Value / ScaleSensitivityTrackBar.Maximum;

		RefreshScreen();
	}
	catch ( Exception e )
	{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	}
    }

#endregion

#endif



