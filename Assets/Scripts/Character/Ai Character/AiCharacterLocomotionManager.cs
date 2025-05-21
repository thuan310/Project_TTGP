using UnityEngine;

public class AiCharacterLocomotionManager : CharacterLocomotionManager
{
    public void RotateTowardAgent(AICharacterManager aICharacter)
    {
        if (aICharacter.isMoving.Value)
        {
            aICharacter.transform.rotation = aICharacter.navMeshAgent.transform.rotation;
        }
    }
}
