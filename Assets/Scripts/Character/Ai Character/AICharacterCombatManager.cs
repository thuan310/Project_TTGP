using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    protected AICharacterManager aICharacter;

    [Header("Action Recovery")]
    public float actionRecoveryTimer = 0;

    [Header("Pivot")]
    public bool enablePivot = true;

    [Header("Target Information")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetsDirection; 

    [Header("Detection")]
    [SerializeField] float detectionRadius = 30;
    public float minimumFOV = -35;
    public float maximumFOV = 35;

    [Header("Attack Rotation Speed")]
    public float attackRotationSpeed =25;

    protected override void Awake()
    {
        base.Awake();

        aICharacter = GetComponent<AICharacterManager>();
        lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
    }
    public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
    {
        if (currentTarget != null)
            return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if(targetCharacter == null)
                continue;

            if(targetCharacter == aiCharacter)
                continue;

            if(targetCharacter.isDead.Value)
                continue;

            // can i attack this character, if so, make them my target

            if (WorldUtilityManager.instance.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup))
            {
                // if a potential target is found, it has to be infront of us
                Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                if(angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget <maximumFOV)
                {
                    // Lastly, we check for enviro blocks
                    if(Physics.Linecast(
                        aiCharacter.characterCombatManager.lockOnTransform.position, 
                        targetCharacter.characterCombatManager.lockOnTransform.position, 
                        WorldUtilityManager.instance.GetEnviroLayers()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position,Color.red);
                        Debug.Log("blocked");
                    }
                    else
                    {
                        targetsDirection= targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetsDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                        
                        if(enablePivot)
                        {
                            PivotTowardTarget(aiCharacter);
                        }
                    }
                }
            }
           
        }
    }

    public void PivotTowardTarget(AICharacterManager aiCharacter)
    {
        // play a pivot aniamtion depending on viewable angle of target
        if(aiCharacter.isPerformingAction) 
            return;

        //if (viewableAngle >= 20 && viewableAngle <= 60)
        //{
        //    aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_45", true);
        //}
        //else if (viewableAngle <= -20 && viewableAngle >= -60)
        //{
        //    aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_45", true);
        //}
        else if (viewableAngle >= 20 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
        }
        else if (viewableAngle <= -20 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
        }
        //else if (viewableAngle >= 110 && viewableAngle <= 145)
        //{
        //    aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_135", true);
        //}
        //else if (viewableAngle <= -110 && viewableAngle >= -145)
        //{
        //    aICharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_135", true);
        //}
        else if (viewableAngle >= 110 && viewableAngle <= 189)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
        }
        else if (viewableAngle <= -110 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180", true);
        }
    }

    public void RotateTowardAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isMoving.Value)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }

    public void RotateTowardTargetWhilstAttacking(AICharacterManager aiCharacter)
    {
        if (currentTarget == null)
            return;

        //  1. check if we can rotate
        if(!aiCharacter.canRotate)
            return;

        if (!aiCharacter.isPerformingAction)
            return;

        // 2. rotate towards the target a specifed rotation speed during specified frames
        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0;   
        targetDirection.Normalize();

        if(targetDirection == Vector3.zero)
            targetDirection = aiCharacter.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
        //print("da rotate");
    }

    public void HandleActionRecovery(AICharacterManager aiCharacter)
    {
        if(actionRecoveryTimer > 0)
        {
            if(!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }

}
