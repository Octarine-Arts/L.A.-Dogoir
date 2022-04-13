using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public PlayerSpecies player;
    public GameObject prefab;
    private List<SpeechBubble> messageQueue = new List<SpeechBubble> ();

    public void SpawnBubble (string message)
    {
        SpeechBubble bubble = Instantiate (prefab, transform).GetComponentInChildren<SpeechBubble> ();
        bubble.manager = this;
        bubble.SetMessage (message);
        messageQueue.Add (bubble);
    }

    public void RemoveBubble (SpeechBubble bubble)
    {
        messageQueue.Remove (bubble);
        UpdatePositions ();
    }

    public void UpdatePositions ()
    {
        StopAllCoroutines ();
        StartCoroutine (IUpdatePositions ());
    }

    private IEnumerator IUpdatePositions ()
    {
        float currentHeight = 0;
        float[] toHeights = new float[messageQueue.Count];
        float[] fromHeights = new float[messageQueue.Count];
        for (int i = 0; i < toHeights.Length; i++)
        {
            fromHeights[i] = messageQueue[i].transform.localPosition.y;
            toHeights[toHeights.Length - i - 1] = currentHeight;
            currentHeight += messageQueue[i].BubbleHeight;
        }

        float time = 0.3f;
        for (float elapsed = 0; elapsed < time; elapsed += Time.deltaTime)
        {
            float t = elapsed / time;
            t *= t * (3f - 2f * t);
            for (int i = 0; i < toHeights.Length; i++)
            {
                float lerpHeight = Mathf.Lerp (fromHeights[i], toHeights[i], t);
                messageQueue[i].transform.localPosition = new Vector3 (0, lerpHeight);
                print (lerpHeight);
            }
            yield return null;
        }
    }
}

public enum PlayerSpecies
{
    Dog,
    Human
}