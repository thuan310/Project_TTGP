using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    protected override void Start()
    {
        base.Start();

        // why calculate these here
        // when we make a character creation menu, and set the stats depending on the class, this will be calculated there
        // until then however, stats are never calculate, so we do it here on start, if a save file exist they will be over written when loading into a scene
        CalculateHealthBasedOnVitalityLevel(player.vitality.Value);
        CalculateStaminaBasedOnEnduranceLevel(player.endurance.Value);
    }
}
