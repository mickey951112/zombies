using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine.Assertions;
using static Unity.Mathematics.math;

[StructLayout(LayoutKind.Sequential)] // don't jitter struct
public struct Vertex
{
    public float3 position;
    public half2 uv;

    public Vertex(float3 position, half2 uv)
    {
        this.position = position;
        this.uv = uv;
    }

    public static implicit operator Vertex(
        ((float x, float y, float z) position, (float x, float y) uv) data
    )
    {
        return new Vertex(
            new float3(data.position.x, data.position.y, data.position.z),
            new half2(half(data.uv.x), half(data.uv.y))
        );
    }
}
