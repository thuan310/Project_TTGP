using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;

    public WeaponItem currentWeaponBeingUsed;

    [Header("Flags")]
    public bool canComboWithMainHandWeapon = false;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
        
        // perform the action
        weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

        weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(weaponAction.actionID);

        if (weaponAction != null)
        {
            weaponAction.AttemptToPerformAction(player, WorldItemDatabase.instance.GetWeaponByID(weaponPerformingAction.itemID));
        }
        else
        {
            Debug.LogError("Action is null, cannot be performed");
        }

    }

    public virtual void DrainStaminaBaseOnAttack()
    {
        float staminaDeducted = 0f;

        if (currentWeaponBeingUsed == null)
            return;

        switch(currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            default:
                break;
        }

        //Debug.Log("Stamina Drained: " +staminaDeducted);
        player.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        PlayerCamera.instance.SetLockCameraHeight();
    }

}
