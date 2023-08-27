using UnityEngine;

public class FloorConstructor : MonoBehaviour
{
    void Start()
    {
        GetComponent<StructureConstructor>().Construct();
    }
}
