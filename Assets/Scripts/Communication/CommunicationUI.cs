using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text;
using Photon.Pun;

public class CommunicationUI : MonoBehaviour
{
    public LexiconEntry phrases;
    public LexiconEntry[] categories;

    public GameObject button, phraseText, blankSpace, wordSelector, historyButton;
    public Transform UIContainer, oldOrNewPanel, historyPanel, phraseSelectPanel, phraseContainer, phrasePanel, categorySelectPanel, wordSelectPanel;
    //public UnityEngine.EventSystems.EventSystem eventSystem;

    private int activeWordIndex;
    private int activePanel = -1;
    //private string[] history = new string[3];
    private Queue<string> history = new Queue<string> ();
    private Button[] historyButtons;
    private PhotonView photonView;
    private BubbleManager dogBubbles, humanBubbles;

    private void Awake ()
    {
        photonView = GetComponent<PhotonView> ();
        photonView.ViewID = 69;

        //historyButtons = historyPanel.GetComponentsInChildren<Button> ();
        
        foreach (string phrase in phrases)
            PlaceButton (phrase, phraseSelectPanel, () => OnPhraseSelected (phrase));
        for (int c = 0; c < categories.Length; c++)
        {
            int uhhh = c;
            PlaceButton (categories[c].name, categorySelectPanel, () => OnCategorySelected (uhhh));
            Transform parent = Instantiate (wordSelector, wordSelectPanel).transform;
            foreach (string word in categories[c])
                PlaceButton (word, parent, () => OnWordSelected (word));
        }

        SelectFirstButton (phraseSelectPanel);

        if (EventManager.I != null)
            EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }

    private void OnPlayersSpawned (GameObject human, GameObject dog)
    {
        humanBubbles = human.transform.parent.GetComponentInChildren<BubbleManager> ();
        dogBubbles = dog.transform.parent.GetComponentInChildren<BubbleManager> ();
    }

    public void Submit ()
    {
        StringBuilder sb = new StringBuilder (50);
        foreach (Text text in phraseContainer.GetComponentsInChildren<Text> ())
        {
            if (text.text == "__") continue;

            sb.Append (text.text + " ");
        }
        string message = sb.ToString ();
        SubmitMessage (message);

        if (!history.Contains (message))
        {
            history.Enqueue (message);

            if (history.Count > 3)
                history.Dequeue ();
        }

        foreach (string h in history) print (h);
    }

    private void SubmitMessage (string message)
    {
        photonView.RPC ("PublishMessage", RpcTarget.All, PlayerManager.ThisPlayer, message);
        Close ();
    }

    [PunRPC]
    public void PublishMessage (PlayerSpecies player, string message)
    {
        switch (player)
        {
            case PlayerSpecies.Dog: dogBubbles.SpawnBubble (message);
                break;
            case PlayerSpecies.Human: humanBubbles.SpawnBubble (message);
                break;
        }
    }

    public void OnOldNewSelected (bool newPhrase)
    {
        if (newPhrase)
            SetPanel (2);
        else
        {
            historyPanel.OffTheWeans ();
            foreach (string h in history)
                PlaceButton (h, historyPanel, () => SubmitMessage (h));
            
            SetPanel (1);
        }
    }

    public void OnHistorySelected (int button)
    {

    }

    private void OnPhraseSelected (string phrase)
    {
        PlacePhrase (phrase);
        NextPanel ();
        LayoutRebuilder.ForceRebuildLayoutImmediate (phraseContainer.GetComponent<RectTransform> ());
    }

    private void OnBlankSelected (int siblingIndex)
    {
        NextPanel ();

        activeWordIndex = siblingIndex;
        SelectFirstButton (categorySelectPanel);
    }

    private void OnCategorySelected (int category)
    {
        NextPanel ();

        for (int child = 0; child < wordSelectPanel.childCount; child++)
            wordSelectPanel.GetChild (child).gameObject.SetActive (child == category);
        SelectFirstButton (wordSelectPanel);
    }

    private void OnWordSelected (string word)
    {
        NextPanel ();

        if (phraseContainer.GetChild (activeWordIndex).GetComponentInChildren<Text> ().text == "__")
            InsertPhraseWord (word, activeWordIndex);
        else
        {
            Text text = phraseContainer.GetChild (activeWordIndex).GetComponentInChildren<Text> ();
            text.text = word;
            RectTransform rect = phraseContainer.GetChild (activeWordIndex).GetComponent<RectTransform> ();
            rect.sizeDelta = new Vector2 (text.preferredWidth, rect.sizeDelta.y);
        }

        phraseContainer.GetChild (activeWordIndex + 1).GetComponent<Button> ().Select();
    }

