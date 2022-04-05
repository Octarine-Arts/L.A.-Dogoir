using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathCreatorTool : MonoBehaviour
{
    public Transform parent;
    public float distanceThreshold;

    private Vector3 lastPoint;
    private void Update ()
    {
        if (lastPoint == null) lastPoint = CreatePoint ();

        if (F.FastDistance (transform.position, lastPoint) > distanceThreshold) lastPoint = CreatePoint ();
    }

    private Vector3 CreatePoint ()
    {
        GameObject spawn = new GameObject ();
        spawn.transform.SetParent (parent);
        spawn.transform.position = transform.position;
        return transform.position;
    }
}
