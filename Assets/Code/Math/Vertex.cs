using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using static Unity.Mathematics.math;

[StructLayout(LayoutKind.Sequential)] // don't jitter struct
public struct Vertex
{
    public float3 position;
    public float3 normal;
    public half2 uv;

    public Vertex(float3 position, half2 uv, float3 normal)
    {
        this.position = position;
        this.normal = normal;
        this.uv = uv;
    }

    public static implicit operator Vertex(
        (
            (float x, float y, float z) position,
            (float x, float y) uv,
            (float x, float y, float z) normal
        ) data
    )
    {
        return new Vertex(
            new float3(data.position.x, data.position.y, data.position.z),
            new half2(half(data.uv.x), half(data.uv.y)),
            new float3(data.normal.x, data.normal.y, data.normal.z)
        );
    }

    public static implicit operator Vertex((Vector3 position, Vector2 uv, Vector3 normal) data)
    {
        return new Vertex(
            new float3(data.position.x, data.position.y, data.position.z),
            new half2(half(data.uv.x), half(data.uv.y)),
            new float3(data.normal.x, data.normal.y, data.normal.z)
        );
    }
}
