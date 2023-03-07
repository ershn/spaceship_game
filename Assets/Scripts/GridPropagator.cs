using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPropagator : MonoBehaviour
{
    public GridPositioner GridPositioner;

    void Awake()
    {
        GridPositioner.Grid = GetComponent<Grid>();
    }

    void OnDestroy()
    {
        GridPositioner.Grid = null;
    }
}
