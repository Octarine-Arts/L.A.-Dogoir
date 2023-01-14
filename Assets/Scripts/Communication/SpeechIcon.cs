using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeechIcon : MonoBehaviour
{
    public float disappearDelay, canvasRefWidth;
    public Sprite humanIcon, dogIcon;
    public Image iconImage;
    public TextMeshProUGUI text;

    private PlayerSpecies targetSpecies;
    private Transform target;
    private RectTransform icon;
    private Camera cam;
    private Rect canvasRect;
    private Animator anim;

    private void Awake()
    {
        icon = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
        canvasRect = new Rect(Vector2.zero, new Vector2(canvasRefWidth, canvasRefWidth * ((float)Screen.height / Screen.width)));

        //DEBUG ONLY
        //targetSpecies = PlayerSpecies.Dog;
        //iconImage.sprite = dogIcon;
        //target = FindObjectOfType<Transform>();
    }

    private void Update()
    {
        if (!cam || !target) return;

        PositionIcon(out bool isOnScreen);
        anim.SetBool("isOnScreen", isOnScreen);
        //if (Input.GetKeyDown(KeyCode.E))
        //    PlayerWoofed(PlayerSpecies.Dog, "Hello " + Time.time);
    }

    public void PlayerWoofed(PlayerSpecies player, string message)
    {
        if (player != targetSpecies) return;

        if (!anim.GetBool("isActive"))
        {
            anim.SetBool("isActive", true);
            text.text = "";
        }

        string[] splitMessage = text.text.Split('\n');
        if (splitMessage.Length > 0 && splitMessage.Last ().Length > 1)
            text.text = splitMessage.Last() + '\n' + message;
        else
            text.text = message;

        StopAllCoroutines();
        StartCoroutine(IDisableTimer(disappearDelay));
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

    private IEnumerator IDisableTimer(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("isActive", false);
    }

    private void OnPlayersSpawned(GameObject human, GameObject dog)
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Human)
        {
            cam = human.transform.root.GetComponentInChildren<Camera> ();
            target = dog.transform;
            targetSpecies = PlayerSpecies.Dog;
            iconImage.sprite = dogIcon;
        }
        else
        {
            cam = dog.transform.root.GetComponentInChildren<Camera> ();
            target = human.transform;
            targetSpecies = PlayerSpecies.Human;
            iconImage.sprite = humanIcon;
        }
    }

    private void OnEnable()
    {
        EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }

    private void OnDisable()
    {
        EventManager.I.OnPlayersSpawned -= OnPlayersSpawned;
    }
}