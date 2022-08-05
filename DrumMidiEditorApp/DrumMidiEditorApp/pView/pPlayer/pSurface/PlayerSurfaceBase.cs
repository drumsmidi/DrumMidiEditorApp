using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Input;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pDMS;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class PlayerSurfaceBase : IPlayerSurface
{
    /// <summary>
    /// プレイヤー設定（共通）
    /// </summary>
    protected ConfigPlayer DrawSetCom => Config.Player;

    /// <summary>
    /// システム設定
    /// </summary>
    protected ConfigSystem ConfigSystem => Config.System;

    /// <summary>
    /// メディア設定
    /// </summary>
    protected ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// スコア
    /// </summary>
    protected Score Score => DMS.SCORE;

    /// <summary>
    /// スクリーンサイズ
    /// </summary>
    protected Size _ScreenSize = new();

    /// <summary>
    /// ノート位置（絶対値）小数点あり
    /// </summary>
    protected float _SheetPosX = 0;

    /// <summary>
    /// ノート位置（絶対値）
    /// </summary>
    protected int _NotePositionX = 0;

    /// <summary>
    /// 再生開始処理後の現在の再生時間（秒）
    /// </summary>
    protected double _DmsPlayTime = 0.0D;

    /// <summary>
    /// 再生状態
    /// </summary>
    private PlayState _DmsPlayState = PlayState.Stop;

    /// <summary>
    /// 再生要求内容（通常再生、ループ再生の判定に使用）
    /// </summary>
    private PlayState _DmsPlayStatePre = PlayState.Stop;

    /// <summary>
    /// 再生状態一覧
    /// </summary>
    private enum PlayState : int
    {
        Stop = 0,
        PrePlayStart,
        PreLoopPlayStart,
        Playing,
        PreRecord,
        Recording,
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    protected PlayerSurfaceBase() { }

    public virtual void MouseDown( object sender, PointerRoutedEventArgs args ) { }

    public virtual void MouseMove( object sender, PointerRoutedEventArgs args ) { }

    public virtual void MouseUp( object sender, PointerRoutedEventArgs args ) { }

	public virtual bool OnMove( double aFrameTime )
    {
        #region Request play or loop play or stop
        { 
            switch ( DrawSetCom.PlayReq )
            {
                case ConfigPlayer.PlayRequest.PrePlay      : _DmsPlayState = PlayState.PrePlayStart;     break;
                case ConfigPlayer.PlayRequest.PreLoopPlay  : _DmsPlayState = PlayState.PreLoopPlayStart; break;
                case ConfigPlayer.PlayRequest.PreStop      : _DmsPlayState = PlayState.Stop;             break;
                case ConfigPlayer.PlayRequest.PreRecord    : _DmsPlayState = PlayState.PreRecord;        break;
            }

            DrawSetCom.PlayReq = ConfigPlayer.PlayRequest.None;

            switch ( _DmsPlayState )
            {
                case PlayState.Playing:
                    {
                        if ( DrawSetCom.UpdateScoreFlag )
                        {
                            UpdateScore();
                            UpdateScoreLine();
                            UpdateScoreHeader();

                            if ( _DmsPlayStatePre == PlayState.PrePlayStart )
                            {
                                var measureMaxNo = Score.GetMaxMeasureNo();

                                for ( int measure_no = 0; measure_no <= measureMaxNo; measure_no++ )
                                {
									UpdateBpmMeasure( measure_no );
									UpdateScoreMeasure( measure_no );
                                }
                            }
                            else
                            {
                                for ( int measure_no = ConfigMedia.PlayLoopStart; measure_no <= ConfigMedia.PlayLoopEnd; measure_no++ )
                                {
                                    UpdateBpmMeasure( measure_no );
                                    UpdateScoreMeasure( measure_no );
                                }
                            }

                            DrawSetCom.UpdateScoreFlag = false;
                        }

                        // Calc sheet position
						_DmsPlayTime = DmsControl.PlayTime;

                        var note_pos = DmsControl.PlayNote( _DmsPlayTime );

                        _SheetPosX = note_pos;

                        var limit_width = ConfigSystem.NoteCount;

                        if ( _SheetPosX > limit_width )
                        {
                            _SheetPosX = limit_width;
                        }

                        _NotePositionX = (int)note_pos;
                    }
                    break;
                case PlayState.Recording:
                    {
                        // Calc sheet position
                        _DmsPlayTime = aFrameTime;

                        var note_pos = DmsControl.PlayNote( _DmsPlayTime );

                        _SheetPosX = note_pos;

                        var limit_width = ConfigSystem.NoteCount;

                        if ( _SheetPosX > limit_width )
                        {
                            _SheetPosX = limit_width;
                        }

                        _NotePositionX = (int)note_pos;
                    }
                    break;
                case PlayState.PrePlayStart:
                    {
                        _DmsPlayStatePre = _DmsPlayState;

                        UpdateScore();
                        UpdateScoreLine();
                        UpdateScoreHeader();

                        ClearMeasure();

                        var measureMaxNo = Score.GetMaxMeasureNo();

                        for ( int measure_no = 0; measure_no <= measureMaxNo; measure_no++ )
                        {
							UpdateBpmMeasure( measure_no );
							UpdateScoreMeasure( measure_no );
                        }

                        DmsControl.WaitAudio();

						_SheetPosX     = 0;
                        _NotePositionX = 0;
                        _DmsPlayTime   = DmsControl.StartPlayTime;
                        _DmsPlayState  = PlayState.Playing;
					}
                    break;
                case PlayState.PreLoopPlayStart:
                    {
                        _DmsPlayStatePre = _DmsPlayState;

                        UpdateScore();
                        UpdateScoreLine();
                        UpdateScoreHeader();

                        ClearMeasure();

                        for ( int measure_no = ConfigMedia.PlayLoopStart; measure_no <= ConfigMedia.PlayLoopEnd; measure_no++ )
                        {
                            UpdateBpmMeasure( measure_no );
                            UpdateScoreMeasure( measure_no );
                        }

                        DmsControl.WaitAudio();

						_SheetPosX     = 0;
                        _NotePositionX = 0;
                        _DmsPlayTime   = DmsControl.StartPlayTime;
                        _DmsPlayState  = PlayState.Playing;
					}
					break;
                case PlayState.PreRecord:
                    {
                        _DmsPlayStatePre = _DmsPlayState;

                        UpdateScore();
                        UpdateScoreLine();
                        UpdateScoreHeader();

                        ClearMeasure();

                        var measureMaxNo = Score.GetMaxMeasureNo();

                        for ( int measure_no = 0; measure_no <= measureMaxNo; measure_no++ )
                        {
							UpdateBpmMeasure( measure_no );
							UpdateScoreMeasure( measure_no );
                        }

                        DmsControl.WaitAudio();

                        _SheetPosX     = 0;
                        _NotePositionX = 0;
                        _DmsPlayTime   = DmsControl.StartPlayTime;
                        _DmsPlayState  = PlayState.Recording;
                    }
                    break;
            }
        }
        #endregion

        return true;
    }

    /// <summary>
    /// スコア範囲設定更新
    /// </summary>
    protected virtual void UpdateScore() 
    {
        // screen
        _ScreenSize.Height  = DrawSetCom.ResolutionScreenHeight;
        _ScreenSize.Width   = DrawSetCom.ResolutionScreenWidth;
    }

    /// <summary>
    /// 小節線表示更新
    /// </summary>
	protected virtual void UpdateScoreLine() { }

    /// <summary>
    /// ヘッダ表示更新
    /// </summary>
    protected virtual void UpdateScoreHeader() { }

    /// <summary>
    /// 小節NOTE／BPM表示設定クリア
    /// </summary>
    protected virtual void ClearMeasure() { }

    /// <summary>
    /// 小節NOTE表示更新
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    protected virtual void UpdateScoreMeasure( int aMeasureNo ) { }

    /// <summary>
    /// 小節BPM表示更新
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    protected virtual void UpdateBpmMeasure( int aMeasureNo ) { }

    public virtual bool OnDraw( CanvasSwapChainPanel sender, CanvasDrawEventArgs args )
    {
        // SwapChainの描画セッション作成時に背景色指定済み
        //args.DrawingSession.Clear( DrawSetCom.SheetColor.Color );

        if ( _DmsPlayState != PlayState.Playing 
          && _DmsPlayState != PlayState.Recording )
        {
            return false;
        }
        return true;
    }
}
