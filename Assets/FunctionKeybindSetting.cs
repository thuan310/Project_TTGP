using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FunctionKeybindSetting : MonoBehaviour
{
    [Header("Name")]
    public string nameFunction;
    public TextMeshProUGUI nameButtonText;

    [Header("Option Function")]
    public TextMeshProUGUI keyBlindText;

    private PlayerControl playerPauseMenuInput;
    private InputAction anyKeyAction;

    void Awake()
    {
        playerPauseMenuInput = new PlayerControl(); // Auto-generated C# class from Input Actions asset
        anyKeyAction = playerPauseMenuInput.PauseMenu.AnyKey; // Example action
        anyKeyAction.Enable();
    }

    void OnAnyKeyPressed(InputAction.CallbackContext context)
    {
        if (Input.anyKeyDown)
        {
            // assign name
            keyBlindText.text = Input.inputString;
        }
        //Debug.Log("Button pressed: " + Input.inputString);
        // unsubcribe event
        anyKeyAction.performed -= OnAnyKeyPressed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameButtonText.text = nameFunction;

    }

    public void ChooseKey()
    {
        anyKeyAction.performed += OnAnyKeyPressed;
    }
}
