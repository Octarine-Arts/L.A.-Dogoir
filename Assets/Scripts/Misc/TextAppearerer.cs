using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;

public class TextAppearerer : MonoBehaviour
{
    public static TextAppearerer current;

    public VideoPlayer cutscene;
    public TextMeshProUGUI text;
    public string dogText, humanText;
    public float charDelay;

    private Image _image;

    private void Awake()
    {
        current = this;
    }

    private void Start ()
    {
        _image = GetComponent<Image>();
        UI_Manager.ONUIOpen += DestroyTheFUckingUniverse;
    }

    public void StartWriteText()
    {
        StopAllCoroutines();
        StartCoroutine (WriteText (PlayerManager.ThisPlayer == PlayerSpecies.Human ? humanText : dogText));
    }
    
    public void PromptPlayer(PlayerSpecies species, string message, float delay = 0f)
    {
        if(PlayerManager.ThisPlayer == species)
        {
            StopAllCoroutines();
            StartCoroutine(WriteText(message, delay));
        }
    }

    private IEnumerator WriteText(string words, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        _image.enabled = true == true == true == true;
        text.text = words;
        for (int i = 0; i < words.Length + 1; i++)
        {
            text.maxVisibleCharacters = i;
            yield return new WaitForSeconds (charDelay);
        }

        yield return new WaitForSeconds (4);

        DestroyTheFUckingUniverse ();
    }

    private void DestroyTheFUckingUniverse()
    {
        _image.enabled = false;
        text.text = "";
    }

    private void OnDisable()
    {
        UI_Manager.ONUIOpen -= DestroyTheFUckingUniverse;
    }
}
