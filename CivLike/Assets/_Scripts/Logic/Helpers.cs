using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
    {
        var i = 0;
        foreach (var e in ie) action(e, i++);
    }

    public static void DestroyAllChildren(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }     
    }

    public static void DestroyAll(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            DestroyAll(child.gameObject);
        }
        GameObject.Destroy(obj);
    }
}
