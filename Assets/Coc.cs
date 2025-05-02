using TMPro.Examples;
using UnityEngine;

public class Coc : MonoBehaviour,IInteractableObject
{
    PlayerManager player;

    [SerializeField] private Tree.Type treeType;

    private Transform carryArea;
    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponentInParent<PlayerManager>();
        carryArea = player.gameObject.transform.Find("CarryArea");
    }

    public void OnReset()
    {
        PlayerUIManager.instance.playerUIDynamicHUDManager.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().ResetValue();
    }

    void AttachToPlayer()
    {
        if (PlayerInputManager.instance.action != PlayerInputManager.Action.CarrySomething)
        {
            return;
        }
        this.gameObject.transform.position = carryArea.position;
        this.gameObject.transform.rotation = carryArea.rotation;

        if (!PlayerUIManager.instance.playerUIDynamicHUDManager.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().CheckIfValided())
        {
            OnExitInteracted();
        }
        return;
    }
    public void OnInteracted()
    {
        OnReset();
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false; 
        PlayerInputManager.instance.action = PlayerInputManager.Action.CarrySomething;
        InvokeRepeating("AttachToPlayer", 0f, 0.01f);
    }

    public void OnExitInteracted()
    {
        PlayerInputManager.instance.Quit();
        CancelInvoke("AttachToPlayer");
    }
}
