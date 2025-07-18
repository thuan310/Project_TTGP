using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIBossCharacterManager : AICharacterManager
{
    // give this A.I unique ID
    public int bossID = 0;

    [Header("Music")]
    [SerializeField] AudioClip bossIntroClip;
    [SerializeField] AudioClip bossBattleLoopClip;

    [Header("Status")]
    public Observable<bool> bossFightIsActie;
    public Observable<bool> hasBeenDefeated ;
    public Observable<bool> hasBeenAwakened ;
    [SerializeField] List<FogWallInteractable> fogWalls;
    [SerializeField] string sleepAnimation;
    [SerializeField] string awakenAnimation;

    [Header("Phase Shift")]
    public float minimumHealthPercentageToShift = 50;
    [SerializeField] string phaseShiftAniamtion = "Change_Phase_01";
    [SerializeField] CombatStanceState phase02CombatStanceState;

    [Header("States")]
    [SerializeField] SleepState sleepState;

    // when this A.I is spawned, check our save file (dictionary)

    // if the save file does not contain a boss monster with is I.D add it
    // if it is present, check if the boss has been defeated
    // if the boss has been defeated, disable this gameobject
    // if the boss has not been defeated, allow this object to continue to be active

    //protected override void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape) && !isDummy)
    //    {
    //        //currentHealth.Value = 0;
    //    }
    //}

    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Start()
    {
        base.Start();

        bossFightIsActie.OnValueChanged += OnBossFightIsActiveChanged;
        OnBossFightIsActiveChanged(false, bossFightIsActie.Value); // so uif you join when the fight is already active, you will get a hp bar
        sleepState = Instantiate(sleepState);
        currentState = sleepState;
        // if our save data does not contain information on this boss, add it now
        if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
            WorldSaveGameManager.instance.currentCharacterData.bossesDefeated   .Add(bossID, false);
        }
        // otherwise, load the data that already exist on this boss
        else
        {
            hasBeenDefeated.Value = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];
            hasBeenAwakened.Value = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];
            //

        }

        // locate fog wall
        StartCoroutine(GetFogWallsFromWorldObjectManager());

        // you can either sahre the same Id for the boss and fog wall, or simply place a simply place a fogwall ID variable here on look  for using i

        // if the boss has been awakened, enable the fog walls
        if (hasBeenAwakened.Value)
        {
            for(int i = 0; i<fogWalls.Count; i++)
            {
                fogWalls[i].isActive.Value = true;
            }
        }

        // if the boss has been defeated disable the fog walls
        if (hasBeenDefeated.Value)
        {
            for (int i = 0; i < fogWalls.Count; i++)
            {
                fogWalls[i].isActive.Value = false;
            }
            print("tat Boss");
            isActive.Value = false;
        }

        if(!hasBeenAwakened.Value)
        {
            characterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
        }

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        bossFightIsActie.OnValueChanged -= OnBossFightIsActiveChanged;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

private IEnumerator GetFogWallsFromWorldObjectManager()
    {
        while(WorldObjectManager.instance.fogWalls.Count == 0)
            yield return new WaitForEndOfFrame();

        fogWalls = new List<FogWallInteractable>();

        foreach (var fogwall in WorldObjectManager.instance.fogWalls)
        {
            if (fogwall.fogWallID == bossID)
                fogWalls.Add(fogwall);
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        PlayerUIManager.instance.playerUIPopUpManager.SendBossDefeatedPopUp("Hùng fell");

        currentHealth.Value = 0;
        isDead.Value = true;

        bossFightIsActie.Value = false;

        foreach(var fogWall in fogWalls)
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

        //play some death SFX

        yield return new WaitForSeconds(5);

        // award players with runes

        // disable character
    }
    
    public void WakeBoss()
    {
        if (!hasBeenAwakened.Value)
        {
            hasBeenAwakened.Value = true;
            characterAnimatorManager.PlayTargetActionAnimation(awakenAnimation, true);
        }

        bossFightIsActie.Value = true;
        hasBeenAwakened.Value = true;
        currentState = idle;

        // if our save data does not contain information on this boss, add it now
        if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
        }
        else
        {
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
            WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
        }

        for(int i = 0; i <fogWalls.Count; i++)
        {
            fogWalls[i].isActive.Value = true ;
        }

    }

    private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)
    {
        if (bossFightIsActie.Value)
        {
            WorldSoundFXManager.instance.PlayeBossTrack(bossIntroClip, bossBattleLoopClip);
            // create a HP bar for each boss that is in the fight (if its active)
            // destroy any hp bars currently active( if the boss is no longer active)
            GameObject bossHealthBar = Instantiate(PlayerUIManager.instance.playerUIHUDManager.bossHealthBarObject, PlayerUIManager.instance.playerUIHUDManager.bossHealthBarParent);

            UI_Boss_HP_bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_bar>();
            bossHPBar.EnableBossHpBar(this);
        }
        else
        {
            WorldSoundFXManager.instance.StopBossMusic();
        }

    }

    protected void PhaseShift()
    {
        characterAnimatorManager.PlayTargetActionAnimation(phaseShiftAniamtion, true);

        combatStance = Instantiate(phase02CombatStanceState);
    }

    //
    protected override void CheckHp(int oldValue, int newValue)
    {

        base.CheckHp(oldValue, newValue);

        if(currentHealth.Value <= 0)
            return;
        float healthNeededForShift = maxHealth.Value * (minimumHealthPercentageToShift /100)    ;
        if (currentHealth.Value <= healthNeededForShift)
        {
            PhaseShift();
        }
    }
}
