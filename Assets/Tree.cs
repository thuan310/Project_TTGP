
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class Tree : MonoBehaviour, IDamageable, IInteractableObject
{
    //Các cuộc nghiên cứu cho thấy các cọc ở đây đa phần được làm từ gỗ lim,
    //thân dài từ 2,6 đến 2,8 mét và có đường kính khoảng 20 đến 30 cm.
    //Phần đầu cọc được đẽo nhọn để cắm xuống đáy sông dài từ 0,5 đến 1 mét
    //và khoảng cách trung bình giữa các cọc khoảng 1 mét.[13][14] Gần bãi cọc
    //có một tấm bia được dựng lên để đánh dấu và ghi nhận khu di tích.[13]
    public enum Type
    {
        Tree,
        Log,
        Coc
    }
    [SerializeField] private Type treeType;
    [SerializeField] private Transform fxTreeDestroyed;
    [SerializeField] private Transform fxTreeLogDestroyed;
    [SerializeField] private Transform fxTreeLogHalfDestroy;
    [SerializeField] private Transform fxTreeStumpDestroyed;
    [SerializeField] private Transform log;
    [SerializeField] private Transform treeLogHalf;
    [SerializeField] private Transform treeStump;
    [SerializeField] private Rigidbody topTreeRb;
    [SerializeField] private Rigidbody botTreeRb;
    [SerializeField] private Transform topTree;
    [SerializeField] private Transform botTree;

    private Quaternion logRotation;

    private HealthSystem healthSystem;

    private void Awake()
    {
        int healthAmount;

        switch (treeType)
        {
            default:
            case Type.Tree:
                {
                    healthAmount = 30;
                    break;
                }
            case Type.Log:
                {
                    healthAmount = 50;
                    break;
                }

        }

        healthSystem = new HealthSystem(healthAmount);
        healthSystem.OnDead += HealthSystem_OnDead;

        topTree = transform.GetChild(0).Find("Top");
        topTreeRb = this.GetComponentInChildren<Rigidbody>();
        botTree = transform.GetChild(0).Find("Bot");
        botTree.GetComponent<BoxCollider>().enabled = false;

        this.gameObject.GetComponent<BoxCollider>().enabled = true;


        //print(topTree);
    }

    public void OnReset()
    {
        healthSystem.Heal(30);
        PlayerUIManager.instance.playerUIDynamicHUDManager.treeChopMinigame_UI.GetComponentInChildren<CheckBox>().ResetColor();
    }

    public Vector3 GetRelativeObjectDirection(Vector3 playerEulerRotation)
    {
        Vector3 originalOnjectDiretion = new Vector3(0,0,1);
        Quaternion playerRotation = Quaternion.Euler(playerEulerRotation);
        return playerRotation * originalOnjectDiretion;
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        switch (treeType)
        {
            default:
                break ;
            case Type.Tree:
                this.gameObject.GetComponent<BoxCollider>().enabled = false;

                botTree.GetComponent<BoxCollider>().enabled = true;

                print("Spawn Log");
                //topTreeRb.isKinematic = false;
                //// Spawn FX
                //Instantiate (fxTreeDestroyed, transform.position, transform.rotation);
                OnExitInteracted();
                //spawn log
                Instantiate(log, transform.position , logRotation);

                Destroy(topTree.gameObject);


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

    public void Damage()
    {
        if(PlayerInputManager.instance.action != PlayerInputManager.Action.ChopTree)
        {
            return;
        }
        Quaternion rotation = Quaternion.Euler(GetRelativeObjectDirection(this.transform.rotation.eulerAngles));
        //print(rotation.eulerAngles);
        // Damage Popup
        int damageAmount = 10;

        ////Shake Camera
        //treeShake.GenerateImpulse();

        //// Spawn FX
        //Instantiate(fxTreeHit, hitArea.transform.position, Quaternion.identity);
        //Instantiate(fxTreeHit, hitArea.transform.position, Quaternion.identity);
        healthSystem.Damage(damageAmount);
        logRotation = rotation;
        print(healthSystem.GetHealth());
    }

    public void OnInteracted() 
    {
        PlayerInputManager.instance.action = PlayerInputManager.Action.ChopTree;

        OnReset();
        //print("tien Hanh Chat cay");
    }
    public void OnExitInteracted()
    {
        PlayerInputManager.instance.Quit();
    }

}
