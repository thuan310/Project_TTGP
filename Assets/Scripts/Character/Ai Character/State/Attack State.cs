using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Attack")]
public class AttackState : AIState
{
    [Header("Current Attack")]
    [HideInInspector] public AiCharacterAttackAction currentAttack;
    [HideInInspector] public bool willPerformCOmbo = false;

    [Header("State Flags")]
    protected bool hasPerformedAttack = false;
    protected bool hasPerformedCombo = false;

    [Header("Pivot After Attack")]
    [SerializeField] protected bool pivotAfterAttack = false;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if(aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
            return SwitchState(aiCharacter,aiCharacter.idle);

        // rotate toward the target whilst attacking
        aiCharacter.aiCharacterCombatManager.RotateTowardTargetWhilstAttacking(aiCharacter);

        
        // set movement values to 0
        aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0,0,false);

        // perform a combo
        if(willPerformCOmbo && hasPerformedCombo)
        {
            if (currentAttack.comboAction != null)
            {
                // if can combo
                //hasPerformedCombo = true;
                //currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
            }
        }

        if (aiCharacter.isPerformingAction)
            return this;

        if (!hasPerformedAttack)
        {
            // if we are still recovering from ac action, wait before performing another
            if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
                return this;
            //Debug.Log("2");
            PerformAttack(aiCharacter);

            // return to the top, so if we have combo we process that when we are able
            return this;
        }

        if(pivotAfterAttack)
            aiCharacter.aiCharacterCombatManager.PivotTowardTarget(aiCharacter);

        return SwitchState(aiCharacter, aiCharacter.combatStance);
    }

    protected void PerformAttack(AICharacterManager aiCharacter)
    {

        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(aiCharacter);
        aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
    }

    protected override void ResetStateFlags(AICharacterManager aICharacter)
    {
        base.ResetStateFlags(aICharacter);

        hasPerformedAttack = false;
        hasPerformedCombo = false;
    }
}
