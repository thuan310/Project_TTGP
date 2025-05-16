using UnityEngine;
using UnityEngine.Rendering;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AICharacterManager aiCharacterManager)
    {
        //Debug.Log("We are running this state");

        // do some logic to find the player

        // if we have found the player, return the pirsue target state instead

        // if we have not found the player, continue to return the idle state

        return this;
    }

    protected virtual AIState SwitchState(AICharacterManager aICharacter, AIState newState)
    {
        ResetStateFlags(aICharacter);
        return newState;
    }

    protected virtual void ResetStateFlags(AICharacterManager aICharacter)
    {
        // reset any state flags here so when you return to the state, they are blank once again
    }
}
