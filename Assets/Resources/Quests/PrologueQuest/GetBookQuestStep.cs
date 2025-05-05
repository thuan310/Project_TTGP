using UnityEngine;

public class GetBookQuestStep : QuestStep
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
        EventManager.instance.inputEvents.onInteractPressed += SubmitPressed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.instance.inputEvents.onInteractPressed -= SubmitPressed;

    }

    protected override void SetQuestStepState(string state)
    {

    }

    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        string status = "Got Book";
    //        ChangeState("", status);
    //        FinishQuestStep();
    //    }

    //}

    private void SubmitPressed(InputEventContext context)
    {
        if (!playerIsNear || !context.Equals(InputEventContext.DEFAULT))
        {
            Debug.Log("Player not near or context no default");
            return;
        }

        if (!dialogueKnotName.Equals(""))
        {
            EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }

        string status = "Got Book";
        ChangeState("", status);
        FinishQuestStep();
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
