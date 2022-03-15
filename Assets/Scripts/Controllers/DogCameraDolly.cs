using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCameraDolly : MonoBehaviour
{
	public LayerMask mask;
	public float originOffset;
	public Transform[] camPoints;

	private Transform origin;

	private void Start ()
	{
		origin = transform.parent.transform;
	}

	void Update ()
	{
		//get furthest safe point from origin
		Vector3 furthestPoint = GetFurthestPoint ();

		//move camera forward
		DollyForward (furthestPoint);
	}

	void DollyForward (Vector3 point)
	{
		float distance = Vector3.Distance (point, origin.position);

		Vector3 dollyPos = transform.localPosition;
		dollyPos.z = -distance;

		transform.localPosition = dollyPos;
	}

	Vector3 GetFurthestPoint ()
    {
		Vector3 furthestPoint = camPoints[0].position;

		for (int i = 0; i < camPoints.Length; i++)
		{
			//perform linecast from player to each camera point
			Vector3 hitPoint = LineCast (origin.position, camPoints[i].position, originOffset);

			//set furthest distance if hit point is closer to origin
			if (F.FastDistance (origin.position, hitPoint) < F.FastDistance (origin.position, furthestPoint))
				furthestPoint = hitPoint;
		}

		return furthestPoint;
	}

	Vector3 LineCast (Vector3 start, Vector3 end, float offset)
	{
		//offset origin from centre
		Vector3 offsetStart = (end - start).normalized * offset + start;// Vector3.Lerp (start, end, offset * Vector3.Distance (start, transform.position));

		//if origin is not inside an object
		if (Physics.OverlapSphere (offsetStart, 0.1f, mask, QueryTriggerInteraction.Ignore).Length <= 0)
		{
			Debug.DrawLine (offsetStart, end);
            //if hit return hit point, otherwise return outermost position
            RaycastHit hit;
			if (Physics.Linecast (offsetStart, end, out hit, layerMask: mask, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
				return hit.point;
			else
				return camPoints[0].position;
		}
		//if origin is inside an object cast from centre
		else
		{
			Debug.DrawLine (start, end);
            //if hit return hit point, otherwise return outermost position
            RaycastHit hit;
			if (Physics.Linecast (start, end, out hit, layerMask: mask, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
				return hit.point;
			else
				return camPoints[0].position;
		}
	}
}
