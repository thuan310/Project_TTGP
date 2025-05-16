using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager instance;

    [Header("Layers")]
    [SerializeField] LayerMask characterLayer;
    [SerializeField] LayerMask envoriLayers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LayerMask GetCharacterLayers()
    {
        return characterLayer;
    }

    public LayerMask GetEnviroLayers()
    {
        return envoriLayers;
    }

    public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
    {
        if(attackingCharacter == CharacterGroup.Team_01)
        {
            switch(targetCharacter)
            {
                case CharacterGroup.Team_01: return false;
                case CharacterGroup.Team_02: return true;
                default:
                    break;
            }
        }
        else if(attackingCharacter == CharacterGroup.Team_02)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.Team_01: return true;
                case CharacterGroup.Team_02: return false;
                default:
                    break;
            }
        }

        return false;
    }

    public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
    {
        targetsDirection.y = 0;
        float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

        if(cross.y <0)
            viewableAngle = - viewableAngle;

        return viewableAngle;
    }
}
