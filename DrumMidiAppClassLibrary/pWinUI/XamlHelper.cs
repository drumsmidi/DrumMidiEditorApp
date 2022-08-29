using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Globalization.NumberFormatting;
using Windows.Storage.Pickers;
using Windows.UI;
using WinRT.Interop;

using Microsoft.Graphics.Canvas;

using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pUtil;

namespace DrumMidiClassLibrary.pWinUI;

/// <summary>
/// XAMLユーティリティ
/// </summary>
public static class XamlHelper
{
    public static async void MessageDialogOk( XamlRoot aContentXamlRoot, string aTitle, string aContent, string aButtonText, Action aAction )
    {
        var cd = new ContentDialog
        {
            Title           = aTitle,
            Content         = aContent,
            CloseButtonText = aButtonText,
            XamlRoot        = aContentXamlRoot
        };

        var result = await cd.ShowAsync();
            
        if ( result == ContentDialogResult.None )
        {
            aAction();
        }
    }

    public static async void MessageDialogYesNoAsync( XamlRoot aContentXamlRoot, string aTitle, string aContent, string aYesButtonText, string aNoButtonText, Action aAction )
    {
        var cd = new ContentDialog
        {
            Title               = aTitle,
            Content             = aContent,
            PrimaryButtonText   = aYesButtonText,
            CloseButtonText     = aNoButtonText,
            XamlRoot            = aContentXamlRoot
        };

        var result = await cd.ShowAsync();
            
        if ( result == ContentDialogResult.Primary )
        {
            aAction();
        }
    }

    public static void InputDialogOkCancelAsync( XamlRoot aContentXamlRoot, string aTitle, object aPageContent, Action aAction )
        => InputDialogOkCancelAsync
            (
                aContentXamlRoot,
                string.IsNullOrEmpty( aTitle ) ? ResourcesHelper.GetString( "Dialog/Input" ) : aTitle,
                aPageContent,
                ResourcesHelper.GetString( "Dialog/Ok" ),
                ResourcesHelper.GetString( "Dialog/Cancel" ),
                aAction
            );

    public static async void InputDialogOkCancelAsync( XamlRoot aContentXamlRoot, string aTitle, object aPageContent, string aYesButtonText, string aCancelButtonText, Action aAction )
    {
        var cd = new ContentDialog
        {
            Title               = aTitle,
            Content             = aPageContent,
            PrimaryButtonText   = aYesButtonText,
            CloseButtonText     = aCancelButtonText,
            XamlRoot            = aContentXamlRoot,
        };

        var result = await cd.ShowAsync();
            
        if ( result == ContentDialogResult.Primary )
        {
            aAction();
        }
    }

    /// <summary>
    /// 色選択ダイアログ
    /// （Spectrum が表示されない不具合あり）
    /// </summary>
    /// <param name="aContentXamlRoot"></param>
    /// <param name="aColor"></param>
    /// <param name="aAction"></param>
    public static async void ColorDialogAsync( XamlRoot aContentXamlRoot, Color aColor, Action<Color> aAction )
    {
        var content = new ColorPicker
        {
            Color                           = aColor,
            ColorSpectrumComponents         = ColorSpectrumComponents.ValueSaturation,
            ColorSpectrumShape              = ColorSpectrumShape.Box,
            IsColorSpectrumVisible          = true,
            IsColorPreviewVisible           = true,
            IsMoreButtonVisible             = false,
            IsColorSliderVisible            = true,
            IsColorChannelTextInputVisible  = false,
            IsHexInputVisible               = true,
            IsAlphaEnabled                  = true,
            IsAlphaSliderVisible            = true,
            IsAlphaTextInputVisible         = false,
        };

        var cd = new ContentDialog
        {
            Title               = ResourcesHelper.GetString( "Dialog/Input" ),
            Content             = content,
            PrimaryButtonText   = ResourcesHelper.GetString( "Dialog/Ok" ),
            CloseButtonText     = ResourcesHelper.GetString( "Dialog/Cancel" ),
            XamlRoot            = aContentXamlRoot,
        };

        var result = await cd.ShowAsync();
            
        if ( result == ContentDialogResult.Primary )
        {
            aAction( content.Color );
        }
    }



