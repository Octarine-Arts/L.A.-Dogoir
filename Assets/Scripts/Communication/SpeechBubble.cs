using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public float BubbleHeight => targetSize.y;
    public BubbleManager manager;
    public float sizePercentage;
    [SerializeField] private Vector2 padding;

    private SpriteRenderer sprite;
    private TextMeshPro text;
    private Animator anim;
    private Vector2 targetSize;

    private void Awake ()
    {
        sprite = GetComponentInChildren<SpriteRenderer> ();
        text = GetComponentInChildren<TextMeshPro> ();
        anim = GetComponent<Animator> ();
    }

    private void Update ()
    {
        sprite.size = targetSize * sizePercentage;
    }

    public void SetMessage (string message)
    {
        StartCoroutine (ISetMessage (message));
    }

    private IEnumerator ISetMessage (string message)
    {
        //Color textColour = text.color;
        //text.color = Color.clear;

        text.text = message;

        yield return null;

        targetSize = (Vector2) text.textBounds.size + padding;
        manager.UpdatePositions ();

        yield return new WaitForSeconds (0.5f);

        //text.color = textColour;

        yield return new WaitForSeconds (10f);

        anim.SetTrigger ("Exit");
        manager.RemoveBubble (this);

        yield return new WaitForSeconds (0.5f);

        Destroy (gameObject);
    }
}
