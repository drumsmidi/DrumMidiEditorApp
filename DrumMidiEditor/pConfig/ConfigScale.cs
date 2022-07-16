using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DrumMidiEditor.pConfig;

/// <summary>
/// Scaleタブ設定
/// </summary>
public class ConfigScale
{
    #region 波形

    /// <summary>
    /// 波形表示ON/OFF
    /// </summary>
    [JsonInclude]
    public bool WaveFormOn { get; set; } = true;

    /// <summary>
    /// ボリュームレベルTOP
    /// </summary>
    [JsonInclude]
    public float VolumeLevelTop { get; set; } = 1.00F;

    /// <summary>
    /// ボリュームレベルHIGH
    /// </summary>
    [JsonInclude]
    public float VolumeLevelHigh { get; set; } = 0.75F;

    /// <summary>
    /// ボリュームレベルMID
    /// </summary>
    [JsonInclude]
    public float VolumeLevelMid { get; set; } = 0.65F;

    /// <summary>
    /// ボリュームレベルLOW
    /// </summary>
    [JsonInclude]
    public float VolumeLevelLow { get; set; } = 0.55F;

    /// <summary>
    /// 感度レベル
    /// </summary>
    [JsonInclude]
    public float SensitivityLevel { get; set; } = 0.55F;

    #endregion

    #region 音階

    /// <summary>
    /// 音階アイテム
    /// </summary>
    public class ScaleItem
    {
        /// <summary>
        /// 音階名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// ピッチ
        /// </summary>
        public int Pitch { get; private set; }

        /// <summary>
        /// MIDIノート番号
        /// </summary>
        public byte Midi { get; private set; }

        /// <summary>
        /// Hz
        /// </summary>
        public float Hz { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="aName">音階名称</param>
        /// <param name="aPitch">ピッチ</param>
        /// <param name="aMidi">MIDIノート番号</param>
        /// <param name="aHz">Hz</param>
        public ScaleItem( string aName, int aPitch, byte aMidi, float aHz )
        {
            Name   = aName;
            Pitch  = aPitch;
            Midi   = aMidi;
            Hz     = aHz;
        }

        /// <summary>
        /// ピッチ＋音階名称
        /// </summary>
        public string PitchName => $"{Pitch}-{Name}";
    }

    /// <summary>
    /// 音階リスト
    /// </summary>
    [JsonIgnore]
    public List<ScaleItem> ScaleList => _ScaleList;

