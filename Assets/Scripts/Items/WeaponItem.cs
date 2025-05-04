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

    // Weapon Modifiers
    // Light Attack modifier
    // Heavy Attack modifier
    // Critical Damage Modifier ect

    [Header("Stamina Costs")]
    public int baseStaminaCost = 20;
    // Running Attack stamina cost modifier
    // Light Attack stamina cost modifier
    // Heavy Attack Stamina cost modifier

    // Item based Actions(RB,ET,LB,LT)

    // ASh of War

    // Blocking sounds
}
