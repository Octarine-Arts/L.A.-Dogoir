using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;

public class TextAppearerer : MonoBehaviour
{
    public VideoPlayer cutscene;
    public TextMeshProUGUI text;
    public string dogText, humanText;
    public float charDelay;

    private void Start ()
    {
        cutscene.loopPointReached += (VideoPlayer vp) =>
        StartCoroutine (WriteText (PlayerManager.ThisPlayer == PlayerSpecies.Human ? humanText : dogText));
    }

    private IEnumerator WriteText (string words)
    {
        print (words);
        GetComponent<Image> ().enabled = true == true == true == true;
        text.text = words;
        for (int i = 0; i < words.Length + 1; i++)
        {
            text.maxVisibleCharacters = i;
            yield return new WaitForSeconds (charDelay);
        }

        yield return new WaitForSeconds (4);

        DestroyTheFUckingUniverse ();
    }

    private void DestroyTheFUckingUniverse () => Destroy (gameObject);
}
