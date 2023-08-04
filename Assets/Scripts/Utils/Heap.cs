using System;
using System.Collections.Generic;

public class Heap<T>
    where T : IHeapItem<T>
{
    List<T> _list;

    public Heap()
    {
        _list = new();
    }

    public Heap(int capacity)
    {
        _list = new(capacity);
    }

    public int Count => _list.Count;

    public bool Contains(T element) =>
        element.HeapIndex >= 0
        && element.HeapIndex < _list.Count
        && _list[element.HeapIndex].Equals(element);

    public void Add(T element)
    {
        element.HeapIndex = _list.Count;
        _list.Add(element);
        SortUp(element.HeapIndex);
    }

    public T RemoveFirst()
    {
        if (_list.Count == 0)
            throw new InvalidOperationException("Heap is empty");

        var element = _list[0];
        Move(_list.Count - 1, 0);
        SortDown(0);
        return element;
    }

    public void Update(T element)
    {
        if (!Contains(element))
            throw new ArgumentException("Element not in the heap");

        if (!SortUp(element.HeapIndex))
            SortDown(element.HeapIndex);
    }

    bool SortUp(int index)
    {
        var swapped = false;
        while (index > 0)
        {
            var parentIndex = (index - 1) / 2;
            if (_list[index].CompareTo(_list[parentIndex]) >= 0)
                break;
            Swap(index, parentIndex);
            index = parentIndex;
            swapped = true;
        }
        return swapped;
    }

    bool SortDown(int index)
    {
        var swapped = false;
        while (true)
        {
            var leftChildIndex = index * 2 + 1;
            var rightChildIndex = index * 2 + 2;
            int swapIndex;

            if (leftChildIndex < _list.Count)
            {
                swapIndex = leftChildIndex;
                if (rightChildIndex < _list.Count)
                {
                    if (_list[leftChildIndex].CompareTo(_list[rightChildIndex]) > 0)
                        swapIndex = rightChildIndex;
                }

                if (_list[index].CompareTo(_list[swapIndex]) > 0)
                {
                    Swap(index, swapIndex);
                    index = swapIndex;
                    swapped = true;
                    continue;
                }
            }
            break;
        }
        return swapped;
    }

    void Swap(int indexA, int indexB)
    {
        var element = _list[indexA];
        _list[indexA] = _list[indexB];
        _list[indexA].HeapIndex = indexA;
        _list[indexB] = element;
        _list[indexB].HeapIndex = indexB;
    }

    void Move(int indexFrom, int indexTo)
    {
        _list[indexTo] = _list[indexFrom];
        _list[indexTo].HeapIndex = indexTo;
        _list.RemoveAt(indexFrom);
    }
}
