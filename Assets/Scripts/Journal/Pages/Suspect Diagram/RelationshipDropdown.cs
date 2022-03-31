using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Journal;
using Unity.VisualScripting;

public class RelationshipDropdown : MonoBehaviour
{
    public Relationships correctAnswer;
    public float fadeDuration;

    private bool _isCorrect;
    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(ValueChanged);

        SuspectDiagram.current.ONCorrectGroup += DisableButton;
        
        SetOptions();
    }

    private void SetOptions()
    {
        _dropdown.options.Clear();

        GameObject template = transform.Find("Template").gameObject;
        ScrollRect scrollRect = template.GetComponent<ScrollRect>();
        scrollRect.scrollSensitivity = 10f;
        scrollRect.verticalScrollbar = null;

        string[] optionNames = Enum.GetNames(typeof(Relationships));
        foreach (string optionName in optionNames)
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData {text = optionName});
        }
    }

    private void ValueChanged(int value)
    {
        string newAnswer = _dropdown.options[value].text;
        Relationships relationships = (Relationships) Enum.Parse(typeof(Relationships), newAnswer);

        bool wasCorrect = _isCorrect;
        _isCorrect = relationships == correctAnswer;
        
        if(wasCorrect && !_isCorrect) SuspectDiagram.current.WrongAnswer();
        else if(!wasCorrect && _isCorrect) SuspectDiagram.current.CorrectAnswer();
    }

    private void DisableButton()
    {
        if (!_isCorrect || !_dropdown.IsInteractable()) return;
        
        _dropdown.interactable = false;
        StartCoroutine(ImageFade_CO());
    }
    
    private IEnumerator ImageFade_CO()
    {
        float startTime = Time.time;
        Image image = _dropdown.GetComponent<Image>();
        Color color = image.color;
        
        while (Time.time < startTime + fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, (Time.time - startTime) / fadeDuration);
            image.color = color;
            yield return null;
        }

        color.a = 0;
        image.color = color;
    }
}
