using UnityEngine;

public class CharacterFootstepSFXMaker : MonoBehaviour
{
    CharacterManager character;

    AudioSource audioSource;
    GameObject steppedOnObject;

    private bool hasTouchedGround = false;
    private bool hasPlayedFootStepSFX = false;
    [SerializeField] float distanceToGround = 0.55f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        character = GetComponent<CharacterManager>();   
    }

    private void FixedUpdate()
    {
        CheckForFootSteps();
    }

    private void CheckForFootSteps()
    {
        if(character == null)
            return;

        if(!character.isMoving.Value)
            return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, distanceToGround, WorldUtilityManager.instance.GetEnviroLayers()))
        {
            hasTouchedGround = true;

            if(!hasPlayedFootStepSFX)
                steppedOnObject = hit.transform.gameObject;
        }
        else
        {
            hasTouchedGround = false;
            hasPlayedFootStepSFX = false ;
            steppedOnObject = null;
        }

        if(hasTouchedGround && !hasPlayedFootStepSFX)
        {
            hasPlayedFootStepSFX =true ;
            PlayFootStepSoundFX();
        }
    }

    private void PlayFootStepSoundFX()
    {
        // here you could play a different sfx depending on the layer of the ground or a tag or such(snow, wood, stone ect)
        // method 1
        //audioSource.PlayOneShot(WorldSoundFXManager.instance.ChoosRandomFootStepSoundBasedOnGround(steppedOnObject, character));

        // method 2
        character.characterSoundFXManager.PlayFootStepSoundFX();
    }

}
