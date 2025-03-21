using DrumMidiLibrary.pUtil;
using System;
using System.Collections.Generic;

namespace DrumMidiLibrary.pModel;

/// <summary>
/// 小節ライン
/// </summary>
public partial class MeasureLine<T> : DisposeBaseClass where T : InfoBase
{
    #region Member

    /// <summary>
    /// 小節内1行分のITEM情報（小節内ノート位置、情報）
    /// </summary>
    public Dictionary<int, T> InfoStates { get; private set; } = [];

    #endregion

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                InfoStates.Clear();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// 情報有無判定
    /// </summary>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：あり、False：なし</returns>
    public bool IsInfo( int aNotePos )
        => InfoStates.ContainsKey( aNotePos );

    /// <summary>
    /// 情報取得
    /// </summary>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>取得：情報、未取得：null</returns>
    public T? GetInfo( int aNotePos ) 
        => !InfoStates.TryGetValue( aNotePos, out var info ) ? null : info;

    /// <summary>
    /// 情報追加
    /// </summary>
    /// <param name="aInfo">情報</param>
    public void AddInfo( T aInfo )
        => InfoStates [ aInfo.NotePos ] = aInfo;

    /// <summary>
    /// 指定位置の情報削除
    /// </summary>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：削除後 情報が0件、False：削除後 情報が1件以上</returns>
    public bool RemoveInfo( int aNotePos )
    {
        _ = InfoStates.Remove( aNotePos );

        return InfoStates.Count == 0;
    }

    /// <summary>
    /// 複製
    /// </summary>
    /// <returns></returns>
    public MeasureLine<T> Clone()
    {
        var measureLine = new MeasureLine<T>();

        foreach ( var item in InfoStates )
        {
            if ( item.Value.Clone() is not T info )
            {
                throw new InvalidCastException();
            }

            measureLine.InfoStates.Add( item.Key, info );
        }

        return measureLine;
    }
}
