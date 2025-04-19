using System;
using Windows.ApplicationModel.Resources;

namespace DrumMidiLibrary.pUtil;

public static class HelperResources
{
    /// <summary>
    /// リソース
    /// </summary>
    private static readonly ResourceLoader _Resource = new();

    /// <summary>
    /// 指定されたキーに対応するリソース文字列を取得します。
    /// </summary>
    /// <param name="aKey">リソースキー</param>
    /// <returns>リソース文字列</returns>
    /// <exception cref="ArgumentException">キーが見つからない場合にスローされます。</exception>
    public static string GetString( string aKey )
    {
        var value = _Resource.GetString( aKey );

        if ( string.IsNullOrEmpty( value ) )
        {
            throw new ArgumentException( $"Resource key '{aKey}' not found.", nameof( aKey ) );
        }
        return value;
    }

    /// <summary>
    /// 指定されたキーに対応するリソース文字列を取得し、指定されたパラメータでフォーマットします。
    /// </summary>
    /// <param name="aKey">リソースキー</param>
    /// <param name="aParams">フォーマットに使用するパラメータ</param>
    /// <returns>フォーマット済みのリソース文字列</returns>
    /// <exception cref="ArgumentException">キーが見つからない場合にスローされます。</exception>
    /// <exception cref="FormatException">フォーマットに失敗した場合にスローされます。</exception>
    /// <code>
    /// var message = HelperResources.GetString("Greeting", "John");
    /// // リソースキー "Greeting" の値が "Hello, {0}!" の場合、結果は "Hello, John!" となります。
    /// </code>
    /// </example>
    public static string GetString( string aKey, params object [] aParams )
        => string.Format( GetString( aKey ), aParams ?? [] );
}
