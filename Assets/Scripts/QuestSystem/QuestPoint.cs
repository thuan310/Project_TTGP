
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public class QuestPoint : MonoBehaviour {

    [Header("Dialogue (optional)")]
    [SerializeField] private string dialogueKnotName;

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool endPoint = true;

    private bool playerIsNear = false;

    private string questID;
    private QuestState currentQuestState;

    //private QuestIcon questIcon;

    private void Awake()
    {
        questID = questInfoForPoint.id;
        //questIcon = GetComponentInChildren<QuestIcon>();


    }

    private void OnEnable()
    {
        EventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        EventManager.instance.inputEvents.onInteractPressed += SubmitPressed;
  

    }

    private void OnDisable()
    {
        EventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        EventManager.instance.inputEvents.onInteractPressed -= SubmitPressed;

    }

    private void SubmitPressed(InputEventContext context)
    {
        Debug.Log("Interacted");

        if (!playerIsNear || !context.Equals(InputEventContext.DEFAULT))
        {
            Debug.Log("Player not near or context no default");
            return;
        }

        if (!dialogueKnotName.Equals(""))
        {
            Debug.Log("Enter Dialogue for " + dialogueKnotName);

            EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
        else
        {
            if (currentQuestState == QuestState.CAN_START && startPoint)
            {
                EventManager.instance.questEvents.StartQuest(questID);
            }
            else if (currentQuestState == QuestState.CAN_FINISH && endPoint)
            {
                EventManager.instance.questEvents.FinishQuest(questID);
            }
        }


    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questID))
        {
            currentQuestState = quest.state;
            //questIcon.SetState(currentQuestState, startPoint, endPoint);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = false;

        }
    }
}
