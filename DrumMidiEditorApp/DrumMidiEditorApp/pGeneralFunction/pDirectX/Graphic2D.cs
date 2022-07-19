using System;
using System.Collections.Generic;
using System.Drawing;

using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pGeneralFunction.pDirectX;

/// <summary>
/// Direct2D
/// </summary>
public class Graphic2D : DisposeBaseClass
{
 //   #region Member

 //   /// <summary>
 //   /// 親フォームのハンドル
 //   /// </summary>
 //   private readonly IntPtr	_Handle = IntPtr.Zero;

	///// <summary>
	///// グラフィックデバイス
	///// </summary>
	//private Device? _GraphicDevice = null;

	///// <summary>
	///// スワップチェイン
	///// </summary>
	//private SwapChain? _SwapChain = null;

	///// <summary>
	///// レンダーターゲット
	///// </summary>
	//private RenderTarget? _RenderTarget = null;

	///// <summary>
	///// バックバッファ表示モード
	///// </summary>
	//private ModeDescription _ModeDescription;

	///// <summary>
	///// Direct2Dリソース
	///// </summary>
	//private SharpDX.Direct2D1.Factory? _Factory2D = null;

	///// <summary>
	///// DirectWriteリソース
	///// </summary>
	//private SharpDX.DirectWrite.Factory? _FactoryWrite = null;

	///// <summary>
	///// ブラシディクショナリ
	///// </summary>
	//private readonly Dictionary<int, Brush> _BrushDictionary = new();

	///// <summary>
	///// テキストフォーマットディクショナリ
	///// </summary>
	//private readonly Dictionary<int, TextFormat> _TextFormatDictionary = new();

	///// <summary>
	///// 背景色
	///// </summary>
	//private RawColor4 _BackGroundColor = new();

 //   #endregion

 //   /// <summary>
 //   /// コンストラクタ
 //   /// </summary>
 //   /// <param name="aHandle">親フォームのハンドル</param>
 //   public Graphic2D( IntPtr aHandle )
	//{
	//	_Handle = aHandle;
	//}

	///// <summary>
	///// デバイス初期化
	///// </summary>
	///// <param name="aResolutionWidth">解像度横幅</param>
	///// <param name="aResolutionHeight">解像度高さ</param>
	//public void Initialize( int aResolutionWidth, int aResolutionHeight )
	//{ 
	//	try
	//	{
	//		#region デバイス生成
	//		{
	//			this._ModeDescription = new()
	//				{
	//					Width		= aResolutionWidth,
	//					Height		= aResolutionHeight,
	//					RefreshRate = new( 60, 1 ),
	//					Format		= Format.R8G8B8A8_UNorm,
	//					Scaling		= DisplayModeScaling.Unspecified
	//				};

	//			Device.CreateWithSwapChain
	//				(
	//					DriverType.Hardware,
	//					DeviceCreationFlags.BgraSupport,
	//					new()
	//					{
	//						BufferCount			= 1,
	//						OutputHandle		= _Handle,
	//						IsWindowed			= true,
	//						SampleDescription	= new( 1, 0 ),
	//						ModeDescription		= _ModeDescription,
	//						Usage				= Usage.RenderTargetOutput,
	//						Flags				= SwapChainFlags.AllowModeSwitch,
	//						SwapEffect			= SwapEffect.Discard,
	//					},
	//					out _GraphicDevice,
	//					out _SwapChain
	//				);

	//			// Windowsの不要なイベントを無効にする
	//			var factory = this._SwapChain.GetParent<SharpDX.DXGI.Factory>();
	//			factory.MakeWindowAssociation( this._Handle, WindowAssociationFlags.IgnoreAll );
	//		}
	//		#endregion

	//		#region Direct2Dリソースを作成
	//		{
	//			_Factory2D = new SharpDX.Direct2D1.Factory();

	//			using var backBuffer = Texture2D.FromSwapChain<Texture2D>( _SwapChain, 0 );

