using System.Collections;
using UnityEngine;

public class PlayerDetectArea : MonoBehaviour
{
    public PlayerManager player;
    public IInteractableObject interactableObject;
    public Transform hitArea;
    public Color gizmoColor = Color.red;
    public Vector3 colliderSize;

    public void CheckHitArea()
    {
        //Find objects in Hit area
        if (player.isInteracting)
        {
            return;
        }
        Collider[] colliderArray = Physics.OverlapBox(hitArea.transform.position, colliderSize);
        //print(colliderArray.Length);
        if (colliderArray.Length == 1)
        {
            interactableObject = null;
            PlayerUIManager.instance.playerUIDynamicHUDManager.SetInteractableUI(false);
        }
        foreach (Collider collider in colliderArray)
        {
            if (collider.GetComponentInParent<IInteractableObject>() == null)
            {
                continue;
            }
            interactableObject = collider.GetComponentInParent<IInteractableObject>();
            PlayerUIManager.instance.playerUIDynamicHUDManager.SetInteractableUI(true);
        }

    }
    //private void OnDrawGizmos()
    //{
    //    if (hitArea == null) return;

    //    // Save original Gizmos matrix
    //    Matrix4x4 oldMatrix = Gizmos.matrix;

    //    // Set Gizmos to use DropArea's transform (position + rotation)
    //    Gizmos.matrix = Matrix4x4.TRS(
    //        hitArea.transform.position,
    //        hitArea.transform.rotation,
    //        Vector3.one // Keep scale as 1, unless you want to match local scale too
    //    );

    //    Gizmos.color = gizmoColor;
    //    Gizmos.DrawWireCube(Vector3.zero, colliderSize * 2f); // Centered at origin in local space

    //    // Restore original Gizmos matrix
    //    Gizmos.matrix = oldMatrix;
    //}
    private void Update()
    {
        CheckHitArea();
        //print(interactableObject);
    }
}
