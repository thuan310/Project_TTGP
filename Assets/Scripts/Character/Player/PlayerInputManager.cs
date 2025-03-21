using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    public PlayerManager player;

    // Think about goals in steps
    // 1. find a way to read the values of a joy stick
    // 2. Move character based on those values

    PlayerControl playerControls;

    //Values for camera

    //Values for movement
    [Header("Player Movement input")]
    [SerializeField] Vector2 movementInput;
    [SerializeField] public float verticaInput_Values;
    [SerializeField] public float horizontalInput_Values;

    [Header("Player Action input")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInpput = false;

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

        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;
    }
    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // If we are loading into our world scene, enable our players controls
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
            player.SetUpStammina();
            PlayerCamera.instance.SetCameraToFollowPlayer();

        }
        //otherwise we must be at the main menu, diable our players controls
        // this is so our player cant move around if we enter things like a character creation menu ect
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControl();

            playerControls.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Player.Dodge.performed += i => dodgeInput = true;

            //Holding the input, sets the bool
            playerControls.Player.Sprint.performed += i => sprintInpput = true;
            // releasing the input, sets the bool to false
            playerControls.Player.Sprint.canceled += i => sprintInpput = false;

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
        HandleAllInout();
    }

    //Movement
    private void HandleAllInout()
    {
        HandlePlayerMovementInput();
        HandleDodgeInput();
        HandleSprinting();
    }    
    private void HandlePlayerMovementInput()
    {
        //print("a");
        verticaInput_Values= movementInput.y;
        horizontalInput_Values= movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticaInput_Values) + Mathf.Abs(horizontalInput_Values));

        if (moveAmount > 0f && moveAmount <= 0.5f)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount >= 0.5f &&  moveAmount <= 1f)
        {
            moveAmount = 1f;
        }

        //Why do we pass 0 on the horizontal? because we only want non-strafing movement
        //we use the horizontal when we are strafing or locked on 
        if (player == null)
        {
            return;
        }

        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting.Value);
    }
    //Action
    private void HandleDodgeInput()
    {
        if(dodgeInput)
        {
            dodgeInput = false;

            //future note: return(do nothing) if menu or ui is open
            //perform a dodge
            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }

    private void HandleSprinting()
    {
        if(sprintInpput)
        {
            // Handle Sprinting
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.isSprinting.Value = false;
        }
    }
}