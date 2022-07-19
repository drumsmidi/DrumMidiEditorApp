using System;
using System.Collections.Generic;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pIO.pJson;

namespace DrumMidiEditorApp.pIO.pScore.pTechMania;

/// <summary>
/// Score入出力
/// </summary>
internal class ScoreStream : IScoreWriter
{
	#region Writer

	public void Write( GeneralPath aGeneralPath, Score aScore )
	{
		// TODO: 解析しないとわからない

		var bps		= 16;
		var scale	= Config.System.MeasureNoteNumber / bps;

		var bpmEvents = new List<BpmEvent>();

        foreach ( var info in aScore.SysChannel.BpmInfoList.Values )
		{
            var bpmEvent = new BpmEvent
            {
                pulse	= ( info.MeasureNo * Config.System.MeasureNoteNumber + info.NotePos ) * scale,
                bpm		= info.Bpm
            };

            bpmEvents.Add( bpmEvent );
		}

		var noteEvents = new List<string>();

		foreach ( var info in aScore.EditChannel.NoteInfoList.Values )
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

			if ( noteOnOff == 2 )
			{ 
				var noteEvent = new Note()
                {
					type	= NoteType.Basic,
					pulse	= ( info.MeasureNo * Config.System.MeasureNoteNumber + info.NotePos ) * scale,
					lane	= aScore.EditMidiMapSet.GetDisplayMidiMapIndex( info.MidiMapKey ) % 4,
				};

				noteEvents.Add( noteEvent.Pack() );
			}
		}

		var track = new Track();
		track.trackMetadata.title = aScore.BgmFilePath.FileName;
		track.patterns.Add
			(
				new()
                {
					patternMetadata = new()
                    {
						author			= "Convert by DrumMidiEditor",
						playableLanes	= 4, // aScore.EditMidiMapSet.DisplayGroupCount,
						backingTrack	= aScore.BgmFilePath.AbsoulteFilePath,
						firstBeatOffset = -aScore.BgmPlaybackStartPosition,
						initBpm			= aScore.Bpm,
						bps				= bps,
					},
					bpmEvents	= bpmEvents,
					packedNotes = noteEvents,
				}
			);

		JsonIO.SaveFile<Track>( track, aGeneralPath );
	}

	public void Write( GeneralPath aGeneralPath, MidiMapSet aMidiMapSet ) { }

	#endregion
}
