using UnityEngine;

public class WeaponItem : Item
{
    // Animator Controller Override (change attack animations based on weapon you are currently using)

    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base DAmage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;

    // Weapon guard absorptions (blocking power)

    [Header("Weapon base Poise Damage")]
    public float poiseDamage = 10;
    // Offensive poise bonus when attacking

    [Header("Attack Modifier")]
    // Weapon Modifiers
    // Light Attack modifier
    public float light_Attack_01_Modifier = 1.1f;
    public float light_Attack_02_Modifier = 1.3f;
    public float light_Attack_03_Modifier = 1.5f;
    // Heavy Attack modifier
    public float heavy_Attack_01_Modifier = 1.4f;
    public float heavy_Attack_02_Modifier = 1.6f;

    public float charges_Attack_01_Modifier = 2.0f;
    public float charges_Attack_02_Modifier = 2.5f;
    // Critical Damage Modifier ect

    [Header("Stamina Costs Modifier")]
    public int baseStaminaCost = 20;
    // Running Attack stamina cost modifier
    // Light Attack stamina cost modifier
    public float lightAttackStaminaCostMultiplier = 0.9f;
    // Heavy Attack Stamina cost modifier

    // Item based Actions(RB,ET,LB,LT)
    [Header("Actions")]
    public WeaponItemAction oh_RB_Action;   // one hand right bumper action
    public WeaponItemAction oh_RT_Action;   // one hand right trigger action


    // ASh of War

    // Blocking sounds
    [Header("Whooshes")]
    public AudioClip[] whooses;
}
