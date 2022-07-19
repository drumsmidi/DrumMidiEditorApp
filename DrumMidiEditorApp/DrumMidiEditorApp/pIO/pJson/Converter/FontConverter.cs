using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DrumMidiEditorApp.pIO.pJson;

/// <summary>
/// Json converter : Font
/// </summary>
internal class FontConverter : JsonConverter<Font>
{
    public override bool CanConvert( Type aTypeToConvert ) 
        => typeof( Font ).IsAssignableFrom( aTypeToConvert );

    public override Font Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var font = new Font( FontFamily.GenericSerif, 8F );

        while ( aReader.Read() && aReader.TokenType != JsonTokenType.EndObject )
        {
            if ( aReader.TokenType == JsonTokenType.PropertyName )
            {
                var prop = aReader.GetString();

                aReader.Read();

                switch ( prop )
                {
                    case "Name":
                        font = new( aReader.GetString() ?? String.Empty, font.Size );
                        break;
                    case "Size":
                        font = new( font.OriginalFontName ?? String.Empty, (float)aReader.GetDouble() );
                        break;
                }
            }
        }

        return font;
    }

    public override void Write( Utf8JsonWriter aWriter, Font aValue, JsonSerializerOptions aOptions )
    {
        aWriter.WriteStartObject();
        aWriter.WritePropertyName( "Name" );
        aWriter.WriteStringValue( aValue.Name );
        aWriter.WritePropertyName( "Size" );
        aWriter.WriteNumberValue( aValue.Size );
        aWriter.WriteEndObject();
    }
}
