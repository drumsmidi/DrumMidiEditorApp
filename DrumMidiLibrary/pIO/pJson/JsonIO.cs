using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pIO.pJson;

/// <summary>
/// JSON入出力
/// </summary>
public static class JsonIO
{
    /// <summary>
    /// JSONファイル読込
    /// </summary>
    /// <typeparam name="T">読込クラス</typeparam>
    /// <param name="aGeneralPath">読込ファイルパス</param>
    /// <returns>読込済みオブジェクト</returns>
    public static T? LoadFile<T>( GeneralPath aGeneralPath )
    {
        return Log.TryCatch<T?>
        (
            () =>
            {
                var dat = File.ReadAllText( aGeneralPath.AbsoluteFilePath );

                return JsonSerializer.Deserialize<T>( dat, _JsonSerializerOptions );
            },
            ( e ) => { throw new FileLoadException( "Failed to load file.", aGeneralPath.AbsoluteFilePath, e ); }
        );
    }

    /// <summary>
    /// JSONファイル書込
    /// </summary>
    /// <typeparam name="T">書込クラス</typeparam>
    /// <param name="aSerializeObject">書込オブジェクト</param>
    /// <param name="aGeneralPath">書込ファイルパス</param>
    public static void SaveFile<T>( T aSerializeObject, GeneralPath aGeneralPath )
    {
        Log.TryCatch
        (
            () =>
            {
                File.WriteAllText( aGeneralPath.AbsoluteFilePath, JsonSerializer.Serialize<T>( aSerializeObject, _JsonSerializerOptions ) );
            },
            ( e ) => { throw new FileLoadException( "Failed to save file...", aGeneralPath.AbsoluteFilePath, e ); }
        );
    }

    /// <summary>
    /// 共通JSONシリアライズオプション
    /// </summary>
    private static readonly JsonSerializerOptions _JsonSerializerOptions = CreateJsonSerializerOptionsAll();

    /// <summary>
    /// JSONコンバーター設定（全Converter）
    /// </summary>
    /// <param name="aOption">シリアライズオプション</param>
    private static JsonSerializerOptions CreateJsonSerializerOptionsAll()
    {
        var opt = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };

        // カスタムコンバーターの手動登録
        //SetConverter
        //(
        //    opt,
        //    new ColorConverter(),
        //    new CanvasTextFormatConverter(),
        //    new PointConverter()
        //);

        // GitHub Copilot提案
        // カスタムコンバーターの自動登録
        var converters = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where
                ( 
                    t => 
                        t.IsSubclassOf( typeof( JsonConverter ) ) &&
                        t.Namespace == "DrumMidiLibrary.pIO.pJson.pConverter" &&
                        !t.IsAbstract 
                )
            .Select( t => Activator.CreateInstance( t ) as JsonConverter );

        foreach ( var converter in converters )
        {
            if ( converter != null )
            {
                opt.Converters.Add( converter );

                Log.Info( $"Set JsonConverter Type={converter.GetType().FullName}" );
            }
        }

        return opt;
    }
}
