using UnityEngine;

public class WorldCamera : MonoBehaviour
{
    static readonly Vector3 s_positionRelativeToWorld = new(0, 0, -.9f);

    public void SelectWorld(World world)
    {
        transform.position = world.transform.position + s_positionRelativeToWorld;
    }
}
