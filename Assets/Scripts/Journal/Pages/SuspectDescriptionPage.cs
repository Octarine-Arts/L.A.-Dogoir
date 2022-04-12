using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Journal.Pages
{
    public class SuspectDescriptionPage : MonoBehaviour
    {
        public GameObject suspectButtonGO;
        public Transform row1Parent;
        public Transform row2Parent;
        public Transform row3Parent;

        public TMP_Text fullNameText;
        public TMP_Text occupationText;
        public TMP_Text descriptionText;
        public TMP_Text personalityText;

        private List<Suspect> _suspectList;
        private List<GameObject> _suspectButtons;

        private void Awake()
        {
            _suspectList = new List<Suspect>();
            _suspectButtons = new List<GameObject>();

            //foreach (var suspect in Resources.LoadAll(Journal_Manager.current.suspectsPath))
            foreach (var suspect in Resources.LoadAll("Bar/Suspects"))
            {
                _suspectList.Add((Suspect)suspect);
            }
            CreateButtons();
        }

		private void OnEnable()
		{
            RefreshImages();
        }

        private void RefreshImages()
        {
            for (int ii = 0; ii < _suspectList.Count; ii++)
            {
                _suspectButtons[ii].SetActive(_suspectList[ii].hasTalked);
            }
        }

		private void CreateButtons()
        {
            for (int ii = 0; ii < _suspectList.Count; ii++)
            {
                GameObject instantiatedObject = null;
                var index = ii;

                switch (ii % 3)
                {
                    case 0:
                        instantiatedObject = Instantiate(suspectButtonGO, row1Parent);
                        break;
                    case 1:
                        instantiatedObject = Instantiate(suspectButtonGO, row2Parent);
                        break;
                    case 2:
                        instantiatedObject = Instantiate(suspectButtonGO, row3Parent);
                        break;
                }

                if (instantiatedObject == null) return;
                _suspectButtons.Add(instantiatedObject);
                GameObject button = instantiatedObject.transform.GetChild(0).gameObject;
                button.GetComponent<Image>().sprite = _suspectList[ii].displayImage;
                button.GetComponent<Button>().onClick.AddListener(delegate { ShowDetails(index); });
            }
            //RefreshImages();
        }

        private void ShowDetails(int index)
        {
            fullNameText.text = _suspectList[index].fullName;
            occupationText.text = _suspectList[index].occupation;
            descriptionText.text = _suspectList[index].description;
            personalityText.text = _suspectList[index].personality;
        }
    }
}
