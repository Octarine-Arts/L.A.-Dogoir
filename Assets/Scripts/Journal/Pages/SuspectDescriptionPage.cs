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

        public TMP_Text fullNameText;
        public TMP_Text occupationText;
        public TMP_Text descriptionText;
        public TMP_Text personalityText;

        private List<Suspect> _suspectList;

        private void Awake()
        {
            _suspectList = new List<Suspect>();

            foreach (var suspect in Resources.LoadAll(Journal_Manager.current.suspectsPath))
            {
                _suspectList.Add((Suspect)suspect);
            }
            CreateButtons();
        }

        private void CreateButtons()
        {
            for (int ii = 0; ii < _suspectList.Count; ii++)
            {
                GameObject instantiatedObject = null;
                var index = ii;

                switch (ii % 2)
                {
                    case 0:
                        instantiatedObject = Instantiate(suspectButtonGO, row1Parent);
                        break;
                    case 1:
                        instantiatedObject = Instantiate(suspectButtonGO, row2Parent);
                        break;
                }

                if (instantiatedObject == null) return;
                instantiatedObject.GetComponent<Image>().sprite = _suspectList[ii].displayImage;
                instantiatedObject.GetComponent<Button>().onClick.AddListener(delegate { ShowDetails(index); });
            }
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
