using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pIO.pScore.p0940;

/// <summary>
/// Score入出力
/// </summary>
internal class ScoreStream : IScoreReader
{
	private readonly string TAG_DMS				= "DMS";

    private readonly string TAG_SCORE_INFO      = "INFO";
    private readonly string TAG_SCORE_FILEPATH	= "FILEPATH";
    private readonly string TAG_SCORE_RPP		= "RPP";
    private readonly string TAG_SCORE_VOLUME	= "VOLUME";
    private readonly string TAG_SCORE_BASEBPM	= "BASEBPM";
    private readonly string TAG_SCORE			= "SCORE";
    private readonly string TAG_SCORE_NOTES		= "NOTES";
    private readonly string TAG_SCORE_BPM		= "BPM";
    private readonly string ATR_SCORE_KEY		= "KEY";
    private readonly string ATR_SCORE_MEASURE	= "MEASURE";

	private readonly string TAG_MIDIMAPSET		= "DRUMSET";
	private readonly string TAG_MIDIMAPGROUP	= "DRUMGROUP";
	private readonly string TAG_MIDIMAP			= "DRUM";
	private readonly string TAG_PLAYER			= "PLAYER";

//	private readonly string ATR_VERSION			= "VERSION";
	private readonly string ATR_DISPLAY			= "DISPLAY";
	private readonly string ATR_KEY				= "KEY";
	private readonly string ATR_NAME			= "NAME";
	private readonly string ATR_VOLUME			= "VOLUME";
	private readonly string ATR_MIDI			= "MIDI";
	private readonly string ATR_COLOR			= "COLOR";
	private readonly string ATR_MODE			= "MODE";
	private readonly string ATR_X				= "X";
	private readonly string ATR_Y				= "Y";
	private readonly string ATR_SCALE			= "SCALE";

	#region Reader

	public bool Validation( GeneralPath aGeneralPath )
        {
		var errorFlag = false;

		try
		{
			string xsdMarkup =
				$"<?xml version='1.0' encoding='UTF-8' ?>" +
				$"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
					$"<xsd:element name='{TAG_DMS}' type='{TAG_DMS}_Type' />" +
					$"<xsd:complexType name='{TAG_DMS}_Type'>" +
						$"<xsd:sequence>" +
							$"<xsd:element name='{TAG_MIDIMAPSET}' minOccurs='0' maxOccurs='1' />" +
							$"<xsd:element name='{TAG_SCORE}'      minOccurs='0' maxOccurs='1' />" +
						$"</xsd:sequence>" +
					$"</xsd:complexType>" +
                    $"</xsd:schema>";

			using var xsd_reader = new XmlTextReader( new StringReader( xsdMarkup ) );

			var setting = new XmlReaderSettings();
			setting.Schemas.Add( String.Empty, xsd_reader );
			setting.ValidationType = ValidationType.Schema;
			setting.ValidationEventHandler += new ValidationEventHandler
				( 
					( sender, e ) => 
					{ 
						errorFlag = true;
						Log.Warning( $"Validation Error:{e.Message}" );
					} 
				);

			using var xml_reader = XmlReader.Create( aGeneralPath.AbsoulteFilePath, setting );

			while ( xml_reader.Read() ) { }
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

			errorFlag = true;
        }

		return !errorFlag;
	}

    public void Read( GeneralPath aGeneralPath, out Score aScore )
	{
		using var reader = XmlReader.Create( aGeneralPath.AbsoulteFilePath );

		ReadScore( reader, out aScore );
	}

	public void Read( GeneralPath aGeneralPath, out MidiMapSet aMidiMapSet )
	{
		using var reader = XmlReader.Create( aGeneralPath.AbsoulteFilePath );

		ReadScore( reader, out var score );

		aMidiMapSet = score.GetFirstMidiMapSet()?.Clone() ?? throw new Exception( $"Can't find MidiMapSet" ); ;
	}

