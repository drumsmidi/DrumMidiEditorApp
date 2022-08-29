using System.IO;
using System.Text.Json;

using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pUtil;

namespace DrumMidiClassLibrary.pIO.pJson;

/// <summary>
/// JSON入出力
/// </summary>
internal static class JsonIO
{
	/// <summary>
	/// JSONファイル読込
	/// </summary>
	/// <typeparam name="T">読込クラス</typeparam>
	/// <param name="aGeneralPath">読込ファイルパス</param>
	/// <returns>読込済みオブジェクト</returns>
	public static T? LoadFile<T>( GeneralPath aGeneralPath )
	{
        if ( !aGeneralPath.IsExistFile )
        {
			Log.Warning( $"{Log.GetThisMethodName}:file not found... [{aGeneralPath.AbsoulteFilePath}]" );
			return default;
		}

		var dat = File.ReadAllText( aGeneralPath.AbsoulteFilePath );

		var opt = new JsonSerializerOptions()
            {
            };

		SetConverter( opt );

		return JsonSerializer.Deserialize<T>( dat, opt );
    }

	/// <summary>
	/// JSONファイル書込
	/// </summary>
	/// <typeparam name="T">書込クラス</typeparam>
	/// <param name="aSerializeObject">書込オブジェクト</param>
	/// <param name="aGeneralPath">書込ファイルパス</param>
	public static void SaveFile<T>( T aSerializeObject, GeneralPath aGeneralPath )
	{
		var opt = new JsonSerializerOptions()
            {
				WriteIndented = true,
            };

		SetConverter( opt );

		File.WriteAllText( aGeneralPath.AbsoulteFilePath, JsonSerializer.Serialize<T>( aSerializeObject, opt ) );
    }

	/// <summary>
	/// JSONコンバーター設定
	/// </summary>
	/// <param name="aOption">シリアライズオプション</param>
	private static void SetConverter( JsonSerializerOptions aOption )
    {
		aOption.Converters.Add( new ColorConverter() );
		aOption.Converters.Add( new CanvasTextFormatConverter() );
		aOption.Converters.Add( new PointConverter() );
	}
}
