using TMPro;
using UnityEngine;

public class PlayerUIDynamicHUDManager : MonoBehaviour
{
    public GameObject treeChopMinigame_UI;
    public GameObject carryLogMinigame_UI;
    public GameObject logSharpeningMinigame_UI;
    public GameObject interacted_UI;
    private void Start()
    {
        treeChopMinigame_UI.SetActive(false);
        interacted_UI.SetActive(false);
        logSharpeningMinigame_UI.SetActive(false);
    }

    public void ControlUI()
    {
        switch(PlayerInputManager.instance.action)
        {
            case PlayerInputManager.Action.Normal:
                treeChopMinigame_UI.gameObject.SetActive(false);
                carryLogMinigame_UI.SetActive(false);
                logSharpeningMinigame_UI.SetActive(false);
                return;
            case PlayerInputManager.Action.ChopTree:
                interacted_UI.SetActive(false);
                treeChopMinigame_UI.gameObject.SetActive(true);
                return;
            case PlayerInputManager.Action.CarrySomething:
                interacted_UI.SetActive(false);
                carryLogMinigame_UI.SetActive(true);
                return;
            case PlayerInputManager.Action.LogSharpening:
                interacted_UI.SetActive(false);
                logSharpeningMinigame_UI.SetActive(true);
                return;

        }
    }
    public void SetInteractableUI(bool flag)
    {
        interacted_UI.SetActive(flag);
    }

}
