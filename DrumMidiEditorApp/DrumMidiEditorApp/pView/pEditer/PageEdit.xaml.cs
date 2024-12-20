using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pEvent;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;
using DrumMidiEditorApp.pUtil;
using DrumMidiEditorApp.pMachineLearning;

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
    /// Score情報
    /// </summary>
    private Score Score => DMS.SCORE;

    /// <summary>
    /// 小節番号リスト
    /// </summary>
    private readonly ObservableCollection<string> _MeasureNoList = [];

    /// <summary>
    /// 音量入力タイプリスト
    /// </summary>
    private readonly ObservableCollection<string> _VolumeEditTypeList = [];

    /// <summary>
    /// 範囲選択タイプリスト
    /// </summary>
    private readonly ObservableCollection<string> _RangeSelectTypeList = [];

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageEdit()
    {
        InitializeComponent();

        ControlAccess.PageEdit = this;

        #region 小節番号リスト作成

        var keta = ConfigSystem.MeasureMaxNumber.ToString().Length;

        for ( var measure_no = 0; measure_no <= ConfigSystem.MeasureMaxNumber; measure_no++ )
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
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _NoteWidthNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );

        _VolumeLevelTopNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 3, 0.01 );
        _VolumeLevelHighNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 3, 0.01 );
        _VolumeLevelMidNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 3, 0.01 );
        _VolumeLevelLowNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 3, 0.01 );

        #endregion
    }

    #region INotifyPropertyChanged

    /// <summary>
    /// 描画設定再読み込み
    /// 
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadConfigEditer()
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
            if ( sender is not GridView item )
            {
                return;
            }

            var oldValue = item.SelectedValue;
            var newValue = args.ClickedItem;

            // 同じ小節番号が選択された場合
            if ( oldValue?.Equals( newValue ) ?? false )
            {
                JumpMeasure( Convert.ToInt32( newValue ) );
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

        EventManage.EventEditUpdateSheetPos();
    }

    #endregion

    #region Move sheet Mouse Event

    /// <summary>
    /// マウスダウン押下時のマウス位置
    /// </summary>
    private Point _MouseDownPosition = new();

    /// <summary>
    /// マウスダウン押下中のマウス位置
    /// </summary>
    private Point _MouseMovePosition = new();

    /// <summary>
    /// 等間隔処理実行用タイマー
    /// </summary>
    private PeriodicTimer? _Timer = null;

    /// <summary>
    /// アクション状態一覧
    /// </summary>
    private enum EActionState
    {
        None = 0,
        MoveSheet,
    }

    /// <summary>
    /// アクション状態
    /// </summary>
    private EActionState _ActionState = EActionState.None;

    /// <summary>
    /// Flyout Open
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
	private void MoveSheetFlyout_PointerPressed( object sender, PointerRoutedEventArgs args )
    {
        try
        {
            if ( !_MoveSheetFlyout.IsOpen )
            {
                FlyoutBase.ShowAttachedFlyout( (FrameworkElement)sender );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// シート移動用のFlyoutを閉じる
    /// </summary>
    private void HideMoveSheetFlyout()
    {
        // Flyoutを閉じる
        if ( _MoveSheetFlyout.IsOpen )
        {
            _MoveSheetFlyout.Hide();
        }
    }

    /// <summary>
    /// マウスダウン処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MoveSheetTextBlock_PointerPressed( object sender, PointerRoutedEventArgs args )
    {
        if ( _ActionState != EActionState.None )
        {
            return;
        }

        try
        {
            var p = args.GetCurrentPoint( sender as FrameworkElement );

            if ( p.Properties.IsLeftButtonPressed )
            {
                _MouseDownPosition = p.Position;

                _ActionState = EActionState.MoveSheet;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            _ActionState = EActionState.None;
        }
    }

    /// <summary>
    /// マウス移動処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MoveSheetTextBlock_PointerMoved( object sender, PointerRoutedEventArgs args )
    {
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
            var p = args.GetCurrentPoint( sender as FrameworkElement );

            switch ( _ActionState )
            {
                case EActionState.MoveSheet:
                    {
                        MoveSheetAsync( p.Position );
                    }
                    break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            _ActionState = EActionState.None;
        }
    }

    /// <summary>
    /// マウスアップ処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MoveSheetTextBlock_PointerReleased( object sender, PointerRoutedEventArgs args )
    {
        if ( _ActionState == EActionState.None )
        {
            //HideMoveSheetFlyout();
            return;
        }

        try
        {
            var p = args.GetCurrentPoint( sender as FrameworkElement );

            switch ( _ActionState )
            {
                case EActionState.MoveSheet:
                    {
                        StopTimer();
                        HideMoveSheetFlyout();
                    }
                    break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
        finally
        {
            _ActionState = EActionState.None;
        }
    }

    /// <summary>
    /// タイマー停止
    /// </summary>
    private void StopTimer()
    {
        _Timer?.Dispose();
        _Timer = null;
    }

    /// <summary>
    /// シート移動処理
    /// </summary>
    /// <param name="aMousePoint"></param>
    private async void MoveSheetAsync( Point aMousePoint )
    {
        if ( _Timer != null )
        {
            _MouseMovePosition = aMousePoint;
            return;
        }

        _Timer = new( TimeSpan.FromSeconds( DrawSet.SheetTimerSecond ) );

        while ( await _Timer.WaitForNextTickAsync() )
        {
            var move = new Point
                (
                    ( _MouseMovePosition.X - _MouseDownPosition.X ) / DrawSet.SheetMoveSpeed * DrawSet.SheetTimerSecond,
                    ( _MouseMovePosition.Y - _MouseDownPosition.Y ) / DrawSet.SheetMoveSpeed * DrawSet.SheetTimerSecond
                );

            if ( move.X == 0 && move.Y == 0 )
            {
                return;
            }

            var note_pos = HelperXaml.AdjustRangeIn
                (
                    new
                    (
                        DrawSet.NotePosition.X + move.X,
                        DrawSet.NotePosition.Y + move.Y
                    ),
                    new
                    (
                        0,
                        0,
                        ConfigSystem.NoteCount,
                        Score.EditChannel.MidiMapSet.DisplayMidiMapAllCount
                    )
                );

            DrawSet.NotePosition = new( (int)note_pos.X, (int)note_pos.Y );

            EventManage.EventEditUpdateSheetPos();
        }
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
            EventManage.EventEditClearRangeSelect();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// 範囲選択内ノートを16分割線で左寄せする
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void SelectRangeNoteLeftAlignmentToggleButton_Unchecked( object sender, RoutedEventArgs args )
    {
        try
        {
            EventManage.EventEditLeftAlignNoteOfRangeSelect();
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
            if ( !HelperXaml.NumberBox_RequiredInputValidation( sender, args ) )
            {
                return;
            }

            EventManage.EventEditItemResize();
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
            EventManage.EventEditUndo();
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
            EventManage.EventEditRedo();
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
            if ( !HelperXaml.NumberBox_RequiredInputValidation( sender, args ) )
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
            if ( !HelperXaml.NumberBox_RequiredInputValidation( sender, args ) )
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

            //UpdateVolumeLevel();
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
            if ( !HelperXaml.NumberBox_RequiredInputValidation( sender, args ) )
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

            //UpdateVolumeLevel();
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
            if ( !HelperXaml.NumberBox_RequiredInputValidation( sender, args ) )
            {
                return;
            }

            var m = (float)_VolumeLevelMidNumberBox.Value;
            var l = (float)_VolumeLevelLowNumberBox.Value;

            if ( l > m )
            {
                _VolumeLevelMidNumberBox.Value = l;
            }

            //UpdateVolumeLevel();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// 音量レベル設定
    /// </summary>
    private void UpdateVolumeLevel() => EventManage.EventEditUpdateWaveForm();

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

    /// <summary>
    /// 再生停止
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void WaveForm_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            UpdateVolumeLevel();
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
