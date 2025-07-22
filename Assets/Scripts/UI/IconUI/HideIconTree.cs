using UnityEngine;

public class HideIconTree : MonoBehaviour
{
    public bool isActiveAtStart = true;
    public string id = "";
    public GameObject icon;
    private void OnEnable()
    {
        EventManager.instance.chopWoodMinigameEvents.onDisableIcon += DisableIcon;
        EventManager.instance.chopWoodMinigameEvents.onActivateIcon += EnableIcon;
    }

    private void OnDisable()
    {

        EventManager.instance.chopWoodMinigameEvents.onDisableIcon -= DisableIcon;
        EventManager.instance.chopWoodMinigameEvents.onActivateIcon -= EnableIcon;

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
    private void DisableIcon(string id)
    {
        
        if (id == this.id)
        {
            icon.SetActive(false);
        }
        
    }
}
