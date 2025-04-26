using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigPlayerScoreType2 : Page, INotifyPropertyChanged
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigPlayerScoreType2()
    {
        InitializeComponent();
    }

    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayer DrawSetPlayer => Config.Player;

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayerScoreType2 DrawSet => Config.Player.ScoreType2SelectType;

    #endregion

    /// <summary>
    /// ページロード完了後処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Page_Loaded( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            #region NumberBox の入力書式設定

            _MeasureNoHeightSizeNumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
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
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// ダークモード切り替え時の全プロパティ変更通知
    /// </summary>
    public void NotifyAllPropertiesUsingReflection()
    {
        // NOTE: DarkMode切り替え時に、Bindingを再定義したいがWinUI3ではGetBindingExpression未実装？やり方が間違っている？
        // x:bindでは更新するすべが現状ない？
        // https://github.com/microsoft/microsoft-ui-xaml/issues/5473

        try
        {
            foreach ( var item2 in _SettingStackPanel.Children )
            {
                var prop = item2.GetType().GetProperty( "Name", BindingFlags.Public | BindingFlags.Instance );

                var val = prop?.GetValue( item2 )?.ToString();

                //Log.Info( $"{val}" );

                if ( val != null && val.Length != 0 )
                {
                    if ( item2 is NumberBox num )
                    {
                        var bind = num.GetBindingExpression( NumberBox.ValueProperty );
                        bind?.UpdateSource();
                    }
                    else if ( item2 is Button btn )
                    {
                        var bind = btn.GetBindingExpression( Button.BackgroundProperty );
                        bind?.UpdateSource();
                    }
                    OnPropertyChanged( val );
                }
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    #region 描画設定

    /// <summary>
    /// 共通：トグル切替(DarkMode)
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void DarkModeToggleSwitch_Toggled( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            NotifyAllPropertiesUsingReflection();
            EventManage.Event_Player_UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ノートサイズ変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void SizeNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            EventManage.Event_Player_UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// 共通：トグル切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ToggleSwitch_Toggled( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            EventManage.Event_Player_UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// 共通：色選択
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ColorButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( aSender is Button item )
            {
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
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion
}
