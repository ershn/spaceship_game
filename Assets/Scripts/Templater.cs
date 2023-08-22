using UnityEngine;

public static class Templater
{
    public static void Template<T>(GameObject gameObject, T def)
    {
        foreach (var template in gameObject.GetComponentsInChildren<ITemplate<T>>())
            template.Template(def);
    }
}
