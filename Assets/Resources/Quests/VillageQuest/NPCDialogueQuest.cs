using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NPCDialogueQuest : MonoBehaviour, IInteractableObject {
    public PlayerManager player { get; set; }
    public AICharacterManager aiCharacterManager;

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

    public void Start()
    {
        aiCharacterManager = GetComponent<AICharacterManager>();
    }
    private void EnableNPC(int id)
    {
        if (npcID == id)
        {
            isNPCActivated = false;
        }
    }

    public void Update()
    {
        if (player != null)
        {
            //print("asjc");
            if (player.action.Value == PLayerAction.PlayingDialogue)
            {
                aiCharacterManager.isTalking = true;
                aiCharacterManager.aiCharacterCombatManager.currentTarget = player;
            }
            if (player.action.Value == PLayerAction.Normal)
            {
                aiCharacterManager.isTalking = false;
            }
        }
    }

    public void OnInteracted()
    {
        Debug.Log("Tesst");
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        player.action.Value = PLayerAction.PlayingDialogue;
        EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        OnInteract.Invoke();
        if (isNPCActivated && isFirstTalked) 
        {
            isFirstTalked = false;
            Debug.Log("Tesst2");

            EventManager.instance.talkToNPCEvents.TalkToNPC(npcID);
        }
    }

}
