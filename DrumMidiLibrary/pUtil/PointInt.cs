namespace DrumMidiLibrary.pUtil;

public record struct PointInt
{
    public PointInt( int aXY )
    {
        X = aXY;
        Y = aXY;
    }

    public PointInt( int aX, int aY )
    {
        X = aX;
        Y = aY;
    }

    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;
}
