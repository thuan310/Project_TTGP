using UnityEngine;

public class VillageEndQuestStep : QuestStep
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


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            FinishQuestStep();
            //
            SceneNavigationManager.instance.LoadNextScene();
        }
    }


}
