using System;

[Serializable]
public class GridPosition
{
    public int x;
    public int y;
    
    public GridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        return "X:" + x + "; y:" + y;
    }

    public static bool operator == (GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator != (GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               y == position.y;
    }
    public static GridPosition operator + (GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.y + b.y);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.y - b.y);
    }
}

public enum GridPositionType
{
    Undefined, Free, Path, Obstacle, TemporaryObstacle, MilitaryBuilding, CivilianBuilding
}
