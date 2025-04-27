using System;
using System.Collections.Generic;
using System.Threading;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using NAudio;
using NAudio.Core.Dsp;
using NAudio.Core.Wave.WaveFormats;
using NAudio.Core.Wave.WaveOutputs;
using NAudio.WinMM;
using Windows.Foundation;

namespace DrumMidiLibrary.pAudio;

/// <summary>
/// NAudioData
/// </summary>
public partial class NAudioData : DisposeBaseClass
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aFilePath">オーディオファイルパス</param>
    public NAudioData( GeneralPath aFilePath )
    {
        CreateStreamFromFile( aFilePath.AbsoluteFilePath );
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
            RemoveStream();
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
    /// ロック用
    /// </summary>
    private readonly Lock _LockObj = new();

    /// <summary>
    /// Audioファイル読込
    /// </summary>
    private AudioFileReader? _Reader = null;

    /// <summary>
    /// サンプリング
    /// </summary>
    private Sampling? _Sample = null;

    /// <summary>
    /// Wave出力イベント
    /// </summary>
    private WaveOutEvent? _Wave = null;

    /// <summary>
    /// 高速FFT計算結果
    /// </summary>
    private float[,]? _FFTBuffer = null;

    /// <summary>
    /// 高速FFT計算完了フラグ
    /// </summary>
    private bool _IsFFTLoad = false;

    /// <summary>
    /// イコライザ設定
    /// </summary>
    private readonly Dictionary<int, EqualizerBand> _EqualizerBand = [];

    #endregion

    #region FileStream

    /// <summary>
    /// オーディオファイルストリーム作成
    /// </summary>
    /// <param name="aFilePath">オーディオファイルパス</param>
    private void CreateStreamFromFile( string aFilePath )
    {
        try
        {
            lock ( _LockObj )
            {
                _Reader = new( aFilePath );
                _Sample = new( _Reader );
                _Wave   = new()
                {
                    DesiredLatency  = (int)( ConfigLib.Media.CheckBgmDesiredLatency( ConfigLib.Media.BgmDesiredLatency ) * 1000 ),
                    NumberOfBuffers = ConfigLib.Media.CheckBgmNumberOfBuffers( ConfigLib.Media.BgmNumberOfBuffers ),
                };

                _Wave.Init( _Sample );
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
            throw new InvalidOperationException( $"Failure load audio:{aFilePath}" );
        }
    }

    /// <summary>
    /// オーディオファイルストリーム解放
    /// </summary>
    private void RemoveStream()
    {
        try
        {
            lock ( _LockObj )
            {
                // オーディオ読込解放
                _Wave?.Dispose();
                _Sample?.Dispose();
                _Reader?.Dispose();

                _FFTBuffer = null;

                _EqualizerBand?.Clear();
            }
        }
        catch ( Exception e )
        {
            Log.Warning( e );
        }
    }

    /// <summary>
    /// BGM読込成否
    /// </summary>
    /// <returns>True:読込完了、False:読込未完了</returns>
    public bool IsEnable() => _Reader != null && _Wave != null;

    #endregion

    #region Playback

    /// <summary>
    /// 再生合計時間（秒）取得
    /// </summary>
    /// <returns>再生合計時間（秒）</returns>
    public TimeSpan GetDuration()
    {
        var tm = new TimeSpan();

        lock ( _LockObj )
        {
            try
            {
                if ( _Reader != null )
                {
                    tm = TimeSpan.FromSeconds( _Reader.TotalTime.TotalSeconds );
                }
            }
            catch ( Exception e )
            {
                Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
            }
        }
        return tm;
    }

    /// <summary>
    /// 現在再生中の時間（秒）
    /// </summary>
    /// <returns>再生中の時間（秒）</returns>
    public double GetCurrentTime()
    {
        lock ( _LockObj )
        {
            if ( _Reader == null || _Wave == null )
            {
                return 0;
            }

            // ストリームの位置から算出
            return (double)_Wave.GetPosition() / _Reader.WaveFormat.AverageBytesPerSecond;

            // カレントタイム
            //return _Reader.CurrentTime.TotalSeconds;
        }
    }

    /// <summary>
    /// 指定の再生位置へ移動
    /// </summary>
    /// <param name="aCurrentTime">再生時間（秒）</param>
    public void SetCurrentTime( double aCurrentTime )
    {
        lock ( _LockObj )
        {
            if ( _Reader != null )
            {
                // 再生位置変更
                _Reader.CurrentTime = TimeSpan.FromSeconds( aCurrentTime );
            }
        }
    }

    /// <summary>
    /// 再生開始
    /// </summary>
    public void Play()
    {
        lock ( _LockObj )
        {
            // 再生開始
            _Wave?.Play();
        }
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    public void Pause()
    {
        lock ( _LockObj )
        {
            // 再生停止
            _Wave?.Pause();
        }
    }

    /// <summary>
    /// 再生停止
    /// </summary>
    public void Stop()
    {
        lock ( _LockObj )
        {
            // 再生停止
            _Wave?.Stop();

            if ( _Reader != null )
            {
                // 再生位置を先頭に戻す
                _Reader.Position = 0;
            }
        }
    }

    /// <summary>
    /// 再生中判定
    /// </summary>
    /// <returns>True:再生中、False:停止中</returns>
    public bool IsPlaying()
    {
        lock ( _LockObj )
        {
            return _Wave != null && _Wave.PlaybackState == PlaybackState.Playing;
        }
    }

    #endregion

    #region Volume

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns>音量(100基準)</returns>
    public int GetVolume()
    {
        lock ( _LockObj )
        {
            return (int)( ( _Wave?.Volume ?? 0 ) * MaxVolume );
        }
    }

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="aVolume">音量(100基準)</param>
    public void SetVolume( int aVolume )
    {
        lock ( _LockObj )
        {
            if ( _Wave != null )
            {
                // 音量変更(0-1)
                _Wave.Volume = CheckVolume( aVolume ) / (float)MaxVolume;
            }
        }
    }

    /// <summary>
    /// 最小音量
    /// </summary>
    public static int MinVolume { get; private set; } = 0;

    /// <summary>
    /// 最大音量
    /// </summary>
    public static int MaxVolume { get; private set; } = 100;

    /// <summary>
    /// BGM音量チェック
    /// </summary>
    /// <param name="aVolume">音量</param>
    /// <returns>範囲内の音量(0-100)</returns>
    public static int CheckVolume( int aVolume )
        => Math.Clamp( aVolume, MinVolume, MaxVolume );

    #endregion

    #region 周波数解析

    /// <summary>
    /// 周波数解析：FFTレングス
    /// 
    /// 10=1024,11=2048,12=4096,13=8192,14=16394,15=32798
    /// </summary>
    public int FFTLength { get; private set; } = (int)Math.Pow( 2d, 10d );

    /// <summary>
    /// チェンネル数
    /// </summary>
    public int Channels => _Reader?.WaveFormat.Channels ?? 0;

    /// <summary>
    /// サンプルレート取得
    /// </summary>
    /// <returns>サンプルレート</returns>
    public int GetSampleRate()
    {
        lock ( _LockObj )
        {
            return _Reader?.WaveFormat.SampleRate ?? 0;
        }
    }

    /// <summary>
    /// FFT解析：オフセット位置の再生時間（秒）取得
    /// </summary>
    /// <param name="aFFTOfsetPositionX">オフセット（周波数分解数）</param>
    /// <returns>再生時間（秒）</returns>
    public double GetFFTTime( long aFFTOfsetPositionX )
    {
        lock ( _LockObj )
        {
            return _Reader == null
                ? 0
                : GetDuration().TotalSeconds
                * aFFTOfsetPositionX * FFTBufferLength1
                / ( _Reader.Length / _Reader.BlockAlign );
        }
    }

    /// <summary>
    /// FFT解析：FFTバッファ 周波数分解数
    /// </summary>
    public int FFTBufferLength0 => _FFTBuffer?.GetLength( 0 ) ?? 0;

    /// <summary>
    /// FFT解析：FFTバッファ ブロックサイズ÷２
    /// </summary>
    public int FFTBufferLength1 => _FFTBuffer?.GetLength( 1 ) ?? 0;

    /// <summary>
    /// FFT解析：指定オフセット位置の周波数を取得
    /// </summary>
    /// <param name="aOffset">オフセット（周波数分解数）</param>
    /// <returns>周波数別Dbリスト</returns>
    public List<float> GetFFTBuffer( int aOffset )
    {
        var list = new List<float>();

        if ( _FFTBuffer != null && aOffset < _FFTBuffer.GetLength( 0 ) )
        {
            list.Capacity = _FFTBuffer.GetLength( 1 );

            for ( var k = 0; k < _FFTBuffer.GetLength( 1 ); k++ )
            {
                list.Add( _FFTBuffer [ aOffset, k ] );
            }
        }

        return list;
    }

    /// <summary>
    /// FFT解析：FFTバッファ内の指定範囲の周波数を取得
    /// </summary>
    /// <param name="aRange">切出範囲</param>
    /// <param name="aDbFileter">この値より小さいdbは0として返す</param>
    /// <returns>周波数別Dbリスト</returns>
    public List<float> GetFFTBuffer( Rect aRange, float aDbFileter )
    {
        var list = new List<float>();

        if ( _FFTBuffer != null )
        {
            for ( var y = (int)aRange.Top; y < aRange.Bottom; y++ )
            {
                for ( var x = (int)aRange.Left; x < aRange.Right; x++ )
                {
                    var db = _FFTBuffer[ x, y ];

                    if ( db < aDbFileter )
                    {
                        db = 0;
                    }

                    list.Add( db );
                }
            }
        }

        return list;
    }

    /// <summary>
    /// FFT解析：現在再生中のオフセット（周波数分解数）位置取得
    /// </summary>
    public int FFTPlayingOffset
        => _Reader == null || _FFTBuffer == null
                ? 0 : (int)( _Reader.Position / (double)_Reader.Length * _FFTBuffer.GetLength( 0 ) );

    /// <summary>
    /// FFT解析：現在再生中のオフセット（周波数分解数）位置の周波数を取得
    /// </summary>
    /// <returns>周波数別Dbリスト</returns>
    public List<float> GetFFTPlaying()
    {
        // 音声波形表示を描画する配列のオフセット(インデックス)を計算
        var offset = FFTPlayingOffset;

        var list = new List<float>();

        if ( _FFTBuffer != null )
        {
            if ( offset < _FFTBuffer.GetLength( 0 ) )
            {
                for ( var k = 0; k < _FFTBuffer.GetLength( 1 ); k++ )
                {
                    list.Add( _FFTBuffer [ offset, k ] );
                }
            }
        }

        return list;
    }

    /// <summary>
    /// FFTバッファ読込成否
    /// </summary>
    /// <returns>True:読込完了、False:読込未完了</returns>
    public bool IsEnableFFT() => _IsFFTLoad;

    /// <summary>
    /// FFT解析：FFTバッファ オフセット位置のHzを取得
    /// </summary>
    /// <param name="aOffsetY">オフセット（ブロックサイズ÷２）</param>
    /// <returns>Hz</returns>
    public float GetHz( int aOffsetY )
        => (float)GetSampleRate() / ( FFTLength / 2 * Channels ) * aOffsetY;

    /// <summary>
    /// FFT解析。
    /// refer: https://qiita.com/takesyhi/items/a0f03447bb893c9ab937
    /// </summary>
    public void CalcFFT()
    {
        if ( _Reader == null || _IsFFTLoad )
        {
            return;
        }

        var samples     = new float[ _Reader.Length / _Reader.BlockAlign * _Reader.WaveFormat.Channels ];
        var sampleNum   = _Reader.Read( samples, 0, samples.Length );

        _Reader.Position = 0;

        // 分解脳
        var fftLength   = FFTLength; // (int)Math.Pow( 2d, 10d ); // 10=1024,11=2048,12=4096,13=8192,14=16394,15=32798
        var fftPos      = 0;

        //var hzPerOne = (float)GetSampleRate() / ( fftLength / 2 * Channels );

        _FFTBuffer = new float [ sampleNum / fftLength, fftLength / 2 ];

        var buffer = new Complex[ fftLength ];

        for ( var i = 0; i < sampleNum; i++ )
        {
        //  buffer [ fftPos ].X = (float)( samples[ i ] * FastFourierTransform.BlackmannHarrisWindow( fftPos, fftLength ) );
            buffer [ fftPos ].X = (float)( samples[ i ] * FastFourierTransform.HammingWindow( fftPos, fftLength ) );
        //  buffer [ fftPos ].X = (float)( samples[ i ] * FastFourierTransform.HannWindow( fftPos, fftLength ) );
            buffer [ fftPos ].Y = 0.0f;

            fftPos++;

            if ( fftLength <= fftPos )
            {
                fftPos = 0;

                // 高速フーリエ変換
                FastFourierTransform.FFT( true, (int)Math.Log( fftLength, 2.0d ), buffer );

                for ( var k = 0; k < _FFTBuffer.GetLength( 1 ); k++ )
                {
                    var c = buffer[ k ];

                    var diagonal    = Math.Sqrt( c.X * c.X + c.Y * c.Y );
                    var intensityDB = 10d * Math.Log10( diagonal );

                    var minDB = -90.0d;

                    var percent = intensityDB < minDB ? 1d : intensityDB / minDB;

                    _FFTBuffer [ i / fftLength, k ] = (float)( 1d - percent );
                //  _FFTBuffer [ i / fftLength, k ] = (float)percent;
                }
            }
        }

        _IsFFTLoad = true;
    }

    #endregion

    #region Equalizer

    /// <summary>
    /// イコライザ帯域
    /// </summary>
    private class EqualizerBand
    {
        public float Hz;
        public float Gain;
        public float BandWidth = 0.5F;
    }

    /// <summary>
    /// イコライザ設定。
    /// UpdateEqualizer()実行時に反映。
    /// </summary>
    /// <param name="aKey">設定キー</param>
    /// <param name="aHz">Hz</param>
    /// <param name="aGain">Gain</param>
    public void SetEqualizerGain( int aKey, float aHz, float aGain )
    {
        lock ( _LockObj )
        {
            if ( !_EqualizerBand.TryGetValue( aKey, out var item ) )
            {
                item = new();
            }
            item.Hz   = aHz;
            item.Gain = aGain;

            _EqualizerBand [ aKey ] = item;
        }
    }

    /// <summary>
    /// イコライザ設定反映
    /// </summary>
    public void UpdateEqualizer()
    {
        lock ( _LockObj )
        {
            _Sample?.UpdateEqualizer( _EqualizerBand );
        }
    }

    #endregion

    #region Sampling

    /// <summary>
    /// サンプリング
    /// </summary>
    /// <remarks>
    /// コンストラクタ
    /// 
    /// メモ：IWaveProviderを定義しないと、WaveOutEvent.Initで読み込めなくなった。
    /// 　　　NAudioのサンプルコードでは不要で、クラス分割前は読み込めていたが
    /// 　　　WaveOutEvent.Init(IWaveProvider)なので、むしろ今まではなぜ読み込めていたのだろう・・・
    /// </remarks>
    /// <param name="aSourceProvider"></param>
    private partial class Sampling( ISampleProvider aSourceProvider ) : DisposeBaseClass, ISampleProvider, IWaveProvider
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
                _EqualizerFilter = null;
                _EqualizerBandList.Clear();
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
        /// サンプリングプロバイダー
        /// </summary>
        private readonly ISampleProvider _SampleProvider = aSourceProvider;

        /// <summary>
        /// 波形フォーマット
        /// </summary>
        public WaveFormat WaveFormat => _SampleProvider.WaveFormat;

        /// <summary>
        /// イコライザーフィルター
        /// </summary>
        private BiQuadFilter[,]? _EqualizerFilter;

        /// <summary>
        /// イコライザ更新フラグ
        /// </summary>
        private bool _UpdateEqualizer;

        /// <summary>
        /// イコライザ設定
        /// </summary>
        private readonly List<EqualizerBand> _EqualizerBandList = [];

        #endregion

        /// <summary>
        /// イコライザ設定反映
        /// </summary>
        /// <param name="aEqualizer">イコライザ設定</param>
        public void UpdateEqualizer( Dictionary<int, EqualizerBand> aEqualizer )
        {
            lock ( _EqualizerBandList )
            {
                _EqualizerBandList.Clear();

                foreach ( var band in aEqualizer.Values )
                {
                    _EqualizerBandList.Add( new()
                    {
                        Hz   = band.Hz,
                        Gain = band.Gain
                    } );
                }
            }

            _UpdateEqualizer = true;
        }

        /// <summary>
        /// オーディオバッファ読込
        /// </summary>
        /// <param name="aBuffer">バッファ</param>
        /// <param name="aOffset">オフセット</param>
        /// <param name="aCount">読込数</param>
        /// <returns>読込サンプル数</returns>
        public int Read( float [] aBuffer, int aOffset, int aCount )
        {
            if ( _SampleProvider == null )
            {
                return 0;
            }

            #region Update equalizer

            int bandCnt;

            lock ( _EqualizerBandList )
            {
                bandCnt = _EqualizerBandList.Count;

                if ( _UpdateEqualizer )
                {
                    var ch = _SampleProvider.WaveFormat.Channels;

                    _EqualizerFilter = new BiQuadFilter [ ch, bandCnt ];

                    for ( var b = 0; b < bandCnt; b++ )
                    {
                        var band = _EqualizerBandList[ b ];

                        for ( var c = 0; c < ch; c++ )
                        {
                            _EqualizerFilter [ c, b ] = BiQuadFilter.PeakingEQ
                                (
                                    _SampleProvider.WaveFormat.SampleRate,
                                    band.Hz,
                                    band.BandWidth,
                                    band.Gain
                                );
                        }

                        //Log.Info( $"hz:{band.Hz} gain:{band.Gain}" );
                    }

                    _UpdateEqualizer = false;
                }
            }

            #endregion

            var sampleNum = _SampleProvider.Read( aBuffer, aOffset, aCount );

            #region Sampling
            {
                if ( bandCnt > 0 && _EqualizerFilter != null )
                {
                    for ( var sample = 0; sample < sampleNum; sample++ )
                    {
                        var ch = sample % _SampleProvider.WaveFormat.Channels;

                        for ( var band = 0; band < bandCnt; band++ )
                        {
                            aBuffer [ aOffset + sample ] = _EqualizerFilter [ ch, band ].Transform( aBuffer [ aOffset + sample ] );
                        }
                    }
                }
            }
            #endregion

            return sampleNum;
        }

        /// <summary>
        /// オーディオバッファ読込
        /// </summary>
        /// <param name="aBuffer">バッファ</param>
        /// <param name="aOffset">オフセット</param>
        /// <param name="aCount">読込数</param>
        /// <returns>読込サンプル数</returns>
        public int Read( byte [] aBuffer, int aOffset, int aCount )
        {
            var wb = new WaveBuffer( aBuffer );

            var samplesRead = Read( wb.FloatBuffer, aOffset / 4, aCount / 4 );

            return samplesRead * 4;
        }
    }

    #endregion
}
