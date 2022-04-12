using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class CustomLineView : DialogueViewBase
{
    Effects.CoroutineInterruptToken currentStopToken = new Effects.CoroutineInterruptToken();
    private string _showText;
    private int _currentIndex;

    public bool autoAdvance;
    public TextMeshProUGUI lineText;

    public float typewriterSpeed;
    
    private void OnEnable()
    {
        _currentIndex = 0;
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        StopAllCoroutines();

        StartCoroutine(RunLineInternal(dialogueLine, onDialogueLineFinished));
    }

    private IEnumerator RunLineInternal(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        IEnumerator PresentLine()
        {
            lineText.text += dialogueLine.Text.Text;
            lineText.maxVisibleCharacters = _currentIndex;
            
            yield return StartCoroutine(Typewriter(_currentIndex, lineText, typewriterSpeed, null, currentStopToken));
            if(currentStopToken.WasInterrupted) yield break;
        }

        _currentIndex = lineText.text.Length;
        yield return StartCoroutine(PresentLine());
        currentStopToken.Complete();
        lineText.maxVisibleCharacters = int.MaxValue;

        //if (autoAdvance == false) yield break;
        
        onDialogueLineFinished();
    }

    private IEnumerator Typewriter(int startIndex, TextMeshProUGUI text, float lettersPerSecond, Action onCharacterTyped, Effects.CoroutineInterruptToken stopToken = null)
    {
        stopToken?.Start();

        // Start with everything invisible
        text.maxVisibleCharacters = startIndex;

        // Wait a single frame to let the text component process its
        // content, otherwise text.textInfo.characterCount won't be
        // accurate
        yield return null;

        // How many visible characters are present in the text?
        var characterCount = text.textInfo.characterCount;

        // Early out if letter speed is zero, text length is zero
        if (lettersPerSecond <= 0 || characterCount == 0)
        {
            // Show everything and return
            text.maxVisibleCharacters = characterCount;
            stopToken.Complete();
            yield break;
        }

        // Convert 'letters per second' into its inverse
        float secondsPerLetter = 1.0f / lettersPerSecond;

        // If lettersPerSecond is larger than the average framerate, we
        // need to show more than one letter per frame, so simply
        // adding 1 letter every secondsPerLetter won't be good enough
        // (we'd cap out at 1 letter per frame, which could be slower
        // than the user requested.)
        //
        // Instead, we'll accumulate time every frame, and display as
        // many letters in that frame as we need to in order to achieve
        // the requested speed.
        var accumulator = Time.deltaTime;

        while (text.maxVisibleCharacters < characterCount)
        {
            if (stopToken?.WasInterrupted ?? false) {
                yield break;
            }

            // We need to show as many letters as we have accumulated
            // time for.
            while (accumulator >= secondsPerLetter)
            {
                text.maxVisibleCharacters += 1;
                onCharacterTyped?.Invoke();
                accumulator -= secondsPerLetter;
            }
            accumulator += Time.deltaTime;

            yield return null;
        }

        // We either finished displaying everything, or were
        // interrupted. Either way, display everything now.
        text.maxVisibleCharacters = characterCount;

        stopToken?.Complete();
    }
    
    public void OnContinueClicked()
    {
        // When the Continue button is clicked, we'll do the same thing as
        // if we'd received a signal from any other part of the game (for
        // example, if a DialogueAdvanceInput had signalled us.)
        UserRequestedViewAdvancement();
    }
}
