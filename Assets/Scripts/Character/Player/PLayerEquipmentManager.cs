using UnityEngine;
using UnityEngine.Assertions.Must;

public class PLayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;

    public WeaponModelInstantiattionSlot rightHandSlot;
    public WeaponModelInstantiattionSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>(); 
        // Get our Slots
        InitializeWeaponSLots();
    }

    protected override void Start()
    {
        base.Start();

        LoadWeaponsOnBothHands();
    }

    private void InitializeWeaponSLots()
    {
        WeaponModelInstantiattionSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiattionSlot>();

        foreach( var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModeSLot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModeSLot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    public void UnloadAllWeaponsOnRightHand()
    {
        for (int i = 0; i < player.playerInventoryManager.weaponInRightHandSLots.Length; i++)
        {
            player.playerInventoryManager.weaponInRightHandSLots[i] = WorldItemDatabase.instance.unarmedWeapon;
        }
    }

    public void AddWeaponToNextRightSlot(WeaponItem weapon)
    {
        player.playerInventoryManager.weaponInRightHandSLots[player.playerInventoryManager.rightHandWeaponIndex+1] = weapon;
    }

    // right weapon
    public void SwitchRightWeapon()
    {
        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true,true);

        // elden rings weapon swapping
        // 1. check if we have another weapon besides our main weapon, if we do, never swap to unarmed, rotate between weapon 1 and 2
        // 2. if we don't, swap to unarmed, then skip the other empty slot and swap back. Do not process both empty slots brfore returning to main weapon

        WeaponItem selectedweapon = null;

        // disable two handing if wea two handing

        // add one to our index to switch to the next potential weapon
        player.playerInventoryManager.rightHandWeaponIndex += 1;

        //check our weapon index ( we have 3 slots, so that 3 possible numbers)
        if (player.playerInventoryManager.rightHandWeaponIndex <0 || player.playerInventoryManager.rightHandWeaponIndex >2)
        {
            // if our index is out of bounds, reset it to position #1(0)
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            // we check if we are holding more than one weapon
            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponInRightHandSLots.Length; i++)
            {
                if (player.playerInventoryManager.weaponInRightHandSLots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponInRightHandSLots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedweapon = WorldItemDatabase.instance.unarmedWeapon;
                player.currentRightHandWeaponID.Value = selectedweapon.itemID;
            }
            else
            {
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                player.currentRightHandWeaponID.Value = firstWeapon.itemID;
            }
            return;
        }

        foreach(WeaponItem weapon in player.playerInventoryManager.weaponInRightHandSLots)
        {
            // check to see if this is not the ""Unarmed" weapon
            // if the next potenntial weapon does not equal the unarmed weapon
            if (player.playerInventoryManager.weaponInRightHandSLots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedweapon = player.playerInventoryManager.weaponInRightHandSLots[player.playerInventoryManager.rightHandWeaponIndex];
                //assign the network weapon id so it switches for all connected clients
                player.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponInRightHandSLots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                return;
            }

            if (selectedweapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }
        }
    }

    public void LoadRightWeapon()
    {
        if(player.playerInventoryManager.currentRightHandWeapon != null)
        {
            // remove the old weapon
            rightHandSlot.UnloadWeapon();

            //bring in the new weapon
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            //assign weapons damage, to its collider
        }
    }


    // left weapon
    public void SwitchLefttWeapon()
    {
        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

        // elden rings weapon swapping
        // 1. check if we have another weapon besides our main weapon, if we do, never swap to unarmed, rotate between weapon 1 and 2
        // 2. if we don't, swap to unarmed, then skip the other empty slot and swap back. Do not process both empty slots brfore returning to main weapon

        WeaponItem selectedweapon = null;

        // disable two handing if wea two handing

        // add one to our index to switch to the next potential weapon
        player.playerInventoryManager.leftHandWeaponIndex += 1;

        //check our weapon index ( we have 3 slots, so that 3 possible numbers)
        if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
        {
            // if our index is out of bounds, reset it to position #1(0)
            player.playerInventoryManager.leftHandWeaponIndex = 0;

            // we check if we are holding more than one weapon
            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponInLeftHandSLots.Length; i++)
            {
                if (player.playerInventoryManager.weaponInLeftHandSLots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponInLeftHandSLots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedweapon = WorldItemDatabase.instance.unarmedWeapon;
                player.currentLeftHandWeaponID.Value = selectedweapon.itemID;
            }
            else
            {
                player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                player.currentLeftHandWeaponID.Value = firstWeapon.itemID;
            }
            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponInLeftHandSLots)
        {
            // check to see if this is not the ""Unarmed" weapon
            // if the next potenntial weapon does not equal the unarmed weapon
            if (player.playerInventoryManager.weaponInLeftHandSLots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedweapon = player.playerInventoryManager.weaponInLeftHandSLots[player.playerInventoryManager.leftHandWeaponIndex];
                //assign the network weapon id so it switches for all connected clients
                player.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponInLeftHandSLots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                return;
            }

            if (selectedweapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLefttWeapon();
            }
        }
    }

    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            // remove the old weapon
            leftHandSlot.UnloadWeapon();

            //bring in the new weapon
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
            //assign weapons damage, to its collider
        }
    }

    // Damage Colliders
    public void OpenDamageCollider()
    {
        //print("mo collider");
        // Open Right weapon damage collider
        if (player.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PLaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooses));
        
        }
        // Open Left Weapon damage collider
        else if(player.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PLaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooses));

        }
        //play whoosh SFX
    }

    public void CloseDamageCollider()
    {
        // Open Right weapon damage collider
        if (player.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
        }
        // Open Left Weapon damage collider
        else if (player.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
        }
        //play whoosh SFX
    }
}
