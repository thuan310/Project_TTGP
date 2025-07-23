using UnityEngine;

public class VillageStartQuestStep : QuestStep 
{
    public string status;
    private bool playerIsNear = false;

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
            if (!dialogueKnotName.Equals(""))
            {
                EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
            }
        }
    }


}
