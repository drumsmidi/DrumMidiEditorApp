using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Foundation;

namespace DrumMidiLibrary.pIO.pJson.pConverter;

/// <summary>
/// Json converter : Point
/// </summary>
internal class PointConverter : JsonConverter<Point>
{
    public override Point Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var item = new Point( 0, 0 );

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
                            case "X":
                                item.X = aReader.GetDouble();
                                break;
                            case "Y":
                                item.Y = aReader.GetDouble();
                                break;
                        }
                    }
                    break;
            }
        }

        return item;
    }

    public override void Write( Utf8JsonWriter aWriter, Point aValue, JsonSerializerOptions aOptions )
    {
        aWriter.WriteStartObject();
        aWriter.WritePropertyName( "X" );
        aWriter.WriteNumberValue( aValue.X );
        aWriter.WritePropertyName( "Y" );
        aWriter.WriteNumberValue( aValue.Y );
        aWriter.WriteEndObject();
    }
}
