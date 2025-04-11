using System.Collections.Generic;
using DrumMidiLibrary.pUtil;

namespace DrumMidiPlayerApp.pModel.pOption;

/// <summary>
/// 曲リスト
/// </summary>
public partial class OptionList : DisposeBaseClass
{
    /// <summary>
    /// 親アイテム
    /// </summary>
    public OptionListItem ParentItem { get; private set; }

    /// <summary>
    /// オプションリスト
    /// </summary>
    public readonly List<OptionListItem> ItemList  = [];

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public OptionList( string aRootName )
    {
        ParentItem = new( aRootName );
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aOptionListItem"></param>
    internal OptionList( OptionListItem aOptionListItem )
    {
        ParentItem = aOptionListItem;
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

        OptionListItem? itemCurrent;

        #region 階層別ディレクトリ登録
        {
            var folders = aFilePath.RelativeFolderList;

            foreach ( var folder in folders )
            {
                // ディレクトリ検索
                itemCurrent = listCurrent?.Find( item => folder.Equals( item.ItemName ) );

                // 未登録の場合
                if ( itemCurrent == null )
                {
                    itemCurrent = new( folder );
                    listCurrent?.Add( itemCurrent );
                }

                listCurrent = itemCurrent.OptionList?.ItemList;
            }
        }
        #endregion

        #region ファイル登録
        {
            var filename = aFilePath.FileNameWithoutExtension;

            // ファイル検索
            itemCurrent = listCurrent?.Find( item => filename.Equals( item.ItemName ) );

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
    /// 曲リスト取得
    /// </summary>
    /// <param name="aIndex"></param>
    /// <returns></returns>
    public OptionListItem? GetSongListItem( int aIndex )
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
    public int GetSongListCount( OptionListItem aSongListItem )
        => aSongListItem.OptionList.ItemList.Count ;
}
