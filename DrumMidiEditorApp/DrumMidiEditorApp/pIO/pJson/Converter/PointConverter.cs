using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Foundation;

namespace DrumMidiEditorApp.pIO.pJson;

/// <summary>
/// Json converter : Point
/// </summary>
internal class PointConverter : JsonConverter<Point>
{
    public override bool CanConvert( Type aTypeToConvert ) 
        => typeof( Point ).IsAssignableFrom( aTypeToConvert );

    public override Point Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var item = new Point();

        while ( aReader.Read() && aReader.TokenType != JsonTokenType.EndObject )
        {
            if ( aReader.TokenType == JsonTokenType.PropertyName )
            {
                var prop = aReader.GetString();

                aReader.Read();

                switch ( prop )
                {
                    case "X":
                        item.X = aReader.GetSingle();
                        break;
                    case "Y":
                        item.Y = aReader.GetSingle();
                        break;
                }
            }
        }

        return item;
    }

    public override void Write( Utf8JsonWriter aWriter, Point aValue, JsonSerializerOptions aOptions )
    {
        aWriter.WriteStartObject();
        aWriter.WritePropertyName( "X" );
        aWriter.WriteStringValue( $"{aValue.X}" );
        aWriter.WritePropertyName( "Y" );
        aWriter.WriteStringValue( $"{aValue.Y}" );
        aWriter.WriteEndObject();
    }
}
