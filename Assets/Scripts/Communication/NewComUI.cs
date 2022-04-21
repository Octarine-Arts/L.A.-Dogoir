using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text;
using TMPro;
using Photon.Pun;

public class NewComUI : MonoBehaviour
{
    public LexiconEntry phrases;
    public LexiconEntry[] categories;

    public GameObject button, phraseText, phraseBlank, historyButton;
    public Transform UIContainer, historyPanel, phraseSelectPanel, phraseSelectContainer, phraseFillPanel, phraseContainer;
    public Image categoryBackground;
    public Transform[] categoryContainers;
    public Sprite[] categoryTabSprites;

    private int activeWordIndex;
    private Transform activePanel;
    private int activeTab = 0;
    private Queue<string> history = new Queue<string> ();
    private PhotonView photonView;
    private BubbleManager dogBubbles, humanBubbles;
    private bool open = false;

    private void Awake ()
    {
        photonView = GetComponent<PhotonView> ();
        photonView.ViewID = 69;

        activePanel = phraseSelectPanel;

        foreach (string phrase in phrases)
            PlaceButton(phrase, phraseContainer, () => OnPhraseSelected(phrase));
        for (int c = 0; c < categories.Length; c++)
        {
            foreach (string word in categories[c])
                PlaceButton (word, categoryContainers[c], () => OnWordSelected (word));
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
        foreach (TextMeshProUGUI text in phraseContainer.GetComponentsInChildren<TextMeshProUGUI> ())
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

    public void OnTabSelected(int tab)
    {
        categoryContainers[activeTab].gameObject.SetActive(false);
        activeTab = tab;
        categoryContainers[activeTab].gameObject.SetActive(true);
        categoryBackground.sprite = categoryTabSprites[tab];
    }

    public void OnHistorySelected (int button)
    {
        historyPanel.OffTheWeans ();
        foreach (string h in history)
            PlaceButton (h, historyPanel, () => SubmitMessage (h));

        SetPanel (historyPanel);
    }

    private void OnPhraseSelected (string phrase)
    {
        PlacePhrase (phrase);
        SetPanel(phraseFillPanel);
        LayoutRebuilder.ForceRebuildLayoutImmediate (phraseContainer.GetComponent<RectTransform> ());
    }

    private void OnBlankSelected (int siblingIndex)
    {
        activeWordIndex = siblingIndex;
    }

    private void OnWordSelected (string word)
    {
        if (phraseContainer.GetChild (activeWordIndex).GetComponentInChildren<TextMeshProUGUI> ().text == "__")
            InsertPhraseWord (word, activeWordIndex);
        else
        {
            TextMeshProUGUI text = phraseContainer.GetChild (activeWordIndex).GetComponentInChildren<TextMeshProUGUI> ();
            text.text = word;
            RectTransform rect = phraseContainer.GetChild (activeWordIndex).GetComponent<RectTransform> ();
            rect.sizeDelta = new Vector2 (text.preferredWidth, rect.sizeDelta.y);
        }

        phraseContainer.GetChild (activeWordIndex + 1).GetComponent<Button> ().Select();
    }

    private void Open()
    {
        Player_StaticActions.DisableDogMovement();
        Player_StaticActions.DisableHumanMovement();
        activePanel = phraseSelectPanel;
        activePanel.gameObject.SetActive(true);
        UIContainer.gameObject.SetActive(true);
        open = true;
    }

    private void Close()
    {
        Player_StaticActions.EnableDogMovement();
        Player_StaticActions.EnableHumanMovement(); 
        UIContainer.gameObject.SetActive(false);
        open = false;
    }

    private void SetPanel (Transform panel)
    {
        activePanel.gameObject.SetActive(false);
        activePanel = panel;
        activePanel.gameObject.SetActive(true);
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
        buttonObj.GetComponentInChildren<TextMeshProUGUI> ().text = text;
        buttonObj.GetComponent<Button> ().onClick.AddListener (onClick);
    }

    private void PlacePhraseText (string text)
    {
        Instantiate (phraseText, phraseContainer).GetComponent<TextMeshProUGUI> ().text = text;
    }

    private void PlacePhraseBlank (bool select = false)
    {
        GameObject blank = Instantiate (phraseBlank, phraseContainer);
        Button button = blank.GetComponent<Button> ();
        button.onClick.AddListener (() => OnBlankSelected (blank.transform.GetSiblingIndex ()));
        if (select)
            activeWordIndex = blank.transform.GetSiblingIndex ();
    }

    private void InsertPhraseWord (string word, int index)
    {
        GameObject pWord = Instantiate (phraseBlank, phraseContainer);
        pWord.GetComponent<Button> ().onClick.AddListener (() => OnBlankSelected (index));
        pWord.transform.SetSiblingIndex (index);

        TextMeshProUGUI text = pWord.GetComponentInChildren<TextMeshProUGUI> ();
        text.text = word;
        RectTransform rect = pWord.GetComponent<RectTransform> ();
        rect.sizeDelta = new Vector2 (text.preferredWidth, rect.sizeDelta.y);
    }

    private void SelectFirstButton (Transform parent) => parent.GetComponentInChildren<Button> ()?.Select ();

    private bool cancelHeld = true;
    private bool openHeld = true;
    private void Update ()
    {
        //print($"open: {!open}, openHeld: {!openHeld}, jump: {Input.GetAxis("Jump") > 0}");
        if (!open && !openHeld && Input.GetAxis ("Jump") > 0)
        {
            print("open");
            openHeld = true;
            Open ();
        }
        else if (Input.GetAxis ("Jump") <= 0)
            openHeld = false;

        if (open && !cancelHeld && Input.GetAxis ("Cancel") > 0)
        {
            cancelHeld = true;
            //PreviousPanel ();
        }
        else if (Input.GetAxis ("Cancel") <= 0)
            cancelHeld = false;
    }
}
