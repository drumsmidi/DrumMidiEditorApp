﻿using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// 汎用パス
/// </summary>
public class GeneralPath
{
    /// <summary>
    /// ファイル絶対パス
    /// </summary>
    [JsonIgnore]
    private string _FullPath = string.Empty;

    /// <summary>
    /// 基準フォルダパス
    /// </summary>
    [JsonIgnore]
    private readonly string _BasePath = ConvertPath( StartupFolderPath );

    /// <summary>
    /// パス文字 変更前
    /// </summary>
    [JsonIgnore]
    private static readonly char _ConvertBef = Path.AltDirectorySeparatorChar; // '/'
    /// <summary>
    /// パス文字 変更後
    /// </summary>
    [JsonIgnore]
    private static readonly char _ConvertAft = Path.DirectorySeparatorChar; // '\\'

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GeneralPath()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aGeneralPath">コピー元パス</param>
    public GeneralPath( GeneralPath aGeneralPath )
    {
        _FullPath = aGeneralPath._FullPath;
        _BasePath = aGeneralPath._BasePath;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aPath">ファイルパス</param>
    public GeneralPath( string aPath )
    {
        _FullPath = Path.GetFullPath( ConvertPath( aPath ), StartupFolderPath );
        _BasePath = AbsoulteFolderPath;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aPath">ファイルパス</param>
    /// <param name="aBaseFolderPath">基準ディレクトリパス</param>
    public GeneralPath( string aPath, string aBaseFolderPath )
    {
        _FullPath = Path.GetFullPath( ConvertPath( aPath ), ConvertPath( aBaseFolderPath ) );
        _BasePath = ConvertPath( aBaseFolderPath );
    }

    /// <summary>
    /// ファイル絶対パスの長さ
    /// </summary>
    [JsonIgnore]
    public int Length => _FullPath?.Length ?? 0;

    /// <summary>
    /// アプレケーション起動ディレクトリパス
    /// </summary>
    [JsonIgnore]
    public static string StartupFolderPath => ConvertPath( HelperAppDirectory.AppBaseDirectory );

    /// <summary>
    /// 基準ディレクトリパス
    /// </summary>
    [JsonIgnore]
    public string BaseFolderPath => _BasePath;

    /// <summary>
    /// ファイル絶対パス
    /// </summary>
    [JsonInclude]
    public string AbsoulteFilePath
    {
        get => _FullPath;
        set => _FullPath = Path.GetFullPath( ConvertPath( value ), _BasePath );
    }

    /// <summary>
    /// ファイル相対パス
    /// </summary>
    [JsonIgnore]
    public string RelativeFilePath
        => _FullPath.Equals( string.Empty ) ? string.Empty : Path.GetRelativePath( _BasePath, _FullPath );

    /// <summary>
    /// ディレクトリ絶対パス
    /// </summary>
    [JsonIgnore]
    public string AbsoulteFolderPath
        => Path.GetDirectoryName( AbsoulteFilePath ) ?? string.Empty;

    /// <summary>
    /// ディレクトリ相対パス
    /// </summary>
    [JsonIgnore]
    public string RelativeFolderPath
        => Path.GetDirectoryName( RelativeFilePath ) ?? string.Empty;

    /// <summary>
    /// ディレクトリ一覧（絶対パス）
    /// </summary>
    public string [] AbsoulteFolderList
        => AbsoulteFolderPath.Split( _ConvertAft );

    /// <summary>
    /// ディレクトリ一覧（相対パス）
    /// </summary>
    public string [] RelativeFolderList
        => RelativeFolderPath.Split( _ConvertAft );

    /// <summary>
    /// 拡張子ありファイル名
    /// </summary>
    [JsonIgnore]
    public string FileName
    {
        get => Path.GetFileName( _FullPath ) ?? string.Empty;
        set => _FullPath = $"{AbsoulteFolderPath}{_ConvertAft}{ConvertPath( value )}";
    }

    /// <summary>
    /// 拡張子なしファイル名
    /// </summary>
    [JsonIgnore]
    public string FileNameWithoutExtension
        => _FullPath.Equals( string.Empty ) ? string.Empty : Path.GetFileNameWithoutExtension( _FullPath );

    /// <summary>
    /// ドット含む拡張子
    /// </summary>
    [JsonIgnore]
    public string Extension
    {
        get => Path.GetExtension( _FullPath ) ?? string.Empty;
        set => _FullPath = $"{AbsoulteFolderPath}{_ConvertAft}{FileNameWithoutExtension}{ConvertPath( value )}";
    }

    /// <summary>
    /// ファイル存在チェック
    /// </summary>
    [JsonIgnore]
    public bool IsExistFile => File.Exists( _FullPath );

    /// <summary>
    /// ディレクトリ存在チェック
    /// </summary>
    [JsonIgnore]
    public bool IsExistDirectory => Directory.Exists( AbsoulteFolderPath );

    /// <summary>
    /// ディレクトリ作成
    /// </summary>
    public DirectoryInfo CreateDirectory() => Directory.CreateDirectory( AbsoulteFolderPath );

    /// <summary>
    /// '/' ⇒ '\\' に変換
    /// </summary>
    /// <param name="aPath">変換前パス</param>
    /// <returns>変換後パス</returns>
    private static string ConvertPath( string aPath )
        => aPath.Replace( _ConvertBef, _ConvertAft );


    /// <summary>
    /// ディレクトリ絶対パスにサブフォルダを結合して返す
    /// </summary>
    /// <param name="aSubFolder"></param>
    /// <returns></returns>
    public string GetAbsoulteFolderPathCombineSubFolder( string aSubFolder )
        => Path.Combine( AbsoulteFolderPath, aSubFolder );

}
