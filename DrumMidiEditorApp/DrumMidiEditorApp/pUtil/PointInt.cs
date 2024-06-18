using System;

namespace DrumMidiEditorApp.pUtil;

public struct PointInt
{
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;

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

    public static bool operator ==( PointInt a, PointInt b )
    {
        return a.X == b.X && a.Y == b.Y;
    }
    public static bool operator !=( PointInt a, PointInt b )
    {
        return !(a == b);
    }

    public readonly override bool Equals( object? obj ) => throw new NotImplementedException();

    public readonly override int GetHashCode() => throw new NotImplementedException();
}
