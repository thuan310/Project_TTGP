using UnityEngine;

public class BookMenuCanvas : MonoBehaviour
{
    public static BookMenuCanvas instance;

    public PlayerManager player;

    public GameObject UI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        player = FindFirstObjectByType<PlayerManager>();
    }

    public void TriggerOpenBookMenu()
    {
        UI.SetActive(true);
    }

    public void TriggerCloseBookMenu()
    {
        UI.SetActive(false);
    }

    public void ContinueStory()
    {
        UI.SetActive(false);
        player.action.Value = PLayerAction.PlayingDialogue;
        EventManager.instance.dialogueEvents.EnterDialogue("openQuestMenu");
    }
}
