using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public static class BezierCurve
{
    // Calculate a point on a Bezier curve based on a given set of control points and a 't' parameter
    public static Vector3 CalculateBezierPoint(float t, List<Vector3> controlPoints)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * controlPoints[0]; // Start point influence
        p += 3 * uu * t * controlPoints[1]; // Control point 1 influence
        p += 3 * u * tt * controlPoints[2]; // Control point 2 influence
        p += ttt * controlPoints[3]; // End point influence

        return p;
    }
}
