using UnityEngine;
using UnityEngine.Events;

public class GetSwordQuestStep : QuestStep, IInteractableObject {
    public string wordDisplayWhenInteract;
    public string WordDisplayWhenInteract { get => wordDisplayWhenInteract; set => wordDisplayWhenInteract = value; }
    public PlayerManager player { get; set; }


    public UnityEvent onInteract;
    public UnityEvent OnInteract { get => onInteract; set => onInteract = value; }

    public void OnInteracted()
    {
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        OnInteract.Invoke();
        FinishQuestStep();
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
