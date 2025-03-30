using System.Collections.Generic;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pModel.pSongList;

/// <summary>
/// 曲リスト
/// </summary>
public partial class SongList : DisposeBaseClass
{
    /// <summary>
    /// 親ディレクトリ
    /// </summary>
    public SongListItem ParentItem { get; private set; }

    /// <summary>
    /// 曲リスト
    /// </summary>
    public readonly List<SongListItem> ItemList  = [];

    public SongList()
    {
        ParentItem = new( $"" );
    }

    internal SongList( SongListItem aSongListItem )
    {
        ParentItem = aSongListItem;
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                ItemList.Clear();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// 曲リストへファイル追加
    /// </summary>
    /// <param name="aFilePath"></param>
    public void Add( GeneralPath aFilePath )
    {
        var listCurrent = ItemList;

        SongListItem? itemCurrent;

        #region 階層別ディレクトリ登録
        {
            var folders = aFilePath.RelativeFolderList;

            foreach ( var folder in folders )
            {
                // ディレクトリ検索
                itemCurrent = listCurrent?.Find( item => item.IsDirectory && folder.Equals( item.ItemName ) );

                // 未登録の場合
                if ( itemCurrent == null )
                {
                    itemCurrent = new( folder );
                    listCurrent?.Add( itemCurrent );
                }

                listCurrent = itemCurrent.SongList?.ItemList;
            }
        }
        #endregion

        #region ファイル登録
        {
            var filename = aFilePath.FileNameWithoutExtension;

            // ファイル検索
            itemCurrent = listCurrent?.Find( item => item.IsFile && filename.Equals( item.ItemName ) );

            // 未登録の場合
            if ( itemCurrent == null )
            {
                itemCurrent = new( aFilePath );
                listCurrent?.Add( itemCurrent );
            }
        }
        #endregion
    }

    /// <summary>
    /// 曲リストソート
    /// </summary>
    public void Sort()
    {
        ItemList.Sort();

        foreach( var item in ItemList )
        {
            item.SongList.Sort();
        }
    }

    /// <summary>
    /// 曲リスト取得
    /// </summary>
    /// <param name="aIndex"></param>
    /// <returns></returns>
    public SongListItem? GetSongListItem( int aIndex )
    {
        if ( aIndex >= ItemList.Count )
        {
            return null;
        }
        return ItemList[ aIndex ];
    }

    /// <summary>
    /// 対象データ内の曲リスト数
    /// </summary>
    /// <param name="aSongListItem"></param>
    /// <returns></returns>
    public int GetSongListCount( SongListItem aSongListItem )
        => aSongListItem.SongList.ItemList.Count ;
}
