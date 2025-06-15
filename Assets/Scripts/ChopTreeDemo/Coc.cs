using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;

public class Coc : MonoBehaviour,IInteractableObject
{
    public string wordDisplayWhenInteract;
    public string WordDisplayWhenInteract { get => wordDisplayWhenInteract; set => wordDisplayWhenInteract = value; }
    public PlayerManager player { get; set; }


    public UnityEvent onInteract;
    public UnityEvent OnInteract { get => onInteract; set => onInteract = value; }

    [SerializeField] private TreeType treeType;

    private Transform carryArea;
    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponentInParent<PlayerManager>();
        carryArea = player.gameObject.transform.Find("CarryArea");
    }

    public void OnReset()
    {
        PlayerUIManager.instance.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().ResetValue();
    }

    void AttachToPlayer()
    {
        if (player.action.Value != PLayerAction.CarrySomething)
        {
            return;
        }
        this.gameObject.transform.position = carryArea.position;
        this.gameObject.transform.rotation = carryArea.rotation;

        if (!PlayerUIManager.instance.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().CheckIfValided())
        {
            OnExitInteracted();
        }
        return;
    }
    public  void OnInteracted()
    {
        MinigameInputManager.instance.enabled = true;
        OnReset();
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        player.action.Value = PLayerAction.CarrySomething;
        InvokeRepeating("AttachToPlayer", 0f, 0.01f);
    }

    public void OnExitInteracted()
    {
        MinigameInputManager.instance.enabled = false;
    }

}
