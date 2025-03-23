using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Graphics.Canvas.Text;

namespace DrumMidiLibrary.pIO.pJson;

/// <summary>
/// Json converter : CanvasTextFormat
/// </summary>
internal class CanvasTextFormatConverter : JsonConverter<CanvasTextFormat>
{
    public override bool CanConvert( Type aTypeToConvert )
        => typeof( CanvasTextFormat ).IsAssignableFrom( aTypeToConvert );

    public override CanvasTextFormat Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var textFormat = new CanvasTextFormat();

        while ( aReader.Read() && aReader.TokenType != JsonTokenType.EndObject )
        {
            if ( aReader.TokenType == JsonTokenType.PropertyName )
            {
                var prop = aReader.GetString();

                _ = aReader.Read();

                switch ( prop )
                {
                    case "FontFamily":
                        textFormat.FontFamily = aReader.GetString() ?? string.Empty;
                        break;
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
