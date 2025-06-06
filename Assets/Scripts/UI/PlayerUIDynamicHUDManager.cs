using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIDynamicHUDManager : MonoBehaviour
{
    public GameObject treeChopMinigame_UI;
    public GameObject carryLogMinigame_UI;
    public GameObject logSharpeningMinigame_UI;
    public GameObject interacted_UI;
    public TextMeshProUGUI actionToDo;
    private void Start()
    {
        treeChopMinigame_UI.SetActive(false);
        interacted_UI.SetActive(false);
        logSharpeningMinigame_UI.SetActive(false);
    }

    public void ControlUI()
    {
        switch(MinigameInputManager.instance.action)
        {
            case PLayerAction.Normal:
                treeChopMinigame_UI.gameObject.SetActive(false);
                carryLogMinigame_UI.SetActive(false);
                logSharpeningMinigame_UI.SetActive(false);
                return;
            case PLayerAction.ChopTree:
                interacted_UI.SetActive(false);
                treeChopMinigame_UI.gameObject.SetActive(true);
                return;
            case PLayerAction.CarrySomething:
                interacted_UI.SetActive(false);
                carryLogMinigame_UI.SetActive(true);
                return;
            case PLayerAction.LogSharpening:
                interacted_UI.SetActive(false);
                logSharpeningMinigame_UI.SetActive(true);
                return;

        }
    }
    public void SetInteractableUIWithAction(bool flag, string action)
    {
        interacted_UI.SetActive(flag);
        actionToDo.text = action;
    }

}
