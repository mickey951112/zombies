using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.Rendering;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using System;
using System.Runtime.InteropServices;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawLine : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)] // don't jit struct
    struct Vertex
    {
        public float3 position;
        public half2 uv;
    }

    void OnEnable()
    {
        var mesh = new Mesh { name = "Line" };

        var meshDataArray = Mesh.AllocateWritableMeshData(1);
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
        meshData.SetVertexBufferParams(6, attributeDescriptors);
        attributeDescriptors.Dispose();

        var vertexArray = meshData.GetVertexData<Vertex>(0);
        vertexArray[0] = new Vertex
        {
            position = new float3(0, 0, 0),
            uv = new half2(half(0), half(0))
        };
        vertexArray[1] = new Vertex
        {
            position = new float3(1, 0, 0),
            uv = new half2(half(1), half(0))
        };
        vertexArray[2] = new Vertex
        {
            position = new float3(0, 1, 0),
            uv = new half2(half(0), half(1))
        };
        vertexArray[3] = new Vertex
        {
            position = new float3(1, 1, 0),
            uv = new half2(half(1), half(1))
        };
        vertexArray[4] = new Vertex
        {
            position = new float3(1, 0, -1),
            uv = new half2(half(0), half(0))
        };
        vertexArray[5] = new Vertex
        {
            position = new float3(1, 1, -1),
            uv = new half2(half(1), half(0))
        };

        meshData.SetIndexBufferParams(12, IndexFormat.UInt16);
        var trigVertices = meshData.GetIndexData<UInt16>();
        trigVertices[0] = 0;
        trigVertices[1] = 1;
        trigVertices[2] = 2;
        trigVertices[3] = 3;
        trigVertices[4] = 2;
        trigVertices[5] = 1;
        trigVertices[6] = 1;
        trigVertices[7] = 4;
        trigVertices[8] = 3;
        trigVertices[9] = 3;
        trigVertices[10] = 4;
        trigVertices[11] = 5;

        meshData.subMeshCount = 1;

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);

        var bounds = new Bounds(new Vector3(0.5f, 0.5f, -0.5f), Vector3.one);
        mesh.SetSubMesh(
            index: 0,
            new SubMeshDescriptor(indexStart: 0, indexCount: trigVertices.Length)
            {
                bounds = bounds,
                vertexCount = vertexArray.Length
            },
            MeshUpdateFlags.DontRecalculateBounds
        );
        mesh.bounds = bounds;

        mesh.Optimize();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void OnEnable2()
    {
        var mesh = new Mesh { name = "Line" };

        var meshDataArray = Mesh.AllocateWritableMeshData(1);
        var meshData = meshDataArray[0];

        var attributeDescriptors = new NativeArray<VertexAttributeDescriptor>(
            length: 2,
            Allocator.Temp,
            NativeArrayOptions.UninitializedMemory
        );
        attributeDescriptors[0] = new VertexAttributeDescriptor(
            VertexAttribute.Position,
            VertexAttributeFormat.Float32,
            dimension: 3,
            stream: 0
        );
        attributeDescriptors[1] = new VertexAttributeDescriptor(
            VertexAttribute.TexCoord0,
            VertexAttributeFormat.Float16,
            dimension: 2,
            stream: 1
        );
        meshData.SetVertexBufferParams(6, attributeDescriptors);
        attributeDescriptors.Dispose();

        var positions = meshData.GetVertexData<float3>(0);
        positions[0] = new float3(0, 0, 0);
        positions[1] = new float3(1, 0, 0);
        positions[2] = new float3(0, 1, 0);
        positions[3] = new float3(1, 1, 0);
        positions[4] = new float3(1, 0, -1);
        positions[5] = new float3(1, 1, -1);

        var uvs = meshData.GetVertexData<half2>(1);
        uvs[0] = new half2(half(0), half(0));
        uvs[1] = new half2(half(1), half(0));
        uvs[2] = new half2(half(0), half(1));
        uvs[3] = new half2(half(1), half(1));
        uvs[4] = new half2(half(0), half(0));
        uvs[5] = new half2(half(1), half(0));

        meshData.SetIndexBufferParams(12, IndexFormat.UInt16);
        var vertices = meshData.GetIndexData<UInt16>();
        vertices[0] = 0;
        vertices[1] = 1;
        vertices[2] = 2;
        vertices[3] = 3;
        vertices[4] = 2;
        vertices[5] = 1;
        vertices[6] = 1;
        vertices[7] = 4;
        vertices[8] = 3;
        vertices[9] = 3;
        vertices[10] = 4;
        vertices[11] = 5;

        meshData.subMeshCount = 1;

        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);

        var bounds = new Bounds(new Vector3(0.5f, 0.5f, -0.5f), Vector3.one);
        mesh.SetSubMesh(
            index: 0,
            new SubMeshDescriptor(indexStart: 0, indexCount: vertices.Length)
            {
                bounds = bounds,
                vertexCount = positions.Length
            },
            MeshUpdateFlags.DontRecalculateBounds
        );
        mesh.bounds = bounds;

        mesh.Optimize();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void OldOnEnable()
    {
        var mesh = new Mesh { name = "Line" };

        mesh.vertices = new Vector3[]
        {
            // Front face (z pos)
            Vector3.zero,
            Vector3.right,
            Vector3.up,
            new Vector3(1, 1, 0),
            // Right face
            new Vector3(1, 0, -1),
            new Vector3(1, 1, -1)
        };

        // Defining normals appear to be optional
        // mesh.normals = new Vector3[]
        // {
        //     Vector3.forward,
        //     Vector3.forward,
        //     Vector3.forward,
        //     Vector3.forward
        // };

        mesh.uv = new Vector2[]
        {
            // Front face
            Vector2.zero,
            Vector2.right,
            Vector2.up,
            Vector2.one,
            // Right face
            Vector2.zero,
            Vector2.right
        };

        // Clockwise is a positive normal
        mesh.triangles = new int[]
        {
            // Front face bottom trig
            0,
            1,
            2,
            // Front top
            3,
            2,
            1,
            // Right bottom
            1,
            4,
            3,
            // Right top
            3,
            4,
            5,
        };

        // Tangents impact the normal map
        // Not seeing the impact here yet
        // mesh.tangents = new Vector4[]
        // {
        //     new Vector4(1, 0, 0, -1),
        //     new Vector4(1, 0, 0, -1),
        //     new Vector4(1, 0, 0, -1),
        //     new Vector4(1, 0, 0, -1),
        //     new Vector4(1, 0, 0, -1),
        //     new Vector4(1, 0, 0, -1),
        // };

        mesh.Optimize();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
