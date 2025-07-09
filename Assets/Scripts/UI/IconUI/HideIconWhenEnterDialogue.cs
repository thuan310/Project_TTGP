using UnityEngine;

public class HideIconWhenEnterDialogue : MonoBehaviour
{
    public bool isActiveAtStart = true;
    public bool isSeperateIcon = true;
    public string dialogueName = "";
    public GameObject icon;
    private void OnEnable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue += DisableIcon;
        EventManager.instance.talkToNPCEvents.onActivateIcon += EnableIcon;
    }

    private void OnDisable()
    {

        EventManager.instance.dialogueEvents.onEnterDialogue -= DisableIcon;
        EventManager.instance.talkToNPCEvents.onActivateIcon -= EnableIcon;

    }

    private void Start()
    {
        if (!isActiveAtStart)
        {
            icon.SetActive(false);
        }
    }

    private void EnableIcon()
    {

        icon.SetActive(true);
    }
    private void DisableIcon(string dialogue)
    {
        if (!isSeperateIcon)
        {
            if (dialogue == dialogueName)
            {
                icon.SetActive(false);
            }
        }
        else
        {
            icon.SetActive(false);
        }
    }
}
