using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellTrail : MonoBehaviour
{
    public GameObject lineObject;
    public int poolSize;
    public Transform[] pointsTransforms;
    public Transform target;
    public float radius;

    private TrailPoint[] points;
    private LineRenderer[] _renderers;
    private LineRenderer[] renderers { get { if (_renderers == null) AttachRenderers (poolSize); return _renderers; } }
    private int activeRenderer;

    private void Awake ()
    {
        radius *= radius;

        points = new TrailPoint[pointsTransforms.Length];
        for (int i = 0; i < pointsTransforms.Length; i++)
        {
            points[i] = new TrailPoint (pointsTransforms[i].position);
            if (i > 0)
            {
                points[i].SetPrevious (points[i - 1]);
                points[i - 1].SetNext (points[i]);
            }
        }
    }

    private void Update ()
    {
        foreach (TrailPoint point in points) if (point.Next != null)
            Debug.DrawLine (point, point.Next, Color.white);

        RenderTrail (GetTrailSegments ());
    }

    private List<List<Vector3>> GetTrailSegments ()
    {
        List<List<Vector3>> segments = new List<List<Vector3>> ();

        foreach (TrailPoint point in points)
            point.UpdateDistance (target.position);

        foreach (TrailPoint point in points)
        {
            //if point in threshold
            if (point.Distance <= radius)
            {
                //if last point wasn't in threshold or segments is zero, add a segment
                if (segments.Count <= 0 || (point.Previous != null && point.Previous.Distance > radius)) segments.Add (new List<Vector3> ());

                //if last point wasn't in threshold add first intermediate point
                if (point.Previous != null && point.Previous.Distance > radius)
                {
                    // calculate point along line
                    float t = Mathf.InverseLerp (point.Previous.Distance, point.Distance, radius);
                    segments.Last ().Add ((point.Point - point.Previous.Point) * t + point.Previous.Point);
                }
                segments.Last ().Add (point);
            }
            //if last point was in threshold add intermediate point
            else if (point.Previous != null && point.Previous.Distance < radius)
            {
                float t = Mathf.InverseLerp (point.Previous.Distance, point.Distance, radius);
                segments.Last ().Add ((point.Point - point.Previous.Point) * t + point.Previous.Point);
            }
        }

        return segments;
    }

    public void RenderTrail (List<List<Vector3>> segments)
    {
        UnrenderBeams ();

        foreach (List<Vector3> segment in segments)
        {
            LineRenderer line = renderers[activeRenderer];
            line.enabled = true;
            line.positionCount = segment.Count;
            line.SetPositions (segment.ToArray());

            if (++activeRenderer >= renderers.Length)
                throw new System.Exception ("All line renderers used up, try increasing pool size");
        }
    }

    private void UnrenderBeams ()
    {
        for (; activeRenderer > 0; activeRenderer--)
            renderers[activeRenderer].enabled = false;
    }

    private void AttachRenderers (int amount)
    {
        _renderers = new LineRenderer[amount];
        for (int i = 0; i < amount; i++)
            _renderers[i] = Instantiate (lineObject, transform).GetComponent<LineRenderer> ();
    }

    private void OnDrawGizmos ()
    {
        Gizmos.DrawWireSphere (target.position, Mathf.Sqrt (radius));
    }
}

public class TrailPoint
{
    public Vector3 Point => point;
    public TrailPoint Previous => previous;
    public TrailPoint Next => next;
    public float Distance => distance;

    private Vector3 point;
    private TrailPoint previous, next;
    private float distance;

    public TrailPoint (Vector3 point)
    {
        this.point = point;
        this.previous = previous;
        this.next = next;
    }

    public float UpdateDistance (Vector3 target)
    {
        distance = F.FastDistance (point, target);
        return distance;
    }

    public void SetPrevious (TrailPoint previous) => this.previous = previous;
    public void SetNext (TrailPoint next) => this.next = next;

    public static implicit operator Vector3 (TrailPoint point) => point.Point;
}