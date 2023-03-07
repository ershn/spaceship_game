using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLifecycle : MonoBehaviour
{
    public void Destroy()
    {
        Debug.Log($"Destroy item");
        Destroy(gameObject);
    }
}