using UnityEngine;

public class PlayerEffectManager : CharacterEffectManager
{
    [Header(" Debug Delete Later")]
    [SerializeField] InstantCharacterEffect effectToTest;
    [SerializeField] bool processEffect = false;

    private void Update()
    {
        if (processEffect)
        {
            processEffect = false;
            //When we instantiate the original is not effected
            InstantCharacterEffect effect = Instantiate(effectToTest);

            // When we dont intantiate it, the original is changed (you do not want this in most cases)
            //effectToTest.staminaDamage = 55;
            ProcessInstantEffect(effect);
        }
    }
}
