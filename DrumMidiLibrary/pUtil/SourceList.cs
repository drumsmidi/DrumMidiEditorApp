using System;
using System.Collections.Generic;
using System.IO;
using DrumMidiLibrary.pLog;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// ファイル検索ユーティリティ
/// </summary>
public class SourceList
{
    /// <summary>
    /// ファイルリスト
    /// </summary>
	public List<GeneralPath> Sources { get; private set; } = [];

    /// <summary>
    /// 対象ディレクトリ配下のファイル一覧を取得（サブディレクトリも検索）
    /// </summary>
    /// <param name="aFolderPath">検索対象フォルダ</param>
    /// <param name="aSearchPattern">検索パターン</param>
    public void SearchSource( GeneralPath aFolderPath, string aSearchPattern )
    {
        try
        {
            // ファイル取得
            foreach ( var filpath in Directory.GetFiles( aFolderPath.AbsoulteFolderPath, aSearchPattern ) )
            {
                //Log.Info( $"-----{filpath}" );

                Sources.Add( new( $"{filpath}", aFolderPath.BaseFolderPath ) );
            }
            // サブディレクトリ検索
            foreach ( var folderpath in Directory.GetDirectories( aFolderPath.AbsoulteFolderPath ) )
            {
                //Log.Info( $"{folderpath}" );

                SearchSource( new( $"{folderpath}\\", aFolderPath.BaseFolderPath ), aSearchPattern );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
