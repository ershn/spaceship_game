using UnityEngine;

public class DirectionReader : MonoBehaviour
{
    public Vector2Event OnDirectionUpdated;

    void Update()
    {
        var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        OnDirectionUpdated.Invoke(direction);
    }
}
