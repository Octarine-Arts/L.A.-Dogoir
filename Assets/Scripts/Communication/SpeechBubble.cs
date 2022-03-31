using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public string testMessage;
    [SerializeField] private Vector2 padding;

    private SpriteRenderer sprite;
    private TextMeshPro text;

    private void Awake ()
    {
        sprite = GetComponent<SpriteRenderer> ();
        text = GetComponentInChildren<TextMeshPro> ();
    }

    private void Start ()
    {
        SetMessage ("Walnuts, Peanuts, Pineapple Smells");
    }

    private void Update ()
    {
        //sprite.size = (Vector2) text.textBounds.size + padding;
        if (text.text != testMessage)
            SetMessage (testMessage);
    }

    public void SetMessage (string message)
    {
        StartCoroutine (ISetMessage (message));
    }

    private IEnumerator ISetMessage (string message)
    {
        text.enabled = false;
        text.text = message;

        yield return null;

        float time = 0.5f;
        Vector2 targetSize = (Vector2) text.textBounds.size + padding;

        for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
        {
            float t = elapsed / time;
            sprite.size = Vector2.Lerp (Vector2.zero, targetSize, t);
            yield return null;
        }

        text.enabled = true;
    }
}
