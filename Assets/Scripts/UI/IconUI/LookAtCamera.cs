using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }

    [SerializeField] private Canvas canvas;  // Reference to the Canvas the icon is a child of
    private GameObject targetObject;
    private RectTransform iconRectTransform;  // Reference to the RectTransform of the icon

    [SerializeField] private Vector2 screenPadding = new Vector2(50, 50);

    private void Start()
    {
        iconRectTransform = GetComponent<RectTransform>();
        canvas = transform.parent.GetComponent<Canvas>();
        targetObject = transform.parent.parent.gameObject;
    }

    private void LateUpdate()
    {
        if (targetObject != null && iconRectTransform != null)
        {
            Vector3 targetWorldPosition = targetObject.transform.position;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetWorldPosition);

            if (screenPosition.z > 0)
            {
                screenPosition.x = Mathf.Clamp(screenPosition.x, screenPadding.x, Screen.width - screenPadding.x);
                screenPosition.y = Mathf.Clamp(screenPosition.y, screenPadding.y, Screen.height - screenPadding.y);

                iconRectTransform.position = screenPosition;
            }
            else
            {
                iconRectTransform.position = new Vector3(-1000, -1000, 0); 
            }
        }


    }
}
