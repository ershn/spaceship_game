using UnityEngine;

public interface IComponent
{
    GameObject gameObject { get; }
    Transform transform { get; }
}
