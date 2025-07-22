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
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.transform.position = carryArea.position;
        this.gameObject.transform.rotation = carryArea.rotation;

        if (!PlayerUIManager.instance.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().CheckIfValided())
        {
            this.gameObject.GetComponent<Collider>().enabled = true;
            OnExitInteracted();
        }
        return;
    }
    public  void OnInteracted()
    {
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        OnReset();
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        player.action.Value = PLayerAction.CarrySomething;
        InvokeRepeating("AttachToPlayer", 0f, 0.01f);
    }

    public void OnExitInteracted()
    {
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        player.action.Value = PLayerAction.Normal;
    }

}
