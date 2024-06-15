using System;
using System.Collections.Generic;
using System.IO;
using DrumMidiClassLibrary.pLog;

namespace DrumMidiClassLibrary.pUtil;

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
            foreach ( var filename in Directory.GetFiles( aFolderPath.AbsoulteFolderPath, aSearchPattern ) )
            {
                Sources.Add( new( $"{aFolderPath.AbsoulteFolderPath}/{filename}" ) );
            }
            // サブディレクトリ検索
            foreach ( var foldername in Directory.GetDirectories( aFolderPath.AbsoulteFolderPath ) )
            {
                SearchSource( new( $"{aFolderPath.AbsoulteFolderPath}/{foldername}" ), aSearchPattern );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
