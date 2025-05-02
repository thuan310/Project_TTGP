using UnityEngine;
using UnityEngine.InputSystem.XInput;
using Unity.Cinemachine;
using System.Collections;

public class TreeChopSimulator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CinemachineImpulseSource treeShake;
    [SerializeField] private GameObject fxTreeHit;
    [SerializeField] private GameObject fxTreeHitBlocks;

    PlayerManager player;

    int treeState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerManager>();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //if (treeState >=3)
        //{
        //    Instantiate(treeChopped, transform.position, transform.rotation);
        //    Destroy(gameObject);
        //}
        //SetTreeState("Tree_State1");
        HandleMovement();
        HandleIncreaseProgress();
    }

    private void HandleIncreaseProgress()
    {
    }

    private void HandleMovement()
    {

    }

    //private void SetTreeState(string targetAnimation)
    //{
    //    if (treeState >=3)
    //    {
    //        return;
    //    }
    //    animator.CrossFade(targetAnimation, 0.2f);
    //    treeState += 1;
    //}
    
}
