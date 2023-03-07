using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionReader : MonoBehaviour
{
    public Vector2Event OnDirectionUpdated;

    // Update is called once per frame
    void Update()
    {
        var direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        OnDirectionUpdated.Invoke(direction);
    }
}
