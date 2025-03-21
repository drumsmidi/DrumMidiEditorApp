using DrumMidiEditorApp.pView.pEditer;
using DrumMidiLibrary.pModel;
using System.Collections.Generic;

namespace DrumMidiEditorApp.pResume;

/// <summary>
/// 複数レジュームを１レジュームとして管理
/// </summary>
public class ResumeMultiple : IResume
{
    /// <summary>
    /// レジュームリスト
    /// </summary>
    private readonly List<IResume> _ResumeList = [];

    /// <summary>
    /// レジューム数
    /// </summary>
    public int Count => _ResumeList.Count;

    /// <summary>
    /// ノート設定
    /// </summary>
    /// <param name="aInfoBef">変更前のNOTE情報</param>
    /// <param name="aInfoAft">変更後のNOTE情報</param>
    public void AddNote( InfoNote? aInfoBef, InfoNote aInfoAft )
        => _ResumeList.Add( new ResumeEditAddNote( aInfoBef, aInfoAft ) );

    /// <summary>
    /// ノート削除
    /// </summary>
    /// <param name="aInfo">変更前のNOTE情報</param>
    public void RemoveNote( InfoNote aInfo )
        => _ResumeList.Add( new ResumeEditRemoveNote( aInfo ) );

    /// <summary>
    /// BPM設定
    /// </summary>
    /// <param name="aInfoBef">変更前のBPM情報</param>
    /// <param name="aInfoAft">変更後のBPM情報</param>
    public void AddBpm( InfoBpm? aInfoBef, InfoBpm aInfoAft )
        => _ResumeList.Add( new ResumeEditAddBpm( aInfoBef, aInfoAft ) );

    /// <summary>
    /// BPM削除
    /// </summary>
    /// <param name="aInfo">変更前のBPM情報</param>
    public void RemoveBpm( InfoBpm aInfo )
        => _ResumeList.Add( new ResumeEditRemoveBpm( aInfo ) );

    /// <summary>
    /// MidiMapScale選択
    /// </summary>
    /// <param name="aChannelNo">編集対象のチェンネル番号</param>
    /// <param name="aScaleKey">編集対象のMidiMapGroupの音階キー</param>
    public void SelectMidiMapGroupScale( byte aChannelNo, string aScaleKey )
        => _ResumeList.Add( new ResumeEditSelectMidiMapGroupScale( aChannelNo, aScaleKey ) );

    /// <summary>
    /// MidiMapGroup選択
    /// </summary>
    /// <param name="aMidiMapGroup">編集対象のMidiMapGroup</param>
    public void SelectMidiMapGroup( MidiMapGroup aMidiMapGroup )
        => _ResumeList.Add( new ResumeEditSelectMidiMapGroup( aMidiMapGroup ) );

    /// <summary>
    /// MidiMap選択
    /// </summary>
    /// <param name="aMidiMap">編集対象のMidiMap</param>
    public void SelectMidiMap( MidiMap aMidiMap )
        => _ResumeList.Add( new ResumeEditSelectMidiMap( aMidiMap ) );

    /// <summary>
    /// ノート音量設定
    /// </summary>
    /// <param name="aInfoBef">変更前のNOTE情報</param>
    /// <param name="aInfoAft">変更後のNOTE情報</param>
    public void SetNoteVolume( InfoNote aInfoBef, InfoNote aInfoAft )
        => _ResumeList.Add( new ResumeEditSetNoteVolume( aInfoBef, aInfoAft ) );

    /// <summary>
    /// BPM選択
    /// </summary>
    /// <param name="aInfo">編集対象のBPM情報</param>
    /// <param name="aSelectAft">選択状態</param>
    public void SelectBpm( InfoBpm aInfo, bool aSelectAft )
        => _ResumeList.Add( new ResumeEditSelectBpm( aInfo, aSelectAft ) );

    /// <summary>
    /// ノート選択
    /// </summary>
    /// <param name="aInfo">編集対象のNOTE情報</param>
    /// <param name="aSelectAft">選択状態</param>
    public void SelectNote( InfoNote aInfo, bool aSelectAft )
        => _ResumeList.Add( new ResumeEditSelectNote( aInfo, aSelectAft ) );

    /// <summary>
    /// ノート範囲設定
    /// </summary>
    /// <param name="aRangeRef">変更後のノート範囲</param>
    /// <param name="aRangeBef">変更前のノート範囲</param>
    public void SelectNoteRange( ref DmsItemNoteRange aRangeRef, DmsItemNoteRange aRangeBef )
        => _ResumeList.Add( new ResumeEditNoteRange( ref aRangeRef, aRangeBef ) );

    public void Undo()
    {
        for ( var index = _ResumeList.Count - 1; index >= 0; index-- )
        {
            _ResumeList [ index ].Undo();
        }
    }

    public void Redo()
    {
        foreach ( var resume in _ResumeList )
        {
            resume.Redo();
        }
    }
}
