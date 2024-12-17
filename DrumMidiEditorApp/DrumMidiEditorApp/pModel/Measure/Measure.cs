using System.Collections.Generic;
using DrumMidiEditorApp.pUtil;

namespace DrumMidiEditorApp.pModel;

/// <summary>
/// 小節情報
/// </summary>
public partial class Measure : DisposeBaseClass
{
    #region Member

    /// <summary>
    /// 小節ライン（NOTE）（MidiMapキー、小節ライン）
    /// </summary>
    public Dictionary<int, MeasureLine<InfoNote>> NoteLines { get; private set; } = [];

    #endregion

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                foreach ( var de in NoteLines )
                {
                    de.Value.Dispose();
                }
                NoteLines.Clear();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// 小節ライン（NOTE）取得
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <returns>取得：小節ライン（NOTE）、未取得：null</returns>
    public MeasureLine<InfoNote>? NoteLine( int aMidiMapKey ) 
        => !NoteLines.TryGetValue( aMidiMapKey, out var measure_line ) ? null : measure_line;

    /// <summary>
    /// NOTE情報有無判定
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：あり、False：なし</returns>
    public bool IsNote( int aMidiMapKey, int aNotePos ) 
        => NoteLines.TryGetValue( aMidiMapKey, out var measure_line ) && measure_line.IsInfo( aNotePos );

    /// <summary>
    /// NOTE情報取得
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>取得：NOTE情報、未取得：null</returns>
    public InfoNote? GetNote( int aMidiMapKey, int aNotePos ) 
        => !NoteLines.TryGetValue( aMidiMapKey, out var measure_line ) ? null : measure_line.GetInfo( aNotePos );

    /// <summary>
    /// NOTE情報追加
    /// </summary>
    /// <param name="aInfo">NOTE情報</param>
    public void AddNote( InfoNote aInfo )
    {
        if ( !NoteLines.TryGetValue( aInfo.MidiMapKey, out var measure_line ) )
        {
            measure_line = new();
        }

        measure_line.AddInfo( aInfo );

        NoteLines [ aInfo.MidiMapKey ] = measure_line;
    }

    /// <summary>
    /// NOTE情報削除
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：削除後 NOTE情報が0件、False：削除後 NOTE情報が1件以上</returns>
    public bool RemoveNote( int aMidiMapKey, int aNotePos )
    {
        if ( NoteLines.TryGetValue( aMidiMapKey, out var measure_line ) )
        {
            if ( measure_line.RemoveInfo( aNotePos ) )
            {
                _ = NoteLines.Remove( aMidiMapKey );
            }
        }
        return NoteLines.Count == 0;
    }

    /// <summary>
    /// 複製
    /// </summary>
    /// <returns></returns>
    public Measure Clone()
    {
        var measure = new Measure();

        foreach ( var item in NoteLines )
        {
            measure.NoteLines.Add( item.Key, item.Value.Clone() );
        }

        return measure;
    }
}
