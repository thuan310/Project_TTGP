using UnityEngine;

public abstract class QuestStep : MonoBehaviour {

    [Header("Dialogue (optional)")]
    [SerializeField] protected string dialogueKnotName;

    private bool isFinished = false;

    private string questId;

    private int stepIndex;

    virtual protected void OnEnable()
    {
        EventManager.instance.questEvents.onFinishQuestStep += QuestEvents_onFinishQuestStep;
    }
    virtual protected void OnDisable()
    {
        EventManager.instance.questEvents.onFinishQuestStep -= QuestEvents_onFinishQuestStep;

    }

    private void QuestEvents_onFinishQuestStep(string id)
    {

        if (id == questId)
        {
            FinishQuestStep();
        }
    }

    public void InitializeQuestStep(string questID, int stepIndex, string questStepState)
    {
        this.questId = questID;
        this.stepIndex = stepIndex;
        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            EventManager.instance.questEvents.AdvanceQuest(questId);

            Destroy(this.gameObject);
        }
    }

    protected void ChangeState(string newState, string newStatus)
    {
        EventManager.instance.questEvents.QuestStepStateChange(questId, stepIndex, new QuestStepState(newState, newStatus));
    }

    protected abstract void SetQuestStepState(string state);
}