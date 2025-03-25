using System.Collections.Generic;
using DrumMidiLibrary.pIO.pDatabase.pModel;
using DrumMidiLibrary.pUtil;
using LiteDB;

namespace DrumMidiLibrary.pIO.pDatabase;

/// <summary>
/// DB入出力
/// LiteDatabaseを使用
/// </summary>
public static class DBIO
{
    /// <summary>
    /// データベースファイルパス
    /// </summary>
    private static readonly string _DatabasePath = $"{HelperAppDirectory.AppBaseDirectory}./data.db";

    /// <summary>
    /// SongList登録
    /// </summary>
    public static void MeargeSongList( List<GeneralPath> aSongList )
    {
        DeleteAllSongList();

        using var conn = new LiteDatabase( _DatabasePath );

        // DBへ接続
        var table = conn.GetCollection<RecordDmsFile>( "SongList" );

        aSongList.ForEach
        (
            song =>
            {
                var aRecordDmsFile = new RecordDmsFile()
                {
                    FilePath        = song.AbsoulteFilePath,
                    BaseFolderPath  = song.BaseFolderPath,
                };

                // 作成
                table.Upsert( aRecordDmsFile );
            }
        );
    }

    /// <summary>
    /// SongList取得
    /// </summary>
    public static List<GeneralPath> SelectSongList()
    {
        var songList = new List<GeneralPath>();

        using var conn = new LiteDatabase( _DatabasePath );

        // DBへ接続
        var table = conn.GetCollection<RecordDmsFile>( "SongList" );

        var result = table.FindAll();

        foreach ( var rec in result )
        {
            songList.Add( new( rec.FilePath, rec.BaseFolderPath ) );
        }

        return songList;
    }

    /// <summary>
    /// SongList削除
    /// </summary>
    public static void DeleteAllSongList()
    {
        var songList = new List<GeneralPath>();

        using var conn = new LiteDatabase( _DatabasePath );

        // DBへ接続
        var table = conn.GetCollection<RecordDmsFile>( "SongList" );
        table.DeleteAll();
    }


}
