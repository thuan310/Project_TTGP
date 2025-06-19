using UnityEngine;

public class Reflection : MonoBehaviour
{
    public Camera mainCamera;                 
    public Camera reflectionCamera;          
    public RenderTexture reflectionTexture;   
    public Transform reflectionPlane;         

    void Start()
    {
        if (!reflectionCamera || !reflectionTexture || !reflectionPlane)
        {
            Debug.LogWarning("Reflection setup incomplete.");
            enabled = false;
            return;
        }

        reflectionCamera.enabled = false;
        reflectionCamera.targetTexture = reflectionTexture;
    }

    void LateUpdate()
    {
        if (!mainCamera || !reflectionCamera || !reflectionPlane) return;

        float planeY = reflectionPlane.position.y;

        // 1. Mirror camera position over the plane
        Vector3 camPos = mainCamera.transform.position;
        Vector3 reflectedPos = camPos;
        reflectedPos.y = planeY - (camPos.y - planeY);
        reflectionCamera.transform.position = reflectedPos;

        // 2. Mirror camera direction and up vector
        Vector3 forward = mainCamera.transform.forward;
        Vector3 up = mainCamera.transform.up;
        forward.y = -forward.y;
        up.y = -up.y;
        reflectionCamera.transform.rotation = Quaternion.LookRotation(forward, up);

        // 3. Match projection settings
        reflectionCamera.fieldOfView = mainCamera.fieldOfView;
        reflectionCamera.aspect = mainCamera.aspect;
        reflectionCamera.nearClipPlane = mainCamera.nearClipPlane;
        reflectionCamera.farClipPlane = mainCamera.farClipPlane;

        // 4. Render reflection
        reflectionCamera.Render();
    }
}
