using System;

public static class FunctionalUtils
{
    public static Action<T> Do<T>(Action f, Action<T> g) =>
        x =>
        {
            f();
            g(x);
        };

    public static Action<T> Compose<T>(Action f, Action<T> g) =>
        x =>
        {
            g(x);
            f();
        };
}
