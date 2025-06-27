using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NPCDialogueQuest : MonoBehaviour, IInteractableObject {
    public PlayerManager player { get; set; }

    [Header("Word Display When Interact")]
    public string wordDisplayWhenInteract;
    public string WordDisplayWhenInteract { get => wordDisplayWhenInteract; set => wordDisplayWhenInteract = value; }

    [Header("Action to do When Interact")]
    public UnityEvent onInteract;
    public UnityEvent OnInteract { get => onInteract; set => onInteract = value; }

    [Header("Dialogue (optional)")]
    [SerializeField] protected string dialogueKnotName;

    private bool isNPCActivated = true;
    private bool isFirstTalked = true;

    [Header("NPC ID (1, 2, 3 only)")]
    [Range(1, 3)]
    [SerializeField] protected int npcID;

    private void OnEnable()
    {
        EventManager.instance.talkToNPCEvents.onActivateNPC += EnableNPC;
    }
    private void OnDisable()
    {
        EventManager.instance.talkToNPCEvents.onActivateNPC -= EnableNPC;

    }

    private void EnableNPC(int id)
    {
        if (npcID == id)
        {
            isNPCActivated = false;
        }
    }

    public void OnInteracted()
    {
        Debug.Log("Tesst");
        if(isNPCActivated && isFirstTalked) 
        {
            isFirstTalked = false;

            player.playerDetectArea.interactableObjectsArray.Remove(this);
            player.action.Value = PLayerAction.PlayingDialogue;
            EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
            EventManager.instance.talkToNPCEvents.TalkToNPC(npcID);
            OnInteract.Invoke();
        }

    }

}
