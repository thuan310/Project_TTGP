using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;
using static Unity.Collections.AllocatorManager;


public interface IInteractableObject
{
    string WordDisplayWhenInteract { get; set; }
    PlayerManager player { get; set; }

    public UnityEvent OnInteract { get; set; }

    public virtual void OnInteracted()
    { 
        OnInteract.Invoke();
        player.playerDetectArea.interactableObjectsArray.Remove(this);
    }
    public virtual void OnExitInteracted()
    {
        PlayerInputManager.instance.enabled = true;
        MinigameInputManager.instance.Quit();
    }
}
