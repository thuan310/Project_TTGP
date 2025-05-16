using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    protected CharacterManager character;

    [Header("Last attack animation performed")]
    public string lastAttackAnimationPerformed;

    [Header("Attack Target")]
    public CharacterManager currentTarget;

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (newTarget != null)
        {
            currentTarget = newTarget;

            //character.currentargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
        }
        else
        {
            currentTarget = null;
        }
    }

    public void EnableIsInvulnerable()
    {
        character.isInvulnerable.Value = true;
    }

    public void DisableIsInvulnerable()
    {
        character.isInvulnerable.Value = false;
    }
}
