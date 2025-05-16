using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeWeaponDamageCollider;

    private void Awake()
    {
        meleeWeaponDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon,WeaponItem weapon)
    {
        meleeWeaponDamageCollider.characterCausingManager = characterWieldingWeapon;
        meleeWeaponDamageCollider.physicalDamage = weapon.physicalDamage;
        meleeWeaponDamageCollider.magicDamage = weapon.magicDamage;
        meleeWeaponDamageCollider.fireDamage = weapon.fireDamage;
        meleeWeaponDamageCollider.lightningDamage = weapon.lightningDamage;
        meleeWeaponDamageCollider.holyDamage = weapon.holyDamage;

        meleeWeaponDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
        meleeWeaponDamageCollider.light_Attack_02_Modifier = weapon.light_Attack_02_Modifier;
        meleeWeaponDamageCollider.light_Attack_03_Modifier = weapon.light_Attack_03_Modifier;

        meleeWeaponDamageCollider.heavy_Attack_01_Modifier = weapon.heavy_Attack_01_Modifier;
        meleeWeaponDamageCollider.heavy_Attack_02_Modifier = weapon.heavy_Attack_02_Modifier;

        meleeWeaponDamageCollider.charge_Attack_01_Modifier = weapon.charges_Attack_01_Modifier;
        meleeWeaponDamageCollider.charge_Attack_02_Modifier = weapon.charges_Attack_02_Modifier;
    }
}
