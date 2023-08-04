using UnityEngine;

public static class GridSpiral
{
    public static Vector2Int IndexToCell(int n)
    {
        n += 1;

        var k = Mathf.CeilToInt((Mathf.Sqrt(n) - 1f) / 2f);
        var t = 2 * k;
        var m = Mathf.RoundToInt(Mathf.Pow(t + 1, 2f));

        if (n >= m - t)
            return new(k - (m - n), -k);

        m -= t;
        if (n >= m - t)
            return new(-k, -k + (m - n));

        m -= t;
        if (n >= m - t)
            return new(-k + (m - n), k);
        else
            return new(k, k - (m - n - t));
    }
}
