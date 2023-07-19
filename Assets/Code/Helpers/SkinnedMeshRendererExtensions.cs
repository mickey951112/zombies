using System.Collections.Generic;
using UnityEngine;

public static class SkinnedMeshRendererExtensions
{
    public static List<int> MapBonesToBoneIndexes(
        this SkinnedMeshRenderer skinnedMeshRenderer,
        List<Transform> bones
    )
    {
        var hitBoneIndexes = new List<int>();
        for (var index = 0; index < skinnedMeshRenderer.bones.Length; index++)
        {
            if (bones.Contains(skinnedMeshRenderer.bones[index]))
            {
                hitBoneIndexes.Add(index);
            }
        }
        return hitBoneIndexes;
    }
}
