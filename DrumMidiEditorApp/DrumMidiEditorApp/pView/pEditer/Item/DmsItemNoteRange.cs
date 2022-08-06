using Microsoft.Graphics.Canvas;
using System;
using Windows.Foundation;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：ノート範囲選択
/// </summary>
public class DmsItemNoteRange : DisposeBaseClass
{
    /// <summary>
    /// 範囲選択開始ノート位置（絶対値）
    /// </summary>
    private int _StartNotePosX = 0;

    /// <summary>
    /// 範囲選択開始ノート位置（絶対値）
    /// </summary>
    private int _StartNotePosY = 0;

    /// <summary>
    /// 範囲選択終了ノート位置（絶対値）
    /// </summary>
    private int _EndNotePosX = 0;

    /// <summary>
    /// 範囲選択終了ノート位置（絶対値）
    /// </summary>
    private int _EndNotePosY = 0;

    /// <summary>
    /// 範囲選択 移動開始ノート位置（絶対値）
    /// </summary>
    private int _StartMoveNotePosX = 0;

    /// <summary>
    /// 範囲選択 移動開始ノート位置（絶対値）
    /// </summary>
    private int _StartMoveNotePosY = 0;

    /// <summary>
    /// 範囲選択 移動終了ノート位置（絶対値）
    /// </summary>
    private int _EndMoveNotePosX = 0;

    /// <summary>
    /// 範囲選択 移動終了ノート位置（絶対値）
    /// </summary>
    private int _EndMoveNotePosY = 0;

    /// <summary>
    /// シート左上位置の１ノート範囲
    /// </summary>
    private Rect _NoteRect = new();

    /// <summary>
    /// シート左上位置基準での選択範囲（Pixcel)
    /// </summary>
    private Rect _Range = new();

    /// <summary>
    /// 範囲選択タイプ
    /// </summary>
    private ConfigEditer.RangeSelectType _RangeSelectType = ConfigEditer.RangeSelectType.Normal;

    /// <summary>
    /// 範囲選択 選択状態
    /// </summary>
    public bool Selected { get; private set; } = false;

    /// <summary>
    /// 移動前後で範囲選択の左端のノート位置（絶対値）
    /// </summary>
    public int LeftNotePosX => Math.Min( StartNotePosX, StartNotePosX + MoveNotePosX );

    /// <summary>
    /// 移動前後で範囲選択の右端のノート位置（絶対値）
    /// </summary>
    public int RightNotePosX => Math.Max( EndNotePosX, _EndNotePosX + MoveNotePosX );

    /// <summary>
    /// 選択範囲の移動量X（ノート数）
    /// </summary>
    public int MoveNotePosX => _EndMoveNotePosX - _StartMoveNotePosX;

    /// <summary>
    /// 選択範囲の移動量Y（ノート数）
    /// </summary>
    public int MoveNotePosY => _EndMoveNotePosY - _StartMoveNotePosY;

    /// <summary>
    /// 選択範囲の横幅（ノート数）
    /// </summary>
    public int TermNotePosX 
        => Math.Abs( _EndNotePosX - _StartNotePosX );

    /// <summary>
    /// 移動後、選択範囲内の左端ノート位置（絶対値）
    /// </summary>
    public int StartNotePosX 
        => Math.Min( _StartNotePosX , _EndNotePosX ) + MoveNotePosX;

    /// <summary>
    /// 移動後、選択範囲内の右端ノート位置（絶対値）
    /// </summary>
    public int EndNotePosX 
        => Math.Max( _StartNotePosX , _EndNotePosX ) + MoveNotePosX;

    /// <summary>
    /// 選択範囲の高さ（ノート数）
    /// </summary>
    public int TermNotePosY 
        => Math.Abs( _EndNotePosY - _StartNotePosY ) + 1;

    /// <summary>
    /// 移動後、選択範囲内の上端ノート位置（絶対値）
    /// </summary>
    public int StartNotePosY 
        => Math.Min( _StartNotePosY , _EndNotePosY ) + MoveNotePosY;

    /// <summary>
    /// 移動後、選択範囲内の下端ノート位置（絶対値）
    /// </summary>
    public int EndNotePosY 
        => Math.Max( _StartNotePosY , _EndNotePosY ) + MoveNotePosY;

    /// <summary>
    /// 左上ノート位置
    /// </summary>
    /// <param name="aX">左上ノート位置X座標</param>
    /// <param name="aY">左上ノート位置Y座標</param>
    /// <param name="aWidth">１ノートの横幅</param>
    /// <param name="aHeight">１ノートの高さ</param>
    public void SetNoteRect( float aX, float aY, float aWidth, float aHeight )
    {
        _NoteRect.X		    = aX;
        _NoteRect.Y		    = aY;
        _NoteRect.Width	    = aWidth;
        _NoteRect.Height	= aHeight;

        CalcRange();
    }

    /// <summary>
    /// 選択開始位置設定
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）X座標</param>
    /// <param name="aNotePosY">ノート位置（絶対値）Y座標</param>
    /// <param name="aRangeSelectType">範囲選択タイプ</param>
    public void SetStartPos( int aNotePosX, int aNotePosY, ConfigEditer.RangeSelectType aRangeSelectType )
    {
        if ( aRangeSelectType == ConfigEditer.RangeSelectType.All )
        {
            aNotePosX = 0;
        }

        _StartNotePosX     = aNotePosX;
        _StartNotePosY     = aNotePosY;
        _StartMoveNotePosX = aNotePosX;
        _StartMoveNotePosY = aNotePosY;
        _EndMoveNotePosX   = aNotePosX;
        _EndMoveNotePosY   = aNotePosY;
        _RangeSelectType   = aRangeSelectType;

        Selected = false;
    }

