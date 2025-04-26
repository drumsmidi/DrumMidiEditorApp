using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiLibrary.pLog;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pPlayer;

public sealed partial class PagePlayer : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PagePlayer()
    {
        InitializeComponent();

        ControlAccess.PagePlayer = this;
    }

    #region member

    /// <summary>
    /// 描画設定
    /// </summary>
    private static ConfigPlayer DrawSet => Config.Player;

    /// <summary>
    /// プレイヤー表示位置調整用マージン
    /// </summary>
    private Thickness _PageMargin = new();

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
            UpdatePageSize();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #region Mouse Event

    /// <summary>
    /// マウスアクション
    /// </summary>
    private enum EActionState : int
    {
        None = 0,
        PlayerMove,
        PlayerOff,
    }

    /// <summary>
    /// マウスアクション状態
    /// </summary>
    private EActionState _ActionState = EActionState.None;

    /// <summary>
    /// ページ移動前の位置
    /// </summary>
    private Point _BeforePos = new();

    /// <summary>
    /// マウスダウン処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Page_PointerPressed( object aSender, PointerRoutedEventArgs aArgs )
    {
        try
        {
            if ( DrawSet.EditModeOn || _ActionState != EActionState.None )
            {
                return;
            }

            var p = aArgs.GetCurrentPoint( ControlAccess.PageEditerMain );
        //  var p = aArgs.GetCurrentPoint( aSender as FrameworkElement );

            if ( p.Properties.IsLeftButtonPressed )
            {
                // フォーム移動
                _BeforePos = p.Position;

                _ActionState = EActionState.PlayerMove;
            }
            else if ( p.Properties.IsRightButtonPressed )
            {
                // 非表示
                EventManage.Event_Player_ChangeDisplay( false );
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );

            _ActionState = EActionState.None;
        }
    }

    /// <summary>
    /// マウス移動処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Page_PointerMoved( object aSender, PointerRoutedEventArgs aArgs )
    {
        try
        {
            if ( DrawSet.EditModeOn || _ActionState == EActionState.None )
            {
                return;
            }

            var p = aArgs.GetCurrentPoint( ControlAccess.PageEditerMain );
        //  var p = aArgs.GetCurrentPoint( aSender as FrameworkElement );

            switch ( _ActionState )
            {
                case EActionState.PlayerMove:
                    {
                        SetPagePosition( p.Position );
                    }
                    break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );

            _ActionState = EActionState.None;
        }
    }

    /// <summary>
    /// マウスアップ処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Page_PointerReleased( object aSender, PointerRoutedEventArgs aArgs )
    {
        try
        {
            if ( DrawSet.EditModeOn || _ActionState == EActionState.None )
            {
                return;
            }

            var p = aArgs.GetCurrentPoint( ControlAccess.PageEditerMain );
        //  var p = aArgs.GetCurrentPoint( aSender as FrameworkElement );

            switch ( _ActionState )
            {
                case EActionState.PlayerMove:
                    {
                        SetPagePosition( p.Position );
                    }
                    break;
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
        finally
        {
            _ActionState = EActionState.None;
        }
    }

    #endregion

    /// <summary>
    /// プレイヤー表示位置設定
    /// </summary>
    /// <param name="aMousePoint"></param>
    private void SetPagePosition( Point aMousePoint )
    {
        _PageMargin.Left    += aMousePoint.X - _BeforePos.X;
        _PageMargin.Top     += aMousePoint.Y - _BeforePos.Y;

        _BeforePos = aMousePoint;

        Margin = _PageMargin;
    }

    /// <summary>
    /// ページサイズ更新
    /// </summary>
    public void UpdatePageSize()
    {
        try
        {
            Width   = DrawSet.ResolutionScreenWidth;
            Height  = DrawSet.ResolutionScreenHeight;
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}
