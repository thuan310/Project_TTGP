using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;


public class PlayerInputManager : MonoBehaviour
{

    public static PlayerInputManager instance;

    public PlayerManager player;
    [HideInInspector] public PlayerDetectAreaManager playerDetectArea;

    // Think about goals in steps
    // 1. find a way to read the values of a joy stick
    // 2. Move character based on those values

    [HideInInspector] public PlayerControl playerControls;

    //Values for camera

    //Values for movement

    [Header("DevConfigure")]
    public bool isTesting;

    [Header("Lock On Input")]
    [SerializeField] bool lockOn_Input;
    [SerializeField] bool lockOn_Left_Input;
    [SerializeField] bool lockOn_Right_Input;
    private Coroutine lockOnCoroutine;


    [Header("Player Movement input")]
    [SerializeField] Vector2 movement_Input;
    [SerializeField] public float vertical_Input;
    [SerializeField] public float horizontal_Input;

    [Header("Player Action input")]
    [SerializeField] bool dodge_Input = false;
    [SerializeField] bool sprint_Inpput = false;
    [SerializeField] bool jump_Input = false;
    [SerializeField] bool interact_Input = false;
    [SerializeField] bool switch_Right_Weapon_Input = false;
    [SerializeField] bool switch_Left_Weapon_Input = false;

    [Header("Bumper input")]
    [SerializeField] bool RB_Input = false;

    [Header("Trigger input")]
    [SerializeField] bool RT_Input = false;
    [SerializeField] bool hold_RT_Input = false;

    [SerializeField] public float moveAmount;

