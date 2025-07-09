using UnityEngine;

public class DialogueTriggerByStepIn : MonoBehaviour
{

    [Header("Dialogue (optional)")]
    [SerializeField] private string dialogueKnotName;

    private bool isTriggered = false;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            Debug.Log("test");
            EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
            isTriggered = true;           
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }
}
