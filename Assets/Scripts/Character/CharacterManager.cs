using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Status")]
    public Observable<bool> isDead;

    [HideInInspector] public CharacterController characterController;
     public Animator animator;

    [HideInInspector] public CharacterEffectManager characterEffectManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterCombatManager characterCombatManager;
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
    [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;

    [Header("Character Group")]
    public CharacterGroup characterGroup;

    [Header("Target")]
    //public Observable<ulong> currentargetNetworkObjectID;


    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isJumping = false;
    public bool isGrounded = false;
    public Observable<bool> isMoving;
    public Observable<bool> isInvulnerable;
    public Observable<bool> isSprinting ;
    public Observable<bool> isLockedOn;
    public Observable<bool> isCharingingAttack;
    public bool isInteracting;

    [Header("Active")]
    public Observable<bool> isActive;

    [Header("Equipment")]
    public Observable<int> currentWeaponBeingUsed;
    public Observable<int> currentRightHandWeaponID;
    public Observable<int> currentLeftHandWeaponID;
    public Observable<bool> isUsingRightHand;
    public Observable<bool> isUsingLeftHand;

    [Header("Resources")]
    public Observable<int> currentHealth;
    public Observable<int> maxHealth;
    public Observable<float> currentStamina ;
    public Observable<int> maxStamina;

    [Header("Stats")]
    public Observable<int> vitality;
    public Observable<int> endurance;

    public bool isDummy;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        characterController = GetComponent<CharacterController>();
        //add animator
        animator = GetComponent<Animator>();
        characterEffectManager = GetComponent<CharacterEffectManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        characterSoundFXManager = GetComponent <CharacterSoundFXManager>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        //print(animator);
    }

    protected virtual void Start()
    {
        IgnoreMyOwnColliders();

        OnIsActiveChanged(false,isActive.Value);

        isMoving.OnValueChanged += OnIsMovingChanged;
        isActive.OnValueChanged += OnIsActiveChanged;
        //print("da gan" + this.name);
    }

    virtual protected void OnDestroy()
    {
        isMoving.OnValueChanged -= OnIsMovingChanged;
        isActive.OnValueChanged -= OnIsActiveChanged;
    }

    protected virtual void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    protected virtual void FixedUpdate()
    {

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

    virtual protected void CheckHp(int oldValue, int newValue)
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

    protected virtual void IgnoreMyOwnColliders()
    {
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damageabeCharacterColliders = GetComponentsInChildren<Collider>();

        List<Collider> ignoreColliders = new List<Collider>();

        // Adds all of our damagaeable character colliders, to the list that will be used to ignore collisions
        foreach (var collider in damageabeCharacterColliders)
        {
            ignoreColliders.Add(collider);
        }

        // adds our character controller collider to the list that will be used to ignore collisions
        ignoreColliders.Add(characterControllerCollider);

        // goes through every collider on the list, and ignores collision with each other
        foreach(var collider in ignoreColliders)
        {
            foreach(var otherCollider in ignoreColliders)
            {
                Physics.IgnoreCollision(collider, otherCollider,true);
            }
        }
    }

    //

    private void OnIsMovingChanged(bool oldStatus, bool newStatus)
    {
        //print(isMoving.Value);
        animator.SetBool("isMoving", isMoving.Value);
    }

    public virtual void OnIsActiveChanged(bool oldStatus, bool newStatus)
    {
        gameObject.SetActive(isActive.Value);
    }

}
