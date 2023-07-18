using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

public class ProceduralMesh
{
    Mesh.MeshDataArray meshDataArray;
    Mesh mesh;
    NativeArray<Vertex> vertexArray;
    NativeArray<UInt16> triagleVertices;
    int vertexIndex;
    int triagleIndex;
    float3 minVertex = float3.zero;
    float3 maxVertex = float3.zero;

    public ProceduralMesh(string name, int vertexCount, int triangleCount)
    {
        mesh = new Mesh { name = name };

        meshDataArray = Mesh.AllocateWritableMeshData(1);
        var meshData = meshDataArray[0];
        var attributeDescriptors = new NativeArray<VertexAttributeDescriptor>(
            length: 2,
            Allocator.Temp,
            NativeArrayOptions.UninitializedMemory
        );
        attributeDescriptors[0] = new VertexAttributeDescriptor(
            VertexAttribute.Position,
            VertexAttributeFormat.Float32,
            dimension: 3
        );
        attributeDescriptors[1] = new VertexAttributeDescriptor(
            VertexAttribute.TexCoord0,
            VertexAttributeFormat.Float16,
            dimension: 2
        );
        meshData.SetVertexBufferParams(vertexCount, attributeDescriptors);
        attributeDescriptors.Dispose();

        vertexArray = meshData.GetVertexData<Vertex>(0);

        meshData.SetIndexBufferParams(triangleCount * 3, IndexFormat.UInt16);
        triagleVertices = meshData.GetIndexData<UInt16>();

        meshData.subMeshCount = 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UInt16 AddVertex(Vertex vertex)
    {
        vertexArray[vertexIndex] = vertex;
        if (vertexIndex == 0)
        {
            minVertex = maxVertex = vertex.position;
        }
        else
        {
            minVertex.x = math.min(minVertex.x, vertex.position.x);
            minVertex.y = math.min(minVertex.y, vertex.position.y);
            minVertex.z = math.min(minVertex.z, vertex.position.z);
            maxVertex.x = math.max(maxVertex.x, vertex.position.x);
            maxVertex.y = math.max(maxVertex.y, vertex.position.y);
            maxVertex.z = math.max(maxVertex.z, vertex.position.z);
        }
        return (UInt16)vertexIndex++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddTriagle(UInt16 a, UInt16 b, UInt16 c)
    {
        triagleVertices[triagleIndex++] = a;
        triagleVertices[triagleIndex++] = b;
        triagleVertices[triagleIndex++] = c;
    }

    public void Draw(MeshFilter meshFilter)
    {
        Assert.IsTrue(vertexIndex == vertexArray.Length);
        Assert.IsTrue(triagleIndex == triagleVertices.Length);

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);

        var bounds = new Bounds(center: (minVertex + maxVertex) / 2, size: maxVertex - minVertex);

        mesh.SetSubMesh(
            index: 0,
            new SubMeshDescriptor(indexStart: 0, indexCount: triagleVertices.Length)
            {
                bounds = bounds,
                vertexCount = vertexArray.Length
            },
            MeshUpdateFlags.DontRecalculateBounds
        );
        mesh.bounds = bounds;

        mesh.Optimize();
        meshFilter.mesh = mesh;
    }
}
