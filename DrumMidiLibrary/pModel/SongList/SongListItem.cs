using System;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pModel;

/// <summary>
/// 曲リストアイテム
/// </summary>
public partial class SongListItem : DisposeBaseClass, IComparable, IComparable<SongListItem>
{
    /// <summary>
    /// アイテム名（ディレクトリ名、ファイル名）
    /// </summary>
    public string ItemName { get; private set; } = string.Empty;

    /// <summary>
    /// ファイルパス
    /// </summary>
    public GeneralPath? FilePath { get; private set; } = null;

    /// <summary>
    /// ディレクトリ判定
    /// </summary>
    public bool IsDirectory => !FilePath?.IsExistFile ?? true ;

    /// <summary>
    /// ファイル判定
    /// </summary>
    public bool IsFile => !IsDirectory;

    /// <summary>
    /// 曲リスト
    /// </summary>
    public SongList? SongList { get; private set; } = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aFilePath"></param>
    public SongListItem( string aItemName )
    {
        ItemName = aItemName;
        SongList = new( this );
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aFilePath"></param>
    public SongListItem( GeneralPath aFilePath )
    {
        ItemName = aFilePath.FileNameWithoutExtension;
        FilePath = aFilePath;
        SongList = new( this );
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                SongList?.Dispose();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// 曲リスト 並替用
    /// </summary>
    /// <param name="aOther"></param>
    /// <returns></returns>
    public int CompareTo( SongListItem? aOther )
    {
        if ( aOther == null )
        {
            return 1;
        }

        if ( IsDirectory != aOther.IsDirectory )
        {
            return IsDirectory ? 1 : -1 ;
        }

        return ItemName.CompareTo( aOther.ItemName );
    }

    /// <summary>
    /// 曲リスト 並替用
    /// </summary>
    /// <param name="aOther"></param>
    /// <returns></returns>
    public int CompareTo( object? aOther )
    {
        return aOther == null
            ? 1
            : GetType() != aOther.GetType()
            ? throw new ArgumentException( "Invalid aOther", nameof( aOther ) )
            : CompareTo( aOther as SongListItem );
    }
}
