using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;
    [SerializeField] public Observable<bool> isSprinting = new Observable<bool>(false);

    [Header("Stats")]
    [SerializeField] public Observable<int> endurance;
    [SerializeField] public Observable<float> currentStamina ;
    [SerializeField] public Observable<int> maxStamina;

    virtual protected void Awake()
    {
        characterController = GetComponent<CharacterController>();
        //add animator
        animator = GetComponent<Animator>();

        //print(animator);
    }
    virtual protected void Update()
    {
        
    }


}
