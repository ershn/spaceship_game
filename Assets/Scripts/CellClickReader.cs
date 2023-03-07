using UnityEngine;

[RequireComponent(typeof(GridLayout))]
public class CellClickReader : MonoBehaviour
{
    public Vector2IntGameEvent OnClick;

    GridLayout _gridLayout;

    void Start()
    {
        _gridLayout = GetComponent<GridLayout>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var cellPosition = (Vector2Int)_gridLayout.WorldToCell(mousePosition);
            OnClick.Invoke(cellPosition);
        }
    }
}