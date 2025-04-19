using System;
using System.Drawing;
using System.Drawing.Imaging;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using OpenCvSharp;

namespace DrumMidiLibrary.pIO.pVideo;

/// <summary>
/// MP4出力
/// </summary>
public partial class Mp4IO : DisposeBaseClass
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
            Close();
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
    /// Video書込
    /// </summary>
    private VideoWriter? _Writer = null;

    /// <summary>
    /// 画像コンバーター
    /// </summary>
    private readonly ImageConverter _Converter = new();

    /// <summary>
    /// フレーム書込用Bitmap
    /// </summary>
    private Bitmap? _Bmp = null;

    #endregion

    /// <summary>
    /// ファイルオープン
    /// </summary>
    /// <param name="aGeneralPath">出力ファイルパス</param>
    /// <param name="aCodec">MP4コーデック</param>
    /// <param name="aFrameRate">フレームレート</param>
    /// <param name="aWidth">映像横幅</param>
    /// <param name="aHeight">映像高さ</param>
    /// <returns>映像書込用のBitmap</returns>
    public Bitmap Open( GeneralPath aGeneralPath, string aCodec, int aFrameRate, int aWidth, int aHeight )
    {
        Close();

        _Writer = new VideoWriter
            (
                aGeneralPath.AbsoluteFilePath,
                string.IsNullOrEmpty( aCodec ) ? FourCC.Default : FourCC.FromString( aCodec ),
                aFrameRate,
                new( aWidth, aHeight )
            );

        _Bmp = new Bitmap( aWidth, aHeight, PixelFormat.Format32bppArgb );

        return _Bmp;
    }

    /// <summary>
    /// フレーム追加
    /// </summary>
    /// <returns>True:書込成功、False:書込失敗</returns>
    public bool AddFrame()
    {
        if ( _Writer == null || _Bmp == null )
        {
            return false;
        }

        BitmapData? bmpData = null;

        try
        {
            bmpData = _Bmp.LockBits
                (
                    new( 0, 0, _Bmp.Width, _Bmp.Height ),
                    ImageLockMode.ReadOnly,
                    _Bmp.PixelFormat
                );

            var buffer = (byte[]?)_Converter.ConvertTo( _Bmp, typeof( byte[] ) );

            if ( buffer == null )
            {
                return false;
            }

            using var mat = Mat.FromImageData( buffer );
        //  using var mat = Mat.FromImageData( buffer ).CvtColor( ColorConversionCodes.RGBA2RGB );
        //  using var mat = Mat.FromImageData( buffer ).CvtColor( ColorConversionCodes.RGB2BGR );

            _Writer.Write( mat );

            return true;
        }
        catch ( Exception e )
        {
            Log.Error( e );
            throw new InvalidOperationException( $"Failure Add Frame", e );
        }
        finally
        {
            if ( bmpData != null )
            {
                // Bitmapのロック解除
                _Bmp.UnlockBits( bmpData );
            }
        }
    }

    /// <summary>
    /// ファイルクローズ
    /// </summary>
    public void Close()
    {
        _Writer?.Dispose();
        _Writer = null;

        _Bmp?.Dispose();
        _Bmp = null;
    }
}
