using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    CanvasGroup group;
    int quantityValue;
    [SerializeField] TMP_Text quantityTxt;
    [SerializeField] GameObject quantitySticker;

    bool isEmpty;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        switch(isEmpty)
        {
            case true:
                group.alpha = 0.45f;
                quantitySticker.SetActive(false);
                break;
            case false:
                group.alpha = 1;
                quantitySticker.SetActive(true);
                break;
        }
    }

    public void AddElement()
    {
        if (quantityTxt == null) return;
        quantityValue++;
        UpdateQuantityText();
    }
    public void RemoveElement()
    {
        if (quantityTxt == null) return;
        quantityValue++;
        UpdateQuantityText();
    }

    private void UpdateQuantityText()
    {
        if (quantityTxt == null) return;
        quantityTxt.text = quantityValue.ToString();
    }

}
