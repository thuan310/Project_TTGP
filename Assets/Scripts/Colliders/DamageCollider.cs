using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Damage")]
    public float physicalDamage = 0; // in the future will be split into "Standard", Strike", " slah" and " pierce"
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage;
    public float holyDamage = 0;

    [Header("Contact Point")]
    private Vector3 contactPoint;

    [Header("Character Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    private void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();

        if(damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            // check if we can damage this target based on frindly fire

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
}
