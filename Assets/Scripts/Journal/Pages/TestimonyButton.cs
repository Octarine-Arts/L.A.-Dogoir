using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Journal.Pages
{
    public class TestimonyButton : MonoBehaviour
    {
        public TMP_Text testimonyTitle;

        private string _testimonyDescription;

        public void SetupButton(string title, string description)
        {
            testimonyTitle.text = title;
            _testimonyDescription = description;
            
            GetComponent<Button>().onClick.AddListener(ShowTestimonyDescription);
        }

        private void ShowTestimonyDescription()
        {
            Debug.Log(_testimonyDescription);
        }
    }
}