public class MapGridPathData
{
    public MapGrid parent;
    public int F = 0;
    public int G = 0;
    public int H = 0;

    public void CalF()
    {
        F = G + H;
    }

    public void Clear()
    {
        F = 0;
        G = 0;
        H = 0;
        parent = null;
    }
}