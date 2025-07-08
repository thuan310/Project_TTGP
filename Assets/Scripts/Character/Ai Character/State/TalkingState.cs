using UnityEngine;


[CreateAssetMenu(menuName = "A.I/States/TalkingState")]
public class TalkingState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isTalking)
        {
            Vector3 direction = (aiCharacter.aiCharacterCombatManager.currentTarget.transform.position) - aiCharacter.transform.position;

            // Optional: zero out the y-axis if you only want horizontal rotation
            direction.y = 0f;

            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, Time.deltaTime * 5f);
            if (!aiCharacter.isPerformingAction)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.talkingAction, true, true);
            }
            // return this state, to continueally talking to a target (keep the state here, until a target is found)
            return this;
        }
        else
        {
            // return the idle state (change the state to the idle state)
            return SwitchState(aiCharacter, aiCharacter.idle);
        }
    }
}
