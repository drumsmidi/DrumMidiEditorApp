using System;
using Windows.ApplicationModel;
using Windows.Storage;

namespace DrumMidiEditorApp.pGeneralFunction.pWinUI;

public static class AppDirectory
{
    /// <summary>
    /// アプリケーションベースディレクトリ
    /// </summary>
    public static string AppBaseDirectory => AppContext.BaseDirectory;

    /// <summary>
    /// マイドキュメントフォルダ
    /// </summary>
    public static string MyDocumentsDirectory => Environment.GetFolderPath( Environment.SpecialFolder.Personal );

    /// <summary>
    /// アプリケーションインストールフォルダ情報
    /// 
    /// ファイルアクセスについて
    /// https://docs.microsoft.com/ja-jp/windows/uwp/files/file-access-permissions
    /// </summary>
    //public static readonly StorageFolder InstalledLocation = Package.Current.InstalledLocation;

    /// <summary>
    /// 簡易ローカル設定
    /// 
    /// 参考URL: https://tera1707.com/entry/2022/04/26/224338
    ///   C:\Users\<ユーザー名>\AppData\Local\Packages\<数字の羅列>\Settings\settings.dat
    /// </summary>
    //public static ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

    /// <summary>
    /// ローカルフォルダ
    /// 
    /// 参考URL: https://tera1707.com/entry/2022/04/26/224338
    ///   C:\Users\<ユーザー名>\AppData\Local\Packages\<数字の羅列>\LocalState
    /// </summary>
    //public static StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
}
