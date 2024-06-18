using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pUtil;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace DrumMidiEditorApp.pModel;

/// <summary>
/// MidiMap
/// </summary>
public class MidiMap : DisposeBaseClass
{
    /// <summary>
    /// 所属するMidiMapGroup
    /// </summary>
    public MidiMapGroup? Group { get; set; } = null;

    /// <summary>
    /// 表示設定
    /// </summary>
    public bool Display { get; set; } = Config.System.DefaultMidiMapDisplay;

    /// <summary>
    /// MidiMapキー
    /// </summary>
    public int MidiMapKey { get; set; } = Config.System.MidiMapKeyNotSelect;

    /// <summary>
    /// ラベル名称
    /// </summary>
    public string MidiMapName { get; set; } = Config.System.DefaultMidiMapName;

    /// <summary>
    /// MIDiノート番号
    /// </summary>
    public byte Midi { get; set; } = Config.System.DefaultMidiMapMidi;

    /// <summary>
    /// 音量増減値
    /// </summary>
    public int VolumeAdd { get; set; } = Config.System.DefaultMidiMapVolumeAdd;

    /// <summary>
    /// ノートの色
    /// </summary>
    public Color Color { get; set; } = Config.System.DefaultMidiMapColor;

    /// <summary>
    /// ノートの色
    /// </summary>
    public string ColorText
    {
        get => ColorHelper.GetColor( Color );
        set => Color = ColorHelper.GetColor( value );
    }

    /// <summary>
    /// ノートの色
    /// </summary>
    public Brush ColorBrush
    {
        get => new SolidColorBrush( Color );
        set => Color = ( value as SolidColorBrush )?.Color ?? ColorHelper.EmptyColor;
    }

    /// <summary>
    /// 選択状態
    /// </summary>
    public bool Selected { get; set; } = false;

    /// <summary>
    /// 音階名称（ピッチ＋音階）
    /// </summary>
    public string Scale { get; set; } = Config.System.DefaultMidiMapScale;

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
    public int VolumeAddIncludeGroup => VolumeAdd + Group?.VolumeAdd ?? 0;

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
        };
    }
}
