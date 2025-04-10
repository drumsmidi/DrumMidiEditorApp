using System.Collections.Generic;
using DrumMidiLibrary.pModel.pSongList;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pScreen.pOption;

/// <summary>
/// 描画アイテム：曲スクロールリスト
/// </summary>
internal partial class ItemOptionScrollList() : ItemBase( 0, 0, 0, 0, 0 )
{

    /// <summary>
    /// 曲リスト
    /// </summary>
    private SongList? _SongListCurrent = null;

    /// <summary>
    /// 曲リスト階層別 選択曲リスト
    /// </summary>
    private readonly Stack<SongList> _SongListByHierarchyStack = new();

    /// <summary>
    /// 曲リスト 選択位置
    /// </summary>
    private int _SongListCurrentIndex = 0;

    /// <summary>
    /// 曲リスト階層別 選択位置
    /// </summary>
    private readonly Stack<int> _SongListIndexByHierarchyStack = new();

    /// <summary>
    /// 描画書式：選択中アイテム
    /// </summary>
    private FormatRect? _ActiveFormatRect = Config.ScreenSongList.ActiveFormatRect;

    /// <summary>
    /// 描画書式：未選択中アイテム
    /// </summary>
    private FormatRect? _NormalSelectFormatRect = Config.ScreenSongList.NormalSelectFormatRect;

    /// <summary>
    /// 描画書式：先頭アイテム
    /// </summary>
    private FormatLine? _StartFormatLine = Config.ScreenSongList.StartFormatLine;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _ActiveFormatRect       = null;
                _NormalSelectFormatRect = null;
                _SongListByHierarchyStack.Clear();
                _SongListIndexByHierarchyStack.Clear();
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
        _SongListCurrent        = aSongList;
        _SongListCurrentIndex   = 0;
        _SongListByHierarchyStack.Clear();
        _SongListIndexByHierarchyStack.Clear();
    }

    /// <summary>
    /// 選択中の曲リストアイテム
    /// </summary>
    private SongListItem? CurrentSongListItem
        => _SongListCurrent?.ItemList [ _SongListCurrentIndex ] ?? null ;

    /// <summary>
    /// 選択中のディレクトリ内の曲リストアイテム数
    /// </summary>
    private int CurrentSongListCount
        => _SongListCurrent?.ItemList.Count ?? 0 ;

    /// <summary>
    /// 前の曲へ
    /// </summary>
    /// <returns></returns>
    public SongListItem? PreviewSongList()
    {
        _SongListCurrentIndex--;

        if ( _SongListCurrentIndex < 0 )
        {
            _SongListCurrentIndex = CurrentSongListCount - 1;
        }

        return CurrentSongListItem;
    }

    /// <summary>
    /// 次の曲へ
    /// </summary>
    /// <returns></returns>
    public SongListItem? NextSongList()
    {
        _SongListCurrentIndex++;

        if ( _SongListCurrentIndex >= CurrentSongListCount )
        {
            _SongListCurrentIndex = 0;
        }

        return CurrentSongListItem;
    }

    /// <summary>
    /// 前のディレクトリへ
    /// </summary>
    /// <returns></returns>
    public SongListItem? GoBackSongList()
    {
        if ( _SongListIndexByHierarchyStack.Count == 0 )
        {
            return null;
        }

        _SongListCurrent = _SongListByHierarchyStack.Pop();

        _SongListCurrentIndex = _SongListIndexByHierarchyStack.Pop();

        return CurrentSongListItem;
    }

    /// <summary>
    /// 次のディレクトリへ
    /// </summary>
    /// <returns></returns>
    public SongListItem? GoSongList()
    {
        if ( _SongListCurrent == null || _SongListCurrentIndex >= CurrentSongListCount )
        {
            return null;
        }

        if ( _SongListCurrent?.ItemList [ _SongListCurrentIndex ].IsFile ?? true )
        {
            return CurrentSongListItem;
        }

        _SongListByHierarchyStack.Push( _SongListCurrent );

        _SongListCurrent = _SongListCurrent?.ItemList[ _SongListCurrentIndex ].SongList;

        _SongListIndexByHierarchyStack.Push( _SongListCurrentIndex );

        _SongListCurrentIndex = 0;

        return null;

    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    public void Draw( CanvasDrawingSession aGraphics )
    {
        var parent  = _SongListCurrent?.ParentItem;
        var list    = _SongListCurrent?.ItemList;
        var index   = _SongListCurrentIndex;

        if ( parent != null && list != null )
        {
            var rect = new Rect( 20, 20, 1000, 60 );

            var icon = parent.IsDirectory ? "📁" : "　 " ;

            HelperWin2D.DrawFormatRect( aGraphics, rect, _ActiveFormatRect, $"{icon}{parent.ItemName}" );

            rect.X += 40;
            rect.Y += rect.Height + 40;
            rect.Width -= 40;

            for ( var i = -5; i < 6; i++ )
            {
                var x = ( index + i ) % list.Count;

                if (  x < 0 )
                {
                    x += list.Count;
                }

                var item = list[ x ];

                icon = item.IsDirectory ? "📁" : "　 " ;

                HelperWin2D.DrawFormatRect
                ( 
                    aGraphics, 
                    rect,
                    i == 0 ? _ActiveFormatRect : _NormalSelectFormatRect, 
                    $"{icon}{item.ItemName}" 
                );

                if ( x == 0 )
                {
                    var line = rect;
                    line.Height = 2;
                    HelperWin2D.DrawFormatLine( aGraphics, line, _StartFormatLine );
                }

                rect.Y += rect.Height + 20;
            }
        }
    }
}
