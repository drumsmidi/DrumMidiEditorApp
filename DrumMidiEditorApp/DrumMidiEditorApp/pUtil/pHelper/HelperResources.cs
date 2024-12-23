using Windows.ApplicationModel.Resources;

namespace DrumMidiEditorApp.pUtil.pHelper;

public static class HelperResources
{
    /// <summary>
    /// リソース
    /// </summary>
    private static readonly ResourceLoader _Resource = new();

    /// <summary>
    /// リソースの値取得
    /// </summary>
    /// <param name="aKey"></param>
    /// <returns></returns>
    public static string GetString( string aKey )
        => _Resource.GetString( aKey );

    /// <summary>
    /// リソースの値取得
    /// </summary>
    /// <param name="aKey"></param>
    /// <returns></returns>
    public static string GetString( string aKey, params object [] aParams )
        => string.Format( GetString( aKey ), aParams );
}
