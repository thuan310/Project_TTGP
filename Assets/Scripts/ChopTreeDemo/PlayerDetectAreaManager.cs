using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectAreaManager : MonoBehaviour
{
    public PlayerManager player;
    public List<IInteractableObject> interactableObjectsArray = new List<IInteractableObject>();
    public IInteractableObject interactableObject;

    private void OnTriggerEnter(Collider other)
    {
        //Find objects in Hit area
        if (player.isInteracting)
            return;

         IInteractableObject randomthi = other.gameObject.GetComponent<IInteractableObject>();

        if (randomthi == null)
            return;
        if(interactableObjectsArray.Contains(randomthi)) 
            return;

        interactableObjectsArray.Add(randomthi);
        other.GetComponent<IInteractableObject>().player = player;

    }

    private void OnTriggerExit(Collider other)
    {
        interactableObjectsArray.Remove(other.GetComponent<IInteractableObject>());
    }
    private void Update()
    {
        //print(colliderArray.Length);
        if(interactableObjectsArray.Count > 0)
        {
            interactableObject = interactableObjectsArray[0];
            PlayerUIManager.instance.playerUIDynamicHUDManager.SetInteractableUIWithAction(true, interactableObject.WordDisplayWhenInteract);
        }
        else
        {
            interactableObject = null;
            PlayerUIManager.instance.playerUIDynamicHUDManager.SetInteractableUIWithAction(false, "");
        }
    }
}
