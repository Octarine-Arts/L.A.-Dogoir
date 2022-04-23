using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhraseBlank : MonoBehaviour
{
    public static PhraseBlank ActiveBlank => activeBlank;

    private static PhraseBlank activeBlank;
    private static PhraseBlank lastActiveBlank;
    private bool active = false;

    public Button wordButton;
    public GameObject delete, highlight;
    public TextMeshProUGUI text;

    public void Activate()
    {
        if (activeBlank != null)
            activeBlank.Deactivate();

        activeBlank = this;
        active = true;

        delete.SetActive (true);
        highlight.SetActive(true);
    }

    private void Deactivate()
    {
        if (activeBlank == this)
            activeBlank = null;
        active = false;

        delete.SetActive(false);
        highlight.SetActive(false);
    }

    public void OnClick()
    {
        if (!active) Activate();
        else TryDestroy();
    }

    public void SetWord (string word)
    {
        text.text = word;
    }

    private void TryDestroy()
    {
        int c = transform.GetSiblingIndex();
        bool canRemove = (c - 2 >= 0 && transform.parent.GetChild(c - 2).name.Contains("PhraseBlank")) ||
            (c + 2 < transform.parent.childCount && transform.parent.GetChild(c + 2).name.Contains("PhraseBlank"));

        if (canRemove)
        {
            if (lastActiveBlank != null) lastActiveBlank.Activate();

            Destroy(transform.parent.GetChild(c + 1).gameObject);
            Destroy(gameObject);
        }
    }
}
