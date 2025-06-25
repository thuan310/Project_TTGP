using UnityEngine;


[CreateAssetMenu(menuName = "A.I/States/TalkingState")]
public class TalkingState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isTalking)
        {
            if (!aiCharacter.isPerformingAction)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.talkingAction, true, true);
            }
            // return this state, to continueally search for a target (keep the state here, until a target is found)
            return this;
        }
        else
        {
            // return the pursue target state (change the state to the pursue target state)
            return SwitchState(aiCharacter, aiCharacter.idle);
        }
    }
}
