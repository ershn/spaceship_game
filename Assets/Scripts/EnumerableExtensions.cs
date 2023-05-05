using System;
using System.Collections.Generic;

public static class EnumerableExtensions
{
    public static IEnumerable<TResult> SelectWhile<TSource, TResult>(
        this IEnumerable<TSource> items,
        Func<TSource, (bool, TResult)> selector
        )
    {
        foreach (var item in items)
        {
            var (goNext, value) = selector(item);
            yield return value;
            if (!goNext)
                break;
        }
    }
}