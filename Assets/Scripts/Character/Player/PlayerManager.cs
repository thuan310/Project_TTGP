using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    public Observable<string> playerName;

    [Header("Debug menu")]
    [SerializeField] bool respawnCharcter = false;

    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerDetectArea playerDetectArea;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;

    protected override void Awake()
    {
        base.Awake();
        // do more stuff, only for player
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerDetectArea = GetComponentInChildren<PlayerDetectArea>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();

    }
    private void Start()
    {
        PlayerInputManager.instance.player = this;
        playerDetectArea.player = this;

        if (!PlayerInputManager.instance.isTesting)
        {
            WorldSaveGameManager.instance.player = this;
            playerDetectArea.enabled = false;
        }
        SetUp();

    }

    public void SetUp()
    {
        //print("Dang tinh lai Mana");
        //Update the total amount of health or stamina when the stat linked to either changes
        vitality.OnValueChanged += SetNewMaxHealthValue;
        endurance.OnValueChanged += SetNewMaxStaminaValue;

        //updates UI stat bars when a stat changes(health or stamina
        currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;

        currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
        currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
        // This will be moved when saving and loading is added

        currentHealth.OnValueChanged += CheckHp;

        vitality.Value = 10;
        endurance.Value = 10;
}

    protected override void Update()
    {
        //if (isTesting)
        //{
        //    return;
        //}
        //testing for some property cause my custom observer is suck
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentHealth.Value = 0;
        }

        base.Update();

        if(PlayerInputManager.instance.isActiveAndEnabled)
        {
            //handle movement
            playerLocomotionManager.HandleAllMovement();

            // Regen Stamina
            playerStatsManager.RegenerateStamina();

            DebugMenu();
        }

    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();

        // check for players that are alive, if 0 respawn characters
        return base.ProcessDeathEvent(manuallySelectDeathAnimation);
    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        currentHealth.Value = maxHealth.Value;
        currentStamina.Value = maxStamina.Value;
        // restore focus poiunts
        // playrebirth effects
        playerAnimatorManager.PlayTargetActionAnimation("Empty", false);

    }

    public void SaveGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        //print(playerName.Value);
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

        currentCharacterData.characterName = playerName.Value;
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = currentHealth.Value;
        currentCharacterData.currentStamina =currentHealth.Value;

        currentCharacterData.vitality = vitality.Value;
        currentCharacterData.endurance = endurance.Value;
    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        //currentCharacterData.characterName
        playerName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;

        vitality.Value = currentCharacterData.vitality;
        endurance.Value = currentCharacterData.endurance;

        maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(vitality.Value);
        currentHealth.Value = currentCharacterData.currentHealth;
        //PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value);

        maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(endurance.Value);
        currentStamina.Value = currentCharacterData.currentStamina;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value);
    }

    // Debug delete later
    private void DebugMenu()
    {
        if ( respawnCharcter)
        {
            respawnCharcter = false;
            ReviveCharacter();
        }
    }

    public void SetNewMaxHealthValue(int oldVitality, int newVitality)
    {
        //print("tinh lai mau");
        maxHealth.Value = playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(maxHealth.Value);
        currentHealth.Value = maxHealth.Value;
    }

    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
    {
        maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value);
        currentStamina.Value = maxStamina.Value;
    }


}
