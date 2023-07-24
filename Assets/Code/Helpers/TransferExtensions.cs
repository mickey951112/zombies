using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
        List<Transform> sourceList,
        List<Transform> cloneList,
        Action<Transform, Transform> callbackPerChild
    )
    {
        for (var i = 0; i < sourceParent.childCount; i++)
        {
            var sourceTransform = sourceParent.GetChild(i);
            var targetChild = NewMethod(
                sourceTransform,
                targetParent,
                callbackPerChild,
                sourceList,
                cloneList
            );
            sourceTransform.CloneChildrenTo(targetChild, sourceList, cloneList, callbackPerChild);
        }
    }

    public static (List<Transform> source, List<Transform> clone) CloneChildrenTo(
        this Transform sourceTransform,
        bool includeSelf,
        Transform targetParent,
        Action<Transform, Transform> callbackPerChild
    )
    {
        var targetChild = targetParent;
        var sourceList = new List<Transform>();
        var cloneList = new List<Transform>();
        if (includeSelf)
        {
            targetChild = NewMethod(
                sourceTransform,
                targetParent,
                callbackPerChild,
                sourceList,
                cloneList
            );
        }
        sourceTransform.CloneChildrenTo(targetChild, sourceList, cloneList, callbackPerChild);

        Assert.AreEqual(sourceList.Count, cloneList.Count);
        return (sourceList, cloneList);
    }

    private static Transform NewMethod(
        Transform sourceTransform,
        Transform targetParent,
        Action<Transform, Transform> callbackPerChild,
        List<Transform> sourceList,
        List<Transform> cloneList
    )
    {
        sourceList.Add(sourceTransform);
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
        cloneList.Add(target);
        callbackPerChild(sourceTransform, target);
        return target;
    }
}
