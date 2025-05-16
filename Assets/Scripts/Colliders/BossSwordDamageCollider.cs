using UnityEngine;

public class BossSwordDamageCollider : DamageCollider
{
    [SerializeField] AIBossCharacterManager bossCharacter;

    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        bossCharacter = GetComponentInParent<AIBossCharacterManager>();
    }
    protected override void DamageTarget(CharacterManager damgageTarget)
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
        damageEffect.angleHitFrom = Vector3.SignedAngle(bossCharacter.transform.forward, damgageTarget.transform.forward, Vector3.up);

        // options 01:
        // this will apply damage if the a.i hits its target on the hosts side regaradless of how it looks on any other client side

        // option 02
        // this will apply damage if the a.i hits its target on the connected characters side regardless of how its looks on any other clients side
        damgageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);


    }
}