    /// <summary>
    /// ファイルを開くダイアログ共通処理
    /// </summary>
    /// <param name="aOwnerWindow">親ウィンドウ</param>
    /// <param name="aFileTypeFilters">フィルター設定</param>
    /// <param name="aInitialLocation">初期ロケーション</param>
    /// <param name="aSettingsIdentifier">ピッカー設定名</param>
    /// <param name="aAction">ファイル選択時の後続処理</param>
    /// <returns>True:選択、False:未選択</returns>
    public static async void OpenDialogAsync( Window? aOwnerWindow, List<string> aFileTypeFilters, PickerLocationId aInitialLocation, string aSettingsIdentifier, Action<GeneralPath> aAction )
    {
        if ( aOwnerWindow == null )
        {
            return;
        }

        try
        {
            // 参考URL：ピッカーでファイルやフォルダーを開く
            // https://docs.microsoft.com/ja-jp/windows/uwp/files/quickstart-using-file-and-folder-pickers

            var picker = new FileOpenPicker
            {
                // 表示モード：リスト
                ViewMode = PickerViewMode.List,
                // 初期ディレクトリ（初回のみ）
                SuggestedStartLocation = aInitialLocation,
                // ピッカー識別子（前回開いたフォルダ情報などを共有する際に使用）
                // https://docs.microsoft.com/ja-JP/uwp/api/windows.storage.pickers.fileopenpicker.settingsidentifier?view=winrt-22621#windows-storage-pickers-fileopenpicker-settingsidentifier
                SettingsIdentifier = aSettingsIdentifier,
            };

            // ファイルタイプのフィルタ設定
            aFileTypeFilters.ForEach( item => picker.FileTypeFilter.Add( item ) );

            if ( picker.FileTypeFilter.Count == 0 )
            {
                picker.FileTypeFilter.Add( "*" );
            }

            // WinUI3 だと手間が増えてる・・・
            // https://docs.microsoft.com/ja-jp/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/winui3
            // https://docs.microsoft.com/ja-JP/windows/apps/develop/ui-input/display-ui-objects

            InitializeWithWindow.Initialize( picker, WindowNative.GetWindowHandle( aOwnerWindow ) );

            // ファイル選択
            var file = await picker.PickSingleFileAsync();
            
            if ( file != null )
            {
                aAction( new( file.Path ) );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ファイル保存ダイアログ共通処理
    /// </summary>
    /// <param name="aOwnerWindow">親ウィンドウ</param>
    /// <param name="aFileTypeChoices">フィルター設定</param>
    /// <param name="aSaveFileName">保存ファイル名（初期値）</param>
    /// <param name="aInitialLocation">初期ロケーション</param>
    /// <param name="aSettingsIdentifier">ピッカー設定名</param>
    /// <param name="aAction">ファイル選択時の後続処理</param>
    /// <returns>True:選択、False:未選択</returns>
    public static async void SaveDialogAsync( Window? aOwnerWindow, List<string> aFileTypeChoices, string aSaveFileName, PickerLocationId aInitialLocation, string aSettingsIdentifier, Action<GeneralPath> aAction )
    {
        if ( aOwnerWindow == null )
        {
            return;
        }

        try
        {
            // 参考URL：ピッカーでファイルやフォルダーを開く
            // https://docs.microsoft.com/ja-jp/windows/uwp/files/quickstart-using-file-and-folder-pickers

            var picker = new FileSavePicker
            {
                // 初期ディレクトリ（初回のみ）
                SuggestedStartLocation = aInitialLocation,
                // ピッカー識別子（前回開いたフォルダ情報などを共有する際に使用）
                // https://docs.microsoft.com/ja-JP/uwp/api/windows.storage.pickers.fileopenpicker.settingsidentifier?view=winrt-22621#windows-storage-pickers-fileopenpicker-settingsidentifier
                SettingsIdentifier = aSettingsIdentifier,
                // 保存ファイル名
                SuggestedFileName = aSaveFileName,
            };

            // ファイルタイプのフィルタ設定
            if ( aFileTypeChoices.Count > 0 )
            { 
                picker.FileTypeChoices.Add( "", aFileTypeChoices );
            }
            else
            {
                picker.FileTypeChoices.Add( "", new List<string>(){ "*" } );
            }

            InitializeWithWindow.Initialize( picker, WindowNative.GetWindowHandle( aOwnerWindow ) );

            // ファイル選択
            var file = await picker.PickSaveFileAsync();
            
            if ( file != null )
            {
                aAction( new( file.Path ) );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #region Win2D

    /// <summary>
    /// ラベル描画
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatRect"></param>
    /// <param name="aLabelText"></param>
    public static void FormatRectDraw( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatRect aFormatRect, string aLabelText )
    {
        // 背景色
        aGraphics.FillRectangle
            (
                aDrawRect,
                aFormatRect.Background.Color
            );

        // テキスト
        if ( aLabelText.Length != 0 )
        {
            aGraphics.DrawText
                (
                    aLabelText,
                    aDrawRect,
                    aFormatRect.Text.TextColor.Color,
                    aFormatRect.Text.TextFormat
                );
        }

        // 外枠
        if ( aFormatRect.Line.LineSize > 0 )
        { 
            aGraphics.DrawRectangle
                (
                    aDrawRect,
                    aFormatRect.Line.LineColor.Color,
                    aFormatRect.Line.LineSize
                );
        }
    }

    #endregion

    #region UserControl

    /// <summary>
    /// スレッドアクセスを持たない場合、aActionイベントを実行する
    /// </summary>
    /// <param name="aUserControl"></param>
    /// <param name="aAction"></param>
    /// <returns>True:スレッドアクセスあり、False:スレッドアクセスなし</returns>
    public static bool DispatcherQueueHasThreadAccess( UserControl aUserControl, Action aAction )
    {
		if ( !aUserControl.DispatcherQueue.HasThreadAccess )
        {
            aUserControl.DispatcherQueue.TryEnqueue( DispatcherQueuePriority.Normal, () => aAction() );
            return false;
        }
        return true;
    }

    #endregion

    #region NumberBox

    public static INumberFormatter2 CreateNumberFormatter( int aIntegerDigits, int aFractionDigits, double aIncrement )
    {
        // NumberBox の入力書式設定

        return new DecimalFormatter
            {
                IntegerDigits   = aIntegerDigits,
                FractionDigits  = aFractionDigits,
                NumberRounder   = new IncrementNumberRounder
                {
                    Increment           = aIncrement,
                    RoundingAlgorithm   = RoundingAlgorithm.RoundHalfAwayFromZero,
                },
                IsGrouped       = true,
                IsZeroSigned    = true,
            };
    }

    /// <summary>
    /// 共通：NumberBox 必須入力チェック
    /// 未入力の場合、変更前の値に戻す。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static bool NumberBox_RequiredInputValidation( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
		if ( double.IsNaN( args.NewValue ) )
		{
			sender.Value = args.OldValue;
            return false;
		}
        return true;
    }

    #endregion

    /// <summary>
    /// aMousePos が aRange の範囲内かどうか判定
    /// </summary>
    /// <param name="aMousePos">チェック位置</param>
    /// <param name="aRange">チェック範囲</param>
    /// <returns>True:範囲内にある、False:範囲外にある</returns>
    public static bool CheckRange( in Point aMousePos, in Rect aRange )
    {
        return (    aRange.Left <= aMousePos.X && aMousePos.X <= aRange.Right
                 && aRange.Top  <= aMousePos.Y && aMousePos.Y <= aRange.Bottom );
    }

    /// <summary>
    /// aMousePos が aRange の範囲内に収まるようにPoint調整して返す
    /// </summary>
    /// <param name="aMousePos">チェック位置</param>
    /// <param name="aRange">チェック範囲</param>
    /// <returns>修正後座標</returns>
    public static Point AdjustRangeIn( in Point aMousePos, in Rect aRange )
    {
        var ret = aMousePos;

        if ( ret.X < aRange.Left )
        {
            ret.X = aRange.Left;
        }
        else if ( ret.X > aRange.Right )
        {
            ret.X = aRange.Right;
        }

        if ( ret.Y < aRange.Top )
        {
            ret.Y = aRange.Top;
        }
        else if ( ret.Y > aRange.Bottom )
        {
            ret.Y = aRange.Bottom;
        }

        return ret;
    }

}
