using UnityEngine;

public class WoodCart : MonoBehaviour
{
    public GameObject dropArea;
    public Vector3 colliderSize = new Vector3 (1,1,1) ;
    public Transform dropPlace;

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
            Coc coc = collider.GetComponentInParent<Coc>();
            if (coc == null)
            {
                continue;
            }
            //tree.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            coc.transform.SetParent(dropPlace, false);
            coc.transform.localPosition = new Vector3(0,0,0);
            coc.transform.rotation = Quaternion.Euler(new Vector3 (90,0,0));

        }
    }
    public Color gizmoColor = Color.red;

    public void OnInteracted()
    {

    }

    public void OnExitInteracted()
    {

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
}
