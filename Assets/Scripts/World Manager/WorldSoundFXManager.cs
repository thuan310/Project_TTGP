using System.Collections;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Boss Track")]
    [SerializeField] AudioSource bossIntroPlayer;
    [SerializeField] AudioSource bossLoopPlayer;

    [Header("Damage Sounds")]
    public AudioClip[] physicalDamageSFX;

    [Header("Action Sounds")] 
    public AudioClip rollSFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayeBossTrack(AudioClip introTrack, AudioClip loopTrack)
    {
        bossIntroPlayer.volume = 1;
        bossIntroPlayer.clip = introTrack;
        bossIntroPlayer.loop = false;
        bossIntroPlayer.Play();

        bossLoopPlayer.volume = 1;
        bossLoopPlayer.clip = loopTrack;
        bossLoopPlayer.loop = true;
        bossLoopPlayer.PlayDelayed(bossIntroPlayer.clip.length);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }

    public AudioClip ChoosRandomFootStepSoundBasedOnGround(GameObject steepedOnObject, CharacterManager character)
    {
        if(steepedOnObject.tag == "Dirt")
        {
            return ChooseRandomSFXFromArray(character.characterSoundFXManager.footStepsDirt);
        }
        else if (steepedOnObject.tag == "Stone")
        {
            return ChooseRandomSFXFromArray(character.characterSoundFXManager.footStepsStone);
        }

        return null;
    }

    public void StopBossMusic()
    { 
        StartCoroutine(FadeOutBossMusicThenStop());
    }
    private IEnumerator FadeOutBossMusicThenStop()
    {
        bossIntroPlayer.Stop();

        while(bossLoopPlayer.volume >0)
        {
            bossLoopPlayer.volume -= Time.deltaTime;
            bossLoopPlayer.volume -= Time.deltaTime;
            yield return null;
        }

        bossIntroPlayer.Stop();
        bossLoopPlayer.Stop();
    }
}
