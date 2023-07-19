using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TODO
 * consider skinned mesh so that we can ragdoll the cut parts
*/
public class SkinnedMeshSlicer : MonoBehaviour
{
    public void Slice(GameObject hitComponent, Vector3 hitPosition)
    {
        var parentGameObject = hitComponent.transform.root.gameObject;
        var impactedComponents = new List<Transform>();
        impactedComponents.Add(hitComponent.transform);
        AddChildren(impactedComponents, hitComponent.transform);
        Debug.Log($"impactedComponents.Count: {impactedComponents.Count}");

        var cutParts = new GameObject($"{gameObject.name}: Cut Parts");
        cutParts.transform.SetPositionAndRotation(
            hitComponent.transform.position,
            hitComponent.transform.rotation
        );
        cutParts.transform.localScale = hitComponent.transform.lossyScale;
        var cutRenderer = cutParts.AddComponent<MeshRenderer>();
        var meshFilter = cutParts.AddComponent<MeshFilter>();

        var renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        cutRenderer.sharedMaterial = parentGameObject
            .GetComponentInChildren<SkinnedMeshRenderer>()
            .sharedMaterial;
        var mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        var trigs = mesh.triangles; // 4812
        var verts = mesh.vertices; // 1029

        Transform hitBone = null;
        var hitBoneIndexes = new List<int>();
        for (var index = 0; index < renderer.bones.Length; index++)
        {
            hitBone = renderer.bones[index];
            if (impactedComponents.Contains(hitBone))
            {
                hitBoneIndexes.Add(index);
            }
        }
        Debug.Log($"found bone {hitBone.name}");
        Debug.Log(renderer.rootBone.position);

        var impactedVertexIndex = new List<int>();
        for (var index = 0; index < verts.Length; index++)
        {
            var vert = verts[index];
            var boneWeight = mesh.boneWeights[index];
            if (hitBoneIndexes.Contains(boneWeight.boneIndex0))
            {
                impactedVertexIndex.Add(index);
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

        var impactedTrigs = new List<(int, int, int)>();
        for (var index = 0; index < trigs.Length; index += 3)
        {
            var trig1 = trigs[index];
            var trig2 = trigs[index + 1];
            var trig3 = trigs[index + 2];
            if (
                impactedVertexIndex.Contains(trig1)
                && impactedVertexIndex.Contains(trig2)
                && impactedVertexIndex.Contains(trig3)
            )
            {
                impactedTrigs.Add((trig1, trig2, trig3));
            }
        }
        Debug.Log(
            $"found {impactedVertexIndex.Count} verts in bone {hitBone.name} with {impactedTrigs.Count} trigs"
        );
        var proceduralMesh = new ProceduralMesh(
            "Cut Parts",
            vertexCount: impactedVertexIndex.Count,
            triangleCount: impactedTrigs.Count
        );
        var mappedVertexIndex = new Dictionary<int, UInt16>();
        foreach (var vertex in impactedVertexIndex)
        {
            var index = proceduralMesh.AddVertex(
                (verts[vertex], mesh.uv[vertex], mesh.normals[vertex])
            );
            mappedVertexIndex.Add(vertex, index);
        }
        foreach (var trig in impactedTrigs)
        {
            proceduralMesh.AddTriagle(
                mappedVertexIndex[trig.Item1],
                mappedVertexIndex[trig.Item2],
                mappedVertexIndex[trig.Item3]
            );
        }
        proceduralMesh.Draw(meshFilter);

        // for (var childIndex = hitComponent.transform.childCount - 1; childIndex >= 0; childIndex--)
        // {
        //     Destroy(hitComponent.transform.GetChild(childIndex).gameObject);
        // }
    }

    private static void AddChildren(List<Transform> childrenOfHit, Transform transform)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            childrenOfHit.Add(child);
            AddChildren(childrenOfHit, child);
        }
    }
}