    public void ReadScore( XmlReader aReader, out Score aScore )
    {
		aScore = new();

		int group_count = -1;

		while ( aReader.Read() )
		{
            if ( aReader.NodeType != XmlNodeType.Element )
			{
				continue;
			}

			#region <DRUMSET>
			if ( aReader.LocalName.Equals( TAG_MIDIMAPSET ) )
			{
				var channelNo = Config.Media.ChannelDrum;

				var midiMapSet = aScore.Channels[ channelNo ].MidiMapSet;

				if ( aReader.IsEmptyElement )
				{
					continue;
				}

				while ( aReader.Read() )
				{
					if ( aReader.NodeType == XmlNodeType.EndElement )
					{
						#region </DRUMSET>
						if ( aReader.LocalName.Equals( TAG_MIDIMAPSET ) )
						{
							break;
						}
						#endregion
					}
					else if ( aReader.NodeType == XmlNodeType.Element )
					{
						#region <DRUMGROUP>
						if ( aReader.LocalName.Equals( TAG_MIDIMAPGROUP ) )
						{
							var group = new MidiMapGroup
							{
								GroupKey	= Convert.ToInt32( aReader.GetAttribute( ATR_KEY )		?? String.Empty ),
								Display		= Convert.ToInt32( aReader.GetAttribute( ATR_DISPLAY )	?? String.Empty ) != 0,
								GroupName	=				   aReader.GetAttribute( ATR_NAME )		?? String.Empty,
								VolumeAdd	= Convert.ToInt32( aReader.GetAttribute( ATR_VOLUME )	?? String.Empty ),
							};
							midiMapSet.AddMidiMapGroup( group );

							group_count++;

							if ( aReader.IsEmptyElement )
							{
								continue;
							}

							while ( aReader.Read() )
							{
								if ( aReader.NodeType == XmlNodeType.EndElement )
								{
									#region </DRUMGROUP>
									if ( aReader.LocalName.Equals( TAG_MIDIMAPGROUP ) )
									{
										break;
									}
									#endregion
								}
								else if ( aReader.NodeType == XmlNodeType.Element )
								{
									#region <DRUM />
									if ( aReader.LocalName.Equals( TAG_MIDIMAP ) )
									{
										var midiMap = new MidiMap
										{
											MidiMapKey	= Convert.ToInt32		( aReader.GetAttribute( ATR_KEY )		?? String.Empty ),
											Display		= Convert.ToInt32		( aReader.GetAttribute( ATR_DISPLAY )	?? String.Empty ) != 0,
											MidiMapName =						  aReader.GetAttribute( ATR_NAME )		?? String.Empty,
											Midi		= (byte)Convert.ToInt32	( aReader.GetAttribute( ATR_MIDI )		?? String.Empty ),
											VolumeAdd	= Convert.ToInt32		( aReader.GetAttribute( ATR_VOLUME )	?? String.Empty ),
											Color		= StringUtil.GetColor	( aReader.GetAttribute( ATR_COLOR )		?? String.Empty )
										};

										midiMapSet.AddMidiMap( group_count, midiMap );
									}
									#endregion
								}
							}
						}
						#endregion
						#region <PLAYER>
						else if ( aReader.LocalName.Equals( TAG_PLAYER ) )
						{
							var mode = (ConfigPlayer.PlayerSurfaceMode)int.Parse( aReader.GetAttribute( ATR_MODE ) ?? "0" );

							if ( aReader.IsEmptyElement )
							{
								continue;
							}

							while ( aReader.Read() )
							{
								if ( aReader.NodeType == XmlNodeType.EndElement )
								{ 
									#region </PLAYER>
									if ( aReader.LocalName.Equals( TAG_PLAYER ) )
									{
										break;
									}
									#endregion
								}
								else if ( aReader.NodeType == XmlNodeType.Element )
								{
									#region <DRUMGROUP />
									if ( aReader.LocalName.Equals( TAG_MIDIMAPGROUP ) )
									{
										var group_key = Convert.ToInt32( aReader.GetAttribute( ATR_KEY ) ?? String.Empty );

										var dgp = new MidiMapGroupPosition
										{
											X				= Convert.ToInt32( aReader.GetAttribute( ATR_X )		?? String.Empty ),
											Y				= Convert.ToInt32( aReader.GetAttribute( ATR_Y )		?? String.Empty ),
											Magnification	= Convert.ToInt32( aReader.GetAttribute( ATR_SCALE )	?? String.Empty ),
										};

										midiMapSet.AddMidiMapGroupPosition( mode, group_key, dgp );
									}
									#endregion
								}
							}
						}
						#endregion
					}
				}
			}
			#endregion
			#region <INFO />
			else if ( aReader.LocalName.Equals( TAG_SCORE_INFO ) )
			{
				aScore.Info = aReader.ReadString().Replace( "\n", "\r\n" );
			}
			#endregion
			#region <FILEPATH />
			else if ( aReader.LocalName.Equals( TAG_SCORE_FILEPATH ) )
			{
				aScore.BgmFilePath = new( aReader.ReadString() );
			}
			#endregion
			#region <RPP />
			else if ( aReader.LocalName.Equals( TAG_SCORE_RPP ) )
			{
				aScore.BgmReproducePosition = aReader.ReadElementContentAsDouble();
			}
			#endregion
			#region <BASEBPM />
			else if ( aReader.LocalName.Equals( TAG_SCORE_BASEBPM ) )
			{
				aScore.Bpm = aReader.ReadElementContentAsDouble();
			}
			#endregion
			#region <VOLUME />
			else if ( aReader.LocalName.Equals( TAG_SCORE_VOLUME ) )
			{
				aScore.BgmVolume = aReader.ReadElementContentAsInt();
			}
			#endregion
			#region <BPM></BPM>
			else if ( aReader.LocalName.Equals( TAG_SCORE_BPM ) )
			{
				var measure_no  = Convert.ToInt32 ( aReader.GetAttribute( ATR_SCORE_MEASURE	), 10 );
				var line        = aReader.ReadString();
				var note_cnt    = line.Length / 4;

				int bpm;

				for ( int i = 0; i < note_cnt; i++ )
				{
					bpm = Convert.ToInt32( line.Substring( i * 4, 4 ), 16 );

					if ( bpm <= 0 )
					{
						continue;
					}

					aScore.SysChannel.AddBpm
						( 
							measure_no, 
							Config.System.MeasureNoteNumber * i / note_cnt, 
							bpm / 100 + bpm % 100 / 100.0D, 
							false 
						);
				}
			}
			#endregion
			#region <NOTES></NOTES>
			else if ( aReader.LocalName.Equals( TAG_SCORE_NOTES ) )
			{
				var channelNo	= Config.Media.ChannelDrum;
				var midiMapKey	= Convert.ToInt32( aReader.GetAttribute( ATR_SCORE_KEY	), 10 );
				var measure_no  = Convert.ToInt32( aReader.GetAttribute( ATR_SCORE_MEASURE	), 10 );
				var line		= aReader.ReadString();
				var note_cnt	= line.Length / 2;

				int volume;

				string val;

				for ( int i = 0; i < note_cnt; i++ )
				{
					val = line.Substring( i * 2, 2 );

					if ( val.Equals("##") )
					{
						volume = 0;
					}
					else
					{
						volume = Convert.ToInt32( val, 16 );

						if ( volume == 0 )
						{
							continue;
						}
					}

					aScore.Channels[ channelNo ]?
						.AddNote
							( 
								midiMapKey, 
								measure_no,
								Config.System.MeasureNoteNumber * i / note_cnt, 
								volume, 
								true,
								false,
								false
							);
				}
			}
			#endregion
		}

		aScore.UpdateInfoAllMidiMaps();
	}

	#endregion
}