	//			using ( var surface = backBuffer.QueryInterface<Surface>() )
	//			{
	//				_RenderTarget = new
	//					(
	//						_Factory2D,
	//						surface,
	//						new( new( Format.Unknown, AlphaMode.Premultiplied ) )
	//					);
	//			}
	//			// 非テキストプリミティブのエッジのレンダリング方法を指定
	//			_RenderTarget.AntialiasMode = AntialiasMode.PerPrimitive;

	//			// テキスト描画に使用されるアンチエイリアスモードについて指定
	//			_RenderTarget.TextAntialiasMode = TextAntialiasMode.Cleartype;
	//		}
 //           #endregion

	//		#region DirectWriteリソースを作成
	//		{
	//			_FactoryWrite = new SharpDX.DirectWrite.Factory();
	//		}
 //           #endregion
 //       }
	//	catch ( Exception e )
	//	{
 //           Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
	//	}
	//}

	//protected override void Dispose( bool aDisposing )
	//{
	//	if ( !_Disposed )
	//	{
	//		if ( aDisposing )
	//		{
	//			// Dispose managed resources.
	//			_RenderTarget?.Dispose();
	//			_SwapChain?.Dispose();
	//			_GraphicDevice?.Dispose();
	//		}

	//		// Dispose unmanaged resources.

	//		_Disposed = true;

	//		// Note disposing has been done.
	//		base.Dispose( aDisposing );
	//	}
	//}
	//private bool _Disposed = false;

	///// <summary>
	///// 解像度変更
	///// </summary>
	///// <param name="aResolutionWidth">解像度横幅</param>
	///// <param name="aResolutionHeight">解像度高さ</param>
	//public void ResizeBuffer( int aResolutionWidth, int aResolutionHeight )
	//{
	//	if ( _GraphicDevice == null )
 //       {
	//		return;
 //       }

	//	_RenderTarget?.Dispose();

	//	// ウィンドウサイズの変更
	//	_ModeDescription.Width  = aResolutionWidth;
	//	_ModeDescription.Height = aResolutionHeight;
	//	_SwapChain?.ResizeTarget( ref _ModeDescription );

	//	// 解像度変更
	//	_SwapChain?.ResizeBuffers
	//		(
	//			1,
	//			aResolutionWidth,
	//			aResolutionHeight,
	//			Format.R8G8B8A8_UNorm,
	//			SwapChainFlags.AllowModeSwitch
	//		);

	//	using var backBuffer = Texture2D.FromSwapChain<Texture2D>( _SwapChain, 0 );

	//	using ( var surface = backBuffer.QueryInterface<Surface>() )
	//	{
	//		_RenderTarget = new
	//			(
	//				_Factory2D,
	//				surface,
	//				new( new( Format.Unknown, AlphaMode.Premultiplied ) )
	//			);
	//	}
	//	// 非テキストプリミティブのエッジのレンダリング方法を指定
	//	_RenderTarget.AntialiasMode = AntialiasMode.PerPrimitive;

	//	// テキスト描画に使用されるアンチエイリアスモードについて指定
	//	_RenderTarget.TextAntialiasMode = TextAntialiasMode.Cleartype;
	//}

 //   #region 描画関連

	///// <summary>
	///// スクリーンショット取得
	///// </summary>
	///// <returns>成功:Bitmap画像、失敗:null</returns>
 //   public System.Drawing.Bitmap? GetScreenShot() 
	//{
	//	if ( _GraphicDevice == null )
 //       {
	//		return null;
 //       }

	//	using var backBuffer = Texture2D.FromSwapChain<Texture2D>( _SwapChain, 0 );

	//	var desc = new Texture2DDescription() 
	//	{
	//		Width				= backBuffer.Description.Width,
	//		Height				= backBuffer.Description.Height,
	//		MipLevels			= 1,
	//		ArraySize			= 1,
	//		Format				= backBuffer.Description.Format,
	//		Usage				= ResourceUsage.Staging,
	//		SampleDescription	= new( 1, 0 ),
	//		BindFlags			= BindFlags.None,
	//		CpuAccessFlags		= CpuAccessFlags.Read,
	//		OptionFlags			= ResourceOptionFlags.None,
	//	};

