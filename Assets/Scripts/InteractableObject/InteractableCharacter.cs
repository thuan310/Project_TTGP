using UnityEngine;
using UnityEngine.Events;

public class InteractableCharacter : QuestStep, IInteractableObject
{
    public PlayerManager player { get; set; }
    public AICharacterManager aiCharacterManager;

    [Header("Word Display When Interact")]
    public string wordDisplayWhenInteract;
    public string WordDisplayWhenInteract { get => wordDisplayWhenInteract; set => wordDisplayWhenInteract = value; }

    [Header("Action to do When Interact")]
    public UnityEvent onInteract;
    public UnityEvent OnInteract { get => onInteract; set => onInteract = value; }

    public void Start()
    {
        aiCharacterManager = GetComponent<AICharacterManager>();
    }

    public void OnInteracted()
    {
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        player.action.Value = PLayerAction.PlayingDialogue;
        EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        OnInteract.Invoke();
    }

    public void Update()
    {
        if (player != null)
        {
            //print("asjc");
            if (player.action.Value == PLayerAction.PlayingDialogue)
            {
                aiCharacterManager.isTalking = true;                        // se thay doi hanh dong hay state cua Ai thanh noi chuyen
                aiCharacterManager.aiCharacterCombatManager.currentTarget = player; // them vao de xu ly viec AI se huong ve ng choi khi noi chuyen
            }
            if (player.action.Value == PLayerAction.Normal)
            {
                aiCharacterManager.isTalking = false; // dang tinh la se ko nchuyen nua khi ng choi ket thuc cuoc hoi thoai nhuwng ma no lai anh huong den ca truoc khi bat dau noi chuyen
            }
        }
    }

    protected override void SetQuestStepState(string state)
    {

    }
}
