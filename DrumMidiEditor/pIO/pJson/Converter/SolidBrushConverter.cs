using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DrumMidiEditor.pIO.pJson;

/// <summary>
/// Json converter : SolidBrush
/// </summary>
internal class SolidBrushConverter : JsonConverter<SolidBrush>
{
    public override bool CanConvert( Type aTypeToConvert ) 
        => typeof( SolidBrush ).IsAssignableFrom( aTypeToConvert );

    public override SolidBrush Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var brush = new SolidBrush( Color.White );

        while ( aReader.Read() && aReader.TokenType != JsonTokenType.EndObject )
        {
            if ( aReader.TokenType == JsonTokenType.PropertyName )
            {
                var prop = aReader.GetString();

                aReader.Read();

                switch ( prop )
                {
                    case "ColorArgb":
						brush.Color = Color.FromArgb( aReader.GetInt32() );
                        break;
                }
            }
        }

        return brush;
    }

    public override void Write( Utf8JsonWriter aWriter, SolidBrush aValue, JsonSerializerOptions aOptions )
    {
        aWriter.WriteStartObject();
        aWriter.WritePropertyName( "ColorArgb" );
        aWriter.WriteNumberValue( aValue.Color.ToArgb() );
        aWriter.WriteEndObject();
    }
}
