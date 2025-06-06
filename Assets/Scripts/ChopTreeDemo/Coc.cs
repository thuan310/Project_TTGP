using TMPro.Examples;
using UnityEngine;

public class Coc : IInteractableObject
{
    PlayerManager player;

    [SerializeField] private TreeType treeType;

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
        if (MinigameInputManager.instance.action != PLayerAction.CarrySomething)
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
    public override void OnInteracted()
    {
        base.OnInteracted();
        MinigameInputManager.instance.enabled = true;
        OnReset();
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        MinigameInputManager.instance.action = PLayerAction.CarrySomething;
        InvokeRepeating("AttachToPlayer", 0f, 0.01f);
    }

    public override void OnExitInteracted()
    {
        base.OnExitInteracted();
        MinigameInputManager.instance.enabled = false;
    }

}
