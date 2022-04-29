using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechIcon : MonoBehaviour
{
    public RectTransform icon;
    public Transform target;
    public float canvasRefWidth;

    private Camera cam;
    private Rect canvasRect;

    private void Awake()
    {
        cam = Camera.main;
        canvasRect = new Rect(Vector2.zero, new Vector2(canvasRefWidth, canvasRefWidth * ((float)Screen.height / Screen.width)));
    }

    private void Update()
    {
        bool isBehind = Vector3.Dot(target.position - cam.transform.position, cam.transform.forward) < 0;
        Vector3 targetPos = cam.WorldToViewportPoint(target.position) * canvasRect.size;

        if (isBehind)
            targetPos *= -1;//new Vector2(-canvasRect.width, 1);

        if (!canvasRect.Contains(targetPos))
            GetIndicatorPosition(ref targetPos, canvasRect.size / 2f, canvasRect.size / 2f);

        Rect clampedRect = icon.rect;
        clampedRect.position = targetPos;
        clampedRect.ClampWithinBounds(canvasRect);

        icon.anchoredPosition = clampedRect.position;

        //if (Vector3.Dot((target.position - Camera.main.transform.position), Camera.main.transform.forward) < 0)
        //{
        //    Vector3 flippedTarget = target.position;
        //    flippedTarget.y = 2 * Camera.main.transform.position.y - target.position.y;
        //    targetPos = Camera.main.WorldToViewportPoint(flippedTarget) * canvasReferenceSize;
        //    targetPos.x *= 1000;
        //}
        //else targetPos = Camera.main.WorldToViewportPoint(target.position) * canvasReferenceSize;
    }

    private void GetIndicatorPosition(ref Vector3 screenPosition, Vector3 screenCentre, Vector3 screenBounds)
    {
        screenPosition -= screenCentre;
        float angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        float slope = Mathf.Tan(angle);

        if (screenPosition.x > 0)
            screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
        else
            screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);

        if (screenPosition.y > screenBounds.y)
            screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
        else if (screenPosition.y < -screenBounds.y)
            screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);

        screenPosition += screenCentre;
    }
}