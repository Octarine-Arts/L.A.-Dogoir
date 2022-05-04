using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CustomOptionView : DialogueViewBase
{
    public CanvasGroup canvasGroup;
    public CanvasGroup dialogGroup;
    public OptionView optionViewPrefab;
    //public GameObject Scrollbar;
    public float fadeTime = 0.1f;
    
    private List<OptionView> _optionViews = new();
    private Action<int> _OnOptionSelected;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0;
        //Scrollbar.SetActive(false);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
    public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
    {
        StartCoroutine(Effects.FadeAlpha(dialogGroup, 1, 0, 0.2f));
        //Scrollbar.SetActive(false);

        // Hide all existing option views
        foreach (var optionView in _optionViews)
        {
            optionView.gameObject.SetActive(false);
        }

        // If we don't already have enough option views, create more
        while (dialogueOptions.Length > _optionViews.Count)
        {
            var optionView = CreateNewOptionView();
            optionView.gameObject.SetActive(false);
        }
        
        // Set up all of the option views
        int optionViewsCreated = 0;

        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            var optionView = _optionViews[i];
            var option = dialogueOptions[i];

            if (option.IsAvailable == false)
            {
                // Don't show this option.
                continue;
            }

            optionView.gameObject.SetActive(true);

            optionView.Option = option;

            // The first available option is selected by default
            if (optionViewsCreated == 0)
            {
                optionView.Select();
            }

            optionViewsCreated += 1;
        }
        
        // Note the delegate to call when an option is selected
        _OnOptionSelected = onOptionSelected;

        // Fade it all in
        StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeTime));
        //Scrollbar.SetActive(true);

        OptionView CreateNewOptionView()
        {
            var optionView = Instantiate(optionViewPrefab, transform, false);
            optionView.transform.SetAsLastSibling();

            optionView.OnOptionSelected = OptionViewWasSelected;
            _optionViews.Add(optionView);

            return optionView;
        }
        
        void OptionViewWasSelected(DialogueOption option)
        {
            StartCoroutine(Effects.FadeAlpha(dialogGroup, 0, 1, fadeTime));
            //Scrollbar.SetActive(true);
            StartCoroutine(OptionViewWasSelectedInternal(option));

            IEnumerator OptionViewWasSelectedInternal(DialogueOption selectedOption)
            {
                yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 1, 0, fadeTime));
                //Scrollbar.SetActive(false);
                _OnOptionSelected(selectedOption.DialogueOptionID);
            }
        }
    }
}
