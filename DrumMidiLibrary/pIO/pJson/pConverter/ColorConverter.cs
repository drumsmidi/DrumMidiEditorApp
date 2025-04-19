using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using DrumMidiLibrary.pUtil;
using Windows.UI;

namespace DrumMidiLibrary.pIO.pJson.pConverter;

/// <summary>
/// Json converter : Color
/// </summary>
internal class ColorConverter : JsonConverter<Color>
{
    public override Color Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var c = HelperColor.EmptyColor;;

        var prop = string.Empty;

        while ( aReader.Read() && aReader.TokenType != JsonTokenType.EndObject )
        {
            switch ( aReader.TokenType )
            {
                case JsonTokenType.PropertyName:
                    {
                        prop = aReader.GetString() ?? string.Empty;
                    }
                    break;
                case JsonTokenType.String:
                    {
                        switch ( prop )
                        {
                            case "ColorArgb":
                                c = HelperColor.GetColor( aReader.GetString() ?? string.Empty );
                                break;
                        }
                    }
                    break;
            }
        }

        return c;
    }

    public override void Write( Utf8JsonWriter aWriter, Color aValue, JsonSerializerOptions aOptions )
    {
        aWriter.WriteStartObject();
        aWriter.WritePropertyName( "ColorArgb" );
        aWriter.WriteStringValue( HelperColor.GetColorText( aValue ) );
        aWriter.WriteEndObject();
    }
}
