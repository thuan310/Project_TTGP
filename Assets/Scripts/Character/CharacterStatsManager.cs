using Unity.VisualScripting;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Stamina Regeneration")]
    [SerializeField] float staminaRegenarationAmount = 2f;
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected virtual void Start()
    {

    }
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public int CalculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = 0;

        // Create an equation for how you want your stamina to be calculated

        health = vitality * 10;
        return Mathf.RoundToInt(health);
    }

    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;

        // Create an equation for how you want your stamina to be calculated

        stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina()
    {
        // we do want to regenerate stamina if we are using it
        if (character.isSprinting.Value)
            return;

        if (character.isPerformingAction)
            return;

        staminaRegenerationTimer += Time.deltaTime;

        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            if (character.currentStamina.Value < character.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1f)
                {
                    staminaTickTimer = 0f;
                    character.currentStamina.Value += staminaRegenarationAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
    {
        //We only want to reset the regeneration if the action used stamina
        // we dont want to reset the regeneration if we are already regenerating stamina
        if(currentStaminaAmount<previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
        
    }
}
