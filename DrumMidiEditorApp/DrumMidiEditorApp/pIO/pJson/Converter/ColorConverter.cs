﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using DrumMidiEditorApp.pUtil;
using Windows.UI;

namespace DrumMidiEditorApp.pIO.pJson;

/// <summary>
/// Json converter : Color
/// </summary>
internal class ColorConverter : JsonConverter<Color>
{
    public override bool CanConvert( Type aTypeToConvert )
        => typeof( Color ).IsAssignableFrom( aTypeToConvert );

    public override Color Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var c = Color.FromArgb( 255, 255, 255, 255 );

        while ( aReader.Read() && aReader.TokenType != JsonTokenType.EndObject )
        {
            if ( aReader.TokenType == JsonTokenType.PropertyName )
            {
                var prop = aReader.GetString();

                _ = aReader.Read();

                switch ( prop )
                {
                    case "ColorArgb":
                        c = HelperColor.GetColor( aReader.GetString() ?? string.Empty );
                        break;
                }
            }
        }

        return c;
    }

    public override void Write( Utf8JsonWriter aWriter, Color aValue, JsonSerializerOptions aOptions )
    {
        aWriter.WriteStartObject();
        aWriter.WritePropertyName( "ColorArgb" );
        aWriter.WriteStringValue( HelperColor.GetColor( aValue ) );
        aWriter.WriteEndObject();
    }
}
