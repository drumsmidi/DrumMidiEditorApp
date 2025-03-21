using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pUtil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrumMidiLibrary.pModel;

/// <summary>
/// MidiMapSet
/// </summary>
public partial class MidiMapSet : DisposeBaseClass
{
    #region Member

    /// <summary>
    /// MidiMapGroupリスト（主データ）
    /// </summary>
    public List<MidiMapGroup> MidiMapGroups { get; private set; } = [];

    /// <summary>
    /// MidiMapGroup位置情報リスト（サブデータ）
    /// （描画モード、MidiMapGroupキー、位置情報）
    /// </summary>
    public Dictionary<ConfigSystem.PlayerSurfaceMode, Dictionary<int, MidiMapGroupPosition>> MidiMapGroupPositions
    {
        get; private set;
    } = [];

    /// <summary>
    /// MidiMapリスト
    /// </summary>
    public List<MidiMap> MidiMaps { get; private set; } = [];

    /// <summary>
    /// 表示対象のMidiMapGroupリスト
    /// </summary>
    public List<MidiMapGroup> DisplayMidiMapGroups { get; private set; } = [];

    /// <summary>
    /// 表示対象のMidiMapリスト
    /// </summary>
    public List<MidiMap> DisplayMidiMaps { get; private set; } = [];

    /// <summary>
    /// 表示対象MidiMapGroup別の表示対象MidiMap数リスト
    /// </summary>
    public List<int> DisplayMidiMapCountByGroup { get; private set; } = [];

    /// <summary>
    /// 表示対象のMidiMapGroup数
    /// </summary>
    public int DisplayGroupCount { get; private set; } = 0;

    /// <summary>
    /// 表示対象のMidiMap数
    /// </summary>
    public int DisplayMidiMapAllCount { get; private set; } = 0;

    /// <summary>
    /// MidiMapGroupキーリスト（MidiMapGroups順）
    /// </summary>
    public List<int> MidiMapGroupKeys { get; private set; } = [];

    /// <summary>
    /// MidiMapキーリスト（MidiMaps順）
    /// </summary>
    public List<int> MidiMapKeys { get; private set; } = [];

    /// <summary>
    /// MidiMapGroupキーリスト（DisplayMidiMapGroups順）
    /// </summary>
    public List<int> DisplayMidiMapGroupKeys { get; private set; } = [];

    /// <summary>
    /// MidiMapキーリスト（DisplayMidiMaps順）
    /// </summary>
    public List<int> DisplayMidiMapKeys { get; private set; } = [];

    #endregion

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                DisplayMidiMapGroups.Clear();
                DisplayMidiMaps.Clear();
                DisplayMidiMapCountByGroup.Clear();
                MidiMaps.Clear();
                MidiMapGroupPositions.Clear();

                foreach ( var group in MidiMapGroups )
                {
                    group.Dispose();
                }
                MidiMapGroups.Clear();

                MidiMapGroupKeys.Clear();
                MidiMapKeys.Clear();
                DisplayMidiMapGroupKeys.Clear();
                DisplayMidiMapKeys.Clear();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    #region MidiMapGroup / MidiMap 取得

    /// <summary>
    /// MidiMapGroup取得
    /// </summary>
    /// <param name="aGroupIndex">MidiMapGroup全体の連番（0-）</param>
    /// <returns>取得：MidiMapGroup、未取得：null</returns>
    public MidiMapGroup? GetMidiMapGroup( int aGroupIndex ) 
        => aGroupIndex < 0 || MidiMapGroups.Count < aGroupIndex + 1 ? null : MidiMapGroups.ToArray() [ aGroupIndex ];