    private void Awake()
    {
        if (instance == null)
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
        DontDestroyOnLoad(gameObject);

        // When the scene cahnges, run this logic
        SceneManager.activeSceneChanged += OnSceneChange;

        if (!isTesting)
        {
            instance.enabled = false;
        }
        if (playerControls != null)
        {
            playerControls.Disable();
        }

    }
    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // If we are loading into our world scene, enable our players controls
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            PlayerCamera.instance.enabled = true;
            player.playerDetectArea.enabled = true;
            PositionGuilding.instance.agent = player.GetComponentInChildren<NavMeshAgent>();
            instance.enabled = true;
            PlayerCamera.instance.SetCameraTo(player);
            if (playerControls != null)
            {
                playerControls.Enable();
            }
        }
        //otherwise we must be at the main menu, diable our players controls
        // this is so our player cant move around if we enter things like a character creation menu ect
        else
        {
            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
        }
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);
        player.isMoving.Value = false;
    }


    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControl();

            playerControls.Player.Move.performed += i => movement_Input = i.ReadValue<Vector2>();

            // Action
            playerControls.Player.Dodge.performed += i => dodge_Input = true;
            playerControls.Player.Jump.performed += i => jump_Input = true;
            playerControls.Player.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
            playerControls.Player.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;

            // RB
            playerControls.Player.RB.performed += i => RB_Input = true;

            // RT
            playerControls.Player.RT.performed += i => RT_Input = true;
            playerControls.Player.HoldRT.performed += i => hold_RT_Input = true;
            playerControls.Player.HoldRT.canceled += i => hold_RT_Input = false;

            // Lock on 
            playerControls.Player.LockOn.performed += i => lockOn_Input = true;
            playerControls.Player.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
            playerControls.Player.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;

            //Holding the input, sets the bool
            playerControls.Player.Sprint.performed += i => { sprint_Inpput = true; };
            // releasing the input, sets the bool to false
            playerControls.Player.Sprint.canceled += i => { sprint_Inpput = false; };

            playerControls.Player.Interact.performed += i => interact_Input = true;

            playerControls.Player.ToggleQuest.performed += i =>
            {
                EventManager.instance.inputEvents.ToggleQuestPressed();
            };

            #region LýThuyết
            /*3. Lambda Expression
                The => symbol is part of a lambda expression, which is a concise way to define an anonymous method (a method without a name).
                In your code:
                    i => movementInput = i.ReadValue<Vector2>() is a lambda expression.
                        i is the parameter (of type InputAction.CallbackContext) that represents the context of the input action.
                        movementInput = i.ReadValue<Vector2>() is the body of the lambda expression. It reads the input value as a Vector2 and assigns it to the movementInput variable.
            */
            #endregion
        }
        playerControls.Enable();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInput();
    }

    //Movement
    private void HandleAllInput()
    {
        HandleLockOnInput();
        HandleLockOnSwitchTargetInput();
        HandlePlayerMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleInteractInput();
        HandleRBInput();
        HandleRTInput();
        HandleChargeRTInput();
        HandleSwitchRighttWQeaponInput();
        HandleSwitchLefttWQeaponInput();
    }

    // Lock on
    private void HandleLockOnInput()
    {
        // check for dead target
        if ((player.isLockedOn.Value))
        {
            if (player.playerCombatManager.currentTarget == null)
                return;
            if (player.playerCombatManager.currentTarget.isDead.Value)
            {
                player.isLockedOn.Value = false;
            }

            // Attempt to find new target

            // this assure us that the coroutine never runs multiple times overlapping itself
            if (lockOnCoroutine != null)
                StopCoroutine(lockOnCoroutine);
            lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());

        }
        if (lockOn_Input && player.isLockedOn.Value)
        {
            lockOn_Input = false;
            PlayerCamera.instance.ClearLockOnTargets();
            player.isLockedOn.Value = false;
            PlayerCamera.instance.SetCameraTo(player);
            // Disable lock on
            return;
        }

        if (lockOn_Input && !player.isLockedOn.Value)
        {
            lockOn_Input = false;
            // if we are aiming using ranged weapons return ( do not allow lock whilst aiming)

            // Enable lock on
            PlayerCamera.instance.HandleLocatingLockOnTargets();

            if (PlayerCamera.instance.nearestLockOnTarget != null)
            {
                //print("da goi");
                // Set the target as our current target
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                PlayerCamera.instance.SetCameraTo(PlayerCamera.instance.nearestLockOnTarget);
                player.isLockedOn.Value = true;
                //PlayerCamera.instance.SetCameraTolookAtTarget(PlayerCamera.instance.nearestLockOnTarget.gameObject);

            }
        }


    }

    private void HandleLockOnSwitchTargetInput()
    {
        if (lockOn_Left_Input)
        {
            lockOn_Left_Input = false;
            if (player.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                    PlayerCamera.instance.SetCameraTo(PlayerCamera.instance.leftLockOnTarget);
                }
            }
        }
        if (lockOn_Right_Input)
        {
            //print("tim cach doi nhan vat");
            lockOn_Right_Input = false;
            if (player.isLockedOn.Value)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.instance.rightLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                    PlayerCamera.instance.SetCameraTo(PlayerCamera.instance.rightLockOnTarget);
                }
            }
        }
    }

    private void HandlePlayerMovementInput()
    {
        //print("a");
        vertical_Input = movement_Input.y;
        horizontal_Input = movement_Input.x;

        moveAmount = Mathf.Clamp01(movement_Input.magnitude);

        if (moveAmount > 0f && moveAmount <= 0.5f)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount >= 0.5f && moveAmount <= 1f)
        {
            moveAmount = 1f;
        }

        //Why do we pass 0 on the horizontal? because we only want non-strafing movement
        //we use the horizontal when we are strafing or locked on 
        if (player == null)
        {
            return;
        }

        if (moveAmount != 0f)
        {
            player.isMoving.Value = true;
        }
        else
        {
            player.isMoving.Value = false;
        }

        // if we are not locked on, only use the move amount

        if (!player.isLockedOn.Value || player.isSprinting.Value)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting.Value);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontal_Input, vertical_Input, player.isSprinting.Value);
        }

        // if we are locked on pass the horizontal movement as well

    }

    //Action
    private void HandleDodgeInput()
    {

        if (dodge_Input)
        {
            //print("dodge");
            dodge_Input = false;

            //future note: return(do nothing) if menu or ui is open
            //perform a dodge
            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleSprintInput()
    {
        if (sprint_Inpput)
        {
            // Handle Sprinting
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jump_Input)
        {
            jump_Input = false;

            // if we have a UI windowOPen, simply return without doing aything

            // Attempt to perform jump
            player.playerLocomotionManager.AttemptToPerformJump();
        }
    }

    //RB

    private void HandleRBInput()
    {
        if (RB_Input)
        {
            RB_Input = false;
            // todo : if we have a UI, return and do nothing

            player.SetCharcterActionHand(true);

            // todo: if wea re two handing the weapon, use the two handed action

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    //RT

    private void HandleRTInput()
    {
        if (RT_Input)
        {
            RT_Input = false;
            // todo : if we have a UI, return and do nothing

            player.SetCharcterActionHand(true);

            // todo: if wea re two handing the weapon, use the two handed action

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleChargeRTInput()
    {
        // we only want to check for a charge if we are in an action that requires it  (Attacking)
        if (player.isPerformingAction)
        {
            if (player.isUsingRightHand.Value)
            {
                player.isCharingingAttack.Value = hold_RT_Input;
            }
        }
    }

    private void HandleSwitchRighttWQeaponInput()
    {
        if (switch_Right_Weapon_Input)
        {
            switch_Right_Weapon_Input = false;
            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    private void HandleSwitchLefttWQeaponInput()
    {
        if (switch_Left_Weapon_Input)
        {
            switch_Left_Weapon_Input = false;
            player.playerEquipmentManager.SwitchLefttWeapon();
        }
    }

    private void HandleInteractInput()
    {
        if (!interact_Input)
        {
            return;
        }
        //print("interact");
        interact_Input = false;
        player.playerLocomotionManager.AttemptInteract();

    }


}
