using UnityEngine;

public class TitleScreenLoadMenuInputManager : MonoBehaviour
{
    PlayerControl playerControls;

    [Header("Title Screen Input")]
    [SerializeField] bool deleteCharacterSlot = false;

    private void Update()
    {
        if(deleteCharacterSlot)
        {
            deleteCharacterSlot = false;
            TitleScreenManager.instance.AttemptToDeleteCharacterSlot();
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControl();
            playerControls.UI.X.performed += i => deleteCharacterSlot = true;
        }

        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

}
