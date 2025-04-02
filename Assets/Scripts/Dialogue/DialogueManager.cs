
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    [Header("Ink story")]
    [SerializeField] private TextAsset inkJson;

    private Story story;
    private int currentChoiceIndex = -1;

    private bool dialoguePlaying = false;

    private InkExternalFunctions inkExternalFunctions;
    private InkDialogueVariables inkDialogueVariables;

    private void Awake()
    {
        story = new Story(inkJson.text);
        inkExternalFunctions = new InkExternalFunctions();
        inkExternalFunctions.Bind(story);

        inkDialogueVariables = new InkDialogueVariables(story);
    }

    private void OnDestroy()
    {
        inkExternalFunctions.Unbind(story);
    }

    private void OnEnable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        EventManager.instance.inputEvents.onInteractPressed += SubmitPressed;
        EventManager.instance.dialogueEvents.onUpdateChoiceIndex += UpdateChoiceIndex;
        EventManager.instance.dialogueEvents.onUpdateInkDialogueVariable += UpdateInkDialogueVariable;
        EventManager.instance.questEvents.onQuestStateChange += QuestStateChange;

    }
    private void OnDisable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
        EventManager.instance.inputEvents.onInteractPressed -= SubmitPressed;
        EventManager.instance.dialogueEvents.onUpdateChoiceIndex -= UpdateChoiceIndex;
        EventManager.instance.dialogueEvents.onUpdateInkDialogueVariable -= UpdateInkDialogueVariable;
        EventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;


    }
    private void QuestStateChange(Quest quest)
    {
        EventManager.instance.dialogueEvents.UpdateInkDialogueVariable(
                quest.info.id + "State",
                (Ink.Runtime.Object)new StringValue(quest.state.ToString())
            );

    }

    private void UpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
    {
        inkDialogueVariables.UpdateVariableState(name, value);
    }

    private void UpdateChoiceIndex(int index)
    {
        currentChoiceIndex = index;
    }

    private void SubmitPressed(InputEventContext context)
    {
        if (!context.Equals(InputEventContext.DIALOGUE))
        {
            return;
        }

        ContinueOrExitStory();
    }
    private void EnterDialogue(string knotName)
    {
        if (dialoguePlaying)
        {
            return;
        }
        dialoguePlaying = true;


        EventManager.instance.dialogueEvents.DialogueStarted();
        //EventManager.instance.playerEvents.DisablePlayerMovement();
        EventManager.instance.inputEvents.ChangeInputEventContext(InputEventContext.DIALOGUE);

        if (!knotName.Equals(""))
        {
            story.ChoosePathString(knotName);
        }
        else
        {
            Debug.LogWarning("Know name empty");
        }

        inkDialogueVariables.SyncVariablesAndStartListening(story);

        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            currentChoiceIndex = -1;
        }

        if (story.canContinue)
        {
            string dialogueLine = story.Continue();

            while (IsLineBlank(dialogueLine) && story.canContinue)
            {
                dialogueLine = story.Continue();
            }
            // handle the case where the last line of dialogue is blank
            // (empty choice, external function, etc...)
            if (IsLineBlank(dialogueLine) && !story.canContinue)
            {
                ExitDialogue();
            }
            else
            {
                EventManager.instance.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices);
            }
        }
        else if (story.currentChoices.Count == 0)
        {
            ExitDialogue();
        }
    }


    private void ExitDialogue()
    {
        dialoguePlaying = false;

        EventManager.instance.dialogueEvents.DialogueFinished();
        EventManager.instance.inputEvents.ChangeInputEventContext(InputEventContext.DEFAULT);
        //EventManager.instance.playerEvents.EnablePlayerMovement();

        inkDialogueVariables.StopListening(story);

        story.ResetState();


    }

    private bool IsLineBlank(string dialogueLine)
    {
        return dialogueLine.Trim().Equals("") || dialogueLine.Trim().Equals("\n");
    }
}
