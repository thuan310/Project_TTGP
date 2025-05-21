using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Actions/Attack action")]
public class AiCharacterAttackAction : ScriptableObject
{
    [Header("Attack")]
    [SerializeField] private string attackAnimation;

    [Header("Combo Action")]
    public AiCharacterAttackAction comboAction; // the combo actiuon opf this attack action

    [Header("Action Values")]
    [SerializeField] AttackType attackType;         // attack type
    public int attackWeight = 50;
    // attack can be repeated
    public float actionRecoveryTime = 1.5f; // the time before the chareacter can make another attack after perfroming this one
    public float minimumAttackAngle = -35;
    public float maximumAttackAngle = 35;
    public float minimumAttackDistance = 0;
    public float maximumAttackDistance = 2;


    public void AttemptToPerformAction(AICharacterManager aiCharacter)
    {
        aiCharacter.characterAnimatorManager.PlayTargetAttackActionAnimation(attackType,attackAnimation, true);

    }
}
