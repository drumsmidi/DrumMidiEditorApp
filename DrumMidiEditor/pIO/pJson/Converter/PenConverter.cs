using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DrumMidiEditor.pIO.pJson;

/// <summary>
/// Json converter : Pen
/// </summary>
internal class PenConverter : JsonConverter<Pen>
{
    public override bool CanConvert( Type aTypeToConvert ) 
        => typeof( Pen ).IsAssignableFrom( aTypeToConvert );

    public override Pen Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var pen = new Pen( Brushes.White );

        while ( aReader.Read() && aReader.TokenType != JsonTokenType.EndObject )
        {
            if ( aReader.TokenType == JsonTokenType.PropertyName )
            {
                var prop = aReader.GetString();

                aReader.Read();

                switch ( prop )
                {
                    case "ColorArgb":
                        pen.Color = Color.FromArgb( aReader.GetInt32() );
                        break;
                    case "Width":
                        pen.Width = aReader.GetSingle();
                        break;
                }
            }
        }

        return pen;
    }

    public override void Write( Utf8JsonWriter aWriter, Pen aValue, JsonSerializerOptions aOptions )
    {
        aWriter.WriteStartObject();
        aWriter.WritePropertyName( "ColorArgb" );
        aWriter.WriteNumberValue( aValue.Color.ToArgb() );
        aWriter.WritePropertyName( "Width" );
        aWriter.WriteNumberValue( aValue.Width );
        aWriter.WriteEndObject();
    }
}
