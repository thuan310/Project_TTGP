using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] attackGrunts;

    [Header("Footsteps")]
    public AudioClip[] footSteps;
    public AudioClip[] footStepsDirt;
    public AudioClip[] footStepsStone;


    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PLaySoundFX(AudioClip soundFX, float volume =1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        audioSource.PlayOneShot(soundFX, volume);
        // Reset Pitch
        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }
    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
    }

    public void PlayerDamageGruntsSoundFX()
    {
        if (damageGrunts.Length>0)
        {
            PLaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
        }
    }

    public virtual void PlayAttackGruntSoundFX()
    {
        if (attackGrunts.Length > 0)
        {
            PLaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
        }
    }

    public virtual void PlayFootStepSoundFX()
    {
        if (footSteps.Length > 0)
        {
            PLaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(footSteps));
        }
    }
}
