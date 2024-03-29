using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIInputReader : MonoBehaviour
{
    public UnityEvent<Vector2> OnClick;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnClick.Invoke(clickPosition);
        }
    }
}
