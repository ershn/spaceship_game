using UnityEngine;
using UnityEngine.EventSystems;

public class WorldInputReader : MonoBehaviour
{
    public Vector2Event OnClick;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnClick.Invoke(clickPosition);
        }
    }
}