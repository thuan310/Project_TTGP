using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;


    [SerializeField] float verticalMovement;
    [SerializeField]  float horizontalMovement;
    [SerializeField]  float moveAmount;

    [Header("Movement Settings")]
    Vector3 targetRotationDirection;
    Vector3 moveDirection;
    [SerializeField] private float sprintingSpeed = 10f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float walkingSpeed = 2.5f;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] float sprintingStaminaCost = 2;

    [Header("Jump")]
    [SerializeField] float jumpStaminaCost = 25f;
    [SerializeField] float jumpHeight = 4;
    [SerializeField] float jumpForwardSpeed = 5;
    [SerializeField] float freeFallSpeed = 2;
    private Vector3 jumpDirection;

    [SerializeField] private GameObject hitArea;

    [Header("Dodge")]
    private Vector3 rollDirection;
    [SerializeField] float dodgeStaminaCost = 25f;



    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    protected override void Update()
    {
        base.Update();

        HandlePickUpLog();

        Shader.SetGlobalVector("_Player", transform.position + Vector3.up * 2);
    }

    public void HandleAllMovement()
    {
        HandleGroundMovement();
        HandleRotation();
        HandleJumpingMovement();
        HandleFreeFallMovement();
    }
    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.vertical_Input;
        horizontalMovement = PlayerInputManager.instance.horizontal_Input;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }
    
    private void HandleGroundMovement()
    {
        if (!player.canMove)
        {
            return;
        }

        GetMovementValues();

        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;

        //normalize de con nhan voi toc do
        moveDirection.Normalize();
        //khong cho bay
        moveDirection.y = 0;

        if (player.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            //print("dang chay");
            if (moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            if (moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }


    }

    private void HandleJumpingMovement()
    {
        if(player.isJumping)
        {
            player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
        }
    }

    private void HandleFreeFallMovement()
    {
        if (!player.isGrounded)
        {
            Vector3 freeFallDirection;

            freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.vertical_Input;
            freeFallDirection += freeFallDirection + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontal_Input;
            freeFallDirection.y = 0;

            player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if(player.isDead.Value)
            return;
        if (!player.canRotate)
        {
            return;
        }

        if(player.isLockedOn.Value)
        {
            if(player.isSprinting.Value || player.playerLocomotionManager.isRolling)
            {
                Vector3 targetDirection = Vector3.zero;
                targetDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                targetDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                targetDirection.Normalize();
                targetDirection.y = 0; 

                if (targetDirection == Vector3.zero)
                    targetDirection = transform.forward;

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = finalRotation;
            }
            else
            {
                if (player.playerCombatManager.currentTarget == null)
                    return;

                Vector3 targetDirection;
                targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                targetDirection.y = 0;
                targetDirection.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = finalRotation;
            }
        }
        else
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;
            //print(PlayerCamera.instance.cameraObject.transform.forward);

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            //print(newRotation.eulerAngles);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }

    public void HandleSprinting()
    {
        if(player.isPerformingAction)
        {
            // set sprinting to false
            player.isSprinting.Value = false;
        }

        if (player.currentStamina.Value <=0)
        {
            player.isSprinting.Value = false;
            return;
        }

        //if we are out of stamina, set sprinting to false

        // if we are moving set sprinting to true
        if (moveAmount >= 0.5)
        {
            player.isSprinting.Value = true;
        }
        // if we are stationary/moving slowly sprinting is false
        else
        {
            player.isSprinting.Value = false;
        }

        if (player.isSprinting.Value)
        {
            player.currentStamina.Value -= sprintingStaminaCost*Time.deltaTime;
        }

    }

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
        {
            return;
        }
        //print("lan");
        //het mana thi sao roll
        if (player.currentStamina.Value <= 0)
            return;
        //print("Roll");
        // if we are moving when we attempt to dodge, we perform a roll
        if (PlayerInputManager.instance.moveAmount > 0)
        {
            //print("dodge one time");
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.vertical_Input;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontal_Input;
            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            //perform a roll animation
            //print("Roll");
            //print(player.playerAnimatorManager);
            player.playerAnimatorManager.PlayTargetActionAnimation("Male_Rolling_F", true, true);
            player.playerLocomotionManager.isRolling = true;

        }
        // if we are stationary, we perform a backstep
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step", true, true);
        }

        player.currentStamina.Value -= dodgeStaminaCost;
    }

    public void AttemptToPerformJump()
    {
        // If we are performing a general action, we do not want to allow a jump (will change when combat is added)
        if (player.isPerformingAction)
            return;

        // if we are out of stamina, we do not wish to allow a jump
        if (player.currentStamina.Value <= 0)
            return;

        // If we are already in a jump, we do not want to allow a jump again until the current jump has finsished
        if (player.isJumping)
            return;

        // If we are not ground, we do not want to allow a jump
        if (!player.isGrounded)
            return;

        // if we are two handing our, play the two handed jump animatino, otherwise play the one haned animation (to do)
        player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);

        player.isJumping = true;

        player.currentStamina.Value -= jumpStaminaCost;

        jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.vertical_Input;
        jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontal_Input;
        jumpDirection.y = 0;

        if (jumpDirection != Vector3.zero)
        {
            // if we are sprinting, jump direction is at full distance 
            if (player.isSprinting.Value)
            {
                jumpDirection *= 1;
            }
            // if we are running, jump direction is at half distance
            else if (PlayerInputManager.instance.moveAmount >= 0.5)
            {
                jumpDirection *= 0.5f;
            }
            //if we are walking, jump direction is at quater distance
            else if (PlayerInputManager.instance.moveAmount <= 0.5)
            {
                jumpDirection *= 0.25f;
            }
        }



    }

    public void ApplyJumpingVelocity()
    {
        // apply an upward velocity
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
    }

    //Attack Part
    //Interact part
    public void AttemptInteract()
    {
        switch (player.action.Value)
        {
            default:
                break;
            case PLayerAction.Normal:
                if (player.playerDetectArea.interactableObject != null )
                {
                    //print(player.playerDetectArea.interactableObject);
                    player.playerDetectArea.interactableObject.OnInteracted();
                }
                return;
            case PLayerAction.ChopTree:
                // check if playing animation
                if (player.isPerformingAction)
                {
                    return;
                }
                PlayerUIManager.instance.treeChopMinigame_UI.GetComponentInChildren<ProgressBar>().AddValue();
                return;
            case PLayerAction.CarrySomething:
                // check if playing animation
                if (player.isPerformingAction)
                {
                    return;
                }
                PlayerUIManager.instance.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().AddValue();
                return;
        }
    }
    //Escape part
    public void AttemptingQuitting()
    {
        //print("qua ngu");   
        if(player.action.Value != PLayerAction.Normal)
        {
            player.action.Value = PLayerAction.Normal;
            player.enabled = false;
            PlayerInputManager.instance.enabled = true;
            player.canMove = true;
        }

    }

    public void HandlePickUpLog()
    {
        if (player.action.Value != PLayerAction.CarrySomething) 
        {
            return;
        }
    }

}
