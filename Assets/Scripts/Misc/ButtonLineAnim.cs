using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonLineAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Image image;

    private bool _isSelected;
    private bool _isFilling;
    
    // Update is called once per frame
    void Update()
    {
        if (_isSelected) image.fillAmount = 1;
        else
        {
            if (_isFilling) Mathf.Clamp(image.fillAmount += 5 * Time.deltaTime, 0, 1);
            else Mathf.Clamp(image.fillAmount -= 10 * Time.deltaTime, 0, 1);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isFilling = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isFilling = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _isSelected = true;
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        _isSelected = false;
    }
}
