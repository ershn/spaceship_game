using UnityEngine;

public class ItemLifecycle : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}