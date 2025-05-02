using UnityEngine;

public class CharacterEffectManager : MonoBehaviour
{
    // process instant effects (take damage, heal)

    // process timed effects( poison, build ups)

    // preocess static effect (adding/ removing vuffs from tailsmans ect)
    CharacterManager character;

    private void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        effect.ProcessEffect(character);
    }
}
