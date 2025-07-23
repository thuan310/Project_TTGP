using UnityEngine;
using UnityEngine.Events;

public class WoodCart : MonoBehaviour, IInteractableObject
{
    public GameObject dropArea;
    public Vector3 colliderSize = new Vector3 (1,1,1) ;
    public Transform[] dropPlace;
    public int idx = 0;
    public static WoodCart instance;

    public bool isPickingCoc = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Update()
    {
        Bo();
    }

    public void Bo()
    {
        if (!isPickingCoc)
        {
            return;
        }
        //Find objects in Hit area
        Collider[] colliderArray = Physics.OverlapBox(dropArea.transform.position, colliderSize);
        foreach (Collider collider in colliderArray)
        {
            //print(collider.name);
            Coc coc = collider.GetComponentInParent<Coc>();
            if (coc == null)
            {
                continue;
            }
            coc.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            coc.gameObject.GetComponent<Collider>().enabled = false;
            coc.transform.localPosition = new Vector3(0, 0, 0);
            coc.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            coc.transform.SetParent(dropPlace[idx], false);
            idx++;
            EventManager.instance.chopWoodMinigameEvents.DropWood();

        }
    }
    public Color gizmoColor = Color.red;

    public void OnExitInteracted()
    {

    }

    public string wordDisplayWhenInteract;
    public string WordDisplayWhenInteract { get => wordDisplayWhenInteract; set => wordDisplayWhenInteract = value; }
    public PlayerManager player { get; set; }


    public UnityEvent onInteract;
    public UnityEvent OnInteract { get => onInteract; set => onInteract = value; }

    public void OnInteracted()
    {
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        OnInteract.Invoke();
    }

    // debug
    private void OnDrawGizmos()
    {
        if (dropArea == null) return;

        // Save original Gizmos matrix
        Matrix4x4 oldMatrix = Gizmos.matrix;

        // Set Gizmos to use DropArea's transform (position + rotation)
        Gizmos.matrix = Matrix4x4.TRS(
            dropArea.transform.position,
            dropArea.transform.rotation,
            Vector3.one // Keep scale as 1, unless you want to match local scale too
        );

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(Vector3.zero, colliderSize * 2f); // Centered at origin in local space

        // Restore original Gizmos matrix
        Gizmos.matrix = oldMatrix;
    }
}
