using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField] protected Collider damageCollider;


    [Header("Damage")]
    public float physicalDamage = 0; // in the future will be split into "Standard", Strike", " slah" and " pierce"
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage;
    public float holyDamage = 0;

    [Header("Contact Point")]
    public Vector3 contactPoint;

    [Header("Character Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();
    protected CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponentInParent<CharacterManager>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            // check if we can damage this target based on frindly fire
            if (!WorldUtilityManager.instance.CanIDamageThisTarget(character.characterGroup, damageTarget.characterGroup))
            {
                return;
            }

                // check if target is blocking

                //check if target is vulnerable

                // damage
                DamageTarget(damageTarget);
        }
    }

    protected virtual void DamageTarget(CharacterManager damgageTarget)
    {
        // we don't want to damage the same target more than once in a single attack
        // so we add them to a list that checks before applying damage
        if (charactersDamaged.Contains(damgageTarget))
        {
            return;
        }
        charactersDamaged.Add(damgageTarget);
        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.contactPoint = contactPoint;

        damgageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);
    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        charactersDamaged.Clear(); // we reset the characters that have been hit when we reset the collider, so they may be hit again
    }
}
