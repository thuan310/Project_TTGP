using UnityEngine;
using UnityEngine.TextCore.Text;

public class AIPhanDienCombatManager : AICharacterCombatManager
{
    AIPhanDienCharacterManager phanDienManager;

    [Header("Damage Collider")]
    [SerializeField] BossSwordDamageCollider vikingSwordDamageCollider;
    [SerializeField] PhanDienStompCollider stompCollider;
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
        phanDienManager = GetComponent<AIPhanDienCharacterManager>();
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
        phanDienManager.characterSoundFXManager.PLaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(phanDienManager.phanDienSoundFXManager.swordWhooshes));
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