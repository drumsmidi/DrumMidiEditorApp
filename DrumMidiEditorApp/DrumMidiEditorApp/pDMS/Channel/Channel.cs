using System;
using System.Collections.Generic;
using System.Linq;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pDMS;

/// <summary>
/// チャンネル情報
/// </summary>
public class Channel : DisposeBaseClass
{
    #region Member

    /// <summary>
    /// MIDIチャンネル番号(0-15)
    /// </summary>
    public byte ChannelNo { get; private set; } = 0;

    /// <summary>
    /// MidiMapSet
    /// </summary>
    public MidiMapSet MidiMapSet { get; set; } = new();

    /// <summary>
    /// NOTE情報リスト（検索キー、NOTE情報）
    /// </summary>
    public SortedDictionary<int,InfoNote> NoteInfoList { get; private set; } = new();

    /// <summary>
    /// 小節リスト（小節番号、小節情報）
    /// </summary>
    public Dictionary<int,Measure> MeasureList { get; private set; } = new();

    /// <summary>
    /// NOTEが存在する小節番号最大値
    /// </summary>
    public int MaxMeasureNo { get; private set; } = 0;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aChannelNo">チャンネル番号(0-15)</param>
    public Channel( byte aChannelNo )
    {
        ChannelNo = aChannelNo;
    }

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
    public Measure? GetMeasure( int aMeasureNo )
    {
        if ( !MeasureList.TryGetValue( aMeasureNo, out var measure ) )
        {
            return null;
        }
        return measure;
    }

    #endregion

    #region Note

    /// <summary>
    /// NOTE情報有無判定
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：あり、False：なし</returns>
    public bool IsNote( int aMidiMapKey, int aMeasureNo, int aNotePos )
        => GetMeasure( aMeasureNo )?.IsNote( aMidiMapKey, aNotePos ) ?? false ;

    /// <summary>
    /// NOTE情報取得
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>取得：NOTE情報、未取得：null</returns>
    public InfoNote? GetNote( int aMidiMapKey, int aMeasureNo, int aNotePos )
        => GetMeasure( aMeasureNo )?.GetNote( aMidiMapKey, aNotePos );

    /// <summary>
    /// NOTE情報追加
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aVolume">音量（127基準）</param>
    /// <param name="aSelected">選択状態</param>
    public void AddNoteOn( int aMidiMapKey, int aMeasureNo, int aNotePos, int aVolume, bool aSelected )
		=> AddNote( new( ChannelNo, aMidiMapKey, aMeasureNo, aNotePos, aVolume, true, false, aSelected ) );

    /// <summary>
    /// NOTE情報追加
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aVolume">音量（127基準）</param>
    /// <param name="aSelected">選択状態</param>
    public void AddNoteOff( int aMidiMapKey, int aMeasureNo, int aNotePos, int aVolume, bool aSelected )
		=> AddNote( new( ChannelNo, aMidiMapKey, aMeasureNo, aNotePos, aVolume, false, true, aSelected ) );

    /// <summary>
    /// NOTE情報追加
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNoteNo">小節内ノート位置</param>
    /// <param name="aVolume">音量（127基準）</param>
    /// <param name="aSelected">選択状態</param>
    public void AddNoteOffOn( int aMidiMapKey, int aMeasureNo, int aNoteNo, int aVolume, bool aSelected )
		=> AddNote( new( ChannelNo, aMidiMapKey, aMeasureNo, aNoteNo, aVolume, true, true, aSelected ) );

    /// <summary>
    /// NOTE情報追加
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <param name="aVolume">音量（127基準）</param>
    /// <param name="aNoteOn">ノートON有無</param>
    /// <param name="aNoteOff">ノートOFF有無</param>
    /// <param name="aSelected">選択状態</param>
    public void AddNote( int aMidiMapKey, int aMeasureNo, int aNotePos, int aVolume, bool aNoteOn, bool aNoteOff, bool aSelected )
		=> AddNote( new( ChannelNo, aMidiMapKey, aMeasureNo, aNotePos, aVolume, aNoteOn, aNoteOff, aSelected ) );

    /// <summary>
    /// NOTE情報追加
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aAbsoultNotePos">ノート位置（絶対値）</param>
    /// <param name="aVolume">音量（127基準）</param>
    /// <param name="aNoteOn">ノートON有無</param>
    /// <param name="aNoteOff">ノートOFF有無</param>
    /// <param name="aSelected">選択状態</param>
    public void AddNote( int aMidiMapKey, int aAbsoultNotePos, int aVolume, bool aNoteOn, bool aNoteOff, bool aSelected )
		=> AddNote
            ( 
                new
                (
                    ChannelNo,
                    aMidiMapKey,
                    aAbsoultNotePos / Config.System.MeasureNoteNumber,
                    aAbsoultNotePos % Config.System.MeasureNoteNumber,
                    aVolume,
                    aNoteOn,
                    aNoteOff,
                    aSelected 
                ) 
            );

    /// <summary>
    /// NOTE情報追加
    /// </summary>
    /// <param name="aInfo">NOTE情報</param>
    public void AddNote( InfoNote aInfo )
    {
        NoteInfoList[ aInfo.SearchKey ] = aInfo;

        var measure = GetMeasure( aInfo.MeasureNo );

        if ( measure == null )
        {
            measure = new Measure();
        }

        measure.AddNote( aInfo );

        MeasureList[ aInfo.MeasureNo ] = measure;

        if ( MaxMeasureNo < aInfo.MeasureNo )
        {
            MaxMeasureNo = aInfo.MeasureNo;
        }
    }

