using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Graphics.Canvas.Text;

namespace DrumMidiLibrary.pIO.pJson.pConverter;

/// <summary>
/// Json converter : CanvasTextFormat
/// </summary>
internal class CanvasTextFormatConverter : JsonConverter<CanvasTextFormat>
{
    public override CanvasTextFormat Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var textFormat = new CanvasTextFormat();

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
                case JsonTokenType.Number:
                    {
                        switch ( prop )
                        {
                            case "FontSize":
                                textFormat.FontSize = aReader.GetSingle();
                                break;
                            case "HorizontalAlignment":
                                textFormat.HorizontalAlignment = (CanvasHorizontalAlignment)aReader.GetInt32();
                                break;
                            case "VerticalAlignment":
                                textFormat.VerticalAlignment = (CanvasVerticalAlignment)aReader.GetInt32();
                                break;
                        }
                    }
                    break;
                case JsonTokenType.String:
                    {
                        switch ( prop )
                        {
                            case "FontFamily":
                                textFormat.FontFamily = aReader.GetString() ?? string.Empty;
                                break;
                        }
                    }
                    break;
            }
        }

        return textFormat;
    }

    public override void Write( Utf8JsonWriter aWriter, CanvasTextFormat aValue, JsonSerializerOptions aOptions )
    {
        aWriter.WriteStartObject();
        aWriter.WritePropertyName( "FontFamily" );
        aWriter.WriteStringValue( aValue.FontFamily );
        aWriter.WritePropertyName( "FontSize" );
        aWriter.WriteNumberValue( aValue.FontSize );
        aWriter.WritePropertyName( "HorizontalAlignment" );
        aWriter.WriteNumberValue( (int)aValue.HorizontalAlignment );
        aWriter.WritePropertyName( "VerticalAlignment" );
        aWriter.WriteNumberValue( (int)aValue.VerticalAlignment );
        aWriter.WriteEndObject();
    }
}
