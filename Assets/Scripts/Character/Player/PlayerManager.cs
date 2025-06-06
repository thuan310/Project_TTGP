using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    //                       _oo0oo_
    //                      o8888888o
    //                      88" . "88
    //                      (| -_- |)
    //                      0\  =  /0
    //                    ___/`---'\___
    //                  .' \\|     |// '.
    //                 / \\|||  :  |||// \
    //                / _||||| -:- |||||- \
    //               |   | \\\  -  /// |   |
    //               | \_|  ''\---/''  |_/ |
    //               \  .-\__  '-'  ___/-. /
    //             ___'. .'  /--.--\  `. .'___
    //          ."" '<  `.___\_<|>_/___.' >' "".
    //         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
    //         \  \ `_.   \_ __\ /__ _/   .-` /  /
    //     =====`-.____`.___ \_____/___.-`___.-'=====
    //                       `=---='
    //
    //     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //            Phật phù hộ, không bao giờ BUG
    //     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    public Observable<string> playerName;

    //[Header("Debug menu")]
    //[SerializeField] bool respawnCharcter = false;
    //[SerializeField] bool switchRightWeapon = false;


    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerDetectAreaManager playerDetectArea;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PLayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;

    protected override void Awake()
    {
        base.Awake();
        // do more stuff, only for player
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerDetectArea = GetComponentInChildren<PlayerDetectAreaManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PLayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();

    }
    protected override void Start()
    {
        if (!PlayerInputManager.instance.isTesting)
        {
            WorldSaveGameManager.instance.player = this;
            PlayerInputManager.instance.player = this;

            MinigameInputManager.instance.player = this;
            MinigameInputManager.instance.enabled = false;

            PlayerCamera.instance.player = this;
            playerDetectArea.enabled = false;
        }

        base.Start();
        playerDetectArea.player = this;

        if (PlayerInputManager.instance.isTesting&&!isDummy)
        {
            PlayerInputManager.instance.player = this;

            MinigameInputManager.instance.player = this;
            MinigameInputManager.instance.enabled = false ;

            PlayerCamera.instance.player = this;
            PlayerCamera.instance.SetCameraTo(this);
            if (PlayerInputManager.instance.playerControls != null)
            {
                PlayerInputManager.instance.playerControls.Enable();
            }
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

        // stats
        currentHealth.OnValueChanged += CheckHp;

        // LockOn
        isLockedOn.OnValueChanged += OnIsLockedOnChanged;
        //currentargetNetworkObjectID.OnValueChanged += OnLockOnTargetIDChange;

        // equipment
        currentRightHandWeaponID.OnValueChanged += OnCurrentRightHandWeaponIDChange;
        currentLeftHandWeaponID.OnValueChanged += OnCurrentLeftHandWeaponIDChange;
        currentWeaponBeingUsed.OnValueChanged += OnCurrentWeaponBeingUsedIDChanged;

        // flags
        isCharingingAttack.OnValueChanged += OnIsChargingAttackChanged;

        vitality.Value = 10;
        endurance.Value = 10;
    }

    public void OnRemove()
    {
        //print("Dang tinh lai Mana");
        //Update the total amount of health or stamina when the stat linked to either changes
        vitality.OnValueChanged -= SetNewMaxHealthValue;
        endurance.OnValueChanged -= SetNewMaxStaminaValue;

        //updates UI stat bars when a stat changes(health or stamina
        currentHealth.OnValueChanged -= PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;

        currentStamina.OnValueChanged -= PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
        currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenTimer;
        // This will be moved when saving and loading is added

        // stats
        currentHealth.OnValueChanged -= CheckHp;

        // LockOn
        isLockedOn.OnValueChanged -= OnIsLockedOnChanged;
        //currentargetNetworkObjectID.OnValueChanged += OnLockOnTargetIDChange;

        // equipment
        currentRightHandWeaponID.OnValueChanged -= OnCurrentRightHandWeaponIDChange;
        currentLeftHandWeaponID.OnValueChanged -= OnCurrentLeftHandWeaponIDChange;
        currentWeaponBeingUsed.OnValueChanged -= OnCurrentWeaponBeingUsedIDChanged;

        // flags
        isCharingingAttack.OnValueChanged -= OnIsChargingAttackChanged;

        vitality.Value = 0;
        endurance.Value = 0;
    }

    protected override void Update()
    {
        //if (isTesting)
        //{
        //    return;
        //}
        //testing for some property cause my custom observer is suck
        if (Input.GetKeyDown(KeyCode.Escape) && !isDummy)
        {
            //currentHealth.Value = 0;
        }

        base.Update();

        if(PlayerInputManager.instance.isActiveAndEnabled)
        {
            if (!isDummy)
            {
                playerLocomotionManager.HandleAllMovement();
            }
            //handle movement

            // Regen Stamina
            playerStatsManager.RegenerateStamina();
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
        if(isDummy) 
            return;
        base.ReviveCharacter();
        isDead.Value = false;

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

    //
    public void SetCharcterActionHand(bool rightHandedAction)
    {
        if (rightHandedAction)
        {
            isUsingLeftHand.Value = false;
            isUsingRightHand.Value = true;
        }
        else
        {
            isUsingLeftHand.Value = true;
            isUsingRightHand.Value = false; 
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

    public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        playerInventoryManager.currentRightHandWeapon = newWeapon;
        playerEquipmentManager.LoadRightWeapon();

        PlayerUIManager.instance.playerUIHUDManager.SetRightWeaponQuickSLotICon(newID);
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        playerInventoryManager.currentLeftHandWeapon = newWeapon;
        playerEquipmentManager.LoadLeftWeapon();

        PlayerUIManager.instance.playerUIHUDManager.SetLeftWeaponQuickSLotICon(newID);
    }

    public void OnCurrentWeaponBeingUsedIDChanged(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        playerCombatManager.currentWeaponBeingUsed = newWeapon;
    }

    //public void PerformWeaponBasedAction(int actionID, int weaponID)
    //{
    //    WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

    //    if (weaponAction != null)
    //    {
    //        weaponAction.AttemptToPerformAction(this, WorldItemDatabase.instance.GetWeaponByID(weaponID));
    //    }
    //    else
    //    {
    //        Debug.LogError("Action is null, cannot be performed");
    //    }
    //}

    //public void OnLockOnTargetIDChange(ulong oldID, ulong newID)
    //{
    //    //characterCombatManager.currentTarget = Net
    //}

    public void OnIsLockedOnChanged(bool old, bool isLockedOn)
    {
        if(!isLockedOn)
        {
            characterCombatManager.currentTarget = null;
        }
    }

    public void OnIsChargingAttackChanged(bool oldStatus, bool newStatus)
    {
        animator.SetBool("isChargingAttack", isCharingingAttack.Value);
    }


}
