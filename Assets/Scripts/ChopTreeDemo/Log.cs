using System.Collections;
using TMPro.Examples;
using UnityEngine;

public class Log : MonoBehaviour,IInteractableObject, IDamageable
{
    PlayerManager player;
    private HealthSystem healthSystem;

    [SerializeField] private Tree.Type treeType;
    [SerializeField] private Transform coc;

    public Transform carryArea;

    private void Awake()
    {
        int healthAmount;

        switch (treeType)
        {
            default:
            case Tree.Type.Tree:
                {
                    healthAmount = 30;
                    break;
                }
            case Tree.Type.Log:
                {
                    healthAmount = 30;
                    break;
                }

        }

        healthSystem = new HealthSystem(healthAmount);
        healthSystem.OnDead += HealthSystem_OnDead;


    }

    public void Damage()
    {
        if (PlayerInputManager.instance.action != PlayerInputManager.Action.LogSharpening)
        {
            return;
        }
        //print(rotation.eulerAngles);
        // Damage Popup
        int damageAmount = 10;

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
            case Tree.Type.Log:

                print("Spawn Coc");
                //topTreeRb.isKinematic = false;
                //// Spawn FX
                //Instantiate (fxTreeDestroyed, transform.position, transform.rotation);
                OnExitInteracted();
                //spawn log
                Instantiate(coc, transform.position, this.transform.rotation);

                Destroy(this.gameObject);


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

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponentInParent<PlayerManager>();
        carryArea = player.gameObject.transform.Find("CarryArea");
    }

    public void OnReset()
    {
        PlayerUIManager.instance.playerUIDynamicHUDManager.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().ResetValue();
    }

    void AttachToPlayer()
    {
        if(PlayerInputManager.instance.action != PlayerInputManager.Action.CarrySomething)
        {
            return;
        }
        this.gameObject.transform.position = carryArea.position;
        this.gameObject.transform.rotation = carryArea.rotation;

        if (!PlayerUIManager.instance.playerUIDynamicHUDManager.carryLogMinigame_UI.GetComponentInChildren<ProgressBar>().CheckIfValided())
        {
            OnExitInteracted();
        }
        return;
    }
    public void OnInteracted()
    {
        PlayerInputManager.instance.action = PlayerInputManager.Action.CarrySomething;
        OnReset();
        InvokeRepeating("AttachToPlayer", 0f,0.01f);
    }

    public void OnExitInteracted()
    {
        PlayerInputManager.instance.Quit();
        CancelInvoke("AttachToPlayer");
    }
}
