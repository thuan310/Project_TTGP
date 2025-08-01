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

    [Header("Player Action")]
    public Observable<PLayerAction> action;

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
        DontDestroyOnLoad(gameObject);
        if (!PlayerInputManager.instance.isTesting)
        {
            WorldSaveGameManager.instance.player = this;
            PlayerInputManager.instance.player = this;

            MinigameInputManager.instance.player = this;
            MinigameInputManager.instance.enabled = false;

            PlayerCamera.instance.player = this;
            PlayerUIManager.instance.player = this;
            action.OnValueChanged += PlayerUIManager.instance.ControlUI;

            playerDetectArea.enabled = false;
        }


        base.Start();
        playerDetectArea.player = this;
        SceneNavigationManager.instance.player = this;
        TutorialManager.instance.enabled = true;

        if (PlayerInputManager.instance.isTesting && !isDummy)
        {
            WorldSaveGameManager.instance.player = this;
            PlayerInputManager.instance.player = this;

            MinigameInputManager.instance.player = this;
            MinigameInputManager.instance.enabled = false;

            PlayerCamera.instance.player = this;
            PlayerCamera.instance.SetCameraTo(this);
            PlayerUIManager.instance.player = this;
            action.OnValueChanged += PlayerUIManager.instance.ControlUI;

            //Test thu Tutorial 
            SceneNavigationManager.instance.currentSceneIndex.Value = SceneManager.GetActiveScene().buildIndex;

            if (PlayerInputManager.instance.playerControls != null)
            {
                PlayerInputManager.instance.playerControls.Enable();
            }
        }


        PlayerSetUP();

    }

    public void PlayerSetUP()
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

        base.Update();
        ControlAction();

        if (PlayerInputManager.instance.isActiveAndEnabled)
        {
            playerLocomotionManager.HandleAllMovement();
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
        currentCharacterData.sceneIndex = SceneNavigationManager.instance.currentSceneIndex.Value;

        this.gameObject.SetActive(false);
        playerLocomotionManager.enabled = false;
        currentCharacterData.characterName = playerName.Value;
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y+3;
        currentCharacterData.zPosition = transform.position.z;
        playerLocomotionManager.enabled = true;
        this.gameObject.SetActive(true);
        print(transform.position);

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

    private void ControlAction()
    {
        //print(action);
        switch (action.Value)
        {
            default:
                break;
            case PLayerAction.Normal:
                PlayerInputManager.instance.gameObject.SetActive(true);
                //DialogueInputManager.instance.gameObject.SetActive(false);
                MinigameInputManager.instance.enabled =false;
                PlayerCamera.instance.lockCamera = false;
                break;
            case PLayerAction.ChopTree:
                PlayerInputManager.instance.gameObject.SetActive(false);
                //DialogueInputManager.instance.gameObject.SetActive(false);
                MinigameInputManager.instance.enabled = true;
                PlayerCamera.instance.lockCamera = true;
                break;
            case PLayerAction.CarrySomething:
                PlayerInputManager.instance.gameObject.SetActive(true);
                //DialogueInputManager.instance.gameObject.SetActive(false);
                MinigameInputManager.instance.enabled = true;
                PlayerCamera.instance.lockCamera = false;
                break;
            case PLayerAction.LogSharpening:
                PlayerInputManager.instance.gameObject.SetActive(false);
                //DialogueInputManager.instance.gameObject.SetActive(false);
                MinigameInputManager.instance.enabled = true;
                PlayerCamera.instance.lockCamera = true;
                break;
            case PLayerAction.PlayingDialogue:
                PlayerInputManager.instance.gameObject.SetActive(false);
                //DialogueInputManager.instance.gameObject.SetActive(false);
                MinigameInputManager.instance.enabled = false;
                PlayerCamera.instance.lockCamera = true;
                break;
            case PLayerAction.ConvincingVillagers:
                PlayerInputManager.instance.gameObject.SetActive(false);
                //DialogueInputManager.instance.gameObject.SetActive(false);
                MinigameInputManager.instance.enabled = false;
                PlayerCamera.instance.lockCamera = true;
                break;
            case PLayerAction.CarryCart:
                PlayerInputManager.instance.gameObject.SetActive(false);
                //DialogueInputManager.instance.gameObject.SetActive(false);
                MinigameInputManager.instance.enabled = false;
                playerAnimatorManager.PlayTargetActionAnimation("Wheelbarrow Walk", false, false, true, true);
                break;

        }
    }

    // event subscribe

    private void OnEnable()
    {
        EventManager.instance.dialogueEvents.onEnterDialogue += OnEnterDialogue;
        EventManager.instance.dialogueEvents.onDialogueFinished += OnExitDialogue;
    }

    private void OnEnterDialogue(string t)
    {
        action.Value = PLayerAction.PlayingDialogue;
    }

    private void OnExitDialogue()
    {
        action.Value = PLayerAction.Normal;
    }



}
