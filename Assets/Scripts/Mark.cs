using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mark
{
    public int x;
    public int y;

    public Mark(int nx, int ny)
    {
        x = nx;
        y = ny;
    }


    public void mult(int m)
    {
        x *= m;
        y *= m;
    }
    public void add(Mark p)
    {
        x += p.x;
        y += p.y;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }
    public bool Equals(Mark p)
    {
        return (x == p.x && y == p.y);
    }
    public static Mark fromVector(Vector2 v)
    {
        return new Mark((int)v.x, (int)v.y);
    }

    public static Mark fromVector(Vector3 v)
    {
        return new Mark((int)v.x, (int)v.y);
    }

    public static Mark mult(Mark p, int m)
    {
        return new Mark(p.x * m, p.y * m);
    }
    public static Mark add(Mark p, Mark o)
    {
        return new Mark(p.x * o.x, p.y * o.y);
    }
    public static Mark clone(Mark p)
    {
        return new Mark(p.x, p.y);
    }
    public static Mark zero
    {
        get{ return new Mark(0,0); }
    }
    public static Mark one
    {
        get { return new Mark(1, 1); }
    }
    public static Mark up
    {
        get { return new Mark(0, 1); }
    }
    public static Mark down
    {
        get { return new Mark(0, -1); }
    }
    public static Mark right
    {
        get { return new Mark(1, 0); }
    }
    public static Mark left
    {
        get { return new Mark(-1, 0); }
    }
}
