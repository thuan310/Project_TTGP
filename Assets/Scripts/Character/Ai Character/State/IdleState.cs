using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (!aiCharacter.navMeshAgent.enabled)
        {
            if(aiCharacter.isTalking)
            {
                return SwitchState(aiCharacter, aiCharacter.talking);
            }
            else
            {
                if (!aiCharacter.isPerformingAction)
                {
                    aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.idleAction, true, true);
                }
                // return this state, to continueally search for a target (keep the state here, until a target is found)
                return this;
            }
        }
        else
        {
            // return the pursue target state (change the state to the pursue target state)
            return SwitchState(aiCharacter, aiCharacter.searchingTarget);
        }
    }
}
