using UnityEngine;

public class CharacterEffectManager : MonoBehaviour
{
    // process instant effects (take damage, heal)

    // process timed effects( poison, build ups)

    // preocess static effect (adding/ removing vuffs from tailsmans ect)
    CharacterManager character;

    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterVFX;

    private void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        effect.ProcessEffect(character);
    }

    public void PLayBloodSplatter(Vector3 contactPoint)
    {
        //print("phun mau");
        // if we manually have placed a blood splatter vfx on this model, play its version
        if (bloodSplatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        //else, use  the generic (default version) we have elsewhere
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }
}
