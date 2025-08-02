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

    [SerializeField] private float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;

    [SerializeField] private AudioClip dialogueTypingSound;
    private AudioSource audioSource;
    [SerializeField] private bool stopAudioSource;

    [Range(1,5)]
    [SerializeField] private int frequency = 2;

    [Range(-3,3)]
    [SerializeField] private float minPitch = 0.5f;
    [Range(-3, 3)]
    [SerializeField] private float maxPitch = 2f;


    private void Awake()
    {
        contentParent.SetActive(false);
        ResetPanel();

        audioSource = this.gameObject.AddComponent<AudioSource>();
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
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeDialogue(dialogue));

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

    private IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        int currentCharacterCount = 0;

        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            
            PlayDialogueSound(currentCharacterCount);
            currentCharacterCount++;

            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void PlayDialogueSound(int currentCharacterCount)
    {
        if (currentCharacterCount % frequency == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(dialogueTypingSound);
        }
    }
    private void ResetPanel()
    {
        dialogueText.text = "";
    }
}
