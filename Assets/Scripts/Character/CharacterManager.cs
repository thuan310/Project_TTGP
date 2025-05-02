using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Status")]
    public Observable<bool> isDead;

    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;

    [HideInInspector] public CharacterEffectManager characterEffectManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;


    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isJumping = false;
    public bool isGrounded = false;
    [SerializeField] public Observable<bool> isSprinting = new Observable<bool>(false);
    public bool isInteracting;

    [Header("Resources")]
    public Observable<int> currentHealth;
    public Observable<int> maxHealth;
    public Observable<float> currentStamina ;
    public Observable<int> maxStamina;

    [Header("Stats")]
    public Observable<int> vitality;
    public Observable<int> endurance;

    virtual protected void Awake()
    {
        DontDestroyOnLoad(gameObject);

        characterController = GetComponent<CharacterController>();
        //add animator
        animator = GetComponent<Animator>();
        characterEffectManager = GetComponent<CharacterEffectManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();

        //print(animator);
    }
    virtual protected void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        currentHealth.Value = 0;
        isDead.Value = true;

        // reset any flags here that need to be reset
        // nothing

        // if we are not grounded, play an aerial Death animation
        if(!manuallySelectDeathAnimation)
        {
            characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
        }
        //play some death SFX

        yield return new WaitForSeconds(5);

        // award players with runes

        // disable character
    }
    
    public virtual void ReviveCharacter()
    {

    }

    public void CheckHp(int oldValue, int newValue)
    {
        if(currentHealth.Value <= 0)
        {
            StartCoroutine(ProcessDeathEvent());
        }
        //prevent us from overhealing
        if(currentHealth.Value > maxHealth.Value)
        {
            currentHealth.Value = maxHealth.Value;
        }
    }
}
