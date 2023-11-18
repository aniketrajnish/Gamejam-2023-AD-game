using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If you come up with any helpful little tool put it here.
/// </summary>
public static class CommonTools
{
    //Fisher-Yates shuffle
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
