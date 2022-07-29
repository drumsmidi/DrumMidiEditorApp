using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace DrumMidiEditorApp.pView.pEditer;

public sealed partial class PageEdit : Page, INotifyPropertyChanged
{
    #region Member

    /// <summary>
    /// Editerタブ設定
    /// </summary>
    private ConfigEditer DrawSet => Config.Editer;

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
	/// 小節番号リスト
	/// </summary>
	private readonly ObservableCollection<string> _MeasureNoList = new();

	/// <summary>
	/// 音量入力タイプリスト
	/// </summary>
	private readonly ObservableCollection<string> _VolumeEditTypeList = new();

	/// <summary>
	/// 範囲選択タイプリスト
	/// </summary>
	private readonly ObservableCollection<string> _RangeSelectTypeList = new();

    #endregion

	/// <summary>
	/// コンストラクタ
	/// </summary>
    public PageEdit()
    {
        InitializeComponent();

		#region 小節番号リスト作成

		int keta = ConfigSystem.MeasureMaxNumber.ToString().Length;

		for ( int measure_no = 0; measure_no <= ConfigSystem.MeasureMaxNumber; measure_no++ )
		{
			_MeasureNoList.Add( measure_no.ToString().PadLeft( keta, '0' ) );
		}

		#endregion

		#region 音量入力モードリスト作成

		foreach ( var name in Enum.GetNames<ConfigEditer.VolumeEditType>() )
		{ 
			_VolumeEditTypeList.Add( name );
		}

		#endregion

		#region 範囲選択モードリスト作成

		foreach ( var name in Enum.GetNames<ConfigEditer.RangeSelectType>() )
		{
			_RangeSelectTypeList.Add( name );
		}

        #endregion

        #region NumberBox の入力書式設定

        _NoteHeightNumberBox.NumberFormatter
			= XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );
		_NoteWidthNumberBox.NumberFormatter
			= XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );

		_VolumeLevelTopNumberBox.NumberFormatter
			= XamlHelper.CreateNumberFormatter( 1, 3, 0.01 );
		_VolumeLevelHighNumberBox.NumberFormatter
			= XamlHelper.CreateNumberFormatter( 1, 3, 0.01 );
		_VolumeLevelMidNumberBox.NumberFormatter
			= XamlHelper.CreateNumberFormatter( 1, 3, 0.01 );
		_VolumeLevelLowNumberBox.NumberFormatter
			= XamlHelper.CreateNumberFormatter( 1, 3, 0.01 );

		#endregion

		ControlAccess.PageEdit = this;
	}

	#region INotifyPropertyChanged

	/// <summary>
	/// x:Bind OneWay/TwoWay 再読み込み
	/// </summary>
	public void ReloadRangeSelectButton()
    {
		OnPropertyChanged( "DrawSet" );
	}

	public event PropertyChangedEventHandler? PropertyChanged = delegate { };

	public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
		=> PropertyChanged?.Invoke( this, new( aPropertyName ) );

	#endregion


    #region Move sheet

    /// <summary>
    /// 小節番号移動
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MeasureNoGridView_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
		try
		{
            var value = args.AddedItems[ 0 ].ToString();

			if ( !string.IsNullOrEmpty( value ) )
            {
				JumpMeasure( Convert.ToInt32( value ) );
            }
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}

    }

	/// <summary>
	/// 小節番号移動
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void MeasureNoGridView_ItemClick( object sender, ItemClickEventArgs args )
	{
		try
		{
            var value = args.ClickedItem.ToString();

			if ( !string.IsNullOrEmpty( value ) )
            {
				JumpMeasure( Convert.ToInt32( value ) );
            }
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// シート位置　指定の小節番号へ移動
	/// </summary>
	/// <param name="aMeasureNo">小節番号</param>
	private void JumpMeasure( int aMeasureNo )
    {
		DrawSet.NotePosition = new( aMeasureNo * ConfigSystem.MeasureNoteNumber, DrawSet.NotePosition.Y );

		Config.EventUpdateEditerSheetPos();

		Refresh();
    }

    #endregion

    #region Edit

    /// <summary>
    /// 範囲選択解除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void SelectRangeToggleButton_Unchecked( object sender, RoutedEventArgs args )
	{
		try
		{
			Config.EventClearEditerRange();

			Refresh();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

	#endregion

    #region Note

    /// <summary>
    /// ノートサイズ変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void NoteSizeNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			Config.EventUpdateEditerSize();

			Refresh();
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
	/// <param name="args"></param>
	private void UndoButton_Click( object sender, RoutedEventArgs args )
	{
		try
		{
			Config.EventEditerUndo();

			Refresh();
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
	/// <param name="args"></param>
    private void RedoButton_Click( object sender, RoutedEventArgs args )
    {
		try
		{
			Config.EventEditerRedo();

			Refresh();
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
	/// <param name="args"></param>
	private void VolumeLevelTopNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
	{
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			var t = (float)_VolumeLevelTopNumberBox.Value;
			var h = (float)_VolumeLevelHighNumberBox.Value;

			if ( t < h )
			{
				_VolumeLevelHighNumberBox.Value = t;
			}

			UpdateVolumeLevel();
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
	/// <param name="args"></param>
	private void VolumeLevelHighNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
	{
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			var t = (float)_VolumeLevelTopNumberBox.Value;
			var h = (float)_VolumeLevelHighNumberBox.Value;
			var m = (float)_VolumeLevelMidNumberBox.Value;

			if ( h < ConfigScale.VolumeLevelHigh )
			{
				if ( h < m )
				{
					_VolumeLevelMidNumberBox.Value = h;
				}
			}
			else
			{
				if ( h > t )
				{
					_VolumeLevelTopNumberBox.Value = h;
				}
			}

			UpdateVolumeLevel();
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
	/// <param name="args"></param>
	private void VolumeLevelMidNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
	{
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			var h = (float)_VolumeLevelHighNumberBox.Value;
			var m = (float)_VolumeLevelMidNumberBox.Value;
			var l = (float)_VolumeLevelLowNumberBox.Value;

			if ( m < ConfigScale.VolumeLevelMid )
			{
				if ( m < l )
				{
					_VolumeLevelLowNumberBox.Value = m;
				}
			}
			else
			{
				if ( m > h )
				{
					_VolumeLevelHighNumberBox.Value = m;
				}
			}

			UpdateVolumeLevel();
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
	/// <param name="args"></param>
	private void VolumeLevelLowNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
	{
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			var m = (float)_VolumeLevelMidNumberBox.Value;
			var l = (float)_VolumeLevelLowNumberBox.Value;

			if ( l > m )
			{
				_VolumeLevelMidNumberBox.Value = l;
			}

			UpdateVolumeLevel();
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// 音量レベル設定
	/// </summary>
	private void UpdateVolumeLevel()
	{
		Config.EventUpdateEditerWaveForm();

		Refresh();
	}

	/// <summary>
	/// 感度設定
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void WaveFormSensitivityLevelSlider_ValueChanged( object sender, RangeBaseValueChangedEventArgs args )
	{
		try
		{
			Refresh();
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	#endregion

	/// <summary>
	/// EditerPanel描画更新
	/// </summary>
	public void Refresh() => _EditerPanel.Refresh();
}