    /// <summary>
    /// MidiMapGroup取得
    /// </summary>
    /// <param name="aDisplayIndex">MidiMapGroup表示対象の連番（0-）</param>
    /// <returns>取得：MidiMapGroup、未取得：null</returns>
    public MidiMapGroup? GetDisplayMidiMapGroup( int aDisplayIndex ) 
        => aDisplayIndex < 0 || DisplayMidiMapGroups.Count < aDisplayIndex + 1 ? null : DisplayMidiMapGroups.ToArray() [ aDisplayIndex ];

    /// <summary>
    /// MidiMap取得
    /// </summary>
    /// <param name="aMidiMapIndex">MidiMap全体の連番（0-）</param>
    /// <returns>取得：MidiMap、未取得：null</returns>
    public MidiMap? GetMidiMap( int aMidiMapIndex ) 
        => aMidiMapIndex < 0 || MidiMaps.Count < aMidiMapIndex + 1 ? null : MidiMaps.ToArray() [ aMidiMapIndex ];

    /// <summary>
    /// MidiMap取得
    /// </summary>
    /// <param name="aDisplayIndex">MidiMap表示対象の連番（0-）</param>
    /// <returns>取得：MidiMap、未取得：null</returns>
    public MidiMap? GetDisplayMidiMap( int aDisplayIndex ) 
        => aDisplayIndex < 0 || DisplayMidiMaps.Count < aDisplayIndex + 1 ? null : DisplayMidiMaps.ToArray() [ aDisplayIndex ];

    /// <summary>
    /// MidiMapGroupPosition取得。
    /// 対象データがない場合、新規登録して返します。
    /// </summary>
    /// <param name="aMode">プレイヤー描画モード</param>
    /// <param name="aGroupKey">MidiMapGroupキー</param>
    /// <returns>MidiMapGroupPosition</returns>
    public MidiMapGroupPosition GetMidiMapGroupPosition( ConfigSystem.PlayerSurfaceMode aMode, int aGroupKey )
    {
        if ( !MidiMapGroupPositions.TryGetValue( aMode, out var _ ) )
        {
            AddMidiMapGroupPosition( aMode, aGroupKey, new() );
        }

        if ( !MidiMapGroupPositions [ aMode ].TryGetValue( aGroupKey, out var _ ) )
        {
            MidiMapGroupPositions [ aMode ].Add( aGroupKey, new() );
        }

        return MidiMapGroupPositions [ aMode ] [ aGroupKey ];
    }

    #endregion

    #region Key / Index 取得

    /// <summary>
    /// MidiMapGroupキー取得
    /// </summary>
    /// <param name="aGroupIndex">MidiMapGroup全体の連番（0-）</param>
    /// <returns>取得：MidiMapGroupキー、未取得：-1</returns>
    public int GetMidiMapGroupKey( int aGroupIndex )
        => GetMidiMapGroup( aGroupIndex )?.GroupKey ?? ConfigLib.System.MidiMapGroupKeyNotSelect;

    /// <summary>
    /// MidiMapGroup全体の連番（0-）取得
    /// </summary>
    /// <param name="aGroupKey">MidiMapGroupキー</param>
    /// <returns>取得：MidiMapGroup全体の連番（0-）、未取得：-1</returns>
    public int GetMidiMapGroupIndex( int aGroupKey )
        => MidiMapGroupKeys.IndexOf( aGroupKey );

    /// <summary>
    /// MidiMapGroupキー取得
    /// </summary>
    /// <param name="aDisplayGroupIndex">MidiMapGroup表示対象の連番（0-）</param>
    /// <returns>取得：MidiMapGroupキー、未取得：-1</returns>
    public int GetDisplayMidiMapGroupKey( int aDisplayGroupIndex )
        => GetDisplayMidiMapGroup( aDisplayGroupIndex )?.GroupKey ?? ConfigLib.System.MidiMapGroupKeyNotSelect;

    /// <summary>
    /// MidiMapGroup表示対象の連番（0-）取得
    /// </summary>
    /// <param name="aGroupKey">MidiMapGroupキー</param>
    /// <returns>取得：MidiMapGroup表示対象の連番（0-）、未取得：-1</returns>
    public int GetDisplayMidiMapGroupIndex( int aGroupKey )
        => DisplayMidiMapGroupKeys.IndexOf( aGroupKey );

