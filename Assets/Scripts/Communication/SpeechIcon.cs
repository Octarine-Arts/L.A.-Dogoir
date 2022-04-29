using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechIcon : MonoBehaviour
{
    public Transform target;
    public float canvasRefWidth;

    private RectTransform icon;
    private Camera cam;
    private Rect canvasRect;
    private Animator anim;

    private void Awake()
    {
        icon = GetComponent<RectTransform> ();
        cam = Camera.main;
        canvasRect = new Rect(Vector2.zero, new Vector2(canvasRefWidth, canvasRefWidth * ((float)Screen.height / Screen.width)));
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        PositionIcon(out bool isOnScreen);
        anim.SetBool("isOnScreen", isOnScreen);
    }

    private void PositionIcon(out bool isOnScreen)
    {
        bool isBehind = Vector3.Dot(target.position - cam.transform.position, cam.transform.forward) < 0;
        Vector3 targetPos = cam.WorldToViewportPoint(target.position) * canvasRect.size;

        if (isBehind)
            targetPos *= -1;//new Vector2(-canvasRect.width, 1);

        isOnScreen = !isBehind && canvasRect.Contains(targetPos);
        if (!isOnScreen)
            GetIndicatorPosition(ref targetPos, canvasRect.size / 2f, canvasRect.size / 2f);

        Rect clampedRect = icon.rect;
        clampedRect.position = targetPos;
        clampedRect.ClampWithinBounds(canvasRect);

        icon.anchoredPosition = clampedRect.position;
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