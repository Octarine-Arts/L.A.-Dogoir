using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public GameObject prefab;

    public void SpawnBubble (string message)
    {
        SpeechBubble bubble = Instantiate (prefab, transform).GetComponentInChildren<SpeechBubble> ();
        bubble.manager = this;
        bubble.SetMessage (message);
    }

    public void UpdatePositions () => StartCoroutine (IUpdatePositions ());

    private IEnumerator IUpdatePositions ()
    {
        float currentHeight = 0;
        float[] bubbleHeights = new float[transform.childCount];
        for (int i = 0; i < bubbleHeights.Length; i++)
        {
            bubbleHeights[i] = currentHeight;
            currentHeight += transform.GetChild (i).GetComponentInChildren<SpeechBubble> ().BubbleHeight;
        }

        float time = 1.5f;
        for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
        {
            float t = elapsed / time;
            for (int i = 0; i < bubbleHeights.Length; i++)
            {
                float lerpHeight = Mathf.Lerp (transform.GetChild (i).localPosition.y, bubbleHeights[bubbleHeights.Length - i - 1], t);
                transform.GetChild (i).localPosition = new Vector3 (0, lerpHeight);
            }
            yield return null;
        }
    }
}