    /// <summary>
    /// MidiMapキー取得
    /// </summary>
    /// <param name="aMidiMapIndex">MidiMap全体の連番（0-）</param>
    /// <returns>取得：MidiMapキー、未取得：-1</returns>
    public int GetMidiMapKey( int aMidiMapIndex )
        => GetMidiMap( aMidiMapIndex )?.MidiMapKey ?? ConfigLib.System.MidiMapKeyNotSelect;

    /// <summary>
    /// MidiMap全体の連番（0-）取得
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <returns>取得：MidiMap全体の連番（0-）、未取得：-1</returns>
    public int GetMidiMapIndex( int aMidiMapKey )
        => MidiMapKeys.IndexOf( aMidiMapKey );

    /// <summary>
    /// MidiMapキー取得
    /// </summary>
    /// <param name="aDisplayMidiMapIndex">MidiMap表示対象の連番（0-）</param>
    /// <returns>取得：MidiMapキー、未取得：-1</returns>
    public int GetDisplayMidiMapKey( int aDisplayMidiMapIndex )
        => GetDisplayMidiMap( aDisplayMidiMapIndex )?.MidiMapKey ?? ConfigLib.System.MidiMapKeyNotSelect;

    /// <summary>
    /// MidiMap表示対象の連番（0-）取得
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <returns>取得：MidiMap表示対象の連番（0-）、未取得：-1</returns>
    public int GetDisplayMidiMapIndex( int aMidiMapKey )
        => DisplayMidiMapKeys.IndexOf( aMidiMapKey );

    #endregion

    #region Create key of MidiMapGroup / MidiMap

    /// <summary>
    /// MidiMapGroupキーの採番
    /// </summary>
    /// <returns>採番：0-、不採番：-1</returns>
    public int GetMidiMapGroupNewKey()
    {
        var key = ConfigLib.System.MidiMapGroupKeyNotSelect;

        for ( var i = 0; i < ConfigLib.System.MidiMapGroupMaxNumber; i++ )
        {
            if ( !MidiMapGroupKeys.Contains( i ) )
            {
                key = i;
                break;
            }
        }

        return key;
    }

    /// <summary>
    /// MidiMapGroupキーの採番。
    /// 指定したキーが採番済みの場合は新規キーを採番
    /// </summary>
    /// <param name="aNewKey">キー指定</param>
    /// <returns>採番：0-、不採番：-1</returns>
    public int GetMidiMapGroupNewKey( int aNewKey ) 
        => MidiMapGroupKeys.Contains( aNewKey ) ? GetMidiMapGroupNewKey() : aNewKey;

    /// <summary>
    /// MidiMapキーの採番
    /// </summary>
    /// <returns>採番：0-、不採番：-1</returns>
    public int GetMidiMapNewKey()
    {
        var key = ConfigLib.System.MidiMapKeyNotSelect;

        for ( var i = 0; i < ConfigLib.System.MidiMapMaxNumber; i++ )
        {
            if ( !MidiMapKeys.Contains( i ) )
            {
                key = i;
                break;
            }
        }
        return key;
    }

    /// <summary>
    /// MidiMapキーの採番。
    /// 指定したキーが採番済みの場合は新規キーを採番
    /// </summary>
    /// <param name="aNewKey">キー指定</param>
    /// <returns>採番：0-、不採番：-1</returns>
    public int GetMidiMapNewKey( int aNewKey ) 
        => MidiMapKeys.Contains( aNewKey ) ? GetMidiMapNewKey() : aNewKey;

    #endregion

    #region Update

