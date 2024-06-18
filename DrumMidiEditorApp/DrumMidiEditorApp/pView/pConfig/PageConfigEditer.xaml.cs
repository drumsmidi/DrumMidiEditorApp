using System;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pConfig;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using DrumMidiEditorApp.pUtil;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigEditer : Page
{
    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigEditer DrawSet => ConfigLocal.Editer;

    /// <summary>
    /// Media設定
    /// </summary>
    //private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// プレイヤー描画モードリスト
    /// </summary>
    //private readonly ObservableCollection<string> _PlayerSurfaceModeList = new();

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigEditer()
    {
        // 初期化
        InitializeComponent();

        #region NumberBox の入力書式設定

        _Line128NumberBox.NumberFormatter
            = XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );
        _Line064NumberBox.NumberFormatter
            = XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );
        _Line032NumberBox.NumberFormatter
            = XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );
        _Line016NumberBox.NumberFormatter
            = XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );
        _Line008NumberBox.NumberFormatter
            = XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );
        _Line004NumberBox.NumberFormatter
            = XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );
        _Line001NumberBox.NumberFormatter
            = XamlHelper.CreateNumberFormatter( 1, 1, 0.1 );

        #endregion
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
            if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
                return;
            }
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

            XamlHelper.ColorDialog
                (
                    item,
                    ( item.Background as SolidColorBrush )?.Color ?? ColorHelper.EmptyColor,
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


#if false

	/// <summary>
	/// プレイヤー描画設定
	/// </summary>
	private static ConfigPlayerSequence DrawSetSequence => Config.Player.Sequence;

	/// <summary>
	/// プレイヤー描画設定
	/// </summary>
	private static ConfigPlayerSequenceVertical DrawSetVertical => Config.Player.Vertical;

	/// <summary>
	/// プレイヤー描画設定
	/// </summary>
	private static ConfigPlayerScore DrawSetScore => Config.Player.Score;

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

				// Note
				VolumeSizeCheckBox.Checked			= DrawSetSequence.NoteVolumeSizeOn;
				VolumeZeroCheckBox.Checked			= DrawSetSequence.NoteVolumeZeroOn;
				NoteHeightNumericUpDown.Value		= (Decimal)DrawSetSequence.NoteHeightSize;
				NoteWidthNumericUpDown.Value		= (Decimal)DrawSetSequence.NoteWidthSize;
				NoteTermWidthNumericUpDown.Value	= (Decimal)DrawSetSequence.NoteTermWidthSize;
				NoteTermHeightNumericUpDown.Value	= (Decimal)DrawSetSequence.NoteTermHeightSize;

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

    #region SequenceVertical

                // Header
                HeaderFontVerticalButton.Font					= DrawSetVertical.HeaderGI.Font;
				HeaderFontVerticalButton.ForeColor				= DrawSetVertical.HeaderGI.FontBrush.Color;
				HeaderFontVerticalButton.BackColor				= DrawSetVertical.HeaderGI.BackBrush.Color;
				HeaderBrushVerticalButton.ForeColor				= DrawSetVertical.HeaderGI.BackBrush.Color;
				HeaderLineVerticalButton.BackColor				= DrawSetVertical.HeaderGI.BackBrush.Color;
				HeaderLineVerticalButton.ForeColor				= DrawSetVertical.HeaderGI.ForePen.Color;
				HeaderLineVerticalNumericUpDown.Value			= (Decimal)DrawSetVertical.HeaderGI.ForePen.Width;
				HeaderHeightVerticalNumericUpDown.Value			= (Decimal)DrawSetVertical.HeaderHeightSize;
				HeaderGroupHeightVerticalNumericUpDown.Value	= (Decimal)DrawSetVertical.HeaderGroupHeightSize;
				BpmNowVerticalCheckBox.Checked					= DrawSetVertical.BpmNowDisplay;
				GroupVerticalCheckBox.Checked					= DrawSetVertical.HeaderGroupOn;

				// Note
				VolumeSizeVerticalCheckBox.Checked			= DrawSetVertical.NoteVolumeSizeOn;
				VolumeZeroVerticalCheckBox.Checked			= DrawSetVertical.NoteVolumeZeroOn;
				NoteHeightVerticalNumericUpDown.Value		= (Decimal)DrawSetVertical.NoteHeightSize;
				NoteWidthVerticalNumericUpDown.Value		= (Decimal)DrawSetVertical.NoteWidthSize;
				NoteTermWidthVerticalNumericUpDown.Value	= (Decimal)DrawSetVertical.NoteTermWidthSize;
				NoteTermHeightVerticalNumericUpDown.Value	= (Decimal)DrawSetVertical.NoteTermHeightSize;

				// Sheet
				Line128VerticalButton.BackColor		= DrawSet.SheetColor;
				Line64VerticalButton.BackColor		= DrawSet.SheetColor;
				Line32VerticalButton.BackColor		= DrawSet.SheetColor;
				Line16VerticalButton.BackColor		= DrawSet.SheetColor;
				Line128VerticalButton.ForeColor		= DrawSetVertical.SheetMeasure128Pen.Color;
				Line64VerticalButton.ForeColor		= DrawSetVertical.SheetMeasure064Pen.Color;
				Line32VerticalButton.ForeColor		= DrawSetVertical.SheetMeasure032Pen.Color;
				Line16VerticalButton.ForeColor		= DrawSetVertical.SheetMeasure016Pen.Color;
				Line128VerticalNumericUpDown.Value	= (int)DrawSetVertical.SheetMeasure128Pen.Width;
				Line64VerticalNumericUpDown.Value	= (int)DrawSetVertical.SheetMeasure064Pen.Width;
				Line32VerticalNumericUpDown.Value	= (int)DrawSetVertical.SheetMeasure032Pen.Width;
				Line16VerticalNumericUpDown.Value	= (int)DrawSetVertical.SheetMeasure016Pen.Width;

    #endregion

    #region Score

                // Header
                //HeaderFontButton.Font				= DrawSetSequence.HeaderGI.Font;
				//HeaderFontButton.ForeColor		= DrawSetSequence.HeaderGI.FontBrush.Color;
				//HeaderFontButton.BackColor		= DrawSetSequence.HeaderGI.BackBrush.Color;
				//HeaderBrushButton.ForeColor		= DrawSetSequence.HeaderGI.BackBrush.Color;
				//HeaderLineButton.BackColor		= DrawSetSequence.HeaderGI.BackBrush.Color;
				//HeaderLineButton.ForeColor		= DrawSetSequence.HeaderGI.ForePen.Color;
				//HeaderLineNumericUpDown.Value		= (Decimal)DrawSetSequence.HeaderGI.ForePen.Width;
				//HeaderWidthNumericUpDown.Value	= (Decimal)DrawSetSequence.HeaderWidthSize;
				//BpmNowScoreCheckBox.Checked		= DrawSetScore.BpmNowDisplay;

				// Note
				VolumeSizeScoreCheckBox.Checked			= DrawSetScore.NoteVolumeSizeOn;
				VolumeZeroScoreCheckBox.Checked			= DrawSetScore.NoteVolumeZeroOn;
				NoteHeightScoreNumericUpDown.Value		= (Decimal)DrawSetScore.NoteHeightSize;
				NoteWidthScoreNumericUpDown.Value		= (Decimal)DrawSetScore.NoteWidthSize;
				NoteTermWidthScoreNumericUpDown.Value	= (Decimal)DrawSetScore.NoteTermWidthSize;
				NoteTermHeightScoreNumericUpDown.Value	= (Decimal)DrawSetScore.NoteTermHeightSize;

				// Sheet
				//Line128Button.BackColor		= DrawSet.SheetColor;
				//Line64Button.BackColor		= DrawSet.SheetColor;
				//Line32Button.BackColor		= DrawSet.SheetColor;
				//Line16Button.BackColor		= DrawSet.SheetColor;
				//Line128Button.ForeColor		= DrawSetSequence.SheetMeasure128Pen.Color;
				//Line64Button.ForeColor		= DrawSetSequence.SheetMeasure064Pen.Color;
				//Line32Button.ForeColor		= DrawSetSequence.SheetMeasure032Pen.Color;
				//Line16Button.ForeColor		= DrawSetSequence.SheetMeasure016Pen.Color;
				//Line128NumericUpDown.Value	= (int)DrawSetSequence.SheetMeasure128Pen.Width;
				//Line64NumericUpDown.Value		= (int)DrawSetSequence.SheetMeasure064Pen.Width;
				//Line32NumericUpDown.Value		= (int)DrawSetSequence.SheetMeasure032Pen.Width;
				//Line16NumericUpDown.Value		= (int)DrawSetSequence.SheetMeasure016Pen.Width;

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

    #region SequenceVertical

    #region Header

    /// <summary>
    /// ヘッダフォント　変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void HeaderFontVerticalButton_Click( object sender, EventArgs ev )
    {
		try
		{
            SetFont( HeaderFontVerticalButton, DrawSetVertical.HeaderGI );
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
	private void HeaderBrushVerticalButton_Click( object sender, EventArgs ev )
	{
		try
		{
			if ( !FormUtil.SelectColor( DrawSetVertical.HeaderGI.BackBrush.Color, out var c ) )
			{
				return;
			}

			HeaderBrushVerticalButton.ForeColor	= c;
			HeaderFontVerticalButton.BackColor	= c;
			HeaderLineVerticalButton.BackColor	= c;
			DrawSetVertical.HeaderGI.BackBrush.Color = Color.FromArgb( DrawSetVertical.HeaderGI.BackBrush.Color.A, c );

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
	private void HeaderLineVerticalButton_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( HeaderLineVerticalButton, DrawSetVertical.HeaderGI.ForePen );
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
	private void HeaderLineVerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( HeaderLineVerticalNumericUpDown, DrawSetVertical.HeaderGI.ForePen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// ヘッダ高さサイズ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void HeaderHeightVerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetVertical.HeaderHeightSize = (int)HeaderHeightVerticalNumericUpDown.Value;

			Config.EventUpdatePlayer();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// ヘッダ高さサイズ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void HeaderGroupHeightVerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetVertical.HeaderGroupHeightSize = (int)HeaderGroupHeightVerticalNumericUpDown.Value;

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
	private void GroupVerticalCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetVertical.HeaderGroupOn = GroupVerticalCheckBox.Checked;

			NoteHeightVerticalNumericUpDown.Value		= (Decimal)DrawSetVertical.NoteHeightSize;
			NoteWidthVerticalNumericUpDown.Value		= (Decimal)DrawSetVertical.NoteWidthSize;
			NoteTermWidthVerticalNumericUpDown.Value	= (Decimal)DrawSetVertical.NoteTermWidthSize;

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
	private void BpmNowVerticalCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetVertical.BpmNowDisplay = BpmNowVerticalCheckBox.Checked;

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
	private void VolumeSizeVerticalCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetVertical.NoteVolumeSizeOn = VolumeSizeVerticalCheckBox.Checked;

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
	private void VolumeZeroVerticalCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetVertical.NoteVolumeZeroOn = VolumeZeroVerticalCheckBox.Checked;

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
	private void NoteHeightVerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetVertical.NoteHeightSize = (int)NoteHeightVerticalNumericUpDown.Value;

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
	private void NoteWidthVerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetVertical.NoteWidthSize = (int)NoteWidthVerticalNumericUpDown.Value;

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
	private void NoteTermWidthVerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetVertical.NoteTermWidthSize = (int)NoteTermWidthVerticalNumericUpDown.Value;

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
	private void NoteTermHeightVerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetVertical.NoteTermHeightSize = (int)NoteTermHeightVerticalNumericUpDown.Value;

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
	private void Line128VerticalButton_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( Line128VerticalButton, DrawSetVertical.SheetMeasure128Pen );
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
	private void Line64VerticalButton_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( Line64VerticalButton, DrawSetVertical.SheetMeasure064Pen );
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
	private void Line32VerticalButton_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( Line32VerticalButton, DrawSetVertical.SheetMeasure032Pen );
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
	private void Line16VerticalButton_Click( object sender, EventArgs ev )
	{
		try
		{
            SetLineColor( Line16VerticalButton, DrawSetVertical.SheetMeasure016Pen );
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
	private void Line128VerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( Line128VerticalNumericUpDown, DrawSetVertical.SheetMeasure128Pen );
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
	private void Line64VerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( Line64VerticalNumericUpDown, DrawSetVertical.SheetMeasure064Pen );
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
	private void Line32VerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( Line32VerticalNumericUpDown, DrawSetVertical.SheetMeasure032Pen );
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
	private void Line16VerticalNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
            SetLineSize( Line16VerticalNumericUpDown, DrawSetVertical.SheetMeasure016Pen );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

    #endregion

    #endregion

    #region Score

    #region Note

	/// <summary>
	/// 音量サイズ変更フラグ　変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void VolumeSizeScoreCheckBox_CheckedChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetScore.NoteVolumeSizeOn = VolumeSizeScoreCheckBox.Checked;

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
	private void VolumeZeroScoreCheckBox_CheckedChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetScore.NoteVolumeZeroOn = VolumeZeroScoreCheckBox.Checked;

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
	private void NoteHeightScoreNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetScore.NoteHeightSize = (int)NoteHeightScoreNumericUpDown.Value;

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
	private void NoteWidthScoreNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetScore.NoteWidthSize = (int)NoteWidthScoreNumericUpDown.Value;

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
	private void NoteTermWidthScoreNumericUpDown_ValueChanged( object sender, EventArgs ev )
    {
		try
		{
			DrawSetScore.NoteTermWidthSize = (int)NoteTermWidthScoreNumericUpDown.Value;

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
	private void NoteTermHeightScoreNumericUpDown_ValueChanged( object sender, EventArgs ev )
	{
		try
		{
			DrawSetScore.NoteTermHeightSize = (int)NoteTermHeightScoreNumericUpDown.Value;

			Config.EventUpdatePlayer();
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
