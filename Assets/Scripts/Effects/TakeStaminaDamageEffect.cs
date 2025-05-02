using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
public class TakeStaminaDamageEffect : InstantCharacterEffect
{
    public float staminaDamage;

    public override void ProcessEffect(CharacterManager character)
    {
        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        // compared the base stamina damage against other player effects/modifiers
        // change the value before subtracting/adding it
        // play sound fx or vfx during effect
        Debug.Log("Character is taking" + staminaDamage + " Stamina Damage");
        character.currentStamina.Value -= staminaDamage;
    }
}
