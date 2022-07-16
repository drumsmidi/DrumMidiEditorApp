using System.Drawing;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pEditer.pEdit;

/// <summary>
/// エディター描画アイテム：ノート音量
/// </summary>
public class DmsItemNoteVolume : DisposeBaseClass
{
    /// <summary>
    /// 描画位置
    /// </summary>
    private readonly int _PosX = 0;

    /// <summary>
    /// 描画 底位置
    /// </summary>
    private int _Bottom = 0;

    /// <summary>
    /// NOTE情報
    /// </summary>
    private InfoNote? _NoteInfo = null;

    /// <summary>
    /// 音量背景色
    /// </summary>
    private SolidBrush? _VolumeBrush = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aX">描画位置＋１小節内での相対X座標</param>
    /// <param name="aBottom">描画位置 底位置</param>
    /// <param name="aInfo">NOTE情報</param>
    /// <param name="aBackBrush">音量背景色</param>
    public DmsItemNoteVolume( int aX, int aBottom, InfoNote aInfo, SolidBrush aBackBrush )
    {
        _PosX      		= aX;
        _Bottom         = aBottom;
        _NoteInfo  		= aInfo;
        _VolumeBrush	= aBackBrush;
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !this._Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				_NoteInfo      = null;
                _VolumeBrush   = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

			// Note disposing has been done.
			base.Dispose( aDisposing );
		}
	}
    private bool _Disposed = false;

    /// <summary>
    /// 表示位置の調整
    /// </summary>
    /// <param name="aBottom">底位置</param>
    public void Move( int aBottom ) => _Bottom = aBottom;

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( Graphics aGraphics, int aDiffX, int aDiffY )
    {
        if ( _NoteInfo == null )
        {
            return;
        }

        var r = new Rectangle
            ( 
                _PosX + aDiffX - 1
            ,   _Bottom - _NoteInfo.Volume - aDiffY
            ,   2
            ,   _NoteInfo.Volume
            );

        if ( _VolumeBrush != null )
        { 
            aGraphics.FillRectangle( _VolumeBrush, r );
        }

        // 音量文字表示（手抜き）
        r.X		 -=  10;
        r.Y		 -=  10;
        r.Width   = 100;
        r.Height  =  10;

        aGraphics.DrawString
            (
                _NoteInfo.Volume.ToString(),
                Config.Editer.StrFont,
                Config.Editer.StrBrush,
                r,
                Config.Editer.StrFmt
            );
    }
} 
