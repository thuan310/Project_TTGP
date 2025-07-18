using UnityEngine;

public class TrainingQuestStep : QuestStep
{
    public string status;

    private void Start()
    {
        ChangeState("", status);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void SetQuestStepState(string state)
    {

    }
}
