using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueChoiceButton : MonoBehaviour, ISelectHandler {
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI choiceText;

    private int choiceIndex = -1;

    public void SetChoiceText(string choiceTextString)
    {
        choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }

    public void SelectButton()
    {
        button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        EventManager.instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }
}