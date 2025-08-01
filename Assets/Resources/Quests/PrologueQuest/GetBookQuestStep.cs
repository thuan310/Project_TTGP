using UnityEngine;
using UnityEngine.Events;

public class GetBookQuestStep : QuestStep, IInteractableObject
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

    private void OnTriggerEnter(Collider other)
    {
        TutorialManager.instance.LoadNextTutorials();
    }


}
