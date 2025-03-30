using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace DrumMidiLibrary.pModel.pScore;

/// <summary>
/// MidiMap
/// </summary>
public partial class MidiMap : DisposeBaseClass
{
    /// <summary>
    /// 所属するMidiMapGroup
    /// </summary>
    public MidiMapGroup? Group { get; set; } = null;

    /// <summary>
    /// 表示設定
    /// </summary>
    public bool Display { get; set; } = ConfigLib.System.DefaultMidiMapDisplay;

    /// <summary>
    /// MidiMapキー
    /// </summary>
    public int MidiMapKey { get; set; } = ConfigLib.System.MidiMapKeyNotSelect;

    /// <summary>
    /// ラベル名称
    /// </summary>
    public string MidiMapName { get; set; } = ConfigLib.System.DefaultMidiMapName;

    /// <summary>
    /// MIDiノート番号
    /// </summary>
    public byte Midi { get; set; } = ConfigLib.System.DefaultMidiMapMidi;

    /// <summary>
    /// 音量増減値
    /// </summary>
    public int VolumeAdd { get; set; } = ConfigLib.System.DefaultMidiMapVolumeAdd;

    /// <summary>
    /// ノートの色
    /// </summary>
    public Color Color { get; set; } = ConfigLib.System.DefaultMidiMapColor;

    /// <summary>
    /// ノートの色
    /// </summary>
    public string ColorText
    {
        get => HelperColor.GetColor( Color );
        set => Color = HelperColor.GetColor( value );
    }

    /// <summary>
    /// ノートの色
    /// </summary>
    public Brush ColorBrush
    {
        get => new SolidColorBrush( Color );
        set => Color = ( value as SolidColorBrush )?.Color ?? HelperColor.EmptyColor;
    }

    /// <summary>
    /// 選択状態
    /// </summary>
    public bool Selected { get; set; } = false;

    /// <summary>
    /// 選択状態（MidiMapGroupScale, MidiMapGroup, MidiMapのいずれかがtrueの場合true) 
    /// </summary>
    public bool AnySelected => Selected || ( Group?.AnySelected ?? false );

    /// <summary>
    /// 音階名称（ピッチ＋音階）
    /// </summary>
    public string Scale { get; set; } = ConfigLib.System.DefaultMidiMapScale;

    /// <summary>
    /// 音階キーテキスト
    /// </summary>
    public string ScaleKeyText { get; set; } = ConfigLib.System.DefaultMidiMapScaleKey;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                Group = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// 音量増減値を取得
    /// （MidiMapの音量増減＋MidiMapGroupの音量増減）
    /// </summary>
    public int VolumeAddIncludeGroup 
        => VolumeAdd + Group?.VolumeAdd ?? ConfigLib.System.DefaultMidiMapGroupVolumeAdd ;

    /// <summary>
    /// MidiMapを複製。
    /// 但し、選択状態は初期化されます。
    /// また、グループの情報はNULLとなるので複製後 再設定が必要です。
    /// </summary>
    public MidiMap Clone()
    {
        return new()
        {
        //  Group       = Group,
            Display     = Display,
            MidiMapKey  = MidiMapKey,
            MidiMapName = MidiMapName,
            VolumeAdd   = VolumeAdd,
            Midi        = Midi,
            Color       = Color,
        //  Selected    = Selected,
            Scale       = Scale,
            ScaleKeyText= ScaleKeyText,
        };
    }
}
