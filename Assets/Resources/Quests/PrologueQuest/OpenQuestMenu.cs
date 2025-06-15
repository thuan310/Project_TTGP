using UnityEngine;
using UnityEngine.Events;

public class OpenQuestMenu : QuestStep , IInteractableObject
{
    public string wordDisplayWhenInteract;
    public string WordDisplayWhenInteract { get => wordDisplayWhenInteract; set => wordDisplayWhenInteract = value; }
    public PlayerManager player { get; set; }


    public UnityEvent onInteract;
    public UnityEvent OnInteract { get => onInteract; set => onInteract = value; }

    public void OnInteracted()
    {
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        player.action.Value = PLayerAction.PlayingDialogue;
        EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        BookMenuCanvas.instance.TriggerOpenBookMenu();
        OnInteract.Invoke();
    }

    public string status;


    private void Start()
    {
        ChangeState("", status);
    }



    protected override void SetQuestStepState(string state)
    {

    }
}