    /// <summary>
    /// MidiMapGroup/MidiMap全ての選択状態を解除
    /// </summary>
    public void ClearSelect()
    {
        foreach ( var group in MidiMapGroups )
        {
            group.Selected          = false;
            group.ScaleKeySelected  = false;
        }
        foreach ( var midiMap in MidiMaps )
        {
            midiMap.Selected = false;
        }
    }

    /// <summary>
    /// MidiMapGroup / MidiMap の情報を検索しやすい様に更新。
    /// MidiMapGroup / MidiMap の追加／移動／削除など実施した後に必ず実施してください。
    /// </summary>
    public void UpdateInfo()
    {
        #region クリア処理

        MidiMaps.Clear();
        DisplayMidiMapGroups.Clear();
        DisplayMidiMaps.Clear();
        DisplayMidiMapCountByGroup.Clear();
        DisplayGroupCount = 0;
        DisplayMidiMapAllCount = 0;
        MidiMapGroupKeys.Clear();
        MidiMapKeys.Clear();
        DisplayMidiMapGroupKeys.Clear();
        DisplayMidiMapKeys.Clear();

        #endregion

        #region 参照情報更新

        var group_cnt = MidiMapGroups.Count;

        for ( var group_index = 0; group_index < group_cnt; group_index++ )
        {
            var group = GetMidiMapGroup( group_index );

            if ( group == null )
            {
                continue;
            }

            // MidiMapGroup設定
            MidiMapGroupKeys.Add( group.GroupKey );

            // MidiMap全体設定
            foreach ( var midiMap in group.MidiMaps )
            {
                MidiMaps.Add( midiMap );
                MidiMapKeys.Add( midiMap.MidiMapKey );
            }

            if ( !group.Display )
            {
                continue;
            }

            // 表示対象のMidiMapGroup設定
            foreach ( var midiMap in group.MidiMaps )
            {
                if ( midiMap.Display )
                {
                    DisplayMidiMapGroups.Add( group );
                    DisplayMidiMapGroupKeys.Add( group.GroupKey );
                    break;
                }
            }

            var midiMapSum = 0;

            // 表示対象のMidiMap設定
            foreach ( var midiMap in group.MidiMaps )
            {
                if ( midiMap.Display )
                {
                    midiMapSum++;
                    DisplayMidiMaps.Add( midiMap );
                    DisplayMidiMapKeys.Add( midiMap.MidiMapKey );
                }
            }

            // 表示対象MidiMapGroup別の表示対象MidiMap数を設定
            if ( midiMapSum != 0 )
            {
                DisplayMidiMapCountByGroup.Add( midiMapSum );
            }
        }

        // 表示対象MidiMapGroup/MidiMap数を設定
        DisplayGroupCount       = DisplayMidiMapGroups.Count;
        DisplayMidiMapAllCount  = DisplayMidiMaps.Count;

        #endregion

        #region MidiMapGroupPositions更新

        foreach ( var item in MidiMapGroupPositions.Values )
        {
            foreach ( var dgp in item )
            {
                if ( GetMidiMapGroupIndex( dgp.Key ) == -1 )
                {
                    _ = item.Remove( dgp.Key );
                }
            }
        }

        foreach ( var group in MidiMapGroups )
        {
            foreach ( var mode in Enum.GetValues<ConfigSystem.PlayerSurfaceMode>() )
            {
                if ( !MidiMapGroupPositions.TryGetValue( mode, out var list ) )
                {
                    list = [];
                }

                MidiMapGroupPositions [ mode ] = list;

                if ( !list.ContainsKey( group.GroupKey ) )
                {
                    AddMidiMapGroupPosition( mode, group.GroupKey, new() );
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 対象MidiMapGroupの位置を移動。
    /// MidiMapSet更新後、必ずUpdateInfo()を実行してください。
    /// </summary>
    /// <param name="aGroupIndex">MidiMapGroup全体の連番（0-）</param>
    /// <param name="aMove">MidiMapGroupの連番内移動量</param>
    public void MoveMidiMapGroup( int aGroupIndex, int aMove )
    {
        var index = aGroupIndex + aMove;

        if ( index < 0 )
        {
            index = 0;
        }
        else if ( index > MidiMapGroups.Count - 1 )
        {
            index = MidiMapGroups.Count - 1;
        }
        var group = GetMidiMapGroup( aGroupIndex );

        if ( group != null )
        {
            MidiMapGroups.RemoveAt( aGroupIndex );

            MidiMapGroups.Insert( index, group );
        }
    }

    /// <summary>
    /// 指定したMidiMapGroupを追加。
    /// MidiMapGroupの情報を複製して登録します。
    /// MidiMapSet更新後、必ずUpdateInfo()を実行してください。
    /// </summary>
    /// <param name="aMidiMapGroup">追加するMidiMapGroup</param>
    public void AddMidiMapGroup( MidiMapGroup aMidiMapGroup )
    {
        var group = aMidiMapGroup.Clone();

        MidiMapGroups.Add( group );
    }

    /// <summary>
    /// 指定したMidiMapGroupを削除
    /// MidiMapSet更新後、必ずUpdateInfo()を実行してください。
    /// </summary>
    /// <param name="aGroupIndex">MidiMapGroup全体の連番（0-）</param>
    public void RemoveMidiMapGroup( int aGroupIndex )
    {
        var group = GetMidiMapGroup( aGroupIndex );

        if ( group != null )
        {
            group.Dispose();
            MidiMapGroups.RemoveAt( aGroupIndex );
        }
    }

    /// <summary>
    /// 対象MidiMapの位置を移動
    /// MidiMapSet更新後、必ずUpdateInfo()を実行してください。
    /// </summary>
    /// <param name="aGroupIndex">MidiMapGroup全体の連番（0-）</param>
    /// <param name="aMidiMapIndexOfGroup">Group内のMidiMap連番（0-）</param>
    /// <param name="aMove">MidiMapの連番内移動量</param>
    public void MoveMidiMap( int aGroupIndex, int aMidiMapIndexOfGroup, int aMove )
    {
        var group = GetMidiMapGroup( aGroupIndex );

        if ( group == null )
        {
            return;
        }

        var index = aMidiMapIndexOfGroup + aMove;

        if ( index < 0 )
        {
            index = 0;
        }
        else if ( index > group.MidiMaps.Count - 1 )
        {
            index = group.MidiMaps.Count - 1;
        }

        var midiMap = group.MidiMaps[ aMidiMapIndexOfGroup ];

        if ( midiMap != null )
        {
            group.MidiMaps.RemoveAt( aMidiMapIndexOfGroup );
            group.MidiMaps.Insert( index, midiMap );
        }
    }

    /// <summary>
    /// 対象MidiMapGroup内に指定したMidiMapを追加。
    /// MidiMapの情報を複製して登録します。
    /// MidiMapSet更新後、必ずUpdateInfo()を実行してください。
    /// </summary>
    /// <param name="aGroupIndex">MidiMapGroup全体の連番（0-）</param>
    /// <param name="aMidiMap">追加するMidiMap</param>
    public void AddMidiMap( int aGroupIndex, MidiMap aMidiMap )
        => GetMidiMapGroup( aGroupIndex )?.AddMidiMap( aMidiMap );

    /// <summary>
    /// 対象MidiMapGroup内の指定したMidiMapを削除
    /// MidiMapSet更新後、必ずUpdateInfo()を実行してください。
    /// </summary>
    /// <param name="aGroupIndex">MidiMapGroup全体の連番（0-）</param>
    /// <param name="aMidiMapIndex">MidiMap全体の連番（0-）</param>
    public void RemoveMidiMap( int aGroupIndex, int aMidiMapIndex )
        => GetMidiMapGroup( aGroupIndex )?.RemoveMidiMap( aMidiMapIndex );

    /// <summary>
    /// 対象MidiMapGroup内のMidiMapをすべて削除
    /// MidiMapSet更新後、必ずUpdateInfo()を実行してください。
    /// </summary>
    /// <param name="aGroupIndex">MidiMapGroup全体の連番（0-）</param>
    public void RemoveMidiMaps( int aGroupIndex )
        => GetMidiMapGroup( aGroupIndex )?.RemoveMidiMaps();

    /// <summary>
    /// 指定したMidiMapGroupPositionを追加。
    /// MidiMapGroupの情報を複製して登録します。
    /// </summary>
    /// <param name="aMode">描画モード</param>
    /// <param name="aGroupKey">追加するMidiMapGroupキー</param>
    /// <param name="aMidiMapGroupPosition">追加するMidiMapGroupPosition</param>
    public void AddMidiMapGroupPosition( ConfigSystem.PlayerSurfaceMode aMode, int aGroupKey, MidiMapGroupPosition aMidiMapGroupPosition )
    {
        if ( !MidiMapGroupPositions.TryGetValue( aMode, out var list ) )
        {
            list = [];
        }

        list [ aGroupKey ] = aMidiMapGroupPosition.Clone();

        MidiMapGroupPositions [ aMode ] = list;
    }

    #endregion

    #region Function

    /// <summary>
    /// MidiMapSetを複製
    /// </summary>
    /// <returns>MidiMapSet</returns>
    public MidiMapSet Clone()
    {
        var midiMapSet = new MidiMapSet();

        foreach ( var group in MidiMapGroups )
        {
            midiMapSet.AddMidiMapGroup( group );
        }

        CopyToMidiMapGroupPosition( ref midiMapSet );

        midiMapSet.UpdateInfo();

        return midiMapSet;
    }

    /// <summary>
    /// MidiMapGroup位置情報を指定したMidiMapSetへコピー
    /// </summary>
    /// <param name="aMidiMapSet">設定先MidiMapSet</param>
    public void CopyToMidiMapGroupPosition( ref MidiMapSet aMidiMapSet )
    {
        foreach ( var item in MidiMapGroupPositions )
        {
            foreach ( var dgp in item.Value )
            {
                aMidiMapSet.AddMidiMapGroupPosition( item.Key, dgp.Key, dgp.Value );
            }
        }
    }

    /// <summary>
    /// 指定したMidiMapキーのグループ＋MidiMap名称を取得
    /// </summary>
    /// <param name="aMidiMapKey">MidiMapキー</param>
    /// <returns>一致：グループ＋MidiMap名称、不一致：空文字</returns>
    public string GetGroupAndMidiMapName( int aMidiMapKey )
    {
        var index = GetMidiMapIndex( aMidiMapKey );

        if ( index == -1 )
        {
            return string.Empty;
        }

        var midiMap = GetMidiMap( index );

        var display = ( midiMap?.Group?.Display ?? false ) && ( midiMap?.Display ?? false ) ? "〇" : "✕" ;

        return $"{midiMap?.Group?.GroupName,-20} {midiMap?.MidiMapName,-20} {display}";
    }

    /// <summary>
    /// 指定したMIDIノート番号に最初に一致するMidiMapキーを取得
    /// </summary>
    /// <param name="aMidi">MIDIノート番号</param>
    /// <returns>一致：MidiMapKey、不一致：-1</returns>
    public int GetMidiMapKeyForMatchMidi( byte aMidi )
    {
        foreach ( var midiMap in MidiMaps )
        {
            if ( midiMap.Midi == aMidi )
            {
                return midiMap.MidiMapKey;
            }
        }
        return ConfigLib.System.MidiMapKeyNotSelect;
    }

    /// <summary>
    /// 選択中のMidiMapGroup/MidiMap有無判定
    /// </summary>
    public bool IsSelectMidiMap() 
        => DisplayMidiMaps.Where( item => item.AnySelected ).Any();

    #endregion
}
