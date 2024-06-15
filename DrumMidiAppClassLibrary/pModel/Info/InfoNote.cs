using System;
using DrumMidiClassLibrary.pConfig;

namespace DrumMidiClassLibrary.pModel;

/// <summary>
/// NOTE情報
/// </summary>
public class InfoNote : InfoBase
{
    #region Member

    /// <summary>
    /// チェンネル番号
    /// </summary>
    public byte ChannelNo { get; private set; } = 0;

    /// <summary>
    /// MidiMapキー
    /// </summary>
    public int MidiMapKey { get; private set; } = Config.System.MidiMapKeyNotSelect;

    /// <summary>
    /// 音量（127基準）
    /// </summary>
    public int Volume { get; set; } = 0;

    /// <summary>
    /// ノートON有無
    /// </summary>
    public bool NoteOn { get; set; } = false;

    /// <summary>
    /// ノートOFF有無
    /// </summary>
    public bool NoteOff { get; set; } = false;

    /// <summary>
    /// 検索キー（MidiMapキー✕1,000,000＋小節番号✕1,000＋小節内ノート位置）
    /// </summary>
    public override int SearchKey
        => ( MidiMapKey * 1000000 ) + ( MeasureNo * 1000 ) + NotePos;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    private InfoNote()
        : base( 0, 0, false )
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aChannelNo">チャンネル番号(0-15)</param>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aNoteOn">ノートON有無</param>
    /// <param name="aNoteOff">ノートOFF有無</param>
    public InfoNote( byte aChannelNo, int aMidiMapKey, int aMeasureNo, int aNotePos, bool aNoteOn, bool aNoteOff )
        : base( aMeasureNo, aNotePos, false )
    {
        ChannelNo = aChannelNo;
        MidiMapKey = aMidiMapKey;
        NoteOn = aNoteOn;
        NoteOff = aNoteOff;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aChannelNo">チャンネル番号(0-15)</param>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aVolume">音量（127基準）</param>
    /// <param name="aNoteOn">ノートON有無</param>
    /// <param name="aNoteOff">ノートOFF有無</param>
    public InfoNote( byte aChannelNo, int aMidiMapKey, int aMeasureNo, int aNotePos, int aVolume, bool aNoteOn, bool aNoteOff )
        : base( aMeasureNo, aNotePos, false )
    {
        ChannelNo = aChannelNo;
        MidiMapKey = aMidiMapKey;
        Volume = aVolume;
        NoteOn = aNoteOn;
        NoteOff = aNoteOff;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aChannelNo">チャンネル番号(0-15)</param>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aVolume">音量（127基準）</param>
    /// <param name="aNoteOn">ノートON有無</param>
    /// <param name="aNoteOff">ノートOFF有無</param>
    /// <param name="aSelected">選択状態</param>
    public InfoNote( byte aChannelNo, int aMidiMapKey, int aMeasureNo, int aNotePos, int aVolume, bool aNoteOn, bool aNoteOff, bool aSelected )
        : base( aMeasureNo, aNotePos, aSelected )
    {
        ChannelNo = aChannelNo;
        MidiMapKey = aMidiMapKey;
        Volume = aVolume;
        NoteOn = aNoteOn;
        NoteOff = aNoteOff;
    }

    /// <summary>
    /// NOTE情報を設定
    /// </summary>
    /// <param name="aInfo">NOTE情報</param>
    public override void Set( object aInfo )
    {
        base.Set( aInfo );

        if ( aInfo is not InfoNote info )
        {
            throw new InvalidCastException();
        }

        ChannelNo = info.ChannelNo;
        MidiMapKey = info.MidiMapKey;
        Volume = info.Volume;
        NoteOn = info.NoteOn;
        NoteOff = info.NoteOff;
    }

    /// <summary>
    /// NOTE情報を複製
    /// </summary>
    /// <returns>NOTE情報(InfoNote)</returns>
    public override object Clone()
    {
        return new InfoNote()
        {
            MeasureNo = MeasureNo,
            NotePos = NotePos,
            Selected = Selected,
            ChannelNo = ChannelNo,
            MidiMapKey = MidiMapKey,
            Volume = Volume,
            NoteOn = NoteOn,
            NoteOff = NoteOff,
        };
    }
}