 //       using var copyTexture = new Texture2D( this._GraphicDevice, desc );

	//	_GraphicDevice.ImmediateContext.CopyResource( backBuffer, copyTexture );

 //       var mapResource = _GraphicDevice.ImmediateContext.MapSubresource
	//	( 
	//		copyTexture, 
	//		0, 
	//		MapMode.Read, 
	//		SharpDX.Direct3D11.MapFlags.None 
	//	);

 //       var texturePtr = mapResource.DataPointer;

	//	var bmp = new System.Drawing.Bitmap
	//	(
	//		desc.Width, 
	//		desc.Height, 
	//		System.Drawing.Imaging.PixelFormat.Format24bppRgb
	//	);

	//	var bmpData = bmp.LockBits
	//	(
	//		new( 0, 0, desc.Width, desc.Height ),
	//		System.Drawing.Imaging.ImageLockMode.WriteOnly,
	//		bmp.PixelFormat
	//	);

	//	var bmpPtr = bmpData.Scan0;

	//	for ( int y = 0; y < desc.Height; y++ )
	//	{
	//		for ( int x = 0; x < desc.Width; x++ )
	//		{
	//			Utilities.CopyMemory( bmpPtr, texturePtr, 3 );

	//			bmpPtr		= IntPtr.Add( bmpPtr	, 3 );
	//			texturePtr	= IntPtr.Add( texturePtr, 4 );
	//		}
	//	}
	//	bmp.UnlockBits( bmpData );

	//	_GraphicDevice.ImmediateContext.UnmapSubresource( copyTexture, 0 );

	//	return bmp;
 //   }

	///// <summary>
	///// 描画開始
	///// </summary>
	//public void DrawStart()
	//{
	//	_RenderTarget?.BeginDraw();

	//	_RenderTarget?.Clear( _BackGroundColor );
	//}

	///// <summary>
	///// 描画終了
	///// </summary>
	//public void DrawEnd()
	//{
	//	_RenderTarget?.EndDraw();

	//	// syncInterval=垂直同期設定
	//	_SwapChain?.Present( 1, PresentFlags.None );
	//}

 //   /// <summary>
 //   /// 背景色設定
 //   /// </summary>
 //   /// <param name="aColor">色</param>
 //   public void SetBackGroundColor( Color aColor )
 //   {
	//	_BackGroundColor = new( aColor.R / 255F, aColor.G / 255F, aColor.B / 255F, aColor.A / 255F );
	//}

	///// <summary>
	///// 線描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aBrush">色</param>
	//public void DrawLine( RectangleF aRect, Brush aBrush )
	//	=> _RenderTarget?.DrawLine( new( aRect.Left, aRect.Top ), new( aRect.Right, aRect.Bottom ), aBrush );

	///// <summary>
	///// 四角外枠描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aColor">色</param>
	//public void DrawRectangle( RectangleF aRect, float aWidth, Color aColor )
	//{ 
	//	if ( !_BrushDictionary.TryGetValue( aColor.ToArgb(), out var b ) )
 //       {
	//		return;
 //       }
	//	DrawRectangle( aRect, aWidth, b );
	//}

	///// <summary>
	///// 四角外枠描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aBrush">色</param>
	//public void DrawRectangle( RectangleF aRect, float aWidth, Brush aBrush )
	//	=> _RenderTarget?.DrawRectangle( new( aRect.Left, aRect.Top, aRect.Right, aRect.Bottom ), aBrush, aWidth );

	///// <summary>
	///// 四角塗りつぶし描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aColor">色</param>
	//public void FillRectangle( RectangleF aRect, Color aColor )
	//{ 
	//	if ( !_BrushDictionary.TryGetValue( aColor.ToArgb(), out var b ) )
 //       {
	//		return;
 //       }
	//	FillRectangle( aRect, b );
	//}

