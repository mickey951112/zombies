using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugDraw
{
    public static void xAtPoint(
        Vector3 lineDirection,
        Vector3 atPoint,
        Color color,
        float duration = 0
    )
    {
        // Draw a small x at the point on the line
        var xAxis = Quaternion.LookRotation(Vector3.right) * lineDirection;
        var yAxis = Quaternion.LookRotation(Vector3.up) * lineDirection;
        Debug.DrawLine(
            atPoint - xAxis * 0.1f - yAxis * 0.1f,
            atPoint + xAxis * 0.1f + yAxis * 0.1f,
            color,
            duration
        );
        Debug.DrawLine(
            atPoint - xAxis * 0.1f + yAxis * 0.1f,
            atPoint + xAxis * 0.1f - yAxis * 0.1f,
            color,
            duration
        );
    }
}
