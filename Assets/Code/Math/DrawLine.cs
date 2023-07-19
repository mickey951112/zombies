using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawLine : MonoBehaviour
{
    void OnEnable()
    {
        var mesh = new ProceduralMesh(vertexCount: 6, triangleCount: 4);

        var leftBottomFront = mesh.AddVertex((position: (0, 0, 0), uv: (0, 0), normal: (-1, 0, 1)));
        var rightBottomFront = mesh.AddVertex((position: (1, 0, 0), uv: (1, 0), normal: (1, 0, 1)));
        var leftTopFront = mesh.AddVertex((position: (0, 1, 0), uv: (0, 1), normal: (-1, 0, 1)));
        var rightTopFront = mesh.AddVertex((position: (1, 1, 0), uv: (1, 1), normal: (1, 0, 1)));
        var rightBottomBack = mesh.AddVertex(
            (position: (1, 0, -1), uv: (0, 0), normal: (1, 0, -1))
        );
        var rightTopBack = mesh.AddVertex((position: (1, 1, -1), uv: (1, 0), normal: (1, 0, -1)));

        mesh.AddTriagle(leftBottomFront, rightBottomFront, leftTopFront);
        mesh.AddTriagle(rightTopFront, leftTopFront, rightBottomFront);
        mesh.AddTriagle(rightBottomFront, rightBottomBack, rightTopFront);
        mesh.AddTriagle(rightTopFront, rightBottomBack, rightTopBack);

        GetComponent<MeshFilter>().mesh = mesh.Draw();
    }
}
