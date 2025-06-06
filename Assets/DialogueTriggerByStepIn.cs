using UnityEngine;

public class DialogueTriggerByStepIn : MonoBehaviour
{

    [Header("Dialogue (optional)")]
    [SerializeField] private string dialogueKnotName;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {

            EventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }
}
