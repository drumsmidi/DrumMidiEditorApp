using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DrumMidiEditor.pIO.pScore.pTechMania;

#region Enums

[Serializable]
internal enum ControlScheme
{
    Touch = 0,
    Keys = 1,
    KM = 2
}

[Serializable]
internal enum NoteType
{
    Basic,
    ChainHead,
    ChainNode,
    Hold,
    Drag,
    RepeatHead,
    RepeatHeadHold,
    Repeat,
    RepeatHold
}

[Serializable]
internal enum CurveType
{
    Bezier = 0,
    BSpline = 1
}

#endregion

internal partial class Track
{
    [JsonInclude]
    public string version = "3";

    [JsonInclude]
    public TrackMetadata trackMetadata = new();

    [JsonInclude]
    public List<Pattern> patterns = new();
}

internal class TrackMetadata
{
    [JsonInclude]
    public string guid = Guid.NewGuid().ToString();

    [JsonInclude]
    public string title = String.Empty;

    [JsonInclude]
    public string artist = String.Empty;

    [JsonInclude]
    public string genre = String.Empty;

    [JsonInclude]
    public string additionalCredits = String.Empty;

    [JsonInclude]
    public string eyecatchImage = String.Empty;

    [JsonInclude]
    public string previewTrack = String.Empty;

    [JsonInclude]
    public double previewStartTime = 0;

    [JsonInclude]
    public double previewEndTime = 0;

    [JsonInclude]
    public bool autoOrderPatterns = true;
}

[Serializable]
internal partial class Pattern
{
    [JsonInclude]
    public PatternMetadata patternMetadata = new();

    [JsonInclude]
    public LegacyRulesetOverride legacyRulesetOverride = new();

    [JsonInclude]
    public List<BpmEvent> bpmEvents = new();

    [JsonInclude]
    public List<TimeStop> timeStops = new();

    [JsonInclude]
    public List<string> packedNotes = new();

    [JsonInclude]
    public List<string> packedHoldNotes = new();

    [JsonInclude]
    public List<PackedDragNote> packedDragNotes = new();

    public void AddHoldNote( HoldNote aNote ) => packedHoldNotes.Add( aNote.Pack() );

    public void AddDragNote( DragNote aNote ) => packedDragNotes.Add( aNote.Pack() );

    public void AddNote( Note aNote ) => packedNotes.Add( aNote.Pack() );
}

#region TimeEvent

internal class TimeEvent
{
    [JsonInclude]
    public int pulse = 0;

    [JsonIgnore]
    public float time = 0;
}

internal class BpmEvent : TimeEvent
{
    [JsonInclude]
    public double bpm = 0;
}

internal class TimeStop : TimeEvent
{
    [JsonInclude]
    public int duration = 0;

    [JsonIgnore]
    public float endTime = 0;

    [JsonIgnore]
    public double bpmAtStart = 0;
}

#endregion

#region Note

internal class Note
{
    public const int minVolume = 0;
    public const int defaultVolume = 100;
    public const int maxVolume = 100;
    public const int minPan = -100;
    public const int defaultPan = 0;
    public const int maxPan = 100;

    // Calculated at unpack time:

    public NoteType type = NoteType.Basic;

    public int pulse = 0;

    public int lane = 0;

    public string sound = String.Empty;  // Filename with extension, no folder

    // Optional parameters:

    public int volumePercent = defaultVolume;

    public int panPercent = defaultPan;

    public bool endOfScan = false;

    protected string EndOfScanString => endOfScan ? "1" : "0";

    public virtual bool IsExtended()
        => ( volumePercent != defaultVolume || panPercent != defaultPan || endOfScan );

    public virtual string Pack()
        => IsExtended()
            ? $"E|{type}|{pulse}|{lane}|{volumePercent}|{panPercent}|{EndOfScanString}|{sound}"
            : $"{type}|{pulse}|{lane}|{sound}" ;
}

internal class HoldNote : Note
{
    public int duration = 0;  // In pulses.

    public override string Pack()
        => IsExtended()
            ? $"E|{type}|{pulse}|{lane}|{duration}|{volumePercent}|{panPercent}|{EndOfScanString}|{sound}"
            : $"{type}|{pulse}|{lane}|{duration}|{sound}" ;
}

internal class DragNote : Note
{
    public CurveType curveType = CurveType.Bezier;

