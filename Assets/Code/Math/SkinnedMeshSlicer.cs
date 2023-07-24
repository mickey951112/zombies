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
        Debug.Log(skinnedMeshRenderer.rootBone.name);
        var slideForwardBy = new Vector3(0, 0, 2);

        // Create cut parts
        var cutParts = GameObjectHelpers.Create(
            $"{gameObject.name}: Cut Parts",
            hitComponent.transform.root.position,
            hitComponent.transform.root.rotation,
            skinnedMeshRenderer.rootBone.parent.lossyScale
        );

        (_, var cutRenderer) = GameObjectHelpers.Create<SkinnedMeshRenderer>(
            "Renderer",
            rotation: hitComponent.transform.root
                .GetComponentInChildren<SkinnedMeshRenderer>()
                .transform.rotation,
            localScale: hitComponent.transform.lossyScale, // Not clear if this matters
            parent: cutParts.transform
        );
        cutRenderer.sharedMaterial = skinnedMeshRenderer.sharedMaterial;

        var childRoot = GameObjectHelpers.Create(
            "Root",
            skinnedMeshRenderer.rootBone.localPosition,
            skinnedMeshRenderer.rootBone.localRotation,
            skinnedMeshRenderer.rootBone.localScale,
            useWorldSpace: false,
            parent: cutParts.transform
        );
        // This causes the position of the rendered mesh to change and now be incorrect.
        cutRenderer.rootBone = childRoot.transform;

        // Add bone transforms
        (var impactedSourceBoneTransforms, var clonedBoneTransforms) =
            hitComponent.transform.CloneChildrenTo(true, childRoot.transform, CloneComponents);
        cutRenderer.bones = clonedBoneTransforms.ToArray();

        // Gather details
        var mesh = skinnedMeshRenderer.sharedMesh; // TODO remove temp?

        var hitBoneIndices = skinnedMeshRenderer.MapBonesToBoneIndexes(
            impactedSourceBoneTransforms
        );
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

        var boneWeights = new BoneWeight[impactedVertexIndices.Count];
        var iBoneWeight = 0;
        foreach (var index in impactedVertexIndices)
        {
            // TODO map the indexes to the procedural mesh indexes
            // boneWeights[iBoneWeight].boneIndex0

            boneWeights[iBoneWeight] = mesh.boneWeights[index];

            iBoneWeight++;
        }

        // This causes scale to not be respected
        cutRenderer.sharedMesh.boneWeights = boneWeights;

        // Desired once we handle the cut on the primary game object
        // for (var childIndex = hitComponent.transform.childCount - 1; childIndex >= 0; childIndex--)
        // {
        //     Destroy(hitComponent.transform.GetChild(childIndex).gameObject);
        // }

        // cutParts.transform.position += slideForwardBy;
    }

    private void CloneComponents(Transform sourceTransform, Transform target)
    {
        var sourceBody = sourceTransform.GetComponent<Rigidbody>();
        if (sourceBody != null)
        {
            var childBody = target.gameObject.AddComponent<Rigidbody>();
            childBody.mass = sourceBody.mass;
            childBody.useGravity = false;
        }

        var sourceSphereCollider = sourceTransform.GetComponent<SphereCollider>();
        if (sourceSphereCollider != null)
        {
            var childCollider = target.gameObject.AddComponent<SphereCollider>();
            childCollider.center = sourceSphereCollider.center;
            childCollider.radius = sourceSphereCollider.radius;
            childCollider.isTrigger = true;
        }

        var sourceCapsuleCollider = sourceTransform.GetComponent<CapsuleCollider>();
        if (sourceCapsuleCollider != null)
        {
            var childCollider = target.gameObject.AddComponent<CapsuleCollider>();
            childCollider.center = sourceCapsuleCollider.center;
            childCollider.radius = sourceCapsuleCollider.radius;
            childCollider.height = sourceCapsuleCollider.height;
            childCollider.direction = sourceCapsuleCollider.direction;
            childCollider.isTrigger = true;
        }
    }
}
