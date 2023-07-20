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
            hitComponent.transform.root.position,
            hitComponent.transform.root.rotation,
            hitComponent.transform.lossyScale
        );
        cutRenderer.sharedMaterial = skinnedMeshRenderer.sharedMaterial;

        var childRoot = GameObjectHelpers.Create("Root", parent: cutParts.transform);

        // Add bone transforms
        var impactedBoneTransforms = hitComponent.transform.CloneChildrenTo(
            true,
            childRoot.transform,
            CloneComponents
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

    private void CloneComponents(Transform sourceTransform, Transform target)
    {
        var sourceBody = sourceTransform.GetComponent<Rigidbody>();
        if (sourceBody != null)
        {
            var childBody = target.gameObject.AddComponent<Rigidbody>();
            childBody.mass = sourceBody.mass;
            childBody.useGravity = false; // TODO
        }

        var sourceSphereCollider = sourceTransform.GetComponent<SphereCollider>();
        if (sourceSphereCollider != null)
        {
            var childCollider = target.gameObject.AddComponent<SphereCollider>();
            childCollider.center = sourceSphereCollider.center;
            childCollider.radius = sourceSphereCollider.radius;
            childCollider.isTrigger = true; // TODO disable
        }

        var sourceCapsuleCollider = sourceTransform.GetComponent<CapsuleCollider>();
        if (sourceCapsuleCollider != null)
        {
            var childCollider = target.gameObject.AddComponent<CapsuleCollider>();
            childCollider.center = sourceCapsuleCollider.center;
            childCollider.radius = sourceCapsuleCollider.radius;
            childCollider.height = sourceCapsuleCollider.height;
            childCollider.direction = sourceCapsuleCollider.direction;
            childCollider.isTrigger = true; // TODO disable
        }
    }
}
