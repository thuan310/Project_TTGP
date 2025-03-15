using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    protected override void Awake()
    {
        base.Awake();
        // do more stuff, only for player
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();

    }
    private void Start()
    {
        PlayerInputManager.instance.player = this;


        currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
        currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
        // This will be moved when saving and loading is added
        maxStamina.Value = playerStatsManager.CalculateStaminaOnEnduranceLevel(endurance.Value);
        currentStamina.Value = playerStatsManager.CalculateStaminaOnEnduranceLevel(endurance.Value);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value);
    }
    protected override void Update()
    {
        base.Update();
        //handle movement
        playerLocomotionManager.HandleAllMovement();

        // Regen Stamina
        playerStatsManager.RegenerateStamina();
    }
}
