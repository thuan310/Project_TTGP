using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
public class PursueTargetState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        // check if we are performing an action (if so do nothing until action is complete)
        if (aiCharacter.isPerformingAction)
            return this;

        // check if our target is null, if we do not have a target. return to idle state
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        // make sure our navmesh agent is active, if its not enable it
        if (aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        // if our target goes outside of the characters F.O.V, pivot to fact them
        if (aiCharacter.aiCharacterCombatManager.enablePivot)
        {
            if (aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumFOV
          || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumFOV)
                aiCharacter.aiCharacterCombatManager.PivotTowardTarget(aiCharacter);
        }

        aiCharacter.aiCharacterLocomotionManager.RotateTowardAgent(aiCharacter);

        // if we are within combat range of a target, switch state to combat stance state
        // option 1
        //if (aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.combatStance.maximumEngagementDistance)
        //    return SwitchState(aiCharacter,aiCharacter.combatStance);

        // option 2
        if ( aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance)
            return SwitchState(aiCharacter, aiCharacter.combatStance);

        // if the target is not reachable, and they are far away. return home

        // pursue the target 

        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);
        //Debug.Log(aiCharacter.navMeshAgent.isOnNavMesh);


        return this;
    }
}
