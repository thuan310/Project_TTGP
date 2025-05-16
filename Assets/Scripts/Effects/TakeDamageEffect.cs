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

        //check for "Invelnerability"
        if (character.isInvulnerable.Value)
            return;
        base.ProcessEffect(character);

        //if the character is dead, no additional damage effects should be processed
        if (character.isDead.Value)
        {
            return;
        }

        // calculate damage
        CalculateDamage(character);
        // check which directional damage came from
        PlayDirectionalBasedDamageAnimation(character);
        //play a damage animation
        // check for build ups (poison, bleed ect)
        // play damage sound fx
        PlayDamageSFX(character);
        //play damage VFX(blood)
        PlayDamageVFX(character);


        // if character is A.I, check for new target if chartacter causing damage is present
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (CharacterCausingManager != null)
        {
            // check for damage modifiers and modify base damage (Physical/ Elemental damage buff)
            // (physical *= physicalModifier ect
        }

        // check character for flat defenses and substract them from the damage

        // check character for armor absorptions, and substract the percentage from the damage

        // add all damage types together , and apply final damage

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        Debug.Log("Final Damage given: " + finalDamageDealt);
        if (character.isDummy == true)
        {
            finalDamageDealt = 1;
        }
        character.currentHealth.Value -= finalDamageDealt;
        // calculate poise damage to determine if the character will be stunned
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        // If we have fire damage, play fire particles
        // lightning damage, lightning particle ect
        character.characterEffectManager.PLayBloodSplatter(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
{
    AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

    character.characterSoundFXManager.PLaySoundFX(physicalDamageSFX);
    character.characterSoundFXManager.PlayerDamageGruntsSoundFX();    
    // if fire damage is greater than 0, play burn sfx
    // if lightning damage is greate than 0, play zap sfx
}

    private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {

        // calculate if poise is broken
        poiseIsBroken = true;

        if (character.isDead.Value)
            return;



        if (angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            //Debug.Log(character.characterAnimatorManager.forward_Medium_Damage.Count);
            // PLay Front Animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (angleHitFrom <= -145 && angleHitFrom >= -180)
        {
            //Debug.Log(character.characterAnimatorManager.forward_Medium_Damage.Count);
            // PLay Front Animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            //Debug.Log(character.characterAnimatorManager.backward_Medium_Damage.Count);
            // PLay Back Animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            //Debug.Log(character.characterAnimatorManager.left_Medium_Damage.Count);
            // PLay Left Animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            //Debug.Log(character.characterAnimatorManager.right_Medium_Damage.Count);
            // play Right Animation
            damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
        }

        // if poise is broken, play a staggering damage animation
        if (poiseIsBroken)
        {
            character.characterAnimatorManager.lastDamagAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
        }
    }

}

