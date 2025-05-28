using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [Header("Dialogue (optional)")]
    [SerializeField] private string dialogueKnotName;

    private bool playerIsNear = false;

    private void OnEnable()
    {
        EventManager.instance.inputEvents.onInteractPressed += SubmitPressed;
    }

    private void OnDisable()
    {
        EventManager.instance.inputEvents.onInteractPressed -= SubmitPressed;
    }

    private void SubmitPressed(InputEventContext context)
    {

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
