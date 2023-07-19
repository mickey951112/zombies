using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TODO
 * consider skinned mesh so that we can ragdoll the cut parts
   * requires we add bone transforms and weights to the cut parts
   * copy joints in order to ragdoll the cut parts


   rigidbody and collider per impacted bone
   
   x bone transforms
   bone component (joint, rigidbody, collider)
    bone weights
*/
public class SkinnedMeshSlicer : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;

    void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void Slice(GameObject hitComponent, Vector3 hitPosition)
    {
        // Create cut parts
        (var cutParts, var cutRenderer) = GameObjectHelpers.Create<SkinnedMeshRenderer>(
            $"{gameObject.name}: Cut Parts",
            hitComponent.transform.position,
            hitComponent.transform.rotation,
            hitComponent.transform.lossyScale
        );
        cutRenderer.sharedMaterial = skinnedMeshRenderer.sharedMaterial;

        // Add bone transforms
        var impactedBoneTransforms = hitComponent.transform.CloneChildrenTo(
            true,
            cutParts.transform
        );

        // Gather details
        var mesh = skinnedMeshRenderer.sharedMesh; // TODO remove temp?

        var hitBoneIndices = skinnedMeshRenderer.MapBonesToBoneIndexes(impactedBoneTransforms);
        var impactedVertexIndices = mesh.GetImpactedVertexIndices(hitBoneIndices);
        var impactedTrigs = mesh.GetImpactedTriangles(impactedVertexIndices);

        // Generate the mesh
        var proceduralMesh = new ProceduralMesh(
            vertexCount: impactedVertexIndices.Count,
            triangleCount: impactedTrigs.Count
        );
        var originalVertexIndexToProceduralMeshIndex = new Dictionary<int, UInt16>();
        foreach (var vertex in impactedVertexIndices)
        {
            var index = proceduralMesh.AddVertex(
                (mesh.vertices[vertex], mesh.uv[vertex], mesh.normals[vertex])
            );
            originalVertexIndexToProceduralMeshIndex.Add(vertex, index);
        }
        foreach (var trig in impactedTrigs)
        {
            proceduralMesh.AddTriagle(
                originalVertexIndexToProceduralMeshIndex[trig.Item1],
                originalVertexIndexToProceduralMeshIndex[trig.Item2],
                originalVertexIndexToProceduralMeshIndex[trig.Item3]
            );
        }

        cutRenderer.sharedMesh = proceduralMesh.Draw();

        // Desired once we handle the cut on the primary game object
        // for (var childIndex = hitComponent.transform.childCount - 1; childIndex >= 0; childIndex--)
        // {
        //     Destroy(hitComponent.transform.GetChild(childIndex).gameObject);
        // }
    }
}
