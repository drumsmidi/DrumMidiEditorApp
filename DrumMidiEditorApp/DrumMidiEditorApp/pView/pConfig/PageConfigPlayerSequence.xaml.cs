using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using DrumMidiEditorApp.pUtil;
using DrumMidiEditorApp.pUtil.pHelper;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigPlayerSequence : Page, INotifyPropertyChanged
{
    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayerSequence DrawSet => Config.Player.Sequence;

    /// <summary>
    /// Media設定
    /// </summary>
    //private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// 描画方向リスト
    /// </summary>
    private readonly ObservableCollection<string> _DrawDirectionModeList = [];

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigPlayerSequence()
    {
        // 初期化
        InitializeComponent();

        #region 描画方向リスト作成

        foreach ( var name in Enum.GetNames<ConfigPlayerSequence.DrawDirectionMode>() )
        {
            _DrawDirectionModeList.Add( name );
        }

        #endregion

        #region NumberBox の入力書式設定

        _NoteTermHeightNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _NoteTermWidthNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _NoteHeightNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _NoteWidthNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );

        _Line128NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line064NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line032NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line016NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line008NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line004NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line001NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );

        #endregion
    }

    #region INotifyPropertyChanged

    /// <summary>
    /// Config再読み込み
    /// 
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadConfig()
    {
        try
        {
            OnPropertyChanged( "DrawSet" );
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
    /// 描画方向変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void DrawDirectionModeComboBox_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
        try
        {
            EventManage.Event_Player_UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// 共通：トグル切替
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ToggleSwitch_Toggled( object sender, RoutedEventArgs args )
    {
        try
        {
            EventManage.Event_Player_UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ノートサイズ変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void SizeNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( sender, args ) )
            {
                return;
            }

            EventManage.Event_Player_UpdateScore();
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
            if ( sender is not Button item )
            {
                return;
            }

            HelperXaml.ColorDialog
                (
                    item,
                    ( item.Background as SolidColorBrush )?.Color ?? HelperColor.EmptyColor,
                    ( color ) =>
                    {
                        item.Background = new SolidColorBrush( color );
                    }
                );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    private void HeaderGroupToggleSwitch_Toggled( object sender, RoutedEventArgs args )
    {
        try
        {
            if ( sender is not ToggleSwitch item )
            {
                return;
            }

            DrawSet.HeaderGroupOn = item.IsOn;

            ReloadConfig();
            EventManage.Event_Player_UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }


#if false

	/// <summary>
	/// プレイヤー描画設定
	/// </summary>
	private static ConfigPlayerSequence DrawSetSequence => Config.Player.Sequence;

	/// <summary>
	/// 初期化
	/// </summary>
    private void InitUpdate()
    {
		SuspendLayout();
        {
			try
			{

    #region Sequence

                // Header
                HeaderFontButton.Font				= DrawSetSequence.HeaderGI.Font;
				HeaderFontButton.ForeColor			= DrawSetSequence.HeaderGI.FontBrush.Color;
				HeaderFontButton.BackColor			= DrawSetSequence.HeaderGI.BackBrush.Color;
				HeaderBrushButton.ForeColor			= DrawSetSequence.HeaderGI.BackBrush.Color;
				HeaderLineButton.BackColor			= DrawSetSequence.HeaderGI.BackBrush.Color;
				HeaderLineButton.ForeColor			= DrawSetSequence.HeaderGI.ForePen.Color;
				HeaderLineNumericUpDown.Value		= (Decimal)DrawSetSequence.HeaderGI.ForePen.Width;
				HeaderWidthNumericUpDown.Value		= (Decimal)DrawSetSequence.HeaderWidthSize;
				HeaderGroupWidthNumericUpDown.Value	= (Decimal)DrawSetSequence.HeaderGroupWidthSize;
				BpmNowCheckBox.Checked				= DrawSetSequence.BpmNowDisplay;
				GroupCheckBox.Checked				= DrawSetSequence.HeaderGroupOn;

				// Sheet
				Line128Button.BackColor			= DrawSet.SheetColor;
				Line64Button.BackColor			= DrawSet.SheetColor;
				Line32Button.BackColor			= DrawSet.SheetColor;
				Line16Button.BackColor			= DrawSet.SheetColor;
				Line128Button.ForeColor			= DrawSetSequence.SheetMeasure128Pen.Color;
				Line64Button.ForeColor			= DrawSetSequence.SheetMeasure064Pen.Color;
				Line32Button.ForeColor			= DrawSetSequence.SheetMeasure032Pen.Color;
				Line16Button.ForeColor			= DrawSetSequence.SheetMeasure016Pen.Color;
				Line128NumericUpDown.Value		= (int)DrawSetSequence.SheetMeasure128Pen.Width;
				Line64NumericUpDown.Value		= (int)DrawSetSequence.SheetMeasure064Pen.Width;
				Line32NumericUpDown.Value		= (int)DrawSetSequence.SheetMeasure032Pen.Width;
				Line16NumericUpDown.Value		= (int)DrawSetSequence.SheetMeasure016Pen.Width;

    #endregion

			}
			catch ( Exception e )
			{
				Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
			}
        }
		ResumeLayout();
	}

    #region Sequence

    #region Header

    /// <summary>
    /// ヘッダフォント　変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void HeaderFontButton_Click( object sender, EventArgs ev )
    {
		try
		{
            SetFont( HeaderFontButton, DrawSetSequence.HeaderGI );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// ヘッダ背景色　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void HeaderBrushButton_Click( object sender, EventArgs ev )
	{
		try
		{
			if ( !FormUtil.SelectColor( DrawSetSequence.HeaderGI.BackBrush.Color, out var c ) )
			{
				return;
			}

			HeaderBrushButton.ForeColor	= c;
			HeaderFontButton.BackColor		= c;
			HeaderLineButton.BackColor		= c;
			DrawSetSequence.HeaderGI.BackBrush.Color = Color.FromArgb( DrawSetSequence.HeaderGI.BackBrush.Color.A, c );

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ヘッダ外枠色　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void HeaderLineButton_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( HeaderLineButton, DrawSetSequence.HeaderGI.ForePen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ヘッダ外枠サイズ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void HeaderLineNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( HeaderLineNumericUpDown, DrawSetSequence.HeaderGI.ForePen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ヘッダ横幅サイズ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void HeaderWidthNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetSequence.HeaderWidthSize	= (int)HeaderWidthNumericUpDown.Value;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// ヘッダ横幅サイズ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void HeaderGroupWidthNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetSequence.HeaderGroupWidthSize = (int)HeaderGroupWidthNumericUpDown.Value;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

	/// <summary>
	/// Group表示 変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void GroupCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetSequence.HeaderGroupOn = GroupCheckBox.Checked;

			NoteHeightNumericUpDown.Value		= (Decimal)DrawSetSequence.NoteHeightSize;
			NoteWidthNumericUpDown.Value		= (Decimal)DrawSetSequence.NoteWidthSize;
			NoteTermHeightNumericUpDown.Value	= (Decimal)DrawSetSequence.NoteTermHeightSize;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

	/// <summary>
	/// 現在のBPM値表示フラグ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void BpmNowCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetSequence.BpmNowDisplay = BpmNowCheckBox.Checked;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    #region Note

	/// <summary>
	/// 音量サイズ変更フラグ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void VolumeSizeCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetSequence.NoteVolumeSizeOn = VolumeSizeCheckBox.Checked;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// 音量０表示フラグ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void VolumeZeroCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetSequence.NoteVolumeZeroOn = VolumeZeroCheckBox.Checked;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

	/// <summary>
	/// ノートサイズ：縦　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void NoteHeightNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetSequence.NoteHeightSize = (int)NoteHeightNumericUpDown.Value;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// ノートサイズ：横　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void NoteWidthNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetSequence.NoteWidthSize = (int)NoteWidthNumericUpDown.Value;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// ノート間隔：横　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void NoteTermNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetSequence.NoteTermWidthSize = (int)NoteTermWidthNumericUpDown.Value;

			Config.EventUpdatePlayer();
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
	private void NoteTermHeightNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetSequence.NoteTermHeightSize = (int)NoteTermHeightNumericUpDown.Value;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

    #endregion

    #region Sheet

	/// <summary>
	/// １小節１２８分割線色変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void Line128Button_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( Line128Button, DrawSetSequence.SheetMeasure128Pen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// １小節６４分割線色変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void Line64Button_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( Line64Button, DrawSetSequence.SheetMeasure064Pen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// １小節３２分割線色変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void Line32Button_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( Line32Button, DrawSetSequence.SheetMeasure032Pen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// １小節１６分割線色変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void Line16Button_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( Line16Button, DrawSetSequence.SheetMeasure016Pen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// １小節１２８分割線サイズ変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void Line128NumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( Line128NumericUpDown, DrawSetSequence.SheetMeasure128Pen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// １小節６４分割線サイズ変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void Line64NumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( Line64NumericUpDown, DrawSetSequence.SheetMeasure064Pen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// １小節３２分割線サイズ変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void Line32NumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( Line32NumericUpDown, DrawSetSequence.SheetMeasure032Pen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// １小節１６分割線サイズ変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void Line16NumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( Line16NumericUpDown, DrawSetSequence.SheetMeasure016Pen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

    #endregion

    #endregion

    /// <summary>
    /// 共通：ラインカラー設定
    /// </summary>
    /// <param name="aButton">押下ボタン</param>
    /// <param name="aPen">ペン参照</param>
    private static void SetLineColor( Button aButton, Pen aPen )
    {
		if ( !FormUtil.SelectColor( aPen.Color, out var c ) )
		{
			return;
		}

		aButton.ForeColor	= c;
		aPen.Color			= c;

		Config.EventUpdatePlayer();
	}

	/// <summary>
	/// 共通：ラインサイズ設定
	/// </summary>
	/// <param name="aNumericUpDown">NumericUpDownオブジェクト</param>
	/// <param name="aPen">ペン参照</param>
	private static void SetLineSize( NumericUpDown aNumericUpDown, Pen aPen )
    {
		aPen.Width = (float)aNumericUpDown.Value;

		Config.EventUpdatePlayer();
	}

	/// <summary>
	/// 共通：フォント設定
	/// </summary>
	/// <param name="aButton">押下ボタン</param>
	/// <param name="aGraphicItem">描画アイテム参照</param>
	private static void SetFont( Button aButton, GraphicItem aGraphicItem )
    {
		try
		{
			if ( !FormUtil.SelectFont( aGraphicItem.Font, aGraphicItem.FontBrush.Color, out var f, out var c ) )
			{
				return;
			}

			aButton.Font					= f;
			aButton.ForeColor				= c;

			aGraphicItem.Font				= f;
			aGraphicItem.FontBrush.Color	= c;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

#endif
}
