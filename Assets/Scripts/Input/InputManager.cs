using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a proxy for the PlayerInput component
// such that the input events the game needs to proces will 
// be sent through the GameEventManager. This lets any other
// script in the project easily subscribe to an input action
// without having to deal with the PlayerInput component directly.

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour {

    [SerializeField] private PlayerInput playerInput;

    private void Start()
    {
        playerInput.enabled = false;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        EventManager.instance.dialogueEvents.onDialogueStarted += EnableInput;
        EventManager.instance.dialogueEvents.onDialogueFinished += DisableInput;
    }

    private void OnDisable()
    {

        EventManager.instance.dialogueEvents.onDialogueStarted -= EnableInput;
        EventManager.instance.dialogueEvents.onDialogueFinished -= DisableInput;
    }

    private void EnableInput()
    {

        PlayerInputManager.instance.enabled = false;
        playerInput.enabled = true;
    }

    private void DisableInput()
    {

        PlayerInputManager.instance.enabled = true;
        playerInput.enabled = false;
    }


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

    public void DodgePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EventManager.instance.inputEvents.DodgePressed(true);
        }
    }

    public void SprintPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EventManager.instance.inputEvents.SprintPressed(true);
        }

        if (context.canceled)
        {
            EventManager.instance.inputEvents.SprintPressed(true);
        }
    }

}