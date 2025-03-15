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
    }

    public void HandleAllMovement()
    {
        HandleGroundMovement();
        HandleRotation();
    }
    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticaInput_Values;
        horizontalMovement = PlayerInputManager.instance.horizontalInput_Values;
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

    private void HandleRotation()
    {
        if (!player.canRotate)
        {
            return;
        }

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

        // if we are moving when we attempt to dodge, we perform a roll
        if (PlayerInputManager.instance.moveAmount > 0)
        {
            //print("dodge one time");
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticaInput_Values;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput_Values;
            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            //perform a roll animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Male_Rolling_F", true, true);

        }
        // if we are stationary, we perform a backstep
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step", true, true);
        }

        player.currentStamina.Value -= dodgeStaminaCost;
    }


}
