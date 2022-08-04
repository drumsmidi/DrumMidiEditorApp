using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Threading;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface;

public sealed partial class UserControlPlayerPanel : UserControl
{
    #region Member

    /// <summary>
    /// Player設定
    /// </summary>
    private ConfigPlayer DrawSet => Config.Player;

    /// <summary>
    /// プレイヤーサーフェイス
    /// </summary>
    private IPlayerSurface? _PlayerSurface = null;

    /// <summary>
    /// 描画更新用タイマー
    /// </summary>
    private PeriodicTimer? _Timer = null;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlPlayerPanel()
    {
        InitializeComponent();

        ChangeDrawModeAsync();
    }

	/// <summary>
	/// 描画モード変更
	/// </summary>
	public async void ChangeDrawModeAsync()
    {
        switch ( DrawSet.PlayerSurfaceModeSelect )
        {
			case ConfigPlayer.PlayerSurfaceMode.Sequence:
				_PlayerSurface = new pSequence.PlayerSurface();
				break;
			case ConfigPlayer.PlayerSurfaceMode.SequenceVertical:
				_PlayerSurface = new pSequenceVertical.PlayerSurface();
				break;
			case ConfigPlayer.PlayerSurfaceMode.Score:
				_PlayerSurface = new pScore.PlayerSurface();
				break;
			case ConfigPlayer.PlayerSurfaceMode.Simuration:
				_PlayerSurface = new pSimuration.PlayerSurface();
				break;
        }

		Config.EventReloadScore();

        // TODO: 微妙な作り
        try
        {
            var fps = new Fps();
            fps.Set( 1, 0 );
            fps.Set( 2, DrawSet.Fps );
            fps.Start();

            _Timer?.Dispose();

            _Timer = new( TimeSpan.FromSeconds( 0.001 ) );

            while ( await _Timer.WaitForNextTickAsync() )
            {
                fps.Tick();

			    if ( fps.TickFpsWeight( 1 ) )
			    { 
				    _PlayerSurface?.OnMove( fps.GetFrameTime( 1 ) );
			    }

			    if ( fps.TickFpsWeight( 2 ) )
			    {
                    Refresh();

                    fps.TickFpsWeight( 2 );
			    }
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

            _Timer?.Dispose();
            _Timer = null;
        }
    }

    #region Mouse Event

    /// <summary>
    /// マウスダウン処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerCanvas_PointerPressed( object sender, PointerRoutedEventArgs args )
    {
        try
        {
            _PlayerSurface?.MouseDown( sender, args );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// マウス移動処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerCanvas_PointerMoved( object sender, PointerRoutedEventArgs args )
    {
        try
        {
            _PlayerSurface?.MouseMove( sender, args );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// マウスアップ処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerCanvas_PointerReleased( object sender, PointerRoutedEventArgs args )
    {
        try
        {
            _PlayerSurface?.MouseUp( sender, args );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    #region Draw

    /// <summary>
    /// キャンバス描画更新
    /// </summary>
    public void Refresh() => _PlayerCanvas.Invalidate();

    /// <summary>
    /// Win2D アンロード処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void UserControl_Unloaded( object sender, RoutedEventArgs args )
    {
        try
        {
            // Win2D アンロード
            _PlayerCanvas.RemoveFromVisualTree();
            _PlayerCanvas = null;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerCanvas_Draw( CanvasControl sender, CanvasDrawEventArgs args )
    {
        try
        {
            _PlayerSurface?.OnDraw( sender, args );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
