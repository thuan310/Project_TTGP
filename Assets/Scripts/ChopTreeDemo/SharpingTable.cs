using UnityEngine;
using UnityEngine.Events;

public class SharpingTable : MonoBehaviour, IInteractableObject
{
    public string wordDisplayWhenInteract;
    public string WordDisplayWhenInteract { get => wordDisplayWhenInteract; set => wordDisplayWhenInteract = value; }
    public PlayerManager player { get; set; }


    public UnityEvent onInteract;
    public UnityEvent OnInteract { get => onInteract; set => onInteract = value; }

    public GameObject dropArea;
    public Vector3 colliderSize = new Vector3(1, 1, 1);
    public Transform dropPlace;
    public Log log;

    private void Awake()
    {
        dropArea = transform.Find("DropArea").gameObject;
    }
    private void Update()
    {
        Bo();
    }

    public void Bo()
    {
        //Find objects in Hit area
        Collider[] colliderArray = Physics.OverlapBox(dropArea.transform.position, colliderSize);
        //print(colliderArray[0]);
        foreach (Collider collider in colliderArray)
        {
            if (dropPlace.GetComponentInChildren<Log>() != null)
            {
                return;
            }
            log = collider.GetComponent<Log>();
            if (log == null)
            {
                continue;
            }
            //tree.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            log.transform.localPosition = new Vector3(0, 0, 0);
            log.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            log.transform.SetParent(dropPlace, false);
            log.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            log.gameObject.GetComponent<Collider>().enabled = false;

        }
    }
    public Color gizmoColor = Color.red;


    public void SharpLog()
    {
        log.Damage();
    }
    public  void OnInteracted()
    {
        //print("A");
        if (dropPlace.GetComponentInChildren<Log>() != null)
        {
            player.action.Value = PLayerAction.LogSharpening;
            OnReset();
        }
    }

    public void OnExitInteracted()
    {
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        player.action.Value = PLayerAction.Normal;
    }

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

    public void OnReset()
    {
        PlayerUIManager.instance.logSharpeningMinigame_UI.GetComponentInChildren<CheckBox>().ResetColor();
    }
}
