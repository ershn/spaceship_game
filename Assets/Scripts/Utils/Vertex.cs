using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

public class Vertex<TEdgeId, TValue>
    where TEdgeId : Enum, IConvertible
{
    static readonly uint s_edgeCount;

    static Vertex()
    {
        s_edgeCount = (uint)Enum.GetNames(typeof(TEdgeId)).Length;
    }

    static uint ToEdgeIndex(TEdgeId id) => id.ToUInt32(null);

    public readonly TValue Value;

    uint _leadingEdgeCount;

    readonly Vertex<TEdgeId, TValue>[] _edges;

    public Vertex(TValue value)
    {
        Value = value;
        _edges = new Vertex<TEdgeId, TValue>[s_edgeCount];
    }

    public Vertex(TValue value, out Vertex<TEdgeId, TValue> vertex)
        : this(value)
    {
        vertex = this;
    }

    public bool IsLinkedTo() => _leadingEdgeCount > 0;

    public Vertex<TEdgeId, TValue> this[uint index]
    {
        get { return _edges[index]; }
        set
        {
            var currentEdge = _edges[index];
            if (currentEdge != null)
                currentEdge._leadingEdgeCount--;

            _edges[index] = value;
            if (value != null)
                value._leadingEdgeCount++;
        }
    }

    public Vertex<TEdgeId, TValue> this[TEdgeId id]
    {
        get { return this[ToEdgeIndex(id)]; }
        set { this[ToEdgeIndex(id)] = value; }
    }

    Vertex<TEdgeId, TValue> Link(Vertex<TEdgeId, TValue>[] edges)
    {
        Assert.IsTrue(edges.Length > 0);
        Assert.IsTrue(edges.Length <= s_edgeCount);

        for (uint index = 0; index < edges.Length; index++)
            this[index] = edges[index];
        return edges[0];
    }

    public Vertex<TEdgeId, TValue> Link(Vertex<TEdgeId, TValue> edge1) => Link(new[] { edge1 });

    public Vertex<TEdgeId, TValue> Link(
        Vertex<TEdgeId, TValue> edge1,
        Vertex<TEdgeId, TValue> edge2
    ) => Link(new[] { edge1, edge2 });

    public Vertex<TEdgeId, TValue> Link(
        Vertex<TEdgeId, TValue> edge1,
        Vertex<TEdgeId, TValue> edge2,
        Vertex<TEdgeId, TValue> edge3
    ) => Link(new[] { edge1, edge2, edge3 });

    public Vertex<TEdgeId, TValue> Link(
        Vertex<TEdgeId, TValue> edge1,
        Vertex<TEdgeId, TValue> edge2,
        Vertex<TEdgeId, TValue> edge3,
        Vertex<TEdgeId, TValue> edge4
    ) => Link(new[] { edge1, edge2, edge3, edge4 });

    public Vertex<TEdgeId, TValue> Link(
        Vertex<TEdgeId, TValue> edge1,
        Vertex<TEdgeId, TValue> edge2,
        Vertex<TEdgeId, TValue> edge3,
        Vertex<TEdgeId, TValue> edge4,
        params Vertex<TEdgeId, TValue>[] edges
    ) => Link(new[] { edge1, edge2, edge3, edge4 }.Concat(edges).ToArray());

    public Vertex<TEdgeId, TValue> Unlink(uint index)
    {
        var edge = this[index];
        this[index] = null;
        return edge;
    }

    public Vertex<TEdgeId, TValue> Unlink(TEdgeId id) => Unlink(ToEdgeIndex(id));

    public void Unlink()
    {
        for (uint index = 0; index < _edges.Length; index++)
            Unlink(index);
    }

    public void DeepUnlink(uint index, Action<Vertex<TEdgeId, TValue>> onOrphanedVertex = null)
    {
        var edge = Unlink(index);
        if (edge == null || edge.IsLinkedTo())
            return;

        onOrphanedVertex?.Invoke(edge);
        edge.DeepUnlink(onOrphanedVertex);
    }

    public void DeepUnlink(TEdgeId id, Action<Vertex<TEdgeId, TValue>> onOrphanedVertex = null) =>
        DeepUnlink(ToEdgeIndex(id), onOrphanedVertex);

    public void DeepUnlink(Action<Vertex<TEdgeId, TValue>> onOrphanedVertex = null)
    {
        for (uint index = 0; index < _edges.Length; index++)
            DeepUnlink(index, onOrphanedVertex);
    }

    public HashSet<Vertex<TEdgeId, TValue>> AllLinkedVertices()
    {
        var vertices = new HashSet<Vertex<TEdgeId, TValue>>();
        AddAllLinkedVertices(vertices);
        return vertices;
    }

    void AddAllLinkedVertices(HashSet<Vertex<TEdgeId, TValue>> accumulator)
    {
        accumulator.Add(this);
        foreach (var edge in _edges)
            edge?.AddAllLinkedVertices(accumulator);
    }
}
