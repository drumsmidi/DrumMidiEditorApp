using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using DrumMidiEditorApp.pUtil;
using OpenCvSharp;

namespace DrumMidiEditorApp.pIO.pVideo;

/// <summary>
/// PDF出力
/// </summary>
public partial class PdfIO : DisposeBaseClass
{
    #region member

    /// <summary>
    /// PDF書込
    /// </summary>
    private FileStream? _Writer = null;

    /// <summary>
    /// 画像コンバーター
    /// </summary>
    private readonly ImageConverter _Converter = new();

    /// <summary>
    /// フレーム書込用Bitmap
    /// </summary>
    private Bitmap? _Bmp = null;

    /// <summary>
    /// オブジェクト番号
    /// </summary>
    private int _ObjectNo = 0;

    /// <summary>
    /// 改行コード文字列
    /// </summary>
    private const string CRLF = "\r\n";

    /// <summary>
    /// バイト配列をPDFストリームへ出力
    /// </summary>
    /// <param name="aBuffer"></param>
    private void WriteLine( byte[] aBuffer )
    {
        _Writer?.Write( aBuffer, 0, aBuffer.Length );
        WriteLine( $"" );
    }

    /// <summary>
    /// 文字列をPDFストリームへ出力
    /// </summary>
    /// <param name="aString"></param>
    private void WriteLine( string aString )
    {
        var bytes = GetBytes( aString + CRLF );

        _Writer?.Write( bytes, 0, bytes.Length );
    }

    /// <summary>
    /// 文字列をバイト配列に変換
    /// </summary>
    /// <param name="aString"></param>
    /// <returns></returns>
    private byte [] GetBytes( string aString )
        => Encoding.ASCII.GetBytes( aString );

    /// <summary>
    /// オブジェクト位置情報
    /// </summary>
    private readonly ArrayList _ObjectXref = [];

    /// <summary>
    /// オブジェクト位置情報追加
    /// </summary>
    private void AddObjectXref()
        => _ObjectXref.Add( _Writer?.Length );

    #endregion