    private void NextPanel () => SetPanel (activePanel + 1);
    private void PreviousPanel () => SetPanel (activePanel - 1 == 1 ? 0 : activePanel - 1);
    private void Open()
    {
        Player_StaticActions.DisableDogMovement();
        Player_StaticActions.DisableHumanMovement();
        SetPanel(0);
    }
    private void Close()
    {
        SetPanel(-1);
    }
    private void SetPanel (int panelIndex)
    {
        activePanel = panelIndex;
        print (activePanel);
        if (activePanel < 0)
        {
            Player_StaticActions.EnableDogMovement ();
            Player_StaticActions.EnableHumanMovement ();
            activePanel = -1;
        }
        else if (activePanel > 5)
        {
            activePanel = 3;
        }

        switch (activePanel)
        {
            case 0: SelectFirstButton (oldOrNewPanel);
                historyButton.SetActive (history.Count > 0);
                break;
            case 1: SelectFirstButton (historyPanel);
                break;
            case 2: SelectFirstButton (phraseSelectPanel);
                break;
            case 3: phraseContainer.GetChild (activeWordIndex).GetComponent<Button> ().Select ();
                break;
            case 4: SelectFirstButton (categorySelectPanel);
                break;
            case 5: SelectFirstButton (wordSelectPanel);
                break;
        }

        UIContainer.gameObject.SetActive (activePanel >= 0);
        oldOrNewPanel.gameObject.SetActive (activePanel == 0);
        historyPanel.gameObject.SetActive (activePanel == 1);
        phraseSelectPanel.gameObject.SetActive (activePanel == 2);
        phrasePanel.gameObject.SetActive (activePanel == 3);
        categorySelectPanel.gameObject.SetActive (activePanel == 4);
        wordSelectPanel.gameObject.SetActive (activePanel == 5);
    }

    private void PlacePhrase (string phrase)
    {
        if (phraseContainer.childCount > 0)
            phraseContainer.OffTheWeans ();

        bool firstBlank = true;
        int last = 0;
        for (int i = 0; i < 20; i++)
        {
            if (last == phrase.Length - 1) break;

            int next = phrase.IndexOf ('_', last);

            if (next == -1)
            {
                PlacePhraseText (phrase.Substring (last).Trim ());
                break;
            }

            if (next - last > 0)
                PlacePhraseText (phrase.Substring (last, next - last).Trim ());

            PlacePhraseBlank (firstBlank);
            firstBlank = false;
            
            last = Mathf.Min (next + 1, phrase.Length - 1);
        }
    }

    private void PlaceButton (string text, Transform parent, UnityAction onClick)
    {
        GameObject buttonObj = Instantiate (button, parent);
        buttonObj.GetComponentInChildren<Text> ().text = text;
        buttonObj.GetComponent<Button> ().onClick.AddListener (onClick);
    }

    private void PlacePhraseText (string text)
    {
        Instantiate (phraseText, phraseContainer).GetComponent<Text> ().text = text;
    }

    private void PlacePhraseBlank (bool select = false)
    {
        GameObject blank = Instantiate (blankSpace, phraseContainer);
        Button button = blank.GetComponent<Button> ();
        button.onClick.AddListener (() => OnBlankSelected (blank.transform.GetSiblingIndex ()));
        if (select)
            activeWordIndex = blank.transform.GetSiblingIndex ();
    }

    private void InsertPhraseWord (string word, int index)
    {
        GameObject pWord = Instantiate (blankSpace, phraseContainer);
        pWord.GetComponent<Button> ().onClick.AddListener (() => OnBlankSelected (index));
        pWord.transform.SetSiblingIndex (index);

        Text text = pWord.GetComponentInChildren<Text> ();
        text.text = word;
        RectTransform rect = pWord.GetComponent<RectTransform> ();
        rect.sizeDelta = new Vector2 (text.preferredWidth, rect.sizeDelta.y);
    }

    private void SelectFirstButton (Transform parent) => parent.GetComponentInChildren<Button> ()?.Select ();

    private bool cancelHeld = true;
    private bool openHeld = true;
    private void Update ()
    {
        if (activePanel < 0 && !openHeld && Input.GetAxis ("Jump") > 0)
        {
            openHeld = true;
            Open ();
        }
        else if (Input.GetAxis ("Jump") <= 0)
            openHeld = false;

        if (activePanel >= 0 && !cancelHeld && Input.GetAxis ("Cancel") > 0)
        {
            cancelHeld = true;
            PreviousPanel ();
        }
        else if (Input.GetAxis ("Cancel") <= 0)
            cancelHeld = false;
    }
}
