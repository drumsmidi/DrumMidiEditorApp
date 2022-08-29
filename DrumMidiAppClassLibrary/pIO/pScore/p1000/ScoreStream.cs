using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

using DrumMidiClassLibrary.pConfig;
using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pModel;
using DrumMidiClassLibrary.pUtil;
using DrumMidiClassLibrary.pWinUI;

namespace DrumMidiClassLibrary.pIO.pScore.p1000;

/// <summary>
/// Score入出力
/// </summary>
internal class ScoreStream : IScoreReader, IScoreWriter
{
	private const string TAG_DMS				= "DMS";
	private const string ATR_VERSION			= "VERSION";
	private const string VAL_VERSION			= "1.0";

	private const string TAG_MIDIMAPSET			= "MIDIMAPSET";
	private const string TAG_MIDIMAPGROUP		= "MIDIMAPGROUP";
	private const string TAG_MIDIMAP			= "MIDIMAP";
	private const string TAG_PLAYER				= "PLAYER";
	private const string TAG_PLAYERGROUP		= "PLAYERGROUP";

	private const string ATR_CHANNELNO			= "CHANNEL";
	private const string ATR_DISPLAY			= "DISPLAY";
	private const string ATR_KEY				= "KEY";
	private const string ATR_NAME				= "NAME";
	private const string ATR_VOLUME				= "VOLUME";
	private const string ATR_MIDI				= "MIDI";
	private const string ATR_COLOR				= "COLOR";
	private const string ATR_SCALE				= "SCALE";
	private const string ATR_MODE				= "MODE";
	private const string ATR_X					= "X";
	private const string ATR_Y					= "Y";
	private const string ATR_MAGNIFICATION		= "MAGNIFICATION";

	private const string TAG_SCORE				= "SCORE";
    private const string TAG_SCORE_INFO         = "INFO";
    private const string TAG_SCORE_FILEPATH		= "FILEPATH";
    private const string TAG_SCORE_RPP			= "RPP";
    private const string TAG_SCORE_VOLUME		= "VOLUME";
    private const string TAG_SCORE_BASEBPM		= "BASEBPM";
    private const string TAG_SCORE_BPM			= "BPM";
    private const string TAG_SCORE_NOTE			= "NOTE";

    private const string ATR_SCORE_CHANNELNO    = "C";
    private const string ATR_SCORE_MIDIMAPKEY	= "K";
    private const string ATR_SCORE_MEASURENO	= "M";
    private const string ATR_SCORE_NOTEPOS		= "N";
    private const string ATR_SCORE_NOTEVOLUME	= "V";
    private const string ATR_SCORE_NOTEONOFF	= "F";
    private const string ATR_SCORE_BPM			= "V";

	#region Reader

