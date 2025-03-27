
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tree : MonoBehaviour, ITreeDamageable
{
    public enum Type
    {
        Tree,
        Log,
        LogHalf,
        Stump
    }
    [SerializeField] private Type treeType;
    [SerializeField] private Transform fxTreeDestroyed;
    [SerializeField] private Transform fxTreeLogDestroyed;
    [SerializeField] private Transform fxTreeLogHalfDestroy;
    [SerializeField] private Transform fxTreeStumpDestroyed;
    [SerializeField] private Transform treeLog;
    [SerializeField] private Transform treeLogHalf;
    [SerializeField] private Transform treeStump;

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
            case Type.LogHalf:
                {
                    healthAmount = 50;
                    break;
                }
            case Type.Stump:
                {
                    healthAmount = 50;
                    break;
                }

        }
        healthSystem = new HealthSystem(healthAmount);
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        switch (treeType)
        {
            default:
                break ;
            case Type.Tree:
                print("Spawn Log");
                //// Spawn FX
                //Instantiate (fxTreeDestroyed, transform.position, transform.rotation);

                ////spawn log
                //Vector3 treeLogOffset = transform.up * 0.8f;
                //Instantiate(treeLog, transform.position + treeLogOffset, Quaternion.Euler(Random.Range(-1.5f, +1.5f), 0, Random.Range(-1.5f, 1.5f)));

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
        Destroy(gameObject);
    }
    public void Damage(int amount)
    {
        healthSystem.Damage(amount);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.TryGetComponent<ITreeDamageable>(out ITreeDamageable treeDamageable))
        {
            if(collision.relativeVelocity.magnitude > 1f)
            {
                int damageAmount = Random.Range(5, 20);
                //DamagePopup.Create(collision.GetContact(0).point, damageAmount, damageAmount >14);
                treeDamageable.Damage(damageAmount);
            }
        }
    }
}
