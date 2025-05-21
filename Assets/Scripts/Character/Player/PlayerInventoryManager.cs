using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] weaponInRightHandSLots = new WeaponItem[3];
    public int rightHandWeaponIndex = 0;
    public WeaponItem[] weaponInLeftHandSLots = new WeaponItem[3];
    public int leftHandWeaponIndex = 0;
}
