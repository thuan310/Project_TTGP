using UnityEngine;
using UnityEngine.InputSystem.XInput;
using Unity.Cinemachine;
using System.Collections;

public class TreeChopSimulator : MonoBehaviour
{
    GameObject treeUnchopped;
    [SerializeField ]GameObject treeChopped;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CinemachineImpulseSource treeShake;
    [SerializeField] private GameObject hitArea;
    [SerializeField] private GameObject fxTreeHit;
    [SerializeField] private GameObject fxTreeHitBlocks;


    int treeState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

     IEnumerator AnimationEvent_OnHit()
    {
        //Find objects in Hit area
        Vector3 colliderSize = Vector3.one * 0.3f;
        Collider[] colliderArray = Physics.OverlapBox(hitArea.transform.position, colliderSize);
        print(colliderArray[0]);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<ITreeDamageable>(out ITreeDamageable treeDamageable))
            {
                print("mat quyen cong dan");
                // Damage Popup
                int damageAmount = UnityEngine.Random.Range(10, 30);
                
                // Damage Tree
                treeDamageable.Damage(damageAmount);

                ////Shake Camera
                //treeShake.GenerateImpulse();

                //// Spawn FX
                //Instantiate(fxTreeHit, hitArea.transform.position, Quaternion.identity);
                //Instantiate(fxTreeHit, hitArea.transform.position, Quaternion.identity);
            }
        }
        yield return new WaitForSeconds(0.5f);
    }
    private void Update()
    {
        //if (treeState >=3)
        //{
        //    Instantiate(treeChopped, transform.position, transform.rotation);
        //    Destroy(gameObject);
        //}
        //SetTreeState("Tree_State1");

        HandleAttack();
        HandleMovement();
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if (animator != null)
            //{
            //    animator.SetTrigger("Attack");
            //}

            StartCoroutine(AnimationEvent_OnHit());
        }
    }

    private void HandleMovement()
    {

    }

    public void Damage(int amount)
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