    /// <summary>
    /// NOTE情報削除。
    /// MaxMeasureNo の再計算は行いません。
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aNotePos">小節内ノート位置</param>
    /// <returns>True：削除後 NOTE情報が0件、False：削除後 NOTE情報が1件以上</returns>
    public bool RemoveNote( int aMidiMapKey, int aMeasureNo, int aNotePos )
        => RemoveNote( new( ChannelNo, aMidiMapKey, aMeasureNo, aNotePos, true, false ) );

    /// <summary>
    /// NOTE情報削除。
    /// MaxMeasureNo の再計算は行いません。
    /// </summary>
    /// <param name="aInfo">NOTE情報</param>
    /// <returns>True：削除後 NOTE情報が0件、False：削除後 NOTE情報が1件以上</returns>
    public bool RemoveNote( InfoNote aInfo )
    {
        NoteInfoList.Remove( aInfo.SearchKey );

        var measure = GetMeasure( aInfo.MeasureNo );

        if ( measure != null )
        {
            if ( measure.RemoveNote( aInfo.MidiMapKey, aInfo.NotePos ) )
            {
                MeasureList.Remove( aInfo.MeasureNo );
            }
        }
        return NoteInfoList.Count == 0;
    }

    #endregion

    #region Function

    /// <summary>
    /// 複製
    /// </summary>
    /// <returns></returns>
    public Channel Clone()
    {
        var channel = new Channel( this.ChannelNo )
        {
            MidiMapSet      = this.MidiMapSet.Clone(),
            MaxMeasureNo    = this.MaxMeasureNo,
        };

        foreach ( var item in NoteInfoList )
        {
            if ( item.Value.Clone() is not InfoNote info )
            {
                throw new InvalidCastException();
            }

            channel.NoteInfoList.Add( item.Key, info );
        }

        foreach ( var item in MeasureList )
        {
            channel.MeasureList.Add( item.Key, item.Value.Clone() );
        }

        return channel;
    }

    /// <summary>
    /// データクリア（小節/NOTE情報）
    /// </summary>
    public void ClearAll()
	{
		foreach ( var de in NoteInfoList )
		{
			de.Value.Dispose();
		}
		NoteInfoList.Clear();

        foreach ( var obj in MeasureList )
	    {
		    obj.Value.Dispose();
        }
        MeasureList.Clear();

        MaxMeasureNo = 0;
    }

    /// <summary>
    /// MidiMapキーごとに登録されていうノートの数を取得
    /// </summary>
    /// <returns>MidiMapキー、登録されているノート数</returns>
    public Dictionary<int, int> GetNumberOfNoteUsingMidiMap()
    {
        var items = new Dictionary<int, int>();

        foreach ( var info in NoteInfoList.Values )
        {
            if ( !items.ContainsKey( info.MidiMapKey ) )
            {
                items[ info.MidiMapKey ] = 0;
            }

            items[ info.MidiMapKey ]++;
        }

        return items;
    }

    /// <summary>
    /// MidiMapキーを別キーに一括変更
    /// </summary>
    /// <param name="aMidiMapKeyBef">変更前MidiMapキー</param>
    /// <param name="aMidiMapKeyAft">変更後MidiMapキー</param>
    public void KeyChange( int aMidiMapKeyBef, int aMidiMapKeyAft )
	{
        if ( aMidiMapKeyBef == aMidiMapKeyAft )
        {
            return;
        }

        for ( int measure_no = 0; measure_no <= MaxMeasureNo; measure_no++ )
        {
			var measure = GetMeasure( measure_no );

            if ( measure == null )
			{
				continue;
			}

            var measure_line = measure.NoteLine( aMidiMapKeyBef );

			if ( measure_line != null )
			{
                var del = new List<int>();

				foreach ( var info in measure_line.InfoStates )
				{
					AddNote( aMidiMapKeyAft, measure_no, info.Key, info.Value.Volume, info.Value.NoteOn, info.Value.NoteOff, false );
                    del.Add( info.Value.NotePos );
				}

                foreach ( var note_pos in del )
                {
                    RemoveNote( aMidiMapKeyBef, measure_no, note_pos );
                }
            }
        }
    }

    /// <summary>
    /// 機械学習用MidiMapPattern情報取得（試作）
    /// </summary>
    /// <param name="aAbsoluteNotePos">ノート位置（絶対値）</param>
    /// <returns>MidiMapPattern情報</returns>
    public string GetMLData( int aAbsoluteNotePos )
    {
        var measure_no  = aAbsoluteNotePos / Config.System.MeasureNoteNumber;
        var note_pos    = aAbsoluteNotePos % Config.System.MeasureNoteNumber;

        var measure = GetMeasure( measure_no );

        if ( measure == null )
        {
            return "NONE";
        }

        string data = String.Empty;

        foreach ( var measure_line in measure.NoteLines.Values )
        {
            var info = measure_line.GetInfo( note_pos );

            if ( info == null )
            {
                continue;
            }

            data += $"{info.MidiMapKey}:";
        }

        return data.Length == 0 ? "NONE" : data.Remove( data.Length - 1 ) ;
    }

    /// <summary>
    /// 指定NOTEの NoteOff イベントまでの距離（ノート位置基準）を取得
    /// </summary>
    /// <param name="aInfo">NOTE情報</param>
    /// <returns>NoteOff イベントまでの距離</returns>
    public int GetNotePosDistanceToNextNoteOff( InfoNote aInfo )
    {
        int distance = 0;

        var nextInfo = NoteInfoList.FirstOrDefault( item => item.Key > aInfo.SearchKey ).Value;

        if ( nextInfo?.NoteOff ?? false && aInfo.MidiMapKey == nextInfo.MidiMapKey )
        {
            distance = nextInfo.AbsoluteNotePos - aInfo.AbsoluteNotePos;
        }

        return distance;
    }

    #endregion
}