	///// <summary>
	///// 四角塗りつぶし描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aBrush">色</param>
	//public void FillRectangle( RectangleF aRect, Brush aBrush )
	//	=> _RenderTarget?.FillRectangle( new( aRect.Left, aRect.Top, aRect.Right, aRect.Bottom ), aBrush );

	///// <summary>
	///// 角丸四角塗りつぶし描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aColor">色</param>
	//public void FillRoundedRectangle( RectangleF aRect, Color aColor )
	//{ 
	//	if ( !_BrushDictionary.TryGetValue( aColor.ToArgb(), out var b ) )
 //       {
	//		return;
 //       }
	//	FillRoundedRectangle( aRect, b );
	//}

	///// <summary>
	///// 角丸四角塗りつぶし描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aBrush">色</param>
	//public void FillRoundedRectangle( RectangleF aRect, Brush aBrush )
	//	=> _RenderTarget?.FillRoundedRectangle
	//		( 
	//			new()
	//			{
	//				Rect	= new( aRect.Left, aRect.Top, aRect.Right, aRect.Bottom ),
	//				RadiusX = 5,
	//				RadiusY = 5,
	//			},
	//			aBrush 
	//		);

	///// <summary>
	///// 円外枠描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aColor">色</param>
	//public void DrawEllipse( RectangleF aRect, Color aColor )
	//{ 
	//	if ( !_BrushDictionary.TryGetValue( aColor.ToArgb(), out var b ) )
 //       {
	//		return;
 //       }
	//	DrawEllipse( aRect, b );
	//}

	///// <summary>
	///// 円外枠描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aBrush">色</param>
	//public void DrawEllipse( RectangleF aRect, Brush aBrush )
	//{
 //       var r = new Ellipse
 //           ( 
 //               new
	//			(
	//				aRect.Left + aRect.Width  / 2.0F,
	//				aRect.Top  + aRect.Height / 2.0F
	//			),
	//			aRect.Width  / 2.0F,
	//			aRect.Height / 2.0F
 //           );

 //       _RenderTarget?.DrawEllipse( r, aBrush );
	//}

	///// <summary>
	///// 円塗りつぶし描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aColor">色</param>
	//public void FillEllipse( RectangleF aRect, Color aColor )
	//{ 
	//	if ( !_BrushDictionary.TryGetValue( aColor.ToArgb(), out var b ) )
 //       {
	//		return;
 //       }
	//	FillEllipse( aRect, b );
	//}

	///// <summary>
	///// 円塗りつぶし描画
	///// </summary>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aBrush">色</param>
	//public void FillEllipse( RectangleF aRect, Brush aBrush )
	//{
 //       var r = new Ellipse
 //           ( 
 //               new
	//			(
	//				aRect.Left + aRect.Width  / 2.0F,
	//				aRect.Top  + aRect.Height / 2.0F
	//			),
	//			aRect.Width  / 2.0F,
	//			aRect.Height / 2.0F
 //           );

 //       _RenderTarget?.FillEllipse( r, aBrush );
	//}

	///// <summary>
	///// テキスト描画
	///// </summary>
	///// <param name="aText">表示テキスト</param>
	///// <param name="aTextFormatKey">テキストフォーマットキー</param>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aColor">テキスト色</param>
	//public void DrawText( string aText, int aTextFormatKey, RectangleF aRect, Color aColor )
	//{
	//	if ( !_TextFormatDictionary.TryGetValue( aTextFormatKey, out var t ) )
 //       {
	//		return;
 //       }
	//	if ( !_BrushDictionary.TryGetValue( aColor.ToArgb(), out var b ) )
 //       {
	//		return;
 //       }
	//	DrawText( aText, t, aRect, b );
	//}

