using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellTrail : MonoBehaviour
{
    public Transform[] points;
    public Transform target;

    private LineRenderer line;

    private void Awake ()
    {
        line = GetComponent<LineRenderer> ();
    }

    private void Update ()
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            Debug.DrawLine (points[i].position, points[i + 1].position, Color.white);
            Debug.DrawRay (NearestPointOnLine (points[i].position, points[i + 1].position, target.position, out float ta), Vector3.up * 0.2f, Color.red);
        }

        Vector3 nearestPoint = NearestPointOnPath (points, target.position, out int pathIndex, out float t);

        Debug.DrawRay (nearestPoint, Vector3.up * 0.5f, Color.blue);

        float pathExtension = Mathf.Clamp (t + 3.5f, 0, points.Length - pathIndex - 1);
        Vector3[] pathPoints = new Vector3[Mathf.CeilToInt (pathExtension + 1)];
        pathPoints[0] = nearestPoint;
        for (int i = 1; i < pathExtension; i++)
        {
            pathPoints[i] = points[Mathf.Clamp (i + pathIndex, 0, points.Length - 1)].position;
        }

        if (pathExtension < points.Length - pathIndex - 1)
        {
            Vector3 start = points[pathPoints.Length + pathIndex - 2].position;
            Vector3 end = points[pathPoints.Length + pathIndex - 1].position;
            pathPoints[pathPoints.Length - 1] = start + (pathExtension % 1f) * (end - start);
        }
        else
            pathPoints[pathPoints.Length - 1] = points[points.Length - 1].position;

        line.positionCount = pathPoints.Length;
        line.SetPositions (pathPoints);
    }

    private Vector3 NearestPointOnPath (Transform[] path, Vector3 point, out int pathIndex, out float t)
    {
        Vector3 nearestPoint = Vector3.zero;
        float nearestDist = 99999999;
        pathIndex = 0;
        t = 0;
        for (int i = 0; i < path.Length - 1; i++)
        {
            Vector3 currentPoint = NearestPointOnLine (path[i].position, path[i + 1].position, point, out float currentT);
            float currentDist = F.FastDistance (currentPoint, point);
            if (currentDist <= nearestDist)
            {
                pathIndex = i;
                t = currentT;
                nearestPoint = currentPoint;
                nearestDist = currentDist;
            }
        }

        return nearestPoint;
    }

    private Vector3 NearestPointOnLine (Vector3 start, Vector3 end, Vector3 point, out float t)
    {
        float length = F.FastDistance (start, end);

        if (length == 0)
        {
            t = 0;
            return start;
        }

        t = Mathf.Clamp01 (Vector3.Dot (point - start, end - start) / length);

        if (t <= 0) return start;
        else if (t >= 1) return end;

        return start + t * (end - start);
    }
}
