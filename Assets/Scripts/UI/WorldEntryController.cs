using System;
using TMPro;
using UnityEngine;

public class WorldEntryController : MonoBehaviour
{
    public event Action<World> OnWorldSelected;

    TextMeshProUGUI _textControl;

    World _world;

    void Awake()
    {
        _textControl = GetComponent<TextMeshProUGUI>();
    }

    public void SetWorld(World world)
    {
        _world = world;

        gameObject.name = world.name;
        _textControl.text = world.name;
    }

    public void SelectWorld()
    {
        OnWorldSelected(_world);
    }
}
