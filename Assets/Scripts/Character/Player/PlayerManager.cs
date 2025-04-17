using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    public Observable<string> playerName;


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
        WorldSaveGameManager.instance.player = this;
    }

    public void SetUpStammina()
    {

        if (PlayerInputManager.instance.isActiveAndEnabled)
        {
            //print("Dang tinh lai Mana");
            currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
            currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;
            // This will be moved when saving and loading is added
            maxStamina.Value = playerStatsManager.CalculateStaminaOnEnduranceLevel(endurance.Value);
            currentStamina.Value = playerStatsManager.CalculateStaminaOnEnduranceLevel(endurance.Value);
            PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value);
        }
    }

    protected override void Update()
    {
        base.Update();

        if(PlayerInputManager.instance.isActiveAndEnabled)
        {
            //handle movement
            playerLocomotionManager.HandleAllMovement();

            // Regen Stamina
            playerStatsManager.RegenerateStamina();
        }

    }

    public void SaveGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        //print(playerName.Value);
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

        currentCharacterData.characterName = playerName.Value;
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;
    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        //currentCharacterData.characterName
        playerName.Value = currentCharacterData.characterName;
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;
    }


}
