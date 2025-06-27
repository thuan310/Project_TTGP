using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleInput : MonoBehaviour
{
    private void Start()
    {
        playerInput.enabled = false;
    }
    [SerializeField] private PlayerInput playerInput;
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
        Debug.Log("1");

        PlayerInputManager.instance.enabled = false;
        playerInput.enabled = true;
    }

    private void DisableInput()
    {
        Debug.Log("0");

        PlayerInputManager.instance.enabled = true;
        playerInput.enabled = false;
    }
}
