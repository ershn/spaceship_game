using UnityEngine;

public class ArrayGrid<T>
{
    T[,] _array;
    Vector2Int _centerPosition;

    public ArrayGrid(uint sideSize)
    {
        _array = new T[sideSize, sideSize];
        _centerPosition = new((int)sideSize / 2, (int)sideSize / 2);
    }

    public T this[Vector2Int position]
    {
        get
        {
            position += _centerPosition;
            return _array[position.x, position.y];
        }
        set
        {
            position += _centerPosition;
            _array[position.x, position.y] = value;
        }
    }
}