	///// <summary>
	///// テキスト描画
	///// </summary>
	///// <param name="aText">表示テキスト</param>
	///// <param name="aTextFormat">テキストフォーマット</param>
	///// <param name="aRect">描画範囲</param>
	///// <param name="aBrush">テキスト色</param>
	//public void DrawText( string aText, TextFormat aTextFormat, RectangleF aRect, Brush aBrush )
	//	=> _RenderTarget?.DrawText( aText, aTextFormat, new( aRect.Left, aRect.Top, aRect.Right, aRect.Bottom ), aBrush );

 //   #endregion

 //   #region リソース作成

	///// <summary>
	///// 単色ブラシ作成
	///// </summary>
	///// <param name="aColor">色</param>
	///// <returns>ブラシ</returns>
 //   public Brush CreateSolidColorBrush( Color aColor ) 
	//	=> new SolidColorBrush( _RenderTarget, new( aColor.R / 255F, aColor.G / 255F, aColor.B / 255F, aColor.A / 255F ) );

	///// <summary>
	///// テキストフォーマット作成
	///// </summary>
	///// <param name="aFont">フォント</param>
	///// <param name="aStringFormat">文字列書式</param>
	///// <returns>テキストフォーマット</returns>
	//public TextFormat CreateTextFormat( Font aFont, StringFormat aStringFormat )
	//	=> new( _FactoryWrite, aFont.Name, aFont.Size )
	//			{
	//				TextAlignment		= GetTextAlignment( aStringFormat.Alignment ),
	//				ParagraphAlignment	= GetParagraphAlignment( aStringFormat.LineAlignment ),
	//			};

	///// <summary>
	///// テキスト横方向の配置をSharpDxのパラメータに変更
	///// </summary>
	///// <param name="aValue">テキスト書式</param>
	///// <returns>横方向の配置</returns>
	//public static TextAlignment GetTextAlignment( StringAlignment aValue )
	//{
 //       return aValue switch
 //       {
 //           StringAlignment.Center	=> TextAlignment.Center,
 //           StringAlignment.Near	=> TextAlignment.Leading,
 //           StringAlignment.Far		=> TextAlignment.Trailing,
 //           _						=> TextAlignment.Justified,
 //       };
 //   }

	///// <summary>
	///// テキスト縦方向の配置をSharpDxのパラメータに変更
	///// </summary>
	///// <param name="aValue">テキスト書式</param>
	///// <returns>縦方向の配置</returns>
	//public static ParagraphAlignment GetParagraphAlignment( StringAlignment aValue )
	//{
 //       return aValue switch
 //       {
 //           StringAlignment.Center	=> ParagraphAlignment.Center,
 //           StringAlignment.Near	=> ParagraphAlignment.Near,
 //           StringAlignment.Far		=> ParagraphAlignment.Far,
 //           _						=> ParagraphAlignment.Center,
 //       };
 //   }

 //   #endregion

 //   #region リソース管理（sharpDXのオブジェクトを外だしにしたくなかったのでこの作りになってます）

	///// <summary>
	///// リソースクリア
	///// </summary>
	//public void ClearResource()
 //   {
	//	_BrushDictionary.Clear();
	//	_TextFormatDictionary.Clear();
	//}

	///// <summary>
	///// ブラシリソース作成
	///// </summary>
	///// <param name="aColor">色</param>
	//public void AddBrush( Color aColor )
	//{
	//	var key = aColor.ToArgb();
	//	if ( !_BrushDictionary.ContainsKey( key ) )
 //       {
	//		_BrushDictionary.Add( key, CreateSolidColorBrush( aColor ) );
	//	}
	//}

	///// <summary>
	///// テキストフォーマットリソース作成
	///// </summary>
	///// <param name="aKey">設定キー</param>
	///// <param name="aTextFont">フォント名</param>
	///// <param name="aTextFormat">テキストフォーマット</param>
	//public void AddTextFormat( int aKey, Font aTextFont, StringFormat aTextFormat )
	//{
	//	if ( !_TextFormatDictionary.ContainsKey( aKey ) )
 //       {
	//		_TextFormatDictionary.Add( aKey, CreateTextFormat( aTextFont, aTextFormat ) );
	//	}
	//}

 //   #endregion
}
