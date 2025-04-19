using System;
using System.Collections.Generic;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pModel.pScore;

/// <summary>
/// 小節ライン
/// </summary>
public partial class MeasureLine<T> : DisposeBaseClass where T : InfoBase
{
    protected override void Dispose( bool aDisposing )
    {
        if ( _Disposed )
        {
            return;
        }

        // マネージドリソースの解放
        if ( aDisposing )
        {
            InfoStates.Clear();
        }

        // アンマネージドリソースの解放
        {
        }

        _Disposed = true;

        base.Dispose( aDisposing );
    }
    private bool _Disposed = false;

    #region Member

    /// <summary>
    /// 小節内1行分のITEM情報（小節内ノート位置、情報）
    /// </summary>
    public Dictionary<int, T> InfoStates { get; private set; } = [];

    #endregion

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
                throw new InvalidCastException( $"Failed to clone item of type {typeof( T )}." );
            }

            measureLine.InfoStates.Add( item.Key, info );
        }

        return measureLine;
    }
}