	public bool Validation( GeneralPath aGeneralPath )
    {
		var errorFlag = false;

		try
		{
			var xsdMarkup =
				$"<?xml version='1.0' encoding='UTF-8' ?>" +
				$"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>" +
					$"<xsd:element name='{TAG_DMS}' type='{TAG_DMS}_Type' />" +
					$"<xsd:complexType name='{TAG_DMS}_Type'>" +
						$"<xsd:sequence>" +
							$"<xsd:element name='{TAG_MIDIMAPSET}' type='{TAG_MIDIMAPSET}_Type' minOccurs='1' maxOccurs='unbounded' />" +
							$"<xsd:element name='{TAG_SCORE}'      type='{TAG_SCORE}_Type'      minOccurs='0' maxOccurs='1' />" +
						$"</xsd:sequence>" +
						$"<xsd:attribute name='{ATR_VERSION}' type='xsd:string' use='required' fixed='{VAL_VERSION}' />" +
					$"</xsd:complexType>" +
                    $"<xsd:complexType name='{TAG_MIDIMAPSET}_Type'>" +
                        $"<xsd:sequence>" +
                            $"<xsd:element name='{TAG_MIDIMAPGROUP}' type='{TAG_MIDIMAPGROUP}_Type' minOccurs='0' maxOccurs='{Config.System.MidiMapGroupMaxNumber}' />" +
							$"<xsd:element name='{TAG_PLAYER}'       type='{TAG_PLAYER}_Type'       minOccurs='0' maxOccurs='unbounded' />" +
						$"</xsd:sequence>" +
                        $"<xsd:attribute name='{ATR_CHANNELNO}' type='xsd:int' use='required' />" +
                    $"</xsd:complexType>" +
                    $"<xsd:complexType name='{TAG_MIDIMAPGROUP}_Type'>" +
                        $"<xsd:sequence>" +
                            $"<xsd:element name='{TAG_MIDIMAP}' type='{TAG_MIDIMAP}_Type' minOccurs='1' maxOccurs='{Config.System.MidiMapMaxNumber}' />" +
						$"</xsd:sequence>" +
						$"<xsd:attribute name='{ATR_DISPLAY}' type='xsd:int'    use='required' />" +
						$"<xsd:attribute name='{ATR_KEY}'     type='xsd:int'    use='required' />" +
						$"<xsd:attribute name='{ATR_NAME}'    type='xsd:string' use='required' />" +
						$"<xsd:attribute name='{ATR_VOLUME}'  type='xsd:int'    use='required' />" +
					$"</xsd:complexType>" +
                    $"<xsd:complexType name='{TAG_MIDIMAP}_Type'>" +
                        $"<xsd:attribute name='{ATR_DISPLAY}' type='xsd:int'    use='required' />" +
                        $"<xsd:attribute name='{ATR_KEY}'     type='xsd:int'    use='required' />" +
                        $"<xsd:attribute name='{ATR_NAME}'    type='xsd:string' use='required' />" +
                        $"<xsd:attribute name='{ATR_VOLUME}'  type='xsd:int'    use='required' />" +
                        $"<xsd:attribute name='{ATR_MIDI}'    type='xsd:int'    use='required' />" +
                        $"<xsd:attribute name='{ATR_COLOR}'   type='xsd:string' use='required' />" +
						$"<xsd:attribute name='{ATR_SCALE}'   type='xsd:string' use='optional' />" +
					$"</xsd:complexType>" +
                    $"<xsd:complexType name='{TAG_PLAYER}_Type'>" +
                        $"<xsd:sequence>" +
                            $"<xsd:element name='{TAG_PLAYERGROUP}' type='{TAG_PLAYERGROUP}_Type' minOccurs='0' maxOccurs='unbounded' />" +
                        $"</xsd:sequence>" +
                        $"<xsd:attribute name='{ATR_MODE}' type='xsd:string' use='required' />" +
                    $"</xsd:complexType>" +
                    $"<xsd:complexType name='{TAG_PLAYERGROUP}_Type'>" +
                        $"<xsd:attribute name='{ATR_KEY}'			type='xsd:int'    use='required' />" +
                        $"<xsd:attribute name='{ATR_X}'				type='xsd:double' use='required' />" +
                        $"<xsd:attribute name='{ATR_Y}'				type='xsd:double' use='required' />" +
                        $"<xsd:attribute name='{ATR_MAGNIFICATION}' type='xsd:double' use='required' />" +
                    $"</xsd:complexType>" +
                    $"<xsd:complexType name='{TAG_SCORE}_Type'>" +
                        $"<xsd:sequence>" +
                            $"<xsd:element name='{TAG_SCORE_INFO}'     type='xsd:string' minOccurs='0' maxOccurs='1' />" +
                            $"<xsd:element name='{TAG_SCORE_FILEPATH}' type='xsd:string' minOccurs='0' maxOccurs='1' />" +
                            $"<xsd:element name='{TAG_SCORE_RPP}'      type='xsd:double' minOccurs='0' maxOccurs='1' />" +
							$"<xsd:element name='{TAG_SCORE_BASEBPM}'  type='xsd:double' minOccurs='0' maxOccurs='1' />" +
							$"<xsd:element name='{TAG_SCORE_VOLUME}'   type='xsd:int'    minOccurs='0' maxOccurs='1' />" +
                            $"<xsd:element name='{TAG_SCORE_BPM}'      type='{TAG_SCORE_BPM}_Type'  minOccurs='0' maxOccurs='unbounded' />" +
                            $"<xsd:element name='{TAG_SCORE_NOTE}'     type='{TAG_SCORE_NOTE}_Type' minOccurs='0' maxOccurs='unbounded' />" +
                        $"</xsd:sequence>" +
                    $"</xsd:complexType>" +
                    $"<xsd:complexType name='{TAG_SCORE_BPM}_Type'>" +
                        $"<xsd:attribute name='{ATR_SCORE_MEASURENO}' type='xsd:int'    use='required' />" +
                        $"<xsd:attribute name='{ATR_SCORE_NOTEPOS}'   type='xsd:int'    use='required' />" +
                        $"<xsd:attribute name='{ATR_SCORE_BPM}'       type='xsd:double' use='required' />" +
                    $"</xsd:complexType>" +
                    $"<xsd:complexType name='{TAG_SCORE_NOTE}_Type'>" +
                        $"<xsd:attribute name='{ATR_SCORE_CHANNELNO}'  type='xsd:int' use='required' />" +
                        $"<xsd:attribute name='{ATR_SCORE_MIDIMAPKEY}' type='xsd:int' use='required' />" +
                        $"<xsd:attribute name='{ATR_SCORE_MEASURENO}'  type='xsd:int' use='required' />" +
                        $"<xsd:attribute name='{ATR_SCORE_NOTEPOS}'    type='xsd:int' use='required' />" +
                        $"<xsd:attribute name='{ATR_SCORE_NOTEVOLUME}' type='xsd:int' use='required' />" +
                        $"<xsd:attribute name='{ATR_SCORE_NOTEONOFF}'  type='xsd:int' use='required' />" +
                    $"</xsd:complexType>" +
				$"</xsd:schema>";

			using var xsd_reader = new XmlTextReader( new StringReader( xsdMarkup ) );

			var setting = new XmlReaderSettings();
			setting.Schemas.Add( string.Empty, xsd_reader );
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

    public static void ReadScore( XmlReader aReader, out Score aScore )
    {
		aScore = new();

		var group_count = -1;

		while ( aReader.Read() )
		{
            if ( aReader.NodeType != XmlNodeType.Element )
			{
				continue;
			}

			#region <MIDIMAPSET>
			if ( aReader.LocalName.Equals( TAG_MIDIMAPSET ) )
			{
				var channelNo = (byte)Convert.ToInt32( aReader.GetAttribute( ATR_CHANNELNO ) ?? "0" );

				var midiMapSet = aScore.Channels[ channelNo ].MidiMapSet;

				if ( aReader.IsEmptyElement )
				{
					continue;
				}

				while ( aReader.Read() )
				{
					if ( aReader.NodeType == XmlNodeType.EndElement )
					{ 
						#region </MIDIMAPSET>
						if ( aReader.LocalName.Equals( TAG_MIDIMAPSET ) )
						{
							break;
						}
						#endregion
					}
					else if ( aReader.NodeType == XmlNodeType.Element )
					{ 
						#region <MIDIMAPGROUP>
						if ( aReader.LocalName.Equals( TAG_MIDIMAPGROUP ) )
						{
							var group = new MidiMapGroup
							{
								GroupKey	= Convert.ToInt32( aReader.GetAttribute( ATR_KEY ) ?? string.Empty ),
								Display		= Convert.ToInt32( aReader.GetAttribute( ATR_DISPLAY ) ?? string.Empty ) != 0,
								GroupName	= aReader.GetAttribute( ATR_NAME ) ?? string.Empty,
								VolumeAdd	= Convert.ToInt32( aReader.GetAttribute( ATR_VOLUME ) ?? string.Empty ),
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
									#region </MIDIMAPGROUP>
									if ( aReader.LocalName.Equals( TAG_MIDIMAPGROUP ) )
									{
										break;
									}
									#endregion
								}
								else if ( aReader.NodeType == XmlNodeType.Element )
								{
									#region <MIDIMAP/>
									if ( aReader.LocalName.Equals( TAG_MIDIMAP ) )
									{
										var midiMap = new MidiMap
										{
											MidiMapKey	= Convert.ToInt32( aReader.GetAttribute( ATR_KEY ) ?? string.Empty ),
											Display		= Convert.ToInt32( aReader.GetAttribute( ATR_DISPLAY ) ?? string.Empty ) != 0,
											MidiMapName = aReader.GetAttribute( ATR_NAME ) ?? string.Empty,
											Midi		= (byte)Convert.ToInt32( aReader.GetAttribute( ATR_MIDI ) ?? string.Empty ),
											VolumeAdd	= Convert.ToInt32( aReader.GetAttribute( ATR_VOLUME ) ?? string.Empty ),
											Color		= ColorHelper.GetColor( aReader.GetAttribute( ATR_COLOR ) ?? string.Empty ),
											Scale		= aReader.GetAttribute( ATR_SCALE ) ?? string.Empty,
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
							if ( !Enum.TryParse( typeof( PlayerSurfaceMode ), aReader.GetAttribute( ATR_MODE ), out var mode ) || mode == null )
                            {
								continue;
                            }

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
									#region <MIDIMAPGROUP/>
									if ( aReader.LocalName.Equals( TAG_MIDIMAPGROUP ) )
									{
										var group_key = Convert.ToInt32( aReader.GetAttribute( ATR_KEY ) ?? string.Empty );

										var dgp = new MidiMapGroupPosition
										{
											X				= Convert.ToSingle( aReader.GetAttribute( ATR_X ) ?? string.Empty ),
											Y				= Convert.ToSingle( aReader.GetAttribute( ATR_Y ) ?? string.Empty ),
											Magnification	= Convert.ToSingle( aReader.GetAttribute( ATR_MAGNIFICATION ) ?? string.Empty ),
										};

										midiMapSet.AddMidiMapGroupPosition( (PlayerSurfaceMode)mode, group_key, dgp );
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
			#region <INFO/>
			else if ( aReader.LocalName.Equals( TAG_SCORE_INFO ) )
			{
				aScore.Info = aReader.ReadString().Replace( "\n", "\r\n" );
			}
			#endregion
			#region <FILEPATH/>
			else if ( aReader.LocalName.Equals( TAG_SCORE_FILEPATH ) )
			{
				aScore.BgmFilePath = new( aReader.ReadString() );
			}
			#endregion
			#region <RPP/>
			else if ( aReader.LocalName.Equals( TAG_SCORE_RPP ) )
			{
				aScore.BgmPlaybackStartPosition = aReader.ReadElementContentAsDouble();
			}
			#endregion
			#region <BASEBPM/>
			else if ( aReader.LocalName.Equals( TAG_SCORE_BASEBPM ) )
			{
				aScore.Bpm = aReader.ReadElementContentAsDouble();
			}
			#endregion
			#region <VOLUME/>
			else if ( aReader.LocalName.Equals( TAG_SCORE_VOLUME ) )
			{
				aScore.BgmVolume = aReader.ReadElementContentAsInt();
			}
			#endregion
			#region <BPM/>
			else if ( aReader.LocalName.Equals( TAG_SCORE_BPM ) )
			{
				var measure_no  = Convert.ToInt32 ( aReader.GetAttribute( ATR_SCORE_MEASURENO	), 10 );
				var note_pos	= Convert.ToInt32 ( aReader.GetAttribute( ATR_SCORE_NOTEPOS		), 10 );
				var bpm			= Convert.ToDouble( aReader.GetAttribute( ATR_SCORE_BPM ) );

				aScore.SysChannel.AddBpm( measure_no, note_pos, bpm, false );
			}
			#endregion
			#region <NOTE/>
			else if ( aReader.LocalName.Equals( TAG_SCORE_NOTE ) )
			{
				var channelNo	= Convert.ToInt32( aReader.GetAttribute( ATR_SCORE_CHANNELNO	), 10 );
				var midiMapKey	= Convert.ToInt32( aReader.GetAttribute( ATR_SCORE_MIDIMAPKEY	), 10 );
				var measure_no  = Convert.ToInt32( aReader.GetAttribute( ATR_SCORE_MEASURENO	), 10 );
				var note_pos	= Convert.ToInt32( aReader.GetAttribute( ATR_SCORE_NOTEPOS		), 10 );
				var volume		= Convert.ToInt32( aReader.GetAttribute( ATR_SCORE_NOTEVOLUME	), 10 );
				var noteOnOff	= Convert.ToInt32( aReader.GetAttribute( ATR_SCORE_NOTEONOFF	), 10 );

				aScore.Channels[ (byte)channelNo ]?
					.AddNote
						( 
							midiMapKey, 
							measure_no, 
							note_pos, 
							volume, 
							noteOnOff == 3 || noteOnOff == 2,
							noteOnOff == 3 || noteOnOff == 1,
							false
						);
			}
			#endregion
		}

		aScore.UpdateInfoAllMidiMaps();
	}

	#endregion

	#region Writer

	/// <summary>
	/// Score+MidiMapSet保存
	/// </summary>
	/// <param name="aGeneralPath">出力ファイルパス</param>
	/// <param name="aScore">保存スコア</param>
	public void Write( GeneralPath aGeneralPath, Score aScore )
	{
        using var writer = new XmlTextWriter( aGeneralPath.AbsoulteFilePath, Encoding.UTF8 );

		writer.Formatting = Formatting.Indented;

		#region <DMS>

		writer.WriteStartElement( TAG_DMS );
		writer.WriteAttributeString( ATR_VERSION, VAL_VERSION );
		{
			foreach ( var channel in aScore.Channels.Values )
			{
                WriteMidiMapSet( writer, channel.MidiMapSet, channel.ChannelNo );
			}
            WriteScore( writer, aScore );
		}
		writer.WriteEndElement();

        #endregion
    }

	/// <summary>
	/// MidiMapSet保存
	/// </summary>
	/// <param name="aGeneralPath">出力ファイルパス</param>
	/// <param name="aMidiMapSet">保存MidiMapSet</param>
	public void Write( GeneralPath aGeneralPath, MidiMapSet aMidiMapSet )
	{
        using var writer = new XmlTextWriter( aGeneralPath.AbsoulteFilePath, Encoding.UTF8 );

		writer.Formatting = Formatting.Indented;

		#region <DMS>

		writer.WriteStartElement( TAG_DMS );
		writer.WriteAttributeString( ATR_VERSION, VAL_VERSION );
		{
			WriteMidiMapSet( writer, aMidiMapSet, 0 );
		}
		writer.WriteEndElement();

        #endregion
    }

	/// <summary>
	/// MidiMapセット保存
	/// </summary>
	/// <param name="aWriter">XMLライター</param>
	/// <param name="aMidiMapSet">保存MidiMapSet</param>
	/// <param name="aChannelNo">チェンネル番号(0-15)</param>
	private static void WriteMidiMapSet( XmlTextWriter aWriter, MidiMapSet aMidiMapSet, byte aChannelNo )
	{
		#region <MIDIMAPSET>
		aWriter.WriteStartElement( TAG_MIDIMAPSET );
		aWriter.WriteAttributeString( ATR_CHANNELNO, $"{aChannelNo}" );
		{
			foreach ( var group in aMidiMapSet.MidiMapGroups )
			{
				#region <MIDIMAPGROUP>
				aWriter.WriteStartElement( TAG_MIDIMAPGROUP );
				aWriter.WriteAttributeString( ATR_DISPLAY	, $"{( group.Display ? "1" : "0" )}" );
				aWriter.WriteAttributeString( ATR_KEY		, $"{group.GroupKey}"	);
				aWriter.WriteAttributeString( ATR_NAME		, $"{group.GroupName}"	);
				aWriter.WriteAttributeString( ATR_VOLUME	, $"{group.VolumeAdd}"	);
				{
					foreach ( var midiMap in group.MidiMaps )
					{
						#region <MIDIMAP />
						aWriter.WriteStartElement( TAG_MIDIMAP );
						aWriter.WriteAttributeString( ATR_DISPLAY	, $"{( midiMap.Display ? "1" : "0" )}" );
						aWriter.WriteAttributeString( ATR_KEY		, $"{midiMap.MidiMapKey}"	);
						aWriter.WriteAttributeString( ATR_NAME		, $"{midiMap.MidiMapName}"	);
						aWriter.WriteAttributeString( ATR_MIDI		, $"{midiMap.Midi}"			);
						aWriter.WriteAttributeString( ATR_VOLUME	, $"{midiMap.VolumeAdd}"	);
						aWriter.WriteAttributeString( ATR_COLOR		, $"{ColorHelper.GetColor( midiMap.Color )}" );
						aWriter.WriteAttributeString( ATR_SCALE		, $"{midiMap.Scale}"		);
						aWriter.WriteEndElement();
						#endregion
					}
				}
				aWriter.WriteEndElement();
				#endregion
			}

			foreach ( var group in aMidiMapSet.MidiMapGroupPositions )
			{
				#region <PLAYER>
				aWriter.WriteStartElement( TAG_PLAYER );
				aWriter.WriteAttributeString( ATR_MODE, $"{group.Key}" );
				{
					foreach ( var pos in group.Value )
					{
						#region <PLAYERGROUP />
						aWriter.WriteStartElement( TAG_PLAYERGROUP );
						aWriter.WriteAttributeString( ATR_KEY			, $"{pos.Key}"					);
						aWriter.WriteAttributeString( ATR_X				, $"{pos.Value.X}"				);
						aWriter.WriteAttributeString( ATR_Y				, $"{pos.Value.Y}"				);
						aWriter.WriteAttributeString( ATR_MAGNIFICATION	, $"{pos.Value.Magnification}"	);
						aWriter.WriteEndElement();
						#endregion
					}
				}
				aWriter.WriteEndElement();
				#endregion
			}
		}
		aWriter.WriteEndElement();
		#endregion
	}

	/// <summary>
	/// スコア保存
	/// </summary>
	/// <param name="aWriter">XMLライター</param>
	/// <param name="aScore">保存スコア</param>
	private static void WriteScore( XmlTextWriter aWriter, Score aScore )
    {
		aWriter.Formatting = Formatting.Indented; 

		#region <SCORE>

		aWriter.WriteStartElement( TAG_SCORE );
		{
			// <INFO />
			aWriter.WriteElementString( TAG_SCORE_INFO, $"{aScore.Info.Replace("\r\n", "\n")}" );

			// <FILEPATH />
			aWriter.WriteElementString( TAG_SCORE_FILEPATH, $"{aScore.BgmFilePath.RelativeFilePath}" );

			// <RPP />
			aWriter.WriteElementString( TAG_SCORE_RPP, $"{aScore.BgmPlaybackStartPosition}" );

			// <BASEBPM />
			aWriter.WriteElementString( TAG_SCORE_BASEBPM, $"{aScore.Bpm}" );

			// <VOLUME />
			aWriter.WriteElementString( TAG_SCORE_VOLUME, $"{aScore.BgmVolume}" );

            foreach ( var info in aScore.SysChannel.BpmInfoList.Values )
			{
				#region <BPM />
				aWriter.WriteStartElement( TAG_SCORE_BPM );
				aWriter.WriteAttributeString( ATR_SCORE_MEASURENO	, $"{info.MeasureNo}");
				aWriter.WriteAttributeString( ATR_SCORE_NOTEPOS		, $"{info.NotePos}"	);
				aWriter.WriteAttributeString( ATR_SCORE_BPM			, $"{info.Bpm}"		);
				aWriter.WriteEndElement();
				#endregion
			}

			foreach ( var channel in aScore.Channels.Values )
			{ 
				foreach ( var info in channel.NoteInfoList.Values )
				{
					var noteOnOff = 0;
					if ( info.NoteOn && info.NoteOff )
					{
						noteOnOff = 3;
					}
					else if ( info.NoteOn )
					{
						noteOnOff = 2;
					}
					else if ( info.NoteOff )
					{
						noteOnOff = 1;
					}

					if ( noteOnOff != 0 )
					{ 
						#region <NOTE/>
						aWriter.WriteStartElement( TAG_SCORE_NOTE );
						aWriter.WriteAttributeString( ATR_SCORE_CHANNELNO	, $"{channel.ChannelNo}");
						aWriter.WriteAttributeString( ATR_SCORE_MIDIMAPKEY	, $"{info.MidiMapKey}"	);
						aWriter.WriteAttributeString( ATR_SCORE_MEASURENO	, $"{info.MeasureNo}"	);
						aWriter.WriteAttributeString( ATR_SCORE_NOTEPOS		, $"{info.NotePos}"		);
						aWriter.WriteAttributeString( ATR_SCORE_NOTEVOLUME	, $"{info.Volume}"		);
						aWriter.WriteAttributeString( ATR_SCORE_NOTEONOFF	, $"{noteOnOff}"		);
						aWriter.WriteEndElement();
						#endregion
					}
				}
            }
        }
		aWriter.WriteEndElement();

		#endregion
	}

	#endregion
}
