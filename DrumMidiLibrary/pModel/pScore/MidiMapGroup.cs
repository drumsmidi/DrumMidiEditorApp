using System.Collections.Generic;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pModel.pScore;

/// <summary>
/// MidiMapGroup
/// </summary>
public partial class MidiMapGroup : DisposeBaseClass
{
    /// <summary>
    /// 表示設定
    /// </summary>
    public bool Display { get; set; } = ConfigLib.System.DefaultMidiMapGroupDisplay;

    /// <summary>
    /// MidiMapGroupキー
    /// </summary>
    public int GroupKey { get; set; } = ConfigLib.System.DefaultMidiMapGroupVolumeAdd;

    /// <summary>
    /// ラベル名称
    /// </summary>
    public string GroupName { get; set; } = ConfigLib.System.DefaultMidiMapGroupName;

    /// <summary>
    /// 音量増減値
    /// </summary>
    public int VolumeAdd { get; set; } = ConfigLib.System.DefaultMidiMapGroupVolumeAdd;

    /// <summary>
    /// MidiMapGroupに属するMidiMapリスト
    /// </summary>
    public List<MidiMap> MidiMaps { get; private set; } = [];

    /// <summary>
    /// MidiMapGroup選択状態
    /// </summary>
    public bool Selected { get; set; } = false;

    /// <summary>
    /// 音階キー
    /// </summary>
    public string ScaleKey { get; set; } = ConfigLib.System.DefaultMidiMapGroupScaleKey;

    /// <summary>
    /// ScaleKey選択状態
    /// </summary>
    public bool ScaleKeySelected { get; set; } = false;

    /// <summary>
    /// 選択状態（Selected, ScaleKeySelectedのいずれかがtrueの場合true）
    /// </summary>
    public bool AnySelected => Selected || ScaleKeySelected;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                RemoveMidiMaps();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// MidiMapGroupにMidiMapを追加。
    /// MidiMapを複製して登録します。
    /// 但し、MidiMapの選択状態は初期化されます。
    /// </summary>
    /// <param name="aMidiMap">追加するMidiMap情報</param>
    public void AddMidiMap( MidiMap aMidiMap )
    {
        var midiMap = aMidiMap.Clone();
        midiMap.Group = this;

        MidiMaps.Add( midiMap );
    }

    /// <summary>
    /// MidiMapGroupに属する指定したMidiMapを削除
    /// </summary>
    /// <param name="aMidiMapIndex">MidiMap連番（0-）</param>
    public void RemoveMidiMap( int aMidiMapIndex )
    {
        var midiMap = MidiMaps[ aMidiMapIndex ];
        if ( midiMap == null )
        {
            return;
        }

        midiMap.Dispose();

        MidiMaps.RemoveAt( aMidiMapIndex );
    }

    /// <summary>
    /// MidiMapGroupに属するMidiMapを全て削除
    /// </summary>
    public void RemoveMidiMaps()
    {
        foreach ( var midiMap in MidiMaps )
        {
            midiMap.Dispose();
        }
        MidiMaps.Clear();
    }

    /// <summary>
    /// MidiMapGroupを複製。
    /// 但し、MidiMapGroup/MidiMapの選択状態は初期化されます。
    /// </summary>
    public MidiMapGroup Clone()
    {
        var group = new MidiMapGroup
        {
            Display             = Display,
            GroupKey            = GroupKey,
            GroupName           = GroupName,
            VolumeAdd           = VolumeAdd,
        //  Selected	        = Selected,
            ScaleKey            = ScaleKey,
        //  ScaleKeySelected	= ScaleKeySelected,
        };

        foreach ( var midiMap in MidiMaps )
        {
            var obj = midiMap.Clone();
            obj.Group = group;

            group.MidiMaps.Add( obj );
        }

        return group;
    }
}
