using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

using DrumMidiEditor.pGeneralFunction.pDirectX;

namespace DrumMidiEditor.pIO.pJson;

/// <summary>
/// Json converter : GraphicItem
/// </summary>
internal class GrachicItemConverter : JsonConverter<GraphicItem>
{
    public override bool CanConvert( Type aTypeToConvert ) 
        => typeof( GraphicItem ).IsAssignableFrom( aTypeToConvert );

    public override GraphicItem Read( ref Utf8JsonReader aReader, Type aTypeToConvert, JsonSerializerOptions aOptions )
    {
        var item = new GraphicItem();

        while ( aReader.Read() && aReader.TokenType != JsonTokenType.EndObject )
        {
            if ( aReader.TokenType == JsonTokenType.PropertyName )
            {
                var prop = aReader.GetString();

                aReader.Read();

                switch ( prop )
                {
                    case "BackBrush":
                        { 
                            var obj = JsonSerializer.Deserialize<SolidBrush>( ref aReader, aOptions );

                            if ( obj != null )
                            {
                                item.BackBrush = obj;
                            }
                        }
                        break;
                    case "ForePen":
                        { 
                            var obj = JsonSerializer.Deserialize<Pen>( ref aReader, aOptions );

                            if ( obj != null )
                            {
                                item.ForePen = obj;
                            }
                        }
                        break;
                    case "Font":
                        { 
                            var obj = JsonSerializer.Deserialize<Font>( ref aReader, aOptions );

                            if ( obj != null )
                            {
                                item.Font = obj;
                            }
                        }
                        break;
                    case "FontBrush":
                        { 
                            var obj = JsonSerializer.Deserialize<SolidBrush>( ref aReader, aOptions );

                            if ( obj != null )
                            {
                                item.FontBrush = obj;
                            }
                        }
                        break;
                    case "FontFormat":
                        { 
                            var obj = JsonSerializer.Deserialize<StringFormat>( ref aReader, aOptions );

                            if ( obj != null )
                            {
                                item.FontFormat = obj;
                            }
                        }
                        break;
                }
            }
        }

        return item;
    }

    public override void Write( Utf8JsonWriter aWriter, GraphicItem aValue, JsonSerializerOptions aOptions )
    {
        aWriter.WriteStartObject();
        aWriter.WritePropertyName("BackBrush");
        JsonSerializer.Serialize( aWriter, aValue.BackBrush, aOptions );
        aWriter.WritePropertyName("ForePen");
        JsonSerializer.Serialize( aWriter, aValue.ForePen, aOptions );
        aWriter.WritePropertyName("Font");
        JsonSerializer.Serialize( aWriter, aValue.Font, aOptions );
        aWriter.WritePropertyName("FontBrush");
        JsonSerializer.Serialize( aWriter, aValue.FontBrush, aOptions );
        aWriter.WritePropertyName("FontFormat");
        JsonSerializer.Serialize( aWriter, aValue.FontFormat, aOptions );
        aWriter.WriteEndObject();
    }
}
