using UnityEngine;
using Unity.Cinemachine;
public class PlayerCamera : MonoBehaviour
{
    public CinemachineCamera freelookCamera;

    public static PlayerCamera instance;
    public Camera cameraObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        // Ensure the virtualCamera reference is set
        if (freelookCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera reference is not set!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraToFollowPlayer()
    {
        freelookCamera.LookAt = GameObject.FindAnyObjectByType<PlayerManager>().transform;
        freelookCamera.Follow = GameObject.FindAnyObjectByType<PlayerManager>().transform;

    }

}
