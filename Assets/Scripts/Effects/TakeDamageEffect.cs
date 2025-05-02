using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/TakeDamage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager CharacterCausingManager; //if the damage is caused by another characters attack it will be stored here

    [Header("Damage")]
    public float physicalDamage = 0; // in the future will be split into "Standard", Strike", " slah" and " pierce"
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage;
    public float holyDamage = 0;

    [Header("Final Damage")]
    public int finalDamageDealt = 0; // the damage the character takes after all calculations have been made

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false; // if a character's poise is broken, they will be stunned" and play damage animation


    // (to do) build ups
    //build up effect amounts

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX; // used on top of regular sfx if there is elemental damage present (magic/fire/lightning/holy)

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;                 //used to determine what damage animation to play (moveBackwards, to the left, to the right ect)
    public Vector3 contactPoint;                // used to determine where the blood fx instantiate


    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        //if the character is dead, no additional damage effects should be processed
        if (character.isDead.Value)
        {
            return;
        }

        //check for "Invelnerability"

        // calculate damage
        CalculateDamage(character);
        // check which directional damage came from
        //play a damage animation
        // check for build ups (poison, bleed ect)
        // play damage sound fx
        //play damage VFX(blood)

        // if character is A.I, check for new target if chartacter causing damage is present
    }

    private void CalculateDamage(CharacterManager character)
    {
        if(CharacterCausingManager != null)
        {
            // check for damage modifiers and modify base damage (Physical/ Elemental damage buff)
            // (physical *= physicalModifier ect
        }

        // check character for flat defenses and substract them from the damage

        // check character for armor absorptions, and substract the percentage from the damage

        // add all damage types together , and apply final damage

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if(finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        Debug.Log("Final Damage given: " + finalDamageDealt);
        character.currentHealth.Value -= finalDamageDealt;
        // calculate poise damage to determine if the character will be stunned
    }
}
