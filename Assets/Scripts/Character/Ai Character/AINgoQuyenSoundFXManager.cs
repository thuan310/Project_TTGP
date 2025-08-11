using UnityEngine;

public class AINgoQuyenSoundFXManager : CharacterSoundFXManager
{
    [Header("Whooses")]
    [SerializeField] public AudioClip[] swordWhooshes;

    [Header("Sword Impacts")]
    [SerializeField] public AudioClip[] swordImpacts;
    [Header("Stomp Impactws")]
    [SerializeField] public AudioClip[] stompImpacts;

    public virtual void PlaySwordImpactSoundFX()
    {
        if (swordImpacts.Length > 0)
        {
            PLaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(swordImpacts));
        }
    }

    public virtual void PlayStompImpactSoundFX()
    {
        if (stompImpacts.Length > 0)
        {
            PLaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(stompImpacts));
        }
    }
}
