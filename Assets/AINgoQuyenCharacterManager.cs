using System.Collections;
using UnityEngine;

public class AINgoQuyenCharacterManager : AIBossCharacterManager
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

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        PlayerUIManager.instance.playerUIPopUpManager.SendBossDefeatedPopUp("Ngô Quyền fell");

        currentHealth.Value = 0;
        isDead.Value = true;

        bossFightIsActie.Value = false;

        foreach (var fogWall in fogWalls)
        {
            fogWall.isActive.Value = false;
        }

        // reset any flags here that need to be reset
        // nothing

        // if we are not grounded, play an aerial Death animation
        if (!manuallySelectDeathAnimation)
        {
            characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
        }

        hasBeenDefeated.Value = true;
        if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
        }
        else
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
            WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
            WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
        }
        WorldSaveGameManager.instance.SaveGame();

        SceneNavigationManager.instance.BringBackToPreviousPlace();
        EventManager.instance.dialogueEvents.EnterDialogue("chatGeneral2");

        //play some death SFX

        yield return new WaitForSeconds(5);

        // award players with runes

        // disable character
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(ProcessDeathEvent());
        }
    }

}
