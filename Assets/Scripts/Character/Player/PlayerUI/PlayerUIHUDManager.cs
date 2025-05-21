using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHUDManager : MonoBehaviour
{
    [Header("Stat Bars")]
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar staminaBar;

    [Header("Quick Slots")]
    [SerializeField] Image rightWeaponQuickSLotIcon;
    [SerializeField] Image leftWeaponQuickSLotIcon;

    [Header("Boss Health Bar")]
    public Transform bossHealthBarParent;
    public GameObject bossHealthBarObject;

    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true) ;
    }
    public void SetNewHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(Mathf.RoundToInt(newValue));
    }
    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }

    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }
    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina);
    }

    public void SetRightWeaponQuickSLotICon(int weaponID)
    {
        // 1. Method one, directly reference the right weapon in the hand of the player
        // pros: its super straight forward
        // cons: if you forget to call this function after you've loaded your weapons first, it will give you an error
        // example: you load a previously saved, you go to reference the weapons upon loading Ui but they arent instantiated yet
        // Final notes: this method is perfectly fine if you remember your order of your operations

        // 2. MEthod two, require an item ID of weapon, fetch the weapon from our database and use it to get the weapon items icon
        // pros: since you always save the current weapons ID, we dont need to wait to get it from the player we could get it before hand if required
        // cons: it's not as direct
        // final notes: this method is great if you don't want to remember another order of operations

        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);

        if(weapon == null)
        {
            Debug.Log("Item is null");
            rightWeaponQuickSLotIcon.enabled = false;
            rightWeaponQuickSLotIcon.sprite = null;
            return;
        }

        if(weapon.itemIcon == null)
        {
            Debug.Log("Item has no icon");
            rightWeaponQuickSLotIcon.enabled = false;
            rightWeaponQuickSLotIcon.sprite = null;
            return;
        }

        // this is where you would check to see if you meet the items requirements if you want to create the warning for not being able to wield it in the ui\

        rightWeaponQuickSLotIcon.sprite = weapon.itemIcon;
        rightWeaponQuickSLotIcon.enabled = true;

    }

    public void SetLeftWeaponQuickSLotICon(int weaponID)
    {
        // 1. Method one, directly reference the right weapon in the hand of the player
        // pros: its super straight forward
        // cons: if you forget to call this function after you've loaded your weapons first, it will give you an error
        // example: you load a previously saved, you go to reference the weapons upon loading Ui but they arent instantiated yet
        // Final notes: this method is perfectly fine if you remember your order of your operations

        // 2. MEthod two, require an item ID of weapon, fetch the weapon from our database and use it to get the weapon items icon
        // pros: since you always save the current weapons ID, we dont need to wait to get it from the player we could get it before hand if required
        // cons: it's not as direct
        // final notes: this method is great if you don't want to remember another order of operations

        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);

        if (weapon == null)
        {
            Debug.Log("Item is null");
            leftWeaponQuickSLotIcon.enabled = false;
            leftWeaponQuickSLotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("Item has no icon");
            leftWeaponQuickSLotIcon.enabled = false;
            leftWeaponQuickSLotIcon.sprite = null;
            return;
        }

        // this is where you would check to see if you meet the items requirements if you want to create the warning for not being able to wield it in the ui\

        leftWeaponQuickSLotIcon.sprite = weapon.itemIcon;
        leftWeaponQuickSLotIcon.enabled = true;

    }
}
