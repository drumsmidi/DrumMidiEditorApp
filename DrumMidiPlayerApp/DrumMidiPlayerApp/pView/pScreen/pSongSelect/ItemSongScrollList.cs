using System.Collections.Generic;
using DrumMidiLibrary.pModel;
using DrumMidiLibrary.pModel.pSongList;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pScreen.pSongSelect;

/// <summary>
/// 描画アイテム：処理中
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aX">描画位置＋１小節内での相対X座標</param>
/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
/// <param name="aRadius">半径</param>
/// <param name="aFormatRect">描画書式</param>
internal partial class ItemSongScrollList() : DisposeBaseClass
{
    /// <summary>
    /// 曲リスト
    /// </summary>
    private SongList? _SongListRoot = null;

    /// <summary>
    /// 曲リスト
    /// </summary>
    private SongList? _SongListCurrent = null;

    /// <summary>
    /// 曲リスト 選択位置
    /// </summary>
    private int _SongListCurrentIndex = 0;

    /// <summary>
    /// 曲リスト階層別 選択位置
    /// </summary>
    private Stack<int> _SongListIndexByHierarchyStack = new();

    /// <summary>
    /// 現在のフレーム時間
    /// </summary>
    private double _CurrentFrameTime = 0.0F;

    /// <summary>
    /// フレーム終了時間
    /// </summary>
    private readonly double _EndFrameTime = 5.0F;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _ActiveFormatRect = new() {
        Background = new( HelperColor.GetColor("#AA66FFFF" ) ),
        Line        = new( HelperColor.GetColor("#FF000000" ), 1.0F ),
        Text        = new( HelperColor.GetColor("#FFFFFFFF" ),
                            new()
                            {
                                FontFamily          = Config.Media.DefaultFontFamily,
                                FontSize            = 36F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };

    private FormatRect? _NormalSelectFormatRect = new()
    {
        Background = new( HelperColor.GetColor("#AA666666" ) ),
        Line        = new( HelperColor.GetColor("#FF000000" ), 1.0F ),
        Text        = new( HelperColor.GetColor("#FFFFFFFF" ),
                            new()
                            {
                                FontFamily          = Config.Media.DefaultFontFamily,
                                FontSize            = 36F,
                                HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                VerticalAlignment   = CanvasVerticalAlignment.Center,
                            } ),
    };


    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _ActiveFormatRect = null;
                _NormalSelectFormatRect = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// 曲リスト設定
    /// </summary>
    /// <param name="aSongList"></param>
    public void SetSongList( SongList aSongList )
    {
        _SongListRoot           = aSongList;
        _SongListCurrent        = aSongList;
        _SongListCurrentIndex   = 0;
        _SongListIndexByHierarchyStack.Clear();

        GoDirectorySongList();
    }

    private int CurrentSongListCount
        => _SongListCurrent?.ItemList.Count ?? 0 ;

    public void PreviewSongList()
    {
        _SongListCurrentIndex--;

        if ( _SongListCurrentIndex < 0 )
        {
            _SongListCurrentIndex = CurrentSongListCount - 1;
        }
    }

    public void NextSongList()
    {
        _SongListCurrentIndex++;

        if ( _SongListCurrentIndex >= CurrentSongListCount )
        {
            _SongListCurrentIndex = CurrentSongListCount - 1;
        }
    }

    public void GoBackSongList()
    {
        if ( _SongListIndexByHierarchyStack.Count == 0 )
        {
            return;
        }

        _SongListCurrentIndex = _SongListIndexByHierarchyStack.Pop();
    }

    public void GoDirectorySongList()
    {
        if ( _SongListCurrent == null || _SongListCurrentIndex >= CurrentSongListCount )
        {
            return;
        }

        _SongListCurrent = _SongListCurrent?.ItemList[ _SongListCurrentIndex ].SongList;

        _SongListIndexByHierarchyStack.Push( _SongListCurrentIndex );

        _SongListCurrentIndex = 0;

    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    /// <param name="aFrameTime"></param>
    public void Move( double aFrameTime )
    {
        // フレーム時間計算
        _CurrentFrameTime += aFrameTime;
        _CurrentFrameTime %= _EndFrameTime;
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    public void Draw( CanvasDrawingSession aGraphics )
    {
        var progress = (float)( _CurrentFrameTime / _EndFrameTime );

        var parent  = _SongListCurrent?.ParentItem;
        var list    = _SongListCurrent?.ItemList;
        var index   = _SongListCurrentIndex;

        if ( list != null )
        {
            var rect = new Rect( 20, 20, 1000, 60 );

            var icon = parent.IsDirectory ? "📁" : "　 " ;

            HelperWin2D.DrawFormatRect( aGraphics, rect, _ActiveFormatRect, $"{icon}{parent.ItemName}" );

            rect.X += 40;
            rect.Y += rect.Height + 40;
            rect.Width -= 40;

            for ( var i = 0; i < 12; i++ )
            {
                var item = list[ i % list.Count ];

                icon = item.IsDirectory ? "📁" : "　 " ;

                HelperWin2D.DrawFormatRect
                ( 
                    aGraphics, 
                    rect,
                    index == i ? _ActiveFormatRect : _NormalSelectFormatRect, 
                    $"{icon}{item.ItemName}" 
                );

                rect.Y += rect.Height + 20;
            }
        }
    }
}
