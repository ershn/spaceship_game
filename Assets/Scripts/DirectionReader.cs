using UnityEngine;
using UnityEngine.Events;

public class DirectionReader : MonoBehaviour
{
    public UnityEvent<Vector2> OnDirectionUpdated;

    void Update()
    {
        var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        OnDirectionUpdated.Invoke(direction);
    }
}