    /// <summary>
    /// ファイルオープン
    /// </summary>
    /// <param name="aGeneralPath">出力ファイルパス</param>
    /// <param name="aMaxPage">描画ページ数</param>
    /// <param name="aWidth">映像横幅</param>
    /// <param name="aHeight">映像高さ</param>
    /// <returns>映像書込用のBitmap</returns>
    public Bitmap Open( GeneralPath aGeneralPath, int aMaxPage, int aWidth, int aHeight )
    {
        Close();

        _Writer = new FileStream
            (
                aGeneralPath.AbsoulteFilePath,
                FileMode.Create,
                FileAccess.Write
            );

        _Bmp = new Bitmap( aWidth, aHeight, PixelFormat.Format32bppArgb );

        // PDFの仕様参考
        // https://www.pdf-tools.trustss.co.jp/Syntax/catalog.html

        _ObjectNo = 0;

        #region ヘッダー
        {
            WriteLine( $"%PDF-1.7" );
            WriteLine( $"" );
        }
        #endregion

        #region 文章カタログ
        {
            AddObjectXref();

            _ObjectNo++;

            WriteLine( $"{_ObjectNo} 0 obj" );
            WriteLine( $"<<" );
            WriteLine( $"/Pages {_ObjectNo + 1} 0 R" );
            WriteLine( $"/Type /Catalog" );
            WriteLine( $">>" );
            WriteLine( $"endobj" );
        }
        #endregion

        #region ページツリー
        {
            AddObjectXref();

            _ObjectNo++;

            WriteLine( $"{_ObjectNo} 0 obj" );
            WriteLine( $"<<" );
            WriteLine( $"/Count {aMaxPage}" );

            var kids = "";
            for ( var p = 0; p < aMaxPage; p++ )
            {
                kids += $"{3 + p * 4} 0 R ";
            }

            WriteLine( $"/Kids [ {kids}]" );
            WriteLine( $"/Type /Pages" );
            WriteLine( $">>" );
            WriteLine( $"endobj" );
        }
        #endregion

        return _Bmp;
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                Close();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

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

        var bmpData = _Bmp.LockBits
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

        using var mat = Mat.FromImageData( buffer ).CvtColor( ColorConversionCodes.RGBA2RGB );
    //  using var mat = Mat.FromImageData( buffer ).CvtColor( ColorConversionCodes.RGB2BGR );

        // オブジェクトの開始バイト位置
        var obj_pos = _Writer?.Length ?? 0 ;

        #region ページ
        {
            AddObjectXref();

            _ObjectNo++;

            WriteLine( $"{_ObjectNo} 0 obj" );
            WriteLine( $"<<" );
            WriteLine( $"/Contents [ {_ObjectNo + 2} 0 R ]" );
            WriteLine( $"/CropBox [ 0.0 0.0 {_Bmp.Width} {_Bmp.Height} ]" );
            WriteLine( $"/MediaBox [ 0.0 0.0 {_Bmp.Width} {_Bmp.Height} ]" );     // ページの領域
            WriteLine( $"/Parent 2 0 R" );
            WriteLine( $"/Resources {_ObjectNo + 3} 0 R" );
            WriteLine( $"/Rotate 0" );
            WriteLine( $"/Type /Page" );
            WriteLine( $">>" );
            WriteLine( $"endobj" );
        }
        #endregion

        #region 画像：
        {
            AddObjectXref();

            _ObjectNo++;

            var img = mat.ToBytes( ".jpg" );

            WriteLine( $"{_ObjectNo} 0 obj" );
            WriteLine( $"<<" );
            WriteLine( $"/BitsPerComponent 8" );
            WriteLine( $"/ColorSpace /DeviceRGB" );
            WriteLine( $"/Filter /DCTDecode" );
            WriteLine( $"/Height {_Bmp.Height}" );
            WriteLine( $"/Length {img.LongLength}" );
            WriteLine( $"/Subtype /Image" );
            WriteLine( $"/Type /XObject" );
            WriteLine( $"/Width {_Bmp.Width}" );
            WriteLine( $">>" );
            WriteLine( $"stream" );
            WriteLine( img );
            WriteLine( $"endstream" );
            WriteLine( $"endobj" );
        }
        #endregion

        #region 描画位置など
        {
            AddObjectXref();

            _ObjectNo++;

            // 参考：https://azelpg.gitlab.io/azsky2/note/prog/pdf/13_image.html
            // (100, 500) に移動して 200 に拡大
            // 200 0 0 200 100 500 cm

            var pos = GetBytes( $"q{CRLF}{_Bmp.Width} 0 0 {_Bmp.Height} 0 0 cm{CRLF}/Image1 Do{CRLF}Q" );

            WriteLine( $"{_ObjectNo} 0 obj" );
            WriteLine( $"<<" );
        //  WriteLine( $"/Filter /FlateDecode" );
            WriteLine( $"/Length {pos.LongLength}" );
            WriteLine( $">>" );
            WriteLine( $"stream" );
            WriteLine( pos );
            WriteLine( $"endstream" );
            WriteLine( $"endobj" );
        }
        #endregion

        #region ページ内のリソースディクショナリ設定
        {
            AddObjectXref();

            _ObjectNo++;

            WriteLine( $"{_ObjectNo} 0 obj" );
            WriteLine( $"<<" );
            WriteLine( $"/XObject" );
            WriteLine( $"<<" );
            WriteLine( $"/Image1 {_ObjectNo - 2} 0 R" );
            WriteLine( $">>" );
            WriteLine( $">>" );
            WriteLine( $"endobj" );
        }
        #endregion

        _Bmp.UnlockBits( bmpData );

        return true;
    }

    /// <summary>
    /// ファイルクローズ
    /// </summary>
    public void Close()
    {
        // xrefの開始バイト位置
        var xref_pos = _Writer?.Length ?? 0 ;

        // クロスリファレンス数
        var size = _ObjectXref.Count + 1;

        #region クロスリファレンス・テーブル
        {
            WriteLine( $"xref" );
            WriteLine( $"0 {size}" );
            WriteLine( $"0000000000 65535 f" );

            foreach ( var pos in _ObjectXref )
            {
                WriteLine( $"{Convert.ToUInt64( pos ):0000000000} 00000 n" );
            }
        }
        #endregion

        #region trailer
        {
            WriteLine( $"trailer" );
            WriteLine( $"<<" );
            WriteLine( $"/Root 1 0 R" );
            WriteLine( $"/Size {size}" );
            WriteLine( $">>" );
            WriteLine( $"startxref" );
            WriteLine( $"{xref_pos}" );
            WriteLine( $"%%EOF" );
        }
        #endregion

        _Writer?.Close();
        _Writer?.Dispose();
        _Writer = null;

        _Bmp?.Dispose();
        _Bmp = null;

        _ObjectXref.Clear();
    }
}
