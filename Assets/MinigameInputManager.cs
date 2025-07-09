using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameInputManager : MonoBehaviour
{
    public static MinigameInputManager instance;

    public PlayerManager player;

    [SerializeField] bool interact_Input = false;
    [SerializeField] bool attack_Input = false;
    [SerializeField] bool quitting_Input = false;

    [HideInInspector] public PlayerControl minigameControls;

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

        if (minigameControls != null)
        {
            minigameControls.Disable();
        }

    }

    private void OnDisable()
    {
        if (minigameControls != null)
        {
            minigameControls.Disable();
        }
    }
    private void OnEnable()
    {
        if (minigameControls == null)
        {
            minigameControls = new PlayerControl();

            minigameControls.Minigame.Interact.performed += i => interact_Input = true;
            minigameControls.Minigame.Attack.performed += i => attack_Input = true;

            minigameControls.Player.Quit.performed += i => quitting_Input = true;

        }
        minigameControls.Enable();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                minigameControls.Enable();
            }
            else
            {
                minigameControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInput();
    }

    private void HandleAllInput()
    {
        HandleQuittingInput();
        HandleAttackInput();
        HandleInteractInput();
    }

    private void HandleAttackInput()
    {
        if(player == null)
        {
            return;
        }
        if (player.isPerformingAction)
        {
            return;
        }
        if (attack_Input)
        {
            attack_Input = false;
            AttemptToAttack();
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

    public void HandleQuittingInput()
    {
        if (!quitting_Input)
        {
            return;
        }
        quitting_Input = false;
        player.playerLocomotionManager.AttemptingQuitting();
    }

    public void Quit()
    {
        player.playerLocomotionManager.AttemptingQuitting();
    }

    public void AttemptToAttack()
    {
        switch (player.action.Value)
        {
            default:
                break;
            case PLayerAction.Normal:
                player.playerAnimatorManager.PlayTargetActionAnimation("SwingAxe", true, true);
                StartCoroutine(AnimationEvent_OnHit());
                return;
            case PLayerAction.ChopTree:
                if (PlayerUIManager.instance.treeChopMinigame_UI.GetComponentInChildren<ProgressBar>().CheckIfValided())
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation("SwingAxe", true, true, false, false);
                    StartCoroutine(AnimationEvent_OnHit());
                }
                else
                {
                    PlayerUIManager.instance.treeChopMinigame_UI.GetComponentInChildren<CheckBox>().TickColor();
                }
                return;
            case PLayerAction.LogSharpening:
                if (PlayerUIManager.instance.logSharpeningMinigame_UI.GetComponentInChildren<ProgressBar>().CheckIfValided())
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation("SwingAxe", true, true, false, false);
                    Vector3 colliderSize = Vector3.one * 0.3f;
                    //Collider[] colliderArray = Physics.OverlapBox(hitArea.transform.position, colliderSize);
                    ////print(colliderArray[0]);
                    //foreach (Collider collider in colliderArray)
                    //{
                    //    //print(collider.name);
                    //    if (collider.GetComponent<SharpingTable>() != null)
                    //    {
                    //        collider.GetComponent<SharpingTable>().SharpLog();
                    //    }

                    //    //sharpingTable.SharpLog();

                    //}
                }
                else
                {
                    PlayerUIManager.instance.logSharpeningMinigame_UI.GetComponentInChildren<CheckBox>().TickColor();
                }
                return;
        }

    }

    IEnumerator AnimationEvent_OnHit()
    {
        //Find objects in Hit area  
        Vector3 colliderSize = Vector3.one * 0.3f;
        //print(colliderArray[0]);

            if (player.playerDetectArea.interactableObject == null)
            {
                yield break;
            }
            print("hit");
        //player.playerDetectArea.interactableObject.gameObject.GetComponent<Tree>().Damage();
        yield return new WaitForSeconds(0.5f);
    }
}
