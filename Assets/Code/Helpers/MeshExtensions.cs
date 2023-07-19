using System.Collections.Generic;
using UnityEngine;

public static class MeshExtensions
{
    public static List<int> GetImpactedVertexIndices(this Mesh mesh, List<int> hitBoneIndexes)
    {
        var impactedVertexIndices = new List<int>();
        for (var index = 0; index < mesh.vertices.Length; index++)
        {
            var vert = mesh.vertices[index];
            var boneWeight = mesh.boneWeights[index];
            if (hitBoneIndexes.Contains(boneWeight.boneIndex0))
            {
                impactedVertexIndices.Add(index);
            }
            // Head does not weight other than on index 0, when would these apply.
            // else if (boneWeight.boneIndex1 == hitBoneIndex)
            // {
            //     count++;
            // }
            // else if (boneWeight.boneIndex2 == hitBoneIndex)
            // {
            //     count++;
            // }
            // else if (boneWeight.boneIndex3 == hitBoneIndex)
            // {
            //     count++;
            // }
        }

        return impactedVertexIndices;
    }

    public static List<(int, int, int)> GetImpactedTriangles(
        this Mesh mesh,
        List<int> impactedVertexIndices
    )
    {
        var trigs = mesh.triangles;
        var impactedTrigs = new List<(int, int, int)>();
        for (var index = 0; index < trigs.Length; index += 3)
        {
            var trig1 = trigs[index];
            var trig2 = trigs[index + 1];
            var trig3 = trigs[index + 2];
            if (
                impactedVertexIndices.Contains(trig1)
                && impactedVertexIndices.Contains(trig2)
                && impactedVertexIndices.Contains(trig3)
            )
            {
                impactedTrigs.Add((trig1, trig2, trig3));
            }
        }

        return impactedTrigs;
    }
}
