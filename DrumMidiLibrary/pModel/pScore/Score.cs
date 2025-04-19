using System.Collections.Generic;
using System.Threading;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pUtil;

namespace DrumMidiLibrary.pModel.pScore;

/// <summary>
/// スコア情報
/// </summary>
public partial class Score : DisposeBaseClass
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Score()
    {
        for ( var channel_no = MidiNet.ChannelMinNo; channel_no <= MidiNet.ChannelMaxNo; channel_no++ )
        {
            Channels.Add( channel_no, new( channel_no ) );
        }
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( _Disposed )
        {
            return;
        }

        // マネージドリソースの解放
        if ( aDisposing )
        {
            ClearChannel();
        }

        // アンマネージドリソースの解放
        {
        }

        _Disposed = true;

        base.Dispose( aDisposing );
    }
    private bool _Disposed = false;

    #region member

    /// <summary>
    /// スコアロック用オブジェクト
    /// </summary>
    public readonly Lock LockObj = new();

    /// <summary>
    /// ファイルパス
    /// </summary>
    public GeneralPath FilePath { get; set; } = ConfigLib.System.DefaultScoreFilePath;

    /// <summary>
    /// メモ情報
    /// </summary>
    public string Info { get; set; } = ConfigLib.System.DefaultScoreInfo;

    /// <summary>
    /// BGMファイルパス
    /// </summary>
    public GeneralPath BgmFilePath { get; set; } = ConfigLib.System.DefaultBgmFilePath;

    /// <summary>
    /// BGM再生開始位置（秒）
    /// </summary>
    public double BgmPlaybackStartPosition { get; set; } = ConfigLib.System.DefaultBgmPlaybackStartPosition;

    /// <summary>
    /// ベースBPM
    /// </summary>
    public double Bpm { get; set; } = ConfigLib.System.DefaultBpm;

    /// <summary>
    /// BGM音量(100基準)
    /// </summary>
    public int BgmVolume { get; set; } = ConfigLib.System.DefaultBgmVolume;

    /// <summary>
    /// システムチャンネル
    /// </summary>
    public ChannelSys SysChannel { get; private set; } = new();

    /// <summary>
    /// チャンネルリスト（チャンネル番号(0-15)、チャンネル情報）
    /// </summary>
    public Dictionary<byte, Channel> Channels { get; private set; } = [];

    /// <summary>
    /// 編集中のチャンネル番号
    /// </summary>
    public byte EditChannelNo { get; set; } = MidiNet.ChannelDrum;

    /// <summary>
    /// 編集中のチャンネル情報
    /// </summary>
    public Channel EditChannel => Channels [ EditChannelNo ];

    /// <summary>
    /// 編集中のチャンネル情報のMidiMapSet
    /// </summary>
    public MidiMapSet EditMidiMapSet
    {
        get => Channels [ EditChannelNo ].MidiMapSet;
        set => Channels [ EditChannelNo ].MidiMapSet = value;
    }

    #endregion

    #region Function

    /// <summary>
    /// MidiMapSetを複製
    /// </summary>
    /// <returns>MidiMapSet</returns>
    public Score Clone()
    {
        var score = new Score()
        {
            FilePath                    = FilePath,
            Info                        = Info,
            BgmFilePath                 = BgmFilePath,
            BgmPlaybackStartPosition    = BgmPlaybackStartPosition,
            Bpm                         = Bpm,
            BgmVolume                   = BgmVolume,
            EditChannelNo               = EditChannelNo,
            SysChannel                  = SysChannel.Clone(),
        };

        foreach ( var item in Channels )
        {
            score.Channels [ item.Key ] = item.Value.Clone();
        }

        return score;
    }

    /// <summary>
    /// データクリア（チャンネル情報）
    /// </summary>
    public void ClearChannel()
    {
        SysChannel.ClearAll();

        foreach ( var channel in Channels.Values )
        {
            channel.ClearAll();
        }
    }

    /// <summary>
    /// NOTE/BPMが存在する小節番号最大値
    /// </summary>
    public int GetMaxMeasureNo()
    {
        var max = SysChannel.MaxMeasureNo;

        foreach ( var channel in Channels.Values )
        {
            if ( channel.MaxMeasureNo > max )
            {
                max = channel.MaxMeasureNo;
            }
        }

        return max;
    }

    /// <summary>
    /// 全チャンネルのMidiMapSet更新処理を実行
    /// </summary>
    public void UpdateInfoAllMidiMaps()
    {
        foreach ( var channel in Channels.Values )
        {
            channel.MidiMapSet.UpdateInfo();
        }
    }

    /// <summary>
    /// MidiMapSetを一つ取得。
    /// チェンネルを昇順で検索し、MidiMapGroupが１つでも設定されている情報を対象とします。
    /// </summary>
    /// <returns></returns>
    public MidiMapSet? GetFirstMidiMapSet()
    {
        foreach ( var channel in Channels.Values )
        {
            if ( channel.MidiMapSet.MidiMapGroups.Count > 0 )
            {
                return channel.MidiMapSet;
            }
        }
        return null;
    }

    #endregion
}