    /// <summary>
    /// There must be at least 2 nodes, with nodes[0]
    /// describing the note head.
    /// controlBefore of the first node and controlAfter of the last node are ignored.
    /// </summary>
    public List<DragNode> nodes = new();

    public override bool IsExtended()
        => ( volumePercent != defaultVolume || panPercent != defaultPan || curveType != CurveType.Bezier ); 

    public new PackedDragNote Pack()
    {
        var packed = new PackedDragNote
        {
            packedNote = IsExtended()
                ? $"E|{type}|{pulse}|{lane}|{volumePercent}|{panPercent}|{(int)curveType}|{sound}"
                : $"{type}|{pulse}|{lane}|{sound}"
        };

        foreach ( var node in nodes )
        {
            packed.packedNodes.Add( node.Pack() );
        }
        return packed;
    }
}

internal class FloatPoint
{
    public float lane;
    public float pulse;

    public FloatPoint( float aPulse, float aLane )
    {
        this.pulse  = aPulse;
        this.lane   = aLane;
    }

    public static FloatPoint operator +( FloatPoint aLeft, FloatPoint aRight )
        => new( aLeft.pulse + aRight.pulse, aLeft.lane + aRight.lane );

    public static FloatPoint operator *( float aCoeff, FloatPoint aPoint )
        => new( aCoeff * aPoint.pulse, aCoeff * aPoint.lane );

    public static FloatPoint operator /( FloatPoint aPoint, float aCoeff )
        => new( aPoint.pulse / aCoeff, aPoint.lane / aCoeff );
}

internal class DragNode
{
    public FloatPoint anchor = new( 0f, 0f );

    public FloatPoint controlLeft = new( 0f, 0f );

    public FloatPoint controlRight = new( 0f, 0f );

    public string Pack()
        => $"{anchor.pulse}|{anchor.lane}|{controlLeft.pulse}|{controlLeft.lane}|{controlRight.pulse}|{controlRight.lane}";
}

internal class PackedDragNote
{
    public string packedNote = String.Empty;

    public List<string> packedNodes = new();
}

#endregion

internal class PatternMetadata
{
    [JsonInclude]
    public string guid = Guid.NewGuid().ToString();

    // Basics.

    [JsonInclude]
    public string patternName = "New pattern";

    [JsonInclude]
    public int level = 1;

    [JsonInclude]
    public ControlScheme controlScheme = ControlScheme.Touch;

    [JsonInclude]
    public int playableLanes = 4;

    [JsonInclude]
    public string author = String.Empty;

    // Background AV.

    // The backing track played in game.
    // This always plays from the beginning.
    // If no keysounds, this should be the entire track.
    [JsonInclude]
    public string backingTrack = String.Empty;

    // Filename of background image, used in loading screen.
    [JsonInclude]
    public string backImage = String.Empty;

    // Filename of background animation (BGA).
    // If empty, will show background image.
    [JsonInclude]
    public string bga = String.Empty;

    // Play BGA this many seconds after the backing track begins.
    [JsonInclude]
    public double bgaOffset = 0;

    // Take BGA into account when calculating pattern length.
    [JsonInclude]
    public bool waitForEndOfBga = true;

    // If true, game will not wait for BGA regardless of
    // waitForEndOfBga's value.
    [JsonInclude]
    public bool playBgaOnLoop = false;

    // Timing.

    // Beat 0 starts at this time.
    [JsonInclude]
    public double firstBeatOffset = 0;

    // These can be changed by events.
    [JsonInclude]
    public double initBpm = 60;

    // BPS: beats per scan.
    [JsonInclude]
    public int bps = 4;
}

internal class LegacyRulesetOverride
{
    [JsonInclude]
    public List<float> timeWindows = new();
    [JsonInclude]
    public List<int> hpDeltaBasic = new();
    [JsonInclude]
    public List<int> hpDeltaChain = new();
    [JsonInclude]
    public List<int> hpDeltaHold = new();
    [JsonInclude]
    public List<int> hpDeltaDrag = new();
    [JsonInclude]
    public List<int> hpDeltaRepeat = new();
    [JsonInclude]
    public List<int> hpDeltaBasicDuringFever = new();
    [JsonInclude]
    public List<int> hpDeltaChainDuringFever = new();
    [JsonInclude]
    public List<int> hpDeltaHoldDuringFever = new();
    [JsonInclude]
    public List<int> hpDeltaDragDuringFever = new();
    [JsonInclude]
    public List<int> hpDeltaRepeatDuringFever = new();
}
