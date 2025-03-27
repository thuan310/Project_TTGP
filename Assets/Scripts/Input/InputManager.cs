using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a proxy for the PlayerInput component
// such that the input events the game needs to proces will 
// be sent through the GameEventManager. This lets any other
// script in the project easily subscribe to an input action
// without having to deal with the PlayerInput component directly.

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour {
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed || context.started)
        {
            EventManager.instance.inputEvents.MovePressed(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            EventManager.instance.inputEvents.MovePressed(Vector2.zero);
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            EventManager.instance.inputEvents.InteractPressed();
        }
    }

    public void ToggleQuestPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            EventManager.instance.inputEvents.ToggleQuestPressed();
        }
    }


}