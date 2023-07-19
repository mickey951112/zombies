using System.Collections.Generic;
using UnityEngine;

public static class TransferExtensions
{
    private static void AddChildrenToList(this Transform transform, List<Transform> list)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            list.Add(child);
            child.AddChildrenToList(list);
        }
    }

    public static List<Transform> CreateListOfChildren(this Transform transform, bool includeSelf)
    {
        var list = new List<Transform>();
        if (includeSelf)
        {
            list.Add(transform);
        }
        transform.AddChildrenToList(list);
        return list;
    }
}
