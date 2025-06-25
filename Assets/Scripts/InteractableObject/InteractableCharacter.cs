using UnityEngine;
using UnityEngine.Events;

public class InteractableCharacter : QuestStep, IInteractableObject
{
    public PlayerManager player { get; set; }

    [Header("Word Display When Interact")]
    public string wordDisplayWhenInteract;
    public string WordDisplayWhenInteract { get => wordDisplayWhenInteract; set => wordDisplayWhenInteract = value; }

    [Header("Action to do When Interact")]
    public UnityEvent onInteract;
    public UnityEvent OnInteract { get => onInteract; set => onInteract = value; }

    public void OnInteracted()
    {
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        player.action.Value = PLayerAction.PlayingDialogue;
        EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        OnInteract.Invoke();
    }

    protected override void SetQuestStepState(string state)
    {

    }
}
