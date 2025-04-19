﻿using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pModel.pScore;

/// <summary>
/// 小節情報（システム）
/// </summary>
public partial class MeasureSys : DisposeBaseClass
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
            BpmLine.Dispose();
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
    /// 小節ライン（BPM）
    /// </summary>
    public MeasureLine<InfoBpm> BpmLine { get; private set; } = new();

    #endregion

    /// <summary>
    /// BPM情報有無判定
    /// </summary>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：あり、False：なし</returns>
    public bool IsBpm( int aNotePos ) => BpmLine.IsInfo( aNotePos );

    /// <summary>
    /// BPM情報取得
    /// </summary>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>取得：BPM情報、未取得：null</returns>
    public InfoBpm? GetBpm( int aNotePos ) => BpmLine.GetInfo( aNotePos );

    /// <summary>
    /// BPM情報追加
    /// </summary>
    /// <param name="aInfo">BPM情報</param>
    public void AddBpm( InfoBpm aInfo ) => BpmLine.AddInfo( aInfo );

    /// <summary>
    /// BPM情報削除
    /// </summary>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：削除後 BPM情報が0件、False：削除後 BPM情報が1件以上</returns>
    public bool RemoveBpm( int aNotePos )
    {
        _ = BpmLine.RemoveInfo( aNotePos );

        return BpmLine.InfoStates.Count == 0;
    }

    /// <summary>
    /// 複製
    /// </summary>
    /// <returns></returns>
    public MeasureSys Clone()
    {
        var measureSys = new MeasureSys()
        {
            BpmLine = BpmLine.Clone(),
        };

        return measureSys;
    }
}
