using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialoguePanelUI : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceButton[] choiceButtons;
    private void Awake()
    {
        contentParent.SetActive(false);
        ResetPanel();
    }

    private void OnEnable()
    {
        EventManager.instance.dialogueEvents.onDialogueStarted += OnDialogueStarted;
        EventManager.instance.dialogueEvents.onDialogueFinished += OnDialogueFinished;
        EventManager.instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;

    }

    private void OnDisable()
    {
        EventManager.instance.dialogueEvents.onDialogueStarted -= OnDialogueStarted;
        EventManager.instance.dialogueEvents.onDialogueFinished -= OnDialogueFinished;
        EventManager.instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
    }

    private void OnDialogueStarted()
    {
        contentParent.SetActive(true);

    }

    private void OnDialogueFinished()
    {
        contentParent.SetActive(false);
        ResetPanel();
    }

    private void DisplayDialogue(string dialogue, List<Choice> dialogueChoices)
    {
        dialogueText.text = dialogue;

        if (dialogueChoices.Count > choiceButtons.Length)
        {
            Debug.LogError("More choices than buttons");
        }


        foreach (DialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
        }

        // enable and set info for buttons depending on ink choice information
        int choiceButtonIndex = dialogueChoices.Count - 1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceButton choiceButton = choiceButtons[choiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if (inkChoiceIndex == 0)
            {
                choiceButton.SelectButton();
                EventManager.instance.dialogueEvents.UpdateChoiceIndex(inkChoiceIndex);
            }

            choiceButtonIndex--;
        }
    }

    private void ResetPanel()
    {
        dialogueText.text = "";
    }
}
