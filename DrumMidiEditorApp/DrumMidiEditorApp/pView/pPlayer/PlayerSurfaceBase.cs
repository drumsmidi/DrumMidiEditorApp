using System.Windows.Forms;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pDirectX;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pView.pPlayer;

/// <summary>
/// プレイヤーサーフェイス
/// </summary>
public class PlayerSurfaceBase : IPlayerSurface
{
    /// <summary>
    /// DirectX2D
    /// </summary>
    protected readonly Graphic2D _Graphic2D;

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
    private enum PlayState
    {
        Stop = 0,
        PrePlayStart,
        PreLoopPlayStart,
        Playing,
        PreRecord,
        Recording,
    }

    /// <summary>
    /// プレイヤー設定
    /// </summary>
    private static ConfigPlayer DrawSet => Config.Player;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aGraphic2D">DirectX2D</param>
    public PlayerSurfaceBase( Graphic2D aGraphic2D )
    {
        _Graphic2D = aGraphic2D;

        //_Graphic2D.SetBackGroundColor( DrawSet.SheetColor );
    }

    public virtual void MouseDown( object sender, MouseEventArgs ev ) { }

    public virtual void MouseMove( object sender, MouseEventArgs ev ) { }

    public virtual void MouseUp( object sender, MouseEventArgs ev ) { }

	public virtual bool OnMove( double aFrameTime )
    {
        #region Request play or loop play or stop
        { 
            switch ( DrawSet.PlayReq )
            {
                case ConfigPlayer.PlayRequest.PrePlay      : _DmsPlayState = PlayState.PrePlayStart;     break;
                case ConfigPlayer.PlayRequest.PreLoopPlay  : _DmsPlayState = PlayState.PreLoopPlayStart; break;
                case ConfigPlayer.PlayRequest.PreStop      : _DmsPlayState = PlayState.Stop;             break;
                case ConfigPlayer.PlayRequest.PreRecord    : _DmsPlayState = PlayState.PreRecord;        break;
            }

            DrawSet.PlayReq = ConfigPlayer.PlayRequest.None;

            switch ( _DmsPlayState )
            {
                case PlayState.Playing:
                    {
                        if ( DrawSet.UpdateScoreFlag )
                        {
                            //_Graphic2D?.ClearResource();
                            //_Graphic2D?.SetBackGroundColor( DrawSet.SheetColor );

                            UpdateScore();
                            UpdateScoreLine();
                            UpdateScoreHeader();

                            if ( _DmsPlayStatePre == PlayState.PrePlayStart )
                            {
                                var measureMaxNo = DMS.SCORE.GetMaxMeasureNo();

                                for ( int i = 0; i <= measureMaxNo; i++ )
                                {
									UpdateBpmMeasure( i );
									UpdateScoreMeasure( i );
                                }
                            }
                            else
                            {
                                for ( int i = Config.Media.PlayLoopStart; i <= Config.Media.PlayLoopEnd; i++ )
                                {
                                    UpdateBpmMeasure( i );
                                    UpdateScoreMeasure( i );
                                }
                            }

                            DrawSet.UpdateScoreFlag = false;
                        }

                        // Calc sheet position
						_DmsPlayTime = DmsControl.PlayTime;

                        //var note_pos = (float)DmsControl.PlayNote( _DmsPlayTime );

                        //_SheetPosX = (float)note_pos;

                        //int limit_width = Config.System.NoteCount;

                        //if ( _SheetPosX > limit_width )
                        //{
                        //    _SheetPosX = limit_width;
                        //}

                        //_NotePositionX = (int)note_pos;
                    }
                    break;
                case PlayState.Recording:
                    {
                        // Calc sheet position
                        _DmsPlayTime = aFrameTime;

                        //var note_pos = (float)DmsControl.PlayNote( _DmsPlayTime );

                        //_SheetPosX = (float)note_pos;

                        //int limit_width = Config.System.NoteCount;

                        //if ( _SheetPosX > limit_width )
                        //{
                        //    _SheetPosX = limit_width;
                        //}

                        //_NotePositionX = (int)note_pos;
                    }
                    break;
                case PlayState.PrePlayStart:
                    {
                        _DmsPlayStatePre = _DmsPlayState;

                        //_Graphic2D?.ClearResource();
                        //_Graphic2D?.SetBackGroundColor( DrawSet.SheetColor );

                        UpdateScore();
                        UpdateScoreLine();
                        UpdateScoreHeader();

                        ClearMeasure();

                        var measureMaxNo = DMS.SCORE.GetMaxMeasureNo();

                        for ( int i = 0; i <= measureMaxNo; i++ )
                        {
							UpdateBpmMeasure( i );
							UpdateScoreMeasure( i );
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

                        //_Graphic2D?.SetBackGroundColor( DrawSet.SheetColor );

                        UpdateScore();
                        UpdateScoreLine();
                        UpdateScoreHeader();

                        ClearMeasure();

                        for ( int i = Config.Media.PlayLoopStart; i <= Config.Media.PlayLoopEnd; i++ )
                        {
                            UpdateBpmMeasure( i );
                            UpdateScoreMeasure( i );
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

                        //_Graphic2D?.ClearResource();
                        //_Graphic2D?.SetBackGroundColor( DrawSet.SheetColor );

                        UpdateScore();
                        UpdateScoreLine();
                        UpdateScoreHeader();

                        ClearMeasure();

                        var measureMaxNo = DMS.SCORE.GetMaxMeasureNo();

                        for ( int i = 0; i <= measureMaxNo; i++ )
                        {
							UpdateBpmMeasure( i );
							UpdateScoreMeasure( i );
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
    protected virtual void UpdateScore() { }

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

    public virtual bool OnDraw()
    {
        if ( this._DmsPlayState != PlayState.Playing 
          && this._DmsPlayState != PlayState.Recording )
        {
            return false;
        }
        return true;
    }
}
