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

    public GameObject button, phraseText, phraseBlank, addBlankButton, historyButton;
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
            PlaceButton(phrase, phraseSelectContainer, () => OnPhraseSelected(phrase));
        for (int c = 0; c < categories.Length; c++)
        {
            foreach (string word in categories[c])
                PlaceButton (word, categoryContainers[c], () => OnWordSelected (word));
        }

        if (EventManager.I != null)
            EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }

    private void OnPlayersSpawned (GameObject human, GameObject dog)
    {
        humanBubbles = human.transform.parent.GetComponentInChildren<BubbleManager> ();
        dogBubbles = dog.transform.parent.GetComponentInChildren<BubbleManager> ();
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

    private void OnWordSelected (string word)
    {
        PhraseBlank.ActiveBlank.SetWord(word);
    }

    private void OnAddButtonClicked(Transform addButton)
    {
        PlacePhraseBlank(addButton.GetSiblingIndex (), true);
        PlaceAddButtons();
    }

    private void Open()
    {
        UI_Manager.SetIsOpen(true, "Comm");
        Player_StaticActions.DisableDogMovement();
        Player_StaticActions.DisableHumanMovement();
        SetPanel(phraseSelectPanel);
        UIContainer.gameObject.SetActive(true);
        open = true;
    }

    private void Close()
    {
        UI_Manager.SetIsOpen(false, "Comm");
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

            PlacePhraseBlank (phraseContainer.childCount, firstBlank);
            firstBlank = false;
            
            last = Mathf.Min (next + 1, phrase.Length - 1);
        }

        PlaceAddButtons();
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

    private void PlacePhraseBlank (int placeIndex, bool select = false)
    {
        GameObject blank = Instantiate (phraseBlank, phraseContainer);
        blank.transform.SetSiblingIndex(placeIndex);
        Button button = blank.GetComponent<PhraseBlank> ().wordButton;
        //button.onClick.AddListener (() => OnBlankSelected (placeIndex));
        if (select)
            blank.GetComponent<PhraseBlank> ().Activate ();
    }
    
    private void PlaceAddButtons()
    {
        //foreach (Transform child in phraseContainer.GetComponentsInChildren<Transform>())
        //    if (child.name.Contains("AddBlankButton")) Destroy(child.gameObject);

        for (int c = 0; c < phraseContainer.childCount; c++)
        {
            //bool spawnLast = c == phraseContainer.childCount - 1 && phraseContainer.GetChild(c).name.Contains("Blank");
            //print(spawnLast);
            Transform child = phraseContainer.GetChild(c);

            if (child.name.Contains("PhraseBlank"))
            {
                Transform last = c - 1 < 0 ? null : phraseContainer.GetChild(c - 1);
                Transform next = c + 1 >= phraseContainer.childCount ? null : phraseContainer.GetChild(c + 1);
                print($"child: {c} ({child.name})");
                print($"last: {(last == null ? "null" : last.name)} ({last == null || !last.name.Contains("AddBlankButton")})");
                print($"next: {(next == null ? "null" : next.name)} ({next == null || !next.name.Contains("AddBlankButton")})");
                if (last == null || !last.name.Contains("AddBlankButton"))
                {
                    Button addButton = Instantiate(addBlankButton, phraseContainer).GetComponentInChildren<Button>();
                    addButton.onClick.AddListener(() => OnAddButtonClicked(addButton.transform.parent));
                    addButton.transform.parent.SetSiblingIndex(c);
                    c++;
                }
                if (next == null || !next.name.Contains("AddBlankButton"))
                {
                    Button addButton = Instantiate(addBlankButton, phraseContainer).GetComponentInChildren<Button>();
                    addButton.onClick.AddListener(() => OnAddButtonClicked(addButton.transform.parent));
                    addButton.transform.parent.SetSiblingIndex(c + 1);
                }
            }
        }
    }

    public void SubmitPhrase ()
    {
        StringBuilder sb = new StringBuilder (50);
        foreach (TextMeshProUGUI text in phraseContainer.GetComponentsInChildren<TextMeshProUGUI> ())
        {
            if (text.text == "____") continue;

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

    private bool cancelHeld = true;
    private bool openHeld = true;
    private void Update ()
    {
        if (!Player_StaticActions.humanMovementAllowed && PlayerManager.ThisPlayer == PlayerSpecies.Human) return; 
        if (!Player_StaticActions.dogMovementAllowed && PlayerManager.ThisPlayer == PlayerSpecies.Dog) return; 
        if (UI_Manager._isUIOpen && UI_Manager._currentMenu != "Comm") return;
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Open();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    
        //print($"open: {!open}, openHeld: {!openHeld}, jump: {Input.GetAxis("Jump") > 0}");
        // if (!open && !openHeld && Input.GetAxis ("Jump") > 0)
        // {
        //     print("open");
        //     openHeld = true;
        //     Open ();
        // }
        // else if (Input.GetAxis ("Jump") <= 0)
        //     openHeld = false;
        //
        // if (open && !cancelHeld && Input.GetAxis ("Cancel") > 0)
        // {
        //     cancelHeld = true;
        //     //PreviousPanel ();
        // }
        // else if (Input.GetAxis ("Cancel") <= 0)
        //     cancelHeld = false;
    }

    private void OnDisable()
    {
        EventManager.I.OnPlayersSpawned -= OnPlayersSpawned;
    }
}
