using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CommunicationUI : MonoBehaviour
{
    public LexiconEntry phrases;
    public LexiconEntry[] categories;

    public GameObject button, phraseText, blankSpace, wordSelector;
    public Transform phraseButtons, phraseTexts, categoryButtons, wordSelectors;

    private int activeWordIndex;

    public void Awake ()
    {
        foreach (string phrase in phrases)
            PlaceButton (phrase, phraseButtons, () => OnPhraseSelected (phrase));
        for (int c = 0; c < categories.Length; c++)
        {
            int uhhh = c;
            PlaceButton (categories[c].name, categoryButtons, () => OnCategorySelected (uhhh));
            Transform parent = Instantiate (wordSelector, wordSelectors).transform;
            foreach (string word in categories[c])
                PlaceButton (word, parent, () => OnWordSelected (word));
        }
    }

    private void OnPhraseSelected (string phrase)
    {
        phraseButtons.gameObject.SetActive (false);
        phraseTexts.gameObject.SetActive (true);
        PlacePhrase (phrase);
    }

    private void OnBlankSelected (int siblingIndex)
    {
        categoryButtons.gameObject.SetActive (true);
        activeWordIndex = siblingIndex;
    }

    private void OnCategorySelected (int category)
    {
        categoryButtons.gameObject.SetActive (false);
        for (int child = 0; child < wordSelectors.childCount; child++)
            wordSelectors.GetChild (child).gameObject.SetActive (child == category);
        wordSelectors.gameObject.SetActive (true);
    }

    private void OnWordSelected (string word)
    {
        wordSelectors.gameObject.SetActive (false);
        if (phraseTexts.GetChild (activeWordIndex).GetComponentInChildren<Text> ().text == "____")
            InsertPhraseWord (word, activeWordIndex);
        else
        {
            Text text = phraseTexts.GetChild (activeWordIndex).GetComponentInChildren<Text> ();
            text.text = word;
            RectTransform rect = phraseTexts.GetChild (activeWordIndex).GetComponent<RectTransform> ();
            rect.sizeDelta = new Vector2 (text.preferredWidth, rect.sizeDelta.y);
        }

        print (phraseTexts.GetChild (activeWordIndex).GetComponentInChildren<Text> ().preferredWidth);
    }

    private void PlacePhrase (string phrase)
    {
        if (phraseTexts.childCount > 0)
            phraseTexts.OffTheWeans ();

        int last = 0;
        for (int i = 0; i < 20; i++)
        {
            if (last == phrase.Length - 1) break;

            int next = phrase.IndexOf ('_', last);

            if (next == -1)
            {
                PlacePhraseText (phrase.Substring (last));
                break;
            }

            if (next - last > 0)
                PlacePhraseText (phrase.Substring (last, next - last));

            PlacePhraseBlank ();
            
            last = Mathf.Min (next + 1, phrase.Length - 1);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate (phraseTexts.GetComponent<RectTransform> ());
    }

    private void PlaceButton (string text, Transform parent, UnityAction onClick)
    {
        GameObject buttonObj = Instantiate (button, parent);
        buttonObj.GetComponentInChildren<Text> ().text = text;
        buttonObj.GetComponent<Button> ().onClick.AddListener (onClick);
    }

    private void PlacePhraseText (string text)
    {
        Instantiate (phraseText, phraseTexts).GetComponent<Text> ().text = text;
    }

    private void PlacePhraseBlank ()
    {
        GameObject blank = Instantiate (blankSpace, phraseTexts);
        blank.GetComponent<Button> ().onClick.AddListener (() => OnBlankSelected (blank.transform.GetSiblingIndex ()));
    }

    private void InsertPhraseWord (string word, int index)
    {
        GameObject pWord = Instantiate (blankSpace, phraseTexts);
        pWord.GetComponent<Button> ().onClick.AddListener (() => OnBlankSelected (index));
        pWord.transform.SetSiblingIndex (index);

        Text text = pWord.GetComponentInChildren<Text> ();
        text.text = word;
        RectTransform rect = pWord.GetComponent<RectTransform> ();
        rect.sizeDelta = new Vector2 (text.preferredWidth, rect.sizeDelta.y);
    }
}
