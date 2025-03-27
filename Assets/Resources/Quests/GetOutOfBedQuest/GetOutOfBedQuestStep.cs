using UnityEngine;

public class GetOutOfBedQuestStep : QuestStep
{
    public bool test = true;

    private void Start()
    {
        string status;
        if (test) status = "Get out of the bedroom";
        else status = "Go straigh ahead";
        

        ChangeState("", status);
    }

    protected override void SetQuestStepState(string state)
    {

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            string status = "Got out of the bedroom";
            ChangeState("", status);
            FinishQuestStep();
        }

    }

}
