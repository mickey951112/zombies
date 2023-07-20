using System;
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

    private static void CloneChildrenTo(
        this Transform sourceParent,
        Transform targetParent,
        List<Transform> list,
        Action<Transform, Transform> callbackPerChild
    )
    {
        for (var i = 0; i < sourceParent.childCount; i++)
        {
            var sourceTransform = sourceParent.GetChild(i);
            var targetChild = NewMethod(sourceTransform, targetParent, callbackPerChild, list);
            sourceTransform.CloneChildrenTo(targetChild, list, callbackPerChild);
        }
    }

    public static List<Transform> CloneChildrenTo(
        this Transform sourceTransform,
        bool includeSelf,
        Transform targetParent,
        Action<Transform, Transform> callbackPerChild
    )
    {
        var targetChild = targetParent;
        var list = new List<Transform>();
        if (includeSelf)
        {
            targetChild = NewMethod(sourceTransform, targetParent, callbackPerChild, list);
        }
        sourceTransform.CloneChildrenTo(targetChild, list, callbackPerChild);
        return list;
    }

    private static Transform NewMethod(
        Transform sourceTransform,
        Transform targetParent,
        Action<Transform, Transform> callbackPerChild,
        List<Transform> list
    )
    {
        list.Add(sourceTransform);
        var target = GameObjectHelpers
            .Create(
                sourceTransform.name,
                sourceTransform.localPosition,
                sourceTransform.localRotation,
                sourceTransform.localScale,
                targetParent,
                useWorldSpace: false
            )
            .transform;
        callbackPerChild(sourceTransform, target);
        return target;
    }
}
