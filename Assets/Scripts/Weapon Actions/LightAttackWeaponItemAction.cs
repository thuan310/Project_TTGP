using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]

public class LightAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string light_Attack_01 = "Main_Light_Attack_01"; // Main = main hand, 
    [SerializeField] string light_Attack_02 = "Main_Light_Attack_02"; // Main = main hand, 
    [SerializeField] string light_Attack_03 = "Main_Light_Attack_03"; // Main = main hand, 
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        // Check for stops

        if(playerPerformingAction.currentStamina.Value <=0)
            return;

        if(!playerPerformingAction.isGrounded)
            return ;

        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        
    }
    
    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // if we are attacking currently, and we can combo, perform the combo attack
        if(playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;
            
            // perform an attack based on the pervious attack we just played
            if(playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02, light_Attack_02,true);
                return ;
            }
            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_02)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack03, light_Attack_03, true);
                return;
            }
            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_03)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
                return;
            }
        }
        //otherwise, if we are not already attacking just per  form a regular attack
        else if(!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
        }
    }
}
