using System;
using System.Collections.Generic;

using DrumMidiClassLibrary.pUtil;

namespace DrumMidiClassLibrary.pModel;

/// <summary>
/// システムチャンネル情報
/// </summary>
public class ChannelSys : DisposeBaseClass
{
    #region Member

    /// <summary>
    /// BPM情報リスト
    /// </summary>
    public Dictionary<int,InfoBpm> BpmInfoList { get; private set; } = new();

    /// <summary>
    /// 小節リスト
    /// </summary>
    public Dictionary<int,MeasureSys> MeasureList { get; private set; } = new();

    /// <summary>
    /// システム情報が存在する小節番号最大値
    /// </summary>
    public int MaxMeasureNo { get; private set; } = 0;

    #endregion

    protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
                // Dispose managed resources.
                ClearAll();
			}

			// Dispose unmanaged resources.

			_Disposed = true;

			// Note disposing has been done.
			base.Dispose( aDisposing );
		}
	}
    private bool _Disposed = false;

    #region Measure

    /// <summary>
    /// 小節情報取得
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <returns>取得：小節情報、未取得：null</returns>
    public MeasureSys? GetMeasure( int aMeasureNo )
    {
        if ( !MeasureList.TryGetValue( aMeasureNo, out var measure ) )
        {
            return null;
        }
        return measure;
    }

    #endregion

    #region BPM

    /// <summary>
    /// BPM情報有無判定
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：あり、False：なし</returns>
    public bool IsBpm( int aMeasureNo, int aNotePos )
        => GetMeasure( aMeasureNo )?.IsBpm( aNotePos ) ?? false ;

    /// <summary>
    /// BPM情報取得
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>取得：BPM情報、未取得：null</returns>
    public InfoBpm? GetBpm( int aMeasureNo, int aNotePos )
        => GetMeasure( aMeasureNo )?.GetBpm( aNotePos ) ;

    /// <summary>
    /// BPM情報追加
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aBpm">BPM</param>
    /// <param name="aSelected">選択状態</param>
    public void AddBpm( int aMeasureNo, int aNotePos, double aBpm, bool aSelected )
        => AddBpm( new( aMeasureNo, aNotePos, aBpm, aSelected ) );

    /// <summary>
    /// BPM情報追加
    /// </summary>
    /// <param name="aInfo">BPM情報</param>
    public void AddBpm( InfoBpm aInfo )
    {
        BpmInfoList[ aInfo.SearchKey ] = aInfo;

        var measure = GetMeasure( aInfo.MeasureNo ) ?? new() ;

        measure.AddBpm( aInfo );

        MeasureList[ aInfo.MeasureNo ] = measure;

        if ( MaxMeasureNo < aInfo.MeasureNo )
        {
            MaxMeasureNo = aInfo.MeasureNo;
        }
    }

    /// <summary>
    /// BPM情報削除。
    /// MaxMeasureNo の再計算は行いません。
    /// </summary>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：削除後 システム情報が0件、False：削除後 システム情報が1件以上</returns>
    public bool RemoveBpm( int aMeasureNo, int aNotePos )
        => RemoveBpm( new( aMeasureNo, aNotePos ) );

    /// <summary>
    /// BPM情報削除。
    /// MaxMeasureNo の再計算は行いません。
    /// </summary>
    /// <param name="aBpmInfo">BPM情報</param>
    /// <returns>True：削除後 システム情報が0件、False：削除後 システム情報が1件以上</returns>
    public bool RemoveBpm( InfoBpm aBpmInfo )
    {
        BpmInfoList.Remove( aBpmInfo.SearchKey );

        var measure = GetMeasure( aBpmInfo.MeasureNo );

        if ( measure != null )
        {
            if ( measure.RemoveBpm( aBpmInfo.NotePos ) )
            {
                MeasureList.Remove( aBpmInfo.MeasureNo );
            }
        }

        return BpmInfoList.Count == 0;
    }

    #endregion

    #region Function

    /// <summary>
    /// 複製
    /// </summary>
    /// <returns></returns>
    public ChannelSys Clone()
    {
        var channelSys = new ChannelSys()
        {
            MaxMeasureNo = MaxMeasureNo,
        };

        foreach ( var item in BpmInfoList )
        {
            if ( item.Value.Clone() is not InfoBpm info )
            {
                throw new InvalidCastException();
            }

            channelSys.BpmInfoList.Add( item.Key, info );
        }

        foreach ( var item in MeasureList )
        {
            channelSys.MeasureList.Add( item.Key, item.Value.Clone() );
        }

        return channelSys;
    }

    /// <summary>
    /// データクリア（小節/BPM情報）
    /// </summary>
    public void ClearAll()
	{
		foreach ( var de in BpmInfoList )
		{
			de.Value.Dispose();
		}
		BpmInfoList.Clear();

        foreach ( var obj in MeasureList )
	    {
		    obj.Value.Dispose();
        }
        MeasureList.Clear();

        MaxMeasureNo = 0;
    }

    #endregion
}