    /// <summary>
    /// 選択終了位置設定
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）X座標</param>
    /// <param name="aNotePosY">ノート位置（絶対値）Y座標</param>
    public void SetEndPos( int aNotePosX, int aNotePosY )
    {
        switch ( _RangeSelectType )
        {
            case ConfigEditer.RangeSelectType.Normal:
                { 
                    _EndNotePosX = aNotePosX;
                }
                break;
            case ConfigEditer.RangeSelectType.Split:
                {
                    _EndNotePosX = ( _StartNotePosX <= aNotePosX )
                            ? Config.System.NoteCount
                            : 0 ;
                }
                break;
            case ConfigEditer.RangeSelectType.All:
                {
                    _EndNotePosX = Config.System.NoteCount;
                }
                break;
        }

        _EndNotePosY = aNotePosY;

        Selected = true;

        CalcRange();
    }

    /// <summary>
    /// 選択状態を解除
    /// </summary>
    public void ClearPos()
    {
        Selected = false;
    }

    /// <summary>
    /// 選択範囲 移動開始位置設定
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）X座標</param>
    /// <param name="aNotePosY">ノート位置（絶対値）Y座標</param>
    public void SetStartMovePos( int aNotePosX, int aNotePosY )
    {
        // 前回移動後の位置を設定
        _StartNotePosX    += MoveNotePosX;
        _StartNotePosY    += MoveNotePosY;
        _EndNotePosX      += MoveNotePosX;
        _EndNotePosY      += MoveNotePosY;

        // 移動開始／終了位置の初期化
        _StartMoveNotePosX = aNotePosX;
        _StartMoveNotePosY = aNotePosY;
        _EndMoveNotePosX   = aNotePosX;
        _EndMoveNotePosY   = aNotePosY;
    }

    /// <summary>
    /// 選択範囲 移動終了位置設定
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）X座標</param>
    /// <param name="aNotePosY">ノート位置（絶対値）Y座標</param>
    public void SetEndMovePos( int aNotePosX, int aNotePosY )
    {
        _EndMoveNotePosX = aNotePosX;
        _EndMoveNotePosY = aNotePosY;
    }

    /// <summary>
    /// 移動前の位置へ戻す
    /// </summary>
    public void ResetStartPos()
    {
        _StartNotePosX -= MoveNotePosX;
        _StartNotePosY -= MoveNotePosY;
        _EndNotePosX   -= MoveNotePosX;
        _EndNotePosY   -= MoveNotePosY;
    }

    /// <summary>
    /// 選択範囲サイズ計算
    /// </summary>
    private void CalcRange()
	{
		if ( Selected )
		{
            _Range.X        = _NoteRect.X;
            _Range.Y        = _NoteRect.Y;
            _Range.Width    = _NoteRect.Width  * TermNotePosX;
            _Range.Height   = _NoteRect.Height * TermNotePosY;
		}
	}

    /// <summary>
    /// 移動後 選択範囲の取得
    /// </summary>
    /// <param name="aStartNotePos">ノート位置（絶対値）</param>
    /// <returns>選択範囲</returns>
    public Rect GetSelectRange( PointInt aStartNotePos )
    {
        if ( !Selected )
        {
            return new( -1, -1, 0, 0 );
        }

        var rect = _Range;
        rect.X += ( StartNotePosX - (int)aStartNotePos.X ) * _NoteRect.Width;
        rect.Y += ( StartNotePosY - (int)aStartNotePos.Y ) * _NoteRect.Height;

        return rect;
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aStartNotePos">ノート位置（絶対値）</param>
    public void Draw( CanvasDrawingSession aGraphics, PointInt aStartNotePos )
    {
        if ( !Selected )
        {
            return;
        }

        var format = Config.Editer.NoteRangeLine;

        var rect = GetSelectRange( aStartNotePos );

		aGraphics.DrawRectangle
            ( 
                rect,
                format.LineColor.Color,
                format.LineSize
            );
    }

    /// <summary>
    /// 複製
    /// </summary>
    /// <returns>複製データ</returns>
    public DmsItemNoteRange Clone()
    {
        return new()
            {
                _StartNotePosX      = this._StartNotePosX,
                _StartNotePosY      = this._StartNotePosY,
                _EndNotePosX        = this._EndNotePosX,
                _EndNotePosY        = this._EndNotePosY,
                _StartMoveNotePosX  = this._StartMoveNotePosX,
                _StartMoveNotePosY  = this._StartMoveNotePosY,
                _EndMoveNotePosX    = this._EndMoveNotePosX,
                _EndMoveNotePosY    = this._EndMoveNotePosY,
                _NoteRect           = this._NoteRect,
                _Range              = this._Range,
                _RangeSelectType    = this._RangeSelectType,
                Selected            = this.Selected
            };
    }

    /// <summary>
    /// aObjの内容を設定
    /// </summary>
    /// <param name="aObj">コピー元</param>
    public void Set( DmsItemNoteRange aObj )
    {
        _StartNotePosX     = aObj._StartNotePosX;
        _StartNotePosY     = aObj._StartNotePosY;
        _EndNotePosX       = aObj._EndNotePosX;
        _EndNotePosY       = aObj._EndNotePosY;
        _StartMoveNotePosX = aObj._StartMoveNotePosX;
        _StartMoveNotePosY = aObj._StartMoveNotePosY;
        _EndMoveNotePosX   = aObj._EndMoveNotePosX;
        _EndMoveNotePosY   = aObj._EndMoveNotePosY;
        _NoteRect          = aObj._NoteRect;
        _Range             = aObj._Range;
        _RangeSelectType   = aObj._RangeSelectType;
        Selected           = aObj.Selected;
    }
}
