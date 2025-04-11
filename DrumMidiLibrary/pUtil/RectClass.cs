using Windows.Foundation;

namespace DrumMidiLibrary.pUtil;

public record class RectClass
{
    private Rect _Rect = new();

    #region コンストラクタ

    public RectClass( double aX, double aY, double aWidth, double aHeight )
    {
        _Rect.X         = aX;
        _Rect.Y         = aY;
        _Rect.Width     = aWidth;
        _Rect.Height    = aHeight;
    }

    public RectClass( double aXY, double aWidthHeight )
    {
        _Rect.X         = aXY;
        _Rect.Y         = aXY;
        _Rect.Width     = aWidthHeight;
        _Rect.Height    = aWidthHeight;
    }

    public RectClass( float aX, float aY, float aWidth, float aHeight )
    {
        _Rect._x        = aX;
        _Rect._y        = aY;
        _Rect._width    = aWidth;
        _Rect._height   = aHeight;
    }

    public RectClass( float aXY, float aWidthHeight )
    {
        _Rect._x        = aXY;
        _Rect._y        = aXY;
        _Rect._width    = aWidthHeight;
        _Rect._height   = aWidthHeight;
    }

    #endregion

    #region プロパティ

    public Rect GetRect() => _Rect;

    public double X
    {
        get => _Rect.X;
        set => _Rect.X = value;
    }

    public double Y
    {
        get => _Rect.Y;
        set => _Rect.Y = value;
    }

    public double Width
    {
        get => _Rect.Width;
        set => _Rect.Width = value;
    }

    public double Height
    {
        get => _Rect.Height;
        set => _Rect.Height = value;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE1006:命名スタイル", Justification = "<保留中>" )]
    public float _x 
    { 
        get => _Rect._x;
        set => _Rect._x = value;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE1006:命名スタイル", Justification = "<保留中>" )]
    public float _y
    {
        get => _Rect._y;
        set => _Rect._y = value;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE1006:命名スタイル", Justification = "<保留中>" )]
    public float _width
    {
        get => _Rect._width;
        set => _Rect._width = value;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE1006:命名スタイル", Justification = "<保留中>" )]
    public float _height
    {
        get => _Rect._height;
        set => _Rect._height = value;
    }

    #endregion

    /// <summary>
    /// 指定したサイズ分 上下左右の範囲をカット
    /// </summary>
    /// <param name="aTrimmingSize"></param>
    public void Trimming( double aTrimmingSize )
    {
        var size = aTrimmingSize * 2;

        _Rect.X         += aTrimmingSize;
        _Rect.Y         += aTrimmingSize;
        _Rect.Width     -= ( _Rect.Width  - size >= 0 ) ? size : _Rect.Width ;
        _Rect.Height    -= ( _Rect.Height - size >= 0 ) ? size : _Rect.Height;
    }
}
