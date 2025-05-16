using UnityEngine;

public class AIDurkCharacterManager : AIBossCharacterManager
{
    // why give durk his own character manager?
    // our character managers act as a hub to where we can reference all components of a character
    // a "player" manager for example will have all the unique components of a player character
    // an undead will have all the unique components of an undead
    // since durk has his own SFX (Club, stomp) that are unique to his character only, we created a durk sfx manager
    // and to reference this new sfx manager, and to keep our design pattern the same, we need a durk character manager to reference it from

     public AIDurkSoundFXManager durkSoundFXManager;
    [HideInInspector] public AIDurkCombatManager durkCombatManager;
    protected override void Awake()
    {
        base.Awake();

        durkSoundFXManager = GetComponent<AIDurkSoundFXManager>();
        durkCombatManager = GetComponent<AIDurkCombatManager>();
    }
}
