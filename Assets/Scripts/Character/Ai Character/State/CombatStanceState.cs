using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Combat Stance")]
public class CombatStanceState : AIState
{
    // 1. select an attack for the attack state, depending on distance and angle of target in relation to character
    // 2. process any combat logic here whilst waiting to attack (blocking, strafing, dodging ect)
    // 3. if target moves out of combat range, switch to pursue target
    // 4. if target is no logner present, switch to idle state

    [Header("Attacks")]
    public List<AiCharacterAttackAction> aiCharacterAttacks;    // alist of all possible attacks this character can do
    protected List<AiCharacterAttackAction> potentialAttacks;      // a list that is created during this state, all attacks possible in this situation (based on angle distance ect)
    private AiCharacterAttackAction choosenAttack;
    private AiCharacterAttackAction previousAttack;
    protected bool hasAttack = false;


    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false;    // if the character can perform a combo attack, after the initial attack
    [SerializeField] protected int chanceToPerformCombo = 25;   // the chance (int percent) of the character to perform a combo on the next attack
    protected bool hasRolledForComboChance = false;      // if we have already for the chance during this state

    [Header("Engagement Distance")]
    [SerializeField] public float maximumEngagementDistance = 5; // the distance we have to be away from the target before we enter the pursue target state

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;
            
        if(!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        // if you want the ai character to face and turn towards its target when its outside it's fove include this
        if(aiCharacter.aiCharacterCombatManager.enablePivot)
        {
            if (!aiCharacter.isMoving.Value)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                    aiCharacter.aiCharacterCombatManager.PivotTowardTarget(aiCharacter);
            }
        }

       

        // rotate to face our target
        aiCharacter.aiCharacterCombatManager.RotateTowardAgent(aiCharacter);

        // if our target is no longer present, switch back idle
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        // if we do not have an attack, get one
        if(!hasAttack)
        {
            GetNewAttack(aiCharacter);
        }
        else
        {
            // check recvery timer
            // pass attack to attack state
            aiCharacter.attack.currentAttack = choosenAttack;
            // roll for combo chacne 
            // switch state
            return SwitchState(aiCharacter, aiCharacter.attack);
        }

        // if we are outside of the combat engagement distance, switch to pursue target state
        if(aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);


        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this;
    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
        potentialAttacks = new List<AiCharacterAttackAction>();

        // 1. sort through all possible attacks
        foreach (var potentialAttack in aiCharacterAttacks)
        {
            // 2. remove attacks that cant be used in this situation (based on angle and distance)
            // if we are too close for this attack. check the next
            if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;

            // if we are too far for this attack , check the next
            if(potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;

            // if the target is outside minimum field of view for this attack, check the next
            if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            // if the target is outside maximum field of view for this attack, check the next
            if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            // 3. place remaining atacks into a list
            potentialAttacks.Add(potentialAttack);
        }
        if (potentialAttacks.Count <= 0)
            return;

        // 4. pick one of the remaining attacks randomly, based on weight
        var totalWeight = 0;

        foreach(var attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;
        }

        var randomWeightValue = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if(randomWeightValue <= processedWeight)
            {
                // 5. select this attack and pass it to the attack state
                // this is our attack
                choosenAttack = attack;
                previousAttack = choosenAttack;
                hasAttack = true;
                return;
            }
        }
    }

    protected virtual bool ROllForOutcomeChance(int outcomeChance)
    {
        bool outcomeWillBePerformed = false;

        int randomPercentage = Random.Range(0, 100);

        if(randomPercentage < outcomeChance)
        { 
            outcomeWillBePerformed = true;
        }
        return outcomeWillBePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager aICharacter)
    {
        base.ResetStateFlags(aICharacter);

        hasAttack = false;
        hasRolledForComboChance = false;
    }



}
