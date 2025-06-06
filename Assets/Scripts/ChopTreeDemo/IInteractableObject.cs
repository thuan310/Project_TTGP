using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class IInteractableObject : MonoBehaviour
{
    public string wordDisplayWhenInteract;
    private PlayerManager player;
    public virtual void OnInteracted()
    { 
        PlayerInputManager.instance.enabled = false;
    }
    public virtual void OnExitInteracted()
    {
        PlayerInputManager.instance.enabled = true;
        MinigameInputManager.instance.Quit();
    }

    private void OnTriggerExit(Collider other)
    {
        player= other.GetComponentInParent<PlayerManager>();
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        if (player.playerDetectArea.interactableObjectsArray.Count <= 0)
        {
            player.playerDetectArea.interactableObject = null;
            PlayerUIManager.instance.playerUIDynamicHUDManager.SetInteractableUIWithAction(false,"");
        }

    }
}
