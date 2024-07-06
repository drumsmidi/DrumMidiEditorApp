using System.Runtime.InteropServices;

namespace NAudio.WinMM.MmeInterop;

/// <summary>
/// MmTime
/// http://msdn.microsoft.com/en-us/library/dd757347(v=VS.85).aspx
/// </summary>
[StructLayout( LayoutKind.Explicit )]
public struct MmTime
{
    /// <summary>
    /// Time in milliseconds.
    /// </summary>
    public const int TIME_MS = 0x0001;
    /// <summary>
    /// Number of waveform-audio samples.
    /// </summary>
    public const int TIME_SAMPLES = 0x0002;
    /// <summary>
    /// Current byte offset from beginning of the file.
    /// </summary>
    public const int TIME_BYTES = 0x0004;

    /// <summary>
    /// Time format.
    /// </summary>
    [FieldOffset(0)]
    public uint wType;
    /// <summary>
    /// Number of milliseconds. Used when wType is TIME_MS.
    /// </summary>
    [FieldOffset(4)]
    public uint ms;
    /// <summary>
    /// Number of samples. Used when wType is TIME_SAMPLES.
    /// </summary>
    [FieldOffset(4)]
    public uint sample;
    /// <summary>
    /// Byte count. Used when wType is TIME_BYTES.
    /// </summary>
    [FieldOffset(4)]
    public uint cb;
    /// <summary>
    /// Ticks in MIDI stream. Used when wType is TIME_TICKS.
    /// </summary>
    [FieldOffset(4)]
    public uint ticks;
    /// <summary>
    /// SMPTE time structure - hours. Used when wType is TIME_SMPTE.
    /// </summary>
    [FieldOffset(4)]
    public byte smpteHour;
    /// <summary>
    /// SMPTE time structure - minutes. Used when wType is TIME_SMPTE.
    /// </summary>
    [FieldOffset(5)]
    public byte smpteMin;
    /// <summary>
    /// SMPTE time structure - seconds. Used when wType is TIME_SMPTE.
    /// </summary>
    [FieldOffset(6)]
    public byte smpteSec;
    /// <summary>
    /// SMPTE time structure - frames. Used when wType is TIME_SMPTE.
    /// </summary>
    [FieldOffset(7)]
    public byte smpteFrame;
    /// <summary>
    /// SMPTE time structure - frames per second. Used when wType is TIME_SMPTE.
    /// </summary>
    [FieldOffset(8)]
    public byte smpteFps;
    /// <summary>
    /// SMPTE time structure - dummy byte for alignment. Used when wType is TIME_SMPTE.
    /// </summary>
    [FieldOffset(9)]
    public byte smpteDummy;
    /// <summary>
    /// SMPTE time structure - padding. Used when wType is TIME_SMPTE.
    /// </summary>
    [FieldOffset(10)]
    public byte smptePad0;
    /// <summary>
    /// SMPTE time structure - padding. Used when wType is TIME_SMPTE.
    /// </summary>
    [FieldOffset(11)]
    public byte smptePad1;
    /// <summary>
    /// MIDI time structure. Used when wType is TIME_MIDI.
    /// </summary>
    [FieldOffset(4)]
    public uint midiSongPtrPos;
}
