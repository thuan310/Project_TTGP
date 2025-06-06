using UnityEngine;

public class AIDurkCombatManager : AICharacterCombatManager
{
    AIDurkCharacterManager durkManager;

    [Header("Damage Collider")]
    [SerializeField] BossSwordDamageCollider vikingSwordDamageCollider;
    [SerializeField] DurkStompCollider stompCollider;
    [SerializeField] Transform durkStompingFoot;
    public float stompAttackAOERadius;

    [Header("Damage")]
    [SerializeField] int baseDamage = 25;
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;
    [SerializeField] float attack03DamageModifier = 1.6f;
    public float stompDamage = 25;

    [Header("VFX")]
    public GameObject durkImpactVFX;


    protected override void Awake()
    {
        base.Awake();
        durkManager = GetComponent<AIDurkCharacterManager>();
    }
    public void SetAttack01Damage()
    {
        aICharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        vikingSwordDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        aICharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        vikingSwordDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
    }

    public void SetAttack03Damage()
    {
        aICharacter.characterSoundFXManager.PlayAttackGruntSoundFX();
        vikingSwordDamageCollider.physicalDamage = baseDamage * attack03DamageModifier;
    }

    public void OpenVikingSwordHandDamageCollider()
    {
        vikingSwordDamageCollider.EnableDamageCollider();
        durkManager.characterSoundFXManager.PLaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(durkManager.durkSoundFXManager.swordWhooshes));
    }

    public void DisableVikingSwordDamageCollider()
    {
        vikingSwordDamageCollider.DisableDamageCollider();
    }

    public void ActivateDurkStomp()
    {
        stompCollider.StompAttack();
    }
}
