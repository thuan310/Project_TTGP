using System.Collections;
using TMPro.Examples;
using UnityEngine.Events;
using UnityEngine;

public class Log : MonoBehaviour, IInteractableObject, IDamageable
{
    public string wordDisplayWhenInteract;
    public string WordDisplayWhenInteract { get => wordDisplayWhenInteract; set => wordDisplayWhenInteract = value; }
    public PlayerManager player { get; set; }


    public UnityEvent onInteract;
    public UnityEvent OnInteract { get => onInteract; set => onInteract = value; }
    public HealthSystem healthSystem;

    [SerializeField] private TreeType treeType;
    [SerializeField] private Transform coc;

    public Transform carryArea;

    private void Awake()
    {
        int healthAmount;

        switch (treeType)
        {
            default:
            case TreeType.Tree:
                {
                    healthAmount = 30;
                    break;
                }
            case TreeType.Log:
                {
                    healthAmount = 30;
                    break;
                }

        }

        healthSystem = new HealthSystem(healthAmount);
        healthSystem.OnDead += HealthSystem_OnDead;

        print("Khoi tao Log voi mau" + healthAmount);


    }
    private void Update()
    {
        if (healthSystem.GetHealth() == 0)
        {
            return;
        }
        if (player == null)
        {
            return;
        }
        if (player.action.Value != PLayerAction.LogSharpening)
        {
            return ;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Damage(30);
        }
    }

    public void Damage(int damage)
    {
        //print(rotation.eulerAngles);
        // Damage Popup
        int damageAmount = damage;

        ////Shake Camera
        //treeShake.GenerateImpulse();

        //// Spawn FX
        //Instantiate(fxTreeHit, hitArea.transform.position, Quaternion.identity);
        //Instantiate(fxTreeHit, hitArea.transform.position, Quaternion.identity);
        healthSystem.Damage(damageAmount);
        print(healthSystem.GetHealth());
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        switch (treeType)
        {
            default:
                break;
            case TreeType.Log:

                print("Spawn Coc");
                //topTreeRb.isKinematic = false;
                //// Spawn FX
                //Instantiate (fxTreeDestroyed, transform.position, transform.rotation);
                //spawn log
                Instantiate(coc, transform.position, this.transform.rotation);

                EventManager.instance.chopWoodMinigameEvents.SharpenWood();


                //print(player);
                player.playerDetectArea.interactableObjectsArray.Clear();

                player.action.Value = PLayerAction.Normal;
                player = null;

                Destroy(gameObject);


                //// Spawn stump
                //Instantiate(treeStump, transform.position, transform.rotation);
                break;
                //case Type.Log:
                //    // Spawn FX
                //    Instantiate(fxTreeLogDestroyed, transform.position, transform.rotation);

                //    // Spawn log Half
                //    float logYPositionAboveStump = 0.8f;
                //    treeLogOffset = transform.up * logYPositionAboveStump;
                //    Instantiate(treeLogHalf,transform.position + treeLogOffset, transform.rotation);

                //    //Spawn Log half
                //    float logYPositionAboveFirstLogHalf = 1.5f;
                //    treeLogOffset = transform.up * logYPositionAboveFirstLogHalf;
                //    Instantiate(treeLogHalf, transform.position + treeLogOffset, transform.rotation);
                //    break;
                //case Type.LogHalf:
                //    //Spawn FX
                //    Instantiate(fxTreeLogHalfDestroy, transform.position, transform.rotation);
                //    break;
                //case Type.Stump:
                //    // Spawn Fx
                //    Instantiate(fxTreeStumpDestroyed, transform.position, transform.rotation);
                //    break;
        }
        //Destroy(gameObject);
    }

    public void OnReset()
    {
        PlayerUIManager.instance.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().ResetValue();

    }

    void AttachToPlayer()
    {
        if(player.action.Value != PLayerAction.CarrySomething)
        {
            return;
        }
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.transform.position = carryArea.position;
        this.gameObject.transform.rotation = carryArea.rotation;

        if (!PlayerUIManager.instance.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().CheckIfValided())
        {
            this.gameObject.GetComponent<Collider>().enabled = true;
            OnExitInteracted();

        }
        return;
    }
    public void OnInteracted()
    {
        carryArea = player.gameObject.transform.Find("CarryArea");
        player.action.Value = PLayerAction.CarrySomething;
        OnReset();
        InvokeRepeating("AttachToPlayer", 0f,0.01f);
    }

    public void OnExitInteracted()
    {
        player.playerDetectArea.interactableObjectsArray.Remove(this);
        player.action.Value = PLayerAction.Normal;
        player = null;
        CancelInvoke("AttachToPlayer");
    }

}
