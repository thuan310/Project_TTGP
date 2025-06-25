using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/SearchingPlayer")]
public class SearchingTargetState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.characterCombatManager.currentTarget != null)
        {
            // return the pursue target state (change the state to the pursue target state)
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }
        else
        {
            // return this state, to continueally search for a target (keep the state here, until a target is found)
            aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);
            return this;
        }
    }
}
