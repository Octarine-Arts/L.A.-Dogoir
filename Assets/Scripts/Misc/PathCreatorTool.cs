using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathCreatorTool : MonoBehaviour
{
    public SmellTrail targetTrail;
    public float distanceThreshold;

    private Vector3 lastPoint;
    private void Update ()
    {
        if (lastPoint == null)
        {
            if (targetTrail.pathPoints.Count <= 0)
                lastPoint = CreatePoint ();
            else
                lastPoint = targetTrail.pathPoints[targetTrail.pathPoints.Count - 1];
        }

        if (F.FastDistance (transform.position, lastPoint) > distanceThreshold) lastPoint = CreatePoint ();
    }

    private Vector3 CreatePoint ()
    {
        targetTrail.pathPoints.Add (transform.position);
        return transform.position;
    }
}