    /// <summary>
    /// 音階リスト。
    /// Refer: https://composer-instruments.com/scale-and-frequency/
    /// </summary>
    [JsonIgnore]
    private readonly List<ScaleItem> _ScaleList = new()
    {
        new( "C"	, 0,	 24,	   32.703F ),	// 「A0」の実音（55hz）
        new( "C#"	, 0,	 25,	   34.647F ),
        new( "D"	, 0,	 26,	   36.708F ),
        new( "D#"	, 0,	 27,	   38.890F ),
        new( "E"	, 0,	 28,	   41.203F ),
        new( "F"	, 0,	 29,	   43.653F ),
        new( "F#"	, 0,	 30,	   46.249F ),
        new( "G"	, 0,	 31,	   48.999F ),
        new( "G#"	, 0,	 32,	   51.913F ),
        new( "A"	, 0,	 33,	   55.000F ),
        new( "A#"	, 0,	 34,	   58.270F ),
        new( "B"	, 0,	 35,	   61.735F ),
        new( "C"	, 1,	 36,	   65.406F ),	// 「A1」の実音（110hz）
        new( "C#"	, 1,	 37,	   69.296F ),
        new( "D"	, 1,	 38,	   73.416F ),
        new( "D#"	, 1,	 39,	   77.782F ),
        new( "E"	, 1,	 40,	   82.407F ),
        new( "F"	, 1,	 41,	   87.307F ),
        new( "F#"	, 1,	 42,	   92.499F ),
        new( "G"	, 1,	 43,	   97.999F ),
        new( "G#"	, 1,	 44,	  103.826F ),
        new( "A"	, 1,	 45,	  110.000F ),
        new( "A#"	, 1,	 46,	  116.541F ),
        new( "B"	, 1,	 47,	  123.471F ),
        new( "C"	, 2,	 48,	  130.813F ),	// 「A2」の実音（220hz）
        new( "C#"	, 2,	 49,	  138.591F ),
        new( "D"	, 2,	 50,	  146.832F ),
        new( "D#"	, 2,	 51,	  155.563F ),
        new( "E"	, 2,	 52,	  164.814F ),
        new( "F"	, 2,	 53,	  174.614F ),
        new( "F#"	, 2,	 54,	  184.997F ),
        new( "G"	, 2,	 55,	  195.998F ),
        new( "G#"	, 2,	 56,	  207.652F ),
        new( "A"	, 2,	 57,	  220.000F ),
        new( "A#"	, 2,	 58,	  233.082F ),
        new( "B"	, 2,	 59,	  246.942F ),
        new( "C"	, 3,	 60,	  261.626F ),	// 「A3」の実音（440hz）
        new( "C#"	, 3,	 61,	  277.183F ),
        new( "D"	, 3,	 62,	  293.665F ),
        new( "D#"	, 3,	 63,	  311.127F ),
        new( "E"	, 3,	 64,	  329.628F ),
        new( "F"	, 3,	 65,	  349.228F ),
        new( "F#"	, 3,	 66,	  369.994F ),
        new( "G"	, 3,	 67,	  391.995F ),
        new( "G#"	, 3,	 68,	  415.305F ),
        new( "A"	, 3,	 69,	  440.000F ),
        new( "A#"	, 3,	 70,	  466.164F ),
        new( "B"	, 3,	 71,	  493.883F ),
        new( "C"	, 4,	 72,	  523.251F ),	// 「A4」の実音（880hz）
        new( "C#"	, 4,	 73,	  554.365F ),
        new( "D"	, 4,	 74,	  587.330F ),
        new( "D#"	, 4,	 75,	  622.254F ),
        new( "E"	, 4,	 76,	  659.255F ),
        new( "F"	, 4,	 77,	  698.456F ),
        new( "F#"	, 4,	 78,	  739.989F ),
        new( "G"	, 4,	 79,	  783.991F ),
        new( "G#"	, 4,	 80,	  830.609F ),
        new( "A"	, 4,	 81,	  880.000F ),
        new( "A#"	, 4,	 82,	  932.328F ),
        new( "B"	, 4,	 83,	  987.767F ),
        new( "C"	, 5,	 84,	 1046.502F ),	// 「A5」の実音（1760hz）
        new( "C#"	, 5,	 85,	 1108.731F ),
        new( "D"	, 5,	 86,	 1174.659F ),
        new( "D#"	, 5,	 87,	 1244.508F ),
        new( "E"	, 5,	 88,	 1318.510F ),
        new( "F"	, 5,	 89,	 1396.913F ),
        new( "F#"	, 5,	 90,	 1479.978F ),
        new( "G"	, 5,	 91,	 1567.982F ),
        new( "G#"	, 5,	 92,	 1661.219F ),
        new( "A"	, 5,	 93,	 1760.000F ),
        new( "A#"	, 5,	 94,	 1864.655F ),
        new( "B"	, 5,	 95,	 1975.533F ),
        new( "C"	, 6,	 96,	 2093.005F ),	// 「A6」の実音（3520hz）
        new( "C#"	, 6,	 97,	 2217.461F ),
        new( "D"	, 6,	 98,	 2349.318F ),
        new( "D#"	, 6,	 99,	 2489.016F ),
        new( "E"	, 6,	100,	 2637.020F ),
        new( "F"	, 6,	101,	 2793.826F ),
        new( "F#"	, 6,	102,	 2959.955F ),
        new( "G"	, 6,	103,	 3135.963F ),
        new( "G#"	, 6,	104,	 3322.438F ),
        new( "A"	, 6,	105,	 3520.000F ),
        new( "A#"	, 6,	106,	 3729.310F ),
        new( "B"	, 6,	107,	 3951.066F ),
        new( "C"    , 7,    108,     4186.009F ),	// 「A7」の実音（7040hz）
        new( "C#"   , 7,    109,     4434.922F ),
        new( "D"    , 7,    110,     4698.636F ),
        new( "D#"   , 7,    111,     4978.032F ),
        new( "E"    , 7,    112,     5274.041F ),
        new( "F"    , 7,    113,     5587.652F ),
        new( "F#"   , 7,    114,     5919.911F ),
        new( "G"    , 7,    115,     6271.927F ),
        new( "G#"   , 7,    116,     6644.875F ),
        new( "A"    , 7,    117,     7040.000F ),
        new( "A#"   , 7,    118,     7458.620F ),
        new( "B"    , 7,    119,     7902.133F ),
        new( "C"    , 8,    120,     8372.018F ),	// 「A8」の実音（14080hz）
        new( "C#"   , 8,    121,     8869.844F ),
        new( "D"    , 8,    122,     9397.272F ),
        new( "D#"   , 8,    123,     9956.064F ),
        new( "E"    , 8,    124,    10548.082F ),
        new( "F"    , 8,    125,    11175.304F ),
        new( "F#"   , 8,    126,    11839.822F ),
        new( "G"    , 8,    127,    12543.854F ),
        new( "G#"   , 8,    128,    13289.750F ),
        new( "A"    , 8,      0,    14080.000F ),
        new( "A#"   , 8,      0,    14917.240F ),
        new( "B"    , 8,      0,    15804.264F ),
        new( "C"    , 9,      0,    16744.036F ),	// 「A9」の実音（28160hz）
        new( "C#"   , 9,      0,    17739.688F ),
        new( "D"    , 9,      0,    18794.544F ),
        new( "D#"   , 9,      0,    19912.128F ),
        new( "E"    , 9,      0,    21096.164F ),
        new( "F"    , 9,      0,    22350.608F ),
        new( "F#"   , 9,      0,    23679.644F ),
        new( "G"    , 9,      0,    25087.708F ),
        new( "G#"   , 9,      0,    26879.500F ),
        new( "A"    , 9,      0,    28160.000F ),
        new( "A#"   , 9,      0,    29834.480F ),
        new( "B"    , 9,      0,    31608.528F ),
    };

    /// <summary>
    /// 音階リストのインデックス番号取得
    /// </summary>
    /// <param name="aScaleText">[ピッチ名]-[音階] ( 例: "3-C#" )</param>
    /// <returns>階リストのインデックス番号</returns>
    public int GetScaleListIndex( string aScaleText )
    {
        if ( aScaleText.Length == 0 )
        {
            return -1;
        }

        int index = -1;

        foreach ( var item in _ScaleList )
        {
            index++;

            if ( item.PitchName.Equals( aScaleText ) )
            {
                return index;
            }
        }

        return -1;
    }

    #endregion
}